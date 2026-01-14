using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Security;

/// <summary>
/// Implementation of payload decryption service using AES-ECB encryption with SHA-256 checksum verification
/// Based on Safaricom API Payload Decryption Documentation
/// </summary>
public class PayloadDecryptionService : IPayloadDecryptionService
{
    private readonly PayloadDecryptionOptions _options;
    private readonly ILogger<PayloadDecryptionService> _logger;

    public PayloadDecryptionService(
        IOptions<PayloadDecryptionOptions> options,
        ILogger<PayloadDecryptionService> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<string> DecryptAndVerifyAsync(
        string encryptedPayload,
        string originalPayload,
        string? encryptionKey = null)
    {
        try
        {
            // Use provided key or fall back to configured key
            var keyToUse = encryptionKey ?? _options.EncryptionKey;

            if (string.IsNullOrWhiteSpace(keyToUse))
            {
                throw new InvalidOperationException(
                    "Encryption key is not configured. Set PAYLOAD_DECRYPTION__ENCRYPTION_KEY environment variable or provide key in configuration.");
            }

            _logger.LogDebug("Starting payload decryption and verification process");

            // Step 1: Decode the encryption key from Base64
            byte[] keyBytes = Convert.FromBase64String(keyToUse);
            _logger.LogDebug("Encryption key decoded successfully. Key length: {KeyLength} bytes", keyBytes.Length);

            // Step 2: Decrypt the payload using AES-ECB
            string decryptedString = await Task.Run(() => DecryptPayload(encryptedPayload, keyBytes));
            _logger.LogDebug("Payload decrypted successfully");

            // Step 3: Calculate expected checksum from original payload
            string expectedChecksum = await Task.Run(() => CalculateSha256Checksum(originalPayload));
            _logger.LogDebug("Expected checksum calculated");

            // Step 4: Verify integrity - compare checksums
            if (!string.Equals(expectedChecksum, decryptedString, StringComparison.Ordinal))
            {
                _logger.LogError(
                    "Checksum verification failed. Expected: {Expected}, Got: {Actual}",
                    expectedChecksum,
                    decryptedString);

                throw new SecurityException(
                    "Checksum verification failed - payload may be corrupted or tampered with");
            }

            _logger.LogInformation("Payload decryption and verification completed successfully");

            // Step 5: Return the original payload if verification succeeds
            return originalPayload;
        }
        catch (FormatException ex)
        {
            _logger.LogError(ex, "Invalid Base64 input during decryption");
            throw new SecurityException("Invalid Base64 encoding in encrypted payload or key", ex);
        }
        catch (CryptographicException ex)
        {
            _logger.LogError(ex, "Cryptographic operation failed during decryption");
            throw new SecurityException("Decryption failed - invalid key or corrupted encrypted data", ex);
        }
        catch (SecurityException)
        {
            // Re-throw security exceptions as-is
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during payload decryption");
            throw new SecurityException("Unexpected error during payload decryption", ex);
        }
    }

    /// <inheritdoc />
    public bool IsConfigured()
    {
        return _options.IsValid();
    }

    /// <summary>
    /// Decrypts the encrypted payload using AES-ECB mode
    /// </summary>
    private string DecryptPayload(string encryptedPayload, byte[] keyBytes)
    {
        // Decode the encrypted payload from Base64
        byte[] encryptedBytes = Convert.FromBase64String(encryptedPayload);

        using var aes = Aes.Create();
        aes.Key = keyBytes;
        aes.Mode = CipherMode.ECB;
        aes.Padding = PaddingMode.PKCS7; // PKCS5Padding in Java is equivalent to PKCS7 in .NET

        using var decryptor = aes.CreateDecryptor();
        byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

        // Convert decrypted bytes to UTF-8 string
        return Encoding.UTF8.GetString(decryptedBytes);
    }

    /// <summary>
    /// Calculates SHA-256 checksum and returns it as Base64-encoded string
    /// </summary>
    private string CalculateSha256Checksum(string payload)
    {
        using var sha256 = SHA256.Create();
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        byte[] checksumBytes = sha256.ComputeHash(payloadBytes);

        // Return as Base64-encoded string
        return Convert.ToBase64String(checksumBytes);
    }
}

/// <summary>
/// Custom exception for security-related errors during payload decryption
/// </summary>
public class SecurityException : Exception
{
    public SecurityException(string message) : base(message)
    {
    }

    public SecurityException(string message, Exception innerException) : base(message, innerException)
    {
    }
}