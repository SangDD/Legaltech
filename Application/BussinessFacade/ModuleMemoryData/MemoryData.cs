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
    }

    public class MemoryData
    {
        static MyQueue c_queue_changeData = new MyQueue();
        public static Hashtable c_hs_Allcode = new Hashtable();
        static List<GroupUserInfo> c_lst_Group = new List<GroupUserInfo>();
        public static List<Country_Info> c_lst_Country = new List<Country_Info>();

        public static List<AppClassInfo> clstAppClass = new List<AppClassInfo>();

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

            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static List<AllCodeInfo> AllCode_GetBy_CdTypeCdName(string p_cdname, string p_cdtype)
        {
            try
            {
                if (c_hs_Allcode.ContainsKey(p_cdname + "|" + p_cdtype))
                {
                    List<AllCodeInfo> _lst = new List<AllCodeInfo>();

                    List<AllCodeInfo> _lst_tem = (List<AllCodeInfo>)c_hs_Allcode[p_cdname + "|" + p_cdtype];

                    foreach (AllCodeInfo item in _lst_tem)
                        _lst.Add(item);

                    _lst = _lst.OrderBy(m => m.LstOdr).ToList();
                    return _lst;
                }
                else return new List<AllCodeInfo>();
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AllCodeInfo>();
            }
        }

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
    }
}
