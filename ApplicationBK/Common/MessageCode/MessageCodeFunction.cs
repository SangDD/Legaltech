namespace Common.MessageCode
{
	public sealed partial class KnMessageCode
	{
		// Add
		public static readonly KnMessage AddFunctionSuccess = new KnMessage("Thêm mới chức năng thành công!", 1300);

		public static readonly KnMessage AddFunctionFail = new KnMessage("Thêm mới chức năng thất bại!", -1301);

		public static readonly KnMessage AddFunctionFailFunctionExisted = new KnMessage(
			"Thêm mới chức năng thất bại! Url Post/Url Get đã tồn tại trong hệ thống.",
			-1302);

		// Edit
		public static readonly KnMessage EditFunctionSuccess = new KnMessage("Sửa chức năng thành công!", 1303);

		public static readonly KnMessage EditFunctionFail = new KnMessage("Sửa chức năng thất bại!", -1304);

		public static readonly KnMessage EditFunctionFailFunctionNameExisted = new KnMessage(
			"Sửa chức năng thất bại! Url Post/Url Get đã tồn tại trong hệ thống.",
			-1305);

		public static readonly KnMessage EditFunctionFailFunctionNotExist = new KnMessage("Sửa chức năng thất bại! Chức năng không tồn tại trong hệ thống.", -1306);

		// Delete
		public static readonly KnMessage DeleteFunctionSuccess = new KnMessage("Xóa chức năng thành công!", 1307);

		public static readonly KnMessage DeleteFunctionFail = new KnMessage("Xóa chức năng thất bại!", -1308);

		public static readonly KnMessage DeleteFunctionFailFunctionNotExist = new KnMessage("Xóa chức năng thất bại! Chức năng không tồn tại trong hệ thống.", -1309);
	}
}
