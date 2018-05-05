namespace WebApps.Areas.ModuleUsersAndRoles.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Web.Mvc;

	using AppStart;

	using BussinessFacade;
	using BussinessFacade.ModuleUsersAndRoles;

	using Common;

	using ObjectInfos.ModuleUsersAndRoles;

	[ValidateAntiForgeryTokenOnAllPosts]
	[RouteArea("ModuleUsersAndRoles", AreaPrefix = "quan-tri-he-thong")]
	[Route("{action}")]
	public class FunctionController : Controller
	{
		// GET: ModuleUsersAndRoles/ListFunctions
		[HttpGet]
		[Route("quan-ly-chuc-nang")]
		public ActionResult ListFunctions()
		{
			var lstFunctions = new List<FunctionInfo>();
			try
			{
				var functionBL = new FunctionBL();
				lstFunctions = functionBL.FindFunction();
				ViewBag.Paging = functionBL.GetPagingHtml();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return View("~/Areas/ModuleUsersAndRoles/Views/Function/ListFunctions.cshtml", lstFunctions);
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/find-function")]
		public ActionResult FindFunction(string keysSearch, string options)
		{
			var lstFunctions = new List<FunctionInfo>();
			try
			{
				var functionBL = new FunctionBL();
				lstFunctions = functionBL.FindFunction(keysSearch, options);
				ViewBag.Paging = functionBL.GetPagingHtml();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialTableListFunctions.cshtml", lstFunctions);
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/get-view-to-add-function")]
		public ActionResult GetViewToAddNewFunction()
		{
			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialAddNewFunction.cshtml");
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/do-add-function")]
		public ActionResult DoAddNewFunction(FunctionInfo functionInfo)
		{
			var result = new ActionBusinessResult();
			try
			{
				var functionBL = new FunctionBL();
				result = functionBL.AddFunction(functionInfo);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return Json(new { result = result.ToJson() });
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/get-view-to-edit-function")]
		public ActionResult GetViewToEditFunction(int functionId)
		{
			var functionInfo = new FunctionInfo();
			try
			{
				functionInfo = FunctionBL.GetFunctionById(functionId);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialEditFunction.cshtml", functionInfo);
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/do-edit-function")]
		public ActionResult DoEditFunction(FunctionInfo functionInfo)
		{
			var result = new ActionBusinessResult();
			try
			{
				var functionBL = new FunctionBL();
				result = functionBL.EditFunction(functionInfo);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return Json(new { result = result.ToJson() });
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/do-delete-function")]
		public ActionResult DoDeleteFunction(int functionId)
		{
			var result = new ActionBusinessResult();
			try
			{
				var functionBL = new FunctionBL();
				result = functionBL.DeleteFunction(functionId);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return Json(new { result = result.ToJson() });
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/view-all-inner-function")]
		public ActionResult ViewAllInnerFunction(int functionId)
		{
			var lstFunctionInfo = new List<FunctionInfo>();
			try
			{
				var functionInfo = FunctionBL.GetFunctionById(functionId);
				ViewBag.CurrentFunction = functionInfo;
				lstFunctionInfo = FunctionBL.GetAllInnerFunctions(functionId);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialViewAllInnerFunctions.cshtml", lstFunctionInfo);
		}
	}
}