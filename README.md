### Project Details:

This is a WebAPI project with a hexagonal clean architecture implementation to a small use case of a movie registration app, detailed below.

### Thought Process:
- Define a use case
- Define the entities and tables
- Define the endpoints (CRUD for Movies and Users)
- Define the project folder structure
- Create project
- Create the entities
- Create the database adapter
- Create the host webapi
- Create the repositories
- Create the use cases (with TDD)
- Create the controllers (with TDD)
- Define the auth model
- Create the auth service (custom JWT)
- Create the auth controller
- Create the auth middleware
- Create the auth filter
- Create the Error Handler middleware


### Use Case:
![MovieApp-UseCase.png](Images%2FMovieApp-UseCase.png)

### Database:
![MovieApp-Database.png](Images%2FMovieApp-Database.png)

### Getting Started:
 
- Prerequisites:
    - .NET 7.0
    - Visual Studio / Visual Studio Code / Rider
    - SQL Server
    - Docker Desktop
- Clone this repository
- Open the solution in your IDE
- The default database name in the connection string is `MovieApp` (you need create this database in your SQL Server)
- If you want to change the database name, you can change the connection string in the `appsettings.json` file
- If you want to use Docker, you can run the following command in the root of the project: `docker-compose up -d`
- Run the project
- In the first time that you run the project, the tables will be created on database
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
