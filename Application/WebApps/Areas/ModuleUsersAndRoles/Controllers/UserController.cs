namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
	using AppStart;
    using BussinessFacade;
	using CommonData;
	using ObjectInfo;

	[ValidateAntiForgeryTokenOnAllPosts]
	[RouteArea("ModuleUsersAndRoles", AreaPrefix = "quan-tri-he-thong")]
	[Route("{action}")]
	public class UserController : Controller
	{
		// GET: ModuleUsersAndRoles/User
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
				LogInfo.LogException(ex);
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
				LogInfo.LogException(ex);
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
		public ActionResult DoAddUser(UserInfo userInfo, string arrGroupId)
		{
			var userBL = new UserBL();
			try
			{
				userInfo.CreatedBy = SessionData.CurrentUser.Username;
				userBL.AddUser(userInfo, arrGroupId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new
			{
				userBL.AddUserResult
			});
		}

		[HttpPost][Route("quan-ly-nguoi-dung/get-view-to-edit-user")]
		public ActionResult GetViewToEditUser(int userId)
		{
			var userInfo = new UserInfo();
			try
			{
				userInfo = UserBL.GetUserById(userId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/User/_PartialEditUser.cshtml", userInfo);
		}

		[HttpPost][Route("quan-ly-nguoi-dung/do-edit-user")]
		public ActionResult DoEditUser(UserInfo userInfo, string arrGroupId)
		{
			var userBL = new UserBL();
			try
			{
				userInfo.ModifiedBy = SessionData.CurrentUser.Username;
				userBL.EditUser(userInfo, arrGroupId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { userBL.EditUserResult });
		}

		[HttpPost][Route("quan-ly-nguoi-dung/do-delete-user")]
		public ActionResult DoDeleteUser(int userId)
		{
			var userBL = new UserBL();
			try
			{
				var modifiedBy = SessionData.CurrentUser.Username;
				userBL.DeleteUser(userId, modifiedBy);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { userBL.DeleteUserResult });
		}
	}
}