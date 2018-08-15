using PillarBox.Business.Services.Smtp;
using PillarBox.Data;
using PillarBox.Data.Filters;
using PillarBox.Data.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PillarBox.Business.Services.Filters
{
    public class MessageFiltering
    {
        PillarBoxContext _dbContext;
        IMessageContext _context;

        public MessageFiltering(PillarBoxContext dbContext, IMessageContext context)
        {
            _dbContext = dbContext;
            _context = context;
        }

        public void FilterMessage(Message message, Dictionary<string,string> contextVariables)
        {
            var matchedFilters = _dbContext.MessageFilters.ToList().Where(filter => FilterApplies(filter, contextVariables));

            var actions = matchedFilters.SelectMany(f => f.Rule.Actions).ToList();

            actions.ForEach(action => {
                
                // TODO handle actions here!

            });
            
        }

        public bool FilterApplies(MessageFilter filter, Dictionary<string, string> contextVariables)
        {
            if (contextVariables.ContainsKey(filter.FieldName))
            {
                var value = contextVariables[filter.FieldName];

                string pattern = filter.Pattern;

                if (!filter.IsRegularExpression)
                {
                    // wildcard filter
                    pattern = $"^{Regex.Escape(pattern).Replace(@"\*", ".*?")}$";
                }

                return Regex.IsMatch(value, pattern);
            }
            return false;
        }
    }
}
