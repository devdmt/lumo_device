namespace API.Infrastructure.Security;

/// <summary>
/// Attribute to mark controller actions that should skip payload decryption
/// even if they match the configured encrypted routes
/// </summary>
/// <example>
/// <code>
/// [HttpPost("health")]
/// [SkipPayloadDecryption]
/// public IActionResult HealthCheck()
/// {
///     return Ok("Service is healthy");
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class SkipPayloadDecryptionAttribute : Attribute
{
}