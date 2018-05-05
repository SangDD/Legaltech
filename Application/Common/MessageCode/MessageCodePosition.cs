namespace Common.MessageCode
{
	public sealed partial class KnMessageCode
	{
		// Add
		public static readonly KnMessage AddPositionSuccess = new KnMessage("Thêm mới chức vụ thành công!", 1500);

		public static readonly KnMessage AddPositionFail = new KnMessage("Thêm mới chức vụ thất bại!", -1501);

		public static readonly KnMessage AddPositionFailPositionExisted = new KnMessage(
			"Thêm mới chức vụ thất bại! Chức vụ đã tồn tại trong hệ thống.",
			-1502);
	}
}
