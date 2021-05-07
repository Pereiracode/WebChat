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
        private readonly IProfanityFilter _profanityFilter;

        public ChatHub(IMessageService messageService, IProfanityFilter profanityFilter)
        {
            _messageService = messageService;
            _profanityFilter = profanityFilter;
        }

        public async Task SendMessage(User user, string message, int receiverID)
        {

            //string message2 = ValidaMensagem(message);
            string message2 = _profanityFilter.Filter(message);

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
