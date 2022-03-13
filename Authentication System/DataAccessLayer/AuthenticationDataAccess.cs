using Authentication_System.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication_System.DataAccessLayer
{
    public class AuthenticationDataAccess : IAuthenticationDataAccess
    {
        public readonly IConfiguration _configuration;
        public readonly MySqlConnection _mySqlConnection;

        public AuthenticationDataAccess(IConfiguration configuration)
        {
            _configuration = configuration;
            _mySqlConnection = new MySqlConnection(_configuration["ConnectionStrings:MysqlConnection"]);
        }

        public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest request)
        {
            RegisterUserResponse response = new RegisterUserResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";
            try
            {

                if(_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                if(request.Password != request.ConfirmPassword)
                {
                    response.IsSuccess = false;
                    response.Message = "Password Not Match";
                    return response;
                }

                string SqlQuery = @"INSERT INTO crudoperation.userdetail 
                                    (UserName, PassWord, Role) Values 
                                    (@UserName, @PassWord, @Role);";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@PassWord", request.Password);
                    sqlCommand.Parameters.AddWithValue("@Role", request.Role);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "Register Query Not Executed";
                        return response;
                    }
                }

            }catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<UserLoginResponse> UserLogin(UserLoginRequest request)
        {
            UserLoginResponse response = new UserLoginResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT UserId, UserName, Role
                                    FROM crudoperation.userdetail
                                    WHERE UserName=@UserName AND PassWord=@PassWord";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@PassWord", request.Password);
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            await dataReader.ReadAsync();
                            response.Message = "Login Successful";
                            response.data = new UserLoginInformation();
                            response.data.UserID = dataReader["UserId"] != DBNull.Value ? Convert.ToString(dataReader["UserId"]) : string.Empty;
                            response.data.UserName = dataReader["UserName"] != DBNull.Value ? Convert.ToString(dataReader["UserName"]) : string.Empty;
                            response.data.Role = dataReader["Role"] != DBNull.Value ? Convert.ToString(dataReader["Role"]) : string.Empty;
                            response.Token = GenerateJwt(response.data.UserID, response.data.UserName, response.data.Role);
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.Message = "Login Unsuccessful";
                            return response;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<AddInformationResponse> AddInformation(AddInformationRequest request)
        {
            AddInformationResponse response = new AddInformationResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"INSERT INTO crudoperation.crudapplication
                                    (UserName, EmailID, MobileNumber, Salary, Gender) Values
                                    (@UserName, @EmailID, @MobileNumber, @Salary, @Gender)";
                
                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    //UserName, EmailID, MobileNumber, Salary, Gender
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    sqlCommand.Parameters.AddWithValue("@UserName", request.UserName);
                    sqlCommand.Parameters.AddWithValue("@EmailID", request.EmailID);
                    sqlCommand.Parameters.AddWithValue("@MobileNumber", request.MobileNumber);
                    sqlCommand.Parameters.AddWithValue("@Salary", request.Salary);
                    sqlCommand.Parameters.AddWithValue("@Gender", request.Gender);
                    int Status = await sqlCommand.ExecuteNonQueryAsync();
                    if (Status <= 0)
                    {
                        response.IsSuccess = false;
                        response.Message = "AddInformation Query Not Executed";
                        return response;
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public async Task<GetInformationResponse> GetInformation()
        {
            GetInformationResponse response = new GetInformationResponse();
            response.IsSuccess = true;
            response.Message = "SuccessFul";

            try
            {
                if (_mySqlConnection.State != System.Data.ConnectionState.Open)
                {
                    await _mySqlConnection.OpenAsync();
                }

                string SqlQuery = @"SELECT * FROM crudoperation.crudapplication";

                using (MySqlCommand sqlCommand = new MySqlCommand(SqlQuery, _mySqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.Text;
                    sqlCommand.CommandTimeout = 180;
                    using (DbDataReader dataReader = await sqlCommand.ExecuteReaderAsync())
                    {
                        if (dataReader.HasRows)
                        {
                            response.getInformation = new List<GetInformation>();

                            while(await dataReader.ReadAsync())
                            {
                                GetInformation data = new GetInformation();

                                data.UserName = dataReader["UserName"] != DBNull.Value ? Convert.ToString(dataReader["UserName"]) : string.Empty;
                                data.EmailID = dataReader["EmailID"] != DBNull.Value ? Convert.ToString(dataReader["EmailID"]) : string.Empty;
                                data.MobileNumber = dataReader["MobileNumber"] != DBNull.Value ? Convert.ToString(dataReader["MobileNumber"]) : string.Empty;
                                data.Salary = dataReader["Salary"] != DBNull.Value ? Convert.ToString(dataReader["Salary"]) : string.Empty;
                                data.Gender = dataReader["Gender"] != DBNull.Value ? Convert.ToString(dataReader["Gender"]) : string.Empty;
                                data.IsActive = dataReader["IsActive"] != DBNull.Value ? Convert.ToBoolean(dataReader["IsActive"]) : false;

                                response.getInformation.Add(data);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            finally
            {
                await _mySqlConnection.CloseAsync();
                await _mySqlConnection.DisposeAsync();
            }

            return response;
        }

        public string GenerateJwt(string UserID, string Email, string Role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //claim is used to add identity to JWT token
            var claims = new[] {
         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
         new Claim(JwtRegisteredClaimNames.Sid, UserID),
         new Claim(JwtRegisteredClaimNames.Email, Email),
         new Claim(ClaimTypes.Role,Role),
         new Claim("Date", DateTime.Now.ToString()),
         };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
              _configuration["Jwt:Audiance"],
              claims,    //null original value
              expires: DateTime.Now.AddMinutes(120),

              //notBefore:
              signingCredentials: credentials);

            string Data = new JwtSecurityTokenHandler().WriteToken(token); //return access token 
            return Data;
        }

    }
}
