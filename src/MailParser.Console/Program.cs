using EmailParser.Client.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MailParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            var contentRootPath = new FileInfo(location.AbsolutePath).Directory.FullName;
            var builder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile($"{contentRootPath}\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            var configuration = builder.Build();

            Startup startup = new Startup();
            IServiceCollection services = new ServiceCollection();
            startup.ConfigureServices(services);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var gmailClient = serviceProvider.GetService<IGmailClient>();
            var lmsClient = serviceProvider.GetService<ILMSClient>();
            var lmsApiEndpoint = configuration.GetSection("LMSApiEndpoint").Get<string>();
            while (true)
            {
                var messages = gmailClient.ReadMessage();
                foreach (var message in messages)
                {
                    lmsClient.Send(lmsApiEndpoint, message);
                }

                Thread.Sleep(2000);
            }
        }
    }


}
