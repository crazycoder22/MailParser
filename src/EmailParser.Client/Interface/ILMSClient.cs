using MailParser.Common;
using System.Threading.Tasks;

namespace EmailParser.Client.Interface
{
    public interface ILMSClient
    {
        Task<string> Send(string url, EmailMessage message);
    }
}