# Copilot Instructions for This Project

## Architecture & Design Patterns

- **Clean Architecture**: Follow the existing clean architecture structure in this project
- **CQRS Pattern**: Use Command Query Responsibility Segregation as already implemented in this project
- **Repository Pattern**: Implement data access using the repository pattern
- **Dependency Injection**: Always use dependency injection for all dependencies

## API Development

- **Controllers**: Always use controllers for API endpoints
- **Keep Controllers Clean**: Controllers should be thin and only handle HTTP concerns (routing, model binding, response formatting)
- **DTOs**: Use Data Transfer Objects (DTOs) where it makes sense to separate domain models from API contracts
- **Validation**: Use FluentValidation for all input validation

## Code Organization

- **Single Responsibility**: Use individual files for classes, interfaces, DTOs, records, enums, etc.
- **File Naming**: File names should match the type name they contain

## Documentation

- **XML Comments**: Always add XML summary comments to:
  - Public classes, interfaces, and records
  - Public methods and properties
  - Controllers and their actions
  - DTOs and their properties

## Performance & Scalability

- **Redis Caching**: Use Redis caching where needed to improve performance
- **High Traffic Optimization**: This API serves over 10M+ active users - optimize for:
  - Performance
  - Scalability
  - Efficient database queries
  - Appropriate caching strategies
  - Async/await patterns

## Error Handling

- **Meaningful Error Responses**: Define meaningful error responses for:
  - Validation failures
  - Business logic errors
  - System errors
- **Consistent Error Format**: Use a consistent error response structure across the API

## Testing

- **xUnit**: Use xUnit for all unit and integration tests
- **Test Coverage**: Write tests for:
  - Business logic
  - Validators
  - Command/Query handlers
  - Repository implementations

## Best Practices

- Follow SOLID principles
- Use async/await for I/O operations
- Follow RESTful conventions for API design
- Use appropriate HTTP status codes
- Implement proper exception handling and logging

## Workflow

- **Always Show Plan First**: Before implementing any feature or change, present a plan outlining:
  - What will be created/modified
  - Which layers will be affected
  - File structure and organization
  - Any architectural decisions
  
Wait for approval before proceeding with implementation.

## Technology Stack

- **.NET 10**: Target framework for all projects
- **Background Services**: Use `BackgroundService` class for long-running tasks
