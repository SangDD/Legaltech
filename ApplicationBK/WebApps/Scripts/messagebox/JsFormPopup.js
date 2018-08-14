$(function (e) {
    if (e.keyCode == 27) {
       // document.getElementsByClassName('divWrapperPopup').style.display = "none";
    }

    //Cho phép kéo thả form popup 
    //if ($(".divPopup2").length > 0) {
    //    $(".divPopup2").draggable();
    //}
    ////cho phép resize form 
    //if ($(".divPopup2").length > 0) {

    //    $(".divPopup2").resizable();
    //}


    ////Cho phép kéo thả form popup 
    //$(".divPopup").draggable();
    ////cho phép resize form 
    //$(".divPopup").resizable();
});


function CloseDivPopUp(divWrapperPopup) {
    document.getElementById(divWrapperPopup).style.display = "none";

    $(".divPopupHeader").remove();
    if ($(".divPopup").length > 0) {
        $(".divPopup").css({ "height": "auto", "width": "auto" });
    }
    $("body").removeClass("hide-scroll");
}

function CloseDivPopUp2(divWrapperPopup) {
    document.getElementById(divWrapperPopup).style.display = "none";
    $(".divPopupHeader2").remove();
    if ($(".divPopup2").length > 0) {
        $(".divPopup2").css({ "height": "auto", "width": "auto" });
    }
    $("body").removeClass("hide-scroll");
}

//không truyền Width với Height vào form sẽ tự giãn theo độ rộng của div popup
//divWrapperPopup:Id div cần popup 
//Title: Tiêu đề  
function ShowPopupDialog_Ipace(divWrapperPopup, Title) {
    $("#divWrapperPopup").focus();
    if ($("#btnExitPopups").length == 0) {
        $(".divPopup").prepend("<div class=divPopupHeader><b class=divTitlePopups>" + Title + "</b><a id=btnExitPopups href=javascript:; onclick=CloseDivPopUp('" + divWrapperPopup + "')><img src=/Content/Version2/icon/close.png style=float:right /></a></div>");
    }
    document.getElementById(divWrapperPopup).style.display = "block";

    CssPopUpTag(divWrapperPopup, ".divPopup");
    $("body").addClass("hide-scroll");
    return true;
}


function ShowPopupDialog_Ipace(divWrapperPopup, Title, txtIdFocus) {
    $("#divWrapperPopup").focus();
    if ($("#btnExitPopups").length == 0) {
        $(".divPopup").prepend("<div class=divPopupHeader><b class=divTitlePopups>" + Title + "</b><a id=btnExitPopups href=javascript:; onclick=CloseDivPopUp('" + divWrapperPopup + "')><img src=/Content/Version2/icon/close.png style=float:right /></a></div>");
    }
    document.getElementById(divWrapperPopup).style.display = "block";

    CssPopUpTag(divWrapperPopup, ".divPopup");

    var idFocus = "#" + txtIdFocus;
    $(idFocus).focus();
    $("body").addClass("hide-scroll");
    return true;
}

