namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;
    using BussinessFacade;
	using CommonData;
	using ObjectInfo;
	using WebApps.AppStart;

	[ValidateAntiForgeryTokenOnAllPosts]
	[RouteArea("ModuleUsersAndRoles", AreaPrefix = "quan-tri-he-thong")]
	[Route("{action}")]
	public class GroupUserController : Controller
	{
		// GET: ModuleUsersAndRoles/GroupUser
		[HttpGet]
		[Route("quan-ly-nhom-quyen")]
		public ActionResult ListGroup()
		{
			var lstGroups = new List<GroupUserInfo>();
			try
			{
				var groupBL = new GroupUserBL();
				lstGroups = groupBL.FindGroup();
				ViewBag.Paging = groupBL.GetPagingHtml();
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return View("~/Areas/ModuleUsersAndRoles/Views/GroupUser/ListGroup.cshtml", lstGroups);
		}

		[HttpPost]
		[Route("quan-ly-nhom-quyen/find-group")]
		public ActionResult FindGroup(string keysSearch, string options)
		{
			var lstGroups = new List<GroupUserInfo>();
			try
			{
				var groupBL = new GroupUserBL();
				lstGroups = groupBL.FindGroup(keysSearch, options);
				ViewBag.Paging = groupBL.GetPagingHtml();
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/GroupUser/_PartialTableListGroups.cshtml", lstGroups);
		}

		[HttpPost]
		[Route("quan-ly-nhom-quyen/get-view-to-add-group")]
		public ActionResult GetViewToAddGroup()
		{
			return PartialView("~/Areas/ModuleUsersAndRoles/Views/GroupUser/_PartialAddGroup.cshtml");
		}

		[HttpPost]
		[Route("quan-ly-nhom-quyen/do-add-group")]
		public ActionResult DoAddGroup(GroupUserInfo groupInfo)
		{
			var groupBL = new GroupUserBL();
			try
			{
				groupInfo.CreatedBy = SessionData.CurrentUser.Username;
				groupBL.AddGroup(groupInfo);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { groupBL.AddGroupResult });
		}

		[HttpPost]
		[Route("quan-ly-nhom-quyen/get-view-to-edit-group")]
		public ActionResult GetViewToEditGroup(int groupId)
		{
			var groupInfo = new GroupUserInfo();
			try
			{
				groupInfo = GroupUserBL.GetGroupById(groupId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/GroupUser/_PartialEditGroup.cshtml", groupInfo);
		}

		[HttpPost]
		[Route("quan-ly-nhom-quyen/do-edit-group")]
		public ActionResult DoEditGroup(GroupUserInfo groupInfo)
		{
			var groupBL = new GroupUserBL();
			try
			{
				groupInfo.ModifiedBy = SessionData.CurrentUser.Username;
				groupBL.EditGroup(groupInfo);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { groupBL.EditGroupResult });
		}

		[HttpPost][Route("quan-ly-nhom-quyen/do-delete-group")]
		public ActionResult DoDeleteGroup(int groupId)
		{
			var groupBL = new GroupUserBL();
			try
			{
				var modifiedBy = SessionData.CurrentUser.Username;
				groupBL.DeleteGroup(groupId, modifiedBy);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { groupBL.DeleteGroupResult });
		}

		[HttpPost][Route("quan-ly-nhom-quyen/get-functions-in-group")]
		public ActionResult GetFunctionsInGroup(int groupId)
		{
			var htmlTreeViewFunctionsInGroup = string.Empty;
			try
			{
				var groupInfo = GroupUserBL.GetGroupById(groupId);
				ViewBag.GroupInfo = groupInfo;
				var groupBL = new GroupUserBL();
				htmlTreeViewFunctionsInGroup = groupBL.GetHtmlTreeViewFunctionsInGroup(groupId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/GroupUser/_PartialSetupFunctionsToGroup.cshtml", htmlTreeViewFunctionsInGroup);
		}

		[HttpPost][Route("quan-ly-nhom-quyen/setup-functions-to-group")]
		public ActionResult DoSetupFunctionsToGroup(int[] arrFunctionId, int groupId)
		{
			var groupBL = new GroupUserBL();
			try
			{
				groupBL.SetupFunctionsToGroup(groupId, arrFunctionId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { groupBL.SetupFunctionsToGroupResult });
		}
	}
}