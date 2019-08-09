using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Common
    {
        public static int RecordOnpage = 10;
        public static decimal Country_VietNam_Id = 234;
        public static string Currency_Type_VND = "VND";
        public static string Currency_Type_USD = "USD";

        public static decimal Tax = 5;
        public static bool c_is_call_change_remind = false;

        public static string BaseUrl { get; set; }
        public static string BaseDir { get; set; }
        public static Dictionary<string, string> c_dic_Injection = new Dictionary<string, string>();

    }

    public class ErrorCode
    {
        public static readonly int Error = -33;
        public static readonly int Success = 0;
    }
}
