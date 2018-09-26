using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
   public class B_Todos_Info
    {
        public decimal STT { get; set; }

        public decimal TODO_ID { get; set; }

        public decimal TYPE { get; set; }

        public string CASE_CODE { get; set; }

        //public string Code { get; set; }

        public string CONTENT { get; set; }

        public string REQUEST_BY { get; set; }

        public DateTime REQUEST_DATE { get; set; }

        public string PROCESSOR_BY { get; set; }

        public DateTime PROCESSOR_DATE { get; set; }

        public int STATUS { get; set; }

        public string STATUS_NAME { get; set; }

        public string LANGUAGE_CODE { get; set; }

        public string REQUEST_BY_NAME { get; set; }

        public string PROCESSOR_BY_NAME { get; set; }
    }

    public class B_Remind_Info
    {
        public decimal STT { get; set; }

        public decimal REMIND_ID { get; set; }

        public string CASE_CODE { get; set; }

        public string Code { get; set; }

        public string CONTENT { get; set; }

        public string REQUEST_BY { get; set; }

        public DateTime REQUEST_DATE { get; set; }

        public string PROCESSOR_BY { get; set; }

        public DateTime PROCESSOR_DATE { get; set; }

        public int STATUS { get; set; }

        public string LANGUAGE_CODE { get; set; }

        public string REQUEST_BY_NAME { get; set; }

        public string PROCESSOR_BY_NAME { get; set; }
    }

    public class B_TodoNotify_Info
    {
        public int NUMBER_APPS { get; set; }

        public int NUMBER_QUETIONS { get; set; }

        public int NUMBER_SEARCH { get; set; }

        public int NUMBER_TIMESHEET { get; set; }

        public int NUMBER_BILLS { get; set; }

        int _total;
        public int Total {

            get
            {
                return NUMBER_APPS + NUMBER_QUETIONS + NUMBER_SEARCH + NUMBER_TIMESHEET + NUMBER_BILLS;
            }
            set
            {
                _total = value;
            }
        }
    }
}
