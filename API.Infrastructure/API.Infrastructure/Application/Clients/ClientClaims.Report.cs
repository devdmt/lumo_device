//using AspNetCore.Reporting;
//using Microsoft.Reporting.NETCore;
//using Microsoft.EntityFrameworkCore;
//using System.Data;
//using LocalReport = Microsoft.Reporting.NETCore.LocalReport;
//using Dapper;
//using System.Data.SqlClient;
//using DAL.ViewModels;
//using Microsoft.ReportingServices.Interfaces;
//using Mapster;

//namespace API.Infrastructure.Application.Clients
//{
//    internal partial class ClaimManager
//    {

//         public async Task<Response<FileResponse>> ClaimReport(ReportDTO status)
//        {
//            var resp = new Response<FileResponse>();
//            string reportPath = "";
                
//            try
//            {
//                Dictionary<string, string> paramss = new Dictionary<string, string>();

               
//                string criteria = "";
//                string reportpath = _reportSettings.RDLCFilePath+ "\\Claim.rdlc";
               
//                    if (!string.IsNullOrEmpty(status.claimType.ToString()))
//                    {
//                        criteria += " and a.ClaimType=" + (int)status.claimType + "";
//                    }
//                    if (!string.IsNullOrEmpty(status.datefrom) && !string.IsNullOrEmpty(status.dateto))
//                    {
//                        criteria += " and Convert(nvarchar,a.ClaimDate,105)  between cast('"+ status.datefrom+"' as datetime) and cast('"+ status.dateto+"' as datetime)) ";
//                    }
//                    if (!string.IsNullOrEmpty(status.phonenumber))
//                    {
//                        criteria += " and b.PhoneNumber='" + status.phonenumber + "'";
//                    }
//                    if (!string.IsNullOrEmpty(status.Idnumber))
//                    {
//                        criteria += " and a.IDNumber='" + status.Idnumber + "'";
//                    }
//                    if (!string.IsNullOrEmpty(status.PhoneModel))
//                    {
//                        criteria += " and c.PhoneModel='" + status.PhoneModel + "'";
//                    }
                    
//                    string query = "SELECT distinct d.ShopName,c.PhoneModel,b.PhoneNumber,a.[Id],a.[PartnerID],a.[ProductID],a.[CustomerName],a.[ClaimRefNumber],a.[IDNumber]," +
//                        "a.[Narration], case when a.[ClaimType] =0 then 'damage' when a.[ClaimType] =1 then 'theft' when  a.[ClaimType] =2 then 'Credit Life' end  as  ClaimType ,a.[DamagePart],a.[ReplacementCost],a.[IncidentDate],a.[ClaimDate],a.[Abstract], " +
//                        "a.[Processed],a.[CreatedOn],a.[PartnerCode],a.[RequestId],a.[TrnId],a.[UserId],a.[medicalReportUpload], " +
//                        "a.[policeAbstractUpload],a.SourceOfClaim,a.[imagePhoneUpload],a.[imageIMEIUpload],a.[ResponseId],a.[PhoneInsuranceCustomerId]," +
//                        " a.[claimStatus],a.[PartId],a.[IMEINumber],a.[IMEINumber1],a.[IMEINumber2],a.[LabourCost],a.[PartCost],a.[Comments]," +
//                        " a.[PhoneId],a.[ShopId],a.[ShopType],a.[Dispatched],a.[DispatchedOn],a.[NotificationNumber],a.[DispatchedId], " +
//                        "a.[DispatchedShopId],a.[AlternativeContact],a.[ErrorMessage],a.[Approved],a.[passedForProcessing] ," +
//                        " a.[policeAbstractUploadBase64],a.[imagePhoneUploadbase64],a.[imageIMEIUploadbase64] FROM [dbo].[claimRequests]  a  WITH (NOLOCK)," +
//                        " [dbo].[phoneInsuranceCustomers] b  WITH (NOLOCK),[dbo].[PhoneInsuranceRequest] c  WITH (NOLOCK), Shops d WITH (NOLOCK) where" +
//                        " a.[PhoneInsuranceCustomerId]= b.Id and a.PhoneId =c.Id  and a.[ShopId]=d.Id " + criteria;
                  
              
               
