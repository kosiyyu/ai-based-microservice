using Microsoft.AspNetCore.Mvc;
using Models;
using Microsoft.IdentityModel.Tokens;
using Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Controller;

[ApiController]
[Route("api/")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("authentication")]
    public async Task<IActionResult> Authentication(User user)
    {   
        var fetchedUser = await _userService.GetAsync(user.Email);

        if(fetchedUser == null)
        {
            return Unauthorized();
        }

        if(!(user.Email == fetchedUser.Email && user.Password == fetchedUser.Password))
        {
            return Unauthorized();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(EnvSettings.JwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, fetchedUser.Id.ToString())
        };

        var Sectoken = new JwtSecurityToken(
            issuer: EnvSettings.JwtIssuer,
            audience: EnvSettings.JwtIssuer,
            claims: claims,
            expires: DateTime.Now.AddMinutes(5),
            signingCredentials: credentials
        );

        var token =  new JwtSecurityTokenHandler().WriteToken(Sectoken);

        return Ok(token);
    }

    [Authorize]
    [HttpGet("validatebearer")]
    public IActionResult ValidateBearer()
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim != null)
        {
            var userId = userIdClaim.Value;
            return Ok(userId);
        }

        return Unauthorized();
    }
}