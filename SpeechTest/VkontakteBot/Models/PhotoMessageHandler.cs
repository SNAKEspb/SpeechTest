using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest.VkontakteBot.Models
{
    public class PhotoMessageHandler : IUpdatesHandler<IIncomingMessage>
    {
        public bool CanHandle(IIncomingMessage message, IVityaBot bot)
        {
            return message.attachments.Any(x => x.type == "photo");
        }
        public Task<bool> HandleAsync(IIncomingMessage message, IVityaBot bot)
        {
            return bot.SendMessageAsync(new OutgoingMessage() { peer_id = message.peer_id, message = string.Format("Твоя картинка говно") });
        }
    }
}
