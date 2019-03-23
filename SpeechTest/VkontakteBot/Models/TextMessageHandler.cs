using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest.VkontakteBot.Models
{
    public class TextMessageHandler : IUpdatesHandler<IIncomingMessage>  
    {
        public bool CanHandle(IIncomingMessage message, IVityaBot bot)
        {
            return !string.IsNullOrEmpty(message.text);
        }
        public Task<bool> HandleAsync(IIncomingMessage message, IVityaBot bot)
        {
            return bot.SendMessageAsync(new OutgoingMessage() { peer_id = message.peer_id, message = string.Format("Все говорят {0}, а ты пошел на хуй",message.text) });
        }
    }
}
