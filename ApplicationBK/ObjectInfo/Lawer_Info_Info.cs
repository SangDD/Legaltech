using System;

namespace ObjectInfos
{
    public class Lawer_Info
    {
        public decimal Lawer_Id { get; set; }
        public string Lawer_Name { get; set; }
        public decimal Hourly_Rate { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }

         public decimal Status { get; set; }

        public decimal Deleted { get; set; }
        public string Created_By { get; set; }
        public DateTime Created_Date { get; set; }
        public string Modify_By { get; set; }
        public DateTime Modify_Date { get; set; }
    }

}
