# SchoolHub API

RESTful API for managing students and school classes, built on .NET 9.0 using Clean Architecture and Domain-Driven Design principles.

The project provides CRUD operations for students (with unique student IDs) and school classes (with ability to assign/unassign students, max 20 per class). The architecture is organized into Domain, Application, Infrastructure, and API layers.

## Technologies

- **.NET 9.0**
- **ASP.NET Core Minimal API**
- **Entity Framework Core 9.0** (In-Memory Database)
- **Scalar** for interactive API documentation
- **xUnit** for unit testing
- **Moq** for mocking dependencies in tests

## Project Structure

```
Schoolhub test task/
├── src/
│   ├── SchoolHub.API/
│   │   ├── Endpoints/
│   │   ├── Extensions/
│   │   └── Program.cs
│   │
│   ├── SchoolHub.Application/
│   │   ├── DTOs/
│   │   ├── Interfaces/
│   │   ├── Services/
│   │   ├── Mappers/
│   │   └── Exceptions/
│   │
│   ├── SchoolHub.Domain/
│   │   ├── Entities/
│   │   ├── ValueObjects/
│   │   └── Exceptions/
│   │
│   └── SchoolHub.Infrastructure/
│       ├── Persistence/
│       └── Repositories/
│
└── tests/
    ├── SchoolHub.Domain.Tests/
    └── SchoolHub.Application.Tests/
```

## Run and Test

```bash
dotnet build
```

```bash
dotnet test
```

```bash
cd src/SchoolHub.API
dotnet run
```

API will be available at `https://localhost:5001` or `http://localhost:5000`.  
OpenAPI documentation: `https://localhost:5001/openapi/v1.json`  
Scalar UI (Development only): `https://localhost:5001/scalar/v1`

## API Endpoints

### Students

#### Get all students
```
GET /students
```

#### Get student by ID
```
GET /students/{id}
```

#### Get student by StudentId
```
GET /students/by-id/{studentId}
```

#### Create student
```
POST /students
Content-Type: application/json

{
  "studentId": "STU001",
  "firstName": "John",
  "lastName": "Doe",
  "birthDate": "2010-01-15",
  "city": "New York",
  "street": "Main St",
  "postalCode": "10001"
}
```

**Required fields**: `studentId`, `firstName`, `lastName`, `birthDate`  
**Optional fields**: `city`, `street`, `postalCode`

#### Update student
```
PUT /students/{id}
Content-Type: application/json

{
  "studentId": "STU001",
  "firstName": "Jane",
  "lastName": "Smith",
  "birthDate": "2010-01-15",
  "city": "Boston",
  "street": "Park Ave",
  "postalCode": "02101"
}
```

#### Delete student
```
DELETE /students/{id}
```

### School Classes

#### Get all classes
```
GET /classes
```

#### Get class by ID
```
GET /classes/{id}
```

#### Create class
```
POST /classes
Content-Type: application/json

{
  "name": "Mathematics",
  "teacher": "Mr. Smith"
}
```

#### Update class
```
PUT /classes/{id}
Content-Type: application/json

{
  "name": "Advanced Mathematics",
  "teacher": "Ms. Johnson"
}
```

#### Delete class
```
DELETE /classes/{id}
```

#### Assign student to class
```
POST /classes/{classId}/assign/{studentId}
```

**Constraints**:
- Maximum 20 students per class
- A student cannot be assigned to the same class twice

#### Unassign student from class
```
DELETE /classes/{classId}/unassign/{studentId}
```

## Business Rules

### Students
- `studentId` must be unique
- `firstName` and `lastName` are required and cannot be empty
- `birthDate` is required
- Address (city, street, postalCode) is optional

### School Classes
- `name` and `teacher` are required and cannot be empty
- Maximum 20 students per class
- A student cannot be assigned to the same class twice

## Error Handling

API endpoints return appropriate HTTP status codes (200, 201, 204, 400, 404) without error details in the response body.
