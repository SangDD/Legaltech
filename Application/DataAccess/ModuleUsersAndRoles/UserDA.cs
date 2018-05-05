namespace DataAccess.ModuleUsersAndRoles
{
	using System;
	using System.Data;

	using Common;
	using Common.MessageCode;
	using Common.SearchingAndFiltering;

	using ObjectInfos.ModuleUsersAndRoles;

	using Oracle.DataAccess.Client;

	public class UserDA
	{
		public static DataSet GetUserById(int userId)
        {
	        var ds = new DataSet();
	        try
	        {
		        ds = OracleHelper.ExecuteDataset(
			        Configuration.connectionString,
			        CommandType.StoredProcedure,
			        "pkg_s_users.proc_User_GetById",
			        new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
			        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
	        }
	        catch (Exception ex)
	        {
		        Logger.LogException(ex);
	        }

	        return ds;
        }

		public static DataSet GetUserByUsername(string username)
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_users.proc_User_GetByUsername",
					new OracleParameter("p_username", OracleDbType.Varchar2, username, ParameterDirection.Input),
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return ds;
		}

        public static DataSet GetAllUsers()
        {
	        var ds = new DataSet();
	        try
	        {
		        ds = OracleHelper.ExecuteDataset(
			        Configuration.connectionString,
			        CommandType.StoredProcedure,
			        "pkg_s_users.proc_User_GetAll",
			        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
	        }
	        catch (Exception ex)
	        {
		        Logger.LogException(ex);
	        }

	        return ds;
        }

        public static DataSet GetAllUserIdByGroupId(int groupId)
        {
            var ds = new DataSet();
            try
            {
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_GetAllUserId",
		            new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
		            new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return ds;
        }

		/// <summary>
		/// Find all user by key search
		/// </summary>
		/// <param name="keysSearch">contain collection of parameter of condition searching, split by '|'</param>
		/// <param name="options">contain collection of parameter to custom value return</param>
		/// <param name="totalRecord">Total record matching condition</param>
		/// <returns>Data set contain rows matching</returns>
		public static DataSet FindUser(string keysSearch, OptionFilter options, ref int totalRecord)
        {
            var ds = new DataSet();
            try
            {
                // Keys search
                var userName        = Null.NullString; // Find by near value matching of  UserName
	            var fullName        = Null.NullString; // Find by near value matching of  FullName
	            var departmentId    = Null.NullString; // Array departmentId, split by ',' and user IN operator in sql for searching
	            var positionId      = Null.NullString; // Array positionId, split by ',' and user IN operator in sql for searching
	            var branchId        = Null.NullString; // Array branchId, split by ',' and user IN operator in sql for searching
	            var wareHouseId     = Null.NullString; // Array wareHouseId, split by ',' and user IN operator in sql for searching
	            var productMarkCode = Null.NullString; // Array productMarkCode, split by ',' and user IN operator in sql for searching
	            var groupId         = Null.NullString; // Find by near value matching of  GroupId
	            var status          = Null.NullString; // Array Status, split by ',' and user IN operator in sql for searching
                if (!string.IsNullOrEmpty(keysSearch))
                {
                    var arrKeySearch = keysSearch.Split('|');
                    if (arrKeySearch.Length == 9)
                    {
                        userName = arrKeySearch[0];
	                    fullName = arrKeySearch[1];
	                    departmentId = KeySearch.FilterComboboxValue(arrKeySearch[2]);
	                    positionId = KeySearch.FilterComboboxValue(arrKeySearch[3]);
	                    branchId = KeySearch.FilterComboboxValue(arrKeySearch[4]);
	                    wareHouseId = KeySearch.FilterComboboxValue(arrKeySearch[5]);
	                    productMarkCode = KeySearch.FilterComboboxValue(arrKeySearch[6]);
	                    groupId = KeySearch.FilterComboboxValue(arrKeySearch[7]);
	                    status = KeySearch.FilterComboboxValue(arrKeySearch[8]);
                    }
                }

	            var paramTotalRecord = new OracleParameter("p_totalRecord", OracleDbType.Int32, ParameterDirection.Output);
	            ds = OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_Find",
		            new OracleParameter("p_userName", OracleDbType.Varchar2, userName, ParameterDirection.Input),
		            new OracleParameter("p_fullName", OracleDbType.Varchar2, fullName, ParameterDirection.Input),
		            new OracleParameter("p_departmentId", OracleDbType.Varchar2, departmentId, ParameterDirection.Input),
		            new OracleParameter("p_positionId", OracleDbType.Varchar2, positionId, ParameterDirection.Input),
		            new OracleParameter("p_branchId", OracleDbType.Varchar2, branchId, ParameterDirection.Input),
		            new OracleParameter("p_wareHouseId", OracleDbType.Varchar2, wareHouseId, ParameterDirection.Input),
		            new OracleParameter("p_productMarkCode", OracleDbType.Varchar2, productMarkCode, ParameterDirection.Input),
		            new OracleParameter("p_groupId", OracleDbType.Varchar2, groupId, ParameterDirection.Input),
		            new OracleParameter("p_status", OracleDbType.Varchar2, status, ParameterDirection.Input),

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

        public static int AddUser(UserInfo userAdd, string arrGroupId)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_AddNew",
		            new OracleParameter("p_username", OracleDbType.Varchar2, userAdd.Username, ParameterDirection.Input),
		            new OracleParameter("p_password", OracleDbType.Varchar2, userAdd.Password, ParameterDirection.Input),
		            new OracleParameter("p_fullName", OracleDbType.Varchar2, userAdd.FullName, ParameterDirection.Input),
		            new OracleParameter("p_DateOfBirth", OracleDbType.Date, userAdd.DateOfBirth, ParameterDirection.Input),
		            new OracleParameter("p_Sex", OracleDbType.Varchar2, userAdd.Sex, ParameterDirection.Input),
		            new OracleParameter("p_Email", OracleDbType.Varchar2, userAdd.Email, ParameterDirection.Input),
		            new OracleParameter("p_Phone", OracleDbType.Varchar2, userAdd.Phone, ParameterDirection.Input),
		            new OracleParameter("p_PositionId", OracleDbType.Int32, userAdd.PositionId, ParameterDirection.Input),
		            new OracleParameter("p_DepartmentId", OracleDbType.Int32, userAdd.DepartmentId, ParameterDirection.Input),
		            new OracleParameter("p_BranchId", OracleDbType.Int32, userAdd.BranchId, ParameterDirection.Input),
		            new OracleParameter("p_WareHouseId", OracleDbType.Int32, userAdd.WareHouseId, ParameterDirection.Input),
		            new OracleParameter("p_UnitPriceType", OracleDbType.Varchar2, userAdd.UnitPriceType, ParameterDirection.Input),
		            new OracleParameter("p_ViewOtherBranch", OracleDbType.Varchar2, userAdd.ViewOtherBranch, ParameterDirection.Input),
		            new OracleParameter("p_SeeProductTypes", OracleDbType.Varchar2, userAdd.SeeProductTypeS, ParameterDirection.Input),
		            new OracleParameter("p_ChangeInstance", OracleDbType.Varchar2, userAdd.ChangeInstanceWhenOutStock, ParameterDirection.Input),
		            new OracleParameter("p_ProductMarkCode", OracleDbType.Varchar2, userAdd.ProductMarkCode, ParameterDirection.Input),
		            new OracleParameter("p_Status", OracleDbType.Int32, userAdd.Status, ParameterDirection.Input),
		            new OracleParameter("p_createdby", OracleDbType.Varchar2, userAdd.CreatedBy, ParameterDirection.Input),
		            new OracleParameter("p_arrGroupId", OracleDbType.Varchar2, arrGroupId, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.AddUserFail.GetCode();
        }

        public static int EditUser(UserInfo userEdit, string arrGroupId)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_Edit",
					new OracleParameter("p_userId", OracleDbType.Int32, userEdit.Id, ParameterDirection.Input),
		            new OracleParameter("p_fullName", OracleDbType.Varchar2, userEdit.FullName, ParameterDirection.Input),
		            new OracleParameter("p_DateOfBirth", OracleDbType.Date, userEdit.DateOfBirth, ParameterDirection.Input),
		            new OracleParameter("p_Sex", OracleDbType.Varchar2, userEdit.Sex, ParameterDirection.Input),
		            new OracleParameter("p_Email", OracleDbType.Varchar2, userEdit.Email, ParameterDirection.Input),
		            new OracleParameter("p_Phone", OracleDbType.Varchar2, userEdit.Phone, ParameterDirection.Input),
		            new OracleParameter("p_PositionId", OracleDbType.Int32, userEdit.PositionId, ParameterDirection.Input),
		            new OracleParameter("p_DepartmentId", OracleDbType.Int32, userEdit.DepartmentId, ParameterDirection.Input),
		            new OracleParameter("p_BranchId", OracleDbType.Int32, userEdit.BranchId, ParameterDirection.Input),
		            new OracleParameter("p_WareHouseId", OracleDbType.Int32, userEdit.WareHouseId, ParameterDirection.Input),
		            new OracleParameter("p_UnitPriceType", OracleDbType.Varchar2, userEdit.UnitPriceType, ParameterDirection.Input),
		            new OracleParameter("p_ViewOtherBranch", OracleDbType.Varchar2, userEdit.ViewOtherBranch, ParameterDirection.Input),
		            new OracleParameter("p_SeeProductTypes", OracleDbType.Varchar2, userEdit.SeeProductTypeS, ParameterDirection.Input),
		            new OracleParameter("p_ChangeInstance", OracleDbType.Varchar2, userEdit.ChangeInstanceWhenOutStock, ParameterDirection.Input),
		            new OracleParameter("p_ProductMarkCode", OracleDbType.Varchar2, userEdit.ProductMarkCode, ParameterDirection.Input),
		            new OracleParameter("p_Status", OracleDbType.Int32, userEdit.Status, ParameterDirection.Input),
		            new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, userEdit.ModifiedBy, ParameterDirection.Input),
		            new OracleParameter("p_arrGroupId", OracleDbType.Varchar2, arrGroupId, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.EditUserFail.GetCode();
        }

        public static int DeleteUser(int userId, string modifiedBy)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_Delete",
		            new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
		            new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, modifiedBy, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

            return KnMessageCode.DeleteUserFail.GetCode();
        }

        public static int CheckUserLogin(string userName, string password)
        {
            try
            {
	            var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
	            OracleHelper.ExecuteDataset(
		            Configuration.connectionString,
		            CommandType.StoredProcedure,
		            "pkg_s_users.proc_User_CheckLogin",
		            new OracleParameter("p_username", OracleDbType.Varchar2, userName, ParameterDirection.Input),
		            new OracleParameter("p_password", OracleDbType.Varchar2, password, ParameterDirection.Input),
		            paramReturn);
	            var result = Convert.ToInt32(paramReturn.Value.ToString());
	            return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }

	        return KnMessageCode.LoginFailed.GetCode();
        }

        public static DataSet GetAllUserRoles(int userId)
        {
	        var ds = new DataSet();
	        try
	        {
		        ds = OracleHelper.ExecuteDataset(
			        Configuration.connectionString,
			        CommandType.StoredProcedure,
			        "pkg_s_users.proc_User_GetAllUserRoles",
			        new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
			        new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
	        }
	        catch (Exception ex)
	        {
		        Logger.LogException(ex);
	        }

	        return ds;
        }

		public static int ChangeUserPassword(UserInfo userInfo, string newPassword)
		{
			try
			{
				var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
				OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_users.proc_User_ChangePassword",
					new OracleParameter("p_userId", OracleDbType.Int32, userInfo.Id, ParameterDirection.Input),
					new OracleParameter("p_oldPwd", OracleDbType.Varchar2, userInfo.Password, ParameterDirection.Input),
					new OracleParameter("p_newPwd", OracleDbType.Varchar2, newPassword, ParameterDirection.Input),
					new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, userInfo.ModifiedBy, ParameterDirection.Input),
					paramReturn);
				var result = Convert.ToInt32(paramReturn.Value.ToString());
				return result;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return KnMessageCode.ChangePasswordUserFail.GetCode();
		}

		public static DataSet GetUserSelfGroups(int userId)
		{
			var ds = new DataSet();
			try
			{
				ds = OracleHelper.ExecuteDataset(
					Configuration.connectionString,
					CommandType.StoredProcedure,
					"pkg_s_users.proc_User_GetAllSelfGroup",
					new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
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
