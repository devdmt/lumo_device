# LumoDevice API

> .NET 8.0 ASP.NET Core REST API for device insurance operations, claims processing, partner onboarding, and M-Pesa payment integration.

## Overview

LumoDevice is a comprehensive insurance platform serving multiple business domains:
- **Safaricom Device Insurance** - Phone insurance and claims processing
- **MSure Insurance Services** - Partner onboarding and insurance requests
- **Pension Management** - Contribution tracking and beneficiary management
- **Credit Life Insurance** - Insurance policy management
- **M-Pesa Integration** - Payment processing and callbacks

## Quick Start

```bash
# Restore dependencies
dotnet restore LumoDevice.sln

# Run the API
dotnet run --project LumoDevice

# API available at: https://localhost:5001
# Swagger UI: https://localhost:5001/swagger
```

## Architecture

Built using **Clean Architecture** principles:
- **LumoDevice/** - API Layer (Controllers, Configurations)
- **API.Infrastructure/** - Application & Infrastructure (Services, Middleware)
- **DAL/** - Data Access Layer (Entities, DbContext, Repositories)

**Key Technologies:**
- .NET 8.0 / ASP.NET Core
- Entity Framework Core 7.0.3
- MediatR (CQRS pattern)
- SQL Server
- Serilog (structured logging)
- JWT Authentication

## Documentation

📚 **[Complete Documentation →](./docs/README.md)**

### Quick Links
- **[Architecture Guide](./CLAUDE.md)** - Project structure, patterns, and development guidelines
- **[Payload Decryption](./docs/features/payload-decryption/README.md)** - Safaricom encrypted API integration
- **[Quick Reference](./docs/features/payload-decryption/quick-reference.md)** - Configuration cheat sheet

### By Role
- **Developers:** [Architecture Guide](./CLAUDE.md) | [Development Commands](./CLAUDE.md#development-commands)
- **DevOps:** [Payload Decryption Config](./docs/features/payload-decryption/quick-reference.md#-configuration-cheat-sheet)
- **QA:** [Testing Guide](./docs/features/payload-decryption/implementation-guide.md#testing--validation)

## Features

### ✅ Implemented
- [x] JWT Authentication & Authorization
- [x] Claims processing workflow
- [x] Safaricom device insurance integration
- [x] M-Pesa payment callbacks
- [x] Partner onboarding system
- [x] **Payload Decryption** - AES-ECB with SHA-256 verification
- [x] Pension management
- [x] MSure insurance services

### 🚧 In Progress
- [ ] Response encryption
- [ ] Enhanced monitoring and metrics
- [ ] Automated testing suite

## Configuration

Key configuration files in `LumoDevice/Configurations/`:
- `database.json` - Database connection and provider
- `security.json` - JWT settings and payload decryption
- `cors.json` - CORS policies
- `logger.json` - Serilog configuration

**Environment Variables:**
```bash
# Payload Decryption (Safaricom Integration)
export PayloadDecryptionOptions__Enabled=true
export PayloadDecryptionOptions__EncryptionKey=YOUR_BASE64_KEY
export PayloadDecryptionOptions__EncryptedRoutes=/api/saf/v1/PhoneInsurance/customeronboarding
```

See [.env.example](./.env.example) for complete list.

## API Endpoints

### Safaricom
- `POST /api/saf/v1/PhoneInsurance/customeronboarding` - Purchase insurance
- `POST /api/saf/v1/PhoneInsurance/MakeClaim` - Submit claim
- `POST /api/saf/v1/PhoneInsurance/ReplaceRequest` - Device replacement

### MSure
- `POST /api/msure/onboarding` - Partner onboarding
- `POST /api/msure/InsureRequest` - Insurance request
- `POST /api/msure/GetProduct/{PartnerCode}` - Get products

### Claims
- `POST /api/Claims/...` - Claims management

### M-Pesa
- `POST /api/Mpesa/...` - Payment callbacks

**Full API documentation:** https://localhost:5001/swagger

## Development

### Prerequisites
- .NET 8.0 SDK
- SQL Server (or Docker: `docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword" -p 1433:1433 -d mcr.microsoft.com/mssql/server`)
- Visual Studio 2022 / Rider / VS Code

### Setup
```bash
# Clone repository
git clone <repository-url>
cd lumo_device

# Restore packages
dotnet restore

# Update database
dotnet ef database update --project LumoDevice

# Run application
dotnet run --project LumoDevice
```

### Code Style
```bash
# Format code before committing
dotnet format LumoDevice.sln
```

See [Architecture Guide](./CLAUDE.md) for detailed development guidelines.

## Project Structure

```
lumo_device/
├── docs/                    # 📚 Documentation
│   ├── features/            # Feature-specific docs
│   │   └── payload-decryption/
│   └── README.md            # Documentation index
├── LumoDevice/              # API Layer
│   ├── Controllers/         # API endpoints
│   ├── Configurations/      # JSON config files
│   └── Migrations/          # EF Core migrations
├── API.Infrastructure/      # Application & Infrastructure
│   ├── Security/            # Payload decryption, auth
│   ├── Application/         # Business services
│   └── Middleware/          # Custom middleware
├── DAL/                     # Data Access Layer
│   ├── Model/               # Entity models
│   └── ModelView/           # DTOs
├── .env.example             # Environment variable template
└── CLAUDE.md                # Architecture guide
```

## Contributing

1. Follow conventional commits: `feat:`, `fix:`, `docs:`, `chore:`
2. Run `dotnet format` before committing
3. Update documentation for new features
4. Add tests for new functionality

## Support

- **Documentation:** [docs/README.md](./docs/README.md)
- **Architecture:** [CLAUDE.md](./CLAUDE.md)
- **Issues:** [GitHub Issues](#)

## License

[Your License Here]

---

**Current Branch:** `feat/endpoint-encrytption`
**Status:** ✅ Active Development
**Last Updated:** 2025-11-07