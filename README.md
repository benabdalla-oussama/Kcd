# Kcd API

## Project Overview

Kcd API is a .NET 8-based application that serves as a comprehensive solution for managing user applications with advanced features. The project is structured into multiple components and functionalities, including identity management, application management, and a Blazor WebAssembly UI. It also integrates various tools for testing, and Docker deployment. This README provides an overview of the project structure, setup instructions, implemented UI scenarios, features, and performance analysis tools.

## Project Structure

- **`Kcd.API`**: The main ASP.NET Core Web API project.
- **`Core`**:
  - **`Kcd.Application`**: Contains the business logic and application services.
  - **`Kcd.Common`**: Contains common utilities and shared components.
  - **`Kcd.Domain`**: Contains domain models and entities.
- **`Infrastructure`**:
  - **`Kcd.Identity`**: Manages identity and authentication logic. Exposes an `IAuthService` interface for authentication, allowing flexibility for future changes (e.g., integrating with an identity server).
  - **`Kcd.Infrastructure`**: Contains infrastructure-level components and services.
  - **`Kcd.Persistence`**: Manages data persistence for `UserApplication` and avatar storage, including repositories.
- **`Kcd.UI`**: A Blazor WebAssembly project showcasing the API and providing a user interface.
- **`Tests`**:
  - **`Kcd.API.Tests`**: Integration tests for the API, including tests for the `ApplicationsController` using Testcontainers.
  - **`Kcd.Core.Tests`**: Unit tests for core application logic.
  - **`Kcd.Infrastructure.Tests`**: Unit tests for infrastructure components.
  - **`K6Performance`**: Contains the `K6` performance test scripts for stress and load testing of the API.
- **`Benchmarks`**:
  - **`Kcd.Benchmarks`**: Contains benchmark tests using `BenchmarkDotNet` to analyze API performance.

## Setup Instructions

To set up and run the project, follow these steps:

1. **Run SQL Server Docker Container**:
   Ensure you have Docker installed. Start a SQL Server container using the following command:
   ```sh
   docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=your_password' -p 1433:1433 --name sqlserver --hostname sqlserver -d mcr.microsoft.com/mssql/server:2019-latest
   ```
   Replace `your_password` with a strong password for the SQL Server instance.

2. **Run the API**:
   - Navigate to the `Kcd.API` project directory.
   - Restore dependencies and run the application:
     ```sh
     dotnet restore
     dotnet run
     ```
   - The API should now be running on `http://localhost:5160` (or your configured port).

3. **Run the Blazor UI**:
   - Navigate to the `Kcd.UI` project directory.
   - Restore dependencies and run the Blazor WebAssembly application:
     ```sh
     dotnet restore
     dotnet run
     ```
   - The UI should be available on `http://localhost:5058` (or your configured port).
   - Note: For testing purposes, a seed admin user is available with the following credentials:
     - **`Email`**: `admin@admin.com`
     - **`Password`**: `Azerty123456789!.`

4. **Run Tests**:
   - Navigate to the respective test project directory.
   - Ensure the SQL Server container is running.
   - Execute the tests:
     ```sh
     dotnet test
     ```
     
5. **Run K6 Performance Tests**:
   - Ensure k6 is install from the official website: [Install k6](https://grafana.com/docs/k6/latest/set-up/install-k6/).
   - Navigate to the Tests/K6Performance directory.
   - Ensure the API is running (either via Docker or locally).
   - Restore dependencies and run the application:
     ```sh
     k6 run k6-performance-test.js
     ```
     
## Implemented UI Scenarios

- **Login**: Users can authenticate and access the application.
  - **Roles**: Two roles are implemented:
    - **User**: Default role assigned to users after being approved by the admin.
    - **Admin**: Can view and manage user applications.
- **User Applications**:
  - **Anonymous Users**: Can apply for user applications without being logged in.
  - **Admin Users**: Receive and manage applications, including filtering by status, and approving or rejecting applications.
- **User Profiles**: Once approved, users can log in to view their profile information and avatar.

## Features

- **Authentication & Authorization**: Integrated with ASP.NET Core Identity for managing user roles and access.
- **User Application Management**: API endpoints and UI functionalities for managing user applications.
- **Avatar File Saving Strategies**:
  - **Database**: Store avatars directly in the database.
  - **FileSystem**: Save avatars to the file system.
  - **Blob Storage**: Use Azure Blob Storage for avatar file storage.
- **Health Checks**: Monitors the health of both SQL Server and SQLite databases.
- **Entity Framework & Migrations**:
  - Applies migrations to both Identity and UserApplication databases.
  - SQLite database is used in-memory for testing purposes.
- **Dockerized API**: Docker configuration for easy deployment.
- **Swagger Documentation**: Provides interactive API documentation.
- **Exception Handling**: Uses global exception handling middleware to manage and log exceptions.
- **Logging**: Integrated logging using Serilog for detailed logging and diagnostics.
  
## Performance Analysis

For performance analysis, consider using these tools:

- **BenchmarkDotNet**: Ideal for micro-benchmarking .NET code to profile and optimize specific methods or code snippets.
- **k6**: Developer-centric tool for load testing with scripting capabilities.

## Assumptions

- **User Application Process**: Users submit an application without initial login credentials. Applications are stored in SQLite and moved to SQL Server upon approval.
- **Authentication & Authorization**: ASP.NET Identity manages authentication. No integration with external identity providers is included in this version but could be added later.
- **Admin Role**: The Admin role is responsible for reviewing, approving, or rejecting applications. No other roles or intermediate validation processes are implemented.
- **Avatar Upload**: Avatar upload is optional. Images are stored in the file system, database or Azure Blob Storage based on the configured strategy, with future potential for additional strategies (ftp ...).
- **Optional Fields**: "Company", "Referal" and "Avatar" are optional and do not affect the application process if left empty.
- **Performance Testing**: Performance testing with k6 focuses on the load and stress handling of the endpoints (added one exemple).
       
## Notes

- **SQLite Database Cleanup Integration Test**: The SQLite database is recreated on each test run to ensure a fresh state.
  
- **Perspectives**:
  - **Improvements**: Consider adding features like password reset, email invitations, and enhanced user management based on requirements.
  - **CI/CD Pipelines**: Implementing continuous integration and continuous deployment (CI/CD) pipelines can streamline development and deployment processes.
  - **Code Quality Checks**: Adding SonarQube for code quality analysis can help maintain high code quality and identify potential issues early in the development process.

