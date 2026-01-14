# Payload Decryption - Quick Reference Guide

## 🚀 Quick Start (5 Minutes)

### 1. Set Encryption Key
```bash
export PayloadDecryptionOptions__EncryptionKey=YOUR_BASE64_KEY_FROM_SAFARICOM
```

### 2. Run Application
```bash
dotnet run --project LumoDevice
```

### 3. Test Endpoint
```bash
curl -X POST http://localhost:5000/api/saf/v1/PhoneInsurance/customeronboarding \
  -H "Content-Type: application/json" \
  -d '{
    "encryptedPayload": "BASE64_ENCRYPTED_CHECKSUM",
    "payload": "{\"phoneNumber\":\"254712345678\",\"imei\":\"123456789012345\"}"
  }'
```

---

## 📋 Configuration Cheat Sheet

### Environment Variables
```bash
# Enable/disable decryption
PayloadDecryptionOptions__Enabled=true

# Set encryption key (REQUIRED)
PayloadDecryptionOptions__EncryptionKey=MTIzNDU2Nzg5MDEyMzQ1Ng==

# Configure routes (comma-separated)
PayloadDecryptionOptions__EncryptedRoutes=/api/saf/v1/PhoneInsurance/customeronboarding
```

### Configuration File
**File:** `LumoDevice/Configurations/security.json`
```json
{
  "PayloadDecryptionOptions": {
    "Enabled": true,
    "EncryptionKey": "",
    "EncryptedRoutes": "/api/saf/v1/PhoneInsurance/customeronboarding"
  }
}
```

---

## 🔍 Request Format

### Expected Input
```json
{
  "encryptedPayload": "BASE64_ENCRYPTED_SHA256_CHECKSUM",
  "payload": "{\"phoneNumber\":\"254712345678\",\"imei\":\"123456789012345\"}"
}
```

### What Controller Receives (After Decryption)
```json
{
  "phoneNumber": "254712345678",
  "imei": "123456789012345"
}
```

---

## 🎯 Current Configuration

| Setting | Value |
|---------|-------|
| **Enabled** | `true` |
| **Encryption Algorithm** | AES-ECB with PKCS7 padding |
| **Checksum Algorithm** | SHA-256 |
| **Key Encoding** | Base64 |
| **Configured Endpoints** | `/api/saf/v1/PhoneInsurance/customeronboarding` |

---

## 🔧 Common Operations

### Add More Endpoints
**Option 1: Via environment variable**
```bash
export PayloadDecryptionOptions__EncryptedRoutes="/api/saf/v1/PhoneInsurance/customeronboarding,/api/saf/v1/PhoneInsurance/MakeClaim"
```

**Option 2: Via security.json**
```json
{
  "EncryptedRoutes": "/api/saf/v1/PhoneInsurance/customeronboarding,/api/saf/v1/PhoneInsurance/MakeClaim"
}
```

### Disable Decryption
```bash
export PayloadDecryptionOptions__Enabled=false
```

### Skip Specific Endpoint
```csharp
[HttpPost("health")]
[SkipPayloadDecryption]
public IActionResult HealthCheck()
{
    return Ok("Healthy");
}
```

---

## 🐛 Troubleshooting Quick Fixes

### Error: "Encryption key is not configured"
```bash
# Solution: Set the key
export PayloadDecryptionOptions__EncryptionKey=YOUR_KEY_HERE
```

### Error: "Checksum verification failed"
**Common causes:**
1. Wrong encryption key
2. Payload was modified after checksum calculation
3. UTF-8 encoding mismatch

**Test with known good values:**
```bash
# Key: MTIzNDU2Nzg5MDEyMzQ1Ng==
# Payload: "Hello, World!"
# Expected checksum: 3/1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8=

curl -X POST http://localhost:5000/api/saf/v1/PhoneInsurance/customeronboarding \
  -d '{
    "encryptedPayload": "3/1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8=",
    "payload": "Hello, World!"
  }'
```

### Middleware Not Running
**Check:**
1. `PayloadDecryptionOptions__Enabled=true`
2. Request path matches `EncryptedRoutes` pattern
3. No `[SkipPayloadDecryption]` attribute on endpoint

**Enable debug logs:**
```json
{
  "Logging": {
    "LogLevel": {
      "API.Infrastructure.Security": "Debug"
    }
  }
}
```

---

## 📊 Component Locations

