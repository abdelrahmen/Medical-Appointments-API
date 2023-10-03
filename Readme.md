# Medical Appointments API

Medical Appointments API is a RESTful web service built with ASP.NET Core for managing medical appointments, medical histories, and related data. It provides features for scheduling appointments, maintaining medical histories, and more.

## Roles
- Admin(only 1):
	- Can Add Medical Professionals.
- Medical Professionals:
	- Can Create Appointments That are avaialable For Patients to Book.
	- Can cancel booked appoinmtents.
- Patient:
	- can search by Medical Professional Speciality for available appointments.
	- can book appointments created by Medical Professionals.
	- can create a medical history record for him self.

## Table of Contents

- [Getting Started](#getting-started)
- [Features](#features)
- [API Endpoints](#api-endpoints)
- [Authentication](#authentication)
- [Contributing](#contributing)

## Getting Started

### Prerequisites

Ensure you have the following prerequisites installed:

- **.NET Core 6.0**: Download and install from [dotnet.microsoft.com](https://dotnet.microsoft.com/download/dotnet/6.0).

### Installation

1. Clone the repository:

	```git clone https://github.com/abdelrahmen/Medical-Appointments-API.git```

2. Navigate to the project directory:

	```cd medical-appointments-api```

3. Build and run the application:

	```
	dotnet build
	dotnet run
	```
The API is now accessible at `http://localhost:5157`.

## Features

- **Medical Histories**: Record and manage patient medical histories.
- **Appointments**: Schedule, view, and cancel appointments.
- **Identity Framework**: Secure user authentication and authorization using Identity Framework.
- **Pagination**: Support for pagination in API responses.
- **Role-based Access**: Define roles for users, including Medical Professionals and Admins.
- **JWT Authentication**: Authenticate users using JSON Web Tokens.

## API Endpoints

View the API documentation for a complete list of endpoints.

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. To access protected endpoints, users must obtain a valid token.

Contributions are welcome! If you'd like to contribute, follow these steps:

1. Fork the repository.
2. Create a new branch for your feature: `git checkout -b feature-name`
3. Implement your changes and commit them: `git commit -m 'Add new feature'`
4. Push to your branch: `git push origin feature-name`
5. Submit a pull request.
