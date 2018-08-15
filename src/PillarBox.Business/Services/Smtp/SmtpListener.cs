using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PillarBox.Business.Services.Common;
using PillarBox.Data.Config;
using SmtpServer;
using SmtpServer.Authentication;
using SmtpServer.IO;
using SmtpServer.Mail;
using SmtpServer.Protocol;
using SmtpServer.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PillarBox.Business.Services.Smtp
{
    public class SmtpListener : BackgroundService
    {
        private SmtpServer.SmtpServer smtpServer;

        private readonly IServiceScopeFactory _scopeFactory;
        ServerDefaults _serverDefaults;

        public SmtpListener(IServiceScopeFactory scopeFactory, IOptions<ServerDefaults> serverDefaults)
        {
            _scopeFactory = scopeFactory;
            _serverDefaults = serverDefaults.Value;

            var options = new OptionsBuilder()
            .ServerName("localhost")
            .Port(GetSmtpPorts())
            .AuthenticationRequired(false)
            .AllowUnsecureAuthentication(true)
            .UserAuthenticator(new AuthFactory())
            .MessageStore(new SmtpMessageStore())
            .Build();

            smtpServer = new SmtpServer.SmtpServer(options);
            smtpServer.SessionCreated += SmtpServer_SessionCreated;
            smtpServer.SessionCompleted += SmtpServer_SessionCompleted;
        }

        int[] GetSmtpPorts()
        {
           return  _serverDefaults.SmtpPorts.Split(',')
                .Select(s => int.TryParse(s.Trim(), out int x) ? (int?)x : null)
                .Where(p => p != null)
                .Select(p => p.Value)
                .ToArray();
        }

        private void SmtpServer_SessionCreated(object sender, SessionEventArgs e)
        {
            e.Context.Properties["scope"] = _scopeFactory.CreateScope();

            IMessageContext messageContext = ((IServiceScope)e.Context.Properties["scope"]).ServiceProvider.GetRequiredService<IMessageContext>();
            messageContext.IPEndPoint = (IPEndPoint)e.Context.RemoteEndPoint;
        }

        private void SmtpServer_SessionCompleted(object sender, SessionEventArgs e)
        {
            ((IServiceScope)e.Context.Properties["scope"]).Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return smtpServer.StartAsync(stoppingToken);
        }
        
        public class AuthFactory : IUserAuthenticatorFactory
        {
            public IUserAuthenticator CreateInstance(ISessionContext context)
            {
                return new SampleUserAuthenticator();
            }
        }

        public class SampleUserAuthenticator : IUserAuthenticator
        {
            public Task<bool> AuthenticateAsync(ISessionContext context, string user, string password, CancellationToken cancellationToken)
            {
                var authenticate = ((IServiceScope)context.Properties["scope"]).ServiceProvider.GetRequiredService<ISmtpAuthenticate>();

                var result = authenticate.Authenticate(user, password);
                
                return Task.FromResult(result);
            }
        }

        public class SmtpMessageStore : MessageStore
        {
            public IServiceScopeFactory ScopeFactory { get; set; }
            public Microsoft.Extensions.Logging.ILogger Logger { get; set; }

            public override Task<SmtpResponse> SaveAsync(ISessionContext context, IMessageTransaction transaction, CancellationToken cancellationToken)
            {
                var handler = ((IServiceScope)context.Properties["scope"]).ServiceProvider.GetRequiredService<IMessageHandler>();
                var textMessage = (ITextMessage)transaction.Message;
                try
                {
                    handler.HandleMessage(textMessage.Content);
                }
                catch (UnauthorizedAccessException ex)
                {
                    return Task.FromResult(SmtpResponse.AuthenticationFailed);
                }
                catch (Exception ex)
                {
#if DEBUG
                    throw;
#endif
                    return Task.FromResult(SmtpResponse.TransactionFailed);
                }
                
                return Task.FromResult(SmtpResponse.Ok);
            }
        }
    }
}
