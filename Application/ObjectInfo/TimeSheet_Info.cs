using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class Timesheet_Info
    {
        public decimal STT { get; set; }
        public decimal Id { get; set; }
        public string Name { get; set; }

        //public decimal App_Header_Id { get; set; }
        //public string App_Code { get; set; }
        //public string App_Name { get; set; }


        public decimal Lawer_Id { get; set; }
        public string Lawer_Name { get; set; }

        public string App_Case_Code { get; set; }
        public string Client_Reference { get; set; }
        public string Case_Name { get; set; }

        public string From_Time { get; set; }
        public string To_Time { get; set; }


        public DateTime Time_Date { get; set; }
        public decimal Hours { get; set; }
        public decimal Hours_Adjust { get; set; }

        public string Notes { get; set; }
        public decimal Status { get; set; }
        public string Status_Name { get; set; }

        public string Reject_Reason { get; set; }
        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
    }
}
