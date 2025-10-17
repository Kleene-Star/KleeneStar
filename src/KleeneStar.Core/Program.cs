using KleeneStar.Abstractions;
using KleeneStar.Core.Services;
using KleeneStar.Modules.Example;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Register the module loader
builder.Services.AddSingleton<IModuleLoader, ModuleLoader>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Initialize modules
var moduleLoader = app.Services.GetRequiredService<IModuleLoader>();

// Register example module
if (moduleLoader is ModuleLoader loader)
{
    loader.RegisterModule(new ExampleModule());
}

var modules = await moduleLoader.LoadModulesAsync();

foreach (var module in modules)
{
    await module.InitializeAsync(app.Services);
    app.Logger.LogInformation("Initialized module: {ModuleName} v{Version}", module.Name, module.Version);
}

// API endpoints
app.MapGet("/", () => new
{
    Name = "KleeneStar",
    Version = "1.0.0",
    Description = "Central hub for the modular KleeneStar system",
    LoadedModules = moduleLoader.LoadedModules.Select(m => new
    {
        m.Name,
        m.Version,
        m.Description
    })
})
.WithName("GetSystemInfo")
.WithTags("System");

app.MapGet("/modules", () => moduleLoader.LoadedModules.Select(m => new
{
    m.Name,
    m.Version,
    m.Description
}))
.WithName("GetModules")
.WithTags("Modules");

// Handle application shutdown
var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
lifetime.ApplicationStopping.Register(() =>
{
    foreach (var module in moduleLoader.LoadedModules)
    {
        // Call ShutdownAsync synchronously in the shutdown handler
        // In a production system, consider using IHostedService for proper async shutdown
        module.ShutdownAsync().GetAwaiter().GetResult();
        app.Logger.LogInformation("Shut down module: {ModuleName}", module.Name);
    }
});

app.Run();
