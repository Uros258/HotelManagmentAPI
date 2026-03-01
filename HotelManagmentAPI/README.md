# Hotel Management API

A REST API for hotel management built with ASP.NET Core 8 and C#.

## Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core 8 (Code First)
- SQL Server
- JWT Authentication
- FluentValidation
- xUnit

## Features
- Guest, room, reservation, and billing management
- Check-in / check-out with automatic room status updates
- Overbooking protection
- Role-based access control (Admin, Receptionist, Housekeeping)
- Global error handling middleware
- Input validation with FluentValidation
- Unit tests with xUnit

### Setup
1. Clone the repository
2. Update the connection string in `appsettings.json`
3. Run `dotnet ef database update`
4. Run `dotnet run`
5. Open Swagger at `https://localhost:{port}/swagger`

## Authentication
Register via `POST /api/Auth/register`, login via `POST /api/Auth/login` to receive a JWT token. Pass it as `Bearer {token}` in the Authorize header in Swagger.

## Roles
| Role | Access |
|------|--------|
| Admin | Full access |
| Receptionist | Guests, reservations, bills |
| Housekeeping | Room status updates only |