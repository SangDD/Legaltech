// ---------------------------------------    Loading    -----------------------------------------------
function waitForLoading(val) {
    if (val) {
        $("body").append(
            '<div class="loading-container"><div class="loading-opacity"></div><div class="circle-loading"></div><div class="circle-loading-inner"></div></div>');
    } else {
        $(".loading-container").remove();
    }
}

function getCurrentDate() {
    var currentdate = new Date();
    var datetime = currentdate.getDate() + "/" + (currentdate.getMonth() + 1) + "/" + currentdate.getFullYear() + " @ "
        + currentdate.getHours() + ":" + currentdate.getMinutes() + ":" + currentdate.getSeconds();
    return datetime;
}

function SpinLoading($create) {
    $create = $create || false;
    if ($create) {
        var _loader = '<div class="load-container"><div class="load-background"><div class="loader"></div><p>Loading...</p></div></div>';
        $("body").append(_loader);
    }
    else {
        $(".load-container").remove();
    }
}

function HourGlassProcessing($create) {
    $create = $create || false;
    if ($create) {
        var _loader = '<div class="load-container"><div class="load-background"><div class="loading"><div class="hourglass"><div class="hourglass-top"><div class="sand-top"></div></div><div class="hourglass-bottom"><div class="sand-bottom"></div></div></div></div><p>Processing...</p></div></div>';
        $("body").append(_loader);
    }
    else {
        $(".load-container").remove();
    }
}


// ---------------------------------------    Input    ----------------------------------------------
function limitMaxlength(el, maxlength) {
    if ($(el).val().length > maxlength) {
        $(el).val($(el).val().substring(0, maxlength));
    }
}

function limitValueInInputNumber(el, minValue) {
    var inputValue = $(el).val();
    if (inputValue !== "") {
        if (isNaN(inputValue) || inputValue / 1 < minValue || inputValue / 1 > 999999999) {
            inputValue = 1;
            $(el).val(inputValue);
        }
        var indexOfDot = (inputValue + "").indexOf(".");
        if (indexOfDot !== -1) {
            $(el).val(inputValue.substr(0, indexOfDot));
        }
    } else {
        $(el).val(inputValue);
    }
}

function reFillWithNotValidNumberInputData(el, defaultValue) {
    var inputValue = $(el).val();
    if (inputValue === "" || isNaN(inputValue) || inputValue / 1 < 0 || inputValue / 1 > 999999999) {
        $(el).val(defaultValue);
    }
}

function limitInputByOnlyNumber(_inputval, _control_id) {
    if (_inputval.indexOf(".") !== -1) {
        while (_inputval.indexOf(".") > -1) {
            _inputval = _inputval.replace(".", "");
        }
    }
    var _temp_input_val = "";
    var _childtemp_input_val = "";
    var _Regex = new RegExp("^[0-9,]+$");
    for (var i = 0; i < _inputval.length - 1; i++) {
        _childtemp_input_val += _inputval[i];
        if (_Regex.test(_childtemp_input_val)) {
            _temp_input_val = _childtemp_input_val;
        }
        else {
            $("input[id=" + _control_id + "]").val(_temp_input_val);
            return;
        }
    }
    if (!_Regex.test(_inputval)) {
        _inputval = _temp_input_val;
    }

    _inputval = _inputval.replace(/[,]/g, "");
    while (_inputval.indexOf(0) === "0" && _inputval.length > 1) {
        _inputval = _inputval.substring(1);
    }
    var ctrl = document.getElementById(_control_id);

    var before_length = ctrl.value.length;
    var key_index = doGetCaretPosition(ctrl);

    var x = _inputval.split(",");
    var x1 = x[0];
    var x2 = x.length > 1 ? x[1] : "";
    var result = x1 + x2;
    $("input[id=" + _control_id + "]").val(result);

    var after_length = document.getElementById(_control_id).value.length;
    setCaretPosition(ctrl, key_index + (parseInt(after_length) - parseInt(before_length)));
}

function inputPositiveInteger(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    var _value = String.fromCharCode(charCode);
    if (!isNaN(_value)) {
        return true;
    }
    evt.preventDefault();
    return false;
}


// ----------------------------------------   Image    -------------------------------------
function imgError(image) {
    image.onerror = "";
    image.src = "/Content/Image/logo.jpg";
    return true;
}

function imgErrorMobile() {
    jQuery("img").one('error', function () {
        jQuery(this).attr("src", "/Content/Image/logo.jpg");
    }).each(function () {
        if (this.complete && !this.naturalHeight && !this.naturalWidth) {
            $(this).triggerHandler('error');
        }
    });
}


