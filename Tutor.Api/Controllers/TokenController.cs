using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
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

    [HttpPost("RegisterStudent")]
    public async Task<string> RegisterStudentAsync(String username, String password, String firstName, String lastName)
    {
        if (await _userManager.FindByNameAsync(username) == null)
        {
            try
            {
                var emailAddress = new MailAddress(username);
            }
            catch
            {
                return "Error, username is not a vaild email.";
            }

            AppUser user = new AppUser
            {
                UserName = username,
                Email = username,
            };
            IdentityResult result = _userManager.CreateAsync(user, password).Result;
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Student);
                ApiUser appUser = new()
                {
                    ExternalId = user.UserName,
                    IsStudent = true,
                    UserId = _context.ApiUsers.OrderBy(x => x.UserId).Last().UserId + 1
                };
                _context.ApiUsers.Add(appUser);
                Student s = new()
                {
                    UserId = appUser.UserId,
                    FirstName = firstName,
                    LastName = lastName,
                    Email = appUser.ExternalId,
                    Id = _context.Students.OrderBy(x => x.Id).Last().Id + 1
                };
                _context.Students.Add(s);
                _context.SaveChanges();
                return "Success, user registered.";
            }
            return "Error, failed to create role.";
        }
        return "Error, user already exists.";
    }

    [HttpPost("AddTutor")]
    [Authorize(Roles = "Tutor, Admin")]
    public async Task<string> AddTutorAsync(String username)
    {
        if (await _userManager.FindByNameAsync(username) == null)
        {
            return "Error, user does not exist";
        }

        ApiUser apiUser = _context.ApiUsers.Where(x => x.ExternalId == username).First();
        if (apiUser == null) { return "Error, internal user does not exist."; }
        apiUser.IsTutor = true;
        _context.ApiUsers.Update(apiUser);
        String fname = "";
        String lname = "";
        Student? s = null;
        Admin? a = null;
        try
        {
            s = await _context.Students.Where(x => x.UserId == apiUser.UserId).FirstAsync();
        }
        catch { }
        try
        {
            a = await _context.Admins.Where(x => x.UserId == apiUser.UserId).FirstAsync();
        }
        catch { }
        if (s != null)
        {
            fname = s.FirstName; lname = s.LastName;
        }
        else if (a != null)
        {
            fname = a.FirstName; lname = a.LastName;
        }
        else
        {
            return "Error, failed find existing Student or Admin";
        }
        Models.Tutor t = new()
        {
            Id = _context.Tutors.OrderBy(x => x.Id).Select(x => x.Id).Last() + 1,
            UserId = apiUser.UserId,
            FirstName = fname,
            LastName = lname,
        };
        _context.Tutors.Add(t);
        _context.SaveChanges();
        return "Success, added tutor privliges to user.";
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

    [HttpGet("testtutor")]
    [Authorize(Roles = Roles.Tutor)]
    public string TestTutor()
    {
        return "Authorized as Tutor";
    }

    [HttpGet("teststudent")]
    [Authorize(Roles = Roles.Student)]
    public string TestStudent()
    {
        return "Authorized as Student";
    }

    [HttpPost("getName")]
    [Authorize]
    public string GetName(string username)
    {
        if (username == null) { return ""; }
        else 
        {
            Student? s = null;
            try
            {
                s = _context.Students.Where(x => x.Email == username).FirstOrDefault();
            } catch { }
            if (s != null) 
            {
                return s.FirstName; 
            }
            int i = _context.ApiUsers.Where(x => x.ExternalId == username).First().UserId;
            Models.Tutor? t = null;
                try { _context.Tutors.Where(x => x.UserId == i).First(); }
            catch { }
            if (t != null)
            {
                return t.FirstName;
            }
            Admin? a = null;
            try
            {
                a = _context.Admins.Where(x => x.UserId == i).First();
            }catch { }
            if (a != null)
            {
                return a.FirstName;
            }
        }
        return "";
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