using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Messages
{
    public class MessageSummaryModel
    {
        public string Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Intro { get; set; }
        public DateTime DateSent { get; set; }
        public bool IsNew { get; set; }
        public string InboxId { get; set; }
    }
}
