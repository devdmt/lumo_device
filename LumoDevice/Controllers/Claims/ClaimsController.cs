using API.Infrastructure.Interface;
using DAL;
using DAL.Core.Interface;
using DAL.Model.ClaimDTO;
using DAL.ModelView.ClaimDTO;
using DocumentFormat.OpenXml.Spreadsheet;
using LumoDevice.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SanlamAdmin.Controllers.Safaricom
{
    public class ClaimsController : VersionedApiController
    {
        readonly ILogger<ClaimsController> _logger;
        readonly IClaimManager _safclaims;
          readonly ICurrentUser _user;
       public ClaimsController(ILogger<ClaimsController> logger,IClaimManager safclaims,ICurrentUser user) 
        {
        _logger = logger;
            _user = user;   
            _safclaims = safclaims;
        }

        [HttpPost("GetClaims")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllClaims([FromBody] SearchCriteria searchCriteria)
        {
            var result=await _safclaims.GetAllClaims(searchCriteria);
            return Ok(result);
        } 
        [HttpPost("GetClaim/{id}")]
       // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetClaim(long id)
        {
            var result=await _safclaims.GetClaim(id);
            return Ok(result);
        }
         [HttpPost("GetCustomers")]
       // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomers([FromBody] Devicesearch searchCriteria)
        {
            var result=await _safclaims.GetCustomers(searchCriteria);
            return Ok(result);
        }
         [HttpPost("GetDevices")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetDevices([FromBody] Devicesearch searchCriteria)
        {
            var result=await _safclaims.GetDevices(searchCriteria);
            return Ok(result);
        }
        //[HttpPost("GetPhoneModels")]
        ////[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> GetPhoneModels()
        //{
        //    var result=await _safclaims.GetDeviceModel();
        //    return Ok(result);
        //}
        // [HttpPost("GetCustomerDevices")]
        //public async Task<IActionResult> GetCustomerDevices([FromBody] Customersearch searchCriteria)
        //{
        //    var result=await _safclaims.GetCustomers(searchCriteria);
        //    return Ok(result);
        //}
        [HttpPost("GetCustomerClaims")]
       // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetCustomerClaims([FromBody] SearchCriteria searchCriteria)
        {
            var result=await _safclaims.GetAllClaims(searchCriteria);
            return Ok(result);
        }
     
         [HttpPost("GetAutoApprovedClaims")]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAutoApprovedClaims([FromBody] ApprovalDTO searchCriteria)
        {
            var result=await _safclaims.GetAutoApproved(searchCriteria);
            return Ok(result);
        }
        //[HttpPost("DownloadClaims")]
        ////[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> DownloadClaims([FromBody] ReportDTO searchCriteria)
        //{
        //    var result=await _safclaims.ClaimReport(searchCriteria);
        //    return Ok(result);
        //}
        //[HttpPost("UploadShops/{logintype}/{shoptype}")]
        //[AllowAnonymous]
        //[Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> UploadShops([FromBody]FileUploadRequest uploadRequest,int shoptype,int logintype)
        //{
        //    var userclaim = Utili.getCurrentuser(this.User);
        //    var result= await _safclaims.UploadShops(uploadRequest,shoptype,logintype,userclaim);
        //    return Ok(result);
        //}
       // [HttpPost("CreateUpdateShop")]
       //// [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
       // public async Task<IActionResult> CreateUpdateShop([FromBody]ShopDTO request)
       // {
       //     var userclaim = Utilities.getCurrentuser(this.User);
       //     var result= await _safclaims.CreateShop(request,userclaim);
       //     return Ok(result);
       // }
       // [HttpPost("DeleteShop")]
       //// [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
       // public async Task<IActionResult> DeleteShop(string id)
       // {
       //     var claim= Utilities.getCurrentuser(this.User);
       //     var result= await _safclaims.DeleteShop(id, claim);
       //     return Ok(result);  
       // }
         //[HttpPost("getShops")]
       // [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        //public async Task<IActionResult> getShops([FromBody]ShopFilters request)
        //{
        //   // var userclaim = Utilities.getCurrentuser(this.User);
        //    var claim = _user.GetUserClaims();
        //    var result= await _safclaims.getShops(request);
        //    return Ok(result);
        //}
    }
}
