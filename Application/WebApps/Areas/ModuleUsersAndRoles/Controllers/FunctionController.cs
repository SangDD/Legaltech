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
				LogInfo.LogException(ex);
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
				LogInfo.LogException(ex);
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
			var functionBL = new FunctionBL();
			try
			{
				functionBL.AddFunction(functionInfo);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new
			{
				functionBL.AddFunctionResult
			});
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
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialEditFunction.cshtml", functionInfo);
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/do-edit-function")]
		public ActionResult DoEditFunction(FunctionInfo functionInfo)
		{
			var functionBL = new FunctionBL();
			try
			{
				functionBL.EditFunction(functionInfo);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new
			{
				functionBL.EditFunctionResult
			});
		}

		[HttpPost]
		[Route("quan-ly-chuc-nang/do-delete-function")]
		public ActionResult DoDeleteFunction(int functionId)
		{
			var functionBL = new FunctionBL();
			try
			{
				functionBL.DeleteFunction(functionId);
			}
			catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return Json(new { functionBL.DeleteFunctionResult });
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
				LogInfo.LogException(ex);
			}

			return PartialView("~/Areas/ModuleUsersAndRoles/Views/Function/_PartialViewAllInnerFunctions.cshtml", lstFunctionInfo);
		}
	}
}