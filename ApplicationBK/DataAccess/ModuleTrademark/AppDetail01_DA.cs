using Common;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess.ModuleTrademark
{
    public class AppDetail01_DA
    {

        public int AppDetailInsert(AppDetail01Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_01.PROC_APP_DETAIL_01_INSERT",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("P_REQUEST", OracleDbType.Varchar2, pInfo.Request, ParameterDirection.Input),
                    new OracleParameter("P_CORRECT_REQUEST", OracleDbType.Varchar2, pInfo.Correct_Request, ParameterDirection.Input),
                    new OracleParameter("P_CORRECT_REQUEST_TO", OracleDbType.Varchar2, pInfo.Correct_Request_To, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_APPNO", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("P_SOCHUNGTU_NOPTHEO", OracleDbType.Varchar2, pInfo.Sochungtu_Noptheo, ParameterDirection.Input),
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


        public int AppDetailUpDate(AppDetail01Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_01.PROC_APP_DETAIL_01_UPDATE",
                     new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                    new OracleParameter("P_REQUEST", OracleDbType.Varchar2, pInfo.Request, ParameterDirection.Input),
                    new OracleParameter("P_CORRECT_REQUEST", OracleDbType.Varchar2, pInfo.Correct_Request, ParameterDirection.Input),
                    new OracleParameter("P_CORRECT_REQUEST_TO", OracleDbType.Varchar2, pInfo.Correct_Request_To, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                    new OracleParameter("P_APPNO", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                    new OracleParameter("P_SOCHUNGTU_NOPTHEO", OracleDbType.Varchar2, pInfo.Sochungtu_Noptheo, ParameterDirection.Input),
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


        public int AppDetailDeleted(decimal pID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_HEADER.PROC_APP_DETAIL_01_DELETED",
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
    }
}
