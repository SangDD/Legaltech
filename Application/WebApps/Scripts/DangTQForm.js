// ham script
function CreateRollingWaitingIcon(is_display) {
    var html = "<div style=\"height: 100%; width: 100%; top: 0; left: 0; z-index: 2001; position: absolute;\"><div class=\"loader\"><div></div><div></div><div></div><div></div><div></div></div></div>";
    if (is_display) {
        $("body").append(html);
    }
    else {
        $("body").find(".loader").parent().remove();
    }
}

//Check định dạng
function checkDate(p_name, p_id, p_val) {
    try {
        if (p_val == "__/__/____") {
            p_val = "";
        }
        var isPass = isDate_ddMMyyyy(p_val);
        if (p_val == "") {
            jError(p_name + " không được để trống!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 0) {
            jError(p_name + " " + p_val + " sai định dạng ngày dd/mm/yyyy!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 1) {
            jError(p_name + " " + p_val + " không tồn tại!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        return true;
    }
    catch (e) {
        alert(e);
        return false;
    }
}

function checkDate_NoShowMsg(p_id, p_val) {
    try {
        if (p_val == "__/__/____") {
            p_val = "";
        }
        var isPass = isDate_ddMMyyyy(p_val);
        if (p_val == "") {
            $(p_id).focus();
            return false;
        }
        if (isPass == 0) {
            $(p_id).focus();
            return false;
        }
        if (isPass == 1) {
            $(p_id).focus();
            return false;
        }
        return true;
    }
    catch (e) {
        alert(e);
        return false;
    }
}

function checkValidate_Search(p_name, p_id, p_val) {
    try {
        if (p_val == "__/__/____" || p_val == "") {
            return true;
        }

        var isPass = isDate_ddMMyyyy(p_val);
        if (isPass == 0) {
            jError(p_name + " " + p_val + " sai định dạng ngày dd/mm/yyyy!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 1) {
            jError(p_name + " " + p_val + " không tồn tại!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        return true;
    }
    catch (e) {
        alert(e);
        return false;
    }
}

//  check giờ định dạng HH:mm
// type = am/pm
function Check_fomat_Hours(p_name, p_id, p_hour, p_type) {
    try {
        var _arr_time = p_hour.split(":");
        if (_arr_time.length != 2) {
            jError(p_name + " không đúng định dạng!", "THÔNG BÁO", function () {
                $(p_id).focus();
            });
            return false;
        }
        for (var i in _arr_time) {
            var _time = _arr_time[i];
           
            if (_time.length != 2) {
                jError(p_name + " không đúng định dạng!", "THÔNG BÁO", function () {
                    $(p_id).focus();
                });
                return false;
            }

            if (i == 0) {
                if (parseFloat(_time) > 23 || parseFloat(_time) < 0) {
                    jError("Giờ trong khoảng " + p_name + " phải trong khoảng từ 0h-23h!", "THÔNG BÁO", function () {
                        $(p_id).focus();
                    });
                    return false;
                }

                if (p_type == "AM") {
                    if (parseFloat(_time) > 12) {
                        jError("Giờ trong khoảng " + p_name + " phải trong khoảng từ 0h-11h59 !", "THÔNG BÁO", function () {
                            $(p_id).focus();
                        });
                        return false;
                    }
                }
                else if (p_type == "PM") {
                    if (parseFloat(_time) < 12) {
                        jError("Giờ trong khoảng " + p_name + " phải trong khoảng từ 12h-23h59 !", "THÔNG BÁO", function () {
                            $(p_id).focus();
                        });
                        return false;
                    }
                }
            }
            else if (i == 1) {
                if (parseFloat(_time) > 59 || parseFloat(_time) < 0) {
                    jError("Phút trong khoảng " + p_name + " phải trong khoảng từ 00-59!", "THÔNG BÁO", function () {
                        $(p_id).focus();
                    });
                    return false;
                }
            }
        }

        return true;

    } catch (e) {
        alert(e);
        return false;
    }
}

// check khoảng thời gian có ok hay không p_time_1 <= p_time_2
function Check_Validate_RangeTime(p_time_1, p_time_2) {
    try {

        var _arr_time_1 = p_time_1.split(":");
        var _h1 = _arr_time_1[0];
        var _m1 = _arr_time_1[1];

        var _arr_time_2 = p_time_2.split(":");
        var _h2 = _arr_time_2[0];
        var _m2 = _arr_time_2[1];

        if (_h1 < _h2) {
            return true;
        }
        else if (_h1 == _h2) {
            // giờ bằng nhau thì check đến phút

            //if (_m1 >= _m2) {
            //    return false;
            //}
            if (_m1 > _m2) {
                return false;
            }
        }
        else // h1 > h2
            return false;

    } catch (e) {
        alert(e);
        return false;
    }

}

function Check_Time_Reg() {
    try {

        // Sáng đăng ký từ giờ -> đến giờ
        var txt_Hour_Expire_Reg_From_Am = $("#txt_Hour_Expire_Reg_From_Am").val();
        var txt_Hour_Expire_Reg_To_Am = $("#txt_Hour_Expire_Reg_To_Am").val();

        // chiều đăng ký từ giờ -> đến giờ
        var txt_Hour_Expire_Reg_From_Pm = $("#txt_Hour_Expire_Reg_From_Pm").val();
        var txt_Hour_Expire_Reg_To_Pm = $("#txt_Hour_Expire_Reg_To_Pm").val();


        // sáng từ
        if (txt_Hour_Expire_Reg_From_Am == "") {
            jError("TG bắt đầu đăng ký buổi sáng không được bỏ trống!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_From_Am").val('');
                $("#txt_Hour_Expire_Reg_From_Am").focus();
            });
            return false;
        }

        var _arr_time_From_Am = txt_Hour_Expire_Reg_From_Am.split(":");
        if (Check_fomat_Hours("TG bắt đầu đăng ký buổi sáng", "#txt_Hour_Expire_Reg_From_Am", txt_Hour_Expire_Reg_From_Am, "AM") == false) {
         
            return false;
        }

        // sáng đến 
        if (txt_Hour_Expire_Reg_To_Am == "") {
            jError("TG kết thúc đăng ký buổi sáng không được bỏ trống!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Am").val('');
                $("#txt_Hour_Expire_Reg_To_Am").focus();
            });
            return false;
        }

        var _arr_time_To_Am = txt_Hour_Expire_Reg_To_Am.split(":");
        if (Check_fomat_Hours("TG kết thúc đăng ký buổi sáng", "#txt_Hour_Expire_Reg_To_Am", txt_Hour_Expire_Reg_To_Am, "AM") == false) {
            return false;
        }

        // bắt đầu buổi sáng < kết thúc buổi sáng
        if (Check_Validate_RangeTime(txt_Hour_Expire_Reg_From_Am, txt_Hour_Expire_Reg_To_Am) == false) {
            jError("TG kết thúc đăng ký buổi sáng phải lớn hơn Thời gian bắt đầu đăng ký buổi sáng !", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Am").val('');
                $("#txt_Hour_Expire_Reg_To_Am").focus();
            });
            return false;
        }

        // chiều từ
        if (txt_Hour_Expire_Reg_From_Pm == "") {
            jError("TG bắt đầu đăng ký buổi chiều không được bỏ trống!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_From_Pm").val('');
                $("#txt_Hour_Expire_Reg_From_Pm").focus();
            });
            return false;
        }

        if (txt_Hour_Expire_Reg_From_Pm == "00:00") {
            jError("TG bắt đầu đăng ký buổi chiều phải lớn hơn 12h !", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_From_Pm").val('');
                $("#txt_Hour_Expire_Reg_From_Pm").focus();
            });
            return false;
        }

        var _arr_time_From_Pm = txt_Hour_Expire_Reg_From_Pm.split(":");
        if (Check_fomat_Hours("TG bắt đầu đăng ký buổi chiều", "#txt_Hour_Expire_Reg_From_Pm", txt_Hour_Expire_Reg_From_Pm, "PM") == false) {
            return false;
        }

        // chiều đến
        if (txt_Hour_Expire_Reg_To_Pm == "") {
            jError("TG kết thúc đăng ký buổi chiều không được bỏ trống!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Pm").val('');
                $("#txt_Hour_Expire_Reg_To_Pm").focus();
            });
            return false;
        }

        var _arr_time_To_Pm = txt_Hour_Expire_Reg_To_Pm.split(":");
        if (Check_fomat_Hours("TG kết thúc đăng ký buổi chiều", "#txt_Hour_Expire_Reg_To_Pm", txt_Hour_Expire_Reg_To_Pm, "PM") == false) {
            return false;
        }

        // bắt đầu buổi sáng < kết thúc buổi sáng
        if (Check_Validate_RangeTime(txt_Hour_Expire_Reg_From_Pm, txt_Hour_Expire_Reg_To_Pm) == false) {
            jError("TG kết thúc đăng ký buổi chiều phải lớn hơn Thời gian bắt đầu đăng ký buổi chiều !", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Pm").val('');
                $("#txt_Hour_Expire_Reg_To_Pm").focus();
            });
            return false;
        }

        // ngày cuối
        var txt_Hour_Expire_Reg_Last_Date = $("#txt_Hour_Expire_Reg_Last_Date").val();
        if (txt_Hour_Expire_Reg_Last_Date == "") {
            jError("Giờ hết hạn ĐK cho ngày cuối không được bỏ trống!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_Last_Date").val('');
                $("#txt_Hour_Expire_Reg_Last_Date").focus();
            });
            return false;
        }

        var _arr_time_Last_Date = txt_Hour_Expire_Reg_Last_Date.split(":");
        if (Check_fomat_Hours("Giờ hết hạn ĐK cho ngày cuối", "#txt_Hour_Expire_Reg_Last_Date", txt_Hour_Expire_Reg_Last_Date, "PM") == false) {
            return false;
        }

        // check thời gian sáng với chiều;
        var _int_fa = parseFloat("2014" + _arr_time_From_Am[0] + _arr_time_From_Am[1]);
        var _int_ta = parseFloat("2014" + _arr_time_To_Am[0] + _arr_time_To_Am[1]);

        var _int_fp = parseFloat("2014" + _arr_time_From_Pm[0] + _arr_time_From_Pm[1]);
        var _int_tp = parseFloat("2014" + _arr_time_To_Pm[0] + _arr_time_To_Pm[1]);


        if (_int_fa > _int_ta) {
            jError("Thời gian bắt đầu đăng ký buổi sáng không được lớn hơn thời gian kết thúc đăng ký buổi sáng!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Am").val('');
                $("#txt_Hour_Expire_Reg_To_Am").focus();
            });
            return false;
        }

        if (_int_fp > _int_tp) {
            jError("Thời gian bắt đầu đăng ký buổi chiều không được lớn hơn thời gian kết thúc đăng ký buổi chiều!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Pm").val('');
                $("#txt_Hour_Expire_Reg_To_Pm").focus();
            });
            return false;
        }
        if (_int_ta > _int_fp) {
            jError("Thời gian kết thúc đăng ký buổi sáng không được lớn hơn thời gian bắt đầu đăng ký buổi chiều!", "lỗi", function () {
                $("#txt_Hour_Expire_Reg_To_Pm").val('');
                $("#txt_Hour_Expire_Reg_To_Pm").focus();
            });
            return false;
        }
        return true;

    } catch (e) {
        alert(e);
        return false;
    }
}

