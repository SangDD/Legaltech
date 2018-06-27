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
        public decimal Fee_Id_1_Check { get; set; }
        public string Fee_Id_1_Val { get; set; }

        public decimal Fee_Id_2 { get; set; }
        public decimal Fee_Id_2_Check { get; set; }
        public string Fee_Id_2_Val { get; set; }

        public decimal Fee_Id_21 { get; set; }
        public decimal Fee_Id_21_Check { get; set; }
        public string Fee_Id_21_Val { get; set; }

        public decimal Fee_Id_22 { get; set; }
        public decimal Fee_Id_22_Check { get; set; }
        public string Fee_Id_22_Val { get; set; }

        public decimal Total_Fee { get; set; }
        public string Total_Fee_Str { get; set; }

        // các tài liệu trong đơn
        public string Doc_Id_1 { get; set; }
        public decimal Doc_Id_1_Check { get; set; }

        public string Doc_Id_2 { get; set; }
        public decimal Doc_Id_2_Check { get; set; }

        public string Doc_Id_3 { get; set; }
        public decimal Doc_Id_3_Check { get; set; }

        public string Doc_Id_4 { get; set; }
        public decimal Doc_Id_4_Check { get; set; }

        public string Doc_Id_5 { get; set; }
        public decimal Doc_Id_5_Check { get; set; }
        public decimal Doc_Id_6_Check { get; set; }
        public decimal Doc_Id_7_Check { get; set; }
        public decimal Doc_Id_8_Check { get; set; }

        public string Doc_Id_9 { get; set; }
        public decimal Doc_Id_9_Check { get; set; }
        public decimal Doc_Id_10_Check { get; set; }

        public string Doc_Id_11 { get; set; }
        public decimal Doc_Id_11_Check { get; set; }

    }
}
