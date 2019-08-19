$(function () {
	//$(document).ajaxSend(function (event, request) {
	//	var securityToken = document.head.querySelector('meta[name="__RequestVerificationToken"]').content;
	//	request.setRequestHeader("__RequestVerificationToken", securityToken);
	//});
});

//check session time out 
function CheckSessionTimeOut() {
    try {
        return true;
        // tạm thời rào lại đã
        //var rBool = false;
        //$.ajax({
        //    type: 'POST',
        //    url: '/home/CheckSessionTimeOut',
        //    dataType: "json",
        //    traditional: true,
        //    async: false,//Chuyen ve synchonize đồng bộ
        //    success: function (data) {
        //        if (data != null) {
        //            if (data["Code"] == -1) {
        //                rBool = false;
        //                jAlert("Hệ thống đã hết thời gian kết nối, bạn hãy đăng nhập lại", "@Html.Raw(WebApps.Resources.Resource.ThongBao)", function () {
        //                    window.location.href = "home";
        //                });

        //            }
        //            else {
        //                rBool = true;
        //            }
        //        }
        //        else {
        //            rBool = false;
        //        }
        //    }
        //});

        //return rBool;
    } catch (e) {
        alert(e.toString);
        return false;
    }
}

function CheckSessionTimeOutPortal() {
    try {
        var rBool = false;
        $.ajax({
            type: 'POST',
            url: '/wiki-view/CheckSessionTimeOutPortal',
            dataType: "json",
            traditional: true,
            async: false,//Chuyen ve synchonize đồng bộ
            success: function (data) {
                if (data != null) {
                    if (data["Code"] == -1) {
                        rBool = false;
                        alert(data["Msg"]);
                        window.location.href = "/";
                    }
                    else {
                        rBool = true;
                    }
                }
                else {
                    rBool = false;
                }
            }
        });

        return rBool;
    } catch (e) {
        alert(e.toString);
        return false;
    }
}
function onResponse(data) {
	if(validateResponse(data)) {
		if (data["result"]["IsActionSuccess"]) {
            showSuccess(data["result"]["message"]);
            //jAlert(data["result"]["message"]);
			return true;
		} else {
            showError(data["result"]["message"]);
            //jError(data["result"]["message"]);
			return false;
		}
	}
}

function onResponseWhenSearching(data, elementRenderData, callback) {
	if(validateResponse(data)) {
		$("#" + elementRenderData).html(data);
		ChangeIConSortWhenSortColumns();
		bindColumnSortWithSearching(callback);
	}
}

function validateResponse(data) {
	var urlRedirect = data['redirectTo'];
	if (urlRedirect != undefined && urlRedirect !== "") {
		var dataInPage = data['dataInPage'];
		if (dataInPage != undefined && dataInPage !== "") {
			showError('Bạn không có quyền truy cập chức năng này!');
			return false;
		}
		window.location.href = urlRedirect;
		return false;
	} else {
		return true;
	}
}

function getCookieByName(a) {
	for (var b = a + "=", c = document.cookie.split(";"), d = 0; d < c.length; d++) {
		for (var e = c[d]; " " === e.charAt(0) ;) {
			e = e.substring(1);
			if (0 === e.indexOf(b)) {
				return e.substring(b.length, e.length);
			}
		}
	}
	return "";
}

