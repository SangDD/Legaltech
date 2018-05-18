using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;

namespace DataAccess.ModuleTrademark
{
   public class AppDocumentDA
    {
        public int AppDocumentInsertBatch(List<AppDocumentInfo> pInfo, decimal pAppHeaderid)
        {
            try
            {
                int numberRecord = pInfo.Count;
                string[] Language = new string[numberRecord];
                decimal[] App_Header_Id = new decimal[numberRecord];
                string[] DocumentID = new string[numberRecord];
                decimal[] Isuse = new decimal[numberRecord];
                string[] Note = new string[numberRecord];
                string[] FileName = new string[numberRecord];
                string[] UrlHardCopy = new string[numberRecord];
                DateTime[] Document_Filling_Date = new DateTime[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    Language[i] = pInfo[i].Language_Code;
                    App_Header_Id[i] = pAppHeaderid;
                    DocumentID[i] = pInfo[i].Document_Id;
                    Isuse[i] = pInfo[i].Isuse;
                    Note[i] = pInfo[i].Note;
                    FileName[i] = pInfo[i].Filename;
                    UrlHardCopy[i] = pInfo[i].Url_Hardcopy;
                    Document_Filling_Date[i] = pInfo[i].Document_Filing_Date;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_INSERT", numberRecord,
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, Language, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENT_ID", OracleDbType.Varchar2, DocumentID, ParameterDirection.Input),
                    new OracleParameter("P_ISUSE", OracleDbType.Decimal, Isuse, ParameterDirection.Input),
                    new OracleParameter("P_NOTE", OracleDbType.Varchar2, Note, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENT_FILING_DATE", OracleDbType.Date, Document_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("P_FILENAME", OracleDbType.Varchar2, FileName, ParameterDirection.Input),
                    new OracleParameter("P_URL_HARDCOPY", OracleDbType.Varchar2, UrlHardCopy, ParameterDirection.Input),
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

        public int AppDocumentDeletedByID(decimal pID )
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_DEL_BY_ID",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
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

        public int AppHeaderDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_DEL_BY_APP",
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
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
