using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using PillarBox.Business.Services.Messages;
using PillarBox.Data.Messages;
using PillarBox.Web.Models.Messages;
using PillarBox.Web.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PillarBox.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Message")]
    [TypescriptGenerate]
    public class MessageController : Controller
    {
        IMapper _mapper;
        IInboxService _inboxService;
        IMessagesService _messagesService;

        public MessageController(IMapper mapper, IInboxService inboxService, IMessagesService messagesService)
        {
            _mapper = mapper;
            _inboxService = inboxService;
            _messagesService = messagesService;
        }


        [Route("[action]")]
        public async Task<MessageDetailsModel> Get(string id)
        {
            return _mapper.Map<MessageDetailsModel>(_messagesService.GetById(Guid.Parse(id)));
        }

        [Route("[action]")]
        public string Delete(Guid id)
        {
            _messagesService.Delete(id);
            return "ok";
        }

    }
}
