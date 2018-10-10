using System;

namespace ObjectInfos
{
    public class Sys_Search_Fix_Info
    {
        public decimal Id { get; set; }

        public decimal Country_Id { get; set; }
        public string Country_Name { get; set; }

        public decimal Search_Object { get; set; }
        public string Search_Object_Name { get; set; }

        public decimal Search_Type { get; set; }
        public string Search_Type_Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Amount_usd { get; set; }

        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modified_By { get; set; }
        public DateTime Modified_Date { get; set; }
    }

}
