namespace WebApps.Areas.Account.Controllers
{
	using System;
	using System.Web.Mvc;
	using AppStart;
	using BussinessFacade;
	using CommonData;
    using RequestFilter;

	[ValidateAntiForgeryTokenOnAllPosts]
	[RouteArea("Account", AreaPrefix = "")]
	[Route("{action}")]
	public class AccountController : Controller
	{
		// GET: Account/Login
		[HttpGet][Route("")]
		[AllowAnonymous]
		public ActionResult Login(string returnUrl = "")
		{
			if (SessionData.CurrentUser != null)
			{
				return this.Redirect(SessionData.CurrentUser.DefaultHomePage);
			}

			ViewBag.returnUrl = returnUrl;
			return View();
		}

		[HttpPost][Route("dang-nhap")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password)
		{
			if (SessionData.CurrentUser != null)
			{
				return Json(new { redirectTo = SessionData.CurrentUser.DefaultHomePage });
			}

			var userBL = new UserBL();
			try
			{
				userBL.DoLoginAccount(userName, password);
				if (userBL.IsLoginSuccess)
				{
					var ipAddress = HttpHelper.GetClientIPAddress(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
					FileHelper.WriteFileLogin(CommonVariables.KnFileLogin, userName, ipAddress);

					SessionData.CurrentUser = userBL.CurrentUserInfo;
					SessionData.CurrentUser.DefaultHomePage = IdentityRequest.GetDefaultPageForAccountLogged();

                    return Json(new { userBL.LoginResult, urlContinue = SessionData.CurrentUser.DefaultHomePage });
				}
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { userBL.LoginResult });
		}

		[AllowAnonymous]
		[HttpGet][Route("dang-xuat")]
		public ActionResult Logout()
		{
			SessionData.CurrentUser = null;
			Session.Abandon();
			return this.Redirect("/");
		}
	}
}