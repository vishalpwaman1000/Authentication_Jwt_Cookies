using Authentication_System.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication_System.DataAccessLayer
{
    public interface IAuthenticationDataAccess
    {
        public Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request);
        public Task<UserLoginResponse> UserLogin(UserLoginRequest request);
        public Task<AddInformationResponse> AddInformation(AddInformationRequest request);
        public Task<GetInformationResponse> GetInformation();
    }
}
