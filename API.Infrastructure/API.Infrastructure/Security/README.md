# Payload Decryption Module

This module implements AES-ECB payload decryption with SHA-256 checksum verification for encrypted API requests from Safaricom and other partners.

## Overview

The implementation follows the **Safaricom API Payload Decryption Documentation** specification:
- **Algorithm:** AES-ECB with PKCS7 padding
- **Checksum:** SHA-256 for integrity verification
- **Encoding:** Base64 for keys and encrypted payloads

## Components

### 1. `PayloadDecryptionOptions`
Configuration model that reads from `security.json` and environment variables.

**Properties:**
- `Enabled` (bool): Enable/disable decryption globally
- `EncryptionKey` (string): Base64-encoded AES key
- `EncryptedRoutes` (string): Comma-separated list of routes requiring decryption

### 2. `IPayloadDecryptionService` / `PayloadDecryptionService`
Core service that performs AES decryption and SHA-256 verification.

**Methods:**
- `DecryptAndVerifyAsync()`: Decrypts payload and verifies checksum
- `IsConfigured()`: Validates configuration

### 3. `PayloadDecryptionMiddleware`
ASP.NET Core middleware that intercepts requests to configured routes and decrypts payloads automatically.

**Features:**
- Route-based filtering (only decrypts configured endpoints)
- Attribute-based opt-out with `[SkipPayloadDecryption]`
- Comprehensive error handling and logging
- Request body buffering for multiple reads

### 4. `SkipPayloadDecryptionAttribute`
Attribute to mark specific endpoints that should skip decryption.

## Configuration

### Option 1: Environment Variables (Recommended for Production)

```bash
# Set in your hosting environment (Azure, AWS, etc.)
export PayloadDecryptionOptions__Enabled=true
export PayloadDecryptionOptions__EncryptionKey=MTIzNDU2Nzg5MDEyMzQ1Ng==
export PayloadDecryptionOptions__EncryptedRoutes=/api/saf/v1/PhoneInsurance/customeronboarding
```

### Option 2: Configuration File (Development)

Edit `LumoDevice/Configurations/security.json`:

```json
{
  "PayloadDecryptionOptions": {
    "Enabled": true,
    "EncryptionKey": "YOUR_BASE64_ENCRYPTION_KEY_HERE",
    "EncryptedRoutes": "/api/saf/v1/PhoneInsurance/customeronboarding"
  }
}
```

**⚠️ Security Warning:** Never commit encryption keys to source control!

## Usage

### Automatic Decryption (Default)

Endpoints matching configured routes are automatically decrypted:

```csharp
[HttpPost("customeronboarding")]
[AllowAnonymous]
public async Task<IActionResult> Onboarding([FromBody] PhoneInsuranceRequest request)
{
    // Request body is already decrypted by middleware
    var response = await _phoneInsurance.PurchaseInsurance(request);
    return Ok(response);
}
```

### Opt-Out with Attribute

Skip decryption for specific endpoints:

```csharp
[HttpPost("health")]
[SkipPayloadDecryption] // This endpoint will not be decrypted
public IActionResult HealthCheck()
{
    return Ok("Service is healthy");
}
```

## Expected Request Format

Encrypted requests should include both the encrypted checksum and the original payload:

```json
{
  "encryptedPayload": "BASE64_ENCRYPTED_CHECKSUM",
  "payload": "{\"phoneNumber\":\"254712345678\",\"imei\":\"123456789012345\"}"
}
```

**Decryption Process:**
1. Middleware extracts `encryptedPayload` and `payload`
2. Decrypts the `encryptedPayload` using AES-ECB
3. Calculates SHA-256 checksum of `payload`
4. Verifies decrypted value matches calculated checksum
5. If valid, replaces request body with `payload` (original JSON)
6. Controller receives clean, decrypted data

## Route Configuration

Routes are matched using `StartsWith` logic:

| Configuration | Matches |
|--------------|---------|
| `/api/saf` | All Safaricom endpoints |
| `/api/saf/v1/PhoneInsurance/customeronboarding` | Only this specific endpoint |
| `/api/saf,/api/mpesa` | Both Safaricom and M-Pesa endpoints |

## Error Handling

The middleware returns standardized error responses:

**400 Bad Request** (Security Exception):
```json
{
  "error": "Payload decryption failed",
  "message": "Checksum verification failed - payload may be corrupted or tampered with"
}
```

**500 Internal Server Error** (Unexpected):
```json
{
  "error": "Internal server error",
  "message": "An unexpected error occurred during request processing"
}
```

## Logging

The module logs at various levels:

- **Debug:** Route matching, decryption steps
- **Info:** Successful decryption/verification
- **Warning:** Missing fields, configuration issues
- **Error:** Decryption failures, checksum mismatches

## Testing

Use the provided test vector from Safaricom documentation:

```bash
# Test data
encryptionKey: "MTIzNDU2Nzg5MDEyMzQ1Ng=="
originalPayload: "Hello, World!"
expectedChecksum: "dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f"
expectedChecksumB64: "3/1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8="
```

## Security Considerations

1. **Key Management:** Store encryption keys in secure vaults (Azure Key Vault, AWS Secrets Manager)
2. **HTTPS Only:** Always use HTTPS in production
3. **Key Rotation:** Implement key rotation policies
4. **Logging:** Never log decrypted payloads or encryption keys
5. **Validation:** Always validate decrypted data against your schema

## Troubleshooting

### "Encryption key is not configured"
- Ensure `PayloadDecryptionOptions__EncryptionKey` is set in environment or `security.json`

### "Missing 'encryptedPayload' field"
- Request body must include `encryptedPayload` field with Base64-encoded checksum

### "Checksum verification failed"
- Payload may be corrupted or tampered with
- Ensure encryption key matches the one used by the sender
- Verify the original payload is UTF-8 encoded

### Middleware not processing requests
- Check `PayloadDecryptionOptions__Enabled` is set to `true`
- Verify route matches configured `EncryptedRoutes`
- Check logs for route matching debug messages

## Integration with Other Systems

This module is designed to integrate with:
- Safaricom Phone Insurance API
- M-Pesa payment callbacks
- Any partner API using AES-ECB encryption with SHA-256 checksums

## Future Enhancements

- [ ] Support for additional encryption algorithms (AES-CBC, AES-GCM)
- [ ] Async key rotation from Azure Key Vault
- [ ] Metrics and monitoring integration
- [ ] Response payload encryption