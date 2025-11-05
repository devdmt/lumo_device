
using DAL.ModelView;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Application.Accounts
{
    internal partial class AccountManager
    {
        public async Task<List<MenuPermissionDTOAdd>> GetPermission()
        {
            var permissions = new List<MenuPermissionDTOAdd>();
            return permissions;
        }
        public async Task<List<MenuPermissionDTOAdd>> GetPermissionByRoleId(int roleId)
        {
            var permissions = new List<MenuPermissionDTOAdd>();
            return permissions;
        }
       public async Task<ResponseDTO> AddMenuPermission(List<RolePermissions> rolePermissions, 
           UserClaim claim, int RoleId)
        {
            var response = new ResponseDTO();
            return response;
        }
    }
}
