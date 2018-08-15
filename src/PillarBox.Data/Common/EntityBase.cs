using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Common
{
    public abstract class EntityBase
    {
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }
        
    }
}
