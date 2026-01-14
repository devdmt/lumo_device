using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace API.Infrastructure.Security;

/// <summary>
/// Middleware that intercepts requests to configured routes and decrypts encrypted payloads
/// </summary>
public class PayloadDecryptionMiddleware : IMiddleware
{
    private readonly IPayloadDecryptionService _decryptionService;
    private readonly PayloadDecryptionOptions _options;
    private readonly ILogger<PayloadDecryptionMiddleware> _logger;

    public PayloadDecryptionMiddleware(
        IPayloadDecryptionService decryptionService,
        IOptions<PayloadDecryptionOptions> options,
        ILogger<PayloadDecryptionMiddleware> logger)
    {
        _decryptionService = decryptionService;
        _options = options.Value;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Check if decryption is enabled
        if (!_options.Enabled)
        {
            await next(context);
            return;
        }

        // Check if current route requires decryption
        if (!ShouldDecryptRequest(context))
        {
            await next(context);
            return;
        }

        // Check if endpoint has [SkipPayloadDecryption] attribute
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<SkipPayloadDecryptionAttribute>() != null)
        {
            _logger.LogDebug("Skipping payload decryption for {Path} due to [SkipPayloadDecryption] attribute", context.Request.Path);
            await next(context);
            return;
        }

        try
        {
            await DecryptRequestPayloadAsync(context);
        }
        catch (SecurityException ex)
        {
            _logger.LogError(ex, "Security error during payload decryption for {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = "Payload decryption failed",
                message = ex.Message
            }));
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during payload decryption for {Path}", context.Request.Path);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                error = "Internal server error",
                message = "An unexpected error occurred during request processing"
            }));
            return;
        }

        await next(context);
    }

    /// <summary>
    /// Determines if the current request should be decrypted based on configured routes
    /// </summary>
    private bool ShouldDecryptRequest(HttpContext context)
    {
        var path = context.Request.Path.Value;
        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        var encryptedRoutes = _options.GetEncryptedRoutesArray();
        if (encryptedRoutes.Length == 0)
        {
            return false;
        }

        // Check if the request path matches any of the configured encrypted routes
        foreach (var route in encryptedRoutes)
        {
            if (path.StartsWith(route, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogDebug("Request path {Path} matches encrypted route pattern {Route}", path, route);
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Decrypts the request payload and replaces the request body with decrypted content
    /// </summary>
    private async Task DecryptRequestPayloadAsync(HttpContext context)
    {
        // Enable request body buffering to allow multiple reads
        context.Request.EnableBuffering();

        // Read the request body
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var requestBody = await reader.ReadToEndAsync();

        // Reset the stream position for potential re-reading
        context.Request.Body.Position = 0;

        if (string.IsNullOrWhiteSpace(requestBody))
        {
            _logger.LogDebug("Request body is empty, skipping decryption");
            return;
        }

        _logger.LogDebug("Processing encrypted payload for {Path}", context.Request.Path);

        // Parse the request to extract encrypted payload
        JObject? requestJson;
        try
        {
            requestJson = JObject.Parse(requestBody);
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse request body as JSON");
            throw new SecurityException("Invalid JSON format in request body");
        }

        // Look for 'encryptedPayload' field in the request
        var encryptedPayload = requestJson["encryptedPayload"]?.ToString();
        if (string.IsNullOrWhiteSpace(encryptedPayload))
        {
            _logger.LogWarning("No 'encryptedPayload' field found in request body for encrypted route {Path}", context.Request.Path);
            throw new SecurityException("Missing 'encryptedPayload' field in request body");
        }

        // For verification, we need the original payload (this should be sent separately or embedded)
        // According to the spec, originalPayload is provided alongside encryptedPayload
        var originalPayload = requestJson["payload"]?.ToString();
        if (string.IsNullOrWhiteSpace(originalPayload))
        {
            _logger.LogWarning("No 'payload' field found in request body for verification");
            throw new SecurityException("Missing 'payload' field in request body for verification");
        }

        // Decrypt and verify the payload
        var verifiedPayload = await _decryptionService.DecryptAndVerifyAsync(encryptedPayload, originalPayload);

        _logger.LogInformation("Payload decrypted and verified successfully for {Path}", context.Request.Path);

        // Replace the request body with the verified (original) payload
        // Parse it as the actual request DTO
        var decryptedBytes = Encoding.UTF8.GetBytes(verifiedPayload);
        context.Request.Body = new MemoryStream(decryptedBytes);
        context.Request.ContentLength = decryptedBytes.Length;

        _logger.LogDebug("Request body replaced with decrypted payload");
    }
}