using Microsoft.AspNetCore.SignalR;

namespace Edison.GameHub
{
    public class GameHub : Hub
    {
        public async Task Send(string user, string message)
        {
            Console.WriteLine("user is "+ user + " message "+ message);
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}