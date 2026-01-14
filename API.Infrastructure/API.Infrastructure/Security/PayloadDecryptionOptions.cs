namespace API.Infrastructure.Security;

public class PayloadDecryptionOptions
{
    /// <summary>
    /// Enables or disables payload decryption globally
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Base64-encoded AES encryption key for decryption
    /// Should be provided via environment variable: PAYLOAD_DECRYPTION__ENCRYPTION_KEY
    /// </summary>
    public string EncryptionKey { get; set; } = string.Empty;

    /// <summary>
    /// Comma-separated list of route paths that require decryption
    /// Example: "/api/safaricom,/api/mpesa"
    /// Can be provided via environment variable: PAYLOAD_DECRYPTION__ENCRYPTED_ROUTES
    /// </summary>
    public string EncryptedRoutes { get; set; } = "/api/safaricom,/api/mpesa";

    /// <summary>
    /// Gets the list of routes that require decryption as an array
    /// </summary>
    public string[] GetEncryptedRoutesArray()
    {
        if (string.IsNullOrWhiteSpace(EncryptedRoutes))
        {
            return Array.Empty<string>();
        }

        return EncryptedRoutes
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(r => r.Trim())
            .Where(r => !string.IsNullOrWhiteSpace(r))
            .ToArray();
    }

    /// <summary>
    /// Validates the configuration options
    /// </summary>
    /// <returns>True if valid, false otherwise</returns>
    public bool IsValid()
    {
        if (!Enabled)
        {
            return true; // Configuration is valid when disabled
        }

        if (string.IsNullOrWhiteSpace(EncryptionKey))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(EncryptedRoutes))
        {
            return false;
        }

        return true;
    }
}