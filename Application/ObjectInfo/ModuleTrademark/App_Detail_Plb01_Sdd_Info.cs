using System;
using System.Web;

namespace ObjectInfos
{

    public class App_Detail_PLB01_SDD_Info : ApplicationHeaderInfo
    {
        public decimal Id { get; set; }
        public string Language_Code { get; set; }

        public decimal App_Header_Id { get; set; }
        public string Appcode { get; set; }
        public int Request_Change_Type { get; set; }
        public string App_No_Change { get; set; }
        public int Request_To_Type { get; set; }
        public string Request_To_Content { get; set; }
        public int Number_Pic { get; set; }
        public int Number_Page { get; set; }

        public decimal Fee_Id_1 { get; set; }
        public decimal Fee_Id_1_Val { get; set; }

        public decimal Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Val { get; set; }

        public decimal Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Val { get; set; }

        public decimal Fee_Id_22 { get; set; }
        public decimal Fee_Id_22_Val { get; set; }

        public decimal Total_Fee { get; set; }

    }
}
