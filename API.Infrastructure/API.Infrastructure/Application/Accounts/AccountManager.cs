using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Infrastructure.Interface;
using DAL.ModelView;
using System.Security.Claims;
namespace API.Infrastructure.Application.Accounts
{
    internal partial class AccountManager : IAccountManager
    {
        private readonly ApplicationDbContext _db;

        public AccountManager(ApplicationDbContext dbContext)
        {
            _db = dbContext;
        }
        public async  Task<List<IroleDTO>> GetRoles()
        {
            var roles= new List<IroleDTO>();
            return roles;
        }
       public async Task<RoleAddDTO> GetRole(int id)
        {
            var role = new RoleAddDTO();
            return role;
        }
         public async  Task<ResponseDTO>  AddRole(RolesDTO roles, Claim claim)
        {
            var role = new ResponseDTO();

            return role;
        }
        public async Task<List<MenuPermissionDTOAdd>> GetMenus(string userId)
        {
            var menus = new List<MenuPermissionDTOAdd>();
            return menus;
        }
    }
}
