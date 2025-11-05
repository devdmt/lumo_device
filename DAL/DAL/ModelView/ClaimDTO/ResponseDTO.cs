using DAL.ViewModels.ClaimDTO;
using System;
using System.Collections.Generic;
using DAL.Model.Safaricom;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using DAL.ModelView.Safaricom;

namespace DAL.ModelView.ClaimDTO
{
    

    public class ReportSettings
    {
        public string? ReportPath { get; set; }
        public string? RDLCFilePath { get; set; }
    }
    public class ShopDTO
    {
        public int Id { get; set; }
        public string? ShopName { get; set; }
        public string? Phonenumber { get; set; }
        public string? ShopLocation { get; set; }
        public string? County { get; set; }
        public string? Subcountry { get; set; }
        public string? Ward { get; set; }
        public string? Town { get; set; }
        public string? Address { get; set; }
        public string? SecondaryPhone { get; set; }
        public string? Email { get; set; }
        public string? ContactName { get; set; }
        public string? ShopOwner { get; set; }
        public string? iSActive { get; set; }
        public string? IsDeleted { get; set; }
        public int? loginType { get; set; }
        public int? shopType { get; set; }
        public string? Shortcode {  get; set; }
        public bool Active { get; set; } = true;
        

    }

    public class SafaricomResponse
    {
        public Safheader? header { get; set; }
        public SafDetailsShop? body { get; set; }  
    }
    public class Safheader
    {
        public string? requestRefId { get; set; }
        public string? responseCode { get; set; }
        public string? responseMessage { get; set; }
        public string? timestamp { get; set; }
    }

    
    public class SafDetailsShopId : SafDetailsShop
    {
        public long Id { get; set; }
    }
    public class SafDetailsShop
    {
        public string? name { get; set; }
        public string? category { get; set; }
        public string? msisdn { get; set; }
        public string? email { get; set; }
        public string? short_code { get; set; }
        public string? location { get; set; }
        public string? county { get; set; }
        public string? region { get; set; }
        public string? merchant_id { get; set; }
    }
    public class ShopFilters : DefaultFilter
    {
        public string? Phonenumber { get; set; }
        public string? Shopname { get; set; }
        public string? Shoplocation { get; set; }
        public string? ContactPerson { get; set; }
    }
       public class DefaultFilter
    {
  public int PageNumber { get;set;}   
        public int PageSize { get;set;}
        public string[]? DateRange { get; set; }
    }
    
    //public class ClaimsDTODetails : ClaimsDTO
    //{
    //    public string? imagePhoneUploadbase64 { get; set; }
    //    public string? policeAbstractUploadBase64 { get; set; }
    //    public string? imageIMEIUploadbase64 { get; set; }
    //     public string? deactivationProof { get;set;

          
    //    }
    //    //public string? deactivationProof { get

