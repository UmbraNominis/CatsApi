namespace CatsApi.Controllers;

using CatsApi.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

/// <summary>
/// Controller for Identities.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class IdentityController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public IdentityController(UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    /// <summary>
    /// Registers a User.
    /// </summary>
    /// <param name="DTO"></param>
    /// <returns></returns>
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDTO DTO)
    {
        var user = new IdentityUser()
        {
            UserName = DTO.Username
        };
        var result = await _userManager.CreateAsync(user, DTO.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        return Ok();
    }

    /// <summary>
    /// Logs in a User.
    /// </summary>
    /// <param name="DTO"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDTO DTO)
    {
        var user = await _userManager.FindByNameAsync(DTO.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, DTO.Password))
            return Unauthorized();

        var token = GenerateToken(user);

        return Ok(token);
    }

    private string GenerateToken(IdentityUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        };

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(333),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
