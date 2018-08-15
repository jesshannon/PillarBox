using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Users
{
    public class User : EntityBase
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