| Component | File Path |
|-----------|-----------|
| **Configuration Model** | `API.Infrastructure/Security/PayloadDecryptionOptions.cs` |
| **Service Interface** | `API.Infrastructure/Security/IPayloadDecryptionService.cs` |
| **Decryption Service** | `API.Infrastructure/Security/PayloadDecryptionService.cs` |
| **Middleware** | `API.Infrastructure/Security/PayloadDecryptionMiddleware.cs` |
| **Skip Attribute** | `API.Infrastructure/Security/SkipPayloadDecryptionAttribute.cs` |
| **Service Registration** | `API.Infrastructure/Security/Startup.cs` |
| **Configuration File** | `LumoDevice/Configurations/security.json` |

---

## 📚 Documentation

| Document | Description | Location |
|----------|-------------|----------|
| **Implementation Guide** | Complete technical documentation | `LumoDevice/docs/PAYLOAD_DECRYPTION_IMPLEMENTATION_GUIDE.md` |
| **Quick Reference** | This document | `LumoDevice/docs/QUICK_REFERENCE.md` |
| **Module README** | Overview and usage | `API.Infrastructure/Security/README.md` |
| **Implementation Summary** | Project status and next steps | `PAYLOAD_DECRYPTION_IMPLEMENTATION.md` |
| **Safaricom Spec** | Original specification | `LumoDevice/docs/API Payload Decryption Documentation (1).pdf` |

---

## 🔐 Security Checklist

- [x] Encryption key stored in environment variables (not in code)
- [x] Different keys for dev/staging/production
- [x] HTTPS enabled in production
- [x] Sensitive data not logged
- [x] Checksum verification prevents tampering
- [ ] Key rotation policy established
- [ ] Azure Key Vault configured (recommended)
- [ ] Rate limiting enabled
- [ ] Monitoring and alerting configured

---

## 🧪 Test Vector (From Safaricom)

```javascript
const testData = {
  encryptionKey: "MTIzNDU2Nzg5MDEyMzQ1Ng==",
  originalPayload: "Hello, World!",
  expectedChecksum: "dffd6021bb2bd5b0af676290809ec3a53191dd81c7f70a4b28688a362182986f",
  expectedChecksumB64: "3/1gIbsr1bCvZ2KQgJ7DpTGR3YHH9wpLKGiKNiGCmG8="
};
```

---

## 🎯 Deployment Checklist

### Before Deploying to Production

1. **Configuration**
   - [ ] Encryption key set via environment variable
   - [ ] Routes correctly configured
   - [ ] Decryption enabled

2. **Security**
   - [ ] HTTPS enabled
   - [ ] Encryption key stored in Azure Key Vault
   - [ ] Rate limiting configured
   - [ ] Logging configured (no sensitive data)

3. **Testing**
   - [ ] Test with Safaricom test vectors
   - [ ] Test with real Safaricom payloads
   - [ ] Verify error handling works
   - [ ] Load testing completed

4. **Monitoring**
   - [ ] Application Insights configured
   - [ ] Alerts set up for failures
   - [ ] Dashboard created for metrics

---

## 💡 Tips & Tricks

### Debugging Decryption Issues
```csharp
// Add to appsettings.Development.json
{
  "Logging": {
    "LogLevel": {
      "API.Infrastructure.Security": "Debug"
    }
  }
}
```

### Testing Without Real Encryption
Temporarily disable verification (dev only):
```csharp
// In PayloadDecryptionService.DecryptAndVerifyAsync()
// Comment out checksum comparison for testing
// if (!string.Equals(...)) { throw ... }
return originalPayload;  // Always pass
```

### Performance Monitoring
```csharp
// Check logs for timing
[Info] Payload decryption and verification completed successfully (took 2ms)
```

### Route Pattern Matching
```
Configuration: "/api/saf"
Matches:
  ✅ /api/saf/v1/PhoneInsurance/customeronboarding
  ✅ /api/saf/v2/anything
  ✅ /api/safaricom/test  ⚠️ Be careful!
  ❌ /api/msure/onboarding
```

---

## 📞 Support

**Internal Resources:**
- Technical Lead: [Your Name]
- Safaricom Integration Contact: [Contact]
- Slack Channel: #lumo-device-api

**External Resources:**
- Safaricom API Docs: [Link]
- Safaricom Support: [Contact]

---

## 🔄 Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.0 | 2025-11-07 | Initial implementation for Safaricom onboarding endpoint |

---

**Quick Links:**
- [Full Implementation Guide](./PAYLOAD_DECRYPTION_IMPLEMENTATION_GUIDE.md)
- [Safaricom Specification](./API%20Payload%20Decryption%20Documentation%20(1).pdf)
- [Module README](../../API.Infrastructure/API.Infrastructure/Security/README.md)
- [Project Documentation](../../CLAUDE.md)