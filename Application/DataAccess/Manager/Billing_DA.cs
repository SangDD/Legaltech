using Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using ObjectInfos;

namespace DataAccess
{
    public class Billing_DA
    {
        public DataSet Billing_Search(string p_key_search, string p_from, string p_to, string p_sort_type, ref decimal p_total_record)
        {
            try
            {
                OracleParameter paramReturn = new OracleParameter("p_total_record", OracleDbType.Decimal, ParameterDirection.Output);
                DataSet _ds = OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_biling_search",
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

        public DataSet Billing_GetAll()
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_getall",
                new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Billing_GetBy_Id(decimal p_billing_id, string p_app_case_code, string p_language_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_getbyid",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_app_case_code", OracleDbType.Varchar2, p_app_case_code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_detail", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_header", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public DataSet Billing_GetBy_Code(string p_case_code, string p_language_code)
        {
            try
            {
                return OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_getbycode",
                    new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_detail", OracleDbType.RefCursor, ParameterDirection.Output),
                    new OracleParameter("p_cursor_header", OracleDbType.RefCursor, ParameterDirection.Output));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new DataSet();
            }
        }

        public string Billing_GenCaseCode()
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Varchar2, ParameterDirection.Output);
                paramReturn.Size = 50;
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_get_casecode", paramReturn);
                return paramReturn.Value.ToString();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }


