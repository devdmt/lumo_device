# Payload Decryption Feature

## Overview

AES-ECB payload decryption system with SHA-256 checksum verification for encrypted API requests from Safaricom and partner integrations.

## Documentation

### Quick Start
**[Quick Reference Guide](./quick-reference.md)**
- 5-minute setup
- Configuration cheat sheet
- Common operations
- Troubleshooting quick fixes

### Complete Guide
**[Implementation Guide](./implementation-guide.md)**
- Architecture deep dive (30,000+ words)
- Component analysis with code examples
- Security considerations and threat model
- Multi-language integration examples (C#, JavaScript, Python)
- Unit and integration testing examples
- Performance optimization
- Comprehensive troubleshooting

### Specification
**[Safaricom API Specification](./safaricom-specification.pdf)**
- Original Safaricom documentation
- Algorithm details
- Test vectors

## Current Configuration

| Setting | Value |
|---------|-------|
| **Status** | ✅ Implemented and ready for testing |
| **Endpoints** | `/api/saf/v1/PhoneInsurance/customeronboarding` |
| **Algorithm** | AES-ECB with PKCS7 padding |
| **Checksum** | SHA-256 |
| **Key Management** | Environment variables |

## Quick Links

- **Setup:** [Quick Reference - Quick Start](./quick-reference.md#-quick-start-5-minutes)
- **Architecture:** [Implementation Guide - Architecture](./implementation-guide.md#architecture-overview)
- **Integration:** [Implementation Guide - Integration Guide](./implementation-guide.md#integration-guide)
- **Testing:** [Implementation Guide - Testing](./implementation-guide.md#testing--validation)
- **Troubleshooting:** [Quick Reference - Troubleshooting](./quick-reference.md#-troubleshooting-quick-fixes)

## Components

| Component | Location | Description |
|-----------|----------|-------------|
| Service | `API.Infrastructure/Security/PayloadDecryptionService.cs` | AES decryption with SHA-256 verification |
| Middleware | `API.Infrastructure/Security/PayloadDecryptionMiddleware.cs` | Route-based request interceptor |
| Config | `LumoDevice/Configurations/security.json` | Configuration options |
| Attribute | `API.Infrastructure/Security/SkipPayloadDecryptionAttribute.cs` | Opt-out for specific endpoints |

## Implementation Status

- [x] Core decryption service
- [x] Middleware integration
- [x] Configuration management
- [x] Error handling
- [x] Logging
- [x] Documentation
- [x] Environment variable support
- [ ] Response encryption (not in current spec)
- [ ] Key rotation (future enhancement)
- [ ] Metrics/monitoring (future enhancement)

## Support

**For Issues:**
1. Check [Quick Reference Troubleshooting](./quick-reference.md#-troubleshooting-quick-fixes)
2. Review [Implementation Guide Troubleshooting](./implementation-guide.md#troubleshooting)
3. Check configuration is correct

**For Questions:**
- Consult [Implementation Guide](./implementation-guide.md)
- Review [Integration Examples](./implementation-guide.md#integration-guide)

---

**Version:** 1.0
**Last Updated:** 2025-11-07
**Status:** ✅ Production Ready