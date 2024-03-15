using BookingApp.Server.Database;
using BookingApp.Server.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BookingApp.Server.Controllers.WebApi
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly GetUserByJwt _getUserByJwt;
        public ChatController(GetUserByJwt getUserByJwt, DatabaseContext databaseContext)
        {
            _getUserByJwt = getUserByJwt;
            _context = databaseContext;
        }
        [HttpGet("Conversation")]
        public IResult Conversation()
        {
            Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
            string? tokenHeader = headerValues.FirstOrDefault();
            var claims = _getUserByJwt.GetClaims(tokenHeader);
            if (claims == null)
            {
                return Results.Unauthorized();
            }
            int.TryParse(claims.FirstOrDefault(x => x.Type == "Id")?.Value, out int userId);

            var latestChat = _context.Chat
                                    .GroupBy(chat => new { FirstId = chat.SenderId < chat.ReceiverId ? chat.SenderId : chat.ReceiverId, SecondId = chat.SenderId > chat.ReceiverId ? chat.SenderId : chat.ReceiverId })
                                    .Select(x => new
                                    {
                                        x.Key.FirstId,
                                        x.Key.SecondId,
                                        LastMessage = x.OrderByDescending(e => e.CreatedAt).FirstOrDefault()!.Text,
                                        LastMessageTime = x.Max(item => item.CreatedAt)
                                    })
                                    .Where(x => x.FirstId == userId || x.SecondId == userId)
                                    .ToArray();
            return Results.Ok(latestChat);
        }

        [HttpGet("Chat/{receiverId}")]
        public IResult Chat(int receiverId)
        {
            Request.Headers.TryGetValue("Authorization", out StringValues headerValues);
            string? tokenHeader = headerValues.FirstOrDefault();
            var claims = _getUserByJwt.GetClaims(tokenHeader);
            var userIdString = claims!.FirstOrDefault(c => c.Type == "Id")?.Value;
            bool isSuccess = int.TryParse(userIdString, out int userId);
            if (!isSuccess)
            {
                Results.BadRequest("Failed to get user data");
            }
            var chat = _context.Chat
                .Where(x => (x.SenderId == userId && x.ReceiverId == receiverId) || (x.SenderId == receiverId && x.ReceiverId == userId))
                .ToArray();
            return Results.Ok(chat);
        }
    }
}
