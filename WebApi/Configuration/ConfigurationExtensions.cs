namespace WebApi.Configuration;

/// <summary>
/// Extension methods for IConfiguration to easily bind environment variables
/// </summary>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Gets the database connection string, prioritizing environment variables
    /// </summary>
    public static string GetDatabaseConnectionString(this IConfiguration configuration, string name = "DefaultConnection")
    {
        // First check for full connection string in env var
        var envConnectionString = Environment.GetEnvironmentVariable($"ConnectionStrings__{name}");
        if (!string.IsNullOrEmpty(envConnectionString))
            return envConnectionString;

        // Then check appsettings
        var configConnectionString = configuration.GetConnectionString(name);
        if (!string.IsNullOrEmpty(configConnectionString))
            return configConnectionString;

        // Build from individual env vars
        return EnvironmentConfiguration.Database.ConnectionString;
    }

    /// <summary>
    /// Gets a configuration value, prioritizing environment variables
    /// </summary>
    public static string GetValueOrEnv(this IConfiguration configuration, string configKey, string envKey, string defaultValue = "")
    {
        // Environment variable takes priority
        var envValue = Environment.GetEnvironmentVariable(envKey);
        if (!string.IsNullOrEmpty(envValue))
            return envValue;

        // Fallback to configuration
        return configuration[configKey] ?? defaultValue;
    }

    /// <summary>
    /// Loads environment variables from .env file if it exists.
    /// Call this at application startup before building configuration.
    /// </summary>
    public static void LoadEnvFile()
    {
        // Load .env file if it exists (for local development)
        var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
        if (File.Exists(envFilePath))
        {
            LoadEnvFileFromPath(envFilePath);
        }

        // Also check parent directory (for when running from WebApi folder)
        var parentEnvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
        if (File.Exists(parentEnvPath))
        {
            LoadEnvFileFromPath(parentEnvPath);
        }
    }

    /// <summary>
    /// Loads environment variables from a specific .env file path
    /// </summary>
    private static void LoadEnvFileFromPath(string path)
    {
        if (!File.Exists(path))
            return;

        foreach (var line in File.ReadAllLines(path))
        {
            // Skip empty lines and comments
            var trimmedLine = line.Trim();
            if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith('#'))
                continue;

            var separatorIndex = trimmedLine.IndexOf('=');
            if (separatorIndex <= 0)
                continue;

            var key = trimmedLine[..separatorIndex].Trim();
            var value = trimmedLine[(separatorIndex + 1)..].Trim();

            // Remove quotes if present
            if ((value.StartsWith('"') && value.EndsWith('"')) ||
                (value.StartsWith('\'') && value.EndsWith('\'')))
            {
                value = value[1..^1];
            }

            // Only set if not already set (environment takes priority)
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
            {
                Environment.SetEnvironmentVariable(key, value);
            }
        }
    }
}

