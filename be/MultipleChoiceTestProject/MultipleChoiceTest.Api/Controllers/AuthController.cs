using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.UnitOfWork;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        public AuthController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(Login model)
        {
            try
            {
                User user = await _unitOfWork.UserRepository.CheckLogin(model);
                if (user != null)
                {
                    return Ok(new ApiResponse<LoginResponse>
                    {
                        Success = true,
                        Data = new LoginResponse()
                        {
                            AccessToken = GenerateTokenUser(user),
                            User = user
                        },
                        Message = "Login Successfully"
                    });
                }

                return Ok(new ApiResponse<LoginResponse>()
                {
                    Success = false,
                    Data = null,
                    Message = "Login fail"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Route("RegisterForUser")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(Register model)
        {
            try
            {
                var userModel = _mapper.Map<User>(model);
                userModel.IsAdmin = false;
                var userRs = await _unitOfWork.UserRepository.RegisterUser(userModel);
                return Ok(userRs);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        private string GenerateTokenUser(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            var nowUtc = DateTime.UtcNow;
            var expirationDuration =
                TimeSpan.FromHours(1); // Adjust as needed
            var expirationUtc = nowUtc.Add(expirationDuration);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(JwtRegisteredClaimNames.Sub,
                               _configuration["JWT:Subject"]),
                     new Claim(JwtRegisteredClaimNames.Jti,
                               Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Iat,
                               EpochTime.GetIntDate(nowUtc).ToString(),
                               ClaimValueTypes.Integer64),
                     new Claim(JwtRegisteredClaimNames.Exp,
                               EpochTime.GetIntDate(expirationUtc).ToString(),
                               ClaimValueTypes.Integer64),
                     new Claim(JwtRegisteredClaimNames.Iss,
                               _configuration["JWT:Issuer"]),
                     new Claim(JwtRegisteredClaimNames.Aud,
                               _configuration["JWT:Audience"]),
                     new Claim("id", user.Id.ToString()),
                     new Claim(ClaimTypes.Name, user.AccountName ?? "Auth"),
                     new Claim(ClaimTypes.Role, user.IsAdmin == true ? "1" : "0")
                }),
                Expires = expirationUtc,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
