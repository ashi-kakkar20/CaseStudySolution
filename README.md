# CaseStudySolution
This repository aims at creating a WebAPI for doing a top up on the phone numbers so that the user can make phone calls

# Top Up WebAPI

This project is a .NET Core WebAPI for managing top-up transactions in a mobile application.

## Features

- **User Authentication**: Provides authentication and authorization mechanisms for users.
- **Top Up Transactions**: Allows users to initiate and track their top-up transactions.
- **Transaction History**: Keeps a record of past top-up transactions for each user.
- **ExternalService- This is used to fetch the balance information for the users.
- **Top UP Test- This has the unit tests for the Top UP API.
- **Database script- This has the database script for all the database objects that are to be created.

## Prerequisites

Before you begin, ensure you have met the following requirements:
- .NET Core SDK installed. You can download it from [here](https://dotnet.microsoft.com/download).
- An SQL Server instance for database storage.
  
## Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/your-username/casestudysolution.git
    ```

2. Navigate to the project directory:
    ```bash
    cd casestudysolution
    ```

3. Update the `appsettings.json` file with your database connection string and any other necessary configurations.

4. Apply Entity Framework migrations to set up the database schema:
    ```bash
    dotnet ef database update
    ```

5. Build the project:
    ```bash
    dotnet build
    ```

6. Run the project:
    ```bash
    dotnet run
    ```

The API should now be running. You can use swagger to test the same  locally  by default.


## Authentication

For accessing protected endpoints, include the JWT token obtained after logging in. Add the token to the `Authorization` header in the format `Bearer <token>`.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for any improvements or bug fixes.
