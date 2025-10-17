using KleeneStar.Abstractions;
using KleeneStar.Core.Services;
using KleeneStar.Modules.Example;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// Register the module loader
builder.Services.AddSingleton<IModuleLoader>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<ModuleLoader>>();
    var loader = new ModuleLoader(logger);
    
    // Register example module
    loader.RegisterModule(new ExampleModule());
    
    return loader;
});

// Register the hosted service for module lifecycle management
builder.Services.AddHostedService<ModuleHostedService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Get module loader for API endpoints
var moduleLoader = app.Services.GetRequiredService<IModuleLoader>();

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

app.Run();
