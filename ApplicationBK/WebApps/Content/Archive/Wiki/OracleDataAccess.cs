using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Xml;

namespace OracleDataAccess
{
    public sealed class OracleDataAccess
    {
        private enum OracleConnectionOwnership
        {
            Internal,
            External
        }

        private OracleDataAccess()
        {
        }

        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            for (int i = 0; i < commandParameters.Length; i++)
            {
                OracleParameter p = commandParameters[i];
                //if (p.get_Direction() == 3 && p.get_Value() == null)
                if (p.Direction == ParameterDirection.InputOutput && p.Value == null)
                {
                    p.Value = DBNull.Value;
                }
                command.Parameters.Add(p);
            }
        }

        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            if (commandParameters == null || parameterValues == null)
            {
                return;
            }
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }
            int i = 0;
            int j = commandParameters.Length;
            while (i < j)
            {
                commandParameters[i].Value = parameterValues[i];
                i++;
            }
        }

        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters)
        {
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            command.Connection = connection;
            command.CommandText = commandText;
            command.CommandType = commandType;
            if (commandParameters != null)
            {
                OracleDataAccess.AttachParameters(command, commandParameters);
            }
        }

        public static DataSet ExecuteDatasetPage(OracleTransaction transaction, CommandType commandType, string commandText, string tableName, int pageIndex, int pageSize)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, null);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, pageIndex, pageSize, tableName);
            cmd.Parameters.Clear();
            return ds;
        }

        public static int ExecuteNonQueryBulk(string connectionString, CommandType commandType, string commandText, int numRecords, params OracleParameter[] commandParameters)
        {
            int result;
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();
                result = OracleDataAccess.ExecuteNonQueryBulk(cn, commandType, commandText, numRecords, commandParameters);
            }
            return result;
        }

        public static int ExecuteNonQueryBulk(OracleConnection connection, CommandType commandType, string commandText, int numRecords, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            cmd.ArrayBindCount = numRecords;
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }

        public static int ExecuteNonQueryBulk(OracleTransaction transaction, CommandType commandType, string commandText, int numRecords, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            cmd.ArrayBindCount = numRecords;
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            int result;
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();
                result = OracleDataAccess.ExecuteNonQuery(cn, commandType, commandText, commandParameters);
            }
            return result;
        }

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteNonQuery(connection, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }

        public static int ExecuteNonQuery(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
        }

        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }

        public static int ExecuteNonQuery(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteDataset(connectionString, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            DataSet result;
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();
                result = OracleDataAccess.ExecuteDataset(cn, commandType, commandText, commandParameters);
            }
            return result;
        }

        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteDataset(connection, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }

        public static DataSet ExecuteDataset(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
        }

        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteDataset(transaction, commandType, commandText, null);
        }

        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            OracleDataAdapter da = new OracleDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.Parameters.Clear();
            return ds;
        }

        public static DataSet ExecuteDataset(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
        }

        private static OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, OracleDataAccess.OracleConnectionOwnership connectionOwnership)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters);
            OracleDataReader dr;
            if (connectionOwnership == OracleDataAccess.OracleConnectionOwnership.External)
            {
                dr = cmd.ExecuteReader();
            }
            else
            {
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            cmd.Parameters.Clear();
            return dr;
        }

        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteReader(connectionString, commandType, commandText, null);
        }

        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleConnection cn = new OracleConnection(connectionString);
            cn.Open();
            OracleDataReader result;
            try
            {
                result = OracleDataAccess.ExecuteReader(cn, null, commandType, commandText, commandParameters, OracleDataAccess.OracleConnectionOwnership.Internal);
            }
            catch
            {
                cn.Close();
                throw;
            }
            return result;
        }

        public static OracleDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
        }

        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteReader(connection, commandType, commandText, null);
        }

        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return OracleDataAccess.ExecuteReader(connection, null, commandType, commandText, commandParameters, OracleDataAccess.OracleConnectionOwnership.External);
        }

        public static OracleDataReader ExecuteReader(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteReader(connection, CommandType.StoredProcedure, spName);
        }

        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteReader(transaction, commandType, commandText, null);
        }

        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            return OracleDataAccess.ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, OracleDataAccess.OracleConnectionOwnership.External);
        }

        public static OracleDataReader ExecuteReader(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteScalar(connectionString, commandType, commandText, null);
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            object result;
            using (OracleConnection cn = new OracleConnection(connectionString))
            {
                cn.Open();
                result = OracleDataAccess.ExecuteScalar(cn, commandType, commandText, commandParameters);
            }
            return result;
        }

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteScalar(connection, commandType, commandText, null);
        }

        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        public static object ExecuteScalar(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
        }

        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteScalar(transaction, commandType, commandText, null);
        }

        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            object retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        public static object ExecuteScalar(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(OracleConnection connection, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteXmlReader(connection, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters);
            XmlReader retval = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return retval;
        }

        public static XmlReader ExecuteXmlReader(OracleConnection connection, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
        }

        public static XmlReader ExecuteXmlReader(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            return OracleDataAccess.ExecuteXmlReader(transaction, commandType, commandText, null);
        }

        public static XmlReader ExecuteXmlReader(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            OracleDataAccess.PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);
            XmlReader retval = cmd.ExecuteXmlReader();
            cmd.Parameters.Clear();
            return retval;
        }

        public static XmlReader ExecuteXmlReader(OracleTransaction transaction, string spName, params object[] parameterValues)
        {
            if (parameterValues != null && parameterValues.Length > 0)
            {
                OracleParameter[] commandParameters = OracleDataAccessParameterCache.GetSpParameterSet(transaction.Connection.ConnectionString, spName);
                OracleDataAccess.AssignParameterValues(commandParameters, parameterValues);
                return OracleDataAccess.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
            }
            return OracleDataAccess.ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
        }
    }
}
