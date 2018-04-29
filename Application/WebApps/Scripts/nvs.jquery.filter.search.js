// ---------------------------------------------     SESSION STORAGE     -----------------------------------------
function pageContainDataOfSearching() {
    return location.href.indexOf("_searching_") !== -1;
}

function initSessionStorage(callback) {
    if (sessionStorage.getItem(window.__seasionstorage_keySearch) === null) {
        sessionStorage.setItem(window.__seasionstorage_keySearch, window.keySearch);
        sessionStorage.setItem(window.__seasionstorage_optionFilter, optionFilter);
        if (pageContainDataOfSearching()) {
            window.searchingMatch = window.location.href.split("?")[1].split("&")[0].split("=");
            if (searchingMatch[1] === "y") {
                callback(1, 1);
            }
        }
    } else {
        if (pageContainDataOfSearching()) {
            window.searchingMatch = window.location.href.split("?")[1].split("&")[0].split("=");
            if (searchingMatch[1] === "y") {
                callback(-1, 0);
            } else {
                sessionStorage.setItem(window.__seasionstorage_keySearch, window.keySearch);
                sessionStorage.setItem(window.__seasionstorage_optionFilter, optionFilter);
            }
        } else {
            sessionStorage.setItem(window.__seasionstorage_keySearch, window.keySearch);
            sessionStorage.setItem(window.__seasionstorage_optionFilter, optionFilter);
        }
    }
}

function setOptionFilter() {
	window.colSort = $("#colSorted").val();
	window.sortType = $("#SortType").val();
	window.optionSorting = $("#OptionSorting").val();
}

function updateFilterOption() {
    window.optionFilter = window.colSort + "|" + window.sortType + "|" + window.optionSorting + "|" + window.pageNumber + "|" + window.recordPerPage;
    return optionFilter;
}

function getFilterOptionWhenExportDataToFile() {
	return window.colSort + "|" + window.sortType + "|" + window.optionSorting + "|" + 1 + "|" + 0;
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
	    var _currentOptionSorting = $(el).attr("data-sortoption");
		if (typeof _currentOptionSorting === "undefined" || _currentOptionSorting == null) {
			_currentOptionSorting = "";
		}
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