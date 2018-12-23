using System;
using System.Collections.Generic;
using System.Web.Mvc;

using BussinessFacade;
using BussinessFacade.ModuleUsersAndRoles;

using Common;
using Common.CommonData;
using Common.Ultilities;
using ObjectInfos;
using WebApps.AppStart;
using WebApps.Session;

namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("ModuleUsersAndRoles", AreaPrefix = "luat-su")]
    [Route("{action}")]
    public class LawerController : Controller
    {
        [HttpGet]
        [Route("quan-ly-luat-su")]
        public ActionResult ListLawer()
        {
            var lstUsers = new List<UserInfo>();
            return PartialView("~/Areas/ModuleUsersAndRoles/Views/Lawer/ListLawer.cshtml", lstUsers);
        }

        [HttpPost]
        [Route("quan-ly-luat-su/find-lawer")]
        public ActionResult FindUser(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort, string p_type)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                var userBL = new UserBL();
                List<UserInfo> _lst = userBL.User_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<Timesheet_Info>((int)_total_record, p_CurrentPage, "Luật sư");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                return PartialView("~/Areas/ModuleUsersAndRoles/Views/Lawer/_PartialTableListLawer.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/Lawer/_PartialTableListLawer.cshtml");
            }
        }

        [HttpGet]
        [Route("quan-ly-luat-su/get-view-to-add-lawer")]
        public ActionResult GetViewToAddlawer()
        {
            return View("~/Areas/ModuleUsersAndRoles/Views/Lawer/_PartialInsert.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-luat-su/do-add-lawer")]
        public ActionResult DoAddlawer(UserInfo userInfo, string GroupId)
        {
            var result = new ActionBusinessResult();
            try
            {
                var userBL = new UserBL();
                userInfo.CreatedBy = SessionData.CurrentUser.Username;
                result = userBL.AddUser(userInfo, GroupId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = result.ToJson() });
        }

        [HttpGet]
        [Route("quan-ly-luat-su/get-view-to-edit-lawer/{id}")]
        public ActionResult GetViewToEditlawer()
        {
            var userInfo = new UserInfo();
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    int userId = Convert.ToInt32(RouteData.Values["id"].ToString());
                    UserBL _UserBL = new UserBL();
                    userInfo = _UserBL.GetUserById(userId);
                    userInfo.GroupSelectedCollection = UserBL.GetUserSelfGroups(userId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View("~/Areas/ModuleUsersAndRoles/Views/Lawer/_PartialEdit.cshtml", userInfo);
        }

        [HttpPost]
        [Route("quan-ly-luat-su/do-edit-lawer")]
        public ActionResult DoEditlawer(UserInfo userInfo, string GroupId)
        {
            var result = new ActionBusinessResult();
            try
            {
                var userBL = new UserBL();
                userInfo.ModifiedBy = SessionData.CurrentUser.Username;
                result = userBL.EditUser(userInfo, GroupId);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = result.ToJson() });
        }

        [HttpPost]
        [Route("quan-ly-luat-su/do-delete-lawer")]
        public ActionResult DoDeletelawer(int userId)
        {
            var result = new ActionBusinessResult();
            try
            {
                var userBL = new UserBL();
                var modifiedBy = SessionData.CurrentUser.Username;
                result = userBL.DeleteUser(userId, modifiedBy);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = result.ToJson() });
        }

        [HttpGet]
        [Route("quan-ly-luat-su/view-detail-lawer/{id}")]
        public ActionResult ViewDetaillawer()
        {
            var userInfo = new UserInfo();
            try
            {
                if (RouteData.Values["id"] != null && RouteData.Values["id"].ToString() != "")
                {
                    int userId = Convert.ToInt32(RouteData.Values["id"].ToString());
                    UserBL _UserBL = new UserBL();
                    userInfo = _UserBL.GetUserById(userId);
                    userInfo.GroupSelectedCollection = UserBL.GetUserSelfGroups(userId);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return View("~/Areas/ModuleUsersAndRoles/Views/Lawer/_PartialDetail.cshtml", userInfo);
        }
    }
}