//function OpenPopup() {
//  var options = {
//tiêu đề của popup
// title: "Tiêu đề",
//nội dung của popup - cái này có thể truyền cả 1 partailview html vào
//nếu là modal popup thì chỉ cần truyền div element vào là được
// content: ".partialModal",
/*
Purpose:Xác định loại Popup -- xác định các thuộc tính của popup
Parameter:Truyền vào info,confirm hoặc modal
*/
//type: "info",
// width: 1000,
//height: 500,
// icon: "/jquerypopup/resource/about_icon.jpg",//hiện tại cái icon em chưa để defaul nên phải muốn
//hiển thị icon thì truyền đường dẫn vào đây
// confirm: {
//sự kiện confirm khi người dùng chọn yes
//  yes: {
//   value: "Đồng ý",//value button yes
//   event: function () {
//       alert("Bạn vừa chọn yes");
//  },//event xử lý sự kiện khi chọn yes
//},
//sự kiện confirm khi người dùng chọn no
//no: {
//    value: "Hủy bỏ",//value button no
//    event: function () {
//        alert("Bạn vừa chọn no");
//    },//event xử lý sự kiện khi chọn no
// },
// }
//};
//  jpopup.show(options);
//}
var IPopup = function (options) {
    /*
        Khai báo private variables dùng trong namespace popup
    */
    //body: sẽ append popupBoxOverlay vào nó
    var body = $('body');
    //element contains popup
    var popupBoxOverlay = $("<div id='popup-box-overlay'></div>");
    //element popupwrapper
    var popupWrapper = $("#popup-wrapper");
    //element popupheader
    var popupHeader = $("#popup-header");
    //index element Popup title
    var indexPopupTitle = 3;
    //index element Popup Icon
    var indexPopupIcon = 11;
    //index element PopupContent
    var indexPopupContent = 14;
    //index element PopupContent Modal
    var indexPopupContentModal = 11;
    //index value yes callback confirm
    var indexValueYesCallBackConfirm = 18;
    //index event yes callback confirm
    var indexEventYesCallBackConfirm = 20;
    //index value no callback confirm
    var indexValueNoCallBackConfirm = 23;
    //index event no callback confirm
    var indexEventNoCallBackConfirm = 25;
    //icon information
    var img_info = "/Content/Version2/icon/info-icon.png";
    //icon warning
    var img_warning = "/Content/Version2/icon/warning-icon.png";
    //incon error
    var img_error = "/Content/Version2/icon/error-icon.png";
    //Html Popup Info
    var htmlPopupInfo = [
        '<div id="popup-wrapper">',
            '<div id="popup-header">',
                '<div id="popup-title">', '', '</div>',
                '<div id="popup-close">',
                    '<a href="javascript:;" onclick="$(this).npopuphide()">X</a>',
                '</div>',
            '</div>',
            '<div id="popup-main-content">',
                '<div id="popup-icon">', '', '</div>',
                '<div id="popup-content">', '', '</div>',
                '<div id="popup-control">',
                    '<input type="button" value="Đóng" onclick="$(this).npopuphide()" />',
                '</div>',
            '</div>',
        '</div>'];
    //Html Popup Confirm
    var htmlPopupConfirm = [
       '<div id="popup-wrapper">',
            '<div id="popup-header">',
                '<div id="popup-title">', '', '</div>',
                '<div id="popup-close">',
                    '<a href="javascript:;" onclick="$(this).npopuphide()">X</a>',
                '</div>',
            '</div>',
            '<div id="popup-main-content">',
                '<div id="popup-icon">', '', '</div>',
                '<div id="popup-content">', '', '</div>',
                '<div id="popup-control">',
                    '<input type="button" value="', '', '" onclick="', '', '" />',
                    '<input type="button" value="', '', '" onclick="', '', '" />',
                '</div>',
            '</div>',
        '</div>'];
    //Html Popup Modal
    var htmlPopupModal = [
         '<div id="popup-wrapper">',
            '<div id="popup-header">',
                '<div id="popup-title">', '', '</div>',
                '<div id="popup-close">',
                    '<a href="javascript:;" onclick="$(this).npopuphide()">X</a>',
                '</div>',
            '</div>',
            '<div id="popup-main-content">',
                '<div id="popup-content">', '', '</div>',
            '</div>',
        '</div>'
    ];
    //default option
    var defaults = {
        //tiêu đề của popup
        title: "Message",
        //nội dung của popup - cái này có thể truyền cả 1 partailview html vào
        content: "Nội dung Popup",
        /*
        Purpose:Xác định loại Popup -- xác định các thuộc tính của popup
        Parameter:Truyền vào info,wanning or error
        */
        type: "info",
        width: 300,
        height: 150,
        icon: "",
        confirm: {
            //sự kiện confirm khi người dùng chọn yes
            yes: {
                value: "Yes",//value button yes
                event: null,//event xử lý sự kiện khi chọn yes
            },
            //sự kiện confirm khi người dùng chọn no
            no: {
                value: "No",//value button no
                event: null,//event xử lý sự kiện khi chọn no
            },
        }
    };
    /*
        Khai báo các biến public
    */
    var type = {
        info: "info",
        error: "error",
        warning: "warning",
        confirm: "confirm",
        modal: "modal"
    };
    /*
        Tạo form thông qua option người dùng truyền vào
    */
    var init = function () {
        //Extens giữa option do người dùng truyền vào và option default
        var popup_options = $.extend(defaults, options);
        var popup_type = popup_options.type;
        //append options title, message
        if (popup_type === type.info || popup_type === type.error || popup_type === type.warning) {
            //Khởi tạo popup infomation
            //Add optional vào mảng htmlPopupInfo
            htmlPopupInfo[indexPopupTitle] = popup_options.title;//set title
            htmlPopupInfo[indexPopupContent] = popup_options.content;//set content
            //set default icon cho popup - nếu người dùng không truyền icon khác vào
            if (popup_options.icon === "") {
                if (popup_type === type.info) popup_options.icon = img_info;
                else if (popup_type == type.error) popup_options.icon = img_error;
                else if (popup_type == type.warning) popup_options.icon = img_warning;
                else popup_options.icon = img_info;
            }
            htmlPopupInfo[indexPopupIcon] = '<img src="' + popup_options.icon + '" id="imgIconPopup" />';//set popup icon
            //Apend chuỗi html popup infomation vào div id PopupBoxOverlay
            var htmlPopup = htmlPopupInfo.join("");
            $(popupBoxOverlay).append(htmlPopup);
            //Append popupBoxOverlay vào body
            $(body).append(popupBoxOverlay);
            $(popupBoxOverlay).fadeIn(100);
            //tạo sự kiện dragable cho thẻ popup-wraper
            $("#popup-wrapper").draggable({
                cursor: "move",//control view khi draggable
                handle: "#popup-header",//xử lý sự kiện tại id này
                containment: "parent"// phạm vi draggable
            });
        } else if (popup_type === type.confirm) {
            //Khởi tạo popup confirm
            //Khởi tạo popup infomation
            //Add optional vào mảng htmlPopupInfo
            htmlPopupConfirm[indexPopupTitle] = popup_options.title;//set title
            htmlPopupConfirm[indexPopupContent] = popup_options.content;//set content
            htmlPopupConfirm[indexPopupIcon] = '<img src="' + popup_options.icon + '" id="imgIconPopup" />';//set popup icon
            htmlPopupConfirm[indexValueYesCallBackConfirm] = popup_options.confirm.yes.value;
            if (typeof (popup_options.confirm.yes.event) === 'function'
                || popup_options.confirm.yes.event === null) {
                //tạo 1 prototype function confirm yes cho thằng jquery
                window.jQuery.prototype.nconfirmyes = f_confirm_yes;
                //cái này có thể đổi thành jpopup.confirmyes như show và hide không cần đến public function của window nữa
                htmlPopupConfirm[indexEventYesCallBackConfirm] = "$(this).nconfirmyes()";
            }
            htmlPopupConfirm[indexValueNoCallBackConfirm] = popup_options.confirm.no.value;
            if (typeof (popup_options.confirm.no.event) === 'function'
                || popup_options.confirm.no.event === null) {
                //tạo 1 prototype function confirm no cho thằng jquery
                window.jQuery.prototype.nconfirmno = f_confirm_no;
                //cái này có thể đổi thành jpopup.confirmyes như show và hide không cần đến public function của window nữa
                htmlPopupConfirm[indexEventNoCallBackConfirm] = "$(this).nconfirmno();";
            }
            //Apend chuỗi html popup infomation vào div id PopupBoxOverlay
            var htmlPopup = htmlPopupConfirm.join("");
            $(popupBoxOverlay).append(htmlPopup);
            //Append popupBoxOverlay vào body
            $(body).append(popupBoxOverlay);
            $(popupBoxOverlay).fadeIn(100);
            //tạo sự kiện dragable cho thẻ popup-wraper
            $("#popup-wrapper").draggable({
                cursor: "move",//control view khi draggable
                handle: "#popup-header",//xử lý sự kiện tại id này
                containment: "parent"// phạm vi draggable
            });
        } else if (popup_type === type.modal) {
            //Khởi tạo popup modal
            htmlPopupModal[indexPopupTitle] = popup_options.title;//set title
            htmlPopupModal[indexPopupContentModal] = $(popup_options.content).html();//set content
            var htmlPopup = htmlPopupModal.join("");
            $(popupBoxOverlay).append(htmlPopup);
            //Append popupBoxOverlay vào body
            $(body).append(popupBoxOverlay);
            $(popupBoxOverlay).fadeIn(100);
            $("#popup-content").css({
                "width": "98%",
                "float": "none",
                "padding-left": "1%",
                "padding-right": "1%"
            });
            //tạo css cho popup
            var popup_width = popup_options.width;
            var popup_height = popup_options.height;
            if ((popup_width != 0 && popup_width != null) && (popup_height != 0 && popup_height != null)) {
                $("#popup-wrapper").css({
                    "width": popup_width,
                    "height": popup_height
                });
            }
            //tạo sự kiện dragable cho thẻ popup-wraper
            $("#popup-wrapper").draggable({
                cursor: "move",//control view khi draggable
                handle: "#popup-header",//xử lý sự kiện tại id này
                containment: "parent"// phạm vi draggable
            });
        }
        //tạo 1 prototype function confirm yes cho thằng jquery
        window.jQuery.prototype.npopuphide = popupclose;
        //hàm set vị trí của popup trên window
        setPosition();
    }
    /*
        <---Popup Info Event--->
    */
    //private function
    //function khi close popup
    var popupclose = function () {
        $("#popup-box-overlay").fadeOut(100, function () {
            $("#popup-box-overlay").remove();
        });
    };
    //function khi click yes
    var f_confirm_yes = function () {
        //Extens giữa option do người dùng truyền vào và option default
        var popup_options = $.extend(defaults, options);
        if (popup_options.confirm.yes.event != null) {
            popup_options.confirm.yes.event();
        }
        popupclose();
    };
    //function khi click no
    var f_confirm_no = function () {
        //Extens giữa option do người dùng truyền vào và option default
        var popup_options = $.extend(defaults, options);
        if (popup_options.confirm.no.event != null) {
            popup_options.confirm.no.event();
        }
        popupclose();
    };
    //function set vị trí cho popup
    var setPosition = function () {
        var popup_options = $.extend(defaults, options);
        var screenwidth = $(window).width();
        var screenheight = $(window).height();
        var popup = $("#popup-wrapper");
        var popup_main_content = $("#popup-main-content");
        if ($(popup).width() < screenwidth) {
            //xet position là vị trí chính giữa của window
            $("#popup-wrapper").position({
                my: 'center',
                at: 'center',
                of: window,
                collision: 'fit'
            });
        }
        if ($(popup_main_content).height() > popup_options.height &&
            popup_options.height >= 500) {
            $(popup_main_content).css({
                "height": popup_options.height - 50,
                "overflow-y": "scroll",
            });
        }
        else {
            $(popup_main_content).css({
                "height": "auto",
                "overflow-y": "none",
            });
        }
    };
    //privileged function
    //function khi hiển thị popup
    this.popupShow = function () {
        init();
    };
    //function khi đóng popup
    this.popupClose = popupclose();
    /*
        Popup Info Event Close
    */
    //tạo sự kiện khi click vào button controls xử lý đóng form popup 
};
var jpopup = {
    show: function (options) {
        new IPopup(options).popupShow();
    },
    hide: function () {
        new IPopup().popupClose();
    }
}