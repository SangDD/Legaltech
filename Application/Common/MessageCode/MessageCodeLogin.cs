namespace Common.MessageCode
{
	public sealed partial class KnMessageCode
	{
		public static readonly KnMessage LoginSuccess = new KnMessage("Đăng nhập thành công!", 1000);
        public static readonly KnMessage LoginSuccess_En = new KnMessage("Login Sucess!", 1000);

        public static readonly KnMessage LoginFailed = new KnMessage("Đăng nhập thất bại!", -1001);

		public static readonly KnMessage LoginFailedUsernameOrPwdWrong = new KnMessage("Tên đăng nhập hoặc mật khẩu không đúng!", -1002);

		public static readonly KnMessage LoginFailAccountStopped = new KnMessage("Đăng nhập thất bại. Tài khoản đã ngừng hoạt động!", -1003);
	}
}
