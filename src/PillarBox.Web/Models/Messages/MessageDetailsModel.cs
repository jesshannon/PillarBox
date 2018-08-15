using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Messages
{
    public class MessageDetailsModel
    {
        public string Id { get; set; }
        public string InboxId { get; set; }

        public DateTime DateSent { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string Subject { get; set; }

        public string From { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }

        public int SizeBytes { get; set; }
    }
}
