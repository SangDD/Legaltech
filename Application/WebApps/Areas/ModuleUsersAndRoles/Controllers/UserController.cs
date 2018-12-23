namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    using AppStart;

    using BussinessFacade;
    using BussinessFacade.ModuleUsersAndRoles;

    using Common;
    using Common.CommonData;
    using Common.Ultilities;
    using ObjectInfos;
    using ObjectInfos.ModuleUsersAndRoles;
    using Session;

    [ValidateAntiForgeryTokenOnAllPosts]
    [RouteArea("ModuleUsersAndRoles", AreaPrefix = "quan-tri-he-thong")]
    [Route("{action}")]
    public class UserController : Controller
    {
        [HttpGet]
        [Route("quan-ly-nguoi-dung")]
        public ActionResult ListUser()
        {
            var lstUsers = new List<UserInfo>();
            try
            {
                var userBL = new UserBL();
                lstUsers = userBL.FindUser();

                string htmlPaging = CommonFuc.Get_HtmlPaging<UserInfo>((int)lstUsers.Count, 1, "Người dùng");
                ViewBag.Paging = htmlPaging;
                //ViewBag.Paging = userBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ListUser.cshtml", lstUsers);
        }

        [HttpGet]
        [Route("quan-ly-luat-su")]
        public ActionResult ListLawer()
        {
            var lstUsers = new List<UserInfo>();
            //try
            //{
            //    var userBL = new UserBL();
            //    lstUsers = userBL.FindUser();
            //    ViewBag.Paging = userBL.GetPagingHtml();
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogException(ex);
            //}

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ListLawer.cshtml", lstUsers);
        }

        [HttpGet]
        [Route("quan-ly-khach-hang")]
        public ActionResult ListCustomer()
        {
            var lstUsers = new List<UserInfo>();
            //try
            //{
            //    var userBL = new UserBL();
            //    lstUsers = userBL.FindUser();
            //    ViewBag.Paging = userBL.GetPagingHtml();
            //}
            //catch (Exception ex)
            //{
            //    Logger.LogException(ex);
            //}

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ListCustomer.cshtml", lstUsers);
        }

        [HttpPost]
        [Route("quan-ly-nguoi-dung/find-user")]
        public ActionResult FindUser(string p_keysearch, int p_CurrentPage, string p_column, string p_type_sort, string p_type)
        {
            try
            {
                decimal _total_record = 0;

                string p_to = "";
                string p_from = CommonFuc.Get_From_To_Page(p_CurrentPage, ref p_to);

                var userBL = new UserBL();
                List<UserInfo> _lst = userBL.User_Search(p_keysearch, ref _total_record, p_from, p_to);
                string htmlPaging = CommonFuc.Get_HtmlPaging<UserInfo>((int)_total_record, p_CurrentPage, "Người dùng");

                ViewBag.Paging = htmlPaging;
                ViewBag.Obj = _lst;
                ViewBag.SumRecord = _total_record;

                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return PartialView("~/Areas/Manager/Views/TimeSheet/_PartialTableTimeSheet.cshtml");
            }
        }

        //[HttpPost]
        //[Route("quan-ly-nguoi-dung/find-user")]
        //public ActionResult FindUser(string keysSearch, string options, string p_type)
        //{
        //    var lstUsers = new List<UserInfo>();
        //    try
        //    {
        //        var userBL = new UserBL();
        //        lstUsers = userBL.FindUser(keysSearch, options);
        //        ViewBag.Paging = userBL.GetPagingHtml();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //    }

        //    if (p_type == "ALL")
        //    {
        //        return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml", lstUsers);
        //    }
        //    else if (p_type == ((int)(int)CommonEnums.UserType.Lawer).ToString())
        //    {
        //        return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListLawer.cshtml", lstUsers);
        //    }
        //    else if (p_type == ((int)(int)CommonEnums.UserType.Customer).ToString())
        //    {
        //        return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListCustomer.cshtml", lstUsers);
        //    }
        //    else return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml", new List<UserInfo>());
        //}

        [HttpGet]
        [Route("quan-ly-nguoi-dung/get-view-to-add-user")]
        public ActionResult GetViewToAddUser()
        {
            return View("~/Areas/ModuleUsersAndRoles/Views/User/_PartialAddUser.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-nguoi-dung/do-add-user")]
        public ActionResult DoAddUser(UserInfo userInfo, string GroupId)
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
        [Route("quan-ly-nguoi-dung/get-view-to-edit-user/{id}")]
        public ActionResult GetViewToEditUser()
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

            return View("~/Areas/ModuleUsersAndRoles/Views/User/_PartialEditUser.cshtml", userInfo);
        }

        [HttpPost]
        [Route("quan-ly-nguoi-dung/do-edit-user")]
        public ActionResult DoEditUser(UserInfo userInfo, string GroupId)
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
        [Route("quan-ly-nguoi-dung/do-delete-user")]
        public ActionResult DoDeleteUser(int userId)
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
        [Route("quan-ly-nguoi-dung/view-detail-user/{id}")]
        public ActionResult ViewDetailUser()
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

            return View("~/Areas/ModuleUsersAndRoles/Views/User/_PartialDetailUser.cshtml", userInfo);
        }

        [HttpPost]
        [Route("quan-ly-nguoi-dung/get-view-to-reset-pass")]
        public ActionResult GetViewToResetPass(string p_user_name)
        {
            ViewBag.User_Name = p_user_name;
            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialResetPass.cshtml");
        }

        [HttpPost]
        [Route("quan-ly-nguoi-dung/do-reset-pass")]
        public ActionResult DoResetPass(string p_user_name, string p_password, string p_re_password)
        {
            try
            {
                var userBL = new UserBL();
                int re = userBL.DoResetPass(p_user_name, Encription.EncryptAccountPassword(p_user_name, p_password), p_re_password, SessionData.CurrentUser.Username);
                return Json(new { success = re });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { success = -1 });
            }
        }


        [HttpGet]
        [Route("danh-sach-kh-dk")]
        public ActionResult DanhSachKHDangKy()
        {
            var lstUsers = new List<RegisterInfo>();
            try
            {
                decimal totalRecord = 0;
                var userBL = new UserBL();
                lstUsers = userBL.RegisterGetAll("ALL|ALL|ALL|ALL",0,20,ref totalRecord);
                ViewBag.lstUsers = lstUsers;
                string htmlPaging = CommonFuc.Get_HtmlPaging<RegisterInfo>((int)totalRecord, 1, "bản ghi",10, "jsPageKH");
                ViewBag.Paging = htmlPaging;
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/DanhSachKHDangKy.cshtml"  );
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/DanhSachKHDangKy.cshtml");
        }


        [HttpPost]
        [Route("tim-kiem-kh-dk")]
        public ActionResult SearchCustomerRegis(string pKeySearch, int pNumPage)
        {
            var lstUsers = new List<RegisterInfo>();
            try
            {
                decimal totalRecord = 0;
                var userBL = new UserBL();
                int p_to = 0;
                int p_from = CommonFuc.GetFromToPage(pNumPage, ref p_to);
                lstUsers = userBL.RegisterGetAll(pKeySearch, p_from, p_to, ref totalRecord);
                ViewBag.lstUsers = lstUsers;
                string htmlPaging = CommonFuc.Get_HtmlPaging<RegisterInfo>((int)totalRecord, 1, "bản ghi", 10, "jsPageKH");
                ViewBag.Paging = htmlPaging;
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListRegistor.cshtml");
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListRegistor.cshtml");
        }

        [Route("chi-tiet-dang-ky/{id}")]
        public ActionResult RegisterDetail()
        {
            var _RegisterInfo = new RegisterInfo();
            string _casecode = "";
            if(RouteData.Values["id"] != null)
            {
                _casecode = RouteData.Values["id"].ToString();
            }
            try
            {
                var userBL = new UserBL();
                _RegisterInfo = userBL.RegisterGetByCaseCode(_casecode);
                //  lấy dữ liệu lịch sử giao dịch
                B_Todos_BL _B_Todos_BL = new B_Todos_BL();
                List<B_Remind_Info> _ListRemind = new List<B_Remind_Info>();
                List<B_Todos_Info> _Listtodo = _B_Todos_BL.NotifiGetByCasecode(_casecode, ref _ListRemind);
                ViewBag.ListTodo = _Listtodo;
                ViewBag.ListRemind = _ListRemind;
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ViewRegisterInfo.cshtml", _RegisterInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return null;
        }

        [HttpPost]
        [Route("xac-nhan-kh-dk")]
        public ActionResult XacNhanKhachHangDK(decimal pID,string pEmail)
        {
            var lstUsers = new List<RegisterInfo>();
            try
            {
                var passwordEncrypt = Encription.EncryptAccountPassword(pEmail, "123456");
                UserBL objBL = new UserBL();
                RegisterInfo pInfo = new RegisterInfo();
                pInfo.Id = pID;
                pInfo.Modifiedby = SessionData.CurrentUser.Username;
                pInfo.ModifiedDate = SessionData.CurrentUser.CurrentDate;
                pInfo.KeySecret = passwordEncrypt;
                pInfo.Status = 1;
                int preturn = objBL.RegisterUpdate(pInfo);
                if (preturn >= 0)
                {
                   EmailHelper.SendMail(pEmail, "doduysang@gmail.com", "Email thông báo đăng ký mở tài khoản thành công", "Dear Customer, Quí khách đăng ký thành công tài khoản username:" + pEmail + " password:123456" + "\n quí khách vui lòng truy cập vào địa chỉ <a href='http://pathlaw.net/vi-vn/login'>http://pathlaw.net/vi-vn/login</a> để đổi mật khẩu của tài khoản. \n cảm ơn quí khách hàng. ", new List<string>());
                }
                return Json(new { status = preturn });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return Json(new { status = -3 });
            }
        }



    }
}