//divWrapperPopup:Id div cần popup 
//Title: Tiêu đề  
//pWidth:chiều rộng form popup
//pHeight: chiều cao form popup
function ShowPopupDialog_Ipace(divWrapperPopup, Title, pWidth, pHeight) {

    var _heigt = pHeight + "px";
    if (pHeight == 0) {
        _heigt = "auto";
    }
    var _width = pWidth + "px";
    if ($("#btnExitPopups").length == 0) {
        $(".divPopup").prepend("<div class=divPopupHeader ><b class=divTitlePopups>" + Title + "</b><a id=btnExitPopups href=javascript:; onclick=CloseDivPopUp('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
        $(".divPopup").css({ "height": _heigt, "width": _width });
    }
    document.getElementById(divWrapperPopup).style.display = "block";

    CssPopUpTag(divWrapperPopup, ".divPopup");

    $("body").addClass("hide-scroll");
    return true;
}


function ShowPopupDialog_Ipace(divWrapperPopup, Title, pWidth, pHeight, txtIdFocus) {
    var _heigt = pHeight + "px";
    if (pHeight == 0) {
        _heigt = "auto";
    }
    var _width = pWidth + "px";
    if ($("#btnExitPopups").length == 0) {
        $(".divPopup").prepend("<div class=divPopupHeader ><b class=divTitlePopups>" + Title + "</b><a id=btnExitPopups href=javascript:; onclick=CloseDivPopUp('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
        $(".divPopup").css({ "height": _heigt, "width": _width });
    }
    document.getElementById(divWrapperPopup).style.display = "block";
    var idFocus = "#" + txtIdFocus;
    $(idFocus).focus().val($(idFocus).val());

    CssPopUpTag(divWrapperPopup, ".divPopup");
    $("body").addClass("hide-scroll");
    return true;
}



//tạm thời rem vào chưa dùng 
function CloseMessageBox() {
    //neu co 2 thang thi xoa thang 2 truoc 
    if ($('#msgboxOverlay2').length > 0) {
        document.getElementById('msgboxOverlay2').style.display = "none";
        $(".msgboxOverlay2").remove();
        return ;
    }
    if ($('#msgboxOverlay').length > 0) {
        document.getElementById('msgboxOverlay').style.display = "none";
        $(".msgboxOverlay").remove();
        return;
    }


}

function MessageBox(Title, MsgThongbao) {
    var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);" style="padding: 5px;"  onclick=CloseMessageBox() class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MsgThongbao + '</div> </div></div></div></div>';
    $('body').prepend(html);
    document.getElementById('msgboxOverlay').style.display = "block";
    return false;
}

//Title: Tiêu đề
//MessageConfirm Nội dung msg 
//YesCallBack Đồng ý thì làm gì
//no thì làm gì 
function jConfirm(MessageConfirm, YesCallBack, NoCallBack) {
    try {
        var yes = function () {
            //alert("Chưa có sự kiện xử lý confirm-yes!");
        };
        var no = function () {
            //alert("Chưa có sự kiện xử lý confirm-no!");
        };
        if (typeof (YesCallBack) === "function") {
            yes = YesCallBack;
        }
        if (typeof (NoCallBack) === "function") {
            no = NoCallBack;
        }
        window.jQuery.prototype.gyes = function () {
            yes();
            CloseMessageBox();
        };
        window.jQuery.prototype.gno = function () {
            no();
            CloseMessageBox();
        };
        var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);"  onclick="CloseMessageBox()" class="msgclose">Close</a><span>' + 'Thông báo' + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup" onclick="$(this).gyes()">Đồng ý</button><button id="btnHuyPopup" onclick="$(this).gno()">Hủy bỏ</button></div></div></div></div></div>';
        $('body').prepend(html);
        $("#btnSubmitPopup").focus();
        document.getElementById('msgboxOverlay').style.display = "block";

    } catch (e) {
        console.log(e.message);
    }

}

function MessageBox(Title, MsgThongbao, pTagFocus) {
    var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);"  onclick=CloseMessageBox() class="msgclose">Close</a><span>' + Title + '</span></div><div class="msg">' + MsgThongbao + '</div> </div></div></div></div>';
    $('body').prepend(html);
    document.getElementById('msgboxOverlay').style.display = "block";
    var idFocus = "#" + pTagFocus;
    $(idFocus).focus().val($(idFocus).val());
}


function FunctionReturn(pRturn) {
    document.getElementById('msgboxOverlay').style.display = "none";
    $(".msgboxOverlay").remove();
    if (pRturn == 0) {
        alert(0);
    } else {
        alert(1);
    }
    CloseMessageBox();
}


