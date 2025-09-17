using MyProject.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(MyProject.Application.DependencyInjection).Assembly));

var defaultDirectory = EnvironmentSetup.ConfigureEnvironment(builder);
LoggingConfiguration.ConfigureLogging(builder, defaultDirectory);

builder.Services.AddServiceExtensions(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

await app.MigrateDatabaseAsync();

app.ConfigurePipeline();
app.MapApplicationEndpoints();

app.Run();