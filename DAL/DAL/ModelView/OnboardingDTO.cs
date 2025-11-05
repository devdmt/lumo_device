using DAL.Model;
using System.ComponentModel.DataAnnotations;

namespace DAL.ModelView
{
    public class OnboardingDTO
    {
        public string? PartnerCode { get; set; }
        public int? ProductId { get; set; }
        public string? CustomerName { get; set; }
        //Format dd/mm/yyyy
        [MaxLength(10)]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Date of birth should be in the format of dd/mm/yyyy")]
        public string? DateOfBirth { get; set; }
        [MinLength(6, ErrorMessage = "ID Number must be at least 6 characters long.")]
        [MaxLength(9, ErrorMessage = "ID Number must be a maximum of 9 characters long.")]
        public string? IDNumber { get; set; } //Reg No
        public Gender? Gender { get; set; }
        public double? Premium { get; set; }
        public benefitOption? BenefitOption { get; set; }
        public string? BeneficiaryName { get; set; } //Institution Name
        public string? RegNumber { get; set; } //Registration Number
        //[MinLength(9, ErrorMessage = "The Beneficiary Mobile Number must be at least 9 characters long.")]
        //[MaxLength(13, ErrorMessage = "The Beneficiary Mobile Number must be at most 13 characters long.")]
        public string? BeneficiaryMobileNumber { get; set; }
    }
}
