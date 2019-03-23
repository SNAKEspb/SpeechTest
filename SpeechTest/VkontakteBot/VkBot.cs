using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SpeechTest.VkontakteBot.Models;

namespace SpeechTest
{
    public class VkBot : IVityaBot
    {
        static HttpClient _httpClient = new HttpClient();
        static string _url { get; set; } = "https://api.vk.com/";
        static string _token { get; set; } = "5d711c0cd2689e386490cae8d08d38e8186480330cd1f7a5de4e8fdfb2e02ec4da3881ac991bb0d2fb63e";
        static string _groupId { get; set; } = "180024929";
        static string _apiVersion { get; set; } = "5.92";
        static string _wait { get; set; } = "25";

        static string _key { get; set; }
        static string _ts { get; set; }
        static string _server { get; set; }

        private NLog.Logger _logger;

        public VkBot(NLog.Logger logger)
        {
            _logger = logger;
        }

        public async Task<IRegisterResponse> AuthorizeAsync()
        {
            try
            {
                _logger.Log(NLog.LogLevel.Info, "registerStart");
                var urlBuilder = new UriBuilder(_url)
                {
                    Path = "method/groups.getLongPollServer",
                    Query = $"group_id={_groupId}&access_token={_token}&v={_apiVersion}"
                };
                var response = await _httpClient.GetStringAsync(urlBuilder.Uri);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterResponse>(response);

                _key = result.response.key;
                _ts = result.response.ts;
                _server = result.response.server;

                result.Success = !string.IsNullOrEmpty(_key);
                _logger.Log(NLog.LogLevel.Info, "registerEnd");

                return result;
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "registerErr");
                return new RegisterResponse() { Success = false };
            }
        }

        public async Task<IUpdatesResponse> GetUpdatesAsync()
        {
            try
            {
                _logger.Log(NLog.LogLevel.Info, " GetUpdatesStart");

                int ts = 1;
                int.TryParse(_ts, out ts);
                //ts--;

                var url = $"{_server}?act=a_check&ts={ts}&key={_key}&wait={_wait}&version={_apiVersion}";

                var response = await _httpClient.GetStringAsync(url);

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<UpdateResponse>(response);

                if (result.failed != 0)
                {
                    _logger.Log(NLog.LogLevel.Error, " Error received");
                    switch (result.failed)
                    {
                        case 1:
                            _ts = result.new_ts ?? result.ts;
                            _logger.Log(NLog.LogLevel.Info, " Ts updated");
                            break;
                        case 2:
                        case 3:
                            throw new Exception("Session expired. Reconnect to service.");
                        default:
                            throw new Exception("Unknown error");
                    }
                }
                _ts = result.new_ts ?? result.ts;

                _logger.Log(NLog.LogLevel.Info, " GetUpdatesEnd");

                return result;
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "GetUpdatesErr");
                return null;
            }


            throw new NotImplementedException();
        }


        public async Task<bool> SendMessageAsync(IOutgoingMessage message)
        {
            try
            {
                var tmessage = (OutgoingMessage)message;

                var urlBuilder = new UriBuilder(_url)
                {
                    Path = "method/messages.send",
                    Query = $"group_id={_groupId}&access_token={_token}&v={_apiVersion}"
                };

                var values = new Dictionary<string, string>
            {
                { "random_id", tmessage.random_id},
                { "peer_id", tmessage.peer_id.ToString()},
                { "message", tmessage.message},
               // { "attachment", "364384447" }
            };

                var content = new FormUrlEncodedContent(values);
                var response = await _httpClient.PostAsync(urlBuilder.Uri, content);
                var responseBody = await response.Content.ReadAsStringAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.Log(NLog.LogLevel.Error, e, "send error");
                return false;
            }

        }
    }
}
