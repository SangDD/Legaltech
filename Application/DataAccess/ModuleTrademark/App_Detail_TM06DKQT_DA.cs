using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using DataAccess;

namespace DataAccess
{
  public  class App_Detail_TM06DKQT_DA
    {
        public int App_Detail_06TMDKQT_Insert(App_Detail_TM06DKQT_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_TM06DKQT.PROC_TM06DKQT_INSERT",
                new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                new OracleParameter("P_APP_HEADER_ID", OracleDbType.Varchar2, pInfo.APP_HEADER_ID, ParameterDirection.Input),
                new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pInfo.LANGUAGE_CODE, ParameterDirection.Input),
                new OracleParameter("P_APPNO", OracleDbType.Varchar2, pInfo.APPNO, ParameterDirection.Input),
                new OracleParameter("P_THANHVIEN_ND_TC", OracleDbType.Varchar2, pInfo.THANHVIEN_ND_TC, ParameterDirection.Input),
                new OracleParameter("P_LOGOURL", OracleDbType.Varchar2, pInfo.LOGOURL, ParameterDirection.Input),
                new OracleParameter("P_DON_GIAY_DKNHCS", OracleDbType.Varchar2, pInfo.DON_GIAY_DKNHCS, ParameterDirection.Input),
                new OracleParameter("P_REF_APPNO", OracleDbType.Varchar2, pInfo.REF_APPNO, ParameterDirection.Input),            
                new OracleParameter("P_COUNTRY_ID01", OracleDbType.Varchar2, pInfo.COUNTRY_ID01, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID02", OracleDbType.Varchar2, pInfo.COUNTRY_ID02, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID03", OracleDbType.Varchar2, pInfo.COUNTRY_ID03, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID04", OracleDbType.Varchar2, pInfo.COUNTRY_ID04, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID05", OracleDbType.Varchar2, pInfo.COUNTRY_ID05, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID06", OracleDbType.Varchar2, pInfo.COUNTRY_ID06, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID07", OracleDbType.Varchar2, pInfo.COUNTRY_ID07, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID08", OracleDbType.Varchar2, pInfo.COUNTRY_ID08, ParameterDirection.Input),
                new OracleParameter("P_LEPHI", OracleDbType.Decimal, pInfo.LEPHI, ParameterDirection.Input),               
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

        public int App_Detail_06TMDKQT_Update(App_Detail_TM06DKQT_Info pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_TM06DKQT.PROC_TM06DKQT_INSERT",
                new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                new OracleParameter("P_APP_HEADER_ID", OracleDbType.Varchar2, pInfo.APP_HEADER_ID, ParameterDirection.Input),
                new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pInfo.LANGUAGE_CODE, ParameterDirection.Input),
                new OracleParameter("P_APPNO", OracleDbType.Varchar2, pInfo.APPNO, ParameterDirection.Input),
                new OracleParameter("P_THANHVIEN_ND_TC", OracleDbType.Date, pInfo.THANHVIEN_ND_TC, ParameterDirection.Input),
                new OracleParameter("P_LOGOURL", OracleDbType.Varchar2, pInfo.LOGOURL, ParameterDirection.Input),
                new OracleParameter("P_DON_GIAY_DKNHCS", OracleDbType.Varchar2, pInfo.DON_GIAY_DKNHCS, ParameterDirection.Input),
                new OracleParameter("P_REF_APPNO", OracleDbType.Varchar2, pInfo.REF_APPNO, ParameterDirection.Input),             
                new OracleParameter("P_COUNTRY_ID01", OracleDbType.Varchar2, pInfo.COUNTRY_ID01, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID02", OracleDbType.Varchar2, pInfo.COUNTRY_ID02, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID03", OracleDbType.Varchar2, pInfo.COUNTRY_ID03, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID04", OracleDbType.Varchar2, pInfo.COUNTRY_ID04, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID05", OracleDbType.Varchar2, pInfo.COUNTRY_ID05, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID06", OracleDbType.Varchar2, pInfo.COUNTRY_ID06, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID07", OracleDbType.Varchar2, pInfo.COUNTRY_ID07, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID08", OracleDbType.Varchar2, pInfo.COUNTRY_ID08, ParameterDirection.Input),
                new OracleParameter("P_LEPHI", OracleDbType.Decimal, pInfo.LEPHI, ParameterDirection.Input),
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

        public DataSet AppTM06DKQTGetByID(decimal pAppHeaderId, string pLanguage, int pStatus)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_GET_DATA.PROC_APP_TM06DKQT_GET_BY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderId, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pStatus, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_CLASS_DETAIL", OracleDbType.RefCursor, ParameterDirection.Output)
                    );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


    }
}
