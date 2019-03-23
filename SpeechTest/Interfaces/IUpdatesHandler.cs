using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechTest
{
    public interface IUpdatesHandler<T>
    {
        bool CanHandle(T message, IVityaBot bot);
        Task<bool> HandleAsync(T message, IVityaBot bot);
    }
}
