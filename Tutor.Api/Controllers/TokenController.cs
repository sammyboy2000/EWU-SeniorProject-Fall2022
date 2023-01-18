using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tutor.Api.Identity;
using Tutor.Api.Models;


namespace Tutor.Api.Controllers;
[Route("Token")]
[ApiController]
public class TokenController : Controller
{
    public tutor_dbContext _context;
    public UserManager<AppUser> _userManager;
    public JwtConfiguration _jwtConfiguration;
    public TokenController(tutor_dbContext context, UserManager<AppUser> userManager, JwtConfiguration jwtConfiguration)
    {
        _context = context;
        _userManager = userManager;
        _jwtConfiguration = jwtConfiguration;
    }
    [HttpPost("GetToken")]
    public async Task<IActionResult> GetToken([FromBody] UserCredentials userCredentials)
    {
        if (string.IsNullOrEmpty(userCredentials.Username))
        {
            return BadRequest("Username is required");
        }
        if (string.IsNullOrEmpty(userCredentials.Password))
        {
            return BadRequest("Password is required");
        }

        var user = _context.Users.FirstOrDefault(u => u.UserName == userCredentials.Username);

        if (user is null) { return Unauthorized("The user account was not found"); }

        bool results = await _userManager.CheckPasswordAsync(user, userCredentials.Password);
        if (results)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId", user.Id.ToString()),
                new Claim(Claims.Random, (new Random()).NextDouble().ToString()),
                new Claim(Claims.UserName, user.UserName.ToString().Substring(0,user.UserName.ToString().IndexOf("@"))),
            };
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtConfiguration.ExpirationInMinutes),
                signingCredentials: credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new { token = jwtToken });
        }
        return Unauthorized("The username or password is incorrect");
    }

    [HttpGet("test")]
    [Authorize]
    public string Test()
    {
        return "something";
    }

    [HttpGet("testadmin")]
    [Authorize(Roles = Roles.Admin)]
    public string TestAdmin()
    {
        return "Authorized as Admin";
    }

    [HttpGet("testruleroftheuniverse")]
    [Authorize(Roles = "RulerOfTheUniverse,Meg")]
    public string TestRulerOfTheUniverseOrMeg()
    {
        return "Authorized as Ruler of the Universe or Meg";
    }

    [HttpGet("testrandomadmin")]
    [Authorize(Policy = Policies.RandomAdmin)]
    public string TestRandomAdmin()
    {
        return $"Authorized randomly as Random Admin with {User.Identities.First().Claims.First(c => c.Type == Claims.Random).Value}";
    }

}

public class UserCredentials
{
    public string Username { get; set; }
    public string Password { get; set; }
    public UserCredentials(string username, string password)
    {
        Username = username;
        Password = password;
    }
}