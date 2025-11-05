//using DAL.Model.Safaricom;
//using DAL.ModelView;
//using DAL;
//using DocumentFormat.OpenXml.Presentation;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using DAL.ModelView.ClaimDTO;
//using DAL.ViewModels.ClaimDTO;
//using Dapper;
//namespace API.Infrastructure.Application.Clients
//{
//    internal partial class ClaimManager
//    {
//        public async Task<ResponseDTO<FileResponse>> PolicyReport(PolicyReportDTO report)
//        { 
//              var resp = new Response<FileResponse>();
//            string reportPath = "";
            
//            try
//            {
//                string criteria = "";
//                 string reportpath = _reportSettings.RDLCFilePath+ "\\policy.rdlc";
               
                   
//                    if (report.PurchaseDate !=null && report.PurchaseDate.Length>1)
//                    {
//                        criteria += " and Convert(nvarchar,PurchaseDate,100)  between cast('"+ report.PurchaseDate[0].ToString() +"' as datetime) and cast('"+ report.PurchaseDate[1].ToString()+"' as datetime)) ";
//                    }
//                    if(report.CreatedDate !=null && report.CreatedDate.Length > 1){
//                    criteria+= criteria != "" ? "where  Format(cast(CreatedOn as datetime),'dd-MMM-yyyy HH:mm:ss','en-us')" : "and  Format(cast(CreatedOn as datetime),'dd-MMM-yyyy HH:mm:ss','en-us')";
//                    }    
//                    if (!string.IsNullOrEmpty(report.phonenumber))
//                    {
//                    criteria += criteria != "" ? " and " : " where ";
//                        criteria += "  PhoneNumber='" + report.phonenumber + "'";
//                    }
//                    if (!string.IsNullOrEmpty(report.Idnumber))
//                    {
//                       criteria += criteria != "" ? " and " : " where ";
//                        criteria += "  IDNumber='" + report.Idnumber + "'";
//                    }
//                    if (!string.IsNullOrEmpty(report.PhoneModel))
//                    {
//                       criteria += criteria != "" ? " and " : " where ";
//                        criteria += "  PhoneModel='" + report.PhoneModel + "'";
//                    }

//                      if (!string.IsNullOrEmpty(report.IMEINumber))
//                    {
//                       criteria += criteria != "" ? " and " : " where ";
//                        criteria += "  IMEINumber like '%" + report.IMEINumber + "%' or IMEINumber1 like '%" + report.IMEINumber + "%' or IMEINumber2 like '%" + report.IMEINumber + "%' ";
//                    }

//                string query = "SELECT [PhoneNumber],[IdNumber],[CustomerName],[DateofBirth],[PhoneModel],[IMEINumber],[PhoneCost],[PurchaseMode],[LoanRefNumber],[RepaymentTerms],[LoanAmount],[InterestRate]," +
//                    "[PremiumPaid],[PurchaseDate],[Processed],[RequestedOn],[PolicyStatus],[IMEINumber1],[IMEINumber2],[SecondaryContactName],[Active],[PhoneName],[repaymentPeriod],[serialnumber],[CreatedOn]  FROM [dbo].[vw_policy] "+ criteria +"";
//                //var result = await _db.Connection.QueryAsync<PolicyDTO>(query);
//                resp=await GenerateReport(report.docTypes, query, reportpath,"DataSet1","Policy",true);


//            }
//            catch (Exception ex) {
            
//            }
//        return resp;
//        }
         
//    }a
//}
