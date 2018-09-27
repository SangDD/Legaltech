using System;


namespace ObjectInfos.ModuleTrademark
{
    public class AppFeeFixInfo
    {
        public AppFeeFixInfo()
        {

        }

        public AppFeeFixInfo(decimal p_Fee_Id, decimal p_Isuse)
        {
            Fee_Id = p_Fee_Id;
            Isuse = p_Isuse;
        }

        public decimal Id { get; set; }
        public string Case_Code { get; set; }
        //public decimal App_Header_Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_Usd { get; set; }

        public decimal Fee_Id { get; set; }
        public decimal Isuse { get; set; }
        public decimal Number_Of_Patent { get; set; }


    }
}
