using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApps.Areas.Home.Controllers
{
    [Route("wiki")]
    public class WikiController : Controller
    {
        // GET: Home/Wiki

        [HttpGet]
        public ActionResult List()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return View("/Areas/Home/Views/Wiki/List.cshtml");
        }
    }
}