        public int Billing_Update_Status(decimal p_billing_id, string p_language_code, decimal p_status, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_updateStatus",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Billing_Update_Pay_Status(decimal p_billing_id, string p_language_code, decimal p_status, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_update_payStatus",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        

        public decimal Billing_Insert(string p_case_code, decimal p_billing_type, string p_app_case_code, DateTime p_billing_date, DateTime p_deadline,
            string p_request_by, string p_approve_by, decimal p_status, decimal p_total_pre_tex, decimal p_tex_fee, decimal p_total_amount,
            string p_currency, decimal p_currency_rate, string p_created_by, DateTime p_created_date, string p_language_code, string p_notes)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.Proc_Billing_Header_Insert",
                     new OracleParameter("p_case_code", OracleDbType.Varchar2, p_case_code, ParameterDirection.Input),
                     new OracleParameter("p_billing_type", OracleDbType.Decimal, p_billing_type, ParameterDirection.Input),
                     new OracleParameter("p_app_case_code", OracleDbType.Varchar2, p_app_case_code, ParameterDirection.Input),
                     new OracleParameter("p_billing_date", OracleDbType.Date, p_billing_date, ParameterDirection.Input),
                     new OracleParameter("p_deadline", OracleDbType.Date, p_deadline, ParameterDirection.Input),
                     new OracleParameter("p_request_by", OracleDbType.Varchar2, p_request_by, ParameterDirection.Input),
                     new OracleParameter("p_approve_by", OracleDbType.Varchar2, p_approve_by, ParameterDirection.Input),
                     new OracleParameter("p_status", OracleDbType.Decimal, p_status, ParameterDirection.Input),
                     //new OracleParameter("p_pay_status", OracleDbType.Decimal, p_pay_status, ParameterDirection.Input),
                     //new OracleParameter("p_pay_date", OracleDbType.Date, p_pay_date, ParameterDirection.Input),
                     new OracleParameter("p_total_pre_tex", OracleDbType.Decimal, p_total_pre_tex, ParameterDirection.Input),
                     new OracleParameter("p_tex_fee", OracleDbType.Decimal, p_tex_fee, ParameterDirection.Input),
                     new OracleParameter("p_total_amount", OracleDbType.Decimal, p_total_amount, ParameterDirection.Input),
                     new OracleParameter("p_currency", OracleDbType.Varchar2, p_currency, ParameterDirection.Input),
                     new OracleParameter("p_currency_rate", OracleDbType.Decimal, p_currency_rate, ParameterDirection.Input),
                     new OracleParameter("p_created_by", OracleDbType.Varchar2, p_created_by, ParameterDirection.Input),
                     new OracleParameter("p_created_date", OracleDbType.Date, p_created_date, ParameterDirection.Input),
                     new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                     new OracleParameter("p_notes", OracleDbType.Varchar2, p_notes, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Billing_Update(decimal p_billing_id, DateTime p_billing_date, DateTime p_deadline,
            decimal p_total_pre_tex, decimal p_tex_fee, decimal p_total_amount,
            string p_currency, decimal p_currency_rate, string p_modify_by, DateTime p_modify_date, string p_language_code)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_header_update",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                     new OracleParameter("p_billing_date", OracleDbType.Date, p_billing_date, ParameterDirection.Input),
                     new OracleParameter("p_deadline", OracleDbType.Date, p_deadline, ParameterDirection.Input),
                     new OracleParameter("p_total_pre_tex", OracleDbType.Decimal, p_total_pre_tex, ParameterDirection.Input),
                     new OracleParameter("p_tex_fee", OracleDbType.Decimal, p_tex_fee, ParameterDirection.Input),
                     new OracleParameter("p_total_amount", OracleDbType.Decimal, p_total_amount, ParameterDirection.Input),
                     new OracleParameter("p_currency", OracleDbType.Varchar2, p_currency, ParameterDirection.Input),
                     new OracleParameter("p_currency_rate", OracleDbType.Decimal, p_currency_rate, ParameterDirection.Input),
                     new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                     new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                     new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    paramReturn);

                return Convert.ToDecimal(paramReturn.Value.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Billing_Update_Delete(decimal p_billing_id, string p_language_code, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.proc_billing_delete",
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_language_code", OracleDbType.Varchar2, p_language_code, ParameterDirection.Input),
                    new OracleParameter("p_modify_by", OracleDbType.Varchar2, p_modify_by, ParameterDirection.Input),
                    new OracleParameter("p_modify_date", OracleDbType.Date, p_modify_date, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Billing_Detail_InsertBatch(List<Billing_Detail_Info> pInfo, decimal _billing_id)
        {
            try
            {
                int numberRecord = pInfo.Count;
                decimal[] p_billing_id = new decimal[numberRecord];
                string[] p_biling_detail_name = new string[numberRecord];
                decimal[] p_ref_id = new decimal[numberRecord];
                decimal[] p_type = new decimal[numberRecord];
                decimal[] p_nation_fee = new decimal[numberRecord];
                decimal[] p_represent_fee = new decimal[numberRecord];
                decimal[] p_service_fee = new decimal[numberRecord];

                for (int i = 0; i < pInfo.Count; i++)
                {
                    p_billing_id[i] = _billing_id;
                    p_biling_detail_name[i] = pInfo[i].Biling_Detail_Name;
                    p_ref_id[i] = pInfo[i].Ref_Id;
                    p_type[i] = pInfo[i].Type;
                    p_nation_fee[i] = pInfo[i].Nation_Fee;
                    p_represent_fee[i] = pInfo[i].Represent_Fee;
                    p_service_fee[i] = pInfo[i].Service_Fee;
                }

                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "pkg_billing.Proc_Billing_Detail_Insert", numberRecord,
                    new OracleParameter("p_billing_id", OracleDbType.Decimal, p_billing_id, ParameterDirection.Input),
                    new OracleParameter("p_biling_detail_name", OracleDbType.Varchar2, p_biling_detail_name, ParameterDirection.Input),
                    new OracleParameter("p_ref_id", OracleDbType.Decimal, p_ref_id, ParameterDirection.Input),
                    new OracleParameter("p_type", OracleDbType.Decimal, p_type, ParameterDirection.Input),
                    new OracleParameter("p_nation_fee", OracleDbType.Decimal, p_nation_fee, ParameterDirection.Input),
                    new OracleParameter("p_represent_fee", OracleDbType.Decimal, p_represent_fee, ParameterDirection.Input),
                    new OracleParameter("p_service_fee", OracleDbType.Decimal, p_service_fee, ParameterDirection.Input),
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
                        result = 1;
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
    }
}
