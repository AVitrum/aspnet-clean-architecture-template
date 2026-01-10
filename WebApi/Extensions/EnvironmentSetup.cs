namespace WebApi.Extensions;

public static class EnvironmentSetup
{
    public static string ConfigureEnvironment(WebApplicationBuilder builder)
    {
        var defaultDirectory = "/app";

        if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
        {
            defaultDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..");
            DotNetEnv.Env.Load(Path.Combine(defaultDirectory, ".env.dev"));
        }

        builder.Configuration.AddEnvironmentVariables();
        return defaultDirectory;
    }
}
