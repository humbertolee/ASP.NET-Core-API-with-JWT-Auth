using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ASPNetCoreJWTAuthentication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ASPNetCoreJWTAuthentication.Controllers
{
	[AllowAnonymous]
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IConfiguration _configuration; 

		public AuthController(UserManager<IdentityUser> userManager, IConfiguration configuration) 
		{
			_userManager = userManager;
			_configuration = configuration;
		}

		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] Login login)
		{
			var user = await _userManager.FindByNameAsync(login.EmailAddress);

			if (user != null && await _userManager.CheckPasswordAsync(user, login.Password)) 
			{
				var authClaims = new[]
				{
					new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.UserName),
					new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
					new System.Security.Claims.Claim("MyCustomClaim", "MyClaimValue")
				};

				var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

				var token = new JwtSecurityToken(
					issuer: _configuration["JwtSettings:Issuer"],
					audience: _configuration["JwtSettings:Audience"],
					expires: DateTime.UtcNow.AddMinutes(10),
					claims: authClaims,
					signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
				);

				return Ok(new 
				{
					token = new JwtSecurityTokenHandler().WriteToken(token),
					expiration = token.ValidTo
				});
			}

			return Unauthorized();
		}

		[HttpPost("CreateTestUser")]
		public async Task<IActionResult> GetCreateUser([FromBody] Login login)
		{
			var user = new IdentityUser { UserName = login.EmailAddress, Email = login.EmailAddress };
			var result = await _userManager.CreateAsync(user, login.Password);

			if (result.Succeeded)
			{
				return Ok($"User {login.EmailAddress} created!");
			}
			else
			{
				string summary = string.Empty;

				foreach (var error in result.Errors) 
				{
					summary += error.Description;
				}

				return StatusCode(400, summary);
			}			
		}
	}
}
