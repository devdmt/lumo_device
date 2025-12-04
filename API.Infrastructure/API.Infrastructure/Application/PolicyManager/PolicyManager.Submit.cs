using API.Infrastructure.Interface;
using DAL.ModelView.Safaricom;
using DAL.ModelView;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Application.PolicyManager
{
    internal partial class PolicyManager
    {

        //public async Task<OnboardingResponseDTO> PurchaseInsurance(PhoneInsuranceRequest request)
        //{

        //    var response = new OnboardingResponseDTO();
        //    try
        //    {
        //        // _isettings.LogRequests(JsonConvert.SerializeObject(request),"PurchaseInsurance",RequestType.Comparison);

        //        if(request == null)
        //        {
        //            response.ErrorMsg = "invalid request, please  check the data";
        //            response.Success = false;
                    
        //            return response;
        //        }

        //        if(request.IMEINumber == null)
        //        {
        //            response.ErrorMsg = "Invalid IMEI, please  check the data";
        //            response.Success = false;

        //            return response;
        //        }
        //        if (string.IsNullOrEmpty(request.CustomerName))
        //        {
        //            response.ErrorMsg = "Invalid customer name, please  check the data";
        //            response.Success = false;

        //            return response;
        //        }
        //        if (string.IsNullOrEmpty(request.SerialNumber))
        //        {
        //            response.ErrorMsg = "Invalid serial number";
        //            response.Success = false;

        //            return response;
        //        }
        //        if (Convert.ToDouble(request.PhoneCost)==0)
        //        {
        //            response.ErrorMsg = "Invalid Phone cost";
        //            response.Success = false;

        //            return response;
        //        }
        //        if(request.ModeOfPurchase==null)
        //        {
        //            response.ErrorMsg = "Invalid Mode of Purchase";
        //            response.Success = false;

        //            return response;
        //        }
        //        if (string.IsNullOrEmpty(request.PhoneModel))
        //        {
        //            response.ErrorMsg = "Invalid Phone Model";
        //            response.Success = false;

        //            return response;
        //        }

        //        int partnerId = (int)_db.Connection.ExecuteScalar("select Id as partnerId from  [dbo].[Partners] where [PartnerCode]='" + request.PartnerCode + "'");
        //        if (partnerId == 0)
        //        {
        //            response.ErrorMsg = "Invalid Partner Code";
        //            response.Success = false;
        //            response.ResponseId = request.RequestId ?? "";
        //            return response;
        //        }
        //        int transactionId = (int)_db.Connection.ExecuteScalar(" select count(1) from [dbo].[PhoneInsuranceRequest] where RequestId='" + request.RequestId + "' and PartnerID='"+ partnerId +"'");
        //        if (transactionId > 0)
        //        {
        //            response.ErrorMsg = "Request already exists";
        //            response.Success = false;
        //            response.ResponseId = request.RequestId ?? "";
        //            return response;
        //        }
        //  string? LoanRefNumber = request.LoanPurchase?.LoanRefNumber;
        //  string? RepaymentTerms = request.LoanPurchase?.RepaymentTerms;
        //  double? LoanAmount  = request.LoanPurchase?.LoanAmount;
        //  double? InterestRate = request.LoanPurchase?.InterestRate;
        //   double? PremiumPaid= request.LoanPurchase?.PremiumPaid;
        //        var customer_Id = _db.Connection.ExecuteScalar("select isnull(Id,0) as customerId from  [dbo].[phoneInsuranceCustomers] where [IdNumber]='" + request.Idnumber + "' and " +
        //            "right(PhoneNumber,9)=right('"+ request.Phonenumber +"',9)")??0;
        //        if (Convert.ToInt64(customer_Id)==0)
        //        {
        //            var custdetails = new PhoneCustomerDTO()
        //            {
        //                CreatedBy = "API",
        //                 CustomerName = request.CustomerName,
        //                  IdNumber = request.Idnumber,
        //                SecondaryContact = request.SecondaryContact,
        //                SecondaryContactName = request.SecondaryContactName,
        //                     PhoneNumber= request.Phonenumber,
        //                      dateOfBirth= request.dateOfBirth

        //            };
        //            customer_Id = await AddCustomers(custdetails);
        //        }
        //        string ImeiNumber="",ImeiNumber1 = "",ImeiNumber2 = "";
        //        if (request.IMEINumber.Count > 0)
        //        {
        //            ImeiNumber= request.IMEINumber[0];
        //            ImeiNumber1= request.IMEINumber.Count>1? request.IMEINumber[1]:"";
        //            ImeiNumber2 = request.IMEINumber.Count > 2?request.IMEINumber[2]:"";
        //        }
        //        string Id= Guid.NewGuid().ToString();
        //        //string query = "INSERT INTO [dbo].[PhoneInsuranceRequest] ([Id] ,[PartnerID], [ProductID] ,[CustomerName]  ,[PhoneModel] ,[RequestId] ,[IMEINumber] ,[PhoneCost] ,[ModeOfPurchase] ,[LoanRefNumber]," +
        //        //    "[RepaymentTerms],[LoanAmount] ,[InterestRate] ,[PremiumPaid] ,[PurchaseDate] ,[Processed] ,[RequestedOn] ,[PolicyStatus]," +
        //        //    "PhoneInsuranceCustomerId,IMEINumber1,IMEINumber2,SecondaryContactName,SecondaryContactPhone,repaymentPeriod)VALUES ('" + Id + "','" + partnerId + "','"+ request.ProductID +"','"+ request.CustomerName +"','" +
        //        //    request.PhoneModel  + "','" + request.RequestId  +"','" + ImeiNumber  +"','"+ request.PhoneCost +"','"+ (int)request.ModeOfPurchase +"'," +
        //        //    "'"+ LoanRefNumber + "','"+ RepaymentTerms + "','"+ LoanAmount + "','"+ InterestRate + "','"+ PremiumPaid + "'," +
        //        //    "'"+ request.PurchaseDate + "','0',getdate(),'"+ (int)request.PolicyStatus + "',"+ customer_Id + ",'"+ ImeiNumber1 + "','"+ ImeiNumber2 + "','"+ request.SecondaryContact + "'," +
        //        //    "'"+ request.SecondaryContact + "','12',)";

        //        var param = new DynamicParameters();
        //        param.Add("@Id", Id);
        //        param.Add("@PartnerID", partnerId);
        //        param.Add("@ProductID", request.ProductID);
        //        param.Add("@CustomerName", request.CustomerName);
        //        param.Add("@PhoneModel", request.PhoneModel);
        //        param.Add("@RequestId", request.RequestId);
        //        param.Add("@IMEINumber", ImeiNumber);
        //        param.Add("@PhoneCost", request.PhoneCost??"");
        //        param.Add("@ModeOfPurchase", request.ModeOfPurchase);
        //        param.Add("@LoanRefNumber", LoanRefNumber??"");
        //        param.Add("@RepaymentTerms", RepaymentTerms??"");
        //        param.Add("@LoanAmount", LoanAmount ?? 0);
        //        param.Add("@InterestRate", InterestRate??0);
        //        param.Add("@PremiumPaid", PremiumPaid??0);
        //        param.Add("@PurchaseDate", request.PurchaseDate ?? DateTime.Now.ToString());
        //        param.Add("@PolicyStatus", (int)request.PolicyStatus);
        //        param.Add("@PhoneInsuranceCustomerId", customer_Id);
        //        param.Add("@IMEINumber1", ImeiNumber1 ?? "");
        //        param.Add("@IMEINumber2", ImeiNumber2??"");
        //        param.Add("@SecondaryContactName", request.SecondaryContact ?? "");
        //        param.Add("@SecondaryContactPhone", request.SecondaryContact ?? "");
        //        param.Add("@repaymentPeriod", "12");
        //        param.Add("@serialNumber", request.SerialNumber);
        //        param.Add("@phoneNumber", SafeDbObject(request.Phonenumber));
        //        param.Add("@mpesaref", SafeDbObject(request.MpesaRef));
        //        string query = "AddInsuranceRequest";
        //        await _db.Connection.ExecuteAsync(query,param,commandType:System.Data.CommandType.StoredProcedure);
        //        response.ErrorMsg = "Your request was accepted successfuly";
        //        response.Success = true;
        //        response.ResponseId = request.RequestId ?? "";
        //        response.CustomerId = customer_Id.ToString();
        //        response.TransactionId = Id.ToString();


        //        return response;

        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.ResponseId = request.RequestId ?? "";
        //        response.CustomerId = "";
        //        response.TransactionId = "";
        //        //_isettings.LogRequests(JsonConvert.SerializeObject(request),"PurchaseInsurance",RequestType.Comparison);
        //        _isettings.LogRequests(ex.Message, "PurchaseInsurance", RequestType.Error);
        //        return response;
        //    }

        //    return null;
        //}
    }
}
