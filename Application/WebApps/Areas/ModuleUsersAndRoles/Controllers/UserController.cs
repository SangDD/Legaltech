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
				ViewBag.Paging = userBL.GetPagingHtml();
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
            try
            {
                var userBL = new UserBL();
                lstUsers = userBL.FindUser();
                ViewBag.Paging = userBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ListLawer.cshtml", lstUsers);
        }


        [HttpGet]
        [Route("quan-ly-khach-hang")]
        public ActionResult ListCustomer()
        {
            var lstUsers = new List<UserInfo>();
            try
            {
                var userBL = new UserBL();
                lstUsers = userBL.FindUser();
                ViewBag.Paging = userBL.GetPagingHtml();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/ListCustomer.cshtml", lstUsers);
        }


        [HttpPost]
		[Route("quan-ly-nguoi-dung/find-user")]
		public ActionResult FindUser(string keysSearch, string options, string p_type)
		{
			var lstUsers = new List<UserInfo>();
			try
			{
				var userBL = new UserBL();
				lstUsers = userBL.FindUser(keysSearch, options);
				ViewBag.Paging = userBL.GetPagingHtml();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

            if (p_type == "ALL")
            {
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml", lstUsers);
            }
            else if (p_type == ((int)(int)CommonEnums.UserType.Lawer).ToString())
            {
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListLawer.cshtml", lstUsers);
            }
            else if (p_type == ((int)(int)CommonEnums.UserType.Customer).ToString())
            {
                return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListCustomer.cshtml", lstUsers);
            }
            else return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml", new List<UserInfo>());
        }

		[HttpPost]
		[Route("quan-ly-nguoi-dung/get-view-to-add-user")]
		public ActionResult GetViewToAddUser()
		{
			return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialAddUser.cshtml");
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

		[HttpPost][Route("quan-ly-nguoi-dung/get-view-to-edit-user")]
		public ActionResult GetViewToEditUser(int userId)
		{
			var userInfo = new UserInfo();
			try
			{
                UserBL _UserBL = new UserBL();
                userInfo = _UserBL.GetUserById(userId);
				userInfo.GroupSelectedCollection = UserBL.GetUserSelfGroups(userId);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialEditUser.cshtml", userInfo);
		}

		[HttpPost][Route("quan-ly-nguoi-dung/do-edit-user")]
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

		[HttpPost][Route("quan-ly-nguoi-dung/do-delete-user")]
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

		[HttpPost][Route("quan-ly-nguoi-dung/view-detail-user")]
		public ActionResult ViewDetailUser(int userId)
		{
			var userInfo = new UserInfo();
			try
			{
                UserBL _UserBL = new UserBL();
                userInfo = _UserBL.GetUserById(userId);
				userInfo.GroupSelectedCollection = UserBL.GetUserSelfGroups(userId);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialDetailUser.cshtml", userInfo);
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
    }
}