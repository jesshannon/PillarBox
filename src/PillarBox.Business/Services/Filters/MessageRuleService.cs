using PillarBox.Business.Services.Common;
using PillarBox.Data;
using PillarBox.Data.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Business.Services.Filters
{
    public class MessageRuleService : EntityService<MessageRule>, IMessageRuleService
    {
        public MessageRuleService(PillarBoxContext dbContext) : base(dbContext)
        {

        }
    }
}
