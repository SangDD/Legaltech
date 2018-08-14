namespace Common.MessageCode
{
	public sealed partial class KnMessageCode
	{
		// Add
		public static readonly KnMessage AddGroupSuccess = new KnMessage("Thêm mới nhóm quyền thành công!", 1100);

		public static readonly KnMessage AddGroupFail = new KnMessage("Thêm mới nhóm quyền thất bại!", -1101);

		public static readonly KnMessage AddGroupFailGroupExisted = new KnMessage(
			"Thêm mới nhóm quyền thất bại! Nhóm quyền đã tồn tại trong hệ thống.",
			-1102);

		// Edit
		public static readonly KnMessage EditGroupSuccess = new KnMessage("Sửa thông tin nhóm quyền thành công!", 1103);

		public static readonly KnMessage EditGroupFail = new KnMessage("Sửa thông tin nhóm quyền thất bại!", -1104);

		public static readonly KnMessage EditGroupFailGroupNameExisted = new KnMessage(
			"Sửa thông tin nhóm quyền thất bại! Nhóm quyền đã tồn tại trong hệ thống.",
			-1105);

		public static readonly KnMessage EditGroupFailGroupNotExist = new KnMessage(
			"Sửa thông tin nhóm quyền thất bại! Nhóm quyền không tồn tại trong hệ thống.",
			-1106);

		// Delete
		public static readonly KnMessage DeleteGroupSuccess = new KnMessage("Xóa nhóm quyền thành công!", 1107);

		public static readonly KnMessage DeleteGroupFail = new KnMessage("Xóa nhóm quyền thất bại!", -1108);

		public static readonly KnMessage DeleteGroupFailUserExistInGroup = new KnMessage(
			"Xóa nhóm quyền thất bại! Đang tồn tại người dùng trong nhóm.",
			-1109);

		public static readonly KnMessage DeleteGroupFailGroupNotExist = new KnMessage(
			"Xóa nhóm quyền thất bại! Nhóm quyền không tồn tại trong hệ thống.",
			-1110);

		public static readonly KnMessage SetupFunctionsToGroupSuccess = new KnMessage("Phân quyền cho nhóm thành công!", 1111);

		public static readonly KnMessage SetupFunctionsToGroupFail = new KnMessage("Phân quyền cho nhóm thất bại!", -1112);
	}
}
