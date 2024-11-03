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
	public class AuthController : ControllerBase
	{
		private IConfiguration _configuration;
		private IUnitOfWork _unit;
		public AuthController(IConfiguration configuration, IUnitOfWork unitOfWork)
		{
			_configuration = configuration;
			_unit = unitOfWork;
		}

		[Route("Login")]
		[HttpPost]
		public async Task<IActionResult> LoginAsync(Login model)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					return BadRequest(ModelState);
				}
				User user = await _unit.UserRepository.CheckLogin(model);
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
					Message = "Login fail"
				});
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
					 new Claim(ClaimTypes.Name, user.AccountName ?? "Auth")

				}),
				Expires = expirationUtc,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