/*
    Purpose: MessageBoxConfirm - Hàm hiển thị popup confirm người dùng
    Parameter: Title: Tiêu đề box confirm
               MessageConfirm: Nội dung cần người dùng confirm?
               YesCallBack: Hàm xử lý khi người dùng nhấn đồng ý -- hàm này nếu không xử dụng thì truyền vào là null
               NoCallBack: Hàm xử lý khi người dùng nhấn không đồng ý -- hàm này nếu không xử dụng thì truyền vào là null
*/
//cach dung 
//MessageBoxConfirm("Cảnh báo", "Bạn có chắc chắn muốn xóa chỉ số này không?", function () {
//    var TagRemove = "#" + pIdtag.trim();
//    $(TagRemove).remove();
//}, function () {
//    return false;
//});
 

function jAlert(Title, MessageConfirm,pTagFocus) {
    var yes = function () {
        //alert("Chưa có sự kiện xử lý confirm-yes!");
    };
    var no = function () {
        //alert("Chưa có sự kiện xử lý confirm-no!");
    };
    if (typeof (YesCallBack) === "function") {
        yes = YesCallBack;
    }
    if (typeof (NoCallBack) === "function") {
        no = NoCallBack;
    }
    window.jQuery.prototype.gyes = function () {
        FocusToEnd(pTagFocus);
        yes();
        CloseMessageBox();
    };
    window.jQuery.prototype.gno = function () {
        FocusToEnd(pTagFocus);
        no();
        CloseMessageBox();
    };
    var html = '<div class="msgboxOverlay2" id =msgboxOverlay2><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);" style="padding: 5px;"  onclick="CloseMessageBox()" class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup2" onclick="$(this).gyes()">Thoát</button></div></div></div></div></div>';
    $('body').prepend(html);
    $("#btnSubmitPopup2").focus();
    document.getElementById('msgboxOverlay2').style.display = "block";

    FocusToEnd(pTagFocus);

}


function jAlert(Title, MessageConfirm, YesCallBack, NoCallBack, pTagFocus) {
    var yes = function () {
        //alert("Chưa có sự kiện xử lý confirm-yes!");
    };
    var no = function () {
        //alert("Chưa có sự kiện xử lý confirm-no!");
    };
    if (typeof (YesCallBack) === "function") {
        yes = YesCallBack;
    }
    if (typeof (NoCallBack) === "function") {
        no = NoCallBack;
    }
    window.jQuery.prototype.gyes = function () {
        yes();
        CloseMessageBox();
    };
    window.jQuery.prototype.gno = function () {
        no();
        CloseMessageBox();
    };
    var html = '<div class="msgboxOverlay2" id =msgboxOverlay2><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);" style="padding: 5px;"  onclick="CloseMessageBox()" class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup2" onclick="$(this).gyes()">Thoát</button></div></div></div></div></div>';
    $('body').prepend(html);
    $("#btnSubmitPopup2").focus();
    document.getElementById('msgboxOverlay2').style.display = "block";
 
}
 

function MessageBoxConfirm(Title, MessageConfirm, YesCallBack, NoCallBack) {
    var yes = function () {
        //alert("Chưa có sự kiện xử lý confirm-yes!");
    };
    var no = function () {
        //alert("Chưa có sự kiện xử lý confirm-no!");
    };
    if (typeof (YesCallBack) === "function") {
        yes = YesCallBack;
    }
    if (typeof (NoCallBack) === "function") {
        no = NoCallBack;
    }
    window.jQuery.prototype.gyes = function () {
        yes();
        CloseMessageBox();
    };
    window.jQuery.prototype.gno = function () {
        no();
        CloseMessageBox();
    };
    var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);" style="padding: 5px;"  onclick="CloseMessageBox()" class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup" onclick="$(this).gyes()">Đồng ý</button><button id="btnHuyPopup" onclick="$(this).gno()">Hủy bỏ</button></div></div></div></div></div>';
    $('body').prepend(html);
    $("#btnSubmitPopup").focus();
    document.getElementById('msgboxOverlay').style.display = "block";
}

