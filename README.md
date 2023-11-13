# Interview4Create project

## The attachment contains the solution of the Company-Employee task application.

The following methodologies were used for task implementation:

Hexagonal (Clean) architecture combined with the CQRS architectural pattern (only commands were used due to the nature of the task - no GET endpoints), as well as with the Mediator design pattern (specifically the implementation of the pattern from the MediatR library). Command results were handled using the Operation Result pattern. Request validation was performed in the MediatR Behavior Pipeline using FluentValidation.
For the business logic itself, the principles of DDD were applied (business logic in the model itself, Identity as a separate object for the specific identification of entities etc).

The business logic and tests were written using concepts from the Unit Testing book by author Vladimir Khorikov.
Used FluentAssertion library for results assertion.

Used Scrutor library for dependency injection.




The application consists of:

- **Domain layer**

- **Application layer (these two layers are the Core of the application)**

- **Infrastructure layer**

- **Presentation (specifically the API layer)**

- **and a DependencyInjection project, which serves as a Gateway for the Presentation layer.**


```diff
To run the application, it is necessary to change the connection string from the appSettings, which will target the local database on your computer.
Default data has also been created. After configuring the connection string, the application is ready to run. 
When the application starts, migrations will run automatically, and the application will be ready to use. 
```
