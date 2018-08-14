using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectInfos
{
    /// <summary>
    /// Dùng chung cho một số info khác 
    /// </summary>
   public class Base_Info
    {
        public string Created_By { set; get; }

        public DateTime Created_Date { set; get; }

        public string Modified_By { set; get; }

        public DateTime Modified_Date { set; get; }

    }
}
