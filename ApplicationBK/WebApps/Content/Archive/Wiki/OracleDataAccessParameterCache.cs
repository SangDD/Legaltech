using Oracle.DataAccess.Client;
using System;
using System.Collections;
using System.Data;

namespace OracleDataAccess
{
    public sealed class OracleDataAccessParameterCache
    {
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        private OracleDataAccessParameterCache()
        {
        }

        private static OracleParameter[] DiscoverSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            OracleParameter[] result;
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(spName, cn))
                {
                    cn.Open();
                    cmd.CommandType = (CommandType)4;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (!includeReturnValueParameter)
                    {
                        cmd.Parameters.RemoveAt(0);
                    }
                    OracleParameter[] discoveredParameters = new OracleParameter[cmd.Parameters.Count];
                    cmd.Parameters.CopyTo(discoveredParameters, 0);
                    result = discoveredParameters;
                }
            }
            return result;
        }

        private static OracleParameter[] CloneParameters(OracleParameter[] originalParameters)
        {
            OracleParameter[] clonedParameters = new OracleParameter[originalParameters.Length];
            int i = 0;
            int j = originalParameters.Length;
            while (i < j)
            {
                clonedParameters[i] = (OracleParameter)originalParameters[i].Clone();
                i++;
            }
            return clonedParameters;
        }

        public static void CacheParameterSet(string connectionString, string commandText, params OracleParameter[] commandParameters)
        {
            string hashKey = connectionString + ":" + commandText;
            OracleDataAccessParameterCache.paramCache[hashKey] = commandParameters;
        }

        public static OracleParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            string hashKey = connectionString + ":" + commandText;
            OracleParameter[] cachedParameters = (OracleParameter[])OracleDataAccessParameterCache.paramCache[hashKey];
            if (cachedParameters == null)
            {
                return null;
            }
            return OracleDataAccessParameterCache.CloneParameters(cachedParameters);
        }

        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return OracleDataAccessParameterCache.GetSpParameterSet(connectionString, spName, false);
        }

        public static OracleParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            string hashKey = connectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            OracleParameter[] cachedParameters = (OracleParameter[])OracleDataAccessParameterCache.paramCache[hashKey];
            if (cachedParameters == null)
            {
                cachedParameters = (OracleParameter[])(OracleDataAccessParameterCache.paramCache[hashKey] = OracleDataAccessParameterCache.DiscoverSpParameterSet(connectionString, spName, includeReturnValueParameter));
            }
            return OracleDataAccessParameterCache.CloneParameters(cachedParameters);
        }
    }
}
