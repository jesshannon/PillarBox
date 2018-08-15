using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Messages
{
    public class Inbox : EntityBase
    {
        public string Name { get; set; }

        public Guid? ParentInboxId { get; set; }
        public Inbox ParentInbox { get; set; }

        public ICollection<Inbox> Children { get; set; } = new HashSet<Inbox>();

        public ICollection<Message> Messages { get; set; } = new HashSet<Message>();

        public string CreatedByClientIP { get; set; }

        public string CreatedByClientHostname { get; set; }

        public TimeSpan MaxAge { get; set; }

        public int MaxCount { get; set; }
    }
}
