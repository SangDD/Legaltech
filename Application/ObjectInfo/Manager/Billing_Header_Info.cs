using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class Billing_Header_Info
    {
        public decimal STT { get; set; }
        public decimal Billing_Id { get; set; }
        public string Case_Code { get; set; }
        public decimal Billing_Type { get; set; }
        public string Billing_Type_Name { get; set; }

        public string App_Case_Code { get; set; }
        public DateTime Billing_Date { get; set; }
        public DateTime Deadline { get; set; }

        int _SoNgayTre;
        public int SoNgayTre
        {
            get
            {
                TimeSpan _ts = DateTime.Now.Date - Billing_Date.Date;
                return (int)_ts.TotalDays;
            }
            set
            {
                _SoNgayTre = value;
            }
        }

        public string Request_By { get; set; }
        public string Request_By_Name { get; set; }

        public string Approve_By { get; set; }
        public string Approve_By_Name { get; set; }

        public decimal Status { get; set; }
        public string Status_Name { get; set; }

        public decimal Pay_Status { get; set; }
        public string Pay_Status_Name { get; set; }

        public DateTime Pay_Date { get; set; }
        public decimal Total_Pre_Tex { get; set; }
        public decimal Tex_Fee { get; set; }
        public decimal Total_Amount { get; set; }
        public string Currency { get; set; }
        public decimal Currency_Rate { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Language_Code { get; set; }
        public decimal Deleted { get; set; }
        public decimal Is_AdviceFilling { get; set; }

    }
}
