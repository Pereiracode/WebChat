using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebChat.Domain.Entities;
using WebChat.Services.Interfaces;

namespace WebChat.Application.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public async Task SendMessage(User user, string message, int receiverID)
        {

            string message2 = ValidaMensagem(message);

            var messageSend = new Message
            {
                FkIdSender = user.Id,
                FkIdReceiver = receiverID,
                Text = message2,
                SendDate = DateTime.Now
            };

            _messageService.Save(messageSend);

            await Clients.All.SendAsync("ReceiveMessage", messageSend);
        }

        public string ValidaMensagem(string msg)
        {
            string[] palavrasProibidas = { "arroz", "batata" };


            if (msg.Equals("Banana"))
            {
                msg = "*";
            }

            return msg;
        }

    }
}
