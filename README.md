# Todo Management API

A RESTful Todo Management API built with ASP.NET Core 8.0, PostgreSQL, and Docker. Features JWT authentication, full CRUD operations, pagination, filtering, sorting, and search.

## 📁 Project Files

| File | Description |
|------|-------------|
| `FinalProjectDss.csproj` | Project configuration and NuGet packages |
| `Program.cs` | Application entry point and middleware setup |
| `appsettings.json` | Database connection and JWT settings |
| `Dockerfile` | Docker image configuration |
| `docker-compose.yml` | Multi-container orchestration |
| `README.md` | This file |

## 📂 Source Code Folders

| Folder | Contents |
|--------|----------|
| `Controllers/` | API endpoints (Auth, Todo) |
| `Models/` | User and Todo entities |
| `DTOs/` | Request/Response data models |
| `Data/` | Database context |
| `Repositories/` | Data access layer |
| `Services/` | Business logic |

## 📄 Report & Screenshots

| File | Description |
|------|-------------|
| `Project_Report.pdf` | Complete project documentation |
| `Screenshots/` | Folder containing all API and database screenshots |

### Screenshots Included:
- Swagger UI pages
- Register/Login endpoints
- Create/Update/Delete todo operations
- PATCH completion toggle
- Pagination response
- Validation errors (400)
- Duplicate email error (409)
- Unauthorized error (401)
- Database tables (Users & Todos)
- Docker containers

## 🚀 Quick Start

```bash
# Start PostgreSQL
docker run --name postgres-todo -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=todo_db -p 5432:5432 -d postgres:15

# Run API
dotnet restore
dotnet build
dotnet run

# Open Swagger
http://localhost:3087/swagger