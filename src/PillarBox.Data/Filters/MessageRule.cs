using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Filters
{
    public class MessageRule : EntityBase
    {
        public string Name { get; set; }

        public ICollection<MessageFilter> Filters { get; set; } = new HashSet<MessageFilter>();

        public ICollection<MessageAction> Actions { get; set; } = new HashSet<MessageAction>();

    }
}