// ---------------------------------------    Popup    --------------------------------------
function ShowPopupDialog(divWrapperPopup, Title, pWidth, pHeight, txtIdFocus) {
    pWidth = pWidth || 0;
    pHeight = pHeight || 0;
    txtIdFocus = txtIdFocus || "";

    var _height = pHeight + "px";
    if (pHeight === 0) {
        _height = "auto";
    }
    var _width = pWidth + "px";
    if (pWidth === 0) {
        _width = "700px";
    }
    else if (pWidth === -1) {
        _width = "100%";
    }

    if ($("#" + divWrapperPopup + " .d-popup-header").length > 0) {
        $("#" + divWrapperPopup + " .d-popup-header").remove();
    }
    $("#" + divWrapperPopup + " .d-popup").prepend('<div class="d-popup-header"><div class="popup-title">' + Title + '</div><a onclick="ClosePopupDialog(\'' + divWrapperPopup + '\',true)" class="btn-exit-popup fas fa-times"></button></div>');
    $("#" + divWrapperPopup + " .d-popup").css({ "height": _height, "width": _width, "min-width": _width });

    $("#" + divWrapperPopup).fadeIn(150).addClass('popup-flex');

    if (txtIdFocus !== "") {
        var idFocus = "#" + txtIdFocus;
        $(idFocus).focus().val($(idFocus).val());
    }
    else {
        $('#' + divWrapperPopup + ' .btn-exit-popup').focus();
    }
    $("body").addClass("hide-scroll");
    $(".d-container").addClass("hide-scroll-mobile");
    return true;
}

function ClosePopupDialog(divWrapperPopup, confirmClose) {
    confirmClose = confirmClose || false;
    if (confirmClose) {
        $("#" + divWrapperPopup).fadeOut(150);
        setTimeout(function () { $("#" + divWrapperPopup).removeClass('popup-flex'); }, 150);
        $("body").removeClass("hide-scroll");
    }
    else {
        $("#" + divWrapperPopup).fadeOut(150);
        setTimeout(function () { $("#" + divWrapperPopup).removeClass('popup-flex'); }, 150);
        $("body").removeClass("hide-scroll");
        $(".d-container").removeClass("hide-scroll-mobile");
    }
}

function CenterPopup(divWrapperPopup) {
    var _windown_h = $(window).height() - 50;
    var _height_ct = $("#" + divWrapperPopup + " .d-popup").height();
    var top = (_windown_h - _height_ct) / 2;
    $("#" + divWrapperPopup).css('margin-top', '0');
    $("#" + divWrapperPopup + " .d-popup").css('margin-top', top + 'px');
}

function ShowPopupDialog_0(divWrapperPopup, Title, pWidth, pHeight, txtIdFocus, contentRatio) {
    pWidth = pWidth || 0;
    pHeight = pHeight || 0;
    contentRatio = contentRatio || 100;
    txtIdFocus = txtIdFocus || "";

    var _height = pHeight + "px";
    if (pHeight === 0) {
        _height = "auto";
    }
    var _width = pWidth + "px";
    var _rightContentWidth = (pWidth * contentRatio / 100 - 40) + "px";
    var _leftContentWidth = (pWidth * (1 - contentRatio / 100)) + "px";
    if (pWidth === 0) {
        _width = "700px";
    }
    else if (pWidth === -1) {
        _width = "100%";
    }

    if ($("#" + divWrapperPopup + " .d-popup-header-0").length > 0) {
        $("#" + divWrapperPopup + " .d-popup-header-0").remove();
    }
    $("#" + divWrapperPopup + " .d-popup").prepend('<div class="div-left-content-popup" style="width:' + _leftContentWidth + '"></div>'
        + '<div class="d-popup-header-0" style="width:' + _rightContentWidth + '"><div class="popup-title">' + Title + '</div><a onclick="ClosePopupDialog(\'' + divWrapperPopup + '\',true)" class="btn-exit-popup fas fa-times"></button></div>');
    $("#" + divWrapperPopup + " .d-popup").css({ "height": _height, "width": _width, "min-width": _width });

    $("#" + divWrapperPopup).fadeIn(150).addClass('popup-flex');

    if (txtIdFocus !== "") {
        var idFocus = "#" + txtIdFocus;
        $(idFocus).focus().val($(idFocus).val());
    }
    else {
        $('#' + divWrapperPopup + ' .btn-exit-popup').focus();
    }
    $("body").addClass("hide-scroll");
    $(".d-container").addClass("hide-scroll-mobile");
    return true;
}



