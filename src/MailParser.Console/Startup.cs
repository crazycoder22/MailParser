using EmailParser.Client;
using EmailParser.Client.Interface;
using MailParser.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;

namespace MailParser.Console
{
    public class Startup
    {
        public Startup()
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            var contentRootPath = new FileInfo(location.AbsolutePath).Directory.FullName;
            var builder = new ConfigurationBuilder()
                .SetBasePath(contentRootPath)
                .AddJsonFile($"{contentRootPath}\\appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var imapSetting = Configuration.GetSection("ImapSetting").Get<ImapSetting>();
            services.AddTransient(x => imapSetting);
            services.AddTransient<IGmailClient, GmailClient>();
            services.AddTransient<ILMSClient, LMSClient>();
        }
    }
}
