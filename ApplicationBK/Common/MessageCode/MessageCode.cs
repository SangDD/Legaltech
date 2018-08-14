namespace Common.MessageCode
{
	using System;
	using System.Linq;
	using System.Reflection;

	public class KnMessage
	{
		private readonly string _message;

		private readonly int _code;

		public KnMessage()
		{
		}

		public KnMessage(string message, int code)
		{
			this._message = message;
			this._code = code;
		}

		public string GetMessage()
		{
			return this._message;
		}

		public int GetCode()
		{
			return this._code;
		}
	}

	// Each code references to only 1 message
	// Message chung của hệ thống
	public partial class KnMessageCode
	{
		public static readonly KnMessage NullMessage = new KnMessage(Null.NullString, 0);

		public static readonly KnMessage SessionHasBeenTimeOut = new KnMessage("Hệ thống đã hết thời gian kết nối, bạn hãy đăng nhập lại.", -100);

		public static readonly KnMessage CannotConnectToServer = new KnMessage("Không kết nối được tới máy chủ. Vui lòng kiểm tra lại kết nối.", -101);

        // MessageLogin: 1xxx
        // MessageGroup: 11xx
        // MessageUser:  12xx
        // MessageFunction: 13xx
        // MessageFunctionsInGroup: 14xx
        // MessagePosition: 15xx
        // MessageSupplier: 16xx
        // MessageProductGroup : 17sxx
        // MessageWareHouse : 18xx
		// MessageCodeProductInstancesImportedFromSuppliers : 19xx

        // Next Module: code format = (n++)xxx, start at (n++)000
        public static KnMessage GetMvMessageByCode(int code)
		{
			var message = new KnMessage();
			try
			{
				var fieldValue = typeof(KnMessageCode)
					.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
					.Single(f => f.FieldType == typeof(KnMessage) && ((KnMessage)f.GetValue(Activator.CreateInstance(typeof(KnMessageCode)))).GetCode() == code).GetValue(Activator.CreateInstance(typeof(KnMessageCode)));
				message = (KnMessage)fieldValue;
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
			}

			return message;
		}
	}
}
