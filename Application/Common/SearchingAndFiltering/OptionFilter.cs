namespace CommonData
{
	using System;

	public class OptionFilter
	{
		private readonly CommonEnums.OrderOptions _orderOption = CommonEnums.OrderOptions.None;
		private int    _startAt;
		private int    _endAt;
		private string _orderBy;
		private string _columnOrder;
		private string _orderType;
		private int    _pageNumber;
		private int    _recordPerPage;

		public OptionFilter()
		{
			this.SetDefault();
		}

		public OptionFilter(string optionData)
		{
			if (string.IsNullOrEmpty(optionData))
			{
				this.SetDefault();
			}
			else
			{
				var arrOptionValue = optionData.Split('|');
				this._columnOrder = arrOptionValue[0];
				this._orderType = arrOptionValue[1];
				if (!string.IsNullOrEmpty(arrOptionValue[2]))
				{
					this._orderOption = (CommonEnums.OrderOptions)Convert.ToInt32(arrOptionValue[2]);
				}
				
				this._pageNumber = Convert.ToInt32(arrOptionValue[3]);
				this._recordPerPage = Convert.ToInt32(arrOptionValue[4]);

				this.CalculateStartAt();
				this.CalculateEndAt();
				this.SettingOrderBy();
			}
		}

		public int PageNumber
		{
			get
			{
				return this._pageNumber;
			}

			set
			{
				this._pageNumber = value;
			}
		}

		public int RecordPerPage
		{
			get
			{
				return this._recordPerPage;
			}

			set
			{
				this._recordPerPage = value;
			}
		}

		public int StartAt
		{
			get
			{
				return this._startAt;
			}

			set
			{
				this._startAt = value;
			}
		}

		public int EndAt
		{
			get
			{
				return this._endAt;
			}

			set
			{
				this._endAt = value;
			}
		}

		public string OrderColumn
		{
			get
			{
				return this._columnOrder;
			}

			set
			{
				this._columnOrder = value;
			}
		}

		public string OrderType
		{
			get
			{
				return this._orderType;
			}

			set
			{
				this._orderType = value;
			}
		}

		public string OrderBy
		{
			get
			{
				return this._orderBy;
			}

			set
			{
				this._orderBy = value;
			}
		}

		public void CalculateStartAt()
		{
			this._startAt = this._recordPerPage * (this._pageNumber - 1) + 1;
		}

		public void CalculateEndAt()
		{
			this._endAt = this._recordPerPage * this._pageNumber;
		}

		public void SettingOrderBy()
		{
			if (this._orderOption == CommonEnums.OrderOptions.String)
			{
				this._columnOrder = "NLSSORT(UPPER(" + this._columnOrder + "), 'NLS_SORT=BINARY_AI')";
			}

			this._orderBy = (this._columnOrder + " " + this._orderType).Trim().ToUpper();
		}

		private void SetDefault()
		{
			this._orderBy = Null.NullString;
			this._pageNumber = 1;
			this._startAt = 1;
			this._endAt = PagingHelper.DefaultRecordPerPage;
			this._recordPerPage = PagingHelper.DefaultRecordPerPage;
		}
	}
}
