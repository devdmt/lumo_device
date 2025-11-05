using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelView
{
    public class MenuPermissionDTOAdd
    {
        public int Id { get; set; }        
        public string? Route { get; set; }
        public string? PageName { get; set; }
        public string? PageIcon { get; set; }
         public bool Enabled { get; set; }        
        public string? Permission { get; set; }
       // public List<MenuPermissions>? Permissions { get; set; }
        public List<string>? Permissions { get; set; }
        public List<PermissionChildDTO> child { get; set; }
    }
    public class PermissionChildDTO
    {
        public int Id { get; set; }
        public string? Route { get; set; }
        public string? PageName { get; set; }
        public string? PageIcon { get; set; }
        public string? Permission { get; set; }
       // public List<MenuPermissions>? Permissions { get; set; }
        public List<string>? Permissions { get; set; }
        public bool Enabled { get; set; }
    }
    public class RolePermissions
    {
        
        public int MainMenuId { get; set; }
      
        public List<ChildRolePermissionDTO>? ChildMenu { get; set; }  
        //public string? Permission { get; set; }
        public List<string>? Permission { get; set; }

    }
   
    public class MenuPermissions 
    {
        public int Id { get; set; }
        public string? Permission { get; set; }
    }

    public class ChildRolePermissionDTO
    {
        public int Id { get; set; }
        //public string? Permission { get; set;}
        public List<string>? Permission { get; set;}
    }
}
