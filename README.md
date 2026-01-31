NewsScope API

An API for calculating NEWS (National Early Warning Score) values from clinical measurements.

This project was implemented as a backend engineering case assignment. The focus is on extensibility, testability, and clean separation of concerns, rather than building a fully deployed production system.


Design & Architecture

I designed this solution to follow Clean Architecture principles, ensuring that the business logic is decoupled from the HTTP layer.

1. Strategy Pattern for Extensibility

Instead of a large switch statement or hardcoded logic, I implemented the Strategy Pattern for the scoring rules.


INewsScoreService: Acts as the orchestrator. It doesn't know the medical rules; it simply delegates to the strategies.

IMeasurementScoreStrategy: Each vital sign (TEMP, HR, RR) has its own class.

Benefit: This adheres to the Open/Closed Principle. If we need to add "Oxygen Saturation" later, we simply add a new Strategy class and register it. The core service logic remains untouched.


2. Dynamic Validation

Validation is not hardcoded to "3 measurements."


The service checks the incoming request against the registered strategies in the Dependency Injection container.

If a new strategy is added, the validation logic automatically updates to require it.

It also detects and rejects unsupported measurement types to prevent runtime errors.


3. Thin Controllers

The controller handles only HTTP concerns (requests, responses, status codes). All business logic and validation live in the Domain layer.


 Testing Strategy

The project includes a comprehensive test suite using xUnit


Data-Driven Tests: Used [Theory] and [InlineData] to verify clinical boundaries and edge cases (e.g., verifying that 38°C is Score 0 but 39°C is Score 1).

Unit Testing: The NewsScoreService is tested in isolation  to verify the orchestration logic (summing and validation) works independently of the specific medical rules.


 Assumptions & Trade-offs

Since this is a time-boxed assignment, I made the following pragmatic decisions that I would revisit in a production environment:


Data Types (int vs double):


I used int for measurement values to keep the implementation simple based on the provided examples.

Production View: In a real clinical setting, Body Temperature requires decimal precision (e.g., 37.5°C). I would switch to double or decimal to avoid precision loss.


Hardcoded Rules:

The scoring ranges (e.g., 31-35) are currently defined in the Strategy classes.

Production View: Medical rules change. I would move these configurations to a Database or a Configuration Service (like Azure App Config) so they can be updated without a code deployment.

Security:

This MVP does not implement Authentication.

Production View: I would add Azure AD integration and Global Exception Handling middleware to ensure no stack traces are leaked.


How to Run

Prerequisites: .NET 6.0 or later.



Run the API:
 dotnet run --project NewsScope.Api
    

Run the Tests:
dotnet test
    

Explore the API:
Navigate to http://localhost:5144/swagger (port may vary) to test the endpoint via Swagger UI.


Example Request

POST /api/news/calculate

json

{
  "measurements": [
    { "type": "TEMP", "value": 38 },
    { "type": "HR", "value": 90 },
    { "type": "RR", "value": 18 }
  ]
}

Example response
{
  "score": 3
}

Validation

The API returns 400 Bad Request in the following cases:

A required measurement is missing

A measurement type is duplicated

A measurement value is outside the allowed range

The request body cannot be parsed

Validation is performed early to avoid producing incorrect or misleading clinical scores.


Possible Improvements

Given more time, the following could be added:

Persistence using EF Core and PostgreSQL

More detailed error responses

Authentication and authorization
