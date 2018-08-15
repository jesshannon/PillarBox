using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Filters
{
    public class MessageActionForward : MessageAction
    {
        public string ForwardingAddress { get; set; }
    }
}
