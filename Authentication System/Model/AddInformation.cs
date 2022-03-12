using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_System.Model
{
    public class AddInformationRequest
    {
        //UserName, EmailID, MobileNumber, Salary, Gender
        [Required(ErrorMessage ="UserName Is Mandetory")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "EmailID Is Mandetory")]
        public string EmailID { get; set; }

        [Required(ErrorMessage = "MobileNumber Is Mandetory")]
        public int MobileNumber { get; set; }

        [Required(ErrorMessage = "Salary Is Mandetory")]
        public int Salary { get; set; }

        [Required(ErrorMessage = "Gender Is Mandetory")]
        public string Gender { get; set; }

    }

    public class AddInformationResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
