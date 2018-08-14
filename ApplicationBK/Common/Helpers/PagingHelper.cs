namespace Common.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	using SearchingAndFiltering;

	public class PagingHelper
	{
		public const int DefaultRecordPerPage = 10;
		public const int DefaultRecordPerPageMobile = 10;
		public const int DefaultNumberPageDisplay = 5;

		private readonly StringBuilder _htmlPagingResult = new StringBuilder();
		private readonly OptionFilter  _optionFilter;
		private readonly string        _pagingFunctionJS = "page";
		private readonly string        _divNumberRecordOnPage = "divNumberRecordOnPage";
		private readonly int           _totalRecord;
		private int           _curentPage;
		private int           _recordOnPage = 0;
		private string        _sequenceAdditionalPagingFunctionJs;
		private string        _activeClass = "a-active";
		private string        _itemName = "bản ghi";

		public PagingHelper(OptionFilter optionFilter, int totalRecord)
		{
			this._optionFilter = optionFilter;
			this._totalRecord = totalRecord;
			this._sequenceAdditionalPagingFunctionJs = string.Empty;
		}

		public PagingHelper(OptionFilter optionFilter, int totalRecord,  string pagingFunctionJS)
		{
			this._optionFilter = optionFilter;
			this._totalRecord = totalRecord;
			this._pagingFunctionJS = pagingFunctionJS;
			this._sequenceAdditionalPagingFunctionJs = string.Empty;
		}

		public PagingHelper(OptionFilter optionFilter, int totalRecord,  string pagingFunctionJS, string divNumberRecordOnPage)
		{
			this._optionFilter = optionFilter;
			this._totalRecord = totalRecord;
			this._pagingFunctionJS = pagingFunctionJS;
			this._divNumberRecordOnPage = divNumberRecordOnPage;
			this._sequenceAdditionalPagingFunctionJs = string.Empty;
		}

		public PagingHelper(OptionFilter optionFilter, int totalRecord,  string pagingFunctionJS, string divNumberRecordOnPage, string arrSequenceAdditionalPagingFunctionJs)
		{
			this._optionFilter = optionFilter;
			this._totalRecord = totalRecord;
			this._pagingFunctionJS = pagingFunctionJS;
			this._divNumberRecordOnPage = divNumberRecordOnPage;
			FilterArrSequenceAdditionalPagingFunctionJs(arrSequenceAdditionalPagingFunctionJs);
		}

		public static readonly List<int> RecordOnPageCollection = new List<int> { 10, 20, 30, 50 };

		private void CreateCboOptionRecordOnPage()
		{
			this._htmlPagingResult.Append("<select id=\"" + this._divNumberRecordOnPage + "\" class=\"divNumberRecordOnPage\" onchange=\"" + this._pagingFunctionJS + "(1"
			+ this._sequenceAdditionalPagingFunctionJs + ")\">");
			foreach (var item in RecordOnPageCollection)
			{
				this._htmlPagingResult.Append("<option value=\"" + item + "\" " + (item == this._optionFilter.RecordPerPage ? "selected" : string.Empty) + ">" + item + "</option>");
			}

			this._htmlPagingResult.Append("</select>");
		}

		private void CreatePagePrev()
		{
			if (this._optionFilter.PageNumber > 1)
			{
				this._htmlPagingResult.Append("<a class=\"prev\" href=\"javascript:" + this._pagingFunctionJS + "(" + (this._optionFilter.PageNumber - 1) 
				                        + this._sequenceAdditionalPagingFunctionJs + ");\">Prev</a>");
			}
		}

		private void CreatePageNext(int pageCount)
		{
			if (this._optionFilter.PageNumber < pageCount)
			{
				this._htmlPagingResult.Append("<a class=\"next\" href=\"javascript:" + this._pagingFunctionJS + "(" + (this._optionFilter.PageNumber + 1)
				                        + this._sequenceAdditionalPagingFunctionJs + ");\">Next</a>");
			}
		}

		private void CreatePagesWhenNumberPageResultLesserThanDefault(int pageCount)
		{
			for (var j = 0; j < pageCount; j++)
			{
				if (this._optionFilter.PageNumber == j + 1)
				{
					this._htmlPagingResult.Append("<a class=\"" + this._activeClass + "\" href=\"javascript:" + this._pagingFunctionJS + "(" + (j + 1) 
					                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + (j + 1) + "</a>");
				}
				else
				{
					this._htmlPagingResult.Append("<a href=\"javascript:" + this._pagingFunctionJS + "(" + (j + 1)
					                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + (j + 1) + "</a>");
				}
			}
		}

		private void CreatePagesWhenNumberPageResultGreaterThanDefault(int pageCount)
		{
			if (this._optionFilter.PageNumber > 3)
			{
				this._htmlPagingResult.Append("<a href=\"javascript:" + this._pagingFunctionJS + "(" + 1
				                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + 1 + "</a>");
				this._htmlPagingResult.Append("<a class=\"dot\" href=\"javascript:;\">...</a>");
			}

			string cl;
			int t;
			var pagePreview = 0;
			var soTrangVeLui = 2;

			if (pageCount - this._optionFilter.PageNumber == 1)
			{
				soTrangVeLui = soTrangVeLui + 1;
			}
			else if (pageCount - this._optionFilter.PageNumber == 0)
			{
				soTrangVeLui = soTrangVeLui + 2;
			}

			for (t = this._optionFilter.PageNumber - soTrangVeLui; t <= this._optionFilter.PageNumber; t++)
			{
				if (t < 1) continue;
				cl = t == this._optionFilter.PageNumber ? this._activeClass : string.Empty;
				this._htmlPagingResult.Append("<a class=\"" + cl + "\" href=\"javascript:" + this._pagingFunctionJS + "(" + t 
				                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + t + "</a>");
				pagePreview++;
			}

			if (this._optionFilter.PageNumber == 1)
			{
				for (t = this._optionFilter.PageNumber + 1; t < this._optionFilter.PageNumber + 5; t++)
				{
					if (t >= t + 5 || t > pageCount) continue;
					cl = t == this._optionFilter.PageNumber ? this._activeClass : string.Empty;
					this._htmlPagingResult.Append("<a class=\"" + cl + "\" href=\"javascript:" + this._pagingFunctionJS + "(" + t 
					                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + t + "</a>");
				}
			}
			else if (this._optionFilter.PageNumber > 1)
			{
				var incr = 5 - (pagePreview - 1);
				for (t = this._optionFilter.PageNumber + 1; t < this._optionFilter.PageNumber + incr; t++)
				{
					if (t >= t + incr || t > pageCount) continue;
					cl = t == this._optionFilter.PageNumber ? this._activeClass : string.Empty;
					this._htmlPagingResult.Append("<a class=\"" + cl + "\" href=\"javascript:" + this._pagingFunctionJS + "(" + t 
					                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + t + "</a>");
				}
			}

			if (pageCount - this._optionFilter.PageNumber > 2)
			{
				this._htmlPagingResult.Append("<a class=\"dot\" href=\"javascript:;\">...</a>");
				this._htmlPagingResult.Append("<a href=\"javascript:" + this._pagingFunctionJS + "(" + pageCount 
				                        + this._sequenceAdditionalPagingFunctionJs + ");\">" + pageCount + "</a>");
			}
		}

		public string Paging()
		{
			try
			{
				// No paging with EndAt = 0
				if (this._optionFilter.EndAt == 0) return Null.NullString;
				var pageCount = (int)Math.Ceiling(Convert.ToDouble(this._totalRecord) / this._optionFilter.RecordPerPage);

				this._htmlPagingResult.Append("<div class=\"d-paging-left\">");

				this._htmlPagingResult.Append("<label>Hiển thị " + this._optionFilter.StartAt + " - "
										+ (this._optionFilter.EndAt < this._totalRecord ? this._optionFilter.EndAt : this._totalRecord) + " </label>");
				this._htmlPagingResult.Append("<label> trên tổng số <b>" + this._totalRecord + "</b> " + this._itemName + "</label></div>");
				CreateCboOptionRecordOnPage();

				if (pageCount <= 1) return this._htmlPagingResult.ToString();
				this._htmlPagingResult.Append("<div class='paging-page-number'>");
				this.CreatePagePrev();

				if (pageCount <= DefaultNumberPageDisplay)
				{
					this.CreatePagesWhenNumberPageResultLesserThanDefault(pageCount);
				}
				else
				{
					this.CreatePagesWhenNumberPageResultGreaterThanDefault(pageCount);
				}

				this.CreatePageNext(pageCount);
				this._htmlPagingResult.Append("</div>");

				return this._htmlPagingResult.ToString();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
				return Null.NullString;
			}
		}

		public string PagingMobile()
		{
			try
			{
				// No paging with recordOnPage = 0
				if (this._recordOnPage == 0) return Null.NullString;
				var pageCount = (int)Math.Ceiling(Convert.ToDouble(this._totalRecord) / this._recordOnPage);
				this._curentPage = this._curentPage > 0 ? this._curentPage : 1;

				this._htmlPagingResult.Append("<div class=\"d-paging-left\"><label>Trang " + this._curentPage + "/" + pageCount + "</label></div>");
				this._htmlPagingResult.Append("<div class=\"d_number_of_page\" id=\"d_number_of_page\">");
				if (pageCount > 1)
				{
					// -- Prev
					if (this._curentPage > 1)
					{
						this._htmlPagingResult.Append("<a class=\"prev\" href=\"javascript:" + this._pagingFunctionJS + "(" + (this._curentPage - 1) + ");\">Prev</a>");
					}

					// -- Next
					if (this._curentPage < pageCount)
					{
						this._htmlPagingResult.Append("<a class=\"next\" href=\"javascript:" + this._pagingFunctionJS + "(" + (this._curentPage + 1) + ");\">Next</a>");
					}
				}

				this._htmlPagingResult.Append("</div>");
				return this._htmlPagingResult.ToString();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
				return Null.NullString;
			}
		}

		public string PagingShort()
		{
			try
			{
				// No paging with EndAt = 0
				if (this._optionFilter.EndAt == 0) return Null.NullString;
				var pageCount = (int)Math.Ceiling(Convert.ToDouble(this._totalRecord) / this._optionFilter.RecordPerPage);

				if (pageCount > 1)
				{
					this._htmlPagingResult.Append("<div class='paging-page-number'>");
					CreatePagePrev();

					if (pageCount <= DefaultNumberPageDisplay)
					{
						CreatePagesWhenNumberPageResultLesserThanDefault(pageCount);
					}
					else
					{
						CreatePagesWhenNumberPageResultGreaterThanDefault(pageCount);
					}

					CreatePageNext(pageCount);
					this._htmlPagingResult.Append("</div>");
				}

				return this._htmlPagingResult.ToString();
			}
			catch (Exception ex)
			{
				Logger.LogException(ex);
				return Null.NullString;
			}
		}

		public void FilterArrSequenceAdditionalPagingFunctionJs(string arrSequenceAdditionalPagingFunctionJs)
		{
			if (string.IsNullOrEmpty(arrSequenceAdditionalPagingFunctionJs))
			{
				this._sequenceAdditionalPagingFunctionJs = string.Empty;
			}
			else
			{
				var sequenceOptions = arrSequenceAdditionalPagingFunctionJs.Split('|');
				this._sequenceAdditionalPagingFunctionJs = sequenceOptions.Aggregate(string.Empty, (current, option) => current + ("," + option));
			}
		}
	}
}
