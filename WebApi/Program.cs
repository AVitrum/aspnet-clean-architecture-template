using WebApi.Configuration;
using WebApi.Extensions;

WebApi.Configuration.ConfigurationExtensions.LoadEnvFile();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

var defaultDirectory = EnvironmentSetup.GetBaseDirectory();
LoggingConfiguration.ConfigureLogging(builder, defaultDirectory);

if (!builder.Environment.IsDevelopment())
{
    EnvironmentConfiguration.ValidateConfiguration(requireSecureDefaults: true);
}

builder.Services.AddOpenApi();
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    EnvironmentConfiguration.LogConfiguration(logger);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.MigrateDatabaseAsync();

app.ConfigurePipeline();
app.MapApplicationEndpoints();

app.Run();