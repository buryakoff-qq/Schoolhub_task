# SchoolHub API

RESTful API for managing students and school classes, built on .NET 9.0 using Clean Architecture and Domain-Driven Design principles.

The project provides CRUD operations for students (with unique student IDs) and school classes (with ability to assign/unassign students, max 20 per class). The architecture is organized into Domain, Application, Infrastructure, and API layers.

## Technologies

- .NET 9.0
- ASP.NET Core Minimal API
- Entity Framework Core 9.0
- Scalar
- xUnit
- Moq

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
dotnet test
cd src/SchoolHub.API
dotnet run
```

## API Endpoints

### Students

#### Get all students
```
GET /students
Returns: 200 OK
```

#### Get student by ID
```
GET /students/{id}
Returns: 200 OK, 404 Not Found
```

#### Get student by StudentId
```
GET /students/by-id/{studentId}
Returns: 200 OK, 404 Not Found
```

#### Create student
```
POST /students

{
  "studentId": "STU001",
  "firstName": "John",
  "lastName": "Doe",
  "birthDate": "2010-01-15",
  "city": "New York",
  "street": "Main St",
  "postalCode": "10001"
}
Returns: 201 Created, 400 Bad Request

Required fields: studentId, firstName, lastName, birthDate
Optional fields: city, street, postalCode
```

#### Update student
```
PUT /students/{id}

{
  "studentId": "STU001",
  "firstName": "Jane",
  "lastName": "Smith",
  "birthDate": "2010-01-15",
  "city": "Boston",
  "street": "Park Ave",
  "postalCode": "02101"
}
Returns: 200 OK, 404 Not Found, 400 Bad Request
```

#### Delete student
```
DELETE /students/{id}
Returns: 204 No Content, 404 Not Found
```

### School Classes

#### Get all classes
```
GET /classes
Returns: 200 OK
```

#### Get class by ID
```
GET /classes/{id}
Returns: 200 OK, 404 Not Found
```

#### Create class
```
POST /classes

{
  "name": "Mathematics",
  "teacher": "Mr. Smith"
}
Returns: 201 Created, 400 Bad Request
```

#### Update class
```
PUT /classes/{id}

{
  "name": "Advanced Mathematics",
  "teacher": "Ms. Johnson"
}
Returns: 200 OK, 404 Not Found, 400 Bad Request
```

#### Delete class
```
DELETE /classes/{id}
Returns: 204 No Content, 404 Not Found
```

#### Assign student to class
```
POST /classes/{classId}/assign/{studentId}
Returns: 200 OK, 404 Not Found, 400 Bad Request

Constraints: Maximum 20 students per class, student cannot be assigned twice
```

#### Unassign student from class
```
DELETE /classes/{classId}/unassign/{studentId}
Returns: 204 No Content, 404 Not Found
```

## Testing

The project includes unit tests:
- **Domain layer**: 26 tests (entities, value objects, business rules)
- **Application layer**: 28 tests (services with mocked repositories)