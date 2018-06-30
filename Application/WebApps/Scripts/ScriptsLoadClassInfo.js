var _timeoutcount = 500;//ms
// textbox search 

// Init a timeout variable to be used below
var timeout = null;

// Listen for keystroke events
//Nvs001txt_search_app_code.onkeyup = function (e) {

//    // Clear the timeout if it has already been set.
//    // This will prevent the previous task from executing
//    // if it has been less than <MILLISECONDS>
//    clearTimeout(timeout);

//    // Make a new timeout set to go off in 800ms
//    timeout = setTimeout(function () {
//       // alert('Input Value:' + Nvs001txt_search_app_code.value);
//        // tìm kiếm ở đoạn này
//        NVSFunc001ShowComboxSearch()
//    }, _timeoutcount);
//};

function SearchAppCodeOnKeyUp(_textboxid, _divid)
{
    try {
        // Clear the timeout if it has already been set.
        // This will prevent the previous task from executing
        // if it has been less than <MILLISECONDS>
        clearTimeout(timeout);

        // Make a new timeout set to go off in 800ms
        timeout = setTimeout(function () {
            // alert('Input Value:' + Nvs001txt_search_app_code.value);
            // tìm kiếm ở đoạn này
            NVSFunc001ShowComboxSearch(_textboxid, _divid)
        }, _timeoutcount);
    } catch (e) {
        alert(e.toString())
    }
   
}

function NVSFunc001ShowComboxSearch(_textboxid, _divid)
{
    try {
        var inputValue = $("#" + _textboxid).val();
        if (inputValue == "")
            return;
        $.ajax({
            type: "POST",
            url: "/quan-ly-thong-tin/hang-hoa-dich-vu/combobox-search",
            headers: { "cache-control": "no-cache" },
            data: { p_search: $("#" + _textboxid).val() },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (validateResponse(data)) {
                        $("#" + _divid).html(data);
                    }
                }
                return false;
            }
        });
    } catch (e) {
        alert(e.toString())
    }
}


function NVSFunc001ShowComboxSearchByGroup(_textboxid, _divid, _cboGroupid) {
    try {
        var inputValue = $("#" + _textboxid).val();
        if (inputValue == "")
            return;
        var cboGroupValue = $("#" + _cboGroupid).val();
        if (cboGroupValue == "")
            return;
        $.ajax({
            type: "POST",
            url: "/quan-ly-thong-tin/hang-hoa-dich-vu/combobox-search-by-group",
            headers: { "cache-control": "no-cache" },
            data: { p_search: $("#" + _textboxid).val(), p_groupcode: cboGroupValue },
            async: false,
            success: function (data) {
                if (data != null) {
                    if (validateResponse(data)) {
                        $("#" + _divid).html(data);
                    }
                }
                return false;
            }
        });
    } catch (e) {
        alert(e.toString())
    }
}

