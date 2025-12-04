
using DAL.Model.Safaricom;
using DAL.ModelView;
using DAL;
using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.ModelView.ClaimDTO;
using DAL.ViewModels.ClaimDTO;
namespace API.Infrastructure.Interface
{
    public interface IClaimManager:ITransientService
    {
         Task<ResponseDTO<List<ClaimsDTO>>> GetAllClaims(SearchCriteria status);
        Task<ResponseDTO<List<ClaimsDTO>>> GetAutoApproved(ApprovalDTO status);
         Task<ClaimsDTODetails> GetClaim(long id);                      
         Task<ResponseDTO<List<CustomerDTO>>> GetCustomers(Devicesearch status);

         Task<ResponseDTO<List<DeviceDTO>>> GetDevices(Devicesearch devicesearch);
        Task<ResponseDTO<PolicyUploadResult>> UploadPolicyPurchasesAsync(PolicyUploadRequest request);
       // Task<List<DeviceModels>> GetDeviceModel();
       
      // Task<Response<FileResponse>> ClaimReport(ReportDTO status);  
        //Task<Response> UploadShops(FileUploadRequest uploadRequest,int shoptype,int logintype,UserClaim userClaim);
        //Task<PaginationResponse<ShopDTO>> getShops(ShopFilters shopFilters);
        //Task<Response> DeleteShop(string id, UserClaim userClaim);
       // Task<Response<FileRespon>> PolicyReport(PolicyReportDTO reportDTO);
        //Task<Response<ShopDTO>> CreateShop(ShopDTO shopDTO,UserClaim userClaim);
        Task<ResponseDTO<DashboardDTO>> GetDashboard();
    }
}
