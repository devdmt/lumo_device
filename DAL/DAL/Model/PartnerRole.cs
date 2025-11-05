using DAL.Common.Contract;
using DAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DAL.Model
{
  
public class PartnerRole : AuditableEntity, IAggregateRoot
{
    public string ProfileName { get; set; } = string.Empty;
    public string ProfileDescription { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    
    // Navigation properties
    public virtual ICollection<PartnerRoleModule> Modules { get; set; } = new List<PartnerRoleModule>();
    }

public class Module : AuditableEntity, IAggregateRoot
{
    public string ModuleName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public int SortOrder { get; set; } = 0;
    
    // Navigation properties
    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();
    public virtual ICollection<PartnerRoleModule> PartnerRoleModules { get; set; } = new List<PartnerRoleModule>();
    }

public class Permission : AuditableEntity, IAggregateRoot
{
    public string PermissionName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public int SortOrder { get; set; } = 0;
    
    // Foreign key
    public Guid ModuleId { get; set; }
    
    // Navigation properties
    public virtual Module Module { get; set; } = null!;
    public virtual ICollection<PartnerRolePermission> PartnerRolePermissions { get; set; } = new List<PartnerRolePermission>();
}

    // Junction table for PartnerRole and Module (many-to-many)
public class PartnerRoleModule : AuditableEntity
{
    public Guid PartnerRoleId { get; set; }
    public Guid ModuleId { get; set; }
    
    // Navigation properties
    public virtual PartnerRole PartnerRole { get; set; } = null!;
    public virtual Module Module { get; set; } = null!;
    public virtual ICollection<PartnerRolePermission> Permissions { get; set; } = new List<PartnerRolePermission>();
}

    // Junction table for PartnerRoleModule and Permission (many-to-many)
public class PartnerRolePermission : AuditableEntity
{
    public Guid PartnerRoleModuleId { get; set; }
    public Guid PermissionId { get; set; }
    
    // Navigation properties
    public virtual PartnerRoleModule PartnerRoleModule { get; set; } = null!;
    public virtual Permission Permission { get; set; } = null!;
}

}
