namespace BussinessFacade.ModuleMemoryData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common;

    using DataAccess.ModuleMemoryData;

    using System.Data;
    using ObjectInfos;

    public class AllCodeBL
    {
        public List<AllCodeInfo> AllCode_Gets_List()
        {
            try
            {
                DataSet _ds = AllCodeDA.GetAllInAllCode();
                return CBO<AllCodeInfo>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<AllCodeInfo>();
            }
        }

        public List<Country_Info> Country_GetAll()
        {
            try
            {
                DataSet _ds = AllCodeDA.Country_GetAll();
                return CBO<Country_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Country_Info>();
            }
        }

        public List<Country_Info> Nation_Represent_GetAll()
        {
            try
            {
                DataSet _ds = AllCodeDA.Nation_Represent_GetAll();
                return CBO<Country_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Country_Info>();
            }
        }

        private static readonly object s_lockerAllCode = new object();

        private static List<AllCodeInfo> s_allCodeCollectionInMemory;

        public static void LoadAllCodeToMemory()
        {
            lock (s_lockerAllCode)
            {
                var ds = AllCodeDA.GetAllInAllCode();
                s_allCodeCollectionInMemory = CBO<AllCodeInfo>.FillCollectionFromDataSet(ds);
            }
        }

        public static List<Injection_Info> Get_All_Injection()
        {

            try
            {
                var ds = AllCodeDA.Get_All_Injection();
                return CBO<Injection_Info>.FillCollectionFromDataSet(ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Injection_Info>();
            }
        }

        public static List<AllCodeInfo> GetAllInAllCode()
        {
            lock (s_lockerAllCode)
            {
                return s_allCodeCollectionInMemory;
            }
        }

        public static List<AllCodeInfo> GetAllCodeByCdName(string cdName)
        {
            lock (s_lockerAllCode)
            {
                return s_allCodeCollectionInMemory.Where(o => string.Equals(o.CdName, cdName, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public static List<AllCodeInfo> GetAllCodeByCdType(string cdType)
        {
            lock (s_lockerAllCode)
            {
                return s_allCodeCollectionInMemory.Where(o => string.Equals(o.CdType, cdType, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public decimal Send_Email_Insert(Email_Info info)
        {
            try
            {
                AllCodeDA _AllCodeDA = new AllCodeDA();
                return _AllCodeDA.Send_Email_Insert(info.EmailFrom, info.EmailTo, info.EmailCC, info.Display_Name, info.Subject, info.Content, info.LstAttachment, info.Status, info.Send_Time);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }

        public List<Send_Email_Info> Email_Search(string p_user_name, string p_key_search, ref decimal p_total_record,
           string p_from = "1", string p_to = "10", string p_sort_type = "ALL")
        {
            try
            {
                AllCodeDA _AllCodeDA = new AllCodeDA();
                DataSet _ds = _AllCodeDA.Email_Search(p_user_name, p_key_search, p_from, p_to, p_sort_type, ref p_total_record);
                return CBO<Send_Email_Info>.FillCollectionFromDataSet(_ds);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new List<Send_Email_Info>();
            }
        }
    }
}
