
namespace Common.MessageCode
{
    public sealed partial class KnMessageCode
    {
        // Add
        public static readonly KnMessage AddSupplierSuccess = new KnMessage("Thêm mới nhà cung cấp thành công!", 1600);

        public static readonly KnMessage AddSupplierFail = new KnMessage("Thêm mới nhà cung cấp thất bại!", -1601);

        public static readonly KnMessage AddSupplierFailSupplierExisted = new KnMessage(
            "Thêm mới nhà cung cấp thất bại! nhà cung cấp đã tồn tại trong hệ thống.",
            -1602);
        //delete
        public static readonly KnMessage DeleteSupplierSuccess = new KnMessage("Xóa nhà cung cấp thành công!", 1602);

        public static readonly KnMessage DeleteSupplierFail = new KnMessage("Xóa nhà cung cấp thất bại!", -1603);

        public static readonly KnMessage DeleteSupplierFailSupplierExisted = new KnMessage(
            "Xóa nhà cung cấp thất bại! nhà cung cấp đã tồn tại trong hệ thống.",
            -1604);
    }
}
