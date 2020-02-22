// alert with popup
var _thongbao = "Thông báo";
var _loi = "Lỗi";
var _thanhcong = "Thành công";
var _canhbao = "Cảnh báo";
var _chapnhap = "Chấp nhận";
var _huy = "Hủy";
var _loi1 = "Lỗi!";
var _thanhcong1 = "Thành công!";
var _thongbao1 = "Thông báo!";
var _canhbao1 = "Cảnh báo!";
if (_currentlanguage != undefined) {
    if (_currentlanguage == "EN_US") {
        _thongbao = "Message";
        _loi = "Error";
        _thanhcong = "Success";
        _canhbao = "Warning";
        _chapnhap = "Accept";
        _huy = "Abort";
        _loi1 = "Error!";
        _thanhcong1 = "Success!";
        _thongbao1 = "Message!";
        _canhbao1 = "Warning!";
    }
}
function nvsAlert($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? _thongbao : $title;
        swal({
            title: $title,
            text: $content
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
}

function nvsInfo($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? _thongbao : $title;
        swal({
            title: $title,
            text: $content,
            type: "info"
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
}

function nvsSuccess($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? _thanhcong : $title;
        swal({
            title: $title,
            text: $content,
            type: "success"
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
}

function nvsConfirm($title, $content, $fnokcallback, $fncancelcallback) {
    try {
        $title = $title == undefined ? "" : $title;
        swal({
            title: $title,
            text: $content,
            type: "question",
            showCancelButton: true
        }).then(function () {
            if (typeof $fnokcallback === "function") {
                $fnokcallback();
            }
        }, function () {
            if (typeof $fncancelcallback === "function") {
                $fncancelcallback();
            }
        });
    } catch (e) {
    }
}

function nvsConfirmResize($title, $content, $fnokcallback, $fncancelcallback) {
    try {
        //$title = $title == undefined ? "" : $title;
        //swal({
        //    type: "question",
        //    text: $content,
        //    html: ""  +$content+
        //        "<br>" +
        //       '<button type="button" role="button" tabindex="0" class="SwalBtn1 customSwalBtnOK" style="background-color: rgb(48, 133, 214); border-left-color: rgb(48, 133, 214); border-right-color: rgb(48, 133, 214);">' + 'OK' + '</button>' +
        //       '<button type="button" role="button" tabindex="0" class="SwalBtn1 customSwalBtnOK" style="background-color: rgb(48, 133, 214); border-left-color: rgb(48, 133, 214); border-right-color: rgb(48, 133, 214);">' + 'Resize' + '</button>' +
        //       '<button type="button" role="button" tabindex="0" class="SwalBtn2 customSwalBtnCancel" style="display: inline-block; background-color: rgb(170, 170, 170);">' + 'Cancel' + '</button>',
        //    showCancelButton: false,
        //    showConfirmButton: false
        //});

        var buttons = $('<div>')
           .append(createButton('Ok', function () {
               swal.close();
               console.log('ok');
           })).append(createButton('Later', function () {
               swal.close();
               console.log('Later');
           })).append(createButton('Cancel', function () {
               swal.close();
               console.log('Cancel');
           }));
        
        e.preventDefault();
        swal({
            title: "Are you sure?",
            html: buttons,
            type: "warning",
            showConfirmButton: false,
            showCancelButton: false
        });

    } catch (e) {
    }
}

function nvsWarning($title, $content, $fnokcallback, $fncancelcallback) {
    try {
        $title = $title == undefined ? _canhbao : $title;
        swal({
            title: $title,
            text: $content,
            type: "warning",
            showCancelButton: true
        }).then(function () {
            if (typeof $fnokcallback === "function") {
                $fnokcallback();
            }
        }, function () {
            if (typeof $fncancelcallback === "function") {
                $fncancelcallback();
            }
        });
    } catch (e) {
    }
}

function nvsError($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? _loi : $title;
        swal({
            title: $title,
            text: $content,
            type: "error"
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
}

function nvsAlertWithHtml($title, $html, $type, $fncallback) {
    try {
        swal({
            title: $title,
            html: $html,
            type: $type
        }).then(function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        }, function () {
            if (typeof $fncallback === "function") {
                $fncallback();
            }
        });
    } catch (e) {
    }
}

function nvsPrompt($title, $content, $type, $fnokcallback, $fnreject, $fncancelcallback, $maxlength) {
    try {
        $title = $title == undefined ? "" : $title;
        swal({
            title: $title,
            text: $content,
            input: $type,
            showCancelButton: true,
            //closeOnConfirm: false,
            animation: "slide-from-top",
            inputPlaceholder: "",
            confirmButtonText: _chapnhap,
            cancelButtonText: _huy,
            inputAttributes: { maxlength: $maxlength, id: 'txtPromt_p' },
            preConfirm: function (inputValue) {
                return new window.Promise(function (resolve, reject) {
                    if (typeof $fnreject === "function") {
                        $fnreject(inputValue, resolve, reject);
                    }
                });
            },
            allowOutsideClick: false

        }).then(function (inputValue) {
            if (typeof $fnokcallback === "function") {
                $fnokcallback(inputValue);
            }
        }, function () {
            if (typeof $fncancelcallback === "function") {
                $fncancelcallback();
            }
        });
    } catch (e) {
    }
}
// alert by animation
function showSuccess(_msg, _title, _options) {
    _title = _title || _thanhcong1;

    var defaults = {
        type: "success",
        mouse_over: "pause",
        placement: {
            from: "bottom",
            align: "right"
        }
    };
    _options = $.extend(true, {}, defaults, _options);

    $.notify({
        title: _title,
        message: _msg
    }, _options);
}

function showInfo(_msg, _title, _options) {
    _title = _title || _thongbao1;

    var defaults = {
        type: "info",
        mouse_over: "pause",
        placement: {
            from: "bottom",
            align: "right"
        }
    };
    _options = $.extend(true, {}, defaults, _options);

    $.notify({
        title: _title,
        message: _msg
    }, _options);
}

function showWarning(_msg, _title, _options) {
    _title = _title || _canhbao1;

    var defaults = {
        type: "warning",
        mouse_over: "pause",
        placement: {
            from: "bottom",
            align: "right"
        }
    };
    _options = $.extend(true, {}, defaults, _options);

    $.notify({
        title: _title,
        message: _msg
    }, _options);
}

function showError(_msg, _title, _options) {
    _title = _title || _loi1;

    var defaults = {
        type: "error",
        mouse_over: "pause",
        placement: {
            from: "bottom",
            align: "right"
        }
    };
    _options = $.extend(true, {}, defaults, _options);

    $.notify({
        title: _title,
        message: _msg
    }, _options);
}

function funcShowOrHidden(ptag) {

    if ($("#" + ptag).is(":hidden") == true) {
        $("#" + ptag).removeAttr("hidden");
    } else {
        $("#" + ptag).attr("hidden", "hidden");
    }
}

function funcHidden(ptag, ptgHiden, pAddNew) {
    $("#" + ptag).attr("hidden", "hidden");
    $("#" + ptgHiden).css('display', 'none');

    //An hien chu don khac 
    var allElems = document.getElementsByClassName('classChuDonKhac');
    var count = 0;
    for (var i = 0; i < allElems.length; i++) {
        var thisElem = allElems[i];
        if (thisElem.style.display == 'block') {
            count++;
        }
    }
    if (count == 0) {
        $("#divChungDonKhac001").css('display', 'none');
    }

    if (pAddNew == "01") {
        $("#divThemChuDon01").css('display', 'block');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon03").css('display', 'none');
        $("#divThemChuDon04").css('display', 'none');
    } else if (pAddNew == "02") {
        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'block');
        $("#divThemChuDon03").css('display', 'none');
        $("#divThemChuDon04").css('display', 'none');
    }
    else if (pAddNew == "03") {

        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon03").css('display', 'block');
        $("#divThemChuDon04").css('display', 'none');
    }
    else if (pAddNew == "04") {
        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon03").css('display', 'none');
        $("#divThemChuDon04").css('display', 'block');
    }
}

function funcShowOrHiddenCD(ptag, ptgShow) {
    $(".ms-drop").css('width', '100%');

    $("#" + ptag).removeAttr("hidden");
    $("#" + ptgShow).css('display', 'block');
    var check = "00";
    if (ptgShow == "divHiddenChuDon01") {

        $("#divChungDonKhac001").css('display', 'block');
        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon03").css('display', 'none');
        $("#divThemChuDon04").css('display', 'none');
        //Nếu 2
        if ($("#divAddNewChuDon02").is(":hidden") == true) {
            $("#divThemChuDon02").css('display', 'block');
        }
        else if ($("#divAddNewChuDon03").is(":hidden") == true) {
            $("#divThemChuDon03").css('display', 'block');
        }
        else if ($("#divAddNewChuDon04").is(":hidden") == true) {
            $("#divThemChuDon04").css('display', 'block');
        }

    } else if (ptgShow == "divHiddenChuDon02") {

        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon04").css('display', 'none');
        //Nếu 2
        if ($("#divAddNewChuDon03").is(":hidden") == true) {
            $("#divThemChuDon03").css('display', 'block');
        }
        else if ($("#divAddNewChuDon04").is(":hidden") == true) {
            $("#divThemChuDon04").css('display', 'block');
        }
        else if ($("#divAddNewChuDon01").is(":hidden") == true) {
            $("#divThemChuDon01").css('display', 'block');
        }
    }
    else if (ptgShow == "divHiddenChuDon03") {
        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon03").css('display', 'none');

        if ($("#divAddNewChuDon04").is(":hidden") == true) {
            $("#divThemChuDon04").css('display', 'block');
        }
        else if ($("#divAddNewChuDon01").is(":hidden") == true) {
            $("#divThemChuDon01").css('display', 'block');
        } else if ($("#divAddNewChuDon02").is(":hidden") == true) {
            $("#divThemChuDon02").css('display', 'block');
        }
    }
    else if (ptgShow == "divHiddenChuDon04") {
        $("#divThemChuDon04").css('display', 'none');
        $("#divThemChuDon01").css('display', 'none');
        $("#divThemChuDon02").css('display', 'none');
        $("#divThemChuDon03").css('display', 'none');

        if ($("#divAddNewChuDon01").is(":hidden") == true) {
            $("#divThemChuDon01").css('display', 'block');
        }
        else if ($("#divAddNewChuDon02").is(":hidden") == true) {
            $("#divThemChuDon02").css('display', 'block');
        }
        else if ($("#divAddNewChuDon03").is(":hidden") == true) {
            $("#divThemChuDon03").css('display', 'block');
        }
    }
}
