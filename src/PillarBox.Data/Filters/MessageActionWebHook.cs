using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Filters
{
    public class MessageActionWebHook : MessageAction
    {
        public string TargetUrl { get; set; }

        public string PostTemplate { get; set; }
    }
}
