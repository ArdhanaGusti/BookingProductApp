using BCrypt.Net;
using BookingApp.Server.Database;
using BookingApp.Server.Models;
using BookingApp.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookingApp.Server.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly DatabaseContext _context;

        public AuthController(IConfiguration configuration, DatabaseContext context)
        {
            _config = configuration;
            _context = context;
        }

        [HttpPost("Register")]
        public IResult Register(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    _context.Entry(user).State = EntityState.Added;
                    _context.SaveChanges();
                    return Results.Ok("Successfully Register");
                } 
                else
                {
                    return Results.BadRequest("Input not valid");
                }
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost("Login")]
        public IResult Login([FromBody]LoginRequest request)
        {
            var userFound = _context.User.FirstOrDefault(x => x.Email == request.Email);
            if (userFound != null && BCrypt.Net.BCrypt.Verify(request.Password, userFound.Password))
            {
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];
                var key = Encoding.ASCII.GetBytes
                (_config["Jwt:Key"]!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("Id", userFound.Id.ToString()),
                        new Claim(ClaimTypes.NameIdentifier, userFound.UserName),
                        new Claim(ClaimTypes.Email, userFound.Email),
                        new Claim(ClaimTypes.Name, userFound.FullName),
                        new Claim(ClaimTypes.Role, "admin")
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    Issuer = issuer,
                    Audience = audience,
                    SigningCredentials = new SigningCredentials
                    (new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha512Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return Results.Ok(jwtToken);
            }
            return Results.Unauthorized();
        }

        [HttpGet("User")]
        [Authorize(Roles = "admin")]
        public IResult GetUserLogin()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
            string? tokenHeader = headerValues.FirstOrDefault();
            string[] tokenSplit = tokenHeader!.Split(' ');
            string token = tokenSplit[1];
            var tokenHandler = new JwtSecurityTokenHandler();
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (_config["Jwt:Key"]!);
            SecurityToken validatedToken;
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
            };

            try
            {
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                var claims = ((JwtSecurityToken)validatedToken).Claims;

                // Access specific claims for authorization (e.g., user ID, role)
                var userId = claims.FirstOrDefault(c => c.Type == "Id")?.Value;
                var fullName = claims.FirstOrDefault(c => c.Type == "Fullname")?.Value;

                return Results.Ok(claims);
            }
            catch (SecurityTokenException ex)
            {
                return Results.BadRequest();
            }
        }
    }
}
