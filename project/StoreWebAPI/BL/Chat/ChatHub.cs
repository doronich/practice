using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BL.Chat
{
    public class ChatHub : Hub
    {
        public async Task Send(string message,string username,string role)
        {
            await this.Clients.All.SendAsync("Send", message, username);
        }
    }
}
