using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API.Infrastructure.Security;

internal static class Startup
{
    /// <summary>
    /// Registers payload decryption services
    /// </summary>
    internal static IServiceCollection AddPayloadDecryption(this IServiceCollection services, IConfiguration config)
    {
        // Bind configuration from security.json and environment variables
        services.Configure<PayloadDecryptionOptions>(config.GetSection(nameof(PayloadDecryptionOptions)));

        // Register the decryption service
        services.AddScoped<IPayloadDecryptionService, PayloadDecryptionService>();

        // Register the middleware
        services.AddScoped<PayloadDecryptionMiddleware>();

        return services;
    }

    /// <summary>
    /// Applies payload decryption middleware to the application pipeline
    /// Only processes requests to routes configured in PayloadDecryptionOptions.EncryptedRoutes
    /// </summary>
    internal static IApplicationBuilder UsePayloadDecryption(this IApplicationBuilder app, IConfiguration config)
    {
        // Get configuration to check if enabled
        var options = config.GetSection(nameof(PayloadDecryptionOptions)).Get<PayloadDecryptionOptions>();

        if (options?.Enabled == true)
        {
            app.UseMiddleware<PayloadDecryptionMiddleware>();
        }

        return app;
    }
}