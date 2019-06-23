using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;

namespace DataAccess.ModuleTrademark
{
   public class AppImageDA
    {
      
        public int AppImageInsertBatch(List<AppDocumentOthersInfo> pInfo)
        {
            try
            {
                int numberRecord = pInfo.Count;
                string[] Language = new string[numberRecord];
                decimal[] App_Header_Id = new decimal[numberRecord];
                string[] IDRef = new string[numberRecord];
                string[] DocumentName = new string[numberRecord];
                string[] FileName = new string[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    Language[i] = pInfo[i].Language_Code;
                    App_Header_Id[i] = pInfo[i].App_Header_Id;
                    DocumentName[i] = pInfo[i].Documentname;
                    FileName[i] = pInfo[i].Filename;
                    IDRef[i] = pInfo[i].IdRef;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_image_public.proc_insert", numberRecord,
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENTNAME", OracleDbType.Varchar2, DocumentName, ParameterDirection.Input),
                    new OracleParameter("P_FILENAME", OracleDbType.Varchar2, FileName, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, Language, ParameterDirection.Input),
                    new OracleParameter("P_IDREF", OracleDbType.Decimal, IDRef, ParameterDirection.Input),
                    
                    paramReturn);
                var result = ErrorCode.Error;
                Oracle.DataAccess.Types.OracleDecimal[] _ArrReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in _ArrReturn)
                {
                    if (Convert.ToInt32(_item.ToString()) < 0)
                    {
                        result = Convert.ToInt32(_item.ToString());
                        break;
                    }
                    else
                    {
                        result = ErrorCode.Success;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppImageDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_image_public.PROC_DEL_BY_APP",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
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


        public int AppImageDelByID(decimal pID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_image_public.PROC_DEL_BY_ID",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
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
    }
}
