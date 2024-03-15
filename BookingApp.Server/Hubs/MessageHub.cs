using BookingApp.Server.Database;
using BookingApp.Server.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Server.Hubs
{
    public class MessageHub : Hub
    {
        private readonly DatabaseContext _context;
        public MessageHub(DatabaseContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string sender, string receiver, string message)
        {
            Chat chat = new Chat {
                SenderId = int.Parse(sender),
                ReceiverId = int.Parse(receiver),
                Text = message
            };
            _context.Entry(chat).State = EntityState.Added;
            await _context.SaveChangesAsync();
            await Clients.All.SendAsync("ReceiveMessage", sender);
            await Clients.All.SendAsync("ReceiveMessage", receiver);
        }
    }
}
