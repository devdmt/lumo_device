using DAL.ModelView;
using DAL.ViewModels;
using DAL.ViewModels.ClaimDTO;
using Dapper;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Infrastructure.Application.Clients
{
    internal partial class ClientClaims
    {
        public async  Task<ResponseDTO<DashboardDTO>> GetDashboard()
        {
            var response= new ResponseDTO<DashboardDTO>();
            try
            {
                var dashboardto= new DashboardDTO();

                string totalquery = "select  count(1)*2  as totalPolicy,sum(round([PremiumPaid],2))*2  as TotalPremium1, sum(round([PhoneCost],2))*2  as TotalSumAssured from [dbo].[PhoneInsuranceRequest]";
                var totals = await _db.Connection.QueryFirstOrDefaultAsync<DashboardDTO>(totalquery);
                dashboardto = totals.Adapt<DashboardDTO>();
                totalquery = "select count(1) *2  as DailyTotalPolicy,sum(round([PremiumPaid],2))*2  as DailyTotalPremium, sum(round([PhoneCost],2))*2  as DailyTotalSumAssured from [PhoneInsuranceRequest] where convert(nvarchar,RequestedOn,105) = convert(nvarchar,GETDATE(),105)";
                var dailyresult=  await _db.Connection.QueryFirstOrDefaultAsync<DashboardDTO>(totalquery);
                dashboardto.DailyTotalSumAssured=dailyresult.DailyTotalSumAssured;
                dashboardto.DailyTotalPremium = dailyresult.DailyTotalPremium;
                dashboardto.DailyTotalPolicy = dailyresult.DailyTotalPolicy;


                string dailysummaryquery = "select count(1)*2 as DailyTotalPolicy,sum(round([PremiumPaid],2)) *2 as DailyTotalPremium," +
                    " sum(round([PhoneCost],2))*2 as DailyTotalSumAssured," +
                    "convert(nvarchar,RequestedOn,106) as TrnDate,CAST(RequestedOn AS DATE)  from [PhoneInsuranceRequest] " +
                    "group by convert(nvarchar,RequestedOn,106)  ,CAST(RequestedOn AS DATE) order by CAST(RequestedOn AS DATE) asc";
                var dailysummary= await _db.Connection.QueryAsync<DailySummary>(dailysummaryquery);
                dashboardto.dailySummaries = dailysummary.ToList();

                response.Result = dashboardto;
                response.Success=true; ;
                response.ErrorMsg = "";
                

            }
            catch (Exception ex) { 
            
            }

            return response;
        }
    }
}
