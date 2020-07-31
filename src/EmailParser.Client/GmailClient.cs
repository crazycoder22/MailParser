using EmailParser.Client.Interface;
using MailKit;
using MailKit.Net.Imap;
using MailParser.Common;
using System.Collections.Generic;

namespace EmailParser.Client
{
    public class GmailClient : IGmailClient
    {
        private ImapSetting _imapSetting;

        public GmailClient(ImapSetting imapSetting)
        {
            _imapSetting = imapSetting;
        }

        public GmailClient()
        {

        }
        public List<EmailMessage> ReadMessage()
        {
            var messages = new List<EmailMessage>();
            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, en) => true;
                client.Connect(_imapSetting.Server, _imapSetting.Port, true);
                client.Authenticate(_imapSetting.UserName, _imapSetting.Password);
                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly | FolderAccess.None);

                for (int i = 0; i < inbox.Count; i++)
                {
                    var message = inbox.GetMessage(i);
                    var subject = message.Subject.Split(",");
                    var body = message.TextBody.Split(",");
                    messages.Add(
                        new EmailMessage
                        {
                            RequestId = subject[2],
                            Comment = body[1]
                        });
                }

                client.Disconnect(true);

                return messages;
            }
        }
    }
}
