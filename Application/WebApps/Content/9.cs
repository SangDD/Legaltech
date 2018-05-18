using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Oracle.DataAccess.Client;
using Npgsql;
using NpgsqlTypes;

namespace DBAccess
{
    public class MarketWatch_DA
    {
        public long MarketWatchChart_Insert(string p_Symbol, string p_PaymentUnit,
            double p_LastestPrice, double p_PrevLastestPrice, double p_PriceChangeVolume, double p_PriceChange,
            double p_HighestPrice, double p_LowestPrice, double p_BtcVolume, double p_CurrencyVolume, int p_SeqNum)
        {
            try
            {
                string _str = "\"dbo\".\"marketwatchs_insert\" ";
                Npgsql_Helper _helper = new Npgsql_Helper();
                NpgsqlParameter[] _para = new NpgsqlParameter[11];
                _para[0] = new NpgsqlParameter("p_symbol", p_Symbol);
                _para[1] = new NpgsqlParameter("p_paymentunit", p_PaymentUnit);

                NpgsqlParameter _NpgsqlParameter2 = new NpgsqlParameter("p_lastestprice", p_LastestPrice);
                _NpgsqlParameter2.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[2] = _NpgsqlParameter2;

                NpgsqlParameter _NpgsqlParameter3 = new NpgsqlParameter("p_prevlastestprice", p_PrevLastestPrice);
                _NpgsqlParameter3.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[3] = _NpgsqlParameter3;

                NpgsqlParameter _NpgsqlParameter4 = new NpgsqlParameter("p_pricechangevolume", p_PriceChangeVolume);
                _NpgsqlParameter4.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[4] = _NpgsqlParameter4;

                NpgsqlParameter _NpgsqlParameter5 = new NpgsqlParameter("p_pricechange", p_PriceChange);
                _NpgsqlParameter5.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[5] = _NpgsqlParameter5;

                NpgsqlParameter _NpgsqlParameter6 = new NpgsqlParameter("p_highestprice", p_HighestPrice);
                _NpgsqlParameter6.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[6] = _NpgsqlParameter6;

                NpgsqlParameter _NpgsqlParameter7 = new NpgsqlParameter("p_lowestprice", p_LowestPrice);
                _NpgsqlParameter7.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[7] = _NpgsqlParameter7;

                NpgsqlParameter _NpgsqlParameter8 = new NpgsqlParameter("p_btcvolume", p_BtcVolume);
                _NpgsqlParameter8.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[8] = _NpgsqlParameter8;

                NpgsqlParameter _NpgsqlParameter9 = new NpgsqlParameter("p_currencyvolume", p_CurrencyVolume);
                _NpgsqlParameter9.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[9] = _NpgsqlParameter9;

                NpgsqlParameter _NpgsqlParameter10 = new NpgsqlParameter("p_seqprocess", p_SeqNum);
                _NpgsqlParameter10.NpgsqlDbType = NpgsqlDbType.Numeric;
                _para[10] = _NpgsqlParameter10;


                long _re = _helper.ExecuteNonQuery(NaviCommon.Config_Info.c_ConnectionString_PG, _str, _para);

                return 0;
            }
            catch (Exception ex)
            {
                NaviCommon.Common.log.Error(ex.ToString());
                return -1;
            }
        }
    }
}
