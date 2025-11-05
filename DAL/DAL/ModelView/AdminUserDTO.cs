using DAL.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelView
{
     public class NewAdminUser
    {
       
        public int? Id { get; set; }
        public virtual string FirstName
        {
            get
            {
                string friendlyName = Name;

                if (!string.IsNullOrWhiteSpace(Name) && Name.Contains(" "))
                    friendlyName = Name.Split(" ")[0];

                return friendlyName;

            }
        }
        public virtual string LastName
        {
            get
            {
                string friendlyName = "";

                if (!string.IsNullOrWhiteSpace(Name) && Name.Contains(" "))
                    friendlyName = Name.Split(" ")[1];

                return friendlyName;
            }
        }
    
        public string UserName { get; set; }
        public string Name { get; set; }
        public string? Email { get; set; }
        public bool? IsEnabled { get; set; }
   
        public string? ErrorMsg { get; set; }
    
        public bool? Success { get; set; } = false;
        public string RoleId { get; set; }

    }
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class Roles :Auditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; } = false;
        
    }
    public class AdminUserDTO
    {
        public string UserName { get; set; }
        public string? Id { get; set; }
        public virtual string FirstName
        {
            get
            {
                string friendlyName = Name;

                if (!string.IsNullOrWhiteSpace(Name) && Name.Contains(" "))
                    friendlyName = Name.Split(" ")[0];

                return friendlyName;

            }
        }
        public virtual string LastName
        {
            get
            {
                string friendlyName = "";

                if (!string.IsNullOrWhiteSpace(Name) && Name.Contains(" "))
                    friendlyName = Name.Split(" ")[1];

                return friendlyName;
            }
        }

        public string Name { get; set; }
       
        public string? Email { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? approved { get; set; }=false;
        //public RoleType RoleTypes { get; set; }
        //public List<roleDTO> roles { get; set; }
        public bool? Success { get; set; }=false;
        [Required]
        public string? RoleId { get; set; }
        public string? RoleName { get; set; }
      
        public string? ErrorMsg { get; set; }
   
    }

    public class CreateOrUpdateRoleRequest
    {
        public string? Id { get; set; }
        public string roleId { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public List<string>? Permissions { get; set; } = default!;
    }
    public class RolesDTO
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; } = false;
        
    }
    public class RoleAddDTO 
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<RolePermissions>? Permissions { get; set; }
    }
    public class IroleDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
