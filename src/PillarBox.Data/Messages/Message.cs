using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Messages
{
    public class Message : EntityBase
    {
        public Inbox Inbox { get; set; }
        public Guid InboxId { get; set; }

        public DateTime DateSent { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }

        public string Summary { get; set; }

        public bool HasAttachments { get; set; }

        public string Source { get; set; }

        public string CreatedByClientIP { get; set; }

        public string CreatedByClientHostname { get; set; }
    }
}
