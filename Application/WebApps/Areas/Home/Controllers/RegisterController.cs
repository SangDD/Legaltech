using BussinessFacade.ModuleUsersAndRoles;
using Common;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApps.AppStart;
using WebApps.Session;

namespace WebApps.Areas.Home.Controllers
{
    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("home", AreaPrefix = "application")]
    [Route("{action}")]
    public class RegisterController : Controller
    {
        [HttpGet]
        [Route("register/{id}")]
        public ActionResult Register_Country()
        {
            try
            {
                if (SessionData.CurrentUser == null)
                {
                    return this.Redirect("/");
                } 

                string _Country = "";
                if (RouteData.Values.ContainsKey("id"))
                {
                    _Country = RouteData.Values["id"].ToString().ToUpper();
                }
                ViewBag.Country = _Country;

                UserBL objBL = new UserBL();
                List<FunctionInfo> functionInfos = objBL.Get_lst_app_right(SessionData.CurrentUser.Id, _Country).OrderBy(x => x.Position).ToList();
                ViewBag.Lst_Function = functionInfos;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View();
        }
    }
}