namespace Common.MessageCode
{
    public sealed partial class KnMessageCode
    {
        // Add
        public static readonly KnMessage AddProductGroupSuccess = new KnMessage("Thêm mới nhóm sản phẩm thành công!", 1700);

        public static readonly KnMessage AddProductGroupFail = new KnMessage("Thêm mới nhóm sản phẩm thất bại!", -1701);

        public static readonly KnMessage AddProductGroupFailProductGroupExisted = new KnMessage(
            "Thêm mới nhóm sản phẩm thất bại! nhóm sản phẩm đã tồn tại trong hệ thống.",
            -1702);
        // Edit
        public static readonly KnMessage EditProductGroupSuccess = new KnMessage("Cập nhật nhóm sản phẩm thành công!", 1701);

        public static readonly KnMessage EditProductGroupFail = new KnMessage("Cập nhật nhóm sản phẩm thất bại!", -1703);

        public static readonly KnMessage EditProductGroupFailProductGroupExisted = new KnMessage(
            "cập nhật nhóm sản phẩm thất bạ! nhóm sản phẩm đã tồn tại trong hệ thống.",
            -1704);
        //Delete 
        public static readonly KnMessage DeleteProductGroupSuccess = new KnMessage("Xóa nhóm sản phẩm thành công!", 1703);

        public static readonly KnMessage DeleteProductGroupFail = new KnMessage("Xóa nhóm sản phẩm thất bại!", -1705);

        public static readonly KnMessage DeleteProductGroupFailProductGroupExisted = new KnMessage(
            "Xóa nhóm sản phẩm thất bại! nhóm sản phẩm đã tồn tại trong hệ thống.",
            -1706);
    }
}
