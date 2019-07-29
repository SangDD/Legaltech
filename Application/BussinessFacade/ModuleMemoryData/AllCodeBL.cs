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
    }
}
