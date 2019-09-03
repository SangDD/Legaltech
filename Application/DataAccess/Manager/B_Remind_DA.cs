using Common;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class B_Remind_DA
    {
        public int Auto_change_remind()
        {
            try
            {
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "pkg_remind.proc_auto_change_remind");
                return 0;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return -1;
            }
        }


    }
}
