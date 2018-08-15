using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PillarBox.Business.Services.Messages;
using PillarBox.Web.Models;
using PillarBox.Web.Models.Messages;
using PillarBox.Web.Utils;

namespace PillarBox.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Inbox")]
    [TypescriptGenerate]
    public class InboxController : Controller
    {
        IMapper _mapper;
        IInboxService _inboxService;
        IMessagesService _messagesService;

        public InboxController(IMapper mapper, IInboxService inboxService, IMessagesService messagesService)
        {
            _mapper = mapper;
            _inboxService = inboxService;
            _messagesService = messagesService;
        }

        [Route("[action]")]
        public async Task<ViewInboxModel> Get(string id, int pageIndex = 0)
        {
            var inbox = _inboxService.GetById(Guid.Parse(id),
                // ugly way to get EF to recurse the parents so we can show the path to the inbox
                i=>i.ParentInbox.ParentInbox.ParentInbox.ParentInbox.ParentInbox.ParentInbox.ParentInbox);

            var mappedInbox = _mapper.Map<ViewInboxModel>(inbox);
            var messages = await _messagesService.GetMessagesForInbox(inbox.Id, pageIndex, 10);
            mappedInbox.Messages = _mapper.Map<PaginatedListModel<MessageSummaryModel>>(messages);

            return mappedInbox;
        }

        [Route("[action]")]
        public IList<UserInboxModel> GetInboxTree()
        {
            var inboxes = _inboxService.GetRootInboxes();

            return inboxes.Select(i => _mapper.Map<UserInboxModel>(i)).ToList();
        }

        [Route("[action]")]
        public string SetStarred(Guid id, bool isStarred)
        {
            _inboxService.SetStar(id, isStarred);
            return "ok";
        }

        [Route("[action]")]
        public string Delete(Guid id)
        {
            _inboxService.Delete(id);
            return "ok";
        }
        
    }
}
