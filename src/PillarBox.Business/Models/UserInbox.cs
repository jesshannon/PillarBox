using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PillarBox.Business.Models
{
    public class UserInbox
    {
        public Inbox Inbox { get; set; }
        public ICollection<UserInbox> Children { get; set; } = new HashSet<UserInbox>();
        public int MessageCount { get; set; }
        public int CountAll
        {
            get
            {
                return MessageCount + Children.Sum(c => c.CountAll);
            }
        }
        public bool Starred { get; set; }
    }
}
