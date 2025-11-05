using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models.Interfaces;
namespace DAL.Model
{
    public class PartnerUsers : IdentityUser, IAuditableEntity
    {
       
        public string? FullName { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(50)]
        public string? Configuration { get; set; }
        public virtual Shops Shop { get; set; }
        public int ShopId { get; set; }
       
        public bool IsEnabled { get; set; }
        public bool? ISActive { get; set; }
        public bool? IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;
        [MaxLength(50)]
        public string? IpAddress { get; set; }
        [MaxLength(50)]
        public string? HostUrl { get; set; }
        [MaxLength(50)]
        public string? HostPort { get; set; }
        [MaxLength(50)]
        public string? CreatedBy { get; set; }
        [MaxLength(50)]
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
       public LoginType? loginType { get; set; } = LoginType.Admin;
        public bool Linked { get; set; }
        public string? Short_code { get; set; } 
        public string? Merchant_id { get; set; }

    }
    public class Shops
    {
      public int Id  {get;set;}
      public string? ShopName  {get;set;}
      public virtual Partners Partners { get; set; }
      public int? PartnerId { get; set; }
      public string? Phonenumber  {get;set;}
      public string? ShopLocation  {get;set;}
      public string? County  {get;set;}
      public string? Subcountry  {get;set;}
      public string? Ward  {get;set;}
      public string? Town  {get;set;}
      public string? Address  {get;set;}
      public string? SecondaryPhone  {get;set;}
      public string? Email  {get;set;}
      public string? ContactName  {get;set;}
      public string? ShopOwner  {get;set;}
      public string? iSActive  {get;set;}
      public string? CreatedBy  {get;set;}
      public string? CreatedName  {get;set;}
      public string? UpdatedBy  {get;set;}
      public string? UpdatedName  {get;set;}
      public string? CreatedDate  {get;set;}
      public string? UpdatedDate  {get;set;}
      public string? IsDeleted  {get;set;}
      public string? DeletedOn  {get;set;}
      public string? DeletedById  {get;set;}
      public string? DeletedByUser  {get;set;}
      public ShopType? shopType  {get;set;}
      public string? Short_code  {get;set;}
      public string? Merchant_id  {get;set;}
      public string? SafaricomResponse  {get;set;}
      public string? Linked  {get;set;}
    }
    public class PartnerAdminUser : IdentityUser, IAuditableEntity
    {
        public string FullName { get; set; }
        [MaxLength(50)]
        public string? FirstName { get; set; }
        [MaxLength(50)]
        public string? LastName { get; set; }
        [MaxLength(50)]
        public string? Configuration { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;
        [MaxLength(50)]
        public string? IpAddress { get; set; }
        [MaxLength(50)]
        public string? HostUrl { get; set; }
        [MaxLength(50)]
        public string? HostPort { get; set; }
        [MaxLength(50)]
        public string? CreatedBy { get; set; }
        [MaxLength(50)]
        public string? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public LoginType loginType { get; set; } = LoginType.Admin;
    }
}
