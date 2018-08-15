using PillarBox.Data.Common;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Users
{
    public class UserInbox : EntityBase
    {
        public User User { get; set; }
        public Guid UserId { get; set; }

        public Inbox Inbox { get; set; }
        public Guid InboxId { get; set; }

        public bool Starred { get; set; }
    }
}
