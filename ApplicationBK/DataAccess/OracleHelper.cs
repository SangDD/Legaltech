namespace DataAccess
{
    // ngocdt cleanup: 05/12/2017
    using System;
    using System.Data;

    using Oracle.DataAccess.Client;

    public class OracleHelper
    {
        #region "private utility methods & constructors"

        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new OracleHelper()".
        private OracleHelper()
        {
        }

        // this enum is used to indicate whether the connection was provided by the caller, or created by OracleHelper, so that
        // we can set the appropriate CommandBehavior when calling ExecuteReader()
        private enum OracleConnectionOwnership
        {
            Internal, // Connection is owned and managed by OracleHelper
            External // Connection is owned and managed by the caller
        }

        // This method is used to attach array of OracleParameters to a OracleCommand.
        // This method will assign a value of DbNull to any parameter with a direction of
        // InputOutput and a value of null. 
        // This behavior will prevent default values from being used, but
        // this will be the less common case than an intended pure output parameter (derived as InputOutput)
        // where the user provided no input value.
        // Parameters:
        // -command - The command to which the parameters will be added
        // -commandParameters - an array of OracleParameters to be added to command
        private static void AttachParameters(OracleCommand command, OracleParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (commandParameters != null)
            {
                foreach (var p in commandParameters)
                {
                    if (p == null) continue;

                    // Check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                    {
                        p.Value = DBNull.Value;
                    }

                    command.Parameters.Add(p);
                }
            }
        }

        // This method assigns dataRow column values to an array of OracleParameters.
        // Parameters:
        // -commandParameters: Array of OracleParameters to be assigned values
        // -dataRow: the dataRow used to hold the stored procedure' s parameter values
        private static void AssignParameterValues(OracleParameter[] commandParameters, DataRow dataRow)
        {
            if (commandParameters == null || dataRow == null)
            {
                // Do nothing if we get no data 
                return;
            }

            var i = 0;
            foreach (var commandParameter in commandParameters)
            {
                // Check the parameter name
                if (commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1)
                {
                    throw new Exception($"Please provide a valid parameter name on the parameter #{i}, the ParameterName property has the following value: ' {commandParameter.ParameterName}' .");
                }

                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                {
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                }

                i = i + 1;
            }
        }

        // This method assigns an array of values to an array of OracleParameters.
        // Parameters:
        // -commandParameters - array of OracleParameters to be assigned values
        // -array of objects holding the values to be assigned
        private static void AssignParameterValues(OracleParameter[] commandParameters, object[] parameterValues)
        {
            int i;
            if (commandParameters == null && parameterValues == null)
            {
                // Do nothing if we get no data
                return;
            }

            // We must have the same number of values as we pave parameters to put them in
            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            // Value array
            var j = commandParameters.Length - 1;
            for (i = 0; i <= j; i++)
            {
                // If the current array value derives from IDbDataParameter, then assign its Value property
                var paramInstance = parameterValues[i] as IDbDataParameter;
                if (paramInstance != null)
                {
                    commandParameters[i].Value = paramInstance.Value ?? DBNull.Value;
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        // This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        // to the provided command.
        // Parameters:
        // -command - the OracleCommand to be prepared
        // -connection - a valid OracleConnection, on which to execute this command
        // -transaction - a valid OracleTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParameters to be associated with the command or ' null' if no parameters are required
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
            }

            // command.Transaction = transaction
            // Set the command type
            command.CommandType = commandType;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            // End PrepareCommand
        }
        #endregion

        #region ExcuteBatchNonQuery
        public static int ExcuteBatchNonQuery(string connectionString, CommandType commandType, string commandText, int numItem, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExcuteBatchNonQuery(connection, commandType, commandText, numItem, commandParameters);
            }
            finally
            {
                connection.Dispose();
            }
        }

        public static int ExcuteBatchNonQuery(OracleConnection connection, CommandType commandType, string commandText, int numItem, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var mustCloseConnection = false;

            PrepareBatchCommand(cmd, connection, null, commandType, commandText, numItem, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            var retval = cmd.ExecuteNonQuery();

            // SangVV ADD de giai phong param truoc do, ko giai phong connecttion
            // if (cmd != null) cmd.Dispose();
            if (mustCloseConnection) connection.Close();

            return retval;
        }

        private static void PrepareBatchCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, int numItem, OracleParameter[] commandParameters, ref bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException(nameof(commandText));

            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
                mustCloseConnection = true;
            }
            else
            {
                mustCloseConnection = false;
            }

            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or Oracle statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it.
            if (transaction != null)
            {
                if (transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));
            }

            // command.Transaction = transaction
            // Set the command type
            command.CommandType = commandType;

            // Set Batch Item
            command.ArrayBindCount = numItem;

            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }
        #endregion

        #region "ExecuteNonQuery"

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connectionString, commandType, commandText, null);
        }

        /// <summary>
        /// Sangdd Thêm hàm sử dụng Commit (chưa test)
        /// Mục đích: Commit Transection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int Commit(string connectionString)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connectionString, CommandType.Text, "Commit", null);
        }

        /// <summary>
        /// HungTD: commit khi sử dụng transaction scope
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int CommitToDB(string connectionString)
        {
            try
            {
                var paramReturn = new OracleParameter("p_return", OracleDbType.Int32, ParameterDirection.Output);
                ExecuteDataset(connectionString, CommandType.StoredProcedure, "pkg_chay_du_lieu.commit_to_db", paramReturn);
                return Convert.ToInt32(paramReturn.Value.ToString());
            }
            catch
            {
                return -1;
            }
        }

        /// <summary>
        /// Sangdd thêm 
        /// Mục đích: để Rollback transection
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static int Rollback(string connectionString)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connectionString, CommandType.Text, "RollBack", null);
        }

        // Execute a OracleCommand (that returns no resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An int representing the number of rows affected by the command
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(connection, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns no resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();

            var mustCloseConnection = false;

            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            var retval = cmd.ExecuteNonQuery();

            // SangVV ADD de giai phong param truoc do, ko giai phong connecttion
            // if (cmd != null) cmd.Dispose();
            if (mustCloseConnection) connection.Close();

            return retval;
        }

        // Execute a OracleCommand (that returns no resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders")
        // Parameters:
        // -transaction - a valid OracleTransaction associated with the connection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteNonQuery(transaction, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns no resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim result As Integer = ExecuteNonQuery(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An int representing the number of rows affected by the command 
        public static int ExecuteNonQuery(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Finally, execute the command
            var retval = cmd.ExecuteNonQuery();

            return retval;
        }
        #endregion

        #region "ExecuteDataset"

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim ds As DataSet = OracleHelper.ExecuteDataset("", commandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connectionString, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(connection, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var ds = new DataSet();
            var dataAdatpter = new OracleDataAdapter();
            var mustCloseConnection = false;

            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);
            try
            {
                // Create the DataAdapter & DataSet
                dataAdatpter = new OracleDataAdapter(cmd);

                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);
            }
            catch (Exception)
            {
                // Ignored
            }
            finally
            {
                dataAdatpter.Dispose();
            }

            if (mustCloseConnection) connection.Close();

            // Return the dataset
            return ds;
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters
        // -transaction - a valid OracleTransaction
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteDataset(transaction, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim ds As Dataset = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or T-Oracle command
        // -commandParameters - an array of OracleParamters used to execute the command
        // Returns: A dataset containing the resultset generated by the command
        public static DataSet ExecuteDataset(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != null && transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var ds = new DataSet();
            var dataAdatpter = new OracleDataAdapter();
            var mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);
            try
            {
                // Create the DataAdapter & DataSet
                dataAdatpter = new OracleDataAdapter(cmd);

                // Fill the DataSet using default values for DataTable names, etc
                dataAdatpter.Fill(ds);
            }
            finally
            {
                dataAdatpter.Dispose();
            }

            return ds;
        }
        #endregion

        #region "ExecuteReader"
        // Create and prepare a OracleCommand, and call ExecuteReader with the appropriate CommandBehavior.
        // If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        // If the caller provided the connection, we want to leave it to them to manage.
        // Parameters:
        // -connection - a valid OracleConnection, on which to execute this command 
        // -transaction - a valid OracleTransaction, or ' null' 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParameters to be associated with the command or ' null' if no parameters are required 
        // -connectionOwnership - indicates whether the connection parameter was provided by the caller, or created by OracleHelper 
        // Returns: OracleDataReader containing the results of the command 
        private static OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, OracleParameter[] commandParameters, OracleConnectionOwnership connectionOwnership)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            var mustCloseConnection = false;

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            try
            {
                // Create a reader
                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                // Call ExecuteReader with the appropriate CommandBehavior
                var dataReader = connectionOwnership == OracleConnectionOwnership.External ? cmd.ExecuteReader() : cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return dataReader;
            }
            catch
            {
                if (mustCloseConnection) connection.Close();
                cmd.Dispose();
                throw;
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteReader(connectionString, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(connString, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            // Create & open a OracleConnection
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the private overload that takes an internally owned connection in place of the connection string
                return ExecuteReader(connection, null, commandType, commandText, commandParameters, OracleConnectionOwnership.Internal);
            }
            catch
            {
                // If we fail to return the OracleDatReader, we need to close the connection ourselves
                connection.Dispose();
                throw;
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText)
        {
            return ExecuteReader(connection, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            // Pass through the call to private overload using a null transaction value
            return ExecuteReader(connection, null, commandType, commandText, commandParameters, OracleConnectionOwnership.External);
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders")
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or SQL command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteReader(transaction, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim dr As OracleDataReader = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.)
        // -commandText - the stored procedure name or SQL command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: A OracleDataReader containing the resultset generated by the command 
        public static OracleDataReader ExecuteReader(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != null && transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            // Pass through to private overload, indicating that the connection is owned by the caller
            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, OracleConnectionOwnership.External);
        }
        #endregion

        #region "ExecuteScalar"

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connectionString, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a 1x1 resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = Cint(ExecuteScalar(connString, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -connectionString - a valid connection string for a OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            // Create & open a OracleConnection, and dispose of it after we are done.
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(connection, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a 1x1 resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -connection - a valid OracleConnection 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleConnection connection, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var mustCloseConnection = false;

            PrepareCommand(cmd, connection, null, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            var retval = cmd.ExecuteScalar();

            if (mustCloseConnection) connection.Close();

            return retval;
        }

        // Execute a OracleCommand (that returns a 1x1 resultset and takes no parameters) against the provided OracleTransaction.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount"))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText)
        {
            // Pass through the call providing null for the set of OracleParameters
            return ExecuteScalar(transaction, commandType, commandText, null);
        }

        // Execute a OracleCommand (that returns a 1x1 resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // Dim orderCount As Integer = CInt(ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new OracleParameter("@prodid", 24)))
        // Parameters:
        // -transaction - a valid OracleTransaction 
        // -commandType - the CommandType (stored procedure, text, etc.) 
        // -commandText - the stored procedure name or T-Oracle command 
        // -commandParameters - an array of OracleParamters used to execute the command 
        // Returns: An object containing the value in the 1x1 resultset generated by the command 
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != null && transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            // Create a command and prepare it for execution
            var cmd = new OracleCommand();
            var mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Execute the command & return the results
            var retval = cmd.ExecuteScalar();

            return retval;
        }
        #endregion

        #region "FillDataset"
        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the database specified in 
        // the connection string. 
        // e.g.: 
        // FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters: 
        // -connectionString: A valid connection string for a OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames);
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset) against the database specified in the connection string 
        // using the provided parameters.
        // e.g.: 
        // FillDataset (connString, CommandType.StoredProcedure, "GetOrders", ds, new String() = {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters: 
        // -connectionString: A valid connection string for a OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);

                connection.Open();

                // Call the overload that takes a connection in place of the connection string
                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            finally
            {
                connection.Dispose();
            }
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleConnection. 
        // e.g.: 
        // FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"})
        // Parameters:
        // -connection: A valid OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(OracleConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleConnection 
        // using the provided parameters.
        // e.g.: 
        // FillDataset (conn, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid OracleConnection
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(OracleConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {
            FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        // Execute a OracleCommand (that returns a resultset and takes no parameters) against the provided OracleTransaction. 
        // e.g.: 
        // FillDataset (trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"})
        // Parameters:
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        public static void FillDataset(OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
        }

        // Execute a OracleCommand (that returns a resultset) against the specified OracleTransaction
        // using the provided parameters.
        // e.g.: 
        // FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        public static void FillDataset(OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != null && transaction.Connection == null) throw new ArgumentException(@"The transaction was rollbacked or commited, please provide an open transaction.", nameof(transaction));

            FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
        }

        // Private helper method that execute a OracleCommand (that returns a resultset) against the specified OracleTransaction and OracleConnection
        // using the provided parameters.
        // e.g.: 
        // FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new String() {"orders"}, new OracleParameter("@prodid", 24))
        // Parameters:
        // -connection: A valid OracleConnection
        // -transaction: A valid OracleTransaction
        // -commandType: the CommandType (stored procedure, text, etc.)
        // -commandText: the stored procedure name or T-Oracle command
        // -dataSet: A dataset wich will contain the resultset generated by the command
        // -tableNames: this array will be used to create table mappings allowing the DataTables to be referenced
        // by a user defined name (probably the actual table name)
        // -commandParameters: An array of OracleParamters used to execute the command
        private static void FillDataset(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params OracleParameter[] commandParameters)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));

            // Create a command and prepare it for execution
            var command = new OracleCommand();

            var mustCloseConnection = false;
            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            // Create the DataAdapter & DataSet
            var dataAdapter = new OracleDataAdapter(command);
            try
            {
                // Add the table mappings specified by the user
                if (tableNames != null && tableNames.Length > 0)
                {
                    var tableName = "Table";
                    int index;
                    for (index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if (tableNames[index] == null || tableNames[index].Length == 0) throw new ArgumentException(@"The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", nameof(tableNames));
                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                        tableName = tableName + (index + 1);
                    }
                }

                // Fill the DataSet using default values for DataTable names, etc
                dataAdapter.Fill(dataSet);
            }
            finally
            {
                dataAdapter.Dispose();
            }

            if (mustCloseConnection) connection.Close();
        }
        #endregion

        #region "UpdateDataset"
        // Executes the respective command for each inserted, updated, or deleted row in the DataSet.
        // e.g.: 
        // UpdateDataset(conn, insertCommand, deleteCommand, updateCommand, dataSet, "Order")
        // Parameters:
        // -insertCommand: A valid transact-Oracle statement or stored procedure to insert new records into the data source
        // -deleteCommand: A valid transact-Oracle statement or stored procedure to delete records from the data source
        // -updateCommand: A valid transact-Oracle statement or stored procedure used to update records in the data source
        // -dataSet: the DataSet used to update the data source
        // -tableName: the DataTable used to update the data source
        public static void UpdateDataset(OracleCommand insertCommand, OracleCommand deleteCommand, OracleCommand updateCommand, DataSet dataSet, string tableName)
        {
            if (insertCommand == null) throw new ArgumentNullException(nameof(insertCommand));
            if (deleteCommand == null) throw new ArgumentNullException(nameof(deleteCommand));
            if (updateCommand == null) throw new ArgumentNullException(nameof(updateCommand));
            if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));
            if (string.IsNullOrEmpty(tableName)) throw new ArgumentNullException(nameof(tableName));

            // Create a OracleDataAdapter, and dispose of it after we are done
            var dataAdapter = new OracleDataAdapter();
            try
            {
                // Set the data adapter commands
                dataAdapter.UpdateCommand = updateCommand;
                dataAdapter.InsertCommand = insertCommand;
                dataAdapter.DeleteCommand = deleteCommand;

                // Update the dataset changes in the data source
                dataAdapter.Update(dataSet, tableName);

                // Commit all the changes made to the DataSet
                dataSet.AcceptChanges();
            }
            finally
            {
                dataAdapter.Dispose();
            }
        }
        #endregion

        // LinhNN kiểm tra database có kết nối được không
        public static bool CheckDatabase(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return false;

            // Create & open a OracleConnection, and dispose of it after we are done
            var connection = new OracleConnection();
            try
            {
                connection = new OracleConnection(connectionString);
                connection.Open();
                connection.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
