using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.MessageCode
{
    public sealed partial class KnMessageCode
    {
        public static readonly KnMessage EditWareHouseSuccess = new KnMessage("Cập nhật kho thành công!", 1801);
        public static readonly KnMessage EditWareHouseFail = new KnMessage("Đã tồn tại kho!", -1801);

        public static readonly KnMessage DeleteWareHouseSuccess = new KnMessage("Xóa kho thành công!", 1802);
      
        public static readonly KnMessage AddWareHouseSuccess = new KnMessage("Thêm mới kho thành công!", 1800);
        public static readonly KnMessage AddWareHouseFail = new KnMessage("Đã tồn tại kho!", -1800);
    }
}
