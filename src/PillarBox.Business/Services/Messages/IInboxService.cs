using PillarBox.Business.Models;
using PillarBox.Business.Services.Common;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillarBox.Business.Services.Messages
{
    public interface IInboxService : IEntityService<Inbox>
    {
        Inbox GetInboxByPath(IEnumerable<string> path);
        IList<UserInbox> GetRootInboxes();
        IEnumerable<string> TokenizeString(string path, Dictionary<string, string> contextVariables);
        void SetStar(Guid inboxId, bool isStarred);
    }
}
