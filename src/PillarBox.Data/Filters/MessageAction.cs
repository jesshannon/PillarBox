using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Filters
{
    public abstract class MessageAction : EntityBase
    {
        public MessageRule Rule { get; set; }
        public Guid RuleId { get; set; }
    }
}