function Check_Time_Reg_Investor() {
    try {
        // Thời gian đăng ký
        var Time_Expire_Reg = $("#Time_Expire_Reg").val();
        var Time_Expire_Reg_Exp = $("#Time_Expire_Reg_Exp").val();

        // Sáng đăng ký từ giờ -> đến giờ
        var Hour_Expire_Reg_From_Am = $("#Hour_Expire_Reg_From_Am").val();
        var Hour_Expire_Reg_To_Am = $("#Hour_Expire_Reg_To_Am").val();

        // chiều đăng ký từ giờ -> đến giờ
        var Hour_Expire_Reg_From_Pm = $("#Hour_Expire_Reg_From_Pm").val();
        var Hour_Expire_Reg_To_Pm = $("#Hour_Expire_Reg_To_Pm").val();
        var Hour_Expire_Reg_Last_Date = $("#Hour_Expire_Reg_Last_Date").val();

        // thời gian đăng ký từ ngày tới ngày
        if (Time_Expire_Reg == "" || Time_Expire_Reg_Exp == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. Ngày đăng ký từ không có dữ liệu!", "lỗi");
            return false;
        }

        // sáng từ
        if (Hour_Expire_Reg_From_Am == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. TG bắt đầu đăng ký buổi sáng không có dữ liệu!", "lỗi");
            return false;
        }

        // sáng đến 
        if (Hour_Expire_Reg_To_Am == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. TG kết thúc đăng ký buổi sáng không có dữ liệu!", "lỗi");
            return false;
        }

        // chiều từ
        if (Hour_Expire_Reg_From_Pm == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. TG bắt đầu đăng ký buổi chiều không có dữ liệu!", "lỗi");
            return false;
        }

        // chiều đến
        if (Hour_Expire_Reg_To_Pm == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. TG kết thúc đăng ký buổi chiều không có dữ liệu!", "lỗi");
            return false;
        }

        // ngày cuối
        if (Hour_Expire_Reg_Last_Date == "") {
            jError("Không đủ điều kiện để check thời gian hết hạn đăng ký. Giờ hết hạn ĐK cho ngày cuối không có dữ liệu!", "lỗi");
            return false;
        }

        var _arr_time_From_Am = Hour_Expire_Reg_From_Am.split(":");
        var _arr_time_To_Am = Hour_Expire_Reg_To_Am.split(":");
        var _arr_time_From_Pm = Hour_Expire_Reg_From_Pm.split(":");
        var _arr_time_To_Pm = Hour_Expire_Reg_To_Pm.split(":");
        var _arr_time_Last_Date = Hour_Expire_Reg_Last_Date.split(":");

        // sửa lại
        var txt_DB_Date = $("#txt_DB_Date").val();
        var txt_DB_Time = $("#txt_DB_Time").val();
        var _arr_DB_Time = txt_DB_Time.split(":");

        // convert ngày ra so
        var _int_fa = parseFloat("2014" + _arr_time_From_Am[0] + _arr_time_From_Am[1]);
        var _int_ta = parseFloat("2014" + _arr_time_To_Am[0] + _arr_time_To_Am[1]);

        var _int_fp = parseFloat("2014" + _arr_time_From_Pm[0] + _arr_time_From_Pm[1]);
        var _int_tp = parseFloat("2014" + _arr_time_To_Pm[0] + _arr_time_To_Pm[1]);
        
        var _int_last_date = parseFloat("2014" + _arr_time_Last_Date[0] + _arr_time_Last_Date[1]);

        var _int_tn = parseFloat("2014" + _arr_DB_Time[0] + _arr_DB_Time[1]);

        // nếu nằm trong khoảng ngày
      
        if (compare2Date(txt_DB_Date, Time_Expire_Reg_Exp, "==") == true) {
            if (_int_tn <= _int_last_date) {
                return true;
            }
            else {
                jError("Hết thời hạn đăng ký đợt đấu giá!", "lỗi");
                return false;
            }
        }
        else if (compare2Date(txt_DB_Date, Time_Expire_Reg, ">=") == true
            && compare2Date(txt_DB_Date, Time_Expire_Reg_Exp, "<=") == true) {
            if ((_int_tn >= _int_fa && _int_tn <= _int_ta) || (_int_tn >= _int_fp && _int_tn <= _int_tp)) {
                return true;
            }
            else
            {
                jError("Hết thời hạn đăng ký đợt đấu giá!", "lỗi");
                return false;
            }
        }
        else
        {
            if (compare2Date(txt_DB_Date, Time_Expire_Reg, "<") == true) {
                jError("Đợt đấu giá chưa đến ngày giờ nhập hồ sơ đăng ký tham dự!", "lỗi");
                return false;
            }
            else {
                jError("Hết thời hạn đăng ký đợt đấu giá!", "lỗi");
                return false;
            }
        }
         
        return true;

    } catch (e) {
        alert(e);
        return false;
    }
}

