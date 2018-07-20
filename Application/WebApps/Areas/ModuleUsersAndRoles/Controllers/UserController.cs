namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;

	using AppStart;

	using BussinessFacade;
	using BussinessFacade.ModuleUsersAndRoles;

	using Common;

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

		[HttpPost]
		[Route("quan-ly-nguoi-dung/find-user")]
		public ActionResult FindUser(string keysSearch, string options)
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

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialTableListUsers.cshtml", lstUsers);
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
	}
}