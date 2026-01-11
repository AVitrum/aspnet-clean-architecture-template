## Description
This project follows the principles of Clean Architecture in ASP.NET Core, ensuring a modular, maintainable, and testable codebase. The architecture separates concerns into distinct layers (Domain, Application, Infrastructure, and Presentation), making the system easier to extend and scale.

## Features
- Clean Architecture principles applied for maintainability and testability.
- Swagger/OpenAPI for API documentation and testing.
- Docker Compose setup for containerized development and deployment.
- Separation of concerns with distinct layers for domain logic, application services, and infrastructure.
- **Environment-based configuration** with support for Docker, GitHub Actions, and cloud deployments.

## Project Structure
- `Domain`: Contains the core business logic and entities.
- `Application`: Contains application services, interfaces, and DTOs.
- `Infrastructure`: Contains data access implementations and external service integrations.
- `WebApi`: Contains the ASP.NET Core Web API project.

## Quick Start

### Prerequisites
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://docs.docker.com/get-docker/) (optional, for containerized development)
- [PostgreSQL](https://www.postgresql.org/) (or use Docker)

### Setup Environment

1. Copy the example environment file:
   ```shell
   cp .env.example .env
   ```

2. Edit `.env` with your local settings (database credentials, JWT key, etc.)

3. Run locally:
   ```shell
   dotnet run --project WebApi
   ```

For detailed environment configuration, see [docs/ENVIRONMENT.md](docs/ENVIRONMENT.md).

## Running with Docker Compose

To run the project using Docker Compose, ensure you have Docker and Docker Compose installed on your system.

1.  Make sure you are in the root directory of the project where the `compose.yaml` file is located.

2.  Create your `.env` file (see Quick Start above).

3.  Run the following command to build the images and start the containers:
    ```shell
    docker-compose up --build
    ```
    You can add the `-d` flag to run the containers in detached mode (in the background):
    ```shell
    docker-compose up --build -d
    ```

4.  Once the application is running, you can find the API documentation at `https://localhost:8001/swagger/index.html`.

5.  To stop and remove the containers, networks, and volumes, run:
    ```shell
    docker-compose down
    ```

## Environment Configuration

The application uses environment variables for configuration, supporting:

| Variable | Description | Default |
|----------|-------------|---------|
| `DB_HOST` | Database hostname | `localhost` |
| `DB_PORT` | Database port | `5432` |
| `DB_NAME` | Database name | `cleanarchdb` |
| `DB_USER` | Database username | `postgres` |
| `DB_PASSWORD` | Database password | `postgres` |
| `JWT_KEY` | JWT signing key (min 32 chars) | - |
| `JWT_ISSUER` | JWT issuer | `CleanArchitecture` |
| `JWT_AUDIENCE` | JWT audience | `CleanArchitectureUsers` |

See [docs/ENVIRONMENT.md](docs/ENVIRONMENT.md) for complete documentation.

## CI/CD

The project includes a GitHub Actions workflow (`.github/workflows/ci-cd.yml`) for:
- Building and testing
- Docker image creation
- Staging and production deployments

Configure GitHub Secrets for your deployment environments.