function Check_Share_Unit() {
    try {

        var txt_Share_Unit = $("#txt_Share_Unit").val();
        var txt_Quote_Qtty = $("#txt_Quote_Qtty").val();
        var txt_Foreign_Rate = $("#txt_Foreign_Rate").val();
        var txt_Min_Reg_Qtty = $("#txt_Min_Reg_Qtty").val();
        var txt_Min_Qtty = $("#txt_Min_Qtty").val();
        var txt_Dc_Rate = $("#txt_Dc_Rate").val();
        var txt_Fc_Rate = $("#txt_Fc_Rate").val();
        var txt_Dp_Rate = $("#txt_Dp_Rate").val();
        var txt_Fp_Rate = $("#txt_Fp_Rate").val();


        if (txt_Share_Unit == "") {
            jError("Đơn vị tính (CK/lô) không được bỏ trống!", "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Share_Unit = parseInt(txt_Share_Unit.replace(/,/g, ""))
        if (_int_txt_Share_Unit == 0) {
            jError("Đơn vị tính (CK/lô) phải lớn hơn 0!", "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Quote_Qtty = parseInt(txt_Quote_Qtty.replace(/,/g, ""))
        if (_int_txt_Quote_Qtty % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng bán đấu giá " + txt_Quote_Qtty, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Foreign_Rate = parseInt(txt_Foreign_Rate.replace(/,/g, ""))
        if (_int_txt_Foreign_Rate % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng bán cho NĐT nước ngoài " + txt_Foreign_Rate, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Min_Reg_Qtty = parseInt(txt_Min_Reg_Qtty.replace(/,/g, ""))
        if (_int_txt_Min_Reg_Qtty % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng đăng ký tối thiểu " + txt_Min_Reg_Qtty, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Min_Qtty = parseInt(txt_Min_Qtty.replace(/,/g, ""))
        if (_int_txt_Min_Qtty % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng tối thiểu/1 mức giá " + txt_Min_Qtty, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Dc_Rate = parseInt(txt_Dc_Rate.replace(/,/g, ""))
        if (_int_txt_Dc_Rate % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng tối đa 1 tổ chức trong nước " + txt_Dc_Rate, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Fc_Rate = parseInt(txt_Fc_Rate.replace(/,/g, ""))
        if (_int_txt_Fc_Rate > 0 && _int_txt_Fc_Rate % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng tối đa 1 tổ chức nước ngoài " + txt_Fc_Rate, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }


        var _int_txt_Dp_Rate = parseInt(txt_Dp_Rate.replace(/,/g, ""))
        if (_int_txt_Dp_Rate % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng tối đa 1 cá nhân trong nước " + txt_Dp_Rate, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

        var _int_txt_Fp_Rate = parseInt(txt_Fp_Rate.replace(/,/g, ""))
        if (_int_txt_Fp_Rate > 0 && _int_txt_Fp_Rate % _int_txt_Share_Unit != 0) {
            jError("Đơn vị tính (CK/lô) phải là Ước của Khối lượng tối đa 1 cá nhân nước ngoài " + txt_Fp_Rate, "lỗi", function () {
                $("#txt_Share_Unit").val('');
                $("#txt_Share_Unit").focus();
            });
            return false;
        }

    } catch (e) {
        alert(e);
        return false;
    }
}

//Tạo ngày theo định dạng MM/dd/yyyy
function formatDate(p_date) {
    try {
        var date = p_date;
        var p_day = date.substr(0, 2);
        var p_month = date.substr(3, 2);
        var p_year = date.substr(6, 4);

        var new_date = new Date(p_year, p_month - 1, p_day);
        return new_date;
    } catch (e) {
        alert("Có lỗi xảy ra formatDate");
        return null;
    }
}
//Tạo ngày kèm theo giờ phút

function formatDate_time(p_date, p_hour, p_minute) {
    try {
        var date = p_date;
        var p_day = date.substr(0, 2);
        var p_month = date.substr(3, 2);
        var p_year = date.substr(6, 4);

        var new_date = new Date(p_year, p_month - 1, p_day, p_hour, p_minute);
        return new_date;
    } catch (e) {
        alert("Có lỗi xảy ra formatDateHT");
        return null;
    }
}
//So sánh 2 ngày với nhau
//type = 1: ngày 1 có lớn hơn ngày 2 ko
//type = 2: ngày 1 có nhỏ hơn ngày 2 ko
//type = 3: ngày 1 có lớn hơn ngày 2 ko bằng cũng đc
//type = 4: ngày 1 có nhỏ hơn ngày 2 ko bằng cũng đc
function compare2Date(p_date1, p_date2, type) {
    try {
        var date1 = formatDate(p_date1);
        var _date1_time = date1.getTime();

        var date2 = formatDate(p_date2);
        var _date2_time = date2.getTime();

        var result = _date1_time - _date2_time;

        switch (type) {
            case ">":
                return (result > 0);
                break;
            case "<":
                return (result < 0);
                break;
            case ">=":
                return (result >= 0);
                break;
            case "<=":
                return (result <= 0);
                break;
            case "==":
                return (result == 0);
                break;
        }
    } catch (e) {
        alert("Lỗi compare2Date");
        return null;
    }
}


function isValid(str) {
    return !/[~`@@!#$%\^&*+=\-\[\]\\';,/{}|\\":<>\?]/g.test(str);
}

//0: Sai định dạng 1: Ngày không tồn tại 2: Thành công
function isDate_ddMMyyyy(strDate) {
    var currVal = strDate;
    //var rxDatePattern = /^(\d{2})(\/|-)(\d{2})(\/|-)(\d{4})$/;
    var rxDatePattern = /^(\d{2})(\/)(\d{2})(\/)(\d{4})$/;
    var dtArray = currVal.match(rxDatePattern); // is format OK?
    if (dtArray == null) {
        return 0;
    }
    dtDay = dtArray[1];
    dtMonth = dtArray[3];
    dtYear = dtArray[5];
    if (dtYear < 1000)
        return 1;
    else if (dtMonth < 1 || dtMonth > 12)
        return 1;
    else if (dtDay < 1 || dtDay > 31)
        return 1;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return 1;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return 1;
    }
    return 2;
}



function FormatSpace(p_str) {
    if (p_str == "") return null;
    var str_fun = p_str.trim();
    var char_search = ' ';
    var index_start = str_fun.indexOf(char_search);
    var index_end = 0;
    var str_result = "";

    while (index_start != -1) {
        for (i = index_start; i < str_fun.length; i++) {
            if (str_fun.charAt(i) != ' ') {
                index_end = i;
                if (index_end - index_start >= 2) {
                    str_result += str_fun.substr(0, index_start + 1);
                    str_fun = str_fun.substr(index_end, str_fun.length)
                }
                break;
            }
        }
        if (str_fun.indexOf('  ') == -1) {
            break;
        }
        index_start = str_fun.indexOf(char_search);
    }

    alert(str_result);
}

function splitString(p_str, p_start, p_end) {
    var part1 = p_str.substr(0, p_start + 1);
    var part2 = p_str.substr(p_end, p_str.length);
    return (part1 + part2);
}

function checkUnicode(p_val) {
    var p_val_lower = p_val.toLowerCase();
    var VietNamKey = "áàạảãâấầậẩẫăắằặẳẵéèẹẻẽêếềệểễóòọỏõôốồộổỗơớờợởỡúùụủũưứừựửữíìịỉĩđýỳỵỷỹ";
    for (var i = 0; i < p_val.length; i++) {
        if (VietNamKey.indexOf(p_val_lower[i]) != -1) {
            return false;
        }
    }
    return true;
}
function checkPassword(p_val) {
    var re = /(?=.*\d)(?=.*[A-z]).{8,}/;
    return re.test(p_val);
}

function checkCapsLock(e, div) {
    var capsLockON;
    keyCode = e.keyCode ? e.keyCode : e.which;
    shiftKey = e.shiftKey ? e.shiftKey : ((keyCode == 16) ? true : false);
    if (((keyCode >= 65 && keyCode <= 90) && !shiftKey) || ((keyCode >= 97 && keyCode <= 122) && shiftKey)) {
        capsLockON = true;
    } else {
        capsLockON = false;
    }
    if (!capsLockON)
        $(div).hide();
    else
        $(div).show();
}

function plus2Time(p_val) {
    if (p_val.length == 1) {
        return ("0" + p_val);
    }
    return p_val;
}