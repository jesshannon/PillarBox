using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Messages
{
    public class InboxModel
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string ParentInboxId { get; set; }

        public List<InboxModel> Children { get; set; }
    }
}
