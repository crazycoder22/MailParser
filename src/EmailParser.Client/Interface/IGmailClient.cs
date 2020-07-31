using MailParser.Common;
using System.Collections.Generic;

namespace EmailParser.Client.Interface
{
    public interface IGmailClient
    {
        List<EmailMessage> ReadMessage();
    }
}