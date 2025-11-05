using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   
         public class UserClaim
    {
        public UserClaim()
        {
            userid = "1";
        }
        public string username { get; set; }
        public string fullname { get; set; }
        public string userid { get; set; }
        public string email { get; set; }
        public string branchId { get; set; }
        public string departmentId { get; set; }    
    }
    
}
