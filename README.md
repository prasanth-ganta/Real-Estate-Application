# Real Estate Property Listing API

The Real Estate Property Listing API is a .NET Web API application that provides the backend services for a real estate property listing platform. It allows users to perform CRUD operations on properties, manage user accounts, and enable communication between buyers/sellers. It also includes an admin interface for managing the platform.

## Features

### Basic Level Requirements

1. **Account Registration**: Users can create an account to post or inquire about properties.
2. **Property Management**: Users can create, read, update, and delete property listings.
3. **Property Search**: Users can search for properties based on location, price, and type.

### Medium Level Requirements

1. **Favorites Management**: Users can save properties they are interested in to a favorites list.
2. **Direct Messaging**: Users can send messages to property owners or agents through the API.

### Admin Basic Level Requirements

1. **Login Capability**: Admins can log in securely to manage listings.
2. **View Listings**: Admins can view all property listings posted by users.

### Admin Medium Level Requirements

1. **Listing Moderation**: Admins can approve or remove property listings for quality control.
2. **Report Generation**: Admins can generate reports on property trends.

## Technologies Used

- .NET Web API
- Entity Framework
- SQL Server
- ASP.NET Core

## Installation and Setup

1. Clone the repository:  https://github.com/prasanth-ganta/Real-Estate-Application.git
2. Open the solution in Visual Studio or your preferred .NET development environment.
3. Update the database connection string in the `appsettings.json` file to match your local database setup.
4. Build and run the application.

## API Documentation

The API documentation can be accessed at the `/swagger` endpoint once the application is running. It provides details on the available endpoints, request/response structures, and authentication requirements.

## Contributing

Contributions are welcome! If you find any issues or have suggestions for improvement, please feel free to submit a pull request or open an issue in the repository.
