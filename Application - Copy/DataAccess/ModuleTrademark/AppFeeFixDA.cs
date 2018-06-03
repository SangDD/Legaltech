using Common;
using ObjectInfos.ModuleTrademark;
using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace DataAccess.ModuleTrademark
{
    public class AppFeeFixDA
    {
        public int AppFeeFixInsertBatch(List<AppFeeFixInfo> pInfo ,decimal pAppHeaderid)
        {
            try
            {
                int numberRecord = pInfo.Count;
                decimal[] App_Header_Id = new decimal[numberRecord];
                decimal[] Fee_Id = new decimal[numberRecord];
                decimal[] Isuse = new decimal[numberRecord];
                decimal[] Number_Of_Patent = new decimal[numberRecord];
                decimal[] Amount = new decimal[numberRecord];
                for (int i = 0; i < pInfo.Count; i++)
                {
                    App_Header_Id[i] = pAppHeaderid;
                    Fee_Id[i] = pInfo[i].Fee_Id;
                    Isuse[i] = pInfo[i].Isuse;
                    Number_Of_Patent[i] = pInfo[i].Number_Of_Patent ;
                    Amount[i] = pInfo[i].Amount;
                }
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExcuteBatchNonQuery(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_FEE_FIX.PROC_APP_FEE_FIX_INSERT", numberRecord,
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_FEE_ID", OracleDbType.Decimal, Fee_Id, ParameterDirection.Input),
                    new OracleParameter("P_ISUSE", OracleDbType.Decimal, Isuse, ParameterDirection.Input),
                    new OracleParameter("P_NUMBER_OF_PATENT", OracleDbType.Decimal, Number_Of_Patent , ParameterDirection.Input),
                    new OracleParameter("P_AMOUNT", OracleDbType.Decimal, Amount, ParameterDirection.Input),
                    paramReturn);

                var result = ErrorCode.Error;
                Oracle.DataAccess.Types.OracleDecimal[] _ArrReturn = (Oracle.DataAccess.Types.OracleDecimal[])paramReturn.Value;
                foreach (Oracle.DataAccess.Types.OracleDecimal _item in _ArrReturn)
                {
                    if (Convert.ToInt32(_item.ToString()) < 0)
                    {
                        result = Convert.ToInt32(_item.ToString());
                        break;
                    }
                    else
                    {
                        result = ErrorCode.Success;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }


        public int AppFeeFixUpdate(AppFeeFixInfo pInfo)
        {
            try
            {
                var paramReturn = new OracleParameter("P_RETURN", OracleDbType.Int32, ParameterDirection.Output);
                OracleHelper.ExecuteDataset(Configuration.connectionString, CommandType.StoredProcedure, "PKG_APP_FEE_FIX.PROC_APP_FEE_FIX_INSERT",
                    new OracleParameter("P_id", OracleDbType.Decimal, pInfo.Id, ParameterDirection.Input),
                    new OracleParameter("P_APP_HEADER_ID", OracleDbType.Decimal, pInfo.App_Header_Id, ParameterDirection.Input),
                    new OracleParameter("P_FEE_ID", OracleDbType.Decimal, pInfo.Fee_Id, ParameterDirection.Input),
                    new OracleParameter("P_ISUSE", OracleDbType.Decimal, pInfo.Isuse, ParameterDirection.Input),
                    new OracleParameter("P_NUMBER_OF_PATENT", OracleDbType.Decimal, pInfo.Number_Of_Patent, ParameterDirection.Input),
                    new OracleParameter("P_AMOUNT", OracleDbType.Decimal, pInfo.Amount, ParameterDirection.Input),
                    paramReturn);
                var result = Convert.ToInt32(paramReturn.Value.ToString());
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return ErrorCode.Error;
            }
        }

    }
}
