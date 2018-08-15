using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PillarBox.Business.Models;
using PillarBox.Business.Services.Notifcations;
using PillarBox.Data.Messages;
using PillarBox.Web.Models.Messages;
using PillarBox.Web.Models.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Controllers
{
    public class MessageHub : Hub
    {
        static ConcurrentDictionary<string, string> subscriptions = new ConcurrentDictionary<string, string>();

        public async Task subscribeInbox(string id)
        {
            if (subscriptions.ContainsKey(Context.ConnectionId))
            {
                await Groups.RemoveAsync(Context.ConnectionId, subscriptions[Context.ConnectionId]);
            }
            subscriptions.AddOrUpdate(Context.ConnectionId, id, (k,v)=> v);
            await Groups.AddAsync(Context.ConnectionId, id);
        }
    }

    public class NotifcationDespatcher : INotificationDespatcher
    {
        IHubContext<MessageHub> _hub;
        IMapper _mapper;

        public NotifcationDespatcher(IHubContext<MessageHub> hub, IMapper mapper)
        {
            _hub = hub;
            _mapper = mapper;
        }

        public async Task Notify(Message message)
        {
            // TODO filter this to starred inboxes only
            await _hub.Clients.All.SendAsync("Notify", _mapper.Map<NotificationModel>(message));

            await _hub.Clients.Group(message.InboxId.ToString()).SendAsync("InboxMessage", _mapper.Map<MessageSummaryModel>(message));
        }

        public async Task UpdateInboxes(IList<UserInbox> list)
        {
            await _hub.Clients.All.SendAsync("InboxUpdate", list.Select(i => _mapper.Map<UserInboxModel>(i)).ToList() );
        }
        
    }
}
