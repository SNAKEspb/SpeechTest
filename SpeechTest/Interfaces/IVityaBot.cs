using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest
{
    public interface IVityaBot
    {
        Task<IRegisterResponse> AuthorizeAsync();
        Task<IUpdatesResponse> GetUpdatesAsync();
        Task<bool> SendMessageAsync(IOutgoingMessage message);
        //сюда добавить методы заливания аттачей? картинок / аудио или хуйнують логику прямо в сенд мессаж
    }
}
