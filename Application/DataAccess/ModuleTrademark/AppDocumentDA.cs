using System;
using System.Collections.Generic;
using System.Data;
using Common;
using ObjectInfos;
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
                decimal[] Status = new decimal[numberRecord];
                string[] Note = new string[numberRecord];
                string[] FileName = new string[numberRecord];
                string[] UrlHardCopy = new string[numberRecord];

                string[] Char01 = new string[numberRecord];
                string[] Char02 = new string[numberRecord];
                string[] Char03 = new string[numberRecord];
                string[] Char04 = new string[numberRecord];
                string[] Char05 = new string[numberRecord];

                DateTime[] Document_Filling_Date = new DateTime[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    Language[i] = pInfo[i].Language_Code;
                    App_Header_Id[i] = pAppHeaderid;
                    DocumentID[i] = pInfo[i].Document_Id;
                    Isuse[i] = pInfo[i].Isuse;
                    Note[i] = pInfo[i].Note;
                    Status[i] = pInfo[i].Status;
                    FileName[i] = pInfo[i].Filename;
                    UrlHardCopy[i] = pInfo[i].Url_Hardcopy;
                    Document_Filling_Date[i] = pInfo[i].Document_Filing_Date;
                    Char01[i] = pInfo[i].CHAR01;
                    Char02[i] = pInfo[i].CHAR02;
                    Char03[i] = pInfo[i].CHAR03;
                    Char04[i] = pInfo[i].CHAR04;
                    Char05[i] = pInfo[i].CHAR05;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_INSERT", numberRecord,
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, Language, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENT_ID", OracleDbType.Varchar2, DocumentID, ParameterDirection.Input),
                    new OracleParameter("P_ISUSE", OracleDbType.Decimal, Isuse, ParameterDirection.Input),
                    new OracleParameter("P_NOTE", OracleDbType.Varchar2, Note, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, Status, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENT_FILING_DATE", OracleDbType.Date, Document_Filling_Date, ParameterDirection.Input),
                    new OracleParameter("P_FILENAME", OracleDbType.Varchar2, FileName, ParameterDirection.Input),
                    new OracleParameter("P_URL_HARDCOPY", OracleDbType.Varchar2, UrlHardCopy, ParameterDirection.Input),
                    new OracleParameter("P_CHAR01", OracleDbType.Varchar2, Char01, ParameterDirection.Input),
                    new OracleParameter("P_CHAR02", OracleDbType.Varchar2, Char02, ParameterDirection.Input),
                    new OracleParameter("P_CHAR03", OracleDbType.Varchar2, Char03, ParameterDirection.Input),
                    new OracleParameter("P_CHAR04", OracleDbType.Varchar2, Char04, ParameterDirection.Input),
                    new OracleParameter("P_CHAR05", OracleDbType.Varchar2, Char05, ParameterDirection.Input),
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

        public int AppDocumentDeletedByID(decimal pID,string pLanguage,decimal pAppHeaderID )
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_DELETE",
                    new OracleParameter("P_ID", OracleDbType.Decimal, pID, ParameterDirection.Input),
                     new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),

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

        public int AppDocumentDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOCUMENT.PROC_APP_DOCUMENT_DEL_BY_APP",
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pAppHeaderID, ParameterDirection.Input),
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

        public int AppDocumentOtherInsertBatch(List<AppDocumentOthersInfo> pInfo)
        {
            try
            {
                int numberRecord = pInfo.Count;
                string[] Language = new string[numberRecord];
                decimal[] App_Header_Id = new decimal[numberRecord];
                string[] DocumentName = new string[numberRecord];
                string[] FileName = new string[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    Language[i] = pInfo[i].Language_Code;
                    App_Header_Id[i] = pInfo[i].App_Header_Id;
                    DocumentName[i] = pInfo[i].Documentname;
                    FileName[i] = pInfo[i].Filename;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOC_OTHERS.PROC_APP_DOC_OTHER_INSERT", numberRecord,
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_DOCUMENTNAME", OracleDbType.Varchar2, DocumentName, ParameterDirection.Input),
                    new OracleParameter("P_FILENAME", OracleDbType.Varchar2, FileName, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, Language, ParameterDirection.Input),
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

        public int AppDocumentOtherDeletedByApp(decimal pAppHeaderID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOC_OTHERS.PROC_APP_DOC_OTHER_DEL_BY_APP",
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


        public int AppDocOtherByID(decimal pID, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_DOC_OTHERS.PROC_APP_DOC_OTHER_DEL_BY_ID",
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

        public DataSet AppDocument_Getby_AppHeader(decimal p_app_header_id, string p_language_code)
        {
            try
            {
               return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_app_document.Proc_GetBy_App_Header",
                    new OracleParameter("p_app_header_id", OracleDbType.Decimal, p_app_header_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor_doc", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

    }
}
