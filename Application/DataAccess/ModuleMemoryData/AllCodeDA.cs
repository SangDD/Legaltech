namespace DataAccess.ModuleMemoryData
{
	using System;
	using System.Data;
	using CommonData;
    using Npgsql;
    using Oracle.DataAccess.Client;
    using NpgsqlTypes;
    public class AllCodeDA
	{
		public static DataSet GetAllInAllCode()
		{
			var ds = new DataSet();
			try
			{
				//ds = OracleHelper.ExecuteDataset( Configuration.GetConnectionString(), CommandType.StoredProcedure, "pkg_allcode.proc_AllCode_GetAll",
				//	new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
              PostGresHelper objHelper = new PostGresHelper();
              ds = objHelper.ExecuteDataset(Configuration.connectionString, "func_get_allcode", new NpgsqlParameter[0]);

              //DataSet  ds2 = objHelper.ExecuteDataset2(Configuration.connectionString, "func_get_allcode2", new NpgsqlParameter[0]);


            }
            catch (Exception ex)
			{
				LogInfo.LogException(ex);
			}

			return ds;
		}




        public static void AllCodeGetAll()
        {
            try
            {

                string fncdb = "func_get_allcode";
                //conn.Close();
                DataSet ds = new DataSet();
                PostGresHelper objHelper = new PostGresHelper();
                NpgsqlParameter[] p = new NpgsqlParameter[0];
                //NpgsqlParameter[] p = new NpgsqlParameter[1];
                //NpgsqlParameter pCusor = new NpgsqlParameter();
                //pCusor.ParameterName = "@ref";
                //pCusor.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Refcursor;
                //pCusor.Direction = ParameterDirection.InputOutput;
                //pCusor.Value = "@ref";
                //p[0] = pCusor;

                ds = objHelper.ExecuteDataset(Configuration.connectionString, fncdb, p);
                #region Lay du lieu theo cau lenh select 

                var conn = new NpgsqlConnection(Configuration.connectionString);
                conn.Open();
                var command = conn.CreateCommand();
                command.CommandText = "SELECT * FROM allcode;";
                var reader = command.ExecuteReader();
                string data = "";
                while (reader.Read())
                {
                    data =
                        string.Format("Reading from table=({0}, {1}, {2})",
                            reader.GetString(0).ToString(),
                            reader.GetString(1),
                            reader.GetString(3).ToString()
                            );
                }
                conn.Dispose();
                #endregion

            }
            catch (Exception ex)
            {
                LogInfo.LogException(ex);
                //return new DataSet();
            }


        }
    }
}
