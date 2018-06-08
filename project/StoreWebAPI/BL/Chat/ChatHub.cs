using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace BL.Chat {
    public class ChatHub : Hub {
        public async Task SendMessage(string username, string message) {
            await this.Clients.All.SendAsync("Send", username, message);
        }

        public async Task SendTyping(string username) {
            await this.Clients.Others.SendAsync("StartTyping", username);

        }

        public async Task DeleteTyping(string username) {
            await this.Clients.Others.SendAsync("StopTyping", username);
        }

    }
}
