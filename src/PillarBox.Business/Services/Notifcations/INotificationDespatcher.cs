using PillarBox.Business.Models;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PillarBox.Business.Services.Notifcations
{
    public interface INotificationDespatcher
    {
        Task Notify(Message message);
        Task UpdateInboxes(IList<UserInbox> list);
    }
}