function MessageBoxConfirm(Title, MessageConfirm, YesCallBack, NoCallBack, Finish) {
    var yes = function () {
        //alert("Chưa có sự kiện xử lý confirm-yes!");
    };
    var no = function () {
        //alert("Chưa có sự kiện xử lý confirm-no!");
    };
    var finish = function () {

    };
    if (typeof (YesCallBack) === "function") {
        yes = YesCallBack;
    }
    if (typeof (NoCallBack) === "function") {
        no = NoCallBack;
    }
    if (typeof (Finish) === "function") {
        finish = Finish;
    }
    window.jQuery.prototype.gyes = function () {
        yes();
        CloseMessageBox();
        finish();
    };
    window.jQuery.prototype.gno = function () {
        no();
        CloseMessageBox();
    };
    var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);" style="padding: 5px;"  onclick="CloseMessageBox()" class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup" onclick="$(this).gyes()">Đồng ý</button><button id="btnHuyPopup" onclick="$(this).gno()">Hủy bỏ</button></div></div></div></div></div>';
    $('body').prepend(html);
    $("#btnSubmitPopup").focus();
    document.getElementById('msgboxOverlay').style.display = "block";
}
//sửa để truyển vào confirm là có hoặc không thay vì đồng ý và hủy bỏ
function MessageBoxConfirm_2(Title, MessageConfirm, YesText, CancelText, YesCallBack, NoCallBack ) {
    var yes = function () {
        //alert("Chưa có sự kiện xử lý confirm-yes!");
    };
    var no = function () {
        //alert("Chưa có sự kiện xử lý confirm-no!");
    };
    if (typeof (YesCallBack) === "function") {
        yes = YesCallBack;
    }
    if (typeof (NoCallBack) === "function") {
        no = NoCallBack;
    }
    window.jQuery.prototype.gyes = function () {
        yes();
        CloseMessageBox();
    };
    window.jQuery.prototype.gno = function () {
        no();
        CloseMessageBox();
    };
    var html = '<div class="msgboxOverlay" id =msgboxOverlay><div class="alert-box msgwrapper"><div class="information"><div class="msgcontainer"><div class="msgheader"><a href="javascript:void(0);"  style="padding: 5px;" onclick="CloseMessageBox()" class="msgclose">Close</a><span style="padding: 5px;">' + Title + '</span></div><div class="msg">' + MessageConfirm + '</div> <div class="msgfooter"><button id="btnSubmitPopup" onclick="$(this).gyes()">' + YesText + '</button><button id="btnHuyPopup" onclick="$(this).gno()">' + CancelText + '</button></div></div></div></div></div>';
    $('body').prepend(html);
    $("#btnSubmitPopup").focus();
    document.getElementById('msgboxOverlay').style.display = "block";


}


