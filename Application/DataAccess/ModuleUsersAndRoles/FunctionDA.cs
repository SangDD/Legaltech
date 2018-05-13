namespace DataAccess.ModuleUsersAndRoles
{
	using System;
	using System.Data;
	using Common;
	using Common.MessageCode;
	using Common.SearchingAndFiltering;
	using ObjectInfos.ModuleUsersAndRoles;
	using Oracle.DataAccess.Client;

	public class FunctionDA
	{
		public static DataSet GetFunctionById(int functionId)
        {
            var ds = new DataSet();
            try
            {
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_GetById",
		            new OracleParameter("p_functionId", OracleDbType.Int32, functionId, ParameterDirection.Input),
		            new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static DataSet GetAllFunctions()
        {
            var ds = new DataSet();
            try
            {
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_GetAll",
		            new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

		public static DataSet GetAllFunctionsRequiredLogin()
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_function.proc_GetAllFnsRequiredLogin",
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return ds;
		}

        public static DataSet GetAllFunctionsNoRequiredLogin()
        {
            var ds = new DataSet();
            try
            {
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_GetAllFnsNoRequiredLogin",
		            new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        /// <summary>
        /// Find all function by key search
        /// </summary>
        /// <param name="keysSearch">contain collection of parameter of condition searching, split by '|'</param>
        /// <param name="options">contain collection of parameter to custom value return</param>
        /// <param name="totalRecord">Total record matching condition</param>
        /// <returns>Dataset rows of record matching</returns>
        public static DataSet FindFunction(string keysSearch, OptionFilter options, ref int totalRecord)
        {
            var ds = new DataSet();
            try
            {
                // Keys search
                var functionName = Null.NullString; // Find by near value matching of FunctionName/DisplayNameOnMenu
                var functionType = Null.NullString; // Array FunctionType, split by ',' and user IN operator in sql for searching
                var href         = Null.NullString; // Find by near value matching of  HrefGet/HrefPost
                var parentId     = Null.NullString; // Array Prid, split by ',' and user IN operator in sql for searching
                var level        = Null.NullString; // Array Level, split by ',' and user IN operator in sql for searching
                if (!string.IsNullOrEmpty(keysSearch))
                {
                    var arrKeySearch = keysSearch.Split('|');
                    if (arrKeySearch.Length == 5)
                    {
                        functionName = arrKeySearch[0];
                        functionType = KeySearch.FilterComboboxValue(arrKeySearch[1]);
                        href = arrKeySearch[2];
                        parentId = KeySearch.FilterComboboxValue(arrKeySearch[3]);
                        level = arrKeySearch[4];
                    }
                }

	            var paramTotalRecord = new OracleParameter("p_total_record", OracleDbType.Int32, ParameterDirection.Output);
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_Find",
		            new OracleParameter("p_functionName", OracleDbType.Varchar2, functionName, ParameterDirection.Input),
		            new OracleParameter("p_functionType", OracleDbType.Varchar2, functionType, ParameterDirection.Input),
		            new OracleParameter("p_href", OracleDbType.Varchar2, href, ParameterDirection.Input),
		            new OracleParameter("p_parentId", OracleDbType.Varchar2, parentId, ParameterDirection.Input),
		            new OracleParameter("p_level", OracleDbType.Varchar2, level, ParameterDirection.Input),

		            new OracleParameter("p_orderBy", OracleDbType.Varchar2, options.OrderBy, ParameterDirection.Input),
		            new OracleParameter("p_startAt", OracleDbType.Decimal, options.StartAt, ParameterDirection.Input),
		            new OracleParameter("p_endAt", OracleDbType.Decimal, options.EndAt, ParameterDirection.Input),
		            paramTotalRecord,
		            new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

	            totalRecord = Convert.ToInt32(paramTotalRecord.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

        public static int AddFunction(FunctionInfo functionAdd)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_AddNew",
		            new OracleParameter("p_functionName", OracleDbType.Varchar2, functionAdd.FunctionName, ParameterDirection.Input),
		            new OracleParameter("p_displayName", OracleDbType.Varchar2, functionAdd.DisplayName, ParameterDirection.Input),
		            new OracleParameter("p_functionType", OracleDbType.Int32, functionAdd.FunctionType, ParameterDirection.Input),
		            new OracleParameter("p_hrefGet", OracleDbType.Varchar2, functionAdd.HrefGet, ParameterDirection.Input),
		            new OracleParameter("p_hrefPost", OracleDbType.Varchar2, functionAdd.HrefPost, ParameterDirection.Input),
		            new OracleParameter("p_position", OracleDbType.Int32, functionAdd.Position, ParameterDirection.Input),
		            new OracleParameter("p_parentId", OracleDbType.Int32, functionAdd.ParentId, ParameterDirection.Input),
		            new OracleParameter("p_lev", OracleDbType.Int32, functionAdd.Lev, ParameterDirection.Input),
		            new OracleParameter("p_menuId", OracleDbType.Int32, functionAdd.MenuId, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.AddFunctionFail.GetCode();
        }

        public static int EditFunction(FunctionInfo functionEdit)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_Edit",
		            new OracleParameter("p_functionId", OracleDbType.Int32, functionEdit.Id, ParameterDirection.Input),
		            new OracleParameter("p_functionName", OracleDbType.Varchar2, functionEdit.FunctionName, ParameterDirection.Input),
		            new OracleParameter("p_displayName", OracleDbType.Varchar2, functionEdit.DisplayName, ParameterDirection.Input),
		            new OracleParameter("p_functionType", OracleDbType.Int32, functionEdit.FunctionType, ParameterDirection.Input),
		            new OracleParameter("p_hrefGet", OracleDbType.Varchar2, functionEdit.HrefGet, ParameterDirection.Input),
		            new OracleParameter("p_hrefPost", OracleDbType.Varchar2, functionEdit.HrefPost, ParameterDirection.Input),
		            new OracleParameter("p_position", OracleDbType.Int32, functionEdit.Position, ParameterDirection.Input),
		            new OracleParameter("p_parentId", OracleDbType.Int32, functionEdit.ParentId, ParameterDirection.Input),
		            new OracleParameter("p_lev", OracleDbType.Int32, functionEdit.Lev, ParameterDirection.Input),
		            new OracleParameter("p_menuId", OracleDbType.Int32, functionEdit.MenuId, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.EditFunctionFail.GetCode();
        }

        public static int DeleteFunction(int functionId)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_function.proc_Function_Delete",
		            new OracleParameter("p_functionId", OracleDbType.Int32, functionId, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.DeleteFunctionFail.GetCode();
        }

		public static DataSet GetAllInnerFunctions(int functionId)
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_function.proc_GetAllInnerFunction",
					new OracleParameter("p_functionId", OracleDbType.Int32, functionId, ParameterDirection.Input),
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return ds;
		}
	}
}
