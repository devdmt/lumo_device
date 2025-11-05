
using System.Data;
using DAL.ViewModels;
using API.Infrastructure.Interface;
using Dapper;
using Mapster;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Microsoft.Extensions.Options;
using DAL.ModelView.ClaimDTO;
using DAL.ViewModels.ClaimDTO;
using DAL.ModelView;
using DAL.Model.Safaricom;
namespace API.Infrastructure.Application.Clients
{
    internal partial class ClientClaims : IClaimManager
    {

        readonly ApplicationDbContext _db;
       
    
        readonly Isettings _settings;
       
        public ClientClaims(ApplicationDbContext db,
           Isettings setting)
        {
            _db = db;
            _settings=setting;
         

        }
        public async Task<ClaimsDTODetails> GetClaim(long id)
        {
            var response = new ClaimsDTODetails();
            try
            {
                string criteria = "";
                int NoOfRecords = 0;
                int TotalNoOfPages = 0;
                var param = new DynamicParameters();
                param.Add("Id", id);


                //param.Add("Skip",status.Skip);
                //param.Add("Take",status.Take);
                //param.Add("NoOfRecords",NoOfRecords, direction: ParameterDirection.Output);
                //param.Add("TotalNoOfPages",TotalNoOfPages, direction: ParameterDirection.Output);
                response = await _db.Connection.QueryFirstOrDefaultAsync<ClaimsDTODetails>("GetClaim", param,
                   commandType: System.Data.CommandType.StoredProcedure);
                //response = await _db.Connection.QueryAsync<List<ClaimsDTO>>("GetClaims",param,
                //commandType: System.Data.CommandType.StoredProcedure);

                //response= claimsq.Adapt<ClaimsDTODetails>();

                return response;
            }
            catch (Exception ex)
            {
                _settings.LogRequests(ex.Message, "GetAllClaims", RequestType.Error);
            }




            return null;
        }
        public async Task<ResponseDTO<List<ClaimsDTO>>> GetAllClaims(SearchCriteria status)
        {
            var response = new ResponseDTO<List<ClaimsDTO>>();
            response.Result = new List<ClaimsDTO>();
            IEnumerable<dynamic> claims = null;
            int NoOfRecords = 0;
            int TotalNoOfPages = 0;
            var _param = new DynamicParameters();
            try
            {
                bool datesnotquery=true;
                string criteria = "";
                DateTime datefrom =  DateTime.MinValue;
                DateTime dateto = DateTime.MinValue;
                if (!string.IsNullOrEmpty(status.datefrom))
                {
                    if (DateTime.TryParseExact(status.datefrom, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out datefrom))
                    { 
                        datesnotquery = true;
                        // Parsed successfully
                    }
                    else
                    {
                        datefrom = DateTime.MinValue;
                    }
                }
                if (!string.IsNullOrEmpty(status.dateto))
                {
                    if (DateTime.TryParseExact(status.dateto, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dateto))
                    {
                    datesnotquery = false;
                    // Parsed successfully
                }
                    else
                    {
                        dateto = DateTime.MinValue;
                    }
                }
                //DateTime.TryParse(status.datefrom, out datefrom);
                //DateTime.TryParse(status.dateto, out dateto);


                _param.Add("Id", status.Id);

                _param.Add("Idnumber", status.Idnumber);
                _param.Add("phonenumber", status.phonenumber);
                if (datesnotquery)
                {
                     _param.Add("datefrom", null);
                _param.Add("dateto", null);
                }
                else
                {
                     _param.Add("datefrom", datefrom);
                _param.Add("dateto", dateto);
                }
               
                _param.Add("claimtype", status.claimStatus == null ? null : (int)status.claimStatus);
                _param.Add("claimref", status.ClaimRefNumber);
                _param.Add("approved", status.Approved);
                _param.Add("shopId", status.shopId);
                _param.Add("location", status.location);
                _param.Add("model", status.model);
                _param.Add("parts", status.parts);
                _param.Add("imeinumber", status.IMEINumber);
                _param.Add("Skip", status.Skip);
                _param.Add("Take", status.Take);
                _param.Add("NoOfRecords", NoOfRecords, direction: ParameterDirection.Output);
                _param.Add("TotalNoOfPages", TotalNoOfPages, direction: ParameterDirection.Output);
                claims = await _db.Connection.QueryAsync("GetClaims", _param,
                    commandType: CommandType.StoredProcedure);
                //response = await _db.Connection.QueryAsync<List<ClaimsDTO>>("GetClaims",_param,
                //commandType: System.Data.CommandType.StoredProcedure);
                NoOfRecords = _param.Get<int>("NoOfRecords");
                TotalNoOfPages = _param.Get<int>("TotalNoOfPages");
                response.Result = claims.Adapt<List<ClaimsDTO>>();

                
                response.NoOfRecords = NoOfRecords;
                response.TotalNoOfPages = TotalNoOfPages;
                return response;
            }
            catch (Exception ex)
            {
                _settings.LogRequests(ex.Message, "GetAllClaims", RequestType.Error);
            }




            return null;
        }
        string converttoBase64(string value)
        {
            string basepath = "";
            if (!string.IsNullOrEmpty(value))
            {
                var base64EncodedBytes = System.IO.File.ReadAllBytes(value);
                return Encoding.UTF8.GetString(base64EncodedBytes);
            }
            return "";
        }
       
       
       

    }
}
