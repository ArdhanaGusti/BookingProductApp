using BookingApp.Server.Database;
using BookingApp.Server.Models;
using BookingApp.Server.Utils;
using BookingApp.Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Dynamic;
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
                    var role = _context.Role.FirstOrDefault(x => x.Name == "user");
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    user.RoleId = role!.Id;
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
        public IResult Login(LoginRequest request)
        {
            var userFound = _context.User.Include(x => x.Role).FirstOrDefault(x => x.Email == request.Email);
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
                        new Claim("Username", userFound.UserName),
                        new Claim("Email", userFound.Email),
                        new Claim("Fullname", userFound.FullName),
                        new Claim(ClaimTypes.Role, userFound.Role!.Name)
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

                dynamic obj = new ExpandoObject();
                obj.token = jwtToken;
                obj.expired = tokenDescriptor.Expires;

                return Results.Ok(obj);
            }
            return Results.Unauthorized();
        }
        
    }
}
