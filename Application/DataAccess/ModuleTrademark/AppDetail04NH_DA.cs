using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Data;
namespace DataAccess.ModuleTrademark
{
    public class AppDetail04NH_DA
    {

        public int App_Detail_04NH_Insert(AppDetail04NHInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_04NH.PROC_APP_DETAIL_04NH_INSERT",
                new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                new OracleParameter("p_duadate", OracleDbType.Date, pInfo.Duadate, ParameterDirection.Input),
                new OracleParameter("p_logourl", OracleDbType.Varchar2, pInfo.Logourl, ParameterDirection.Input),
                new OracleParameter("p_dactichhanghoa", OracleDbType.Decimal, pInfo.Dactichhanghoa, ParameterDirection.Input),
                new OracleParameter("p_color", OracleDbType.Varchar2, pInfo.Color, ParameterDirection.Input),
                new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                new OracleParameter("p_huongquyenuutien", OracleDbType.Varchar2, pInfo.Huongquyenuutien, ParameterDirection.Input),
                new OracleParameter("p_sodon_ut", OracleDbType.Varchar2, pInfo.Sodon_Ut, ParameterDirection.Input),
                new OracleParameter("p_ngaynopdon_ut", OracleDbType.Date, pInfo.Ngaynopdon_Ut, ParameterDirection.Input),
                new OracleParameter("p_nuocnopdon_ut", OracleDbType.Varchar2, pInfo.Nuocnopdon_Ut, ParameterDirection.Input),
                new OracleParameter("p_nguongocdialy", OracleDbType.Varchar2, pInfo.Nguongocdialy, ParameterDirection.Input),
                new OracleParameter("p_chatluong", OracleDbType.Varchar2, pInfo.Chatluong, ParameterDirection.Input),
                new OracleParameter("p_dactinhkhac", OracleDbType.Varchar2, pInfo.Dactinhkhac, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_1", OracleDbType.Varchar2, pInfo.Cdk_Name_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_1", OracleDbType.Varchar2, pInfo.Cdk_Address_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_1", OracleDbType.Varchar2, pInfo.Cdk_Phone_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_1", OracleDbType.Varchar2, pInfo.Cdk_Fax_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_1", OracleDbType.Varchar2, pInfo.Cdk_Email_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_2", OracleDbType.Varchar2, pInfo.Cdk_Name_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_2", OracleDbType.Varchar2, pInfo.Cdk_Address_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_2", OracleDbType.Varchar2, pInfo.Cdk_Phone_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_2", OracleDbType.Varchar2, pInfo.Cdk_Fax_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_2", OracleDbType.Varchar2, pInfo.Cdk_Email_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_3", OracleDbType.Varchar2, pInfo.Cdk_Name_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_3", OracleDbType.Varchar2, pInfo.Cdk_Address_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_3", OracleDbType.Varchar2, pInfo.Cdk_Phone_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_3", OracleDbType.Varchar2, pInfo.Cdk_Fax_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_3", OracleDbType.Varchar2, pInfo.Cdk_Email_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_4", OracleDbType.Varchar2, pInfo.Cdk_Name_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_4", OracleDbType.Varchar2, pInfo.Cdk_Address_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_4", OracleDbType.Varchar2, pInfo.Cdk_Phone_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_4", OracleDbType.Varchar2, pInfo.Cdk_Fax_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_4", OracleDbType.Varchar2, pInfo.Cdk_Email_4, ParameterDirection.Input),
                new OracleParameter("p_used_special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),

                new OracleParameter("P_LOAINHANHIEU", OracleDbType.Varchar2, pInfo.LoaiNhanHieu, ParameterDirection.Input),
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


        public int App_Detail_04NH_Update(AppDetail04NHInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_04NH.PROC_APP_DETAIL_04NH_UPDATE",
                new OracleParameter("P_ID", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                new OracleParameter("p_app_header_id", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                new OracleParameter("p_appcode", OracleDbType.Varchar2, pInfo.Appcode, ParameterDirection.Input),
                new OracleParameter("p_language_code", OracleDbType.Varchar2, pInfo.Language_Code, ParameterDirection.Input),
                new OracleParameter("p_appno", OracleDbType.Varchar2, pInfo.Appno, ParameterDirection.Input),
                new OracleParameter("p_duadate", OracleDbType.Date, pInfo.Duadate, ParameterDirection.Input),
                new OracleParameter("p_logourl", OracleDbType.Varchar2, pInfo.Logourl, ParameterDirection.Input),
                new OracleParameter("p_dactichhanghoa", OracleDbType.Decimal, pInfo.Dactichhanghoa, ParameterDirection.Input),
                new OracleParameter("p_color", OracleDbType.Varchar2, pInfo.Color, ParameterDirection.Input),
                new OracleParameter("p_description", OracleDbType.Varchar2, pInfo.Description, ParameterDirection.Input),
                new OracleParameter("p_huongquyenuutien", OracleDbType.Varchar2, pInfo.Huongquyenuutien, ParameterDirection.Input),
                new OracleParameter("p_sodon_ut", OracleDbType.Varchar2, pInfo.Sodon_Ut, ParameterDirection.Input),
                new OracleParameter("p_ngaynopdon_ut", OracleDbType.Date, pInfo.Ngaynopdon_Ut, ParameterDirection.Input),
                new OracleParameter("p_nuocnopdon_ut", OracleDbType.Varchar2, pInfo.Nuocnopdon_Ut, ParameterDirection.Input),
                new OracleParameter("p_nguongocdialy", OracleDbType.Varchar2, pInfo.Nguongocdialy, ParameterDirection.Input),
                new OracleParameter("p_chatluong", OracleDbType.Varchar2, pInfo.Chatluong, ParameterDirection.Input),
                new OracleParameter("p_dactinhkhac", OracleDbType.Varchar2, pInfo.Dactinhkhac, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_1", OracleDbType.Varchar2, pInfo.Cdk_Name_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_1", OracleDbType.Varchar2, pInfo.Cdk_Address_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_1", OracleDbType.Varchar2, pInfo.Cdk_Phone_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_1", OracleDbType.Varchar2, pInfo.Cdk_Fax_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_1", OracleDbType.Varchar2, pInfo.Cdk_Email_1, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_2", OracleDbType.Varchar2, pInfo.Cdk_Name_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_2", OracleDbType.Varchar2, pInfo.Cdk_Address_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_2", OracleDbType.Varchar2, pInfo.Cdk_Phone_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_2", OracleDbType.Varchar2, pInfo.Cdk_Fax_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_2", OracleDbType.Varchar2, pInfo.Cdk_Email_2, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_3", OracleDbType.Varchar2, pInfo.Cdk_Name_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_3", OracleDbType.Varchar2, pInfo.Cdk_Address_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_3", OracleDbType.Varchar2, pInfo.Cdk_Phone_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_3", OracleDbType.Varchar2, pInfo.Cdk_Fax_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_3", OracleDbType.Varchar2, pInfo.Cdk_Email_3, ParameterDirection.Input),
                new OracleParameter("p_cdk_name_4", OracleDbType.Varchar2, pInfo.Cdk_Name_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_address_4", OracleDbType.Varchar2, pInfo.Cdk_Address_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_phone_4", OracleDbType.Varchar2, pInfo.Cdk_Phone_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_fax_4", OracleDbType.Varchar2, pInfo.Cdk_Fax_4, ParameterDirection.Input),
                new OracleParameter("p_cdk_email_4", OracleDbType.Varchar2, pInfo.Cdk_Email_4, ParameterDirection.Input),
                new OracleParameter("p_used_special", OracleDbType.Decimal, pInfo.Used_Special, ParameterDirection.Input),
                new OracleParameter("P_LOAINHANHIEU", OracleDbType.Varchar2, pInfo.LoaiNhanHieu, ParameterDirection.Input),
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


        public int App_Detail_04NH_Deleted(decimal pAppHeaderID, string pAppCode, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_04NH.PROC_APP_DETAIL_04NH_DELETE",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
                     new OracleParameter("P_APPCODE", OracleDbType.Varchar2, pAppCode, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
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


        public DataSet AppTM04NHGetByID(decimal pAppHeaderId, string pLanguage,int pStatus)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_GET_DATA.PROC_APP_TM04NH_GET_BY_ID",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderId, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, pStatus, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_DOC", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_OTHER", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_CLASS_DETAIL", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_C_FEE", OracleDbType.RefCursor, ParameterDirection.Output)
                    );
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public DataSet AppTM04NHSearchByStatus(int p_status)
        {
            try
            {
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DETAIL_04NH.PROC_APP_04NH_SEARCH",
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output)
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
