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

function goBack() {
    window.history.back();
}

//Check định dạng
function checkDate(p_name, p_id, p_val) {
    try {
        if (p_val == "__/__/____") {
            p_val = "";
        }
        var isPass = isDate_ddMMyyyy(p_val);
        if (p_val == "") {
            jError(p_name + " not be empty!", "NOTIFICATION", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 0) {
            jError(p_name + " " + p_val + " wrong format dd/mm/yyyy!", "NOTIFICATION", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 1) {
            jError(p_name + " " + p_val + " does not exist!", "NOTIFICATION", function () {
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
            jError(p_name + " " + p_val + " wrong format dd/mm/yyyy!", "NOTIFICATION", function () {
                $(p_id).focus();
            });
            return false;
        }
        if (isPass == 1) {
            jError(p_name + " " + p_val + " does not exist!", "NOTIFICATION", function () {
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

function Check_Time_Sheet() {
    try {

        var txtFrom_Time = $("#txtFrom_Time").val();
        var txtTo_Time = $("#txtTo_Time").val();

        // from
        if (txtFrom_Time == "") {
            jError("Start time cannot be left blank!", "Error", function () {
                $("#txtFrom_Time").val('');
                $("#txtFrom_Time").focus();
            });
            return false;
        }

        var _arr_From_Time = txtFrom_Time.split(":");
        if (Check_fomat_Hours("Start time", "#txtFrom_Time", txtFrom_Time) == false) {
            return false;
        }

        // to
        if (txtTo_Time == "") {
            jError("End time must not be blank!", "Error", function () {
                $("#txtTo_Time").val('');
                $("#txtTo_Time").focus();
            });
            return false;
        }

        var _arr_To_Time = txtTo_Time.split(":");
        if (Check_fomat_Hours("End time", "#txtTo_Time", txtTo_Time) == false) {
            return false;
        }

        // bắt đầu buổi sáng < kết thúc buổi sáng
        if (Check_Validate_RangeTime(txtFrom_Time, txtTo_Time) == false) {
            jError("End time must be greater than Start time !", "Error", function () {
                $("#txtTo_Time").val('');
                $("#txtTo_Time").focus();
            });
            return false;
        }

        return true;

    } catch (e) {
        alert(e);
        return false;
    }
}


//  check giờ định dạng HH:mm
// type = am/pm
function Check_fomat_Hours(p_name, p_id, p_hour) {
    try {
        var _arr_time = p_hour.split(":");
        if (_arr_time.length != 2) {
            jError(p_name + " wrong format HH:mm!", "NOTIFICATION", function () {
                $(p_id).focus();
            });
            return false;
        }
        for (var i in _arr_time) {
            var _time = _arr_time[i];

            if (_time.length != 2) {
                jError(p_name + " wrong format HH:mm!", "NOTIFICATION", function () {
                    $(p_id).focus();
                });
                return false;
            }

            if (i == 0) {
                if (parseFloat(_time) > 23 || parseFloat(_time) < 0) {
                    jError("Time of about " + p_name + " must in 0h-23h!", "NOTIFICATION", function () {
                        $(p_id).focus();
                    });
                    return false;
                }
            }
            else if (i == 1) {
                if (parseFloat(_time) > 59 || parseFloat(_time) < 0) {
                    jError("Minutes in approx " + p_name + " must in 00-59!", "NOTIFICATION", function () {
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


//  check giờ định dạng HH:mm type = am/pm
function Check_fomat_Hours_APM(p_name, p_id, p_hour, p_type) {
    try {
        var _arr_time = p_hour.split(":");
        if (_arr_time.length != 2) {
            jError(p_name + " wrong format!", "NOTIFICATION", function () {
                $(p_id).focus();
            });
            return false;
        }
        for (var i in _arr_time) {
            var _time = _arr_time[i];
           
            if (_time.length != 2) {
                jError(p_name + " wrong format!", "NOTIFICATION", function () {
                    $(p_id).focus();
                });
                return false;
            }

            if (i == 0) {
                if (parseFloat(_time) > 23 || parseFloat(_time) < 0) {
                    jError("Time of about " + p_name + " must in 0h-23h!", "NOTIFICATION", function () {
                        $(p_id).focus();
                    });
                    return false;
                }

                if (p_type == "AM") {
                    if (parseFloat(_time) > 12) {
                        jError("Time of about " + p_name + " must in 0h-11h59 !", "@Html.Raw(WebApps.Resources.Resource.ThongBao)", function () {
                            $(p_id).focus();
                        });
                        return false;
                    }
                }
                else if (p_type == "PM") {
                    if (parseFloat(_time) < 12) {
                        jError("Time of about " + p_name + " must in 12h-23h59 !", "@Html.Raw(WebApps.Resources.Resource.ThongBao)", function () {
                            $(p_id).focus();
                        });
                        return false;
                    }
                }
            }
            else if (i == 1) {
                if (parseFloat(_time) > 59 || parseFloat(_time) < 0) {
                    jError("Phút trong khoảng " + p_name + " must in 00-59!", "@Html.Raw(WebApps.Resources.Resource.ThongBao)", function () {
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
