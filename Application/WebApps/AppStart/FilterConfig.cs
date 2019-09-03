namespace WebApps.AppStart
{
    using System;
    using System.Net;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using WebApps.Session;

    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class ValidateAntiForgeryTokenOnAllPosts : AuthorizeAttribute
    {
        private static readonly object s_postAuthorizedLocker = new object();

        private static bool postAuthorized;

        public static void AuthorizedPost()
        {
            lock (s_postAuthorizedLocker)
            {
                postAuthorized = true;
            }
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            try
            {
                var request = filterContext.HttpContext.Request;

                if (SessionData.CurrentUser != null && SessionData.CurrentUser.Type == (decimal)Common.CommonData.CommonEnums.UserType.Customer)
                {
                    if (SessionData.CurrentUser.loginfirst == 0 && request.AppRelativeCurrentExecutionFilePath.Contains("CUSTOMER/QUAN-LY-CUSTOMER/GET-VIEW-TO-EDIT-CUSTOMER") == false &&
                        request.AppRelativeCurrentExecutionFilePath.Contains("ACCOUNT/DANG-XUAT") == false
                        && request.AppRelativeCurrentExecutionFilePath.ToUpper().Contains("CUSTOMER/QUAN-LY-CUSTOMER") == false)
                    {
                        filterContext.Result = new RedirectResult("~/customer/quan-ly-customer/get-view-to-edit-customer/" + SessionData.CurrentUser.Id.ToString());
                    }
                }

                if (SessionData.CurrentUser != null)
                {
                    string language = WebApps.CommonFunction.AppsCommon.GetCurrentLang();
                    string sessionLanguage = SessionData.CurrentUser.Language;
                    if (language != sessionLanguage)
                    {
                        SessionData.CurrentUser.Language = language;

                        var userBL = new BussinessFacade.ModuleUsersAndRoles.UserBL(SessionData.CurrentUser);
                        SessionData.CurrentUser.HtmlMenu = userBL.GetUserHtmlMenu(language);
                    }
                }

                // Only validate POSTs  
                if (request.HttpMethod != WebRequestMethods.Http.Post) return;
                if (postAuthorized)
                {
                    DeAuthorizedPost();
                    return;
                }

                // Ajax POSTs and normal form posts have to be treated differently when it comes  
                // to validating the AntiForgeryToken  
                if (request.IsAjaxRequest())
                {
                    var antiForgeryCookie = request.Cookies[AntiForgeryConfig.CookieName];
                    var cookieValue = antiForgeryCookie?.Value;
                    AntiForgery.Validate(cookieValue, request.Headers["__RequestVerificationToken"]);
                }
                else
                {
                    new ValidateAntiForgeryTokenAttribute().OnAuthorization(filterContext);
                }
            }
            catch (Exception)
            {
                // Ignored
            }
        }

        private void DeAuthorizedPost()
        {
            lock (s_postAuthorizedLocker)
            {
                postAuthorized = false;
            }
        }
    }
}
