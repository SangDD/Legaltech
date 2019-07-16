namespace BussinessFacade.ModuleMemoryData
{
    using System;
    using BussinessFacade.ModuleTrademark;
    using Common;

    using ModuleUsersAndRoles;
    using System.Collections;
    using ObjectInfos;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess.ModuleUsersAndRoles;

    public class Table_Change
    {
        public static string GROUP_USER = "GROUP_USER";
        public static string APPHEADER = "APPHEADER";
        public static string APP_DDSHCN = "APP_DDSHCN";
    }

    public class MemoryData
    {
        /// <summary>
        /// lưu toàn bộ thông tin  khách hàng cache trên mem,  
        /// khi nào thêm đơn mới thì call hàm Enqueue_ChangeData truyền vào   APPHEADER = "APPHEADER";
        /// </summary>
        public static List<CustomerSuggestInfo> lstCacheCustomer = new List<CustomerSuggestInfo>();
        public static List<CustomerSuggestInfo> lstCacheCustomer1 = new List<CustomerSuggestInfo>();
        public static List<CustomerSuggestInfo> lstCacheCustomer2 = new List<CustomerSuggestInfo>();
        public static List<CustomerSuggestInfo> lstCacheCustomer3 = new List<CustomerSuggestInfo>();
        public static List<CustomerSuggestInfo> lstCacheCustomer4 = new List<CustomerSuggestInfo>();

        public static List<CustomerSuggestInfo> lstCache_Represent = new List<CustomerSuggestInfo>();


        public static List<CustomerSuggestInfo> lstCacheRefCustomer = new List<CustomerSuggestInfo>();
        static MyQueue c_queue_changeData = new MyQueue();
        public static Hashtable c_hs_Allcode = new Hashtable();
        static List<GroupUserInfo> c_lst_Group = new List<GroupUserInfo>();
        public static List<Country_Info> c_lst_Country = new List<Country_Info>();

        public static List<AppClassInfo> clstAppClass = new List<AppClassInfo>();
        public static List<CustomerSuggestInfo> clstAppClassSuggest = new List<CustomerSuggestInfo>();

        //public static List<SuggestInfo> clstAppClassShortSuggest = new List<SuggestInfo>();

        /// <summary>
        /// Danh sách chủ đại diện sở hữu công nghiệp
        /// </summary>
        public static List<CustomerSuggestInfo> lstChuDDSHCN = new List<CustomerSuggestInfo>();

        /// <summary>
        /// thông tin fee tĩnh theo đơn, key: appcode_ID(Fee)
        /// </summary>
        public static Dictionary<string, SysAppFixChargeInfo> c_dic_FeeByApp_Fix = new Dictionary<string, SysAppFixChargeInfo>();
        public static Dictionary<string, Sys_Search_Fix_Info> c_dic_FeeBySearch = new Dictionary<string, Sys_Search_Fix_Info>();


        public static void LoadAllMemoryData()
        {
            try
            {
                #region Allcode
                c_hs_Allcode.Clear();
                AllCodeBL _AllCodeBL = new AllCodeBL();
                List<AllCodeInfo> _lst_al = _AllCodeBL.AllCode_Gets_List();

                if (_lst_al.Count > 0)
                {
                    foreach (AllCodeInfo item in _lst_al)
                    {
                        if (c_hs_Allcode.ContainsKey(item.CdName + "|" + item.CdType) == false)
                        {
                            List<AllCodeInfo> _lst = new List<AllCodeInfo>
                            {
                                item
                            };
                            c_hs_Allcode[item.CdName + "|" + item.CdType] = _lst;
                        }
                        else
                        {
                            List<AllCodeInfo> _lst = (List<AllCodeInfo>)c_hs_Allcode[item.CdName + "|" + item.CdType];
                            _lst.Add(item);
                        }
                    }
                }

                AllCodeBL.LoadAllCodeToMemory();
                #endregion

                ReloadGroup();

                ReloadCountry();
                MenuBL.LoadAllMenuToMemory();
                FunctionBL.LoadFunctionCollectionsToMemory();
                SysApplicationBL.SysApplicationAllOnMem();
                //Load lên mem
                clstAppClass = AppClassInfoBL.AppClassGetOnMem();

                // dangtq thêm thông tin fee tĩnh theo đơn
                LoadSys_App_Fee_Fix();

                // DANGTQ load fee cho seach
                LoadSys_Search_Fee_Fix();

                foreach (var item in clstAppClass)
                {
                    CustomerSuggestInfo pinfo = new CustomerSuggestInfo();

                    pinfo.label = item.Name_Vi;
                    pinfo.name = item.Name_Vi;

                    pinfo.label_en = item.Name_En;
                    pinfo.name_en = item.Name_En;
                    if (!string.IsNullOrEmpty(item.Code))
                    {
                        if (item.Code.Length > 2)
                        {
                            //pinfo.value = item.Code.Substring(0, 2);
                            pinfo.value = item.Group_Code;
                        }
                        else
                        {
                            pinfo.value = item.Code;
                        }
                    }
                    clstAppClassSuggest.Add(pinfo);
                }
                //lấy toàn bộ thông tin đơn lên mem, đang đọc toàn bộ cả anh cả việt.
                GetCacheCustomerInfo();

                GetCache_represent();

                GetCacheDDSHCN();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void LoadSys_Search_Fee_Fix()
        {
            try
            {
                c_dic_FeeBySearch = new Dictionary<string, Sys_Search_Fix_Info>();
                Sys_Search_Fix_BL _Sys_Search_Fix_BL = new Sys_Search_Fix_BL();

                List<Sys_Search_Fix_Info> _lst1 = _Sys_Search_Fix_BL.Sys_Search_Fix_GetAll();
                foreach (Sys_Search_Fix_Info item in _lst1)
                {
                    c_dic_FeeBySearch[item.Country_Id.ToString() + "_" + item.Search_Object.ToString() + "_" + item.Search_Type.ToString()] = item;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void LoadSys_App_Fee_Fix()
        {
            try
            {
                c_dic_FeeByApp_Fix = new Dictionary<string, SysAppFixChargeInfo>();
                SysApplicationBL _SysApplicationBL = new SysApplicationBL();
                List<SysAppFixChargeInfo> _lst1 = _SysApplicationBL.Sys_App_Fix_Charge_GetAll();
                foreach (SysAppFixChargeInfo item in _lst1)
                {
                    c_dic_FeeByApp_Fix[item.Appcode + "_" + item.Fee_Id.ToString()] = item;
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        //public static List<AllCodeInfo> AllCode_GetBy_CdTypeCdName(string p_cdname, string p_cdtype, string p_lang = "VI_VN")
        //{
        //    try
        //    {
        //        if (c_hs_Allcode.ContainsKey(p_cdname + "|" + p_cdtype))
        //        {
        //            List<AllCodeInfo> _lst = new List<AllCodeInfo>();

        //            List<AllCodeInfo> _lst_tem = (List<AllCodeInfo>)c_hs_Allcode[p_cdname + "|" + p_cdtype];
        //            //string language = WebApps.CommonFunction.AppsCommon.GetCurrentLang();

        //            foreach (AllCodeInfo item in _lst_tem)
        //            {
        //                if (p_lang != "VI_VN")
        //                {
        //                    item.Content = item.Content_Eng;
        //                }
        //                _lst.Add(item);
        //            }

        //            _lst = _lst.OrderBy(m => m.LstOdr).ToList();
        //            return _lst;
        //        }
        //        else return new List<AllCodeInfo>();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogException(ex);
        //        return new List<AllCodeInfo>();
        //    }
        //}

        #region Group

        public static List<GroupUserInfo> GetAllGroup()
        {
            var LstData = new List<GroupUserInfo>();
            try
            {
                LstData.AddRange(c_lst_Group);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
            return LstData;
        }

        public static void ReloadGroup()
        {
            try
            {
                var ds = GroupUserDA.GetAllGroups();
                c_lst_Group = CBO<GroupUserInfo>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
        #endregion

        // Country
        public static void ReloadCountry()
        {
            try
            {
                AllCodeBL allCodeBL = new AllCodeBL();
                c_lst_Country = allCodeBL.Country_GetAll();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }


        public static void Enqueue_ChangeData(string p_table_name)
        {
            CallBack_Info p_CallBack_Info = new CallBack_Info
            {
                Table_Name = p_table_name
            };
            c_queue_changeData.Enqueue(p_CallBack_Info);
        }

        public static CallBack_Info Dequeue_ChangeData()
        {
            CallBack_Info _CallBack_Info = (CallBack_Info)c_queue_changeData.Dequeue();
            if (_CallBack_Info != null)
                return _CallBack_Info;
            else return _CallBack_Info;
        }

        public static void GetCacheCustomerInfo()
        {
            try
            {
                lstCacheCustomer.Clear();
                lstCacheCustomer1.Clear();
                lstCacheCustomer2.Clear();
                lstCacheCustomer3.Clear();
                lstCacheCustomer4.Clear();
                CustomerSuggestInfo pInfo; CustomerSuggestInfo pInfo1; CustomerSuggestInfo pInfo2; CustomerSuggestInfo pInfo3;
                CustomerSuggestInfo pInfo4;
                CustomerSuggestInfo pInfo5;
                var objAppHeaderBL = new Application_Header_BL();
                var list = objAppHeaderBL.LayThongTinKhachHang("", "", "");
                foreach (var item in list)
                {
                    pInfo = new CustomerSuggestInfo();
                    pInfo1 = new CustomerSuggestInfo();

                    pInfo.label = item.Master_Name + " Phone: " + item.Master_Phone + " Fax: " + item.Master_Fax + " Email: " + item.Rep_Master_Email;
                    pInfo.value = item.Master_Name + "|" + item.Master_Address + "|" + item.Master_Phone + "|" + item.Master_Fax + "|" + item.Rep_Master_Email;
                    pInfo.name = item.Master_Name + " Phone: " + item.Master_Phone + " Fax: " + item.Master_Fax + " Email: " + item.Rep_Master_Email;
                    pInfo.Language = item.Language;

                    pInfo1.label = item.Cdk_Name_1 + " Phone: " + item.Cdk_Phone_1 + " Fax: " + item.Cdk_Fax_1 + " Email: " + item.Cdk_Email_1;
                    pInfo1.value = item.Cdk_Name_1 + "|" + item.Cdk_Address_1 + "|" + item.Cdk_Phone_1 + "|" + item.Cdk_Fax_1 + "|" + item.Cdk_Email_1;
                    pInfo1.name = item.Cdk_Name_1 + " Phone: " + item.Cdk_Phone_1 + " Fax: " + item.Cdk_Fax_1 + " Email: " + item.Cdk_Email_1;
                    pInfo1.Language = item.Language;

                    pInfo2 = new CustomerSuggestInfo();
                    pInfo2.label = item.Cdk_Name_2 + " Phone: " + item.Cdk_Phone_2 + " Fax: " + item.Cdk_Fax_2 + " Email: " + item.Cdk_Email_2;
                    pInfo2.value = item.Cdk_Name_2 + "|" + item.Cdk_Address_2 + "|" + item.Cdk_Phone_2 + "|" + item.Cdk_Fax_2 + "|" + item.Cdk_Email_2;
                    pInfo2.name = item.Cdk_Name_2 + " Phone: " + item.Cdk_Phone_2 + " Fax: " + item.Cdk_Fax_2 + " Email: " + item.Cdk_Email_2;
                    pInfo2.Language = item.Language;
                    pInfo3 = new CustomerSuggestInfo();
                    pInfo3.label = item.Cdk_Name_3 + " Phone: " + item.Cdk_Phone_3 + " Fax: " + item.Cdk_Fax_3 + " Email: " + item.Cdk_Email_3;
                    pInfo3.value = item.Cdk_Name_3 + "|" + item.Cdk_Address_3 + "|" + item.Cdk_Phone_3 + "|" + item.Cdk_Fax_3 + "|" + item.Cdk_Email_3;
                    pInfo3.name = item.Cdk_Name_3 + " Phone: " + item.Cdk_Phone_3 + " Fax: " + item.Cdk_Fax_3 + " Email: " + item.Cdk_Email_3;
                    pInfo3.Language = item.Language;
                    pInfo4 = new CustomerSuggestInfo();
                    pInfo4.label = item.Cdk_Name_4 + " Phone: " + item.Cdk_Phone_4 + " Fax: " + item.Cdk_Fax_4 + " Email: " + item.Cdk_Email_4;
                    pInfo4.value = item.Cdk_Name_4 + "|" + item.Cdk_Address_4 + "|" + item.Cdk_Phone_4 + "|" + item.Cdk_Fax_4 + "|" + item.Cdk_Email_4;
                    pInfo4.name = item.Cdk_Name_4;

                    pInfo4.Language = item.Language;
                    pInfo5 = new CustomerSuggestInfo();
                    pInfo5.label = item.Rep_Master_Name + " Phone: " + item.Rep_Master_Phone + " Fax: " + item.Rep_Master_Fax + " Email: " + item.Rep_Master_Email;
                    pInfo5.value = item.Rep_Master_Name + "|" + item.Rep_Master_Address + "|" + item.Rep_Master_Phone + "|" + item.Rep_Master_Fax + "|" + item.Rep_Master_Email;
                    pInfo5.name = item.Rep_Master_Name + " Phone: " + item.Rep_Master_Phone + " Fax: " + item.Rep_Master_Fax + " Email: " + item.Rep_Master_Email;
                    pInfo5.Language = item.Language;
                    lstCacheCustomer.Add(pInfo);
                    if (!string.IsNullOrEmpty(item.Cdk_Name_1))
                        lstCacheCustomer1.Add(pInfo1);
                    if (!string.IsNullOrEmpty(item.Cdk_Name_2))
                        lstCacheCustomer2.Add(pInfo2);
                    if (!string.IsNullOrEmpty(item.Cdk_Name_3))
                        lstCacheCustomer3.Add(pInfo3);
                    if (!string.IsNullOrEmpty(item.Cdk_Name_4))
                        lstCacheCustomer4.Add(pInfo4);
                    if (!string.IsNullOrEmpty(item.Rep_Master_Name))
                        lstCacheRefCustomer.Add(pInfo5);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void GetCache_represent()
        {
            try
            {
                BussinessFacade.ModuleUsersAndRoles.UserBL _UserBL = new BussinessFacade.ModuleUsersAndRoles.UserBL();
                List<UserInfo> _lstUsersAdmin = _UserBL.GetUserByType(3);
                _lstUsersAdmin = _lstUsersAdmin.FindAll(x => x.Is_Agent == 1).ToList();
                lstCache_Represent = new List<CustomerSuggestInfo>();

                foreach (var item in _lstUsersAdmin)
                {
                    CustomerSuggestInfo pInfo = new CustomerSuggestInfo();

                    pInfo.label = item.FullName + " Phone: " + item.Phone + " Fax: " + item.Fax + " Email: " + item.Email + " Mã đại diện: " + item.Customer_Code;
                    pInfo.value = item.FullName + "|" + item.Address + "|" + item.Phone + "|" + item.Fax + "|" + item.Email + "|" + item.Customer_Code;
                    pInfo.name = item.FullName + " Phone: " + item.Phone + " Fax: " + item.Fax + " Email: " + item.Email;
                    pInfo.Language = item.Language;

                    lstCache_Represent.Add(pInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static void GetCacheDDSHCN()
        {
            try
            {
                lstChuDDSHCN.Clear();
                AppDDSHCN_BL _obj_bl = new AppDDSHCN_BL();
                decimal _total_record = 0;
                CustomerSuggestInfo pInfo;
                List<AppDDSHCNInfo> _lst = _obj_bl.AppDDSHCNGetAll("", "", 0, 0, ref _total_record);
                foreach (var item in _lst)
                {
                    pInfo = new CustomerSuggestInfo();
                    pInfo.label = item.Name_Vi + ", " + item.Address_Vi;
                    pInfo.value = item.Name_Vi + "|" + item.Address_Vi + "|" + item.Phone + "|" + item.Fax + "|" + item.Email + "|" + item.NguoiDDSH + "|" + item.MaNguoiDaiDien;
                    pInfo.name = item.Name_Vi + ", " + item.Address_Vi;
                    lstChuDDSHCN.Add(pInfo);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
