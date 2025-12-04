using DAL.ModelView;
using DAL.ModelView.Safaricom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Interface
{
    public interface IPolicyManager: ITransientService
    {
        Task<OnboardingResponseDTO> PurchaseInsurance(PhoneInsuranceRequest request);
    }
}
