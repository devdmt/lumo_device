using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels.ClaimDTO
{
    public class DeviceParts
    {
      public string? Id {get;set;}
      public string? Name {get;set;}
      public string? Description {get;set;}
      public string? Active {get;set;}
      public string? CreatedOn {get;set;}
      public string? LaborCost {get;set;}
      public string? PartCosts {get;set;}
      public string? ReplacementLimit {get;set;}
      public string? ModelId {get;set;}
      public string? ModelName {get;set;}
      public string? autoapprove {get;set;}

    }
}
