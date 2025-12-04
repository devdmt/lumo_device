using LumoDevice.Controllers;
using API.Infrastructure.Interface;
using DAL.ModelView.Safaricom;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Infrastructure.Application;
using Microsoft.Extensions.Options;
using API.Infrastructure.Auth;
using DAL.Core.Interface;
using DocumentFormat.OpenXml.Spreadsheet;
using FCB.Infrastructure.Caching;

namespace LumoDevice.API.Controllers.Safaricom
{
    public class ClaimController : ClaimApiController
    {
        readonly IClaimPortal _claim;   
        readonly Isettings _isettings;
        readonly SecuritySettings _security;
        readonly ICurrentUser _user;
       
       public ClaimController(IClaimPortal claimPortal,Isettings isettings,
           IOptions<SecuritySettings> _securityotpions,ICurrentUser user,IOptions<CacheSettings> cacheoptions) 
        {
        _claim = claimPortal;
            _security = _securityotpions.Value;
            _user=user;
          
        }   

        [HttpPost("Authenticate")]
        public async Task<IActionResult> Authenticate(ClaimAuth repairShopAUth)
        {

            try
            {
                var result = await _claim.ClaimUserAuth(repairShopAUth);

               return Ok(result);
               
            } catch(Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("MakeClaim")]
        public async Task<IActionResult> MakeClaim([FromBody] ClaimRequestDTOPortal request)
        {

            try
            {
                var claim =  _user.GetUserClaims();
                var result = await _claim.MakeClaim(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("GetSaveForLater/{Id}")]
        public async Task<IActionResult> GetSaveForLater(string Id)
        {

            try
            {
                var claim =  _user.GetUserClaims();
                var result = await _claim.QuerySaveForLaterClaim(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }

         [HttpPost("GetSaveForLater")]
        public async Task<IActionResult> GetSaveForLater([FromBody] ClaimSearchSaveForLaterDTO request)
        {

            try
            {
                var claim =  _user.GetUserClaims();
                var result = await _claim.QuerySaveForLaterClaim(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("SaveForLater")]
        public async Task<IActionResult> SaveForLater([FromBody] ClaimRequestDTOSaveForLater request)
        {

            try
            {
                    
                var result = await _claim.SaveForLaterClaim(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("QueryCustomer/{request}")]
        public async Task<IActionResult> QueryCustomer(string request)
        {

            try
            {
                var claim = _user.GetUserClaims();
                var result = await _claim.QueryCustomer(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("QueryClaims")]
        public async Task<IActionResult> QueryClaims([FromBody] ClaimSearchDTO request)
        {

            try
            {
              
                var result = await _claim.QueryClaim(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("UploadCreditLife")]
        public async Task<IActionResult> UploadCreditLife([FromBody] CreditLifeUpload request)
        {

            try
            {

                var result = await _claim.UploadCreditLife(request,request.UserId,request.Browser,request.Ip);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        } 
        [HttpPost("SendNotification")]
        public async Task<IActionResult> Dispatch([FromBody] SendNotificationRequest request)
        {

            try
            {

                var result = await _claim.SendNotification(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
          [HttpPost("resendDispatch")]
        public async Task<IActionResult> resendDispatch([FromBody] Approvedispatch request)
        {

            try
            {

                var result = await _claim.ResendDispatchCode(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
          [HttpPost("Dispatch")]
        public async Task<IActionResult> Dispatch([FromBody] Approvedispatch request)
        {

            try
            {

                var result = await _claim.AppproveDispatch(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
         [HttpPost("ApproveUpload")]
        public async Task<IActionResult> ApproveUpload([FromBody] CreditLifeApproveUpload request)
        {

            try
            {

                var result = await _claim.ApproveCreditLife(request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("ResendOtp/{userid}/{shoptype}")]
        public async Task<IActionResult> ResendOtp(string userid,int shoptype)
        {

            try
            {
                var result = await _claim.ResendOTP(userid,shoptype);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
         [HttpPost("ValidateDispatchCode")]
        public async Task<IActionResult> ValidateDispatchCode([FromBody] ValidateDispatchDTO validate)
        {
            try
            {
            var result = await _claim.ValidateDispatchCode(validate);
                return Ok(result);
            }catch(Exception ex) { }
            return BadRequest();
        }
        [HttpPost("ValidateOTP")]
        public async Task<IActionResult> ValidateOTP(VerifyOTP verifyOTP)
        {

            try
            {
                var result = await _claim.AuthVerifyOTP(verifyOTP);
                if (result.Success)
                {
                    var authClaims = new List<Claim>
                {
                    new Claim("phonenumber", result.Result.Phonenumber),
                    new Claim("userid",result.Result.Id.ToString()),
                    new Claim("name",result.Result.FullName??""),
                    new Claim("shopname",result.Result.ShopName),
                    new Claim("PartnerCode",result.Result.PartnerCode??""),
                    //new Claim("productCode",result.Result.ProductCode??""),
                    new Claim("shoptype",result.Result.shopType.ToString()??""),
                    new Claim("location",result.Result.Location??""),
                    new Claim("loginType",result.Result.loginType??""),
                    new Claim("shopId",result.Result.ShopId??"0"),
                   //new Claim(LumoClaims.PhoneNumber, result.Result.Phonenumber),
                   // new Claim(LumoClaims.UserId,result.Result.Id.ToString()),
                   // new Claim(LumoClaims.FullName,result.Result.ContactName??""),
                   // new Claim(LumoClaims.ShopName,result.Result.ShopName),
                   // new Claim(LumoClaims.PartnerCode,result.Result.PartnerCode??""),
                   // //new Claim("productCode",result.Result.ProductCode??""),
                   // new Claim(LumoClaims.ShopType,result.Result.shopType.ToString()??""),
                   // new Claim(LumoClaims.Location,result.Result.Location??""),
                   // new Claim(LumoClaims.LoginType,result.Result.loginType??""),
                    //new Claim("forcepasswordchange",user.ForcePasswordChange.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };
                    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_security.jwtSettings.key));

                    var token = new JwtSecurityToken(
                        issuer: _security.jwtSettings.ValidIssuer,
                        audience: _security.jwtSettings.ValidAudience,
                        expires: DateTime.Now.AddHours(3),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                        );

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token),
                        expiration = token.ValidTo
                    });
                }
                else
                {
                    return BadRequest(
                    result.ErrorMsg); 
                }
                
                
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
        [HttpPost("QueryImeiStatus")]
        public async Task<IActionResult> QueryImeiStatus(QueryImei queryImei)
        {
            if(ModelState.IsValid)
            {

            }
            return BadRequest("Please provide a valid imei and Id number");
        }

         [HttpPost("RecordPayments")]
        public async Task<IActionResult> RecordPayments([FromBody] RecordExcessPaymentDTO request)
        {

            try
            {
                var result = await _claim.RecordExcessPayment(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }

        [HttpPost("getDamagedParts")]
        public async Task<IActionResult> getDamagedParts([FromBody] PartsQuery parts)
        {

            try
            {
                var result = await _claim.getPartsCost(parts);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "Authenticate", RequestType.Error);
            }
            return BadRequest("Unable to authenticate you please try again");
        }
    }
}
