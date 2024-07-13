# ItHappened Web Application
## Overview
The "ItHappened" web application is a robust platform designed to help users track, customize, and analyze events in their lives.  
It leverages a .NET backend with a React frontend to provide a seamless and responsive user experience.

## Technology Stack
- Backend: .NET Core, Entity Framework Core, JWT for authentication
- Frontend: React.js, Node.js
- Database: SQL Server
- CI/CD: GitHub Actions

## Project Structure
- App: The main application containing the backend logic.
- Domain: Core business models and domain logic.
- Infrastructure: Database context and repositories.
- Authentication: JWT-based authentication implementation.
- Controllers: API controllers handling HTTP requests.
- Models: Data transfer objects and request/response models.
- Postman collection is provided for API testing.
- Unit tests were created for various components.

## Getting Started
Prerequisites:
- .NET SDK
- Node.js and npm

### Usage

1. Setup Backend from ItHappened.App:
```
sh
Copy code
cd ItHappened.App
dotnet restore
dotnet build
dotnet run
```
2. Setup Frontend from ItHappened.nodejs:

```
sh
Copy code
cd ithappened.nodejs
npm install
npm start
```

Run Tests from ItHappened.Tests:
```
sh
Copy code
cd ItHappened.Tests
dotnet test
```
