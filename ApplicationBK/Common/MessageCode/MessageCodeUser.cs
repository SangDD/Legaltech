namespace Common.MessageCode
{
	public sealed partial class KnMessageCode
	{
		// Add
		public static readonly KnMessage AddUserSuccess = new KnMessage("Thêm mới người dùng thành công!", 1200);
        public static readonly KnMessage AddProductMarkSuccess = new KnMessage("Thêm mới hãng sản xuất thành công!", 1200);
        public static readonly KnMessage EditProductMarkSuccess = new KnMessage("Cập nhật hãng sản xuất thành công!", 1301);
        public static readonly KnMessage DeleteProductMarkSuccess = new KnMessage("Xóa hãng sản xuất thành công!", 1302);

        public static readonly KnMessage AddUserFail = new KnMessage("Thêm mới người dùng thất bại!", -1201);
        public static readonly KnMessage AddProductMarkFail = new KnMessage("Thêm mới hãng sản xuất thất bại!", -1300);
        public static readonly KnMessage EditProductMarkFail = new KnMessage("Cập nhật hãng sản xuất thất bại!", -1301);
        public static readonly KnMessage DeleteProductMarkFail = new KnMessage("Xóa hãng sản xuất thất bại!", -1302);

        public static readonly KnMessage AddUserFailUserExisted = new KnMessage(
			"Thêm mới người dùng thất bại! Tên người dùng đã tồn tại trong hệ thống.",
			-1202);
		
		// Edit
		public static readonly KnMessage EditUserSuccess = new KnMessage("Sửa thông tin người dùng thành công!", 1203);

		public static readonly KnMessage EditUserFail = new KnMessage("Sửa thông tin người dùng thất bại!", -1204);

		public static readonly KnMessage EditUserFailUserNotExist = new KnMessage(
			"Sửa thông tin người dùng thất bại! Người dùng không tồn tại trong hệ thống.",
			-1205);

		// Delete
		public static readonly KnMessage DeleteUserSuccess = new KnMessage("Xóa người dùng thành công!", 1206);

		public static readonly KnMessage DeleteUserFail = new KnMessage("Xóa người dùng thất bại!", -1207);

		public static readonly KnMessage DeleteUserFailUserActive = new KnMessage(
			"Xóa người dùng thất bại! Không thể xóa người dùng có trạng thái đang hoạt động.",
			-1208);

		public static readonly KnMessage DeleteUserFailUserNotExist = new KnMessage(
			"Xóa người dùng thất bại! Người dùng không tồn tại trong hệ thống.",
			-1209);

		public static readonly KnMessage ResetPasswordUserSuccess = new KnMessage("Cài đặt lại mật khẩu thành công !", 1210);

		public static readonly KnMessage ResetPasswordUserFail = new KnMessage("Cài đặt lại mật khẩu thất bại !", -1211);

		public static readonly KnMessage ResetPasswordUserFailUserNotExist = new KnMessage(
			"Cài đặt lại mật khẩu thất bại ! Người dùng không tồn tại trong hệ thống.",
			-1212);

		public static readonly KnMessage ChangePasswordUserFail = new KnMessage("Thay đổi mật khẩu thất bại.", -1213);

		public static readonly KnMessage ChangePasswordUserFailDataInvalid =
			new KnMessage("Thay đổi mật khẩu thất bại! Dữ liệu không hợp lệ.", -1214);

		public static readonly KnMessage ChangePasswordUserFailPwdWrong = new KnMessage(
			"Thay đổi mật khẩu thất bại! Mật khẩu hiện tại không chính xác.",
			-1215);

		public static readonly KnMessage ChangePasswordUserSuccess = new KnMessage(
			"Thay đổi mật khẩu thành công! Vui lòng đăng nhập lại hệ thống.",
			1216);

		// User forgot password
		public static readonly KnMessage UserForgotPasswordUserNotExist = new KnMessage("Người dùng không tồn tại trong hệ thống.", -1217);

		public static readonly KnMessage UserForgotPasswordHaveNoEmail = new KnMessage(
			"Người dùng không có thông tin email để nhận thông tin khôi phục mật khẩu.",
			-1218);

		public static readonly KnMessage UserForgotPasswordCreateMailError = new KnMessage(
			"Có lỗi trong quá trình gửi thông tin đến email của bạn. Vui lòng kiểm tra lại kết nối",
			-1219);

		public static readonly KnMessage UserForgotPasswordCreateMailSuccess = new KnMessage(
			"Thông tin khôi phục mật khẩu sẽ được gửi đến email của bạn. Vui lòng kiểm tra lại email.",
			1220);
	}
}
