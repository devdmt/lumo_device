using Microsoft.AspNetCore.Mvc;
using LumoDevice.Controllers;
using DAL.ModelView;
using API.Infrastructure.Interface;
using DAL.Core.Interface;
namespace LumoDevice.API.Controllers
{
    public class AccountController : VersionedApiController
    {
         readonly IClaimPortal _claim;
        readonly IAccountManager _account;
        readonly ICurrentUser _current;
        public AccountController(IClaimPortal claim, IAccountManager accountManager,ICurrentUser current)
        {
            _current = current;
            _claim = claim;
            _account = accountManager;
        }




        //[HttpGet("GetRoles")]
        //public async Task<IActionResult> GetRoles()
        //{
        //    var roles = await _account.GetAdminRoles();
        //    return Ok(roles);
        //}

        //[HttpGet("GetRole/{Id}")]
        //public async Task<IActionResult> GetRole(int Id)
        //{
        //    var roles = await _account.GetRole(Id);
        //    return Ok(roles);
        //}

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] RolesDTO roleAdd)
        {
            var claim = _current.GetUserClaims()?.FirstOrDefault();
            var roles = await _account.AddRole(roleAdd, claim);
            return Ok(roles);
        }
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetPermissions()
        {
            var permision = await _account.GetPermission();
            return Ok(permision);
        }

        //[HttpGet("GetAllPermissions")]
        //public async Task<IActionResult> GetPermissions()
        //{
        //    var permision = await _account.GetPermission();
        //    return Ok(permision);
        //}

        //[HttpGet("GetAllPermissionsByRoleId/{roleId:int}")]
        //public async Task<IActionResult> GetPermissions([FromRoute] int roleId)
        //{
        //    var permision = await _account.GetPermissionByRoleId(roleId);
        //    return Ok(permision);
        //}

        //[HttpPost("AddRolesandPermissions/{RoleId}")]
        //public async Task<IActionResult> AddRolesandPermissions([FromBody] List<RolePermissions> rolePermissions, int RoleId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var claims = Utilities.getCurrentuser(this.User);
        //        var result = await _account.AddMenuPermission(rolePermissions,claims, RoleId);
        //        return Ok(result);
        //    }
        //    return BadRequest();
        //}


        //[HttpPost("GetMenus/{userId}")]
        //public async Task<IActionResult> GetMenus( string userId)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var claims = this.getCurrentuser(this.User);
        //        //if(claims.userid != userId)
        //        //{
        //        //    return Unauthorized();  
        //        //}
        //        var result = await _account.GetMenus(userId);
        //        return Ok(result);

        //    }
        //    return BadRequest();
        //}
    }
}
