using AutoMapper.Configuration.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Messages
{
    public class UserInboxModel
    {
        public string InboxName { get; set; }
        
        public string InboxId { get; set; }
        
        public string InboxParentInboxId { get; set; }

        public List<UserInboxModel> Children { get; set; }

        public int MessageCount { get; set; }

        public int CountAll { get; set; }

        public bool Starred { get; set; }

        /// <summary>
        /// only used within typescript
        /// </summary>
        public string FullPath { get; set; }
    }
}
