using System;
using System.Data;
using Npgsql;

namespace DataAccess
{
    public class PostGresHelper
    {
        public PostGresHelper(String connectionString)
        {
            _conn = connectionString;
        }

        public PostGresHelper()
        {
        }

        //####  private methods ###
        private string _conn = null;

        private NpgsqlCommand GetCommand(string query, NpgsqlParameter[] npgsqlParameters, CommandType commandType)
        {
            NpgsqlConnection conn = new NpgsqlConnection(_conn);
            //conn.UseSslStream = false;
            conn.Open();

            //query = query.ToLower();

            NpgsqlCommand command = new NpgsqlCommand(query, conn);
            command.CommandType = commandType;

            if (npgsqlParameters is NpgsqlParameter[])
            {
                command.Parameters.AddRange(npgsqlParameters);
            }

            return command;
        }

        //#### public methods ####

        public long ExecuteNonQuery(string connectionString, string query, NpgsqlParameter[] npgsqlParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            NpgsqlConnection connection = new NpgsqlConnection();
            try
            {
                //conn.UseSslStream = false;
                connection = new NpgsqlConnection(connectionString);
                connection.Open();

                return ExecuteNonQuery(connection, CommandType.StoredProcedure, query, npgsqlParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        public long ExecuteNonQuery(string connectionString, CommandType commandType, string query)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
            NpgsqlConnection connection = new NpgsqlConnection();
            try
            {
                //conn.UseSslStream = false;
                connection = new NpgsqlConnection(connectionString);
                connection.Open();

                return ExecuteNonQuery(connection, commandType, query, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        long ExecuteNonQuery(NpgsqlConnection con, CommandType commandType, string query, NpgsqlParameter[] npgsqlParameters)
        {

            NpgsqlCommand command = new NpgsqlCommand(query, con);
            command.CommandType = commandType;
            if (npgsqlParameters is NpgsqlParameter[])
            {
                command.Parameters.AddRange(npgsqlParameters);
            }

            Int32 rowsaffected;
            try
            {
                rowsaffected = command.ExecuteNonQuery();
                return rowsaffected;
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                command.Connection.Close();
            }
        }

        public object ExecuteScalar(string query, NpgsqlParameter[] npgsqlParameters)
        {
            return ExecuteScalar(CommandType.StoredProcedure, query, npgsqlParameters);
        }

        public object ExecuteScalar(CommandType commandType, string query, NpgsqlParameter[] npgsqlParameters)
        {
            using (NpgsqlCommand command = GetCommand(query, npgsqlParameters, commandType))
            {
                object result;

                try
                {
                    result = command.ExecuteScalar();
                    return result;
                }
                catch (Exception Ex)
                {
                    throw Ex;
                }
                finally
                {
                    command.Connection.Close();
                }
            }
        }

        public object ExecuteScalar(CommandType commandType, string query)
        {
            return ExecuteScalar(commandType, query, null);
        }

        public DataSet ExecuteDataset(string connectionString, string query, NpgsqlParameter[] npgsqlParameters)
        {
            NpgsqlConnection connection = GetConnection(connectionString);
            if (connection != null)
            {
                try
                {
                    connection.Open();
                    return ExecuteDataset(connection, CommandType.StoredProcedure, query, npgsqlParameters);
                }
                finally
                {
                    if ((connection != null)) connection.Dispose();
                }
            }
            else return null;
        }

        public DataSet ExecuteDataset(string connectionString, CommandType commandType, string query)
        {
            NpgsqlConnection connection = GetConnection(connectionString);
            if (connection != null)
            {
                try
                {
                    connection.Open();
                    return ExecuteDataset(connection, commandType, query, null);
                }
                finally
                {
                    if ((connection != null)) connection.Dispose();
                }
            }
            else return null;
        }

        DataSet ExecuteDataset(NpgsqlConnection con, CommandType commandType, string query, NpgsqlParameter[] npgsqlParameters)
        {
            NpgsqlCommand command = new NpgsqlCommand(query, con);
            command.CommandType = commandType;
            if (npgsqlParameters is NpgsqlParameter[])
            {
                command.Parameters.AddRange(npgsqlParameters);
            }

            DataSet myDS = new DataSet();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter();

            try
            {
                NpgsqlTransaction t = command.Connection.BeginTransaction();
                da = new NpgsqlDataAdapter(command);
                da.Fill(myDS);

                t.Commit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (((da != null))) da.Dispose();
            }

            return myDS;
 
        }

        NpgsqlConnection GetConnection(string connectionString)
        {
            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");
                NpgsqlConnection connection = null;

                //conn.UseSslStream = false;
                connection = new NpgsqlConnection(connectionString);
                return connection;
            }
            catch (Exception ex)
            {
                return null;
                //throw ex;
            }
        }
    }
}
