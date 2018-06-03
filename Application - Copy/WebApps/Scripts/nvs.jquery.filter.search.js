// ---------------------------------------------     SESSION STORAGE     -----------------------------------------
function initAllSearchingData(_currentKeySearch, _currentOptionFilter, initOrderBySearchingCallback, setKeySearchCallback, findDataCallback) {
	window.__seasionstorage_keySearch = _currentKeySearch;
	window.__seasionstorage_optionFilter = _currentOptionFilter;

	var isDataPostBack = false;
	if (sessionStorage.getItem(window.__seasionstorage_lastLocationPath) !== null) {
		if (sessionStorage.getItem(window.__seasionstorage_lastLocationPath) !== window.location.pathname
			&& sessionStorage.getItem(window.__seasionstorage_keySearch) !== null) {
			isDataPostBack = true;
		}
	}
	sessionStorage.setItem(window.__seasionstorage_lastLocationPath, window.location.pathname);
	if (isDataPostBack) {
		window.keySearch = sessionStorage.getItem(window.__seasionstorage_keySearch);
		window.optionFilter = sessionStorage.getItem(window.__seasionstorage_optionFilter);
		updateOptionFilterFromSessionStorage();
		updateOrderBySearching();
		findDataCallback(-1, 0);
	} else {
		initOrderBySearchingCallback();
		window.pageNumber = 1;
		window.recordPerPage = window.defaultRecordPerPage;
		updateFilterOption();
		setKeySearchCallback();
	}
	
	initSessionStorage();
	bindColumnSortWithSearching(findDataCallback);
	ChangeIConSortWhenSortColumns();
}

function pageContainDataOfSearching() {
    return location.href.indexOf("_searching_") !== -1;
}

function initSessionStorage() {
	if (sessionStorage.getItem(window.__seasionstorage_keySearch) === null) {
		sessionStorage.setItem(window.__seasionstorage_keySearch, window.keySearch);
		sessionStorage.setItem(window.__seasionstorage_optionFilter, optionFilter);
	}
}

function setOrderBySearching() {
	window.colSort = $("#colSorted").val();
	window.sortType = $("#SortType").val();
	window.optionSorting = validOptionSorting($("#OptionSorting").val());
}

function updateFilterOption() {
    window.optionFilter = window.colSort + "|" + window.sortType + "|" + validOptionSorting(window.optionSorting) + "|" + window.pageNumber + "|" + window.recordPerPage;
    return optionFilter;
}

function updateOrderBySearching() {
	 $("#colSorted").val(window.colSort);
	 $("#SortType").val(window.sortType);
	 $("#OptionSorting").val(validOptionSorting(window.optionSorting));
}

function getFilterOptionWhenExportDataToFile() {
	return window.colSort + "|" + window.sortType + "|" + validOptionSorting(window.optionSorting) + "|" + 1 + "|" + 0;
}

function enterKeyPress(e, elementHandle) {
    try {
        var code = e.keyCode || e.which;
        if (code === 13) {
            document.getElementById(elementHandle).click();
        }
    } catch (e) {
        console.log(e);
    }
}

function bindColumnSortWithSearching(callback) {
    $("th[data-sort]").on("click", function () {
        SortByCol(this);
        callback(1, 0);
    });
}

function SortByCol(el) {
    try {
	    var value = $(el).attr("id");
        var _currentColSorted = $("#colSorted").val();
        var _currentSortedType = $("#SortType").val();
	    var _currentOptionSorting = validOptionSorting($(el).attr("data-sortoption"));
		
        if (_currentColSorted === value) {
            if (_currentSortedType.toUpperCase() === "ASC") {
                $("#SortType").val("DESC");
            }
            else {
                $("#SortType").val("ASC");
            }
        }
        else {
            $("#colSorted").val(value);
            $("#SortType").val("ASC");
        }
	    $('#OptionSorting').val(_currentOptionSorting);
    } catch (e) {
        console.info(e.toString());
    }
}

function ChangeIConSortWhenSortColumns() {
    try {
        var _order_by = $("#colSorted").val();
        var _order_type = $("#SortType").val();
        var _colText = $("#" + _order_by).text().replace(" ▼", "").replace(" ▲", "");
        var _text = _order_type === "DESC" ? _colText + " ▼" : _colText + " ▲";
        $("#" + _order_by).text(_text);
    }
    catch (e) {
    }
}

function reinitSearchingConditions(_currentKeySearch, _currentOptionFilter, _pageNumber, _isSearching) {
	window.__seasionstorage_keySearch = _currentKeySearch;
	window.__seasionstorage_optionFilter = _currentOptionFilter;
	window.keySearch = sessionStorage.getItem(window.__seasionstorage_keySearch);
	window.optionFilter = sessionStorage.getItem(window.__seasionstorage_optionFilter);
	window.isSearching = _isSearching;
	window.pageNumber = _pageNumber;
	if (_isSearching === 1) {
		window.pageNumber = 1;
	}
	if (_pageNumber === -1) {
		updateOptionFilterFromSessionStorage();
	}
}

function updateOptionFilterFromSessionStorage() {
	window.arrOptionFilter = window.optionFilter.split('|');
	window.colSort = arrOptionFilter[0];
	window.sortType = arrOptionFilter[1];
	window.optionSorting = validOptionSorting(arrOptionFilter[2]);
	window.pageNumber = arrOptionFilter[3];
	window.recordPerPage = arrOptionFilter[4];
}

function validOptionSorting(_optionSorting) {
	if (typeof _optionSorting === "undefined" || _optionSorting == null) {
		_optionSorting = "";
	}
	return _optionSorting;
}

function updateSearchingConditions(idDivNumberRecordOnPage, setKeySearchCallback) {
	setOrderBySearching();
	window.recordPerPage = $("#" + idDivNumberRecordOnPage).val();
	setKeySearchCallback();
	sessionStorage.setItem(__seasionstorage_keySearch, keySearch);
	updateFilterOption();
	sessionStorage.setItem(__seasionstorage_optionFilter, optionFilter);
}