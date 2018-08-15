using PillarBox.Business.Models;
using PillarBox.Business.Services.Common;
using PillarBox.Data;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PillarBox.Business.Services.Messages
{
    public class MessagesService : EntityService<Message>, IMessagesService
    {
        public MessagesService(PillarBoxContext dbContext) : base(dbContext)
        {

        }

        public async Task<PaginatedList<Message>> GetMessagesForInbox(Guid inboxId, int pageIndex, int pageSize)
        {
            return await PaginatedList<Message>.CreateAsync(
                _dbContext.Messages
                    .Where(m=>m.InboxId == inboxId)
                    .OrderByDescending(m=>m.DateCreated),
                pageIndex,
                pageSize
                );
        }


    }
}
