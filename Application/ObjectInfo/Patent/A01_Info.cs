using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
    public class A01_Info : ApplicationHeaderInfo
    {
        public decimal A01_Id { set; get; }
        public decimal Used_Special { set; get; }

        //public string Case_Code { get; set; }
        public decimal App_Header_Id { get; set; }
        public string Language_Code { get; set; }
        public string Appno { get; set; }
        public string Patent_Type { get; set; }
        public string Patent_Name { get; set; }
        public string Source_PCT { get; set; }
        public string PCT_Number { get; set; }
        public DateTime PCT_Filling_Date_Qt { get; set; }
        public string PCT_Number_Qt { get; set; }
        public DateTime PCT_Date { get; set; }
        public DateTime PCT_VN_Date { get; set; }
        public decimal PCT_Suadoi { get; set; }
        public string PCT_Suadoi_Type { get; set; }
        public string PCT_Suadoi_Content { get; set; }
        public string Source_DQSC { get; set; }
        public string DQSC_Origin_App_No { get; set; }
        public DateTime DQSC_Filling_Date { get; set; }
        public decimal DQSC_Valid_Before { get; set; }
        public decimal DQSC_Valid_After { get; set; }
        public string Source_GPHI { get; set; }
        public string GPHI_Origin_App_No { get; set; }
        public DateTime GPHI_Filling_Date { get; set; }
        public decimal GPHI_Valid_Before { get; set; }
        public decimal GPHI_Valid_After { get; set; }

        public string ThamDinhNoiDung { get; set; }
        public string ChuyenDoiDon { get; set; }
    }
}
