using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using DAL.ModelView.Safaricom;

namespace DAL.ViewModels.ClaimDTO
{
    public class PolicyDTOs
    {
        [JsonPropertyName("phonenumber")]
        public string? PhoneNumber { get; set; }

        [JsonPropertyName("customerName")]
        public string? CustomerName { get; set; }

        [JsonPropertyName("secondaryContactName")]
        public string? SecondaryContactName { get; set; }

        [JsonPropertyName("secondaryContact")]
        public string? SecondaryContact { get; set; }

        [JsonPropertyName("idnumber")]
        public string? IdNumber { get; set; }

        [JsonPropertyName("dateOfBirth")]
        public string? DateOfBirth { get; set; }

        [JsonPropertyName("phoneModel")]
        public string? PhoneModel { get; set; }

        [JsonPropertyName("imeiNumber")]
        public string? ImeiNumber { get; set; }

        [JsonPropertyName("serialNumber")]
        public string? SerialNumber { get; set; }

        [JsonPropertyName("phoneCost")]
        public string? PhoneCost { get; set; }

        [JsonPropertyName("mpesaRef")]
        public string? MpesaRef { get; set; }

        [JsonPropertyName("modeOfPurchase")]
        public ModeOfPurchase? ModeOfPurchase { get; set; } = global::DAL.ViewModels.ClaimDTO.ModeOfPurchase.cash;
       
        [JsonPropertyName("loanRefNumber")]
        public string? LoanRefNumber { get; set; }

        [JsonPropertyName("repaymentTerms")]
        public string? RepaymentTerms { get; set; }

        [JsonPropertyName("loanAmount")]
        public decimal? LoanAmount { get; set; }

        [JsonPropertyName("interestRate")]
        public decimal? InterestRate { get; set; }

        [JsonPropertyName("premiumPaid")]
        public decimal? PremiumPaid { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ModeOfPurchase
    {
        [EnumMember(Value = "cash")] cash,
        [EnumMember(Value = "credit")] credit
    }

    public class PolicyUploadRequest
    {
        public FileDetails fileDetails { get; set; } = default!;
        public string? UploadedBy { get; set; }
    }

    public class PolicyUploadResult
    {
        public string UploadReference { get; set; } = string.Empty;
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public bool HasFailures => FailureCount > 0;
        public string? FailureReport { get; set; }
    }
}

