namespace WebApi.Configuration;

/// <summary>
/// Centralized environment configuration with validation.
/// Reads from environment variables with fallback to appsettings.json
/// </summary>
public static class EnvironmentConfiguration
{
    /// <summary>
    /// Database configuration from environment variables
    /// </summary>
    public static class Database
    {
        public static string Host => GetRequired("DB_HOST", "localhost");
        public static string Port => GetRequired("DB_PORT", "5432");
        public static string Name => GetRequired("DB_NAME", "cleanarchdb");
        public static string User => GetRequired("DB_USER", "postgres");
        public static string Password => GetRequired("DB_PASSWORD", "postgres");

        public static string ConnectionString =>
            Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
            ?? $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password}";
    }

    /// <summary>
    /// JWT authentication configuration from environment variables
    /// </summary>
    public static class Jwt
    {
        public static string Key => GetRequired("JWT_KEY", "YourSuperSecretKeyThatIsAtLeast32CharactersLong!");
        public static string Issuer => GetRequired("JWT_ISSUER", "CleanArchitecture");
        public static string Audience => GetRequired("JWT_AUDIENCE", "CleanArchitectureUsers");
        public static int ExpiryMinutes => int.TryParse(GetOptional("JWT_EXPIRY_MINUTES"), out var minutes) ? minutes : 60;
    }

    /// <summary>
    /// Logging configuration from environment variables
    /// </summary>
    public static class Logging
    {
        public static string Level => GetOptional("LOG_LEVEL") ?? "Information";
        public static string Path => GetOptional("LOG_PATH") ?? "logs";
    }

    /// <summary>
    /// Application configuration from environment variables
    /// </summary>
    public static class Application
    {
        public static string EnvironmentName => GetOptional("ASPNETCORE_ENVIRONMENT") ?? "Production";
        public static bool IsDevelopment => EnvironmentName.Equals("Development", StringComparison.OrdinalIgnoreCase);
        public static bool IsProduction => EnvironmentName.Equals("Production", StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets a required environment variable with a fallback default value
    /// </summary>
    private static string GetRequired(string key, string defaultValue)
    {
        return Environment.GetEnvironmentVariable(key) ?? defaultValue;
    }

    /// <summary>
    /// Gets an optional environment variable, returns null if not set
    /// </summary>
    private static string? GetOptional(string key)
    {
        return Environment.GetEnvironmentVariable(key);
    }

    /// <summary>
    /// Validates that all critical environment variables are set.
    /// Call this at application startup to fail fast if configuration is missing.
    /// </summary>
    public static void ValidateConfiguration(bool requireSecureDefaults = false)
    {
        var errors = new List<string>();

        // In production, ensure secure defaults are not used
        if (requireSecureDefaults || Application.IsProduction)
        {
            if (Jwt.Key.Contains("YourSuperSecret") || Jwt.Key.Contains("YourDevelopment"))
            {
                errors.Add("JWT_KEY: Using default/insecure key in production is not allowed");
            }

            if (Database.Password == "postgres")
            {
                errors.Add("DB_PASSWORD: Using default password in production is not allowed");
            }
        }

        // Validate JWT key length
        if (Jwt.Key.Length < 32)
        {
            errors.Add("JWT_KEY: Must be at least 32 characters long");
        }

        if (errors.Count > 0)
        {
            throw new InvalidOperationException(
                $"Environment configuration validation failed:\n{string.Join("\n", errors.Select(e => $"  - {e}"))}");
        }
    }

    /// <summary>
    /// Logs the current configuration (masks sensitive values)
    /// </summary>
    public static void LogConfiguration(ILogger logger)
    {
        logger.LogInformation("=== Environment Configuration ===");
        logger.LogInformation("Environment: {Environment}", Application.EnvironmentName);
        logger.LogInformation("Database Host: {Host}", Database.Host);
        logger.LogInformation("Database Port: {Port}", Database.Port);
        logger.LogInformation("Database Name: {Name}", Database.Name);
        logger.LogInformation("Database User: {User}", Database.User);
        logger.LogInformation("Database Password: {Password}", MaskValue(Database.Password));
        logger.LogInformation("JWT Issuer: {Issuer}", Jwt.Issuer);
        logger.LogInformation("JWT Audience: {Audience}", Jwt.Audience);
        logger.LogInformation("JWT Key: {Key}", MaskValue(Jwt.Key));
        logger.LogInformation("JWT Expiry: {Expiry} minutes", Jwt.ExpiryMinutes);
        logger.LogInformation("Log Level: {Level}", Logging.Level);
        logger.LogInformation("Log Path: {Path}", Logging.Path);
        logger.LogInformation("=================================");
    }

    private static string MaskValue(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= 4)
            return "****";
        return value[..4] + new string('*', Math.Min(value.Length - 4, 20));
    }
}

