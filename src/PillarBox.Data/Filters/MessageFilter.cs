using PillarBox.Data.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PillarBox.Data.Filters
{
    public class MessageFilter : EntityBase
    {
        public MessageRule Rule { get; set; }
        public Guid RuleId { get; set; }

        public string FieldName { get; set; }

        public string Pattern { get; set; }

        public bool IsRegularExpression { get; set; }
    }
}