    //    //    {
    //    //        string proof=string.Empty;
    //    //        if (deactivationProofByte!=null && deactivationProofByte.Length > 0)
    //    //        {
    //    //            proof=  Encoding.ASCII.GetString(deactivationProofByte);
    //    //        }
    //    //        return proof;
    //    //    }
    //    //}
    //    //public byte[]? deactivationProofByte { get; set; }
    //}
    public class ClaimsDTO
    {
        public long? Id { get; set; }
        public string? PartnerID { get; set; }
        public string? ProductID { get; set; }
        public string? CustomerName { get; set; }
        public string? ClaimRefNumber { get; set; }
        public string? IDNumber { get; set; }
        public string? Narration { get; set; }
        public ClaimType? ClaimType { get; set; }
        public string? DamagePart { get; set; }
        public string? ReplacementCost { get; set; }
        public string? IncidentDate { get; set; }
        public string? ClaimDate { get; set; }
        public string? Abstract { get; set; }
        public string? Processed { get; set; }
        public string? CreatedOn { get; set; }
        public string? PartnerCode { get; set; }
        public string? RequestId { get; set; }
        public string? TrnId { get; set; }
        public string? UserId { get; set; }
        public string? medicalReportUpload { get; set; }
        public string? policeAbstractUpload { get; set; }
        public string? imagePhoneUpload { get; set; }
        public string? imageIMEIUpload { get; set; }
        public string? ResponseId { get; set; }
        public string? PhoneInsuranceCustomerId { get; set; }
        public ClaimStatus? claimStatus { get; set; }
        public string? PartId { get; set; }
        public string? IMEINumber { get; set; }
        public string? IMEINumber1 { get; set; }
        public string? IMEINumber2 { get; set; }
        public string? LabourCost { get; set; }
        public string? PartCost { get; set; }
        public string? Comments { get; set; }
        public string? PhoneId { get; set; }
        public string? ShopId { get; set; }
        public string? ShopType { get; set; }
        public string? Dispatched { get; set; }
        public string? DispatchedOn { get; set; }
        public string? NotificationNumber { get; set; }
        public string? DispatchedId { get; set; }
        public string? DispatchedShopId { get; set; }
        public string? AlternativeContact { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Approved { get; set; }
        public string? passedForProcessing { get; set; }
        public string? ShopName  { get; set; }  
            public string? ContactName { get; set; }

    }
     public class Devicesearch
    {
         public string? Idnumber { get; set; }
        public string? phonenumber { get; set; }    
        public string? phonemodel { get; set; }
        public string? Imeinumber { get; set; }   
        public long? customerId { get; set; }   
        public bool? isActive { get; set; }
          public int? Skip {  get; set; }
        public int? Take {  get; set; }
    }
      public class ApprovalDTO:SearchCriteria
    {
        public bool? AutoApproval { get; set; } = true;
    }
     public class SearchCriteria
    {
        public ClaimStatus? claimStatus {  get; set; }
        public int? shopId {  get; set; }
        public string? Idnumber { get; set; }
        public string? phonenumber { get; set; }
        public string? datefrom { get; set;}
        public string? dateto { get; set;}
         public bool? Approved { get; set; } 
        public string? ClaimRefNumber {  get; set; }        
        public string? IMEINumber {  get; set; }    
        public string? location  {  get; set; }  
        public string? model {  get; set; }  
        public string? parts {  get; set; }  
        public long? Id { get; set; }   
        public int? Skip {  get; set; }
        public int? Take {  get; set; }
       

    }
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum ClaimType
    //{
    //    [EnumMember(Value = "damage")] damage, [EnumMember(Value = "theft")] theft, [EnumMember(Value = "creditlife")] creditlife
    //}
    // [JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum ApprovalType
    //{
    //    [EnumMember(Value = "approved")] approved, [EnumMember(Value = "declined")] declined, [EnumMember(Value = "replace")] replace
    //}

    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum ClaimStatus
    //{
    //   [EnumMember(Value = "pending")] pending, [EnumMember(Value = "approved")] approved, [EnumMember(Value = "declined")] declined, [EnumMember(Value = "replace")] replace
    //}
     public enum actiontype
    {
        damageclaim, theftclaim, dispatch, collection, approveclaim
    }
  public class ActionsDTO
 {
     public string ActionName { get; set; }
     public string ActionDescription { get; set; }
     public string ShopId { get; set; }
     public string ShopType { get; set; }
     public ClaimType ClaimType { get; set; }
     public string IncidenceDate { get; set; }
     public  actiontype actiontype { get; set; }
     public string Reference { get; set; }
     public string RequestId { get; set; }
     public string userId { get; set; }
     public string userName { get; set; }

 }
    public enum ClaimRequestType
    {
        approval,claim, dispatch, collection
    }
    //public class ApproveClaimDTO
    //{
    //    public string? ClaimId { get; set; }
    //    public string? UserId { get; set; }
    //    public string? Browser { get; set; }
    //    public ClaimStatus action { get; set; } = ClaimStatus.approved;
    //    public string? Ip { get; set; }
    //    public string? comments { get; set; }
    //}
    public class PensionsContributions
    {
        public long Id { get; set; }
        public string? Names { get; set; }
        public string? Phonenumber { get; set; }
        public double? Amount { get; set; }
        public string? ResponseCode { get; set; }
        public string? paymentMode { get; set; }
        public string? TrnRefno { get; set; }
        public bool? RequestAcknowledged { get; set; }
        public string? PartnerResponseDesc { get; set; }
        public string? MpesaReceiptNumber { get; set; }

    }


}
