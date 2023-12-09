
As a software developer, your task is to create a **service** that can effectively retrieve and store the prices of a given **list of securities**. To accomplish this, please consider the following guidelines and requirements:

1. Definition of a Security:
A security is a financial instrument identified by its ISIN, which is a 12-character alphanumeric code.

2. Code Design:
Implement the service according to SOLID principles.
Utilize Dependency Injection to enhance code decoupling.

3. Service Method:
The service should expose a method that accepts a list of ISINs as input.
This list will represent the securities for which you need to retrieve and store prices.

4. Data Storage:
The service should retrieve and store the price information for each ISIN in a SQL Server database.

5. API Integration:
To retrieve the price of an ISIN, your service should make use of an external web API (https://securities.dataprovider.com/securityprice/{isin}) to fetch prices.
Ensure that you handle API requests and responses gracefully, including error handling and data parsing.

6. Unit Testing Approach:
Show how you would unit test your service.
Explain the process by providing examples and using the "Arrange, Act, Assert" pattern. You don't need to test all your code, but demonstrate your understanding of how you would test various aspects of the service, including using mocks and providing clear explanations.

---

In order to solve the external dependencies, make sure you have Docker Desktop installed in your local machine,
open this folder in a CLI interface such as powershell or cmd, 
so run the command: <br/>
` docker-compose up --build --force-recreate --remove-orphans` <br/>
it should be enough to have the service up and running with:

- json-server mocking an external api server
- mssqlserver instance ready to receive the required data.
