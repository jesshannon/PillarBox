using MimeKit;
using PillarBox.Business.Services.Messages;
using PillarBox.Business.Services.Notifcations;
using PillarBox.Data;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PillarBox.Business.Services.Smtp
{
    public class MessageHandler : IMessageHandler
    {
        PillarBoxContext _dbContext;
        IInboxService _inboxService;
        IMessageContext _context;
        INotificationDespatcher _notify;

        public MessageHandler(PillarBoxContext dbContext, IInboxService inboxService, IMessageContext context, INotificationDespatcher notify)
        {
            _dbContext = dbContext;
            _inboxService = inboxService;
            _context = context;
            _notify = notify;
        }

        public void HandleMessage(Stream content)
        {
            var memStream = new MemoryStream();
            content.CopyTo(memStream);
            memStream.Seek(0, SeekOrigin.Begin);
            var messageContent = MimeMessage.Load(memStream);
            memStream.Seek(0, SeekOrigin.Begin);

            var contextVariables = messageContent.Headers.ToDictionary(h => h.Field, h => h.Value);
            
            contextVariables.Add("CLIENT_IP", _context.IPEndPoint.Address.ToString());
            contextVariables.Add("CLIENT_HOST", _context.Hostname);

            var inboxPath = _inboxService.TokenizeString(_context.User, contextVariables);

            contextVariables.Add("INBOX", string.Join("/", inboxPath));

            var inbox = _inboxService.GetInboxByPath(inboxPath);

            var message = new Message()
            {
                Inbox = inbox,
                DateSent = messageContent.Date.LocalDateTime,
                From = messageContent.From.ToString(),
                To = messageContent.To.ToString(),
                Cc = messageContent.Cc.ToString(),
                Bcc = messageContent.Bcc.ToString(),
                HasAttachments = messageContent.Attachments.Any(),
                Source = new StreamReader(memStream).ReadToEnd(),
                Subject = messageContent.Subject,
                Summary = messageContent.TextBody,
                DateCreated = DateTime.Now,
                CreatedByClientIP = _context.IPEndPoint.Address.ToString(),
                CreatedByClientHostname = _context.Hostname
            };


            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();

#pragma warning disable CS4014
            SendNotification(message);
#pragma warning restore CS4014

        }

        async Task SendNotification(Message message) {

            // message alert
            await _notify.Notify(message);

            // inbox status change
            await _notify.UpdateInboxes(_inboxService.GetRootInboxes());
        }
    }
}
