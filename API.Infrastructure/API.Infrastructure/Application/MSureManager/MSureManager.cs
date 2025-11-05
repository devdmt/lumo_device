using API.Infrastructure.Interface;
using DAL;
using DAL.Model;
using DAL.ModelView;
using Dapper;
using Mapster;
using Microsoft.IdentityModel.Tokens;

namespace API.Infrastructure.Application.MSureManager
{
    public class MSureManager : IMSureManager
    {
        private readonly ApplicationDbContext _db;
        readonly Isettings _isettings;
        public MSureManager(ApplicationDbContext db, Isettings isettings)
        {
            _db = db;
            _isettings = isettings;
        }
        public string GenerateTrnNo(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ResponseDTO> OnboardingRequest(OnboardingDTO onboardingDto)
        {
            var responseDTO = new ResponseDTO();

            try
            {
                string TransactionId = GenerateTrnNo(7);

                string onboardingId = Guid.NewGuid().ToString();
                var query = """
                    SELECT Id AS partnerId FROM  Partners 
                    WHERE PartnerCode=@PartnerCode
                    """;
                int partnerId = _db.Connection.ExecuteScalar<int>(query, new { PartnerCode = onboardingDto.PartnerCode });
                if (partnerId <= 0)
                {
                    responseDTO.ErrorMsg = "Partner does not exist";
                    responseDTO.Success = false;
                    return responseDTO;
                }

                string prod = """
                                 SELECT Id AS partnerProductId 
                                 FROM partnersProducts 
                                 WHERE PartnerId=@PartnerId
                                 """;
                int partnerProductId = _db.Connection.ExecuteScalar<int>(prod, new { PartnerId = partnerId });
                if (partnerProductId <= 0)
                {
                    responseDTO.ErrorMsg = "Partner Product does not exist";
                    responseDTO.Success = false;
                    return responseDTO;
                }

                var reg = """
                    SELECT RegNumber FROM OnboardingRequests 
                    WHERE RegNumber=@RegNumber
                    """;
                var RegNumber = _db.Connection.ExecuteScalar<string>(reg, new { onboardingDto.RegNumber });
                if (!string.IsNullOrEmpty(RegNumber))
                {
                    responseDTO.ErrorMsg = "Registration Number already exists";
                    responseDTO.Success = false;
                    return responseDTO;
                }

                var idNo = """
                    SELECT IDNumber FROM OnboardingRequests 
                    WHERE IDNumber=@IDNumber
                    """;
                var IDNumber = _db.Connection.ExecuteScalar<string>(idNo, new { onboardingDto.IDNumber });

                if (!string.IsNullOrEmpty(IDNumber))
                {
                    responseDTO.ErrorMsg = "ID Number already exists";
                    responseDTO.Success = false;
                    return responseDTO;
                }
                var request = new OnboardingRequests()
                {
                    PartnerId = partnerId,
                    TransactionId = TransactionId,
                    ProductId = partnerProductId,
                    CustomerName = onboardingDto.CustomerName,
                    DateOfBirth = onboardingDto.DateOfBirth,
                    IDNumber = onboardingDto.IDNumber,
                    Gender = onboardingDto.Gender,
                    Premium = onboardingDto.Premium,
                    BenefitOption = onboardingDto.BenefitOption,
                    BeneficiaryName = onboardingDto.BeneficiaryName,
                    RegNumber = onboardingDto.RegNumber,
                    BeneficiaryMobileNumber = onboardingDto.BeneficiaryMobileNumber,
                    CreatedOn = DateTime.Now,
                    Id = onboardingId.ToString(),
                    Processed = true,
                    Status = "success"
                };
                _db.OnboardingRequests.Add(request);
                await _db.SaveChangesAsync();
                responseDTO.Success = true;
                responseDTO.ErrorMsg = "";
                responseDTO.ResponseId = onboardingId;
                responseDTO.TransactionId = TransactionId;
            }
            catch (Exception ex)
            {
                _isettings.LogRequests(ex.Message, "OnboardingRequest", RequestType.Error);
            }
            return responseDTO;
        }


        public async Task<ResponseDTO> ProcessRequest(MsureDTO msureDTO)
        {
            var responseDTO = new ResponseDTO();
            try
            {
                string trnId = Guid.NewGuid().ToString();
                int partnerId = (int)_db.Connection.ExecuteScalar("select Id as partnerId from  [dbo].[Partners] where [PartnerCode]='" + msureDTO.partnerCode + "'");
                int transactionId = (int)_db.Connection.ExecuteScalar(" select count(1) from [dbo].[MsureRequests] where transactionId='" + msureDTO.transactionId + "'");
                if (transactionId > 0)
                {
                    responseDTO.ErrorMsg = "Transaction already exists";
                    responseDTO.Success = false;
                    return responseDTO;
                }

                int custId = (int)_db.Connection.ExecuteScalar(" select count(1) from [dbo].[MsureRequests] where customerId='" + msureDTO.customerId + "'");
                if (custId > 0)
                {
                    responseDTO.ErrorMsg = "Customer already exists";
                    responseDTO.Success = false;
                    return responseDTO;
                }
                var request = new MsureRequests()
                {
                    benefitOption = msureDTO.benefitOption,
                    CreatedOn = DateTime.Now,
                    customerId = msureDTO.customerId,
                    optinTime = msureDTO.optinTime,
                    PartnersId = partnerId,
                    Customername = msureDTO.customerName,
                    Gender = msureDTO.gender.ToString(),
                    premium = msureDTO.premium,
                    ProductsId = msureDTO.productId == null ? null : Convert.ToUInt16(msureDTO.productId),
                    status = msureDTO.status,
                    transactionId = msureDTO.transactionId,
                    Processed = false,
                    Id = trnId,
                };
                _db.msureRequests.Add(request);
                await _db.SaveChangesAsync();
                responseDTO.Success = true;
                responseDTO.ErrorMsg = "";
                responseDTO.ResponseId = trnId;
                 responseDTO.TransactionId = msureDTO.transactionId.ToString();
            } catch(Exception ex){
                responseDTO.ResponseId = msureDTO.transactionId;
                responseDTO.TransactionId = msureDTO.transactionId.ToString();
            }

            return responseDTO;
        }

        public async Task<List<ProductDTO>> GetProducts(string partnerCode)
        {
            var product = new List<ProductDTO>();
            try
            {
                string query = "SELECT [Id],[Name],[Description] ,[Image] FROM [dbo].[partnersProducts] where isnull([Active],'0')='1' " +
                    "and PartnerCode=(select Id from Partners where PartnerCode=" + partnerCode + ")";
                var result = await _db.Connection.QueryAsync(query);
                product = result.Adapt<List<ProductDTO>>();

            }
            catch (Exception ex)
            {

            }
            return product;
        }
    }
}
