
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.ClaimDTO
{
    public class DashboardDTO
    {
        public int TotalPolicy {  get; set; }
        public double TotalPremium { get { 
                double tPremium = 0;
             
                if(TotalPremium1 != null)
                {
                    tPremium=Math.Round(TotalPremium1,2);
                }
            return tPremium;
            } }
        public double TotalPremium1 {  get; set; }
        public double TotalSumAssured {  get; set; }
          public int DailyTotalPolicy {  get; set; }
        public double DailyTotalPremium {  get; set; }
        public double DailyTotalSumAssured {  get; set; }
        public List<DailySummary>? dailySummaries { get; set; }

    }

    public class DailySummary
    {
        public int DailyTotalPolicy {  get; set; }
        public double DailyTotalPremium {  get; set; }
        public double DailyTotalSumAssured {  get; set; }
        public string TrnDate { get; set; }

    }
}
