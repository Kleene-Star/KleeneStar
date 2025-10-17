# Module Development Guide

This guide walks you through creating a module for the KleeneStar system.

## Table of Contents
- [Prerequisites](#prerequisites)
- [Creating Your First Module](#creating-your-first-module)
- [Module Structure](#module-structure)
- [Module Lifecycle](#module-lifecycle)
- [Best Practices](#best-practices)
- [Advanced Topics](#advanced-topics)

## Prerequisites

- .NET 9.0 SDK or later
- Basic understanding of C# and async/await
- Familiarity with dependency injection

## Creating Your First Module

### Step 1: Create a Class Library

```bash
# From the repository root
cd src
dotnet new classlib -n KleeneStar.Modules.MyModule
```

### Step 2: Add Reference to Abstractions

```bash
cd KleeneStar.Modules.MyModule
dotnet add reference ../KleeneStar.Abstractions/KleeneStar.Abstractions.csproj
```

### Step 3: Implement IModule Interface

Create a class that implements `IModule`:

```csharp
using KleeneStar.Abstractions;
using Microsoft.Extensions.Logging;

namespace KleeneStar.Modules.MyModule;

public class MyModule : IModule
{
    private ILogger<MyModule>? _logger;

    public string Name => "My Module";
    
    public string Version => "1.0.0";
    
    public string Description => "A custom module for KleeneStar";

    public Task InitializeAsync(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetService(typeof(ILogger<MyModule>)) as ILogger<MyModule>;
        _logger?.LogInformation("MyModule initialized successfully");
        
        // Initialize your module here
        // Register services, set up connections, etc.
        
        return Task.CompletedTask;
    }

    public Task ShutdownAsync()
    {
        _logger?.LogInformation("MyModule shutting down");
        
        // Cleanup resources here
        // Close connections, dispose objects, etc.
        
        return Task.CompletedTask;
    }
}
```

### Step 4: Add Required Dependencies

If your module needs logging:

```bash
dotnet add package Microsoft.Extensions.Logging.Abstractions
```

### Step 5: Register Your Module

Add a reference to your module in `KleeneStar.Core`:

```bash
cd ../KleeneStar.Core
dotnet add reference ../KleeneStar.Modules.MyModule/KleeneStar.Modules.MyModule.csproj
```

Update `Program.cs` to register your module:

```csharp
using KleeneStar.Modules.MyModule;

// ... existing code ...

if (moduleLoader is ModuleLoader loader)
{
    loader.RegisterModule(new MyModule());
}
```

### Step 6: Build and Run

```bash
# From repository root
dotnet build
cd src/KleeneStar.Core
dotnet run
```

## Module Structure

A typical module should have:

```
KleeneStar.Modules.MyModule/
├── MyModule.cs              # Main module class implementing IModule
├── Services/                # Module services
│   └── MyService.cs
├── Models/                  # Module data models
│   └── MyModel.cs
├── Configuration/           # Module configuration
│   └── MyModuleOptions.cs
└── KleeneStar.Modules.MyModule.csproj
```

## Module Lifecycle

### Initialization (InitializeAsync)

This is where you:
- Retrieve services from the DI container
- Set up logging
- Initialize connections (databases, APIs, etc.)
- Register module-specific services
- Configure module behavior

```csharp
public async Task InitializeAsync(IServiceProvider serviceProvider)
{
    _logger = serviceProvider.GetService<ILogger<MyModule>>();
    _config = serviceProvider.GetService<IConfiguration>();
    
    // Initialize database connection
    await InitializeDatabaseAsync();
    
    // Register background services
    RegisterBackgroundServices(serviceProvider);
}
```

### Running Phase

Your module is active and can:
- Respond to requests
- Process events
- Execute background tasks
- Interact with other services

### Shutdown (ShutdownAsync)

This is where you:
- Close connections
- Dispose resources
- Save state if needed
- Cancel ongoing operations

```csharp
public async Task ShutdownAsync()
{
    _logger?.LogInformation("Shutting down module");
    
    // Cancel background operations
    await _cancellationTokenSource?.CancelAsync();
    
    // Close database connection
    await _dbConnection?.CloseAsync();
    
    // Dispose resources
    _disposableResource?.Dispose();
}
```

## Best Practices

### 1. Use Dependency Injection

Get services from the service provider:

```csharp
public Task InitializeAsync(IServiceProvider serviceProvider)
{
    _logger = serviceProvider.GetRequiredService<ILogger<MyModule>>();
    _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
    return Task.CompletedTask;
}
```

### 2. Handle Errors Gracefully

```csharp
public async Task InitializeAsync(IServiceProvider serviceProvider)
{
    try
    {
        await InitializeResourcesAsync();
    }
    catch (Exception ex)
    {
        _logger?.LogError(ex, "Failed to initialize module");
        throw; // Re-throw to signal initialization failure
    }
}
```

### 3. Use Async/Await Properly

```csharp
// Good: Use async for I/O operations
public async Task InitializeAsync(IServiceProvider serviceProvider)
{
    await _database.ConnectAsync();
    await LoadConfigurationAsync();
}

// For synchronous initialization:
public Task InitializeAsync(IServiceProvider serviceProvider)
{
    // Synchronous initialization
    return Task.CompletedTask;
}
```

### 4. Implement Proper Logging

```csharp
_logger?.LogInformation("Module {ModuleName} v{Version} initializing", Name, Version);
_logger?.LogDebug("Configuration: {Config}", configValue);
_logger?.LogWarning("Deprecated feature in use");
_logger?.LogError(ex, "Failed to process request");
```

### 5. Clean Up Resources

```csharp
public async Task ShutdownAsync()
{
    // Dispose disposable resources
    _httpClient?.Dispose();
    
    // Close connections
    await _databaseConnection?.CloseAsync();
    
    // Cancel background tasks
    _cancellationTokenSource?.Cancel();
}
```

## Advanced Topics

### Working with Configuration

```csharp
public class MyModule : IModule
{
    private IConfiguration? _configuration;
    
    public Task InitializeAsync(IServiceProvider serviceProvider)
    {
        _configuration = serviceProvider.GetService<IConfiguration>();
        
        var mySettings = _configuration?
            .GetSection("MyModule")
            .Get<MyModuleSettings>();
            
        return Task.CompletedTask;
    }
}
```

### Adding HTTP Endpoints

```csharp
public Task InitializeAsync(IServiceProvider serviceProvider)
{
    var app = serviceProvider.GetService<WebApplication>();
    
    app?.MapGet("/mymodule/status", () => new
    {
        Status = "Running",
        Version = Version
    });
    
    return Task.CompletedTask;
}
```

### Registering Services

```csharp
public Task InitializeAsync(IServiceProvider serviceProvider)
{
    // Note: Service registration should ideally happen before
    // the app is built. This is a simplified example.
    
    var services = serviceProvider.GetService<IServiceCollection>();
    services?.AddTransient<IMyService, MyService>();
    
    return Task.CompletedTask;
}
```

### Background Tasks

```csharp
public class MyModule : IModule
{
    private CancellationTokenSource? _cts;
    private Task? _backgroundTask;
    
    public Task InitializeAsync(IServiceProvider serviceProvider)
    {
        _cts = new CancellationTokenSource();
        _backgroundTask = Task.Run(() => BackgroundWorkerAsync(_cts.Token));
        return Task.CompletedTask;
    }
    
    private async Task BackgroundWorkerAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // Do background work
            await Task.Delay(1000, cancellationToken);
        }
    }
    
    public async Task ShutdownAsync()
    {
        _cts?.Cancel();
        
        if (_backgroundTask != null)
        {
            await _backgroundTask;
        }
        
        _cts?.Dispose();
    }
}
```

### Module Dependencies

If your module depends on another module:

```csharp
public Task InitializeAsync(IServiceProvider serviceProvider)
{
    var moduleLoader = serviceProvider.GetRequiredService<IModuleLoader>();
    
    var dependencyModule = moduleLoader.LoadedModules
        .FirstOrDefault(m => m.Name == "Dependency Module");
        
    if (dependencyModule == null)
    {
        throw new InvalidOperationException("Required module not loaded");
    }
    
    return Task.CompletedTask;
}
```

## Testing Your Module

Create a test project:

```bash
cd tests
dotnet new xunit -n KleeneStar.Modules.MyModule.Tests
cd KleeneStar.Modules.MyModule.Tests
dotnet add reference ../../src/KleeneStar.Modules.MyModule/KleeneStar.Modules.MyModule.csproj
```

Example test:

```csharp
using Xunit;
using Moq;
using Microsoft.Extensions.DependencyInjection;

public class MyModuleTests
{
    [Fact]
    public async Task InitializeAsync_ShouldSucceed()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();
        var module = new MyModule();
        
        // Act
        await module.InitializeAsync(serviceProvider);
        
        // Assert
        Assert.Equal("My Module", module.Name);
    }
}
```

## Next Steps

- Explore the [Example Module](../../src/KleeneStar.Modules.Example) for reference
- Read the [Architecture documentation](ARCHITECTURE.md)
- Check out [Contributing Guidelines](../CONTRIBUTING.md)
- Join the community discussions
