namespace BussinessFacade.ModuleUsersAndRoles
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Common;
	using Common.MessageCode;
	using Common.SearchingAndFiltering;

	using DataAccess.ModuleUsersAndRoles;

	using ObjectInfos;

	public class FunctionBL : RepositoriesBL
	{
		private static readonly object s_lockerAllFunctionRequiredLoginInMemory = new object();
		private static readonly object s_lockerAllFunctionNoRequiredLoginInMemory = new object();
		private static List<FunctionInfo> s_allFunctionRequiredLoginInMemory;
		private static List<FunctionInfo> s_allFunctionNoRequiredLoginInMemory;

		public static FunctionInfo GetFunctionById(int functionId)
		{
			var ds = FunctionDA.GetFunctionById(functionId);
			return CBO<FunctionInfo>.FillObjectFromDataSet(ds);
		}

		public static List<FunctionInfo> GetAllFunctions()
		{
			var ds = FunctionDA.GetAllFunctions();
			return CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
		}

		public static void LoadFunctionCollectionsToMemory()
		{
			LoadAllFunctionsRequiredLogin();
			LoadAllFunctionsNoRequiredLogin();
		}

		public static List<FunctionInfo> GetRootFunctions(List<FunctionInfo> lstFunctionInfo)
		{
			return lstFunctionInfo.Where(t => t.ParentId == 0).ToList();
		}

		public static List<FunctionInfo> GetChildrenFunctions(List<FunctionInfo> lstFunctionInfo, int parentId)
		{
			return lstFunctionInfo.Where(t => t.ParentId.Equals(parentId)).ToList();
		}

		public static List<FunctionInfo> GetAllFunctionsRequiredLogin()
		{
			lock (s_lockerAllFunctionRequiredLoginInMemory)
			{
				return s_allFunctionRequiredLoginInMemory;
			}
		}

		public static List<FunctionInfo> GetAllFunctionsNoRequiredLogin()
		{
			lock (s_lockerAllFunctionNoRequiredLoginInMemory)
			{
				return s_allFunctionNoRequiredLoginInMemory;
			}
		}

		public static List<FunctionInfo> GetAllInnerFunctions(int functionId)
		{
			var ds = FunctionDA.GetAllInnerFunctions(functionId);
			return CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
		}

		public List<FunctionInfo> FindFunction(string keysSearch = "", string options = "")
		{
			try
			{
				var optionFilter = new OptionFilter(options);
				var totalRecordFindResult = 0;
				var ds = FunctionDA.FindFunction(keysSearch, optionFilter, ref totalRecordFindResult);
				this.SetupPagingHtml(optionFilter, totalRecordFindResult, "pageListOfFunctions", "divNumberRecordOnPageListFunctions");
				return CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return Null<FunctionInfo>.GetListCollectionNull();
		}

		public ActionBusinessResult AddFunction(FunctionInfo functionAdd)
		{
			var result = FunctionDA.AddFunction(functionAdd);
			if (result > 0)
			{
				this.SetActionSuccess(true);

				// reload function collections in memory
				LoadFunctionCollectionsToMemory();
			}

			var additionalData = new List<JsonData>
								 {
									 new JsonData("functionIdAdded", result.ToString())
								 };
			this.SetAdditionalDataResult(additionalData);
			return this.SetActionResult(result, KnMessageCode.AddFunctionSuccess);
		}

		public ActionBusinessResult EditFunction(FunctionInfo functionEdit)
		{
			var result = FunctionDA.EditFunction(functionEdit);
			if (result > 0)
			{
				this.SetActionSuccess(true);

				// reload function collections in memory
				LoadFunctionCollectionsToMemory();
			}

			return this.SetActionResult(result, KnMessageCode.EditFunctionSuccess);
		}

		public ActionBusinessResult DeleteFunction(int functionId)
		{
			var result = FunctionDA.DeleteFunction(functionId);
			if (result > 0)
			{
				this.SetActionSuccess(true);

				// reload function collections in memory
				LoadFunctionCollectionsToMemory();
			}

			return this.SetActionResult(result, KnMessageCode.DeleteFunctionSuccess);
		}

		private static void LoadAllFunctionsRequiredLogin()
		{
			lock (s_lockerAllFunctionRequiredLoginInMemory)
			{
				var ds = FunctionDA.GetAllFunctionsRequiredLogin();
				s_allFunctionRequiredLoginInMemory = CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
			}
		}

		private static void LoadAllFunctionsNoRequiredLogin()
		{
			lock (s_lockerAllFunctionNoRequiredLoginInMemory)
			{
				var ds = FunctionDA.GetAllFunctionsNoRequiredLogin();
				s_allFunctionNoRequiredLoginInMemory = CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
			}
		}
	}
}
