
using DAL.ModelView.Safaricom;
using LumoDevice.Controllers;
using API.Infrastructure.Interface;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using API.Infrastructure.Auth;
using DAL.ModelView;
using Microsoft.AspNetCore.Authorization;

namespace LumoDevice.API.Controllers.Safaricom
{
    // [Route("api/[controller]")]
    //[ApiController]
    [Authorize]
    public class PhoneInsuranceController : SafaricomApiController
    {
        readonly IPhoneInsurance _phoneInsurance;
        readonly Isettings _settings;
        readonly IPartnerManager _partnerManager;
            public PhoneInsuranceController(IPhoneInsurance phoneInsurance, Isettings settings,IPartnerManager partnerManager)
        {
            _phoneInsurance = phoneInsurance;
            _partnerManager = partnerManager;
            _settings = settings;
        }
        [HttpPost("customeronboarding")]
        [AllowAnonymous]
        public async Task<IActionResult> Onboarding([FromBody] PhoneInsuranceRequest phoneInsurance)
        {
            try
            {
                var response= await _phoneInsurance.PurchaseInsurance(phoneInsurance);
                return Ok(response);

            } catch (Exception ex) { 
            _settings.LogRequests(ex.Message, "Onboarding",RequestType.Error,JsonConvert.SerializeObject(phoneInsurance));
            }
            return BadRequest("Unable to process your request");
        }
        [HttpPost("MakeClaim")]
        public async Task<IActionResult> MakeClaim([FromBody] ClaimRequestDTO claimRequest)
        {
            try
            {
                var response = await _phoneInsurance.SubmitClaim(claimRequest);
                return Ok(response);
            }
            catch (Exception ex) {
             _settings.LogRequests(ex.Message, "MakeClaim",RequestType.Error,JsonConvert.SerializeObject(claimRequest));
            
            }
            return BadRequest("Unable to process your request");
        }
          [HttpPost("ReplaceRequest")]
        public async Task<IActionResult> ReplaceRequest([FromBody] ReplaceRequestDeviceDTO claimRequest)
        {
            try
            {
                var response = await _phoneInsurance.ReplaceClaimRequest(claimRequest);
                return Ok(response);
            }
            catch (Exception ex) {
             _settings.LogRequests(ex.Message, "MakeClaim",RequestType.Error,JsonConvert.SerializeObject(claimRequest));
            
            }
            return BadRequest("Unable to process your request");
        }
        [HttpPost("GetClaims/{searchParam}")]
        public async Task<IActionResult> GetClaims(string searchParam)
        {
            try
            {
                var response = await _phoneInsurance.GetCustomerClaims(searchParam);
                return Ok(response);
            }
            catch (Exception ex)
            {
                  _settings.LogRequests(ex.Message, "GetClaims",RequestType.Error,searchParam);

            }
            return BadRequest("Unable to process your request");
        }

        [HttpPost("Auth")]
        [AllowAnonymous]
        public async Task<IActionResult> AUth([FromBody] UserLoginDTO login)
        {
            var token = await _partnerManager.AuthenticatePartner(login);
            return Ok(token);
        }


    }
    public class PostRequest
    {
        //public int UserId { get; set; }
        //public string Description { get; set; }
        public IFormFile? Image { get; set; }
        //[JsonIgnore(Condition = JsonIgnoreCondition.Always)]
        //public string? ImagePath { get; set; }
    }


}
