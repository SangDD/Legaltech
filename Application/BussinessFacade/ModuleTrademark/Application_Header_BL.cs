using Common;
using DataAccess.ModuleTrademark;
using ObjectInfos;
using System;
using System.Collections.Generic;
using System.Data;

namespace BussinessFacade.ModuleTrademark
{
    public class Application_Header_BL
    {
        public List<ApplicationHeaderInfo> ApplicationHeader_Search(string p_user_name, string p_key_search, ref decimal p_total_record,
            string p_from = "1", string p_to = "10", string p_sort_type = "ALL", int p_search_from_home = 0)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.ApplicationHeader_Search(p_user_name, p_key_search, p_from, p_to, p_sort_type, ref p_total_record, p_search_from_home);
                return CBO<ApplicationHeaderInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<ApplicationHeaderInfo>();
            }
        }

        public int AppHeader_Update_Status(string p_case_code, decimal p_status, string p_notes, string p_Modify_By, DateTime p_Modify_Date, string p_language_code)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeader_Update_Status(p_case_code, p_status, p_notes, p_Modify_By, p_Modify_Date, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int AppHeader_Update_Employee(string p_case_code, decimal p_employee, string p_notes, string p_Modify_By, DateTime p_Modify_Date, string p_language_code)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeader_Update_Employee(p_case_code, p_employee, p_notes, p_Modify_By, p_Modify_Date, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int AppHeader_Update_Admin(string p_case_code, decimal p_admin, string p_notes, string p_Modify_By, DateTime p_Modify_Date, string p_language_code)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeader_Update_Admin(p_case_code, p_admin, p_notes, p_Modify_By, p_Modify_Date, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


        public int AppHeader_Filing_Status(string p_case_code, decimal p_status, string p_app_no,
            DateTime p_filing_date, DateTime p_expected_accept_date, string p_url_copy, string p_url_translate, string p_notes, string p_comment_filling,
            string p_Modify_By, DateTime p_Modify_Date, string p_language_code)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();

                return objData.AppHeader_Filing_Status(p_case_code, p_status, p_app_no, p_filing_date, p_expected_accept_date, p_url_copy, p_url_translate, p_notes, p_comment_filling, p_Modify_By, p_Modify_Date, p_language_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public int AppHeader_Update_Advise_Url_Billing(string p_case_code, string p_url_billing)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();

                return objData.AppHeader_Update_Advise_Url_Billing(p_case_code, p_url_billing);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pInfo"></param>
        /// <returns>TRA RA ID CUA BANG KHI INSERT THANH CONG</returns>
        public int AppHeaderInsert(ApplicationHeaderInfo pInfo, ref string p_case_code)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeaderInsert(pInfo, ref p_case_code);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppHeaderUpdate(ApplicationHeaderInfo pInfo)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeaderUpdate(pInfo);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public int AppHeaderDeleted(decimal pID, string pLanguage)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                return objData.AppHeaderDeleted(pID, pLanguage);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

        public List<CustomerInfo> LayThongTinKhachHang(string pUser, string pLanguage, string pAppCode)
        {
            try
            {
                Application_Header_DA objData = new Application_Header_DA();
                DataSet dsCustInfo = objData.LayThongTinKhachHang(pUser, pLanguage, pAppCode);
                return CBO<CustomerInfo>.FillCollectionFromDataSet(dsCustInfo);

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<CustomerInfo>();
            }
        }

        public ApplicationHeaderInfo GetMasterByAppNo(string p_appNo, string p_user_name, string p_languague_code)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.GetMasterByAppNo(p_appNo, p_user_name, p_languague_code);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    return CBO<ApplicationHeaderInfo>.FillObjectFromDataSet(_ds);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }

        public ApplicationHeaderInfo GetBilling_ByAppCase_Code(string p_case_code, string p_user_name, string p_language_code, ref List<Billing_Detail_Info> p_lst_billing_detail)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.GetBilling_ByAppCase_Code(p_case_code, p_user_name, p_language_code);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    p_lst_billing_detail = CBO<Billing_Detail_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                    return CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[0]);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }

        }

        /// <summary>
        /// Lấy thông tin app theo casecode 
        /// </summary>
        /// <param name="p_language_code">Nếu = -1 thì ko lấy language</param>
        /// <returns></returns>
        public ApplicationHeaderInfo GetApp_By_Case_Code(string p_case_code, string p_language_code = "-1")
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.GetApp_By_Case_Code(p_case_code, p_language_code);
                return CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new ApplicationHeaderInfo();
            }
        }

        public ApplicationHeaderInfo Get_Info_After_Filling(string p_case_code, string p_user_name, string p_language_code,
            ref List<B_Todos_Info> b_Todos_Infos, ref List<Billing_Header_Info> billing_Header_Infos,
            ref List<Docking_Info> docking_Infos, ref List<App_Notice_Info> app_Notice_Infos, ref List<B_Remind_Info> b_Remind_Infos)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.Get_Info_After_Filling(p_case_code, p_user_name, p_language_code);
                ApplicationHeaderInfo _ApplicationHeaderInfo = CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[0]);

                b_Todos_Infos = CBO<B_Todos_Info>.FillCollectionFromDataTable(_ds.Tables[1]);
                billing_Header_Infos = CBO<Billing_Header_Info>.FillCollectionFromDataTable(_ds.Tables[2]);
                docking_Infos = CBO<Docking_Info>.FillCollectionFromDataTable(_ds.Tables[3]);
                app_Notice_Infos = CBO<App_Notice_Info>.FillCollectionFromDataTable(_ds.Tables[4]);
                b_Remind_Infos = CBO<B_Remind_Info>.FillCollectionFromDataTable(_ds.Tables[5]);


                return _ApplicationHeaderInfo;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new ApplicationHeaderInfo();
            }
        }

        public ApplicationHeaderInfo GetApp_By_Case_Code_Todo(string p_case_code, string p_user_name, string p_language_code)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.GetApp_By_Case_Code_Todo(p_case_code, p_user_name, p_language_code);
                return CBO<ApplicationHeaderInfo>.FillObjectFromDataTable(_ds.Tables[0]);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new ApplicationHeaderInfo();
            }
        }

        public ApplicationHeaderInfo GetApplicationHeader_ById(decimal p_Id, string p_languague_code)
        {
            try
            {
                Application_Header_DA _da = new Application_Header_DA();
                DataSet _ds = _da.GetApplicationHeader_ById(p_Id, p_languague_code);
                if (_ds != null && _ds.Tables.Count > 0 && _ds.Tables[0].Rows.Count > 0)
                {
                    return CBO<ApplicationHeaderInfo>.FillObjectFromDataSet(_ds);
                }
                else return null;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return null;
            }
        }
    }

    public class AppClassInfoBL
    {
        public static List<AppClassInfo> AppClassGetOnMem()
        {
            try
            {
                AppClassInfo_DA objData = new AppClassInfo_DA();
                DataSet _ds = objData.AppClassGetOnMemory();
                return CBO<AppClassInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AppClassInfo>();
            }
        }

    }
}
