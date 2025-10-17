namespace KleeneStar.Abstractions;

/// <summary>
/// Defines the interface for a KleeneStar module.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the version of the module.
    /// </summary>
    string Version { get; }

    /// <summary>
    /// Gets the description of the module.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Initializes the module.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    Task InitializeAsync(IServiceProvider serviceProvider);

    /// <summary>
    /// Shuts down the module.
    /// </summary>
    Task ShutdownAsync();
}
