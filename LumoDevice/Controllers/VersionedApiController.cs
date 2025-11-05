using Microsoft.AspNetCore.Mvc.Versioning;
namespace LumoDevice.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
public class VersionedApiController : BaseApiController
{
}

[Route("api/saf/v{version:apiVersion}/[controller]")]
public class SafaricomApiController : BaseApiController
{
}
[Route("api/claim/v{version:apiVersion}/[controller]")]
public class ClaimApiController : BaseApiController
{

}
