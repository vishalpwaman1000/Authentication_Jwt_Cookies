using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_System.Model
{
    public class GetInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public List<GetInformation> getInformation { get; set; } 
    }

    public class GetInformation
    {
        public string UserName { get; set; } 
        public string EmailID { get; set; }
        public string MobileNumber { get; set; } 
        public string Salary { get; set; } 
        public string Gender { get; set; }
        public bool IsActive { get; set; }
    }
}
