using System;

namespace ObjectInfos
{
    public class Search_Fix_Info
    {
        public decimal Id { get; set; }

        public decimal Search_Id { get; set; }
        public decimal Status { get; set; }
        public decimal Number_Of_Class { get; set; }

        public decimal Country_Id { get; set; }
        public string Country_Name { get; set; }

        public decimal Search_Object { get; set; }
        public string Search_Object_Name { get; set; }

        public decimal Search_Type { get; set; }
        public string Search_Type_Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Amount_usd { get; set; }
    }

}
