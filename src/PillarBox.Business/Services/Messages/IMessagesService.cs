using System;
using System.Threading.Tasks;
using PillarBox.Business.Models;
using PillarBox.Business.Services.Common;
using PillarBox.Data.Messages;

namespace PillarBox.Business.Services.Messages
{
    public interface IMessagesService : IEntityService<Message>
    {
        Task<PaginatedList<Message>> GetMessagesForInbox(Guid inboxId, int pageIndex, int pageSize);
    }
}