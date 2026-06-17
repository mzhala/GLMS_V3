# GLMS_V3 - Global Logistics Management System

## Youtube Demo
https://youtu.be/JWYS4TopSoc

## Overview

GLMS_V3 is a logistics management system developed using ASP.NET Core MVC, ASP.NET Core Web API, and SQL Server. The solution follows a layered architecture using repositories, services, and API integration to provide a maintainable and scalable application.

## Technologies Used

* ASP.NET Core MVC
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* xUnit Testing Framework
* GitHub Actions
* Docker & Docker Compose
* Swagger/OpenAPI

## Solution Structure

### GLMS_V3

MVC front-end application responsible for the user interface and user interactions.

### GLMS_V3_API

RESTful Web API responsible for business logic, data access, and communication with the database.

### GLMS_V3_Tests

Contains unit tests and integration tests used to validate application functionality and API endpoints.

## Features

* Client Management
* Contract Management
* Service Request Management
* API-driven architecture
* Repository and Service Layer pattern
* Automated Unit Testing
* Automated API Integration Testing
* CI/CD using GitHub Actions
* Docker Containerization

## Automated Testing

The project includes:

* Unit Tests for models, services, and controllers
* Integration Tests for API endpoints
* Automated test execution through GitHub Actions

## Docker Containerization

Docker Compose was implemented to containerize:

1. MVC Application
2. Web API
3. SQL Server Database

The solution can be started using:

```bash
docker compose up
```

## CI/CD

GitHub Actions is configured to:

* Build the solution
* Execute automated tests
* Validate changes before deployment

## API Documentation

Swagger is available when running the API project and can be used to test available endpoints.

## Author

Halasile Mzobe
St10355256

## Module

EAPD7111
