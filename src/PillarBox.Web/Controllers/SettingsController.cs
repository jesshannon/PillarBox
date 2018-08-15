using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PillarBox.Web.Utils;

namespace PillarBox.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Settings")]
    [TypescriptGenerate]
    public class SettingsController : Controller
    {

        //[Route("[action]")]
        //public async Task<SmtpSettings> SmtpSettings()
        //{
        //    return new SmtpSettings();
        //}

    }
}