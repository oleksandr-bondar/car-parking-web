using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAppCarParking.Controllers
{
    [Produces("application/json")]
    [Route("api/help")]
    public class HelpController : Controller
    {
        // GET: api/Help
        [HttpGet]
        public ActionResult Get()
        {
            return Content(Helper.GetHelp());
        }
    }
}
