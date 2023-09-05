### Project Details:

This is a WebAPI project with a hexagonal clean architecture implementation to a small use case of a movie registration app, detailed below.

### Use Case:
![MovieApp-UseCase.png](..%2F..%2FDownloads%2FMovieApp-UseCase.png)

### Database:
![MovieApp-Database.png](..%2F..%2FDownloads%2FMovieApp-Database.png)

### Getting Started:
 
- Prerequisites:
    - .NET 7.0
    - Visual Studio / Visual Studio Code / Rider
    - SQL Server
    - Docker Desktop
- Clone this repository
- Open the solution in your IDE
- Run the project
- In the first time that you run the project, the tables will be created on database
- The default database name in the connection string is `MovieApp`
- If you want to change the database name, you can change the connection string in the `appsettings.json` file
- If you want to use Docker, you can run the following command in the root of the project: `docker-compose up -d`
- Open the Swagger UI in your browser: https://localhost:5001/swagger/index.html
- Use the endpoints to test the application
- Default admin user credentials (created in the first time that you run the project)
    - Email: admin@admin.com
    - Password: admin1234

### Technologies:
- .NET 7.0
- Dapper
- FluentValidation
- FluentAssertions
- nUnit
- Moq
- Docker
- Swagger
- SQL Server

### Architecture:
- Hexagonal Clean Architecture
- Domain Driven Design
- SOLID Principles

### Design Patterns:
- Repository
- Dependency Injection
- Factory
- Domain Use Cases

### About Tests
- Unit Tests: All the domain and application (api) layer is covered by unit tests (100% of coverage).
- The adapter layer (database) is not covered by unit tests. If later its necessary, we can create integration tests for this layer.
