// alert with popup
function nvsAlert($title, $content, $fncallback) {
    try {
        $title = $title == undefined ? "Thông báo" : $title;
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
        $title = $title == undefined ? "Thông báo" : $title;
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
        $title = $title == undefined ? "Thành công" : $title;
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

function nvsWarning($title, $content, $fnokcallback, $fncancelcallback) {
    try {
        $title = $title == undefined ? "Cảnh báo" : $title;
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
        $title = $title == undefined ? "Lỗi" : $title;
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
            confirmButtonText: 'Chấp nhận',
            cancelButtonText: 'Hủy',
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
    _title = _title || "Thành công!";

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
    _title = _title || "Thông báo!";

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
    _title = _title || "Cảnh báo!";

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
    _title = _title || "Lỗi!";

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

function funcShowOrHiddenCD(ptag, ptgShow)
{
    $(".ms-drop").css('width', '100%');

    $("#" + ptag).removeAttr("hidden");
    $("#" + ptgShow).css('display', 'block');
    var check = "00";
    if (ptgShow == "divHiddenChuDon01") {
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