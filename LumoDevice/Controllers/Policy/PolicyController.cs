using API.Infrastructure.Interface;
using DAL.ViewModels.ClaimDTO;
using LumoDevice.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace LumoDevice.API.Controllers.Policy
{
    public class PolicyController : VersionedApiController
    {
        private readonly IClaimManager _claimManager;
        private readonly Isettings _settings;
        readonly IPhoneInsurance _phone;
        public PolicyController(IClaimManager claimManager, Isettings settings,IPhoneInsurance phone)
        {
            _claimManager = claimManager;
            _phone = phone;
            _settings = settings;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> UploadPolicies([FromBody] PolicyUploadRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid request payload.");
            }

            try
            {
                var result = await _claimManager.UploadPolicyPurchasesAsync(request);
                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }
            catch (Exception ex)
            {
                _settings.LogRequests(ex.Message, "PolicyUpload", RequestType.Error);
                return BadRequest("Unable to process the upload at this time.");
            }
        }
        [HttpPost("Submit/{refno}")]
        public async Task<IActionResult> SubmitUpload(string refno)
        {
            var result = await _phone.submitUploads(refno);
            return Ok(result);
        }
    }
}
