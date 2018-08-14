﻿using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Data;

namespace DataAccess.ModuleTrademark
{
    public class Application_Header_DA
    {
        public DataSet ApplicationHeader_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_header.proc_application_header_search",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
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

        public int AppHeader_Update_Status(decimal p_id, decimal p_status, string p_notes, string p_Modify_By, DateTime p_Modify_Date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_header.proc_update_status",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_Modify_By", OracleDbType.Varchar2, p_Modify_By, ParameterDirection.Input),
                    new OracleParameter("p_Modify_Date", OracleDbType.Date, p_Modify_Date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppHeader_Filing_Status(decimal p_id, decimal p_status, string p_app_no, DateTime p_filing_date, string p_url_copy, string p_notes, string p_Modify_By, DateTime p_Modify_Date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_header.proc_update_filing",
                    new OracleParameter("p_id", OracleDbType.Decimal, p_id, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_app_no", OracleDbType.Varchar2, p_app_no, ParameterDirection.Input),
                    new OracleParameter("p_filing_date", OracleDbType.Date, p_filing_date, ParameterDirection.Input),
                    new OracleParameter("p_url_copy", OracleDbType.Varchar2, p_url_copy, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_Modify_By", OracleDbType.Varchar2, p_Modify_By, ParameterDirection.Input),
                    new OracleParameter("p_Modify_Date", OracleDbType.Date, p_Modify_Date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns>TRA RA ID CUA BANG KHI INSERT THANH CONG</returns>
        public int AppHeaderInsert(ApplicationHeaderInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_HEADER.PROC_APP_HEADER_INSERT",
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_NAME", OracleDbType.Varchar2, pInfo.Master_Name, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_ADDRESS", OracleDbType.Varchar2, pInfo.Master_Address, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_PHONE", OracleDbType.Varchar2, pInfo.Master_Phone, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_FAX", OracleDbType.Varchar2, pInfo.Master_Fax, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_EMAIL", OracleDbType.Varchar2, pInfo.Master_Email, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_TYPE", OracleDbType.Varchar2, pInfo.Rep_Master_Type, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_NAME", OracleDbType.Varchar2, pInfo.Rep_Master_Name, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_ADDRESS", OracleDbType.Varchar2, pInfo.Rep_Master_Address, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_PHONE", OracleDbType.Varchar2, pInfo.Rep_Master_Phone, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_FAX", OracleDbType.Varchar2, pInfo.Rep_Master_Fax, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_EMAIL", OracleDbType.Varchar2, pInfo.Rep_Master_Email, ParameterDirection.Input),
                    new OracleParameter("P_SEND_DATE", OracleDbType.Date, pInfo.Send_Date, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Int32, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_BY", OracleDbType.Varchar2, pInfo.Created_By, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_DATE", OracleDbType.Date, pInfo.Created_Date, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGUE_CODE", OracleDbType.Varchar2, pInfo.Languague_Code, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS", OracleDbType.Varchar2, pInfo.Address, ParameterDirection.Input),
                    new OracleParameter("P_DATENO", OracleDbType.Varchar2, pInfo.DateNo, ParameterDirection.Input),
                    new OracleParameter("P_MONTHS", OracleDbType.Varchar2, pInfo.Months, ParameterDirection.Input),
                    new OracleParameter("P_YEARS", OracleDbType.Varchar2, pInfo.Years, ParameterDirection.Input),
                    new OracleParameter("p_client_reference", OracleDbType.Varchar2, pInfo.Client_Reference, ParameterDirection.Input),
                    new OracleParameter("p_case_name", OracleDbType.Varchar2, pInfo.Case_Name, ParameterDirection.Input),
                    new OracleParameter("P_DDSHCN", OracleDbType.Varchar2, pInfo.DDSHCN, ParameterDirection.Input),
                    
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppHeaderUpdate(ApplicationHeaderInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_HEADER.PROC_APP_HEADER_UPDATE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_NAME", OracleDbType.Varchar2, pInfo.Master_Name, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_ADDRESS", OracleDbType.Varchar2, pInfo.Master_Address, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_PHONE", OracleDbType.Varchar2, pInfo.Master_Phone, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_FAX", OracleDbType.Varchar2, pInfo.Master_Fax, ParameterDirection.Input),
                    new OracleParameter("P_MASTER_EMAIL", OracleDbType.Varchar2, pInfo.Master_Email, ParameterDirection.Input),

                    new OracleParameter("P_REP_MASTER_TYPE", OracleDbType.Varchar2, pInfo.Rep_Master_Type, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_NAME", OracleDbType.Varchar2, pInfo.Rep_Master_Name, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_ADDRESS", OracleDbType.Varchar2, pInfo.Rep_Master_Address, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_PHONE", OracleDbType.Varchar2, pInfo.Rep_Master_Phone, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_FAX", OracleDbType.Varchar2, pInfo.Rep_Master_Fax, ParameterDirection.Input),
                    new OracleParameter("P_REP_MASTER_EMAIL", OracleDbType.Varchar2, pInfo.Rep_Master_Email, ParameterDirection.Input),

                    new OracleParameter("P_SEND_DATE", OracleDbType.Date, pInfo.Send_Date, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Int32, pInfo.Status, ParameterDirection.Input),
                    new OracleParameter("P_MODIFY_BY", OracleDbType.Varchar2, pInfo.Modify_By, ParameterDirection.Input),
                    new OracleParameter("P_MODIFY_DATE", OracleDbType.Date, pInfo.Modify_Date, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGUE_CODE", OracleDbType.Varchar2, pInfo.Languague_Code, ParameterDirection.Input),
                    new OracleParameter("P_ADDRESS", OracleDbType.Varchar2, pInfo.Address, ParameterDirection.Input),
                    new OracleParameter("P_DATENO", OracleDbType.Varchar2, pInfo.DateNo, ParameterDirection.Input),
                    new OracleParameter("P_MONTHS", OracleDbType.Varchar2, pInfo.Months, ParameterDirection.Input),
                    new OracleParameter("P_YEARS", OracleDbType.Varchar2, pInfo.Years, ParameterDirection.Input),
                    new OracleParameter("p_client_reference", OracleDbType.Varchar2, pInfo.Client_Reference, ParameterDirection.Input),
                    new OracleParameter("p_case_name", OracleDbType.Varchar2, pInfo.Case_Name, ParameterDirection.Input),
                    new OracleParameter("P_DDSHCN", OracleDbType.Varchar2, pInfo.DDSHCN, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppHeaderDeleted(decimal pID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_HEADER.PROC_APP_HEADER_DELETED",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGUE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public DataSet LayThongTinKhachHang(string pUser,string pLanguage,string pAppCode)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_GET_CUSTOMER_INFO.PROC_THONG_TIN_CHU_DON_NDD",
                    new OracleParameter("P_USER", OracleDbType.Varchar2, pUser, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet GetMasterByAppNo(string p_appNo,string p_user_name, string p_languague_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_header.proc_getByAppNo",
                    new OracleParameter("p_appNo", OracleDbType.Varchar2, p_appNo, ParameterDirection.Input),
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_languague_code", OracleDbType.Varchar2, p_languague_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet GetApplicationHeader_ById(decimal p_Id, string p_languague_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_header.proc_getById",
                    new OracleParameter("p_Id", OracleDbType.Decimal, p_Id, ParameterDirection.Input),
                    new OracleParameter("p_languague_code", OracleDbType.Varchar2, p_languague_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet GetWarningData(string p_user_type)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_HEADER.PROC_GET_WARING_DATA",
                    new OracleParameter("P_USER_TYPE", OracleDbType.Varchar2, p_user_type, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }


    public class AppClassInfo_DA
    {
        public DataSet AppClassGetOnMemory()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_CLASS.PROC_APP_CLASS_GET_MEMORY",
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }
    }
}