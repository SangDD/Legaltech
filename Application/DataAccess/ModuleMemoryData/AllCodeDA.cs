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
				ds = OracleHelper.ExecuteDataset( Configuration.GetConnectionString(), CommandType.StoredProcedure, "pkg_allcode.proc_AllCode_GetAll",
					new OracleParameter("p_cursor", OracleDbType.RefCursor, ParameterDirection.Output));
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


                //NpgsqlCommand command = new NpgsqlCommand("select show_cities(@ref)", conn);
                //command.CommandType = CommandType.Text;
                //NpgsqlParameter p = new NpgsqlParameter();
                //p.ParameterName = "@ref";
                //p.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Refcursor;
                //p.Direction = ParameterDirection.InputOutput;
                //p.Value = "ref";
                //command.Parameters.Add(p);
                //command.ExecuteNonQuery();
                string fncdb = "\"legaltech\".\"func_get_allcode\";";
                //conn.Close();
                DataSet ds = new DataSet();
                PostGresHelper objHelper = new PostGresHelper();
                NpgsqlParameter[] p = new NpgsqlParameter[1];
                NpgsqlParameter pCusor = new NpgsqlParameter();
                pCusor.ParameterName = "@ref";
                pCusor.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Refcursor;
                pCusor.Direction = ParameterDirection.InputOutput;
                pCusor.Value = "ref";
                p[0] = pCusor;
                ds = objHelper.ExecuteDataset(Configuration.connectionString, fncdb, p);



                //var conn = new NpgsqlConnection(Configuration.connectionString);

                //conn.Open();

                //var command = conn.CreateCommand();
                //command.CommandText = "SELECT * FROM allcode;";
                //var reader = command.ExecuteReader();
                //string data = "";
                //while (reader.Read())
                //{
                //    data =
                //        string.Format("Reading from table=({0}, {1}, {2})",
                //            reader.GetString(0).ToString(),
                //            reader.GetString(1),
                //            reader.GetString(3).ToString()
                //            );
                //}

                //command.CommandText = "fetch all in \"ref\"";

            }
            catch (Exception ex)
            {
                LogInfo.LogException(ex);
                //return new DataSet();
            }


        }
    }
}
