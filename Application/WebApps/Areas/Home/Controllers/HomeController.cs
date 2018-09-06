namespace WebApps.Areas.Home.Controllers
{
    using System;
    using System.Web.Mvc;
    using AppStart;
    using Common;
    using Session;
    using WebApps.CommonFunction;
    using BussinessFacade.ModuleUsersAndRoles;
    using ObjectInfos;

    [ValidateAntiForgeryTokenOnAllPosts]
 
    public class HomeController : Controller
    {
        // GET: Home/Home
	    [HttpGet][Route("home")]
	    public ActionResult KnHome()
	    {
            if (SessionData.CurrentUser == null)
            {
                return this.Redirect("/");
            }
            var userBL = new UserBL(SessionData.CurrentUser);
            string language = AppsCommon.GetCurrentLang();
            string sessionLanguage = SessionData.CurrentUser.Language;
            if (language != sessionLanguage)
            {
                SessionData.CurrentUser.Language = language;
                SessionData.CurrentUser.HtmlMenu = userBL.GetUserHtmlMenu(language);
            }
            ViewBag.LanguageCode = language;
            return View("~/Areas/Home/Views/Home/LegalHome.cshtml");
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

        [HttpGet]
        [Route("Language")]
        public ActionResult Language(string culture, string returnUrl)
        {
            try
            {
                if (!string.IsNullOrEmpty(culture))
                {
                    var httpCookie = Request.Cookies["language"];
                    if (httpCookie != null)
                    {
                        var cookie = Response.Cookies["language"];
                        if (cookie != null)
                        {
                            cookie.Value = culture;
                            Response.SetCookie(cookie);
                        }
                        httpCookie.Value = culture;
                    }

                    if (culture.ToUpper() != "EN-GB" && Request.UrlReferrer.ToString().ToLower().Contains("/en-gb/"))
                    {
                        return Redirect(Request.UrlReferrer.ToString().ToLower().Replace("/en-gb/", "/"));
                    }
                    else if (culture.ToUpper() != "VI-VN" && Request.UrlReferrer.ToString().ToLower().Contains("/vi-vn/"))
                    {
                        return Redirect(Request.UrlReferrer.ToString().ToLower().Replace("/vi-vn/", "/"));
                    }
                }

                return Redirect(Request.UrlReferrer.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Redirect("/home.html");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        [Route("CheckSessionTimeOut")]
        public JsonResult CheckSessionTimeOut()
        {
            try
            {
                var msg = new MsgReportServerInfo();
                if (SessionData.CurrentUser == null)
                {
                    msg.Code = "-1";
                    msg.Msg = "Hệ thống đã hết thời gian kết nối, bạn hãy đăng nhập lại.";
                }
                else
                {
                    msg.Code = "0";
                }
                return Json(msg);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                var msg = new MsgReportServerInfo();
                msg.Code = "-1";
                msg.Msg = "Không kết nối được tới máy chủ.";
                return Json(msg);
            }
        }
     
    }

    public class MsgReportServerInfo
    {
        public string Code { get; set; }
        public string Msg { get; set; }
    }
}