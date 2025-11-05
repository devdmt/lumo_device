
using DAL.ModelView;


namespace API.Infrastructure.Interface
{
    public interface IMSureManager:ITransientService
    {
        Task<ResponseDTO> OnboardingRequest(OnboardingDTO onboardingDto);
        Task<ResponseDTO> ProcessRequest(MsureDTO msureDTO);
        Task<List<ProductDTO>> GetProducts(string partnerCode);
    }
}
