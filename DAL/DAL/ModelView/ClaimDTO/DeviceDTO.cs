using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.ClaimDTO
{
    public class DeviceModels
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public bool Active { get; set; }
    }
    public class DeviceDTO
    {
        public string? CustomerName { get; set; }
        public string? IDNumber { get; set; }
        public string? Id { get; set; }
        public string? PartnerID { get; set; }
        public string? ProductID { get; set; }
        public string? DateofBirth { get; set; }
        public string? PhoneModel { get; set; }
        public string? RequestId { get; set; }
        public string? IMEINumber { get; set; }
        public string? PhoneCost { get; set; }
        public string? ModeOfPurchase { get; set; }
        public string? LoanRefNumber { get; set; }
        public string? RepaymentTerms { get; set; }
        public string? LoanAmount { get; set; }
        public string? InterestRate { get; set; }
        public string? PremiumPaid { get; set; }
        public string? PurchaseDate { get; set; }
        public string? Processed { get; set; }
        public string? RequestedOn { get; set; }
        public string? PolicyStatus { get; set; }
        public string? PhoneInsuranceCustomerId { get; set; }
        public string? IMEINumber1 { get; set; }
        public string? IMEINumber2 { get; set; }
        public string? SecondaryContactName { get; set; }
        public string? SecondaryContactPhone { get; set; }
        public string? Active { get; set; }
        public string? PhoneName { get; set; }
    }
}
