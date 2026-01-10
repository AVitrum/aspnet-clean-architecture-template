using MyProject.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

var defaultDirectory = EnvironmentSetup.ConfigureEnvironment(builder);
LoggingConfiguration.ConfigureLogging(builder, defaultDirectory);

builder.Services.AddOpenApi();
builder.Services.AddWebApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.MigrateDatabaseAsync();

app.ConfigurePipeline();
app.MapApplicationEndpoints();

app.Run();