function ShowPopupDialog_Ipace2(divWrapperPopup, Title) {
    $("#divWrapperPopup2").focus();
    if ($("#btnExitPopups2").length == 0) {
        $(".divPopup2").prepend("<div class=divPopupHeader2><b class=divTitlePopups2>" + Title + "</b><a id=btnExitPopups2 href=javascript:; onclick=CloseDivPopUp2('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
    }
    document.getElementById(divWrapperPopup).style.display = "block";

    CssPopUpTag(divWrapperPopup, ".divPopup2");
    $("body").addClass("hide-scroll");
    return true;
}


function ShowPopupDialog_Ipace2(divWrapperPopup, Title, txtIdFocus) {
    $("#divWrapperPopup2").focus();
    if ($("#btnExitPopups2").length == 0) {
        $(".divPopup2").prepend("<div class=divPopupHeader2><b class=divTitlePopups2>" + Title + "</b><a id=btnExitPopups2 href=javascript:; onclick=CloseDivPopUp2('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
    }
    document.getElementById(divWrapperPopup).style.display = "block";
    CssPopUpTag(divWrapperPopup, ".divPopup2");

    var idFocus = "#" + txtIdFocus;
    $(idFocus).focus().val($(idFocus).val());
    $("body").addClass("hide-scroll");
    return true;
}


function ShowPopupDialog_Ipace2(divWrapperPopup, Title, pWidth, pHeight) {
    var _heigt = pHeight + "px";
    if (pHeight == 0) {
        _heigt = "auto";
    }
    var _width = pWidth + "px";
    if ($("#btnExitPopups2").length == 0) {
        $(".divPopup2").prepend("<div class=divPopupHeader2 ><b class=divTitlePopups2>" + Title + "</b><a id=btnExitPopups2 href=javascript:; onclick=CloseDivPopUp2('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
        $(".divPopup2").css({ "height": _heigt, "width": _width });
    }
    document.getElementById(divWrapperPopup).style.display = "block";
    CssPopUpTag(divWrapperPopup, ".divPopup2");
    $("body").addClass("hide-scroll");
    return true;
}


function ShowPopupDialog_Ipace2(divWrapperPopup, Title, pWidth, pHeight, txtIdFocus) {
    var _heigt = pHeight + "px";
    if (pHeight == 0) {
        _heigt = "auto";
    }
    var _width = pWidth + "px";
    if ($("#btnExitPopups2").length == 0) {
        $(".divPopup2").prepend("<div class=divPopupHeader2 ><b class=divTitlePopups2>" + Title + "</b><a id=btnExitPopups2 href=javascript:; onclick=CloseDivPopUp2('" + divWrapperPopup + "')><img src=/Content/icons/close1.png title=Close style=float:right;margin:10px /></a></div>");
        $(".divPopup2").css({ "height": _heigt, "width": _width });
    }
    document.getElementById(divWrapperPopup).style.display = "block";
    CssPopUpTag(divWrapperPopup, ".divPopup2");

    var idFocus = "#" + txtIdFocus;
    $(idFocus).focus().val($(idFocus).val());
    $("body").addClass("hide-scroll");
    return true;
}

// cho cái popup về chính giữa màn hình
function CssPopUpTag(divContains, tag) {

    var _screen_h = screen.height;
    var _windown_h = $(window).height() - 50;
    var _height = $("#" + divContains).find(tag).height();
    var top = (_windown_h - _height) / 2;

    $("#" + divContains).css('margin-top', '0px');
    $("#" + divContains).find(tag).css('margin-top', top + 'px');
}


// HUNGTD: khi ấn ESC thì thoát form
$(document).keyup(function (e) {

    if (e.keyCode == 27)
    {
        var _have_popup_2 = 0;// kiểm tra xem có thằng div popup divWrapperPopup2 đang hiện hay ko          
        $('.divWrapperPopup2').each(function ()
        { 
            var _display = $("#" + this.id).css("display");
            if (_display != "none")// nếu có 1 thằng pop con đang hiện thì tý nữa sẽ ko cho tắt thằng cha đi
            {
                _have_popup_2 = 1;
            }
            CloseDivPopUp2(this.id)
        });
        if (_have_popup_2 ==0 )
        {
            $('.divWrapperPopup').each(function () {
            CloseDivPopUp(this.id)
        });
        }
    }
});
//Hunglm. nhấn phím mũi tên trái phải để chọn ĐỒNG Ý hoặc HỦY. ESC để hủy jAlert
//$(document).keyup(function (e) {
//    if (e.keyCode == 37) {
      
     
//        $('.msgboxOverlay #btnSubmitPopup').focus();
//    }
//    if (e.keyCode == 39) {
//        $('.msgboxOverlay #btnHuyPopup').focus();
//    }
//    if (e.keyCode == 27) {
//        $('.msgboxOverlay2 #btnSubmitPopup2').click();
//        $('.msgboxOverlay #btnHuyPopup').click();
//    }
//});