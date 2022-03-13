using Authentication_System.DataAccessLayer;
using Authentication_System.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Authentication_System.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
    public class AuthenticationController : ControllerBase
    {

        public readonly IAuthenticationDataAccess _authenticationDataAccess;

        public AuthenticationController(IAuthenticationDataAccess authenticationDataAccess)
        {
            _authenticationDataAccess = authenticationDataAccess;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();
            try
            {

                response = await _authenticationDataAccess.RegisterUser(request);

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin(UserLoginRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();
            try
            {
                response = await _authenticationDataAccess.UserLogin(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            try
            {
                response = await _authenticationDataAccess.AddInformation(request);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet]
        [Authorize(Roles ="admin, user")]
        public async Task<IActionResult> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse();
            try
            {
                response = await _authenticationDataAccess.GetInformation();
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}
