using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos;
using Oracle.DataAccess.Client;
using DataAccess;

namespace ModuleTrademark
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
                new OracleParameter("P_THANHVIEN_ND_TC", OracleDbType.Date, pInfo.THANHVIEN_ND_TC, ParameterDirection.Input),
                new OracleParameter("P_LOGOURL", OracleDbType.Varchar2, pInfo.LOGOURL, ParameterDirection.Input),
                new OracleParameter("P_DON_GIAY_DKNHCS", OracleDbType.Varchar2, pInfo.DON_GIAY_DKNHCS, ParameterDirection.Input),
                new OracleParameter("P_REF_APPNO", OracleDbType.Varchar2, pInfo.REF_APPNO, ParameterDirection.Input),
                new OracleParameter("P_APPCLASS_ID", OracleDbType.Decimal, pInfo.APPCLASS_ID, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID01", OracleDbType.Varchar2, pInfo.COUNTRY_ID01, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID02", OracleDbType.Varchar2, pInfo.COUNTRY_ID02, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID03", OracleDbType.Varchar2, pInfo.COUNTRY_ID03, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID04", OracleDbType.Varchar2, pInfo.COUNTRY_ID04, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID05", OracleDbType.Varchar2, pInfo.COUNTRY_ID05, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID06", OracleDbType.Varchar2, pInfo.COUNTRY_ID06, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID07", OracleDbType.Varchar2, pInfo.COUNTRY_ID07, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID08", OracleDbType.Varchar2, pInfo.COUNTRY_ID08, ParameterDirection.Input),
                new OracleParameter("P_LEPHI", OracleDbType.Decimal, pInfo.LEPHI, ParameterDirection.Input),
                new OracleParameter("P_TOKHAI_SOTRANG", OracleDbType.Decimal, pInfo.TOKHAI_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_TOKHAI_SOBAN", OracleDbType.Decimal, pInfo.TOKHAI_SOBAN, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SO", OracleDbType.Varchar2, pInfo.MAUDK_VPQT_SO, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_NGONNGU", OracleDbType.Varchar2, pInfo.MAUDK_VPQT_NGONNGU, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SOTRANG", OracleDbType.Decimal, pInfo.MAUDK_VPQT_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SOBAN", OracleDbType.Decimal, pInfo.MAUDK_VPQT_SOBAN, ParameterDirection.Input),
                new OracleParameter("P_MAUNDH_SOMAU", OracleDbType.Decimal, pInfo.MAUNDH_SOMAU, ParameterDirection.Input),
                new OracleParameter("P_BANSAO_YCCAP_GCN", OracleDbType.Varchar2, pInfo.BANSAO_YCCAP_GCN, ParameterDirection.Input),
                new OracleParameter("P_BANSAO_GIAYDK_NHCS", OracleDbType.Varchar2, pInfo.BANSAO_GIAYDK_NHCS, ParameterDirection.Input),
                new OracleParameter("P_BAN_CK_SD_NGANHANG", OracleDbType.Varchar2, pInfo.BAN_CK_SD_NGANHANG, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_NGONNGU", OracleDbType.Varchar2, pInfo.GIAY_UQ_NGONNGU, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_SOTRANG", OracleDbType.Decimal, pInfo.GIAY_UQ_BANDICH_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_BANGOC", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_BANGOC, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_BANSAO", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_BANSAO, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_THEOSO", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_THEOSO, ParameterDirection.Input),
                new OracleParameter("P_CHUNGTU_LEPHI", OracleDbType.Varchar2, pInfo.CHUNGTU_LEPHI, ParameterDirection.Input),
                new OracleParameter("P_TAILIEUBOSUNG", OracleDbType.Varchar2, pInfo.TAILIEUBOSUNG, ParameterDirection.Input),
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
                new OracleParameter("P_APPCLASS_ID", OracleDbType.Decimal, pInfo.APPCLASS_ID, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID01", OracleDbType.Varchar2, pInfo.COUNTRY_ID01, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID02", OracleDbType.Varchar2, pInfo.COUNTRY_ID02, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID03", OracleDbType.Varchar2, pInfo.COUNTRY_ID03, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID04", OracleDbType.Varchar2, pInfo.COUNTRY_ID04, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID05", OracleDbType.Varchar2, pInfo.COUNTRY_ID05, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID06", OracleDbType.Varchar2, pInfo.COUNTRY_ID06, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID07", OracleDbType.Varchar2, pInfo.COUNTRY_ID07, ParameterDirection.Input),
                new OracleParameter("P_COUNTRY_ID08", OracleDbType.Varchar2, pInfo.COUNTRY_ID08, ParameterDirection.Input),
                new OracleParameter("P_LEPHI", OracleDbType.Decimal, pInfo.LEPHI, ParameterDirection.Input),
                new OracleParameter("P_TOKHAI_SOTRANG", OracleDbType.Decimal, pInfo.TOKHAI_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_TOKHAI_SOBAN", OracleDbType.Decimal, pInfo.TOKHAI_SOBAN, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SO", OracleDbType.Varchar2, pInfo.MAUDK_VPQT_SO, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_NGONNGU", OracleDbType.Varchar2, pInfo.MAUDK_VPQT_NGONNGU, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SOTRANG", OracleDbType.Decimal, pInfo.MAUDK_VPQT_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_MAUDK_VPQT_SOBAN", OracleDbType.Decimal, pInfo.MAUDK_VPQT_SOBAN, ParameterDirection.Input),
                new OracleParameter("P_MAUNDH_SOMAU", OracleDbType.Decimal, pInfo.MAUNDH_SOMAU, ParameterDirection.Input),
                new OracleParameter("P_BANSAO_YCCAP_GCN", OracleDbType.Varchar2, pInfo.BANSAO_YCCAP_GCN, ParameterDirection.Input),
                new OracleParameter("P_BANSAO_GIAYDK_NHCS", OracleDbType.Varchar2, pInfo.BANSAO_GIAYDK_NHCS, ParameterDirection.Input),
                new OracleParameter("P_BAN_CK_SD_NGANHANG", OracleDbType.Varchar2, pInfo.BAN_CK_SD_NGANHANG, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_NGONNGU", OracleDbType.Varchar2, pInfo.GIAY_UQ_NGONNGU, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_SOTRANG", OracleDbType.Decimal, pInfo.GIAY_UQ_BANDICH_SOTRANG, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_BANGOC", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_BANGOC, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_BANSAO", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_BANSAO, ParameterDirection.Input),
                new OracleParameter("P_GIAY_UQ_BANDICH_THEOSO", OracleDbType.Varchar2, pInfo.GIAY_UQ_BANDICH_THEOSO, ParameterDirection.Input),
                new OracleParameter("P_CHUNGTU_LEPHI", OracleDbType.Varchar2, pInfo.CHUNGTU_LEPHI, ParameterDirection.Input),
                new OracleParameter("P_TAILIEUBOSUNG", OracleDbType.Varchar2, pInfo.TAILIEUBOSUNG, ParameterDirection.Input),
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
