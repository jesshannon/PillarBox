using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using PillarBox.Business.Services.Filters;
using PillarBox.Web.Models.Settings;
using PillarBox.Web.Utils;

namespace PillarBox.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Filter")]
    [TypescriptGenerate]
    public class FilterController : Controller
    {
        IMapper _mapper;
        IMessageRuleService _messageRuleService;

        public FilterController(IMapper mapper, IMessageRuleService messageRuleService)
        {
            _mapper = mapper;
            _messageRuleService = messageRuleService;
        }

        [Route("[action]")]
        public async Task<RuleSummary[]> Rules()
        {
            return _messageRuleService
                .GetAll(r => r.Filters, r => r.Actions)
                .OrderBy(r => r.Name)
                .ToList()
                .Select(r => _mapper.Map<RuleSummary>(r))
                .ToArray();
        }
    }
}