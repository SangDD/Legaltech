namespace WebApps.Areas.Account.Controllers
{
	using System;
	using System.Web.Mvc;
	using AppStart;
	using BussinessFacade;
	using BussinessFacade.ModuleUsersAndRoles;
	using Common;
	using Common.CommonData;
	using Common.Helpers;
    using ObjectInfos.ModuleUsersAndRoles;
    using RequestFilter;
	using Session;
    using WebApps.CommonFunction;


    [ValidateAntiForgeryTokenOnAllPosts]
	[RouteArea("Account", AreaPrefix = "")]
	[Route("{action}")]
	public class AccountController : Controller
	{
		// GET: Account/Login
		[HttpGet][Route("login")]
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

        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        [ValidateInput(false)]
        public ActionResult Register(RegisterInfo pRegisInfo)
        {
            try
            {
                UserBL objBL = new UserBL();
                int pReturn  = objBL.RegisterInsert(pRegisInfo);
                return Json(new { status = pReturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -2 });
            }
        }

        [HttpPost][Route("dang-nhap")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password, string returnUrl = "")
		{
			if (SessionData.CurrentUser != null)
			{
				return Json(new { redirectTo = SessionData.CurrentUser.DefaultHomePage });
			}
            string language = AppsCommon.GetCurrentLang();
            var result = new ActionBusinessResult();
			try
            {
                var userBL = new UserBL();
				result = userBL.DoLoginAccount(userName, password, language);
				if (result.IsActionSuccess)
				{
					var ipAddress = HttpHelper.GetClientIPAddress(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
					FileHelper.WriteFileLogin(CommonVariables.KnFileLogin, userName, ipAddress);
                    userBL.CurrentUserInfo.Language = language;
                    SessionData.CurrentUser = userBL.CurrentUserInfo;
					SessionData.CurrentUser.DefaultHomePage = IdentityRequest.GetDefaultPageForAccountLogged();
                    SessionData.CurrentUser.CurrentDate = CommonFuc.TruncDate();

                    var urlContinue = SessionData.CurrentUser.DefaultHomePage;
					if (!string.IsNullOrEmpty(returnUrl)) urlContinue = returnUrl;

                    return Json(new { result = result.ToJson(), urlContinue });
				}
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return Json(new { result = result.ToJson() });
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