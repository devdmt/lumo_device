using DAL.ModelView;
using System.ComponentModel.DataAnnotations;

namespace DAL.Model
{
    public class OnboardingRequests
    {
        public required string Id { get; set; }
        public required string TransactionId { get; set; }
        public virtual Partners Partner { get; set; }
        public int PartnerId { get; set; }
        public virtual PartnersProducts Product { get; set; }
        public int? ProductId { get; set; }
        public string? CustomerName { get; set; }
        public string? DateOfBirth { get; set; }
        public string? IDNumber { get; set; } //Reg No
        public Gender? Gender { get; set; }
        public double? Premium { get; set; }
        public benefitOption? BenefitOption { get; set; }
        public string? BeneficiaryName { get; set; } //Institution Name
        public string? RegNumber { get; set; } //Registration Number
        public string? BeneficiaryMobileNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public Boolean Processed { get; set; }
        public string Status { get; set; }
    }
}