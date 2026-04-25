# Inventory Management API

A modern .NET 10 API for managing product inventory, containerized with Docker and using SQLite for data persistence.

## Features

- **Product Management**: CRUD operations for inventory items.
- **SQLite Integration**: Local database persistence with Entity Framework Core.
- **Docker Support**: Fully containerized environment with non-root user security.
- **API Documentation**: Interactive documentation using Scalar.
- **Global Error Handling**: Centralized exception handling and standardized Problem Details.

## How to Run Locally

### 0. Clone the repository

```bash
git clone https://github.com/amaechijude/InventoryManagement
```

### 1. Using Docker (Recommended)

The easiest way to run the application is using Docker Compose, which handles the database setup and volume persistence automatically.

```bash
docker compose up --build
```

Once running, the API will be available at `http://localhost:8080`.

- **API Documentation**: `http://localhost:8080/scalar/v1`
- **Health Check**: `http://localhost:8080/`

### 2. Using .NET CLI

If you have the .NET 10 SDK installed, you can run the API directly.

```bash
dotnet run --project InventoryManagement.Api/InventoryManagement.Api.csproj
```

Or test

```bash
dotnet test
```

_Note: Ensure the `Sqlite` directory exists in the project root if running locally for the first time._

## Assumptions and Design Decisions

- **SQLite for Persistence**: Chosen for its simplicity and zero-configuration requirements, making the project highly portable for demonstrations and local development.
- **Containerization**: The application is designed to be cloud-ready from day one. The Dockerfile uses the latest .NET 10 ASP.NET runtime and enforces a non-root user (`$APP_UID`) for enhanced security.
- **Named Volumes**: Docker named volumes (`sqlite-data`) are used to ensure the database survives container restarts and rebuilds.
- **Scalar for Docs**: Replaced Swagger with Scalar for a more modern and responsive API documentation experience.
- **Automatic Migrations**: The application automatically applies EF Core migrations on startup to ensure the schema is always up to date.
- **Result Pattern**: Used result pattern for cleaner error handling and code.
- **Problem Details**: Standardized Problem Details implementation for consistent error responses.
- **Global Exception Handling**: Centralized exception handling for consistent error handling across the API.
- **Non-root user**: The application runs as a non-root user in Docker for enhanced security.
- **Layered Architecture** : The application is built with a layered architecture for better separation of concerns.
