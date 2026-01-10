# Environment Configuration Guide

This document explains how to configure the application using environment variables for different deployment scenarios.

## Table of Contents

- [Overview](#overview)
- [Configuration Priority](#configuration-priority)
- [Environment Variables](#environment-variables)
- [Local Development](#local-development)
- [Docker Development](#docker-development)
- [Production Deployment](#production-deployment)
- [GitHub Actions](#github-actions)
- [Security Best Practices](#security-best-practices)

## Overview

The application uses a flexible configuration system that supports:

1. **`.env` files** - For local development (never committed to git)
2. **Environment variables** - For Docker and production deployments
3. **`appsettings.json`** - For default/fallback values (safe to commit)
4. **GitHub Secrets** - For CI/CD pipelines

## Configuration Priority

Environment variables take priority over `appsettings.json`:

```
1. Environment Variables (highest priority)
2. .env file (loaded into environment at startup)
3. appsettings.{Environment}.json
4. appsettings.json (lowest priority)
```

## Environment Variables

### Database Configuration

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_HOST` | Database server hostname | `localhost` |
| `DB_PORT` | Database server port | `5432` |
| `DB_NAME` | Database name | `cleanarchdb` |
| `DB_USER` | Database username | `postgres` |
| `DB_PASSWORD` | Database password | `postgres` |

Alternatively, use the full connection string:
| Variable | Description |
|----------|-------------|
| `ConnectionStrings__DefaultConnection` | Full PostgreSQL connection string |

### JWT Authentication

| Variable | Description | Default |
|----------|-------------|---------|
| `JWT_KEY` | Secret key for signing tokens (min 32 chars) | Development key |
| `JWT_ISSUER` | Token issuer | `CleanArchitecture` |
| `JWT_AUDIENCE` | Token audience | `CleanArchitectureUsers` |
| `JWT_EXPIRY_MINUTES` | Token expiry time in minutes | `60` |

### Application Settings

| Variable | Description | Default |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Production` |
| `ASPNETCORE_URLS` | URLs to listen on | `http://+:80` |
| `LOG_LEVEL` | Minimum log level | `Information` |
| `LOG_PATH` | Log file directory | `logs` |

## Local Development

### Quick Start

1. Copy the example environment file:
   ```powershell
   Copy-Item .env.example .env
   ```

2. Edit `.env` with your local settings:
   ```env
   DB_HOST=localhost
   DB_PORT=5432
   DB_NAME=cleanarchdb
   DB_USER=postgres
   DB_PASSWORD=your_local_password
   
   JWT_KEY=YourDevelopmentSecretKeyThatIsAtLeast32CharactersLong!
   ```

3. Run the application:
   ```powershell
   dotnet run --project WebApi
   ```

### Using Visual Studio / Rider

The IDE will automatically load the `.env` file when running the application.

## Docker Development

### Using Docker Compose

1. Create your `.env` file (as above)

2. Run with Docker Compose:
   ```bash
   docker-compose up
   ```

Docker Compose automatically loads variables from `.env` and passes them to containers.

### Override for Development

Use `compose.override.yaml` for development-specific settings:
```yaml
services:
  server:
    environment:
      ASPNETCORE_ENVIRONMENT: Development
```

## Production Deployment

### Docker Production

Use `compose.prod.yaml` with environment variables:

```bash
# Set environment variables
export DB_PASSWORD=secure_production_password
export JWT_KEY=YourSecureProductionKeyThatIsAtLeast32CharactersLong!

# Run with production compose file
docker-compose -f compose.yaml -f compose.prod.yaml up -d
```

### Kubernetes

Create a Secret for sensitive values:
```yaml
apiVersion: v1
kind: Secret
metadata:
  name: app-secrets
type: Opaque
stringData:
  DB_PASSWORD: "secure_production_password"
  JWT_KEY: "YourSecureProductionKeyThatIsAtLeast32CharactersLong!"
```

Reference in your Deployment:
```yaml
spec:
  containers:
  - name: api
    envFrom:
    - secretRef:
        name: app-secrets
    env:
    - name: DB_HOST
      value: "postgres-service"
```

### Cloud Platforms

#### Azure App Service
- Use **Configuration > Application settings** in Azure Portal
- Or use Azure Key Vault references

#### AWS ECS/Fargate
- Use **Task Definition > Container Definitions > Environment**
- Or use AWS Secrets Manager

#### Google Cloud Run
- Use **Service > Edit & Deploy > Variables & Secrets**
- Or use Google Secret Manager

## GitHub Actions

### Setting Up Secrets

1. Go to your repository **Settings > Secrets and variables > Actions**

2. Add the following secrets:

   **For Testing:**
   - `DB_PASSWORD_TEST`
   - `JWT_KEY_TEST`

   **For Staging:**
   - `STAGING_DB_HOST`
   - `STAGING_DB_PORT`
   - `STAGING_DB_NAME`
   - `STAGING_DB_USER`
   - `STAGING_DB_PASSWORD`
   - `STAGING_JWT_KEY`
   - `STAGING_JWT_ISSUER`
   - `STAGING_JWT_AUDIENCE`

   **For Production:**
   - `PROD_DB_HOST`
   - `PROD_DB_PORT`
   - `PROD_DB_NAME`
   - `PROD_DB_USER`
   - `PROD_DB_PASSWORD`
   - `PROD_JWT_KEY`
   - `PROD_JWT_ISSUER`
   - `PROD_JWT_AUDIENCE`

### Using in Workflows

The provided `.github/workflows/ci-cd.yml` automatically uses these secrets:

```yaml
env:
  DB_PASSWORD: ${{ secrets.PROD_DB_PASSWORD }}
  JWT_KEY: ${{ secrets.PROD_JWT_KEY }}
```

## Security Best Practices

### DO

- Use strong, unique passwords for production
- Use JWT keys with at least 32 characters
- Rotate secrets regularly
- Use secret management services (Azure Key Vault, AWS Secrets Manager)
- Keep `.env` files out of version control

### DON'T

- Commit `.env` files to git
- Use default passwords in production
- Share secrets in plain text
- Log sensitive configuration values

### Validation

The application validates configuration at startup in production:

- Rejects default/insecure JWT keys
- Rejects default database passwords
- Validates JWT key length (minimum 32 characters)

If validation fails, the application will not start.

## Troubleshooting

### Environment variables not loading

1. Check file exists: `ls .env`
2. Check file format (no spaces around `=`)
3. Check for BOM characters in file
4. Restart the application

### Docker not reading .env

1. Ensure `.env` is in the same directory as `compose.yaml`
2. Check variable substitution: `docker-compose config`

### Production validation failing

Ensure you've set:
- `JWT_KEY` to a unique, secure value (not containing "YourSuperSecret")
- `DB_PASSWORD` to something other than "postgres"

## File Reference

| File | Purpose | Git |
|------|---------|-----|
| `.env.example` | Template with all variables | ✅ Commit |
| `.env` | Local development values | ❌ Never commit |
| `appsettings.json` | Default configuration | ✅ Commit |
| `appsettings.Development.json` | Development overrides | ✅ Commit |
| `appsettings.Production.json` | Production overrides | ✅ Commit |
| `compose.yaml` | Docker Compose base | ✅ Commit |
| `compose.override.yaml` | Local Docker overrides | ✅ Commit |
| `compose.prod.yaml` | Production Docker config | ✅ Commit |

