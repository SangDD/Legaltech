namespace BussinessFacade
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using CommonData;
	using DataAccess.ModuleUsersAndRoles;
    using ObjectInfo;

	public class FunctionBL : RepositoriesBL
    {
	    public const int   DisplayOnMenu = 1;
	    public const int   NoDisplayOnMenu = 0;

	    private static readonly object s_lockerAllFunctionRequiredLoginInMemory = new object();
	    private static readonly object s_lockerAllFunctionNoRequiredLoginInMemory = new object();
	    private static List<FunctionInfo> s_allFunctionRequiredLoginInMemory;
	    private static List<FunctionInfo> s_allFunctionNoRequiredLoginInMemory;

        public bool        IsAddFunctionSuccess { get; set; }
        public bool        IsEditFunctionSuccess { get; set; }
        public bool        IsDeleteFunctionSuccess { get; set; }
        public object      AddFunctionResult { get; set; } = new object();
        public object      EditFunctionResult { get; set; } = new object();
        public object      DeleteFunctionResult { get; set; } = new object();

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
		        var ds = FunctionDA.FindFunction(keysSearch, optionFilter, ref this._totalRecordFindResult);
		        var pagingHelper = new PagingHelper(optionFilter, this._totalRecordFindResult, "pageListOfFunctions", "divNumberRecordOnPageListFunctions");
		        this._pagingHtml = pagingHelper.Paging();
		        return CBO<FunctionInfo>.FillCollectionFromDataSet(ds);
	        }
	        catch (Exception ex)
	        {
                LogInfo.LogException(ex);
	        }

	        return Null<FunctionInfo>.GetListCollectionNull();
        }

        public void AddFunction(FunctionInfo functionAdd)
        {
            var result = FunctionDA.AddFunction(functionAdd);
            if (result > 0)
            {
                this.IsAddFunctionSuccess = true;

				// reload function collections in memory
	            LoadFunctionCollectionsToMemory();
            }

            this.MesageCode = this.IsAddFunctionSuccess ? KnMessageCode.AddFunctionSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.AddFunctionResult = new
            {
                isAddFunctionSuccess = this.IsAddFunctionSuccess,
				functionIdAdded = result,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
        }

        public void EditFunction(FunctionInfo functionEdit)
        {
            var result = FunctionDA.EditFunction(functionEdit);
            if (result > 0)
            {
                this.IsEditFunctionSuccess = true;

	            // reload function collections in memory
	            LoadFunctionCollectionsToMemory();
            }

            this.MesageCode = this.IsEditFunctionSuccess ? KnMessageCode.EditFunctionSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.EditFunctionResult = new
            {
                isEditFunctionSuccess = this.IsEditFunctionSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
        }

        public void DeleteFunction(int functionId)
        {
            var result = FunctionDA.DeleteFunction(functionId);
            if (result > 0)
            {
                this.IsDeleteFunctionSuccess = true;

	            // reload function collections in memory
	            LoadFunctionCollectionsToMemory();
            }

            this.MesageCode = this.IsDeleteFunctionSuccess ? KnMessageCode.DeleteFunctionSuccess : KnMessageCode.GetMvMessageByCode(result);
            this.DeleteFunctionResult = new
            {
                isDeleteFunctionSuccess = this.IsDeleteFunctionSuccess,
                code = this.MesageCode.GetCode(),
                message = this.MesageCode.GetMessage()
            };
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
