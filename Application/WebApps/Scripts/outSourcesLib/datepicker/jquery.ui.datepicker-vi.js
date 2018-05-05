jQuery(function (e) { e.datepicker.regional["vi"] = { closeText: "Đóng", prevText: "&#x3c;Trước", nextText: "Tiếp&#x3e;", currentText: "Hôm nay", monthNames: ["Tháng Một", "Tháng Hai", "Tháng Ba", "Tháng Tư", "Tháng Năm", "Tháng Sáu", "Tháng Bảy", "Tháng Tám", "Tháng Chín", "Tháng Mười", "Tháng Mười Một", "Tháng Mười Hai"], monthNamesShort: ["Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12"], dayNames: ["Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy"], dayNamesShort: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"], dayNamesMin: ["CN", "T2", "T3", "T4", "T5", "T6", "T7"], weekHeader: "Tu", dateFormat: "dd/mm/yy", firstDay: 0, isRTL: false, showMonthAfterYear: false, yearSuffix: "" }; e.datepicker.setDefaults(e.datepicker.regional["vi"]) })

//disable datepicker
//$("#txtToDate").datepicker('disable');
//$("#txtFromDate").datepicker('disable');

$(document).ready(function () {
    $(".InputDate").datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        showAnim: 'slide',
        yearRange: '-100:+100',
        showOn: "button",
        buttonImage: "/OutsourceLib/datepicker/ui-lightness/images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Chọn ngày dd/MM/yyyy"
    }, $.datepicker.regional['vi']);

    $(".InputDateInset").datepicker({
        showOtherMonths: true,
        selectOtherMonths: true,
        changeMonth: true,
        changeYear: true,
        dateFormat: "dd/mm/yy",
        showAnim: 'slide',
        yearRange: '-100:+100',
    }, $.datepicker.regional['vi']);

    $('.InputYear').datepicker({
        changeYear: true,
        dateFormat: 'yy',
        yearRange: '-100:+100',
        showOn: "button",
        showAnim: 'slide',
        buttonImage: "/OutsourceLib/datepicker/ui-lightness/images/calendar.gif",
        buttonImageOnly: true,
        buttonText: "Chọn năm",
        stepMonths: 12,
        onClose: function () {
            var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
            $(this).datepicker('setDate', new Date(year, 1, 1));
        },
        beforeShow: function () {
            if ((selectDate = $(this).val()).length > 0) {
                $(this).datepicker('option', 'defaultDate', new Date(selectDate, 1, 1));
                $(this).datepicker('setDate', new Date(selectDate, 1, 1));
            }
        }
    }, $.datepicker.regional['vi']);

    //$('.InputMonth').datepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    dateFormat: 'mm/yy',
    //    yearRange: '-100:+100',
    //    showOn: "button",
    //    showAnim: 'slide',
    //    buttonImage: "/Content/datepicker/ui-lightness/images/calendar.gif",
    //    buttonImageOnly: true,
    //    buttonText: "Chọn năm",
    //    onClose: function () {
    //        var year = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
    //        var month = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
    //        $(this).datepicker('setDate', new Date(year, month, 1));
    //    },
    //    beforeShow: function () {
    //        if ((selectDate = $(this).val()).length > 0) {
    //            var arr_date = selectDate.split('/');
    //            $(this).datepicker('option', 'defaultDate', new Date(arr_date[1], arr_date[0] - 1, 1));
    //            $(this).datepicker('setDate', new Date(arr_date[1], arr_date[0] - 1, 1));
    //        }
    //    }
    //}, $.datepicker.regional['vi']);
});