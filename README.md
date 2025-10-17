# KleeneStar

KleeneStar is the central hub for the development and deployment of the modular KleeneStar system. It provides a unified platform for contributors and developers to build, integrate, and deploy modular applications.

## Overview

KleeneStar is built on ASP.NET Core 9.0 and provides a modular architecture that allows developers to:
- Create independent, reusable modules
- Integrate modules seamlessly into the core system
- Deploy modular applications efficiently
- Manage module lifecycles (initialization, shutdown)

## Architecture

The system consists of three main components:

### 1. **KleeneStar.Core**
The core web server that hosts the modular system. It provides:
- RESTful API endpoints for system information and module management
- Module lifecycle management (initialization and shutdown)
- Service dependency injection
- OpenAPI documentation support

### 2. **KleeneStar.Abstractions**
The abstractions library that defines the module interface and contracts:
- `IModule`: Interface that all modules must implement
- `IModuleLoader`: Interface for module loading and management

### 3. **KleeneStar.Modules.Example**
An example module demonstrating how to create modules for the KleeneStar system.

## Getting Started

### Prerequisites
- .NET 9.0 SDK or later

### Building the Project

```bash
# Clone the repository
git clone https://github.com/Kleene-Star/kleenestar.git
cd kleenestar

# Restore dependencies and build
dotnet build
```

### Running the Application

```bash
# Navigate to the Core project
cd src/KleeneStar.Core

# Run the application
dotnet run
```

The application will start on `http://localhost:5208` by default.

### API Endpoints

#### GET /
Returns system information and list of loaded modules.

**Response:**
```json
{
  "name": "KleeneStar",
  "version": "1.0.0",
  "description": "Central hub for the modular KleeneStar system",
  "loadedModules": [
    {
      "name": "Example Module",
      "version": "1.0.0",
      "description": "An example module demonstrating the KleeneStar modular architecture"
    }
  ]
}
```

#### GET /modules
Returns a list of all loaded modules.

**Response:**
```json
[
  {
    "name": "Example Module",
    "version": "1.0.0",
    "description": "An example module demonstrating the KleeneStar modular architecture"
  }
]
```

#### GET /openapi/v1.json (Development only)
Returns OpenAPI specification for the API.

## Creating a Module

To create a new module:

1. **Create a new class library project:**
   ```bash
   dotnet new classlib -n KleeneStar.Modules.YourModule
   ```

2. **Add reference to KleeneStar.Abstractions:**
   ```bash
   dotnet add reference ../KleeneStar.Abstractions/KleeneStar.Abstractions.csproj
   ```

3. **Implement the IModule interface:**
   ```csharp
   using KleeneStar.Abstractions;
   
   namespace KleeneStar.Modules.YourModule;
   
   public class YourModule : IModule
   {
       public string Name => "Your Module Name";
       public string Version => "1.0.0";
       public string Description => "Description of your module";
       
       public Task InitializeAsync(IServiceProvider serviceProvider)
       {
           // Initialize your module
           return Task.CompletedTask;
       }
       
       public Task ShutdownAsync()
       {
           // Cleanup on shutdown
           return Task.CompletedTask;
       }
   }
   ```

4. **Register your module in Program.cs:**
   ```csharp
   if (moduleLoader is ModuleLoader loader)
   {
       loader.RegisterModule(new YourModule());
   }
   ```

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

### Development Guidelines
- Follow the existing code style and conventions
- Add XML documentation comments to public APIs
- Test your changes before submitting
- Keep modules independent and loosely coupled

## Project Structure

```
kleenestar/
├── src/
│   ├── KleeneStar.Core/              # Core web server
│   ├── KleeneStar.Abstractions/      # Module abstractions
│   └── KleeneStar.Modules.Example/   # Example module
├── tests/                             # Test projects
├── KleeneStar.sln                    # Solution file
└── README.md                          # This file
```

## License

See the [LICENSE](LICENSE) file for details.

## Future Enhancements

- Dynamic module loading from directories
- Module dependency management
- Hot reload support for modules
- Module configuration management
- Health check integration
- Metrics and monitoring support