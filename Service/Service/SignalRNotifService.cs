using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Service.Service
{
    [Route("/chatHub")]
    public class SignalRNotifService : Hub
    {
        public Task SendMessage(string user, string message)
        {
            return Clients.All.SendAsync("ReceiveMessage", user, message, DateTime.Now.ToString("MM-dd, HH:mm tt"), DateTime.Now.ToString("HH_mm_ss"));
        }

        public Task SendMessageToCaller(string user, string message)
        {
            return Clients.Caller.SendAsync("ReceiveMessage", user, message);
        }

        public Task SendMessageToGroup(string user, string message)
        {
            return Clients.Group("SignalR Users").SendAsync("ReceiveMessage", user, message);
        }
    }
}
