using BookingApp.Server.Database;
using BookingApp.Server.Models;
using BookingApp.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Data;
using System.Security.Claims;

namespace BookingApp.Server.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly GetUserByJwt _getUserByJwt;

        public UserController(DatabaseContext context, GetUserByJwt getUserByJwt)
        {
            _context = context;
            _getUserByJwt = getUserByJwt;
        }

        [HttpGet("Me")]
        public IResult GetUserLoggedIn()
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
                string? tokenHeader = headerValues.FirstOrDefault();
                var claims = _getUserByJwt.GetClaims(tokenHeader);
                if (claims != null)
                {
                    var userId = claims.FirstOrDefault(x => x.Type == "Id")!.Value;
                    User? user = _context.User.FirstOrDefault(x => x.Id.ToString() == userId);
                    return Results.Ok(user);
                }
                else
                {
                    return Results.BadRequest();
                }
            }
            catch (Exception)
            {
                return Results.Unauthorized();
            }
        }

        [HttpPut("Update")]
        public IResult Update(User user)
        {
            try
            {
                Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
                string? tokenHeader = headerValues.FirstOrDefault();
                var claims = _getUserByJwt.GetClaims(tokenHeader);
                if (claims != null)
                {
                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return Results.Ok("Successfully Update");
                }
                else
                {
                    return Results.BadRequest();
                }
            }
            catch (Exception)
            {
                return Results.Unauthorized();
            }
        }
    }
}
