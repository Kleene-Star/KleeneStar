using KleeneStar.Abstractions;
using Microsoft.Extensions.Logging;

namespace KleeneStar.Modules.Example;

/// <summary>
/// An example module demonstrating the KleeneStar module system.
/// </summary>
public class ExampleModule : IModule
{
    private ILogger? _logger;

    public string Name => "Example Module";

    public string Version => "1.0.0";

    public string Description => "An example module demonstrating the KleeneStar modular architecture";

    public Task InitializeAsync(IServiceProvider serviceProvider)
    {
        _logger = serviceProvider.GetService(typeof(ILogger<ExampleModule>)) as ILogger<ExampleModule>;
        _logger?.LogInformation("Example module initialized");
        return Task.CompletedTask;
    }

    public Task ShutdownAsync()
    {
        _logger?.LogInformation("Example module shutting down");
        return Task.CompletedTask;
    }
}
