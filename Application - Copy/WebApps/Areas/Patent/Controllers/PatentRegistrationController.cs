using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApps.Areas.Patent.Controllers
{
    public class PatentRegistrationController : Controller
    {
        // GET: Patent/PatentRegistration
        public ActionResult Index()
        {
            return View();
        }
    }
}