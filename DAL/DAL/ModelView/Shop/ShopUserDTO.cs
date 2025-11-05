using DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ModelView.Shop
{
    public class ShopUserDTO
    {
       public string Id { get; set; }
        public string? FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? ShopId { get; set; }
        public bool? IsEnabled { get; set; }
        public bool? IsActive { get; set; }
        public string? IpAddress { get; set; }
        public string? HostUrl { get; set; }
        public string? HostPort { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        public int? LoginType { get; set; }
        public string? ShortCode { get; set; }
        public string? MerchantId { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public bool? EmailConfirmed { get; set; }
        public string? NormalizedEmail { get; set; }
        public string? SecurityStamp { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? PhoneNumberConfirmed { get; set; }
        public bool? TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool? LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public string? ShopName { get; set; }
        public string? ShopNumber { get; set; }
        public int? PartnerId { get; set; }
        public string? ShopLocation { get; set; }
        public string? County { get; set; }
        public string? Ward { get; set; }
        public string? ContactName { get; set; }
        public string? ShopEmails { get; set; }
        public string? SecondaryPhone { get; set; }
        public string? ShopOwner { get; set; }
        public bool? ShopActive { get; set; }
        public string? ShopMerchant { get; set; }
        public int? ShopType { get; set; }
        public string? ShopShortCode { get; set; }
        public bool? Linked { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? Subcountry { get; set; }
        public bool? ShopDeleted { get; set; }
    }
}
