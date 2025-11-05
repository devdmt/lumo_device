// ====================================================

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.AspNetCore.Identity;
using DAL.Model;
using DAL.Models.Interfaces;
using DAL.Model.Safaricom;
namespace DAL
{
    public class SingleResponse
    {
        public string id { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    // IdentityDbContext<TenziAdminUsers, TenziApplicationRole, string>
    public class ApplicationDbContext  : IdentityDbContext<PartnerUsers, PartnersRole, string>

    {
       
        public string CurrentUserId { get; set; }

        public IDbConnection Connection => Database.GetDbConnection();
        public DbSet<PartnerUsers> user { get; set; }
        public DbSet<Shops> shops { get; set; }
        public DbSet<Partners> Partners { get; set; }
        public DbSet<PartnersProducts> partnersProducts { get; set; }
        public DbSet<MsureRequests> msureRequests { get; set; }
        public DbSet<PhoneInsuranceRequest> phoneInsuranceRequest { get; set; }
        public DbSet<Notifications> notifications { get; set; }
        public DbSet<PartsCosts> partsCosts { get; set; }
        public DbSet<PortalActions> portalActions { get; set; }
        public DbSet<ClaimRequest> claimRequests { get; set; }
        public DbSet<PhoneInsuranceCustomers> phoneInsuranceCustomers { get; set; }
        public DbSet<OnboardingRequests> OnboardingRequests { get; set; }
        public DbSet<PartnerAdminUser> PartnerAdminUsers { get; set; }
           public DbSet<PartnerRole> PartnerRoles { get; set; }
            public DbSet<Module> Modules { get; set; }
            public DbSet<Permission> Permissions { get; set; }
            public DbSet<PartnerRoleModule> PartnerRoleModules { get; set; }
            public DbSet<PartnerRolePermission> PartnerRolePermissions { get; set; }
       
        //public DbSet<LabourCost> LabourCosts { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            const string priceDecimalType = "decimal(18,2)";
            const string idDecimalType = "decimal(18,0)";
            // builder.Entity<TransactionsUploadTemp>().Property(e => e.id).ValueGeneratedOnAdd();
            //  builder.Entity<TransactionsUploadTemp>().Property(e => e.Key).ValueGeneratedNever();
            builder.Entity<PartnersRole>().ToTable("Roles");
            builder.Entity<Shops>().ToTable("Shops");
            builder.Entity<PartnerAdminUser>().ToTable("PartnerAdminUser");
            builder.Entity<Partners>().ToTable("DevicePartners");
            builder.Entity<Partners>().HasKey(a=>a.Id);
               builder.Entity<PartnerUsers>().ToTable("ShopUsers");
            builder.Entity<PartnerUsers>().HasKey(a=>a.Id);
            builder.Entity<PartnersProducts>().ToTable("partnersProducts");
            builder.Entity<MsureRequests>().HasKey(a=>a.Id);
            builder.Entity<MsureRequests>().ToTable("MsureRequests");
            builder.Entity<PartsCosts>().ToTable("PartCosts");
            //builder.Entity<LabourCost>().ToTable("LabourCost");
           // builder.Entity<RepairShops>().ToTable("RepairShops");
            builder.Entity<Notifications>().ToTable("Notifications");
            builder.Entity<PhoneInsuranceRequest>().ToTable("PhoneInsuranceRequest");
            builder.Entity<PortalActions>().ToTable("PortalActions");
            builder.Entity<OnboardingRequests>().ToTable("OnboardingRequests").HasKey(a => a.Id);

             builder.Entity<PartnerRole>().ToTable("PartnerRoles");
                builder.Entity<Module>().ToTable("Modules");
                builder.Entity<Permission>().ToTable("Permissions");
                builder.Entity<PartnerRoleModule>().ToTable("PartnerRoleModules");
                builder.Entity<PartnerRolePermission>().ToTable("PartnerRolePermissions");

                // Configure relationships
                builder.Entity<Permission>()
                    .HasOne(p => p.Module)
                    .WithMany(m => m.Permissions)
                    .HasForeignKey(p => p.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<PartnerRoleModule>()
                    .HasOne(prm => prm.PartnerRole)
                    .WithMany(pr => pr.Modules)
                    .HasForeignKey(prm => prm.PartnerRoleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<PartnerRoleModule>()
                    .HasOne(prm => prm.Module)
                    .WithMany(m => m.PartnerRoleModules)
                    .HasForeignKey(prm => prm.ModuleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<PartnerRolePermission>()
                    .HasOne(prp => prp.PartnerRoleModule)
                    .WithMany(prm => prm.Permissions)
                    .HasForeignKey(prp => prp.PartnerRoleModuleId)
                    .OnDelete(DeleteBehavior.Cascade);

                builder.Entity<PartnerRolePermission>()
                    .HasOne(prp => prp.Permission)
                    .WithMany(p => p.PartnerRolePermissions)
                    .HasForeignKey(prp => prp.PermissionId)
                    .OnDelete(DeleteBehavior.Cascade);



        }


        public override int SaveChanges()
        {
            UpdateAuditEntities();
            return base.SaveChanges();
        }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateAuditEntities();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        //{
        //    UpdateAuditEntities();
        //    return base.SaveChangesAsync(cancellationToken);
        //}


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            UpdateAuditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }


        private void UpdateAuditEntities()
        {
            try
            {
                var modifiedEntries = ChangeTracker.Entries()
                .Where(x => x.Entity is IAuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));


            foreach (var entry in modifiedEntries)
            {
                var entity = (IAuditableEntity)entry.Entity;
                DateTime now = DateTime.UtcNow;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedDate = now;
                    entity.CreatedBy = CurrentUserId;
                }
                else
                {
                    base.Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                    base.Entry(entity).Property(x => x.CreatedDate).IsModified = false;
                }

                entity.UpdatedDate = now;
                entity.UpdatedBy = CurrentUserId;

            } 
            
            }catch(Exception ex)
            {

            }
        }
    }
}
