using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    public class Billing_Detail_Info
    {
        public decimal Billing_Detail_Id { get; set; }
        public decimal Billing_Id { get; set; }
        public string Biling_Detail_Name { get; set; }
        public string Biling_Detail_Name_EN { get; set; }
        public decimal Type { get; set; }
        public decimal Nation_Fee { get; set; }
        public decimal Represent_Fee { get; set; }
        public decimal Service_Fee { get; set; }
        public decimal Total_Fee { get; set; }
        public decimal Ref_Id { get; set; }
    }
}
