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
    [RouteArea("ModuleUsersAndRoles", AreaPrefix = "customer")]
    [Route("{action}")]
    public class CustomerController : Controller
    {
        [HttpGet]
        [Route("quan-ly-customer")]
        public ActionResult ListCustomer()
        {
            var lstUsers = new List<UserInfo>();
            return PartialView("~/Areas/ModuleUsersAndRoles/Views/Customer/ListCustomer.cshtml", lstUsers);
        }

        [HttpPost]
        [Route("quan-ly-customer/find-customer")]
        public ActionResult FindUser(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort, string p_type)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                var userBL = new UserBL();
                List<UserInfo> _lst = userBL.User_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = WebApps.CommonFunction.AppsCommon.Get_HtmlPaging<Timesheet_Info>((int)_total_record, p_CurrentPage, "khách hàng");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                return PartialView("~/Areas/ModuleUsersAndRoles/Views/Customer/_PartialTableListCustomer.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/Customer/_PartialTableListCustomer.cshtml");
            }
        }

        [HttpGet]
        [Route("quan-ly-customer/get-view-to-add-customer")]
        public ActionResult GetViewToAddcustomer()
        {
            return View("~/Areas/ModuleUsersAndRoles/Views/Customer/_PartialInsert.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-customer/do-add-customer")]
        public ActionResult DoAddcustomer(UserInfo userInfo, string GroupId)
        {
            var result = new ActionBusinessResult();
            try
            {
                var userBL = new UserBL();
                userInfo.CreatedBy = SessionData.CurrentUser.Username;
                result = userBL.AddUser(userInfo, GroupId);
                string _pass = userInfo.Password;
                if (result.IsActionSuccess == true)
                {
                    Email_Info _Email_Info = new Email_Info
                    {
                        EmailFrom = EmailHelper.EmailOriginal.EMailFrom,
                        Pass = EmailHelper.EmailOriginal.PassWord,
                        Display_Name = EmailHelper.EmailOriginal.DisplayName,

                        EmailTo = userInfo.Email,
                        EmailCC = "",
                        Subject = "Email thông báo đăng ký mở tài khoản thành công",
                        Content = "Dear " + userInfo.FullName + ", Quí khách đăng ký thành công tài khoản username:" + userInfo.Email + " password:" + _pass + "\n quí khách vui lòng truy cập vào địa chỉ <a href='http://pathlaw.net/vi-vn/account/login'>http://pathlaw.net/vi-vn/account/login</a> để đổi mật khẩu của tài khoản. \n cảm ơn quí khách hàng. ",
                        LstAttachment = new List<string>(),
                    };

                    CommonFunction.AppsCommon.EnqueueSendEmail(_Email_Info);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = result.ToJson() });
        }

        [HttpGet]
        [Route("quan-ly-customer/get-view-to-edit-customer/{id}")]
        public ActionResult GetViewToEditcustomer()
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

            return View("~/Areas/ModuleUsersAndRoles/Views/Customer/_PartialEdit.cshtml", userInfo);
        }

        [HttpPost]
        [Route("quan-ly-customer/do-edit-customer")]
        public ActionResult DoEditCustomer(UserInfo userInfo, string GroupId)
        {
            var result = new ActionBusinessResult();
            try
            {
                var userBL = new UserBL();
                userInfo.ModifiedBy = SessionData.CurrentUser.Username;
                result = userBL.EditUser(userInfo, GroupId);

                if (SessionData.CurrentUser != null && result.IsActionSuccess == true && userInfo.Id == SessionData.CurrentUser.Id)
                {
                    SessionData.CurrentUser.loginfirst = 1;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return Json(new { result = result.ToJson() });
        }

        [HttpPost]
        [Route("quan-ly-customer/do-delete-customer")]
        public ActionResult DoDeleteCustomer(int userId)
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
        [Route("quan-ly-customer/view-detail-customer/{id}")]
        public ActionResult ViewDetailCustomer()
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

            return View("~/Areas/ModuleUsersAndRoles/Views/Customer/_PartialDetail.cshtml", userInfo);
        }
    }
}