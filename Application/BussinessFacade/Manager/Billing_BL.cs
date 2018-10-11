using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessFacade
{
    public class Billing_BL
    {
        public List<Billing_Header_Info> Billing_Search(string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_Search(p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Billing_Header_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Billing_Header_Info>();
            }
        }

        public List<Billing_Header_Info> Billing_GetAll()
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_GetAll();
                return CBO<Billing_Header_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Billing_Header_Info>();
            }
        }

        public Billing_Header_Info Billing_GetBy_Id(decimal p_billing_id, string p_app_case_code, string p_language_code, 
            ref ApplicationHeaderInfo applicationHeaderInfo, ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_GetBy_Id(p_billing_id, p_app_case_code, p_language_code);

                p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<Billing_Header_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Billing_Header_Info();
            }
        }

        public Billing_Header_Info Billing_Search_GetBy_Id(decimal p_billing_id, string p_app_case_code, string p_language_code,
            ref SearchObject_Header_Info searchObject_Header_Info, ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_GetBy_Id(p_billing_id, p_app_case_code, p_language_code);

                p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                searchObject_Header_Info = CBO<SearchObject_Header_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<Billing_Header_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Billing_Header_Info();
            }
        }

        public Billing_Header_Info Billing_GetBy_Code(string p_case_code, string p_language_code,
            ref ApplicationHeaderInfo applicationHeaderInfo, ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_GetBy_Code(p_case_code, p_language_code);

                p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                applicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<Billing_Header_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Billing_Header_Info();
            }
        }

        public Billing_Header_Info Billing_Search_GetBy_Code(string p_case_code, string p_language_code,
            ref SearchObject_Header_Info searchObject_Header_Info, ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                DataSet _ds = _da.Billing_GetBy_Code(p_case_code, p_language_code);

                p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                searchObject_Header_Info = CBO<SearchObject_Header_Info>.FillObjectFromDataTable(_ds.Tables[2]);
                return CBO<Billing_Header_Info>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new Billing_Header_Info();
            }
        }

        public string Billing_GenCaseCode()
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_GenCaseCode();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return "";
            }
        }

        public int Billing_Update_Status(decimal p_docking_id, string p_language_code, decimal p_status, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Update_Status(p_docking_id, p_language_code, p_status, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int Billing_Update_Pay_Status(decimal p_docking_id, string p_language_code, decimal p_status, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Update_Pay_Status(p_docking_id, p_language_code, p_status, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int Billing_Update_Delete(decimal p_docking_id, string p_language_code, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Update_Delete(p_docking_id, p_language_code, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public decimal Billing_Insert(Billing_Header_Info p_obj)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Insert(p_obj.Case_Code, p_obj.Billing_Type, p_obj.App_Case_Code, p_obj.Billing_Date, p_obj.Deadline,
                    p_obj.Request_By, p_obj.Approve_By, p_obj.Status, p_obj.Total_Pre_Tex, p_obj.Tex_Fee, p_obj.Total_Amount,
                    p_obj.Currency, p_obj.Currency_Rate, p_obj.Created_By, p_obj.Created_Date, p_obj.Language_Code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Billing_Update(Billing_Header_Info p_obj)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Update(p_obj.Billing_Id, p_obj.Billing_Date, p_obj.Deadline, p_obj.Total_Pre_Tex, p_obj.Tex_Fee, p_obj.Total_Amount,
                    p_obj.Currency, p_obj.Currency_Rate, p_obj.Modify_By, p_obj.Modify_Date, p_obj.Language_Code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int Billing_Detail_InsertBatch(List<Billing_Detail_Info> pInfo, decimal p_billing_id)
        {
            try
            {
                Billing_DA _da = new Billing_DA();
                return _da.Billing_Detail_InsertBatch(pInfo, p_billing_id);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}
