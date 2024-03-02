![.NET 6.0](https://img.shields.io/badge/.NET-6.0-blueviolet) ![MongoDB](https://img.shields.io/badge/MongoDB-%234ea94b.svg?&logo=mongodb&labelColor=white)

# File Generation Server Application

The application is a server-side solution intended to generate text file "reports" based on data stored in MongoDB. It's designed to run as a one-off process, making it suitable for scheduling via tools like CloudWatch to execute at predetermined intervals.

This project serves as an example similar to the ETL processes I've designed and managed in my previous role as a backend engineer. I had a strong desire to rewrite processes in a more modern style so that they could benefit from stronger architectural patterns, but the rapid pace of business rarely allows time for those style rewrites. This is my first attempt, and it represents a modern take on C#/.NET development, embracing Dependency Injection and various Design Patterns to structure a robust and maintainable codebase.


## Features

- **Dependency Injection**: Utilizes Microsoft.Extensions.DependencyInjection for managing dependencies.
- **Configuration Management**: Leverages Microsoft.Extensions.Configuration for flexible application settings.
- **MongoDB Integration**: Employs MongoDB.Bson and MongoDB.Driver to interact with MongoDB for both data retrieval and configuration management.
- **Design Patterns**: Implements patterns such as Factory and Singleton for a clean and scalable architecture.

## Getting Started

Follow these steps to get the application up and running on your local machine for development and testing purposes.

### Prerequisites

Ensure you have the following installed:

- .NET 6.0 SDK or later
- MongoDB Server or access to a deployed mongodb cluster/database on Mongo Atlas

### Installation

1. **Clone the repository**

```bash
git clone https://github.com/ricky-bruner/csharp-examples.git
```

2. **Navigate to the project directory**

```bash
cd IntervalProcessing
```

3. **Restore NuGet packages**

```bash
dotnet restore
```

4. **Set up environment variables**

```plaintext
Copy `configExample.json` to `config.json` and adjust the settings, including your MongoDB connection string.
```

5. **Seed your mongo database**

```plaintext
Copy the script from databaseScript.js into your choice of mongo inputs - I used Studio3T. Run the script
```

5. **Run the application**

```bash
dotnet run
```

## Usage

The application is intended to be executed as a one-off process, which can be scheduled to run at specific intervals using external tools like AWS CloudWatch.

```bash
# Executes the file generation process
dotnet run
```

## Contributing

Your contributions are welcome! Here's how you can contribute:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/YourFeature`)
3. Commit your Changes (`git commit -m 'Add some YourFeature'`)
4. Push to the Branch (`git push origin feature/YourFeature`)
5. Open a Pull Request

## License

This project is distributed under the MIT License. See the `LICENSE` file for more information.

## Contact
My Website: [https://www.rickybruner.com/](https://www.rickybruner.com/)

Project Link: [https://github.com/ricky-bruner/csharp-examples](https://github.com/ricky-bruner/csharp-examples)
