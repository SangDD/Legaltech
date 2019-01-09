namespace DataAccess.ModuleUsersAndRoles
{
    using System;
    using System.Data;

    using Common;
    using Common.MessageCode;
    using Common.SearchingAndFiltering;
    using ObjectInfos.ModuleUsersAndRoles;
    using Oracle.DataAccess.Client;
    using ObjectInfos;

    public class UserDA
    {
        public static DataSet GetUserById(int userId)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetById",
                    new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet GetBy_Email(string p_email)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetByEmail",
                    new OracleParameter("p_email", OracleDbType.Varchar2, p_email, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet GetUserByUsername(string username)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetByUsername",
                    new OracleParameter("p_username", OracleDbType.Varchar2, username, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet GetAllUsers()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetAll",
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet GetAllUserIdByGroupId(int groupId)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetAllUserId",
                    new OracleParameter("p_groupId", OracleDbType.Int32, groupId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet User_Search(string p_key_search, string p_from, string p_to, string p_column, string p_sort_type, ref decimal p_total_record)
        {
            try
            {

                string[] arrKeySearch = p_key_search.Split('|');
                string _key_search = "";
                foreach (string item in arrKeySearch)
                {
                    string _tem = item;
                    if (_tem == "")
                    {
                        _tem = "ALL";
                    }
                    if (_key_search == "")
                    {
                        _key_search = _tem;
                    }
                    else
                        _key_search = _key_search + "|" + _tem;
                }

                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_user_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, _key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),

                    new OracleParameter("p_column", OracleDbType.Varchar2, p_column, ParameterDirection.Input),
                    //new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));

                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
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
                var userName = Null.NullString; // Find by near value matching of  UserName
                var fullName = Null.NullString; // Find by near value matching of  FullName
                var departmentId = Null.NullString; // Array departmentId, split by ',' and user IN operator in sql for searching
                var type = Null.NullString; // Find by near value matching of  GroupId
                var status = Null.NullString; // Array Status, split by ',' and user IN operator in sql for searching
                if (!string.IsNullOrEmpty(keysSearch))
                {
                    var arrKeySearch = keysSearch.Split('|');
                    if (arrKeySearch.Length == 4)
                    {
                        userName = arrKeySearch[0];
                        fullName = arrKeySearch[1];
                        type = KeySearch.FilterComboboxValue(arrKeySearch[2]);
                        status = KeySearch.FilterComboboxValue(arrKeySearch[3]);
                    }
                }

                var paramTotalRecord = new OracleParameter("p_totalRecord", OracleDbType.Int32, ParameterDirection.Output);
                ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_Find",
                    new OracleParameter("p_userName", OracleDbType.Varchar2, userName, ParameterDirection.Input),
                    new OracleParameter("p_fullName", OracleDbType.Varchar2, fullName, ParameterDirection.Input),

                    new OracleParameter("p_type", OracleDbType.Varchar2, type, ParameterDirection.Input),
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

        public static DataSet GetUserByType(decimal p_user_type)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetBy_Type",
                     new OracleParameter("p_user_type", OracleDbType.Decimal, p_user_type, ParameterDirection.Input),
                     new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public static int AddUser(UserInfo userAdd, string GroupId)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_AddNew",
                    new OracleParameter("p_username", OracleDbType.Varchar2, userAdd.Username, ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, userAdd.Password, ParameterDirection.Input),
                    new OracleParameter("p_fullName", OracleDbType.Varchar2, userAdd.FullName, ParameterDirection.Input),
                    new OracleParameter("p_address", OracleDbType.Varchar2, userAdd.Address, ParameterDirection.Input),
                    new OracleParameter("p_DateOfBirth", OracleDbType.Date, userAdd.DateOfBirth, ParameterDirection.Input),
                    new OracleParameter("p_Sex", OracleDbType.Varchar2, userAdd.Sex, ParameterDirection.Input),
                    new OracleParameter("p_Email", OracleDbType.Varchar2, userAdd.Email, ParameterDirection.Input),
                    new OracleParameter("p_Phone", OracleDbType.Varchar2, userAdd.Phone, ParameterDirection.Input),
                    new OracleParameter("p_fax", OracleDbType.Varchar2, userAdd.Fax, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Int32, userAdd.Status, ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Int32, userAdd.Type, ParameterDirection.Input),

                    //
                    new OracleParameter("p_country", OracleDbType.Decimal, userAdd.Country, ParameterDirection.Input),
                    new OracleParameter("p_company_name", OracleDbType.NVarchar2, userAdd.Company_Name, ParameterDirection.Input),
                    new OracleParameter("p_main_business", OracleDbType.Varchar2, userAdd.Main_Business, ParameterDirection.Input),
                    new OracleParameter("p_title", OracleDbType.Varchar2, userAdd.Title, ParameterDirection.Input),
                    new OracleParameter("p_copyto", OracleDbType.Varchar2, userAdd.Copyto, ParameterDirection.Input),
                    new OracleParameter("p_face_link", OracleDbType.Varchar2, userAdd.Face_Link, ParameterDirection.Input),
                    new OracleParameter("p_linkedin_link", OracleDbType.Varchar2, userAdd.Linkedin_Link, ParameterDirection.Input),
                    new OracleParameter("p_wechat_link", OracleDbType.Varchar2, userAdd.Wechat_Link, ParameterDirection.Input),
                    new OracleParameter("p_other_link", OracleDbType.Varchar2, userAdd.Other_Link, ParameterDirection.Input),

                    new OracleParameter("p_reason_select", OracleDbType.Decimal, userAdd.Reason_Select, ParameterDirection.Input),
                    new OracleParameter("p_request_credit", OracleDbType.Decimal, userAdd.Request_Credit, ParameterDirection.Input),
                    new OracleParameter("p_customer_code", OracleDbType.Varchar2, userAdd.Customer_Code, ParameterDirection.Input),
                    new OracleParameter("p_contact_person", OracleDbType.Varchar2, userAdd.Contact_Person, ParameterDirection.Input),

                    new OracleParameter("p_other_type", OracleDbType.Decimal, userAdd.Other_Type, ParameterDirection.Input),
                    new OracleParameter("p_hourly_rate", OracleDbType.Decimal, userAdd.Hourly_Rate, ParameterDirection.Input),
                    new OracleParameter("p_hourly_rate_usd", OracleDbType.Decimal, userAdd.Hourly_Rate_USD, ParameterDirection.Input),

                    new OracleParameter("p_GroupId", OracleDbType.Varchar2, GroupId, ParameterDirection.Input),
                    new OracleParameter("p_createdby", OracleDbType.Varchar2, userAdd.CreatedBy, ParameterDirection.Input),
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

        public static int EditUser(UserInfo userEdit, string GroupId)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_Edit",
                    new OracleParameter("p_userId", OracleDbType.Int32, userEdit.Id, ParameterDirection.Input),
                    new OracleParameter("p_fullName", OracleDbType.Varchar2, userEdit.FullName, ParameterDirection.Input),
                    new OracleParameter("p_address", OracleDbType.Varchar2, userEdit.Address, ParameterDirection.Input),
                    new OracleParameter("p_DateOfBirth", OracleDbType.Date, userEdit.DateOfBirth, ParameterDirection.Input),
                    new OracleParameter("p_Sex", OracleDbType.Varchar2, userEdit.Sex, ParameterDirection.Input),
                    new OracleParameter("p_Email", OracleDbType.Varchar2, userEdit.Email, ParameterDirection.Input),
                    new OracleParameter("p_Phone", OracleDbType.Varchar2, userEdit.Phone, ParameterDirection.Input),
                    new OracleParameter("p_fax", OracleDbType.Varchar2, userEdit.Fax, ParameterDirection.Input),
                    new OracleParameter("p_Status", OracleDbType.Int32, userEdit.Status, ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Int32, userEdit.Type, ParameterDirection.Input),

                    //
                    new OracleParameter("p_country", OracleDbType.Decimal, userEdit.Country, ParameterDirection.Input),
                    new OracleParameter("p_company_name", OracleDbType.NVarchar2, userEdit.Company_Name, ParameterDirection.Input),
                    new OracleParameter("p_main_business", OracleDbType.Varchar2, userEdit.Main_Business, ParameterDirection.Input),
                    new OracleParameter("p_title", OracleDbType.Varchar2, userEdit.Title, ParameterDirection.Input),
                    new OracleParameter("p_copyto", OracleDbType.Varchar2, userEdit.Copyto, ParameterDirection.Input),
                    new OracleParameter("p_face_link", OracleDbType.Varchar2, userEdit.Face_Link, ParameterDirection.Input),
                    new OracleParameter("p_linkedin_link", OracleDbType.Varchar2, userEdit.Linkedin_Link, ParameterDirection.Input),
                    new OracleParameter("p_wechat_link", OracleDbType.Varchar2, userEdit.Wechat_Link, ParameterDirection.Input),
                    new OracleParameter("p_other_link", OracleDbType.Varchar2, userEdit.Other_Link, ParameterDirection.Input),

                    new OracleParameter("p_reason_select", OracleDbType.Decimal, userEdit.Reason_Select, ParameterDirection.Input),
                    new OracleParameter("p_request_credit", OracleDbType.Decimal, userEdit.Request_Credit, ParameterDirection.Input),
                    new OracleParameter("p_customer_code", OracleDbType.Varchar2, userEdit.Customer_Code, ParameterDirection.Input),
                    new OracleParameter("p_contact_person", OracleDbType.Varchar2, userEdit.Contact_Person, ParameterDirection.Input),
                    new OracleParameter("p_other_type", OracleDbType.Decimal, userEdit.Other_Type, ParameterDirection.Input),
                    new OracleParameter("p_hourly_rate", OracleDbType.Decimal, userEdit.Hourly_Rate, ParameterDirection.Input),
                    new OracleParameter("p_hourly_rate_usd", OracleDbType.Decimal, userEdit.Hourly_Rate_USD, ParameterDirection.Input),

                    new OracleParameter("p_GroupId", OracleDbType.Varchar2, GroupId, ParameterDirection.Input),
                    new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, userEdit.ModifiedBy, ParameterDirection.Input),
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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_Delete",
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
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_CheckLogin",
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
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetAllUserRoles",
                    new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static int ChangeUserPassword(UserInfo userInfo, string newPassword)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_ChangePassword",
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
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_User_GetAllSelfGroup",
                      new OracleParameter("p_userId", OracleDbType.Int32, userId, ParameterDirection.Input),
                      new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public int DoResetPass(string p_user_name, string p_password, string p_re_password, string p_modifiedBy)
        {

            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_s_users.proc_user_reset_pass",
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_password", OracleDbType.Varchar2, p_password, ParameterDirection.Input),
                    new OracleParameter("p_re_password", OracleDbType.Varchar2, p_re_password, ParameterDirection.Input),
                    new OracleParameter("p_modifiedBy", OracleDbType.Varchar2, p_modifiedBy, ParameterDirection.Input),
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


        #region Dang ky thong tin thanh vien ngoai trang portal 
        public static int RegisterInsert(RegisterInfo pRegistor)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_INSERT",
                    new OracleParameter("P_FISTNAME", OracleDbType.Varchar2, pRegistor.FistName, ParameterDirection.Input),
                    new OracleParameter("P_LASTNAME", OracleDbType.Varchar2, pRegistor.LastName, ParameterDirection.Input),
                    new OracleParameter("P_PHONE", OracleDbType.Varchar2, pRegistor.Phone, ParameterDirection.Input),
                    new OracleParameter("P_EMAIL", OracleDbType.Varchar2, pRegistor.Email, ParameterDirection.Input),
                    new OracleParameter("P_COMPANY", OracleDbType.Varchar2, pRegistor.Company, ParameterDirection.Input),
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


        public static int RegisterUpdate(RegisterInfo pRegistor)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pRegistor.Id, ParameterDirection.Input),
                    new OracleParameter("P_FISTNAME", OracleDbType.Varchar2, pRegistor.FistName, ParameterDirection.Input),
                    new OracleParameter("P_LASTNAME", OracleDbType.Varchar2, pRegistor.LastName, ParameterDirection.Input),
                    new OracleParameter("P_PHONE", OracleDbType.Varchar2, pRegistor.Phone, ParameterDirection.Input),
                    new OracleParameter("P_EMAIL", OracleDbType.Varchar2, pRegistor.Email, ParameterDirection.Input),
                    new OracleParameter("P_COMPANY", OracleDbType.Varchar2, pRegistor.Company, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDBY", OracleDbType.Varchar2, pRegistor.Modifiedby, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Int32, pRegistor.Status, ParameterDirection.Input),
                    new OracleParameter("P_KEYSECRET", OracleDbType.Varchar2, pRegistor.KeySecret, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
           
        }

        public static int RegisterDeleted(decimal pId , string pModified)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_DELETED",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pId, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIEDBY", OracleDbType.Varchar2, pModified, ParameterDirection.Input),
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


        public static DataSet RegisterGetAll(string pSearchKey, int pFrom, int pTo,ref  decimal pTotalRecord )
        {
            try
            {
              var paramReturn = new OracleParameter("P_TOTAL", OracleDbType.Int32, ParameterDirection.Output);
                    paramReturn.Size =10;
                    DataSet ds  = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_GETALL",
                    new OracleParameter("P_SEARCH_KEY", OracleDbType.Varchar2, pSearchKey, ParameterDirection.Input),
                    new OracleParameter("P_FROM", OracleDbType.Int32, pFrom, ParameterDirection.Input),
                    new OracleParameter("P_TO", OracleDbType.Int32, pTo, ParameterDirection.Input),
                    paramReturn ,
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
              pTotalRecord = Convert.ToInt32(paramReturn.Value.ToString());
              return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet RegisterGetById(decimal pID)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_GETBYID",
                new OracleParameter("P_ID", OracleDbType.Int32, pID, ParameterDirection.Input),    
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public static DataSet RegisterGetByCaseCode(string p_casecode)
        {
            try
            {
                DataSet ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_REGISTOR.PROC_REGISTOR_CASECODE",
                new OracleParameter("P_CASECODE", OracleDbType.Varchar2, p_casecode, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        #endregion
    }
}
