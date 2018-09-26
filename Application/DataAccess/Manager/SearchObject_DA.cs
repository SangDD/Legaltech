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
        public decimal  SEARCH_HEADER_INSERT(SearchObject_Header_Info p_SearchObject_Header_Info)
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
                    new OracleParameter("P_LAWER_ID", OracleDbType.Decimal, p_SearchObject_Header_Info.LAWER_ID, ParameterDirection.Input),
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

        public decimal  SEARCH_HEADER_UPDATE(SearchObject_Header_Info p_SearchObject_Header_Info)
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

        public DataSet SEARCH_HEADER_GETBYID(decimal P_ID)
        {
            try
            {
                DataSet _Ds = new DataSet();
                _Ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_GETBYID",
                new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_ID, ParameterDirection.Input),
                new OracleParameter("P_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));
                return _Ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public decimal  SEARCH_HEADER_DELETE(decimal P_SEARCH_ID )
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_HEADER_DELETE",
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

        public DataSet SEARCH_OBJECT_SEARCH(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_OBJECT_SEARCH",
                    new OracleParameter("p_key_search", OracleDbType.Varchar2, p_key_search, ParameterDirection.Input),
                    new OracleParameter("p_from", OracleDbType.Varchar2, p_from, ParameterDirection.Input),
                    new OracleParameter("p_to", OracleDbType.Varchar2, p_to, ParameterDirection.Input),
                    new OracleParameter("p_sort_type", OracleDbType.Varchar2, p_sort_type, ParameterDirection.Input),
                    paramReturn,
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output)
                   );
                p_total_record = Convert.ToDecimal(paramReturn.Value.ToString());
                return _ds;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }


        public decimal SEARCH_DETAIL_INSERT(List<SearchObject_Detail_Info> P_SearchDetails )
        {
            try
            {
                int _totalrec = P_SearchDetails.Count;
                decimal[] P_SEARCH_ID = new decimal[_totalrec];
                string[] P_SEARCH_TYPE = new string[_totalrec];
                string[] P_SEARCH_VALUE = new string[_totalrec];
                string[] P_SEARCH_OPERATOR = new string[_totalrec];
                string[] P_ANDOR = new string[_totalrec];

                OracleParameter paramReturn = new OracleParameter("p_return", OracleDbType.Decimal, ParameterDirection.Output);
                paramReturn.Size = 10;
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_SEARCH_OBJECTS.PROC_SEARCH_DETAIL_INSERT", _totalrec,
                    new OracleParameter("P_SEARCH_ID", OracleDbType.Decimal, P_SEARCH_ID, ParameterDirection.Input),
                    new OracleParameter("P_SEARCH_TYPE", OracleDbType.Varchar2, P_SEARCH_TYPE, ParameterDirection.Input),
                    new OracleParameter("P_SEARCH_VALUE", OracleDbType.Varchar2, P_SEARCH_VALUE, ParameterDirection.Input),
                    new OracleParameter("P_SEARCH_OPERATOR", OracleDbType.Varchar2, P_SEARCH_OPERATOR, ParameterDirection.Input),
                    new OracleParameter("P_ANDOR", OracleDbType.Varchar2, P_ANDOR, ParameterDirection.Input),
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
                    new OracleParameter("P_CONTENT", OracleDbType.Clob, p_question_info.CONTENT, ParameterDirection.Input),
                    new OracleParameter("P_RESULT", OracleDbType.Clob, p_question_info.RESULT, ParameterDirection.Input),
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
    }
}
