using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PillarBox.Web.Models.Settings
{
    public class RuleSummary
    {
        public string Name { get; set; }

        public IEnumerable<string> Fields { get; set; }

        public IEnumerable<string> Actions { get; set; }
    }
}
