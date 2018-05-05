namespace WebApps.Areas.Home.Controllers
{
	using System;
	using System.Web.Mvc;

	using AppStart;
	using Session;

	[ValidateAntiForgeryTokenOnAllPosts]
    public class HomeController : Controller
    {
        // GET: Home/Home
	    [HttpGet][Route("home")]
	    public ActionResult KnHome()
	    {
		    return View("~/Areas/Home/Views/Home/KnHome.cshtml");
	    }

	    [HttpGet][Route("filter-request-not-identity")]
	    public ActionResult FilterRequestNotIdentity(string requestMethod, string isRequestTypeAjax, string urlRedirect, string returnUrl = "")
	    {
		    try
		    {
			    var fullPathResponse = urlRedirect;

			    if (!string.IsNullOrEmpty(returnUrl))
			    {
				    fullPathResponse += "?returnUrl=" + returnUrl;
			    }

			    if (requestMethod.ToUpper() == "POST" || isRequestTypeAjax?.ToUpper() == "TRUE")
			    {
				    if (urlRedirect == RouteConfig.KnAccessDenied)
				    {
					    return this.Json(new { redirectTo = RouteConfig.KnAccessDeniedShortern, dataInTab = true }, JsonRequestBehavior.AllowGet);
				    }

				    return this.Json(new { redirectTo = fullPathResponse }, JsonRequestBehavior.AllowGet);
			    }

			    if (requestMethod.ToUpper() == "GET")
			    {
				    return Redirect(fullPathResponse);
			    }
		    }
		    catch (Exception)
		    {
			    // Ignored
		    }

		    return Json(new { redirectTo = RouteConfig.KnHttpNotFound });
	    }

	    [HttpGet][Route("about-us")]
	    public ActionResult About()
	    {
		    ViewBag.Message = "Your application description page.";

		    return View("~/Areas/Home/Views/Home/About.cshtml");
	    }

	    [HttpGet][Route("contact")]
	    public ActionResult Contact()
	    {
		    ViewBag.Message = "Your contact page.";

		    return View("~/Areas/Home/Views/Home/Contact.cshtml");
	    }

	    [HttpGet][Route("page-not-found")]
	    public ActionResult PageNotFound()
	    {
		    return View("~/Areas/Home/Views/Home/PageNotFound.cshtml");
	    }

	    [HttpGet][Route("access-denied")]
	    public ActionResult AccessDenied()
	    {
		    return View("~/Areas/Home/Views/Home/AccessDenied.cshtml");
	    }

	    [HttpGet][Route("re-login")]
	    public ActionResult ForceRelogin()
	    {
		    SessionData.CurrentUser = null;
		    Session.Abandon();
		    return View("~/Areas/Home/Views/Home/ForceRelogin.cshtml");
	    }

	    [HttpGet][Route("account-session-invalid")]
	    public ActionResult AccountSessionInvalid()
	    {
		    SessionData.CurrentUser = null;
		    Session.Abandon();
		    return View("~/Areas/Home/Views/Home/AccountSessionInvalid.cshtml");
	    }
    }
}