// ---------------------------------------    Other    ------------------------
$.fn.enterKey = function (fnc) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            ev = window.event || ev;
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if (keycode === '13') {
                fnc.call(this, ev);
            }
        });
    });
}

function toNumber(strNum, strDelemiter) {
    strDelemiter = strDelemiter || ',';
    return strNum.replace(eval('/' + strDelemiter + '/g'), '');
}

function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// fomat kiểu number có đấu , ở hàng nghìn và có cả phần thập phân
function jsFormatFloatNumber(el, lengthNumber, lengthFloat) {
    var nStr = $(el).val();
    var _IndexFloat = nStr.indexOf('.');
    var _PhanThapPhan = "";
    var _count_ = 0;
    var _Alltext = "";
    _count_ = (nStr.split(".").length - 1);
    if (_count_ > 1) {
        var Fst = nStr.indexOf(".");
        var Snd = nStr.indexOf(".", Fst + 1);
        nStr = nStr.substring(0, Snd);
    }
    if (_IndexFloat >= 0) {
        _PhanThapPhan = nStr.substring(_IndexFloat, nStr.length);
        if (_PhanThapPhan.length > lengthFloat + 1)
            _PhanThapPhan = _PhanThapPhan.substring(0, _PhanThapPhan.length - 1);
        nStr = nStr.substring(0, _IndexFloat);
    }

    var _PhanNguyen = "";
    _PhanNguyen = nStr;
    var _tempPhanNguyen = "";
    var _newtemp = "";
    var _Regex = new RegExp("^[0-9,.]+$");
    _Alltext = _PhanNguyen + _PhanThapPhan;
    for (var i = 0; i < _Alltext.length; i++) {// cat het nhung ky tu khong phai la so
        _newtemp += _Alltext[i];
        if (_Regex.test(_newtemp)) {
            _tempPhanNguyen = _newtemp;
        }
        else {
            $(el).val(_tempPhanNguyen);
            return;
        }
    }
    if (_Regex.test(_PhanNguyen))//neu la so thi lay
    {
        _PhanNguyen = nStr;
    }
    else {
        _PhanNguyen = _tempPhanNguyen;
    }
    nStr = _PhanNguyen;

    // cắt toàn bộ số 0 trước trường số
    nStr = nStr.replace(/[,]/g, "");
    while (nStr.indexOf(0) === "0" && nStr.length > 1) {
        nStr = nStr.substring(1);
    }

    var before_length = $(el).val().length; // lấy chiều dài trước khi thay đổi
    var key_index = doGetCaretPosition(el); // lấy vị trí con trỏ hiện tại

    var x = nStr.split(",");
    var x1 = x[0];
    var x2 = x.length > 1 ? "," + x[1] : "";
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + "," + "$2");
    }
    var result = x1 + x2;
    if (nStr.length > lengthNumber)
        result = result.substring(0, result.length - 1);

    $(el).val(result + _PhanThapPhan);

    var after_length = $(el).val().length; // lấy thay đổi chiều dài
    setCaretPosition(el, key_index + (parseInt(after_length) - parseInt(before_length))); // set vị trí con trỏ
}

function jsFormatNumberNoFloat(nStr, txtControlId) {
    if (nStr.indexOf(".") !== -1) {
        while (nStr.indexOf(".") > -1) {
            nStr = nStr.replace(".", "");
        }
    }
    var _PhanNguyen = nStr;
    var _tempPhanNguyen = "";
    var _newtemp = "";
    var _Regex = new RegExp("^[0-9,]+$");
    for (var i = 0; i < _PhanNguyen.length - 1; i++) {
        _newtemp += _PhanNguyen[i];
        if (_Regex.test(_newtemp)) {
            _tempPhanNguyen = _newtemp;
        }
        else {
            $("input[id=" + txtControlId + "]").val(_tempPhanNguyen);
            return;
        }
    }
    if (_Regex.test(_PhanNguyen)) {
        _PhanNguyen = nStr;
    }
    else {
        _PhanNguyen = _tempPhanNguyen;
    }
    nStr = _PhanNguyen;

    nStr = nStr.replace(/[,]/g, "");
    while (nStr.indexOf(0) === '0' && nStr.length > 1) {
        nStr = nStr.substring(1);
    }
    var ctrl = document.getElementById(txtControlId);

    var before_length = ctrl.value.length;
    var key_index = doGetCaretPosition(ctrl);

    var x = nStr.split(",");
    var x1 = x[0];
    var x2 = x.length > 1 ? "," + x[1] : "";
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, "$1" + "," + "$2");
    }
    var result = x1 + x2;
    $("input[id=" + txtControlId + "]").val(result);

    var after_length = document.getElementById(txtControlId).value.length;
    setCaretPosition(ctrl, key_index + (parseInt(after_length) - parseInt(before_length)));
}

// format number -> string voi thounsand separator la dau ',', khong co phan thap phan
function formatNumberToStringN1(_number) {
    try {
        if ($.type(_number).toLowerCase() === "number") {
            _number = _number.toFixed(0);
        }

        if (IsValidNumber(_number.replace(/,/g, ""), false) === true) {
            return _number.replace(/,/g, "").replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
        }
        else {
            return _number;
        }
    } catch (e) {
        return _number;
    }
}

function jsgetCurrentDate() {
    var _date = new Date();
    var twoDigitMonth = ((_date.getMonth() + 1) < 10) ? '0' + (_date.getMonth() + 1) : (_date.getMonth() + 1);
    var towDigitDate = (_date.getDate() >= 10) ? (_date.getDate()) : '0' + (_date.getDate());
    //lay ngay theo dinh dang dd/MM/yyyy
    var cur_date = towDigitDate + "/" + twoDigitMonth + "/" + _date.getFullYear();
    return cur_date;
}
//------lấy vị trí hiện tại con trỏ trong textbox----------
function doGetCaretPosition(ctrl) {
    var CaretPos = 0;   // IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart === '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}

//------sét vị trí con trỏ trong textbox----------
function setCaretPosition(ctrl, pos) {
    if (ctrl.setSelectionRange) {
        ctrl.focus();
        ctrl.setSelectionRange(pos, pos);
    }
    else if (ctrl.createTextRange) {
        var range = ctrl.createTextRange();
        range.collapse(true);
        range.moveEnd('character', pos);
        range.moveStart('character', pos);
        range.select();
    }
}

// Drag table


//pIdDataTable :ID cua table 
//sử dụng khi scroll ngang table khi cuộn chuột
var dd = null;
var delta = 0;
var ddc = true;

function MouseScrollOnTable(pIdDataTable) {
    dd = document.getElementById(pIdDataTable).parentElement;
    delta = 0;
    ddc = true;

    //FF doesn't recognize mousewheel as of FF 3.x
    var isFirefox = (/Firefox/i.test(navigator.userAgent)) ? true : false;
    var mousewheelevt = isFirefox ? "DOMMouseScroll" : "mousewheel";
    $(dd).on(mousewheelevt, function (event) {
        ddc = true;
        event.preventDefault();
        if (dd.addEventListener) {
            if (isFirefox)
                dd.addEventListener(mousewheelevt, MouseWheelHandler, false);
            else
                dd.addEventListener(mousewheelevt, MouseWheelHandler(event), false);
        }
        else {
            if (isFirefox)
                dd.attachEvent("on" + mousewheelevt, MouseWheelHandler);
            else
                dd.attachEvent("on" + mousewheelevt, MouseWheelHandler(event));
        }
    });

    function MouseWheelHandler(e) {
        while (ddc === true) {
            e = window.event || e;
            // FF doesn't recognize event.wheelDelta = -120. Only event.detail = 3
            var _wheelDelta = e.detail ? e.detail * 40 : -e.wheelDelta;

            delta = dd.scrollLeft + (_wheelDelta / 2);


            var this_height = document.getElementById(pIdDataTable).offsetHeight;
            if (this_height > dd.offsetHeight)
                delta = delta < 0 ? 0 : delta > dd.scrollWidth + 17 - dd.offsetWidth ? dd.scrollWidth + 17 - dd.offsetWidth : delta;
            else
                delta = delta < 0 ? 0 : delta > dd.scrollWidth - dd.offsetWidth ? dd.scrollWidth - dd.offsetWidth : delta;

            $(dd).scrollLeft(delta);
            ddc = false;
        }
    }
}

function initDateTimePicker() {
    $.datetimepicker.setLocale('vi');
    $('.datetimepicker').datetimepicker({
        timepicker: false,
        format: 'd/m/Y',
        //formatTime: 'H:i',
        formatDate: 'd/m/Y',
        //mask: '29:59 - 39/19/9999',
        mask: '39/19/9999',
        validateOnBlur: false
    });
}

function destroyDateTimePicker() {
	try {
		destroyDateTimePicker();
	}catch(err){}
}

function reInitDateTimePicker() {
	try {
		if ($('.xdsoft_datetimepicker').length > 0) {
			$('.xdsoft_datetimepicker').css('display', 'none');
		}
		if ($('.datetimepicker').length > 0) {
			initDateTimePicker();
		}
	}catch(err){}
}

function clearTextbox(el) {
	$(el).val("");
}