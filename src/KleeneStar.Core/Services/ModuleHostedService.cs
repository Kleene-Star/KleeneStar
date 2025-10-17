using KleeneStar.Abstractions;

namespace KleeneStar.Core.Services;

/// <summary>
/// Hosted service that manages module lifecycle during application startup and shutdown.
/// </summary>
public class ModuleHostedService : IHostedService
{
    private readonly IModuleLoader _moduleLoader;
    private readonly ILogger<ModuleHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public ModuleHostedService(
        IModuleLoader moduleLoader,
        ILogger<ModuleHostedService> logger,
        IServiceProvider serviceProvider)
    {
        _moduleLoader = moduleLoader;
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting module hosted service");

        var modules = await _moduleLoader.LoadModulesAsync();

        foreach (var module in modules)
        {
            await module.InitializeAsync(_serviceProvider);
            _logger.LogInformation("Initialized module: {ModuleName} v{Version}", module.Name, module.Version);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping module hosted service");

        foreach (var module in _moduleLoader.LoadedModules)
        {
            await module.ShutdownAsync();
            _logger.LogInformation("Shut down module: {ModuleName}", module.Name);
        }
    }
}
