using EmailParser.Client.Interface;
using MailParser.Common;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmailParser.Client
{
    public class LMSClient : ILMSClient
    {
        private HttpClient _httpClient;
        public LMSClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> Send(string url, EmailMessage message)
        {
            var data = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, data);
            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
