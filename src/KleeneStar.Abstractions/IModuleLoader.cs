namespace KleeneStar.Abstractions;

/// <summary>
/// Defines the interface for the module loader.
/// </summary>
public interface IModuleLoader
{
    /// <summary>
    /// Loads all available modules.
    /// </summary>
    /// <returns>A collection of loaded modules.</returns>
    Task<IEnumerable<IModule>> LoadModulesAsync();

    /// <summary>
    /// Gets all loaded modules.
    /// </summary>
    IReadOnlyCollection<IModule> LoadedModules { get; }
}
