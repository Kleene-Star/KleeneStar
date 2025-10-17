using KleeneStar.Abstractions;

namespace KleeneStar.Core.Services;

/// <summary>
/// Simple module loader implementation that manages module lifecycle.
/// </summary>
public class ModuleLoader : IModuleLoader
{
    private readonly List<IModule> _modules = new();
    private readonly ILogger<ModuleLoader> _logger;

    public ModuleLoader(ILogger<ModuleLoader> logger)
    {
        _logger = logger;
    }

    public IReadOnlyCollection<IModule> LoadedModules => _modules.AsReadOnly();

    public Task<IEnumerable<IModule>> LoadModulesAsync()
    {
        _logger.LogInformation("Loading modules...");

        // In a real implementation, this would scan directories, load assemblies,
        // and discover modules. For now, it returns the configured modules.
        _logger.LogInformation("Module loading completed. {Count} modules loaded.", _modules.Count);

        return Task.FromResult<IEnumerable<IModule>>(_modules);
    }

    /// <summary>
    /// Registers a module with the loader.
    /// </summary>
    public void RegisterModule(IModule module)
    {
        _modules.Add(module);
        _logger.LogInformation("Registered module: {ModuleName} v{Version}", module.Name, module.Version);
    }
}
