namespace API.Infrastructure.Security;

/// <summary>
/// Service for decrypting encrypted API payloads using AES encryption with SHA-256 checksum verification
/// </summary>
public interface IPayloadDecryptionService
{
    /// <summary>
    /// Decrypts an encrypted payload and verifies its integrity using SHA-256 checksum
    /// </summary>
    /// <param name="encryptedPayload">Base64-encoded encrypted checksum</param>
    /// <param name="originalPayload">The original unencrypted payload data</param>
    /// <param name="encryptionKey">Base64-encoded AES encryption key (optional, uses configured key if not provided)</param>
    /// <returns>The original payload if decryption and verification succeed</returns>
    /// <exception cref="SecurityException">Thrown if checksum verification fails</exception>
    /// <exception cref="InvalidOperationException">Thrown if encryption key is not configured</exception>
    Task<string> DecryptAndVerifyAsync(string encryptedPayload, string originalPayload, string? encryptionKey = null);

    /// <summary>
    /// Validates if the decryption service is properly configured
    /// </summary>
    /// <returns>True if configured correctly, false otherwise</returns>
    bool IsConfigured();
}