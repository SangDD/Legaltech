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
        public decimal Request_Change_Type { get; set; }
        public string App_No_Change { get; set; }
        public decimal Request_To_Type { get; set; }
        public string Request_To_Content { get; set; }
        public decimal Number_Pic { get; set; }
        public decimal Number_Page { get; set; }
    }
}
