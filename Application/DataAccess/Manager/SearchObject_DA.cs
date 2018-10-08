using Common;
using Common.Converters;
using Common.SearchingAndFiltering;
using ObjectInfos;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Collections.Generic;

namespace DataAccess
{
    public class SearchObject_DA
    {
        public decimal SEARCH_HEADER_INSERT(SearchObject_Header_Info p_SearchObject_Header_Info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_INSERT",
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, p_SearchObject_Header_Info.CASE_CODE, ParameterDirection.Input),
                    new OracleParameter("P_CLIENT_REFERENCE", OracleDbType.Varchar2, p_SearchObject_Header_Info.CLIENT_REFERENCE, ParameterDirection.Input),
                    new OracleParameter("P_CASE_NAME", OracleDbType.Varchar2, p_SearchObject_Header_Info.CASE_NAME, ParameterDirection.Input),
                    new OracleParameter("P_REQUEST_DATE", OracleDbType.Date, p_SearchObject_Header_Info.REQUEST_DATE, ParameterDirection.Input),
                    new OracleParameter("P_RESPONSE_DATE", OracleDbType.Date, p_SearchObject_Header_Info.RESPONSE_DATE, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, p_SearchObject_Header_Info.STATUS, ParameterDirection.Input),
                    new OracleParameter("P_COUNTRY_ID", OracleDbType.Decimal, p_SearchObject_Header_Info.Country_Id, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_BY", OracleDbType.Varchar2, p_SearchObject_Header_Info.CREATED_BY, ParameterDirection.Input),
                    new OracleParameter("P_CREATED_DATE", OracleDbType.Date, p_SearchObject_Header_Info.CREATED_DATE, ParameterDirection.Input),
                    new OracleParameter("P_LANGUAGE_CODE", OracleDbType.Varchar2, p_SearchObject_Header_Info.LANGUAGE_CODE, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_HEADER_UPDATE(SearchObject_Header_Info p_SearchObject_Header_Info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_UPDATE",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, p_SearchObject_Header_Info.SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("P_CASE_CODE", OracleDbType.Varchar2, p_SearchObject_Header_Info.CASE_CODE, ParameterDirection.Input),
                    new OracleParameter("P_CLIENT_REFERENCE", OracleDbType.Varchar2, p_SearchObject_Header_Info.CLIENT_REFERENCE, ParameterDirection.Input),
                    new OracleParameter("P_CASE_NAME", OracleDbType.Varchar2, p_SearchObject_Header_Info.CASE_NAME, ParameterDirection.Input),
                    new OracleParameter("P_REQUEST_DATE", OracleDbType.Date, p_SearchObject_Header_Info.REQUEST_DATE, ParameterDirection.Input),
                    new OracleParameter("P_RESPONSE_DATE", OracleDbType.Date, p_SearchObject_Header_Info.RESPONSE_DATE, ParameterDirection.Input),
                    new OracleParameter("P_STATUS", OracleDbType.Decimal, p_SearchObject_Header_Info.STATUS, ParameterDirection.Input),
                    new OracleParameter("P_LAWER_ID", OracleDbType.Decimal, p_SearchObject_Header_Info.LAWER_ID, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_BY", OracleDbType.Varchar2, p_SearchObject_Header_Info.MODIFIED_BY, ParameterDirection.Input),
                    new OracleParameter("P_MODIFIED_DATE", OracleDbType.Date, p_SearchObject_Header_Info.MODIFIED_DATE, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet SEARCH_HEADER_GETBYID(decimal P_ID)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_GETBYID",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_QUETION_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet SEARCH_HEADER_GETBY_CASECODE(string p_casecode)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_GETBY_CASECODE",
                    new OracleParameter("P_CASECODE", OracleDbType.Varchar2, p_casecode, ParameterDirection.Input),
                    new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_DETAIL_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("P_QUETION_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_class", OracleDbType.RefCursor, ParameterDirection.Output)
                );
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal SEARCH_HEADER_DELETE(decimal P_SEARCH_ID, string p_user_name, string p_language)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_DELETE",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("p_user_name", OracleDbType.Varchar2, p_user_name, ParameterDirection.Input),
                    new OracleParameter("p_language", OracleDbType.Varchar2, p_language, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public DataSet SEARCH_OBJECT_SEARCH(string keysSearch, OptionFilter options, ref decimal totalRecord)
        {
            try
            {
                DataSet _Ds = new DataSet();
                OracleParameter paramReturn = new OracleParameter("P_TOTAL_RECORD", OracleDbType.Decimal, ParameterDirection.Output);
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_OBJECT_SEARCH",
                new OracleParameter("P_KEY_SEARCH", OracleDbType.Varchar2, keysSearch.ToFillKeySearch(), ParameterDirection.Input),
                new OracleParameter("P_FROM", OracleDbType.Varchar2, options.StartAt.ToString(), ParameterDirection.Input),
                new OracleParameter("P_TO", OracleDbType.Varchar2, options.EndAt.ToString(), ParameterDirection.Input),
                new OracleParameter("P_SORT_TYPE", OracleDbType.Varchar2, options.OrderBy, ParameterDirection.Input),
                paramReturn,
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                totalRecord = Convert.ToDecimal(paramReturn.Value.ToString());
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal SEARCH_DETAIL_INSERT(List<SearchObject_Detail_Info> P_SearchDetails)
        {
            try
            {
                int _totalrec = P_SearchDetails.Count;
                decimal[] P_SEARCH_ID = new decimal[_totalrec];
                decimal[] P_SEARCH_OBJECT = new decimal[_totalrec];
                decimal[] P_SEARCH_TYPE = new decimal[_totalrec];
                int _count = 0;
                foreach (SearchObject_Detail_Info item in P_SearchDetails)
                {
                    P_SEARCH_ID[_count] = item.SEARCH_ID;
                    P_SEARCH_TYPE[_count] = item.SEARCH_TYPE;
                    P_SEARCH_OBJECT[_count] = item.SEARCH_OBJECT;
                    _count++;
                }

                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_DETAIL_INSERT", _totalrec,
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("P_SEARCH_OBJECT", OracleDbType.Varchar2, P_SEARCH_OBJECT, ParameterDirection.Input),
                    new OracleParameter("P_SEARCH_TYPE", OracleDbType.Varchar2, P_SEARCH_TYPE, ParameterDirection.Input),
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
                return -1;
            }
        }

        public decimal SEARCH_DETAIL_DELETE(decimal P_SEARCH_ID)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_DETAIL_DELETE",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_SEARCH_ID, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public decimal SEARCH_QUESTION_INSERT(SearchObject_Question_Info p_question_info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_QUESTION_INSERT",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, p_question_info.SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("P_SUBJECT", OracleDbType.Varchar2, p_question_info.SUBJECT, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Varchar2, p_question_info.CONTENT, ParameterDirection.Input),
                    new OracleParameter("P_RESULT", OracleDbType.Clob, p_question_info.RESULT, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL", OracleDbType.Varchar2, p_question_info.FILE_URL, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL02", OracleDbType.Varchar2, p_question_info.FILE_URL02, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_QUESTION_UPDATE(SearchObject_Question_Info p_question_info)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_QUESTION_UPDATE",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, p_question_info.SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("P_CONTENT", OracleDbType.Varchar2, p_question_info.CONTENT, ParameterDirection.Input),
                    new OracleParameter("P_RESULT", OracleDbType.Clob, p_question_info.RESULT, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL", OracleDbType.Varchar2, p_question_info.FILE_URL, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL02", OracleDbType.Varchar2, p_question_info.FILE_URL02, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public decimal SEARCH_QUESTION_DELETE(decimal P_SEARCH_ID)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_QUESTION_DELETE",
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_SEARCH_ID, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public decimal Update_Lawer(string p_case_code, decimal p_lawer_id, string p_notes, string p_language_code, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_GRANT_SEARCH2_LAWER",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_lawer_id", OracleDbType.Decimal, p_lawer_id, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Admin_Update(string p_case_code, string p_notes, string p_language_code, string p_modified_by)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.proc_admin_comment",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal SEARCH_RESULT_SEARCH(string p_case_code, decimal p_lawer_id, string p_notes, string p_result, string p_language_code,
            string p_modified_by, string P_FILE_URL_01, string P_FILE_URL_02)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_RESULT_SEARCH",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    new OracleParameter("p_result", OracleDbType.Varchar2, p_result, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL_01", OracleDbType.Varchar2, P_FILE_URL_01, ParameterDirection.Input),
                    new OracleParameter("P_FILE_URL02", OracleDbType.Varchar2, P_FILE_URL_02, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_modified_by", OracleDbType.Varchar2, p_modified_by, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int SearchClass_InsertBatch(List<Search_Class_Info> pInfo, decimal p_seach_id, string pLanguage)
        {
            try
            {
                int numberRecord = pInfo.Count;
                if (numberRecord < 1) return 0;
                string[] TextInput = new string[numberRecord];
                decimal[] _seach_id = new decimal[numberRecord];
                string[] Code = new string[numberRecord];
                string[] Language = new string[numberRecord];
                DateTime[] Document_Filling_Date = new DateTime[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    _seach_id[i] = p_seach_id;
                    TextInput[i] = pInfo[i].Textinput;
                    Code[i] = pInfo[i].Code;
                    Language[i] = pLanguage;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_search_class_detail.proc_insert", numberRecord,
                    new OracleParameter("p_textinput", OracleDbType.Varchar2, TextInput, ParameterDirection.Input),
                    new OracleParameter("p_code", OracleDbType.Varchar2, Code, ParameterDirection.Input),
                    new OracleParameter("p_seach_id", OracleDbType.Decimal, _seach_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, Language, ParameterDirection.Input),
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

        public int SearchClass_Delete(decimal p_seach_id, string pLanguage)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_search_class_detail.proc_delete",
                    new OracleParameter("p_seach_id", OracleDbType.Decimal, p_seach_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, pLanguage, ParameterDirection.Input),
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
