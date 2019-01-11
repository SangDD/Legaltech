namespace WebApps.Areas.Account.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;
    using AppStart;
    using BussinessFacade;
    using BussinessFacade.ModuleUsersAndRoles;
    using Common;
    using Common.CommonData;
    using Common.Helpers;
    using Common.MessageCode;
    using ObjectInfos;
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
        [HttpGet]
        [Route("login")]
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
                int pReturn = objBL.RegisterInsert(pRegisInfo);
                return Json(new { status = pReturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -2 });
            }
        }

        [HttpPost]
        [Route("dang-nhap")]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password, string returnUrl = "")
        {
            if (SessionData.CurrentUser != null)
            {
                return Json(new { redirectTo =  SessionData.CurrentUser.DefaultHomePage });
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
                    if (userBL.CurrentUserInfo.loginfirst == 0 && userName != "SuperAdmin")
                    {
                        if (WebApps.Session.SessionData.CurrentUser.Type == (int)CommonEnums.UserType.Customer)
                        {
                            urlContinue = "/customer/quan-ly-customer/get-view-to-edit-customer/" + userBL.CurrentUserInfo.Id.ToString();
                        }
                        else if (WebApps.Session.SessionData.CurrentUser.Type == (int)CommonEnums.UserType.Lawer)
                        {
                            urlContinue = "/luat-su/quan-ly-luat-su/get-view-to-edit-lawer/" + userBL.CurrentUserInfo.Id.ToString();
                        }
                        else
                        {
                            urlContinue = "/quan-tri-he-thong/quan-ly-nguoi-dung/get-view-to-edit-user/" + userBL.CurrentUserInfo.Id.ToString();
                        }
                    }

                    if (language != "VI_VN")
                    {
                        result.MessageCode = KnMessageCode.LoginSuccess_En;
                    }

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
        [HttpGet]
        [Route("dang-xuat")]
        public ActionResult Logout()
        {
            SessionData.CurrentUser = null;
            Session.Abandon();
            return this.Redirect("/");
        }

        [HttpGet]
        [Route("fogot-pass")]
        [AllowAnonymous]
        public ActionResult Fogot_Pass()
        {
            return View("~/Areas/Account/Views/Account/_Partial_ViewSendPass.cshtml");
        }

        [HttpPost]
        [Route("do-fogot-pass")]
        [AllowAnonymous]
        public ActionResult Do_Fogot_Pass(string gmail)
        {
            bool _ck = false;
            try
            {
                UserInfo _user = new UserInfo();
                UserBL _userBL = new UserBL();
                _user = _userBL.GetBy_Email(gmail);
                List<string> a = new List<string>();

                if (_user.Id == 0)
                {
                    return Json(new { success = -2 });
                }

                DateTime currentTime = DateTime.Now;
                DateTime current = currentTime.AddMinutes(5);

                string _nd_confirm = current.ToString("ddMMyyyy HH:mm") + "_" + _user.Id;

                _nd_confirm = WebApps.CommonFunction.AppsCommon.EncryptString(_nd_confirm);
                //string key_EncryptString = WebApps.CommonFunction.AppsCommon.DecryptString(_nd_confirm);
                string link = Configuration.LinkPathlaw + "/vi-vn/quen-mat-khau/thong-bao?confirm=" + _nd_confirm;
                string content = GetContentMail(link);
                _ck = EmailHelper.SendMail(_user.Email, "", "Đặt lại mật khẩu", content, a);
                return Json(new { success = 1 });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return Json(new { success = _ck });
        }

        public string GetContentMail(string link)
        {
            string title = EmailHelper.EmailOriginal.DisplayName;
            string content = "Chào bạn, Để đặt lại mật khẩu, bạn cần bấm vào link liên kết bên dưới. Thao tác này sẽ giúp bạn thay đổi mật khẩu.";
            return "<div>" +
                "<div>" + title + "</div>" +
                 "<div>" + content + "</div>" +
                  "<div>" + link + "</div>" +
                   "<div>Cảm ơn.</div>" +
                "</div>";

        }
    }
}