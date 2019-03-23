using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Speech.Synthesis;
using SpeechTest.VkontakteBot.Models;
using System.IO;

namespace SpeechTest
{
    class Program
    {
        static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        static IVityaBot bot = null;
        static List<IUpdatesHandler<IIncomingMessage>> updatesHandler = new List<IUpdatesHandler<IIncomingMessage>>()
        {
            new TextMessageHandler(),
            new PhotoMessageHandler(),
            new AudioMessageHandler(),
            new WallMessageHandler(),
        };
        static bool Processing = false;

        static void Main(string[] args)
        {
            SpeechSynthesizer speaker = new SpeechSynthesizer();
            //speaker.SelectVoice("ScanSoft Katerina_Full_22kHz");
            speaker.Rate = 1;
            speaker.Volume = 100;
            // speaker.Speak("vitia pidor"); // надо поставить русский езык(в распозновании речи, можно скачать у мелкософта, но у меня не получилось :( из-за x64 винды и через стрим заливать как музычку

            NLog.LogManager.LoadConfiguration(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "//NLog.config");

            _logger.Info("Hello World");
            bot = new VkBot(_logger);
            Start();

            Console.Read();
        }

        static async Task Start()
        {
            var registerResponse = await bot.AuthorizeAsync();
            if (registerResponse.Success)
            {
                //Timer timer = new Timer(
                //                    new TimerCallback(async (param) => {
                //                        await ProcessMessagesAsync(bot, updatesHandler);
                //                    }), null, 0, 5000);

                while (true) {
                    await ProcessMessagesAsync(bot, updatesHandler);
                    await Task.Delay(5000);
                }

            }
        }

        static async Task ProcessMessagesAsync(IVityaBot bot, IEnumerable<IUpdatesHandler<IIncomingMessage>> handlers)
        {
            try
            {
                if (Processing)
                    return;
                _logger.Log(NLog.LogLevel.Info, "ProcessMessagesStart");
                Processing = true;
                var updatesResult = await bot.GetUpdatesAsync();
                foreach (var message in updatesResult.Updates)
                {
                    foreach (var handler in handlers)
                    {
                        if (handler.CanHandle(message, bot))
                            await handler.HandleAsync(message, bot);
                    }
                }
                Processing = false;
            }
            catch (Exception e)
            {
                Processing = false;
                _logger.Log(NLog.LogLevel.Error, e);
            }
        }
    }
}
