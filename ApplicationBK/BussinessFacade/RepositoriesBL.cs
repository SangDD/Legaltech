namespace BussinessFacade
{
	using System;
	using System.Collections.Generic;

	using Common.Helpers;
	using Common.MessageCode;
	using Common.SearchingAndFiltering;

	public abstract class RepositoriesBL
	{
		private readonly ActionBusinessResult _actionResult;

		private string _pagingHtml;

		protected RepositoriesBL()
		{
			this._actionResult = new ActionBusinessResult();
		}

		protected void SetActionSuccess(bool success)
		{
			this._actionResult.IsActionSuccess = success;
		}

		protected bool GetActionSuccess()
		{
			return this._actionResult.IsActionSuccess;
		}

		protected void SetActionMessage(KnMessage mesageCode)
		{
			this._actionResult.MessageCode = mesageCode;
		}

		protected ActionBusinessResult SetActionResult(int result, KnMessage messageOnSuccess)
		{
			var mesageCode = this.GetActionSuccess() ? messageOnSuccess : KnMessageCode.GetMvMessageByCode(result);
			this.SetActionMessage(mesageCode);
			return this._actionResult;
		}

		protected ActionBusinessResult GetActionResult()
		{
			return this._actionResult;
		}

		protected void SetupPagingHtml(OptionFilter optionFilter, int totalRecordFindResult, string jsPagingFunction, string idDivNumberRecordOnPage)
		{
			var pagingHelper = new PagingHelper(optionFilter, totalRecordFindResult, jsPagingFunction, idDivNumberRecordOnPage);
			this._pagingHtml = pagingHelper.Paging();
		}

		protected void SetAdditionalDataResult(List<JsonData> additionalDataCollection)
		{
			this._actionResult.SetAdditionalData(additionalDataCollection);
		}

		public string GetPagingHtml()
		{
			return this._pagingHtml;
		}
	}

	public sealed class ActionBusinessResult
	{
		private bool _isActionSuccess;

		private KnMessage _mesageCode;

		private List<JsonData> _additionalDataCollection;

		public ActionBusinessResult()
		{
			this._isActionSuccess = false;
			this._mesageCode = KnMessageCode.NullMessage;
		}

		public bool IsActionSuccess
		{
			get
			{
				return this._isActionSuccess;
			}

			set
			{
				this._isActionSuccess = value;
			}
		}

		public KnMessage MessageCode
		{
			get
			{
				return this._mesageCode;
			}

			set
			{
				this._mesageCode = value;
			}
		}

		public object ToJson()
		{
			return this._additionalDataCollection == null ? this.ToJsonData() : this.ToJsonWithAdditionalData();
		}

		private object ToJsonData()
		{
			return new
			{
				IsActionSuccess,
				code = this._mesageCode.GetCode(),
				message = this._mesageCode.GetMessage()
			};
		}

		private object ToJsonWithAdditionalData()
		{
			var jsonResult = new Dictionary<string, object>
							 {
								 { "IsActionSuccess", this.IsActionSuccess },
								 { "code", this._mesageCode.GetCode() },
								 { "message", this._mesageCode.GetMessage() }
							 };
			foreach (var item in this._additionalDataCollection)
			{
				jsonResult.Add(item.GetName(), item.GetValue());
			}

			return jsonResult;
		}

		internal void SetAdditionalData(List<JsonData> additionalDataCollection)
		{
			this._additionalDataCollection = additionalDataCollection;
		}
	}

	public sealed class JsonData
	{
		private readonly string _name;

		private readonly string _value;

		public JsonData(string name, string value)
		{
			this._name = name;
			this._value = value;
		}

		public string GetName()
		{
			return this._name;
		}

		public string GetValue()
		{
			return this._value;
		}
	}
}
