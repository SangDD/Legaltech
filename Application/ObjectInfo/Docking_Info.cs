using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class Docking_Info
    {
        public decimal STT { get; set; }
        public decimal Docking_Id { get; set; }
        public string Case_Code { get; set; }
        public string Ap_Case_Code { get; set; }
        public decimal Docking_Type { get; set; }
        public string Docking_Type_Name { get; set; }

        public string Document_Name { get; set; }

        public decimal Document_Type { get; set; }
        public string Document_Type_Name { get; set; }

        public decimal Status { get; set; }
        public string Status_Name { get; set; }


        public DateTime Deadline { get; set; }
        public decimal Isshowcustomer { get; set; }
        public DateTime In_Out_Date { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
        public string Language_Code { get; set; }
        public string Url { get; set; }
        public string Notes { get; set; }
    }
}