//               resp=await GenerateReport(status.docTypes, query, reportpath,"DataSet1","Claims");
//            }
//            catch (Exception ex)
//            {
//                _settings.LogRequests("ClaimReport", ex.Message,Log_Type.Error);
//            }
//            return resp;
//        }
//        public async Task<Response<FileResponse>> GenerateReport(  DocTypes docTypes,string query,string reportpath, string dataset,string docname,bool hasparams=false)
//        {
//            var resp = new Response<FileResponse>();
//            try
//            {
//                string reportType = docTypes.ToString();
//                 ReportDataSource reportDataSource = new ReportDataSource();
//                reportDataSource.Name = dataset; 
//             var ext = "xls";
//                string contenttype = "data:application/pdf;base64,";
//                string renderformat = "pdf";
//                LocalReport report = new LocalReport();
//                DataTable dataTable = new DataTable();
//                using (SqlConnection conn = _db.GetConnection())
//                {
//                      SqlCommand cmd = new(query, conn)
//                    {
//                        CommandType = CommandType.Text,
//                        CommandTimeout = 1000000

//                    };

//                    SqlDataAdapter da = new SqlDataAdapter(cmd);
//                    da.Fill(dataTable);

//                }
//                 switch (docTypes)
//                {
//                    case DocTypes.Excel:
//                        ext = "xls";
//                        contenttype = "data:application/vnd.ms-excel;base64,";
//                        renderformat="EXCEL";
//                        break;
//                    case DocTypes.Word:
//                         ext = "doc";
//                        contenttype = "data:application/msword;base64,";
//                        renderformat="Word";
//                        break;
//                        case DocTypes.Pdf:
//                         ext = "pdf";
//                        contenttype = "data:application/pdf;base64,"; 
//                        renderformat="PDF";
//                        break;
//                    default:
//                          ext = "xls";
//                         renderformat="EXCEL";
//                        contenttype = "data:application/vnd.ms-excel;base64,";
//                        break;


//                }
//                 Warning[] warnings;
//                string[] streamids;
//                string mimeType;
//                string encoding;
//                string extension;
//                reportDataSource.Value = dataTable;
//                report.ReportPath = reportpath;
//                report.DataSources.Clear();
//                report.DataSources.Add(reportDataSource);
//                //string[] paramvalue = new string();
//                //paramvalue[0] = dataTable.Rows.Count.ToString();
//                if (hasparams)
//                {
//                    report.SetParameters(new ReportParameter() { Name = "totalcount" , Visible = true , Values = { dataTable.Rows.Count.ToString()} });
//                }
                
//                var reportval = report.Render(renderformat, null, out mimeType, out encoding, out extension, out streamids, out warnings);
//                Console.WriteLine("GetTransactionsReport after render ");
//                var base64rep = Convert.ToBase64String(reportval);
//                 docname +=   DateTime.Now.ToString("yyyyMMddHHmmss").Replace(":", "") + "." + ext;
//                // var sreportPath = savePath + docname;
//                string path = System.AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "generatereports";
//                if (!Directory.Exists(path))
//                {
//                    Directory.CreateDirectory(path);
//                }
//                string fullpath = Path.Combine(path, docname);

//                FileStream fs = new FileStream(fullpath, FileMode.Create);
//                fs.Write(reportval, 0, reportval.Length);
//                //   this.reportViewer1.Refresh();
//                fs.Close();

//                    resp.Success = true;
//                    resp.ErrorMsg = "";// baseUrl + docname;
//                    resp.Result = new FileResponse();
//                    resp.Result.Name = docname;
//                    resp.Result.Extension = ext;
//                    resp.Result.Data = contenttype + base64rep;

//            } catch(Exception ex)
//            { 
//             _settings.LogRequests("GenerateReport", ex.Message,Log_Type.Error);
//            }
//            return resp;
//        }
       
//    }
//}
