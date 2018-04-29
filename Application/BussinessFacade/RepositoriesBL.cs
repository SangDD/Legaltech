namespace BussinessFacade
{
    using CommonData;
    using ObjectInfo ;
    using System.Collections.Generic;

    public class RepositoriesBL
	{
		protected int    _totalRecordFindResult;
		protected string _pagingHtml;

		public KnMessage MesageCode { get; set; }
        public List<FunctionInfo> Lst_AllFunc {get;set;}
		public int TotalRecordFindResult
		{
			get
			{
				return this._totalRecordFindResult;
			}

			set
			{
				this._totalRecordFindResult = value;
			}
		}

		public string GetPagingHtml()
		{
			return this._pagingHtml;
		}
	}
}