function setCookie(a, b, c) {
	var d = new Date; d.setTime(d.getTime() + 24 * c * 60 * 60 * 1e3);
	var e = "expires=" + d.toUTCString();
	document.cookie = a + "=" + b + "; " + e;
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

function CheckAndSetCookies() {
	try {
		var currentCookie = getCookieByName("language");
		if(currentCookie == null) setCookie("language", window.cookieLanguage, 1);
	} catch (a) { }
}

function checkRegexPassword(password) {
	try {
		//regular to check password
		var regularExpression = /^(?=.*[0-9])(?=.*[!@#_$%^&*])(?=.*[A-Z])[+a-zA-Z0-9!@#_$%^& *]{6,50}|(?=.*[0-9])(?=.*[!@#_$%^&*])(?=.*[a-z])[+a-zA-Z0-9!@#_$%^& *]{6,50}|(?=.*[a-z])(?=.*[!@#_$%^&*])(?=.*[A-Z])[+a-zA-Z0-9!@#_$%^& *]{6,50}|(?=.*[0-9])(?=.*[A-Z])(?=.*[a-z])[+a-zA-Z0-9!@#_$%^& *]{6,50}$/;
		return regularExpression.test(password);
	} catch (e) {
		console.log(e.message);
		return false;
	}
}

// Check chỉ nhập chữ không dấu và số + "_"
function checkNoneUnicodeAndSpecChar(nStr) {
	try {
		var rxDatePattern = /^[\w]+$/;
		if (nStr.indexOf(" ") >= 0)
			return false;
		var rxVal = nStr.match(rxDatePattern); // is format OK?
		if (rxVal == null || rxVal[0] !== nStr)
			return false;

		return true;
	} catch (e) {
		return false;
	}
}

function checkUnicodeCharacter(pStrString) {
	try {
		pStrString = pStrString.toLowerCase();
		var strRegex = /à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|ì|í|ị|ỉ|ĩ|ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|ỳ|ý|ỵ|ỷ|ỹ|đ/g;

		if (strRegex.test(pStrString)) return false;
		return true;
	} catch (e) {
		return false;
	}
}

function indexRowTable(table_id) {
	try {
		$(table_id).find('tbody').find("tr").each(function () {
			var _index = $(this);
			$(this).find("td:first").text(_index[0].rowIndex);
		});
	} catch (e) {
		return;
	}
}

function isNumberKey(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode !== 43 && charCode !== 45 && charCode !== 46) // 45 "-", 43 "+", 46 "."
		return false;
	return true;
}

// validate truong so bo cá truoc - ,+,.
function isNumberKeyNoCountSign(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) // 45 "-", 43 "+", 46 "."
		return false;
	return true;
}

// validate cho datetimepicker 
function isNumberKeyInDateTimeFormat(evt) {
	evt = (evt) ? evt : window.event;
	var charCode = (evt.which) ? evt.which : evt.keyCode;
	if (charCode === 47) return true;
	if (charCode > 31 && (charCode < 48 || charCode > 57)) // 45 "-", 43 "+", 46 ".", 47 "/"
		return false;
	return true;
}

function IsValidNumber(strNumber, grouping) {
	try {
		// ReSharper disable once AssignedValueIsNeverUsed
		grouping = grouping == undefined ? true : grouping;
		var regexNumberGrouping = /^[\+\-]?\d{1,3}(((,\d{3})+)?(\.\d+)?)$/; // /^[0-9]\d(((,\d{3}){1})?(\.\d{0,2})?)$/
		var regexNumber = /^[\+,\-]?\d+(\.\d+)?$/;
		return regexNumber.test(strNumber) || regexNumberGrouping.test(strNumber);
	} catch (e) {
		return false;
	}
}

// ----- check email -----
function IsvalidEmail(email) {
	try {
		var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
		return re.test(String(email).toLowerCase());
	} catch (e) {
		return false;
	}
}

function readURL(input, id_img) {
	if (input.files && input.files[0]) {
		var reader = new FileReader();
		reader.onload = function (e) {
			$('#' + id_img).attr('src', e.target.result);
		}
		reader.readAsDataURL(input.files[0]);
	}
}

function checkUnikey(pStrString, p_MsgErr, p_Tag) {
	var VietNamKey = "á,à,ạ,ả,ã,â,ấ,ầ,ậ,ẩ,ẫ,ă,ắ,ằ,ặ,ẳ,ẵ,é,è,ẹ,ẻ,ẽ,ê,ế,ề,ệ,ể,ễ,ó,ò,ọ,ỏ,õ,ô,ố,ồ,ộ,ổ,ỗ,ơ,ớ,ờ,ợ,ở,ỡ,ú,ù,ụ,ủ,ũ,ư,ứ,ừ,ự,ử,ữ,í,ì,ị,ỉ,ĩ,đ,ý,ỳ,ỵ,ỷ,ỹ,Á,À,Ạ,Ả,Ã,Â,Ấ,Ầ,Ậ,Ẩ,Ẫ,Ă,Ắ,Ằ,Ặ,Ẳ,Ẵ,É,È,Ẹ,Ẻ,Ẽ,Ê,Ế,Ề,Ệ,Ể,Ễ,Ó,Ò,Ọ,Ỏ,Õ,Ô,Ố,Ồ,Ộ,Ổ,Ỗ,Ơ,Ớ,Ờ,Ợ,Ở,Ỡ,Ú,Ù,Ụ,Ủ,Ũ,Ư,Ứ,Ừ,Ự,Ử,Ữ,Í,Ì,Ị,Ỉ,Ĩ,Đ,Ý,Ỳ,Ỵ,Ỷ,Ỹ";
	$("#divErr").remove();
	for (var i = 0; i < pStrString.length; i++) {
		if (VietNamKey.indexOf(pStrString[i]) !== -1) {
			var _Err = " <div id=\"divErr\" style=\"color:red;padding-top:15px;clear:both; padding-bottom:10px ;\">" + p_MsgErr + "</div>";
			var _tagHtml = "#" + p_Tag;
			$(_tagHtml).append(_Err);
			return -1;
		}
	}
	return 0;
}

function CheckKyTuDacBietShoKyTu(str) {
	try {
		var _specialKey = '|,}{@+&=!?;/#\"$%^*()<>`~[]\\';
		for (var i = 0; i < _specialKey.length; i++) {

			if (str.indexOf(_specialKey[i]) !== -1) {
				return _specialKey[i];
			}
		}
		return '';
	} catch (e) {
		console.log(e);
	}
}

function validateFileName(pStrString) {
	var VietNamKey = "á,à,ạ,ả,ã,â,ấ,ầ,ậ,ẩ,ẫ,ă,ắ,ằ,ặ,ẳ,ẵ,é,è,ẹ,ẻ,ẽ,ê,ế,ề,ệ,ể,ễ,ó,ò,ọ,ỏ,õ,ô,ố,ồ,ộ,ổ,ỗ,ơ,ớ,ờ,ợ,ở,ỡ,ú,ù,ụ,ủ,ũ,ư,ứ,ừ,ự,ử,ữ,í,ì,ị,ỉ,ĩ,đ,ý,ỳ,ỵ,ỷ,ỹ,Á,À,Ạ,Ả,Ã,Â,Ấ,Ầ,Ậ,Ẩ,Ẫ,Ă,Ắ,Ằ,Ặ,Ẳ,Ẵ,É,È,Ẹ,Ẻ,Ẽ,Ê,Ế,Ề,Ệ,Ể,Ễ,Ó,Ò,Ọ,Ỏ,Õ,Ô,Ố,Ồ,Ộ,Ổ,Ỗ,Ơ,Ớ,Ờ,Ợ,Ở,Ỡ,Ú,Ù,Ụ,Ủ,Ũ,Ư,Ứ,Ừ,Ự,Ử,Ữ,Í,Ì,Ị,Ỉ,Ĩ,Đ,Ý,Ỳ,Ỵ,Ỷ,Ỹ";
	var char_not_allow = "\,/,:,*,?,\",<,>,|";

	for (var i = 0; i < pStrString.length; i++) {
		if (VietNamKey.indexOf(pStrString[i]) !== -1 || char_not_allow.indexOf(pStrString[i]) !== -1) {
			return false;
		}
	}
	return true;
}

// check file ảnh 
function validateExtension4File_Content(v) {
	var allowedExtensions = new Array("jpg", "JPG", "jpeg", "JPEG", "GIF", "gif", "BMP", "bmp", "PNG", "png");
	for (var ct = 0; ct < allowedExtensions.length; ct++) {
		var sample = v.lastIndexOf(allowedExtensions[ct]);
		if (sample !== -1) {
			return 1;
		}
	}
	return 0;
}

function getExtension(filename) {
	var parts = filename.split('.');
	return parts[parts.length - 1];
}

function isImage(filename) {
	var ext = getExtension(filename);
	switch (ext.toLowerCase()) {
		case 'jpg':
		case 'gif':
		case 'bmp':
		case 'png':
		case 'jpeg':
		case 'jpx':
		case 'ief':
			//etc
			return true;
	}
	return false;
}

function isVideo(filename) {
	var ext = getExtension(filename);
	switch (ext.toLowerCase()) {
		case 'mp4':
		case 'mp3':
		case 'webm':
		case 'ogg':
		case 'flac':
			//case 'mkv':
			//etc
			return true;
	}
	return false;
}

// chặn nhập string có dấu + space
function funikeyNoSpace(el) {
    if (!$(el).val().match(/^[A-Za-z0-9]+$/) && $(el).val() !== "") {
		$(el).val($("#tmp_v_c").val());
	} else {
		$("#tmp_v_c").val($(el).val());
	}
	return false;
}

// ----------------------------------------    Valid datetime     ----------------------------------
function isValidDate_N0(strDate) {
	try {
		var currVal = strDate;
		if (currVal === '')
			return false;
		if (currVal === '01/01/0000')
			return false;
		var rxDatePattern = /^(\d{2})(\/|-)(\d{2})(\/|-)(\d{4})$/;
		var dtArray = currVal.match(rxDatePattern); // is format OK?
		if (dtArray == null)
			return false;
		var dtDay = dtArray[1] / 1;
		var dtMonth = dtArray[3] / 1;
		var dtYear = dtArray[5] / 1;
		if (dtMonth < 1 || dtMonth > 12)
			return false;
		else if (dtDay < 1 || dtDay > 31)
			return false;
		else if ((dtMonth === 4 || dtMonth === 6 || dtMonth === 9 || dtMonth === 11) && dtDay === 31)
			return false;
		else if (dtMonth === 2) {
			var isleap = (dtYear % 4 === 0 && (dtYear % 100 !== 0 || dtYear % 400 === 0));
			if (dtDay > 29 || (dtDay === 29 && !isleap))
				return false;
		}
		return true;
	} catch (e) {
		return false;
	}
}

function formatDate_N0(date) {
	date = new Date(date);

	var day = ('0' + date.getDate()).slice(-2);
	var month = ('0' + (date.getMonth() + 1)).slice(-2);
	var year = date.getFullYear();

	return day + '/' + month + '/' + year;
}

// return true if strDate1 <= strDate2
function compareDate_N0(strDate1, strDate2) {
	try {
		var rxDatePattern = /^(\d{2})(\/|-)(\d{2})(\/|-)(\d{4})$/;

		// Check xem có phải string date không đã
		var dtArray_1 = strDate1.match(rxDatePattern); // is format OK?
		var dtArray_2 = strDate2.match(rxDatePattern); // is format OK?

		if (dtArray_1 == null || dtArray_2 == null)
			return false;
		// Convert sang date để so sanhs
		var _date_1 = new Date(dtArray_1[5], dtArray_1[3] - 1, dtArray_1[1]);
		var _date_2 = new Date(dtArray_2[5], dtArray_2[3] - 1, dtArray_2[1]);
		if (_date_1 > _date_2) {
			return false;
		}
		return true;
	}
	catch (e) {
		return false;
	}
}

function isValidDateTime_N0(_datetime, mask) {
	if (typeof mask !== 'undefined' && mask) {
		if (mask === _datetime) {
			return true;
		}
	}
	if (_datetime.length === 18) {
		var _time = _datetime.substr(0, 5);
		var _date = _datetime.substr(8, 10);
		if (isValidDate_N0(_date) && isValidTime_N0(_time)) {
			return true;
		}
	}
	return false;
}

function isValidTime_N0(_time) {
	if (_time.length === 5) {
		var _hour = _time.substr(0, 2);
		var _mins = _time.substr(3, 2);
		if (typeof parseInt(_hour) == 'number' && typeof parseInt(_mins) == 'number') {
			if (_hour / 1 > 23 || _mins / 1 > 59) {
				return false;
			} else {
				return true;
			}
		} else { return false; }
	}
	return false;
}

// return true if _date1 <= _date2
function compareDateTime_N0(_datetime1, _datetime2) {
	try {
		var rxDatePattern = /^(\d{2})(\/|-)(\d{2})(\/|-)(\d{4})$/;

		var _date1 = _datetime1.substr(8, 10);
		var _time1 = _datetime1.substr(0, 5);

		var _date2 = _datetime2.substr(8, 10);
		var _time2 = _datetime2.substr(0, 5);

		var _arrdate1 = _date1.match(rxDatePattern); // is format OK?
		var _arrdate2 = _date2.match(rxDatePattern); // is format OK?

		if (_arrdate1 == null || _arrdate2 == null)
			return false;

		var _longdate1 = new Date(_arrdate1[5], _arrdate1[3] - 1, _arrdate1[1]);
		var _longdate2 = new Date(_arrdate2[5], _arrdate2[3] - 1, _arrdate2[1]);
		if (_longdate1 > _longdate2) {
			return false;
		} else if (_date1 === _date2) {
			var _longtime1 = _time1.replace(':', '') / 1;
			var _longtime2 = _time2.replace(':', '') / 1;
			if (_longtime1 > _longtime2)
				return false;
		}
		return true;
	}
	catch (e) {
		return false;
	}
}

function StringToDate_ddMMyyyy(strDate, strDelemiter) {
	strDelemiter = strDelemiter || '/';

	if (isValidDateTime_N0(strDate) === false) {
		return null;
	}

	var dateParts = strDate.split(strDelemiter);
	var year = dateParts[2];
	var month = dateParts[1];
	var day = dateParts[0];

	if (isNaN(day) || isNaN(month) || isNaN(year))
		return null;

	return new Date(year, month - 1, day);
}
function GetValRadioCheckBoxByName(_name)
{
    var list = $("input[name=" + _name + "]:checked").map(function () {
        return this.value;
    }).get();
    return list;
}
function SetValRadioCheckBoxByName(_name, _value) {
      $("input[name=" + _name + "]").map(function () {
        if (this.value == _value) {
            $(this).prop("checked", true);
        }
    });
     
}