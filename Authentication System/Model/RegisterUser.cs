using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_System.Model
{
    public class RegisterUserRequest
    {
        [Required(ErrorMessage ="UserName Is Mandetory")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Is Mandetory")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Mandetory")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role Is Mandetory")]
        public string Role { get; set; }
    }

    public class RegisterUserResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }
}
