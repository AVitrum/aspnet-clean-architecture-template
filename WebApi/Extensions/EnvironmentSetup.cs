namespace WebApi.Extensions;

public static class EnvironmentSetup
{
    public static string GetBaseDirectory()
    {
        var isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
        return isRunningInContainer 
            ? "/app" 
            : Path.Combine(Directory.GetCurrentDirectory(), "..");
    }
}
