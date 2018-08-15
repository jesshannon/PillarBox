using PillarBox.Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Messages
{
    public class ViewInboxModel
    {
        public string Id { get; set; }

        public string ParentPath { get; set; }

        public string Name { get; set; }

        public PaginatedListModel<MessageSummaryModel> Messages { get; set; }
    }
}
