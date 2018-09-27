using Common;
using DataAccess;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;
using ZetaCompressionLibrary;

namespace BussinessFacade
{
    public class TimeSheet_BL
    {
        public List<Timesheet_Info> Timesheet_GetAll()
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                DataSet _ds = _da.Timesheet_GetAll();
                return CBO<Timesheet_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Timesheet_Info>();
            }
        }

        public List<Timesheet_Info> Timesheet_GetBy_Lawer(decimal p_lawer_id)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                DataSet _ds = _da.Timesheet_GetBy_Lawer(p_lawer_id);
                return CBO<Timesheet_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Timesheet_Info>();
            }
        }

        public Timesheet_Info Timesheet_GetBy_Id(decimal p_id)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                DataSet _ds = _da.Timesheet_GetBy_Id(p_id);
                return CBO<Timesheet_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public Timesheet_Info Timesheet_GetBy_Casecode(string p_case_code)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                DataSet _ds = _da.Timesheet_GetBy_Casecode(p_case_code);
                return CBO<Timesheet_Info>.FillObjectFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }


        public List<Timesheet_Info> Timesheet_Search(string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_column = "ALL", string p_sort_type = "ALL")
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                DataSet _ds = _da.Timesheet_Search(p_key_search, p_from, p_to, p_column, p_sort_type, ref p_total_record);
                return CBO<Timesheet_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Timesheet_Info>();
            }
        }

        public decimal Timesheet_Insert(Timesheet_Info p_obj, string p_language_code)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                return _da.Timesheet_Insert(p_obj.Name, p_obj.App_Case_Code, p_obj.Lawer_Id, p_obj.Time_Date,
                 p_obj.From_Time, p_obj.To_Time, p_obj.Hours, p_obj.Hours_Adjust, p_obj.Notes, p_obj.Status, p_obj.Created_By, DateTime.Now, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Timesheet_Update(Timesheet_Info p_obj)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                return _da.Timesheet_Update(p_obj.Id, p_obj.Name, p_obj.Time_Date,
                     p_obj.From_Time, p_obj.To_Time, p_obj.Hours, p_obj.Hours_Adjust, p_obj.Notes, p_obj.Modify_By, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Timesheet_Delete(decimal p_id, string p_modify_by, DateTime p_modify_date)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                return _da.Timesheet_Delete(p_id, p_modify_by, p_modify_date);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public decimal Timesheet_Approve(decimal p_id, int p_status, string p_reject_reason, string p_modify_by)
        {
            try
            {
                TimeSheet_DA _da = new TimeSheet_DA();
                return _da.Timesheet_Approve(p_id, p_status, p_reject_reason, p_modify_by, DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }
    }
}
