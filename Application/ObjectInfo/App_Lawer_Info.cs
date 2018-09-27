using System;

namespace ObjectInfos
{
    public class App_Lawer_Info
    {
        public decimal Id { get; set; }
        //public decimal Application_Header_Id { get; set; }
        public decimal Lawer_Id { get; set; }
        public string Notes { get; set; }
        public string Language_Code { get; set; }

        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }

        public string Case_Code { get; set; }
        public string Client_Reference { get; set; }
        public string Case_Name { get; set; }
        
        public string App_Code { get; set; }
        public string App_Name { get; set; }
    }
}
