# Orders API

A clean, maintainable Orders API using C#, .NET Web API, and SQLite for persistence.

## Project Structure

This project follows a clean architecture approach with the following components:

- **Controllers**: Handle HTTP requests and responses
- **Services**: Implement business logic
- **Repositories**: Handle data access
- **Data**: Contains database context and configurations
- **Models**: Domain entities
- **DTOs**: Data Transfer Objects for API requests and responses

## Setup Instructions

### Prerequisites

- .NET 8 SDK
- dotnet-ef tools (if not installed, run: `dotnet tool install --global dotnet-ef`)

### Configuration

The project uses the following configuration files:

- **appsettings.json**: Contains the main application settings
- **appsettings.Development.json**: Contains development-specific settings

Example appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=orders.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Database Setup

The project uses SQLite as the database. The connection string is configured in `appsettings.json`.

To create the database:

```bash
dotnet ef database update
```

## Running the API

To run the API locally:

```bash
dotnet run
```

The API will be available at:
- https://localhost:5001
- http://localhost:5000

Swagger UI will be available at:
- https://localhost:5001/swagger
- http://localhost:5000/swagger

## API Endpoints

### Create Order

```
POST /api/orders
```

Request body:
```json
{
  "orderId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "customerName": "John Doe",
  "createdAt": "2023-01-01T12:00:00Z",
  "items": [
    {
      "productId": "product-123",
      "quantity": 2
    }
  ]
}
```

Response:
- Status: 201 Created
- Body: The created order ID

### Get Order

```
GET /api/orders/{id}
```

Response:
- Status: 200 OK
- Body: The order details

## Testing

To run the tests:

```bash
dotnet test
```

## Design Decisions

1. **SQLite Database**: Used for simplicity and portability
2. **Async/Await**: Used throughout for better performance and scalability
3. **Repository Pattern**: Used to abstract data access
4. **Service Layer**: Used to encapsulate business logic
5. **DTOs**: Used to decouple API contracts from domain models