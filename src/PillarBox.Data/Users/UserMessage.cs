using PillarBox.Data.Common;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Users
{
    public class UserMessage : EntityBase
    {
        public User User { get; set; }
        public Guid UserId { get; set; }

        public Message Message { get; set; }
        public Guid MessageId { get; set; }

        public bool Starred { get; set; }

    }
}
