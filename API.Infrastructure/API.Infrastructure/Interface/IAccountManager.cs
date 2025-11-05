using DAL.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Security.Claims;
namespace API.Infrastructure.Interface
{
    public interface IAccountManager:ITransientService
    {
        Task<List<IroleDTO>> GetRoles();
        Task<RoleAddDTO> GetRole(int id);
        Task<ResponseDTO> AddRole(RolesDTO addDTO, Claim claim);
        Task<List<MenuPermissionDTOAdd>> GetPermission();
        Task<List<MenuPermissionDTOAdd>> GetPermissionByRoleId(int roleId);
        Task<ResponseDTO> AddMenuPermission(List<RolePermissions> rolePermissions, UserClaim claim, int RoleId);
        Task<List<MenuPermissionDTOAdd>> GetMenus(string userId);   
    }
}
