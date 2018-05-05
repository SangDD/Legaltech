namespace DataAccess.ModuleUsersAndRoles
{
	using System;
	using System.Data;
	using System.Linq;

	using Common;
	using Common.MessageCode;
	using Common.SearchingAndFiltering;

	using ObjectInfos.ModuleUsersAndRoles;

	using Oracle.DataAccess.Client;

	public class GroupUserDA
	{
		public static DataSet GetGroupById(int groupId)
        {
	        var ds = new DataSet();
	        try
	        {
		        ds = OracleHelper.ExecuteDataset(
			        Configuration.connectionString,
			        CommandType.StoredProcedure,
			        "pkg_s_groups.proc_GroupUser_GetById",
			        new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
			        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
	        }
	        catch (Exception ex)
	        {
		        Logger.LogException(ex);
	        }

	        return ds;
        }

        public static DataSet GetAllGroups()
        {
	        var ds = new DataSet();
	        try
	        {
		        ds = OracleHelper.ExecuteDataset(
			        Configuration.connectionString,
			        CommandType.StoredProcedure,
			        "pkg_s_groups.proc_GroupUser_GetAll",
			        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
	        }
	        catch (Exception ex)
	        {
		        Logger.LogException(ex);
	        }

	        return ds;
        }

		/// <summary>
		/// Find all group by key search
		/// </summary>
		/// <param name="keysSearch">contain collection of parameter of condition searching, split by '|'</param>
		/// <param name="options">contain collection of parameter to custom value return</param>
		/// <param name="totalRecord">Total record matching condition</param>
		/// <returns>Dataset of row result match</returns>
		public static DataSet FindGroup(string keysSearch, OptionFilter options, ref int totalRecord)
        {
            var ds = new DataSet();
            try
            {
                // Keys search
                var groupName = Null.NullString; // Find by near value matching of GroupName
                if (!string.IsNullOrEmpty(keysSearch))
                {
                    var arrKeySearch = keysSearch.Split('|');
                    if (arrKeySearch.Length > 0)
                    {
                        groupName = arrKeySearch[0];
                    }
                }

	            var paramTotalRecord = new OracleParameter("p_totalRecord", OracleDbType.Int32, ParameterDirection.Output);
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_groups.proc_GroupUser_Find",
		            new OracleParameter("p_groupName", OracleDbType.Varchar2, groupName, ParameterDirection.Input),

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

        public static int AddGroup(GroupUserInfo groupAdd)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_groups.proc_GroupUser_AddNew",
		            new OracleParameter("p_groupName", OracleDbType.Varchar2, groupAdd.Name, ParameterDirection.Input),
		            new OracleParameter("p_CreatedBy", OracleDbType.Varchar2, groupAdd.CreatedBy, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.AddGroupFail.GetCode();
        }

        public static int EditGroup(GroupUserInfo groupEdit)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_groups.proc_GroupUser_Edit",
		            new OracleParameter("p_groupId", OracleDbType.Int32, groupEdit.Id, ParameterDirection.Input),
		            new OracleParameter("p_groupName", OracleDbType.Varchar2, groupEdit.Name, ParameterDirection.Input),
		            new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, groupEdit.ModifiedBy, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.EditGroupFail.GetCode();
        }

        public static int DeleteGroup(int groupId, string modifiedBy)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_groups.proc_GroupUser_Delete",
		            new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
		            new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, modifiedBy, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.DeleteGroupFail.GetCode();
        }

		public static int AddFunctionToGroupBatch(int[] arrGroupId, int[] arrFunctionId, int totalRecord)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(
	                Configuration.connectionString,
	                CommandType.StoredProcedure,
                    "pkg_s_groups.proc_SetupFunctionToGroup", 
	                totalRecord,
                    new OracleParameter("p_groupId", OracleDbType.Int32, arrGroupId, ParameterDirection.Input),
                    new OracleParameter("p_functionId", OracleDbType.Int32, arrFunctionId, ParameterDirection.Input),
                    paramReturn);

                var totalReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (var item in totalReturn.Where(item => Convert.ToInt32(item.ToString()) < 0))
                {
	                return Convert.ToInt32(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

	        return KnMessageCode.SetupFunctionsToGroupSuccess.GetCode();
        }

        public static int DeleteFunctionFromGroup(int groupId)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(
	                Configuration.connectionString,
	                CommandType.StoredProcedure,
                    "pkg_s_groups.proc_DeleteFunctionFromGroup",
                    new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

	        return KnMessageCode.SetupFunctionsToGroupFail.GetCode();
        }

        public static DataSet GetAllFunctionsByGroup(int groupId)
        {
            var ds = new DataSet();
            try
            {
                ds = OracleHelper.ExecuteDataset(
	                Configuration.connectionString,
	                CommandType.StoredProcedure,
                    "pkg_s_groups.proc_GetAllFunctionInGroup",
                new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
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