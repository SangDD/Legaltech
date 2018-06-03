
//*****************************************************************************
dragtable = {
    ColumnStartIndex: 0,
    ColumnStopIndex: 0,
    // How far should the mouse move before it's considered a drag, not a click?
    dragRadius2: 100,
    setMinDragDistance: function (x) {
        dragtable.dragRadius2 = x * x;
    },

    //How long should cookies persist? (in days)
    cookieDays: 365,
    setCookieDays: function (x) {
        dragtable.cookieDays = x;
    },

    // Determine browser and version.
    // TODO: eliminate browser sniffing except where it's really necessary.
    Browser: function () {
        var ua, s, i;

        this.isIE = false;
        this.isNS = false;
        this.version = null;
        ua = navigator.userAgent;

        s = "MSIE";
        if ((i = ua.indexOf(s)) >= 0) {
            this.isIE = true;
            this.version = parseFloat(ua.substr(i + s.length));
            return;
        }

        s = "Netscape6/";
        if ((i = ua.indexOf(s)) >= 0) {
            this.isNS = true;
            this.version = parseFloat(ua.substr(i + s.length));
            return;
        }

        // Treat any other "Gecko" browser as NS 6.1.
        s = "Gecko";
        if ((i = ua.indexOf(s)) >= 0) {
            this.isNS = true;
            this.version = 6.1;
            return;
        }
    },
    browser: null,

    // Detect all draggable tables and attach handlers to their headers.
    //====**** hàm khởi tạo đã được khai báo bên page đang sử dụng   ****=====
    init: function () {
        // Don't initialize twice
        if (arguments.callee.done) return;
        arguments.callee.done = true;
        if (_dgtimer) clearInterval(_dgtimer);
        if (!document.createElement || !document.getElementsByTagName) return;

        dragtable.dragObj.zIndex = 0;
        dragtable.browser = new dragtable.Browser();
        //forEach(document.getElementsByTagName('table'), function (table) {
        //    if (table.className.search(/\bdraggable\b/) != -1) {
        //        dragtable.makeDraggable(table);
        //    }

        //});
    },

    // The thead business is taken straight from sorttable.
    makeDraggable: function (table) {
        if (table.getElementsByTagName('thead').length == 0) {
            the = document.createElement('thead');
            the.appendChild(table.rows[0]);
            table.insertBefore(the, table.firstChild);
        }

        // Safari doesn't support table.tHead, sigh
        if (table.tHead == null) {
            table.tHead = table.getElementsByTagName('thead')[0];
        }

        var headers = table.tHead.rows[0].cells;
        for (var i = 0; i < headers.length; i++) {
            headers[i].onmousedown = dragtable.dragStart;
        }

        // Replay reorderings from cookies if there are any.
        //if (dragtable.cookiesEnabled() && table.id && -- mặc định 
        //if (dragtable.cookiesEnabled() && dragtable.user!="default" && //chỉnh sửa theo mục đích
        //		table.className.search(/\bforget-ordering\b/) == -1) {
        //    dragtable.replayDrags(table);
        //}
    },

    // Global object to hold drag information.
    dragObj: new Object(),

    // Climb up the DOM until there's a tag that matches.
    findUp: function (elt, tag) {
        do {
            if (elt.nodeName && elt.nodeName.search(tag) != -1)
                return elt;
        } while (elt = elt.parentNode);
        return null;
    },

    // clone an element, copying its style and class.
    fullCopy: function (elt, deep) {
        var new_elt = elt.cloneNode(deep);
        new_elt.className = elt.className;
        forEach(elt.style,
            function (value, key, object) {
                if (value == null) return;
                if (typeof (value) == "string" && value.length == 0) return;

                new_elt.style[key] = elt.style[key];
            });
        return new_elt;
    },

    eventPosition: function (e) {
        var e = window.event || e;
        var x, y;
        //if (dragtable.browser.isIE) {
        x = e.clientX + document.documentElement.scrollLeft + document.body.scrollLeft;
        y = e.clientY + document.documentElement.scrollTop + document.body.scrollTop;
        return { x: x, y: y };
        //}
        //return { x: event.pageX, y: event.pageY };
    },

    // Determine the position of this element on the page. Many thanks to Magnus
    // Kristiansen for help making this work with "position: fixed" elements.
    absolutePosition: function (elt, stopAtRelative) {
        var ex = 0, ey = 0;
        do {
            var curStyle = dragtable.browser.isIE ? elt.currentStyle
                                                  : window.getComputedStyle(elt, '');
            var supportFixed = !(dragtable.browser.isIE &&
                                 dragtable.browser.version < 7);
            if (stopAtRelative && curStyle.position == 'relative') {
                break;
            } else if (supportFixed && curStyle.position == 'fixed') {
                // Get the fixed el's offset
                ex += parseInt(curStyle.left, 10);
                ey += parseInt(curStyle.top, 10);
                // Compensate for scrolling
                ex += document.body.scrollLeft;
                ey += document.body.scrollTop;
                // End the loop
                break;
            } else {
                ex += elt.offsetLeft;
                ey += elt.offsetTop;
            }
        } while (elt = elt.offsetParent);
        return { x: ex, y: ey };
    },

    // MouseDown handler -- sets up the appropriate mousemove/mouseup handlers
    // and fills in the global dragtable.dragObj object.
    dragStart: function (event, id) {
        var el;
        var x, y;
        var dragObj = dragtable.dragObj;

        var browser = dragtable.browser;
        if (browser.isIE)
            dragObj.origNode = window.event.srcElement;
        else
            dragObj.origNode = event.target;
        var pos = dragtable.eventPosition(event);

        // Drag the entire table cell, not just the element that was clicked.
        dragObj.origNode = dragtable.findUp(dragObj.origNode, /T[DH]/);

        // Since a column header can't be dragged directly, duplicate its contents
        // in a div and drag that instead.
        // TODO: I can assume a tHead...
        var table = dragtable.findUp(dragObj.origNode, "TABLE");
        dragObj.table = table;
        dragObj.startCol = dragtable.findColumn(table, pos.x);
        if (dragObj.startCol == -1)
            return;
        else
            dragtable.ColumnStartIndex = dragObj.startCol;

        var new_elt = dragtable.fullCopy(table, false);
        new_elt.style.margin = '0';

        // Copy the entire column
        var copySectionColumn = function (sec, col) {
            var new_sec = dragtable.fullCopy(sec, false);
            forEach(sec.rows, function (row) {
                var cell = row.cells[col];
                var new_tr = dragtable.fullCopy(row, false);
                if (row.offsetHeight) new_tr.style.height = row.offsetHeight + "px";
                var new_td = dragtable.fullCopy(cell, true);
                if (cell.offsetWidth) new_td.style.width = cell.offsetWidth + "px";
                new_tr.appendChild(new_td);
                new_sec.appendChild(new_tr);
            });
            return new_sec;
        };

        // First the heading
        if (table.tHead) {
            new_elt.appendChild(copySectionColumn(table.tHead, dragObj.startCol));
        }
        forEach(table.tBodies, function (tb) {
            new_elt.appendChild(copySectionColumn(tb, dragObj.startCol));
        });
        if (table.tFoot) {
            new_elt.appendChild(copySectionColumn(table.tFoot, dragObj.startCol));
        }

        var obj_pos = dragtable.absolutePosition(dragObj.origNode, true);
        new_elt.style.position = "absolute";
        new_elt.style.left = obj_pos.x + "px";
        new_elt.style.top = obj_pos.y + "px";
        new_elt.style.width = dragObj.origNode.offsetWidth + "px";
        new_elt.style.height = dragObj.origNode.offsetHeight + "px";
        new_elt.style.opacity = 0.7;

        // Hold off adding the element until this is clearly a drag.
        dragObj.addedNode = false;
        dragObj.tableContainer = dragObj.table.parentNode || document.body;
        dragObj.elNode = new_elt;

        // Save starting positions of cursor and element.
        dragObj.cursorStartX = pos.x;// + document.getElementById('cc_ha').scrollLeft;
        dragObj.cursorStartY = pos.y;
        dragObj.elStartLeft = parseInt(dragObj.elNode.style.left, 10);
        dragObj.elStartTop = parseInt(dragObj.elNode.style.top, 10);

        if (isNaN(dragObj.elStartLeft)) dragObj.elStartLeft = 0;
        if (isNaN(dragObj.elStartTop)) dragObj.elStartTop = 0;

        // Update element's z-index.
        dragObj.elNode.style.zIndex = ++dragObj.zIndex;

        // Capture mousemove and mouseup events on the page.
        if (browser.isIE) {
            document.attachEvent("onmousemove", dragtable.dragMove);
            document.attachEvent("onmouseup", dragtable.dragEnd);
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            document.addEventListener("mousemove", dragtable.dragMove, true);
            document.addEventListener("mouseup", dragtable.dragEnd, true);
            event.preventDefault();
        }


    },

    // Move the floating column header with the mouse
    // TODO: Reorder columns as the mouse moves for a more interactive feel.
    dragMove: function (event) {
        var x, y;
        var dragObj = dragtable.dragObj;

        // Get cursor position with respect to the page.
        var pos = dragtable.eventPosition(event);

        var dx = dragObj.cursorStartX - pos.x;
        var dy = dragObj.cursorStartY - pos.y;
        if (!dragObj.addedNode && dx * dx + dy * dy > dragtable.dragRadius2) {
            dragObj.tableContainer.insertBefore(dragObj.elNode, dragObj.table);
            dragObj.addedNode = true;
        }

        // Move drag element by the same amount the cursor has moved.
        var style = dragObj.elNode.style;
        style.left = (dragObj.elStartLeft + pos.x - dragObj.cursorStartX) + "px";
        style.top = (dragObj.elStartTop + pos.y - dragObj.cursorStartY) + "px";

        if (dragtable.browser.isIE) {
            window.event.cancelBubble = true;
            window.event.returnValue = false;
        } else {
            event.preventDefault();
        }
    },

    // Stop capturing mousemove and mouseup events.
    // Determine which (if any) column we're over and shuffle the table.
    dragEnd: function (event) {
        if (dragtable.browser.isIE) {
            document.detachEvent("onmousemove", dragtable.dragMove);
            document.detachEvent("onmouseup", dragtable.dragEnd);
        } else {
            document.removeEventListener("mousemove", dragtable.dragMove, true);
            //document.removeEventListener("mouseup", dragtable.dragEnd, true);
        }

        // If the floating header wasn't added, the mouse didn't move far enough.
        var dragObj = dragtable.dragObj;
        if (!dragObj.addedNode) {
            return;
        }

        dragObj.tableContainer.removeChild(dragObj.elNode);

        // Determine whether the drag ended over the table, and over which column.
        var pos = dragtable.eventPosition(event);
        var table_pos = dragtable.absolutePosition(dragObj.table);
        if (pos.y < table_pos.y || pos.y > table_pos.y + dragObj.table.offsetHeight) {
            return;
        }

        // Nếu có cột fixxed thì không cho kéo vào đấy
        var fixed_width = $(".DTFC_LeftHeadWrapper").width() == null ? 0 : $(".DTFC_LeftHeadWrapper").width();
        if (pos.x < fixed_width + table_pos.x)
            return;

        var targetCol = dragtable.findColumn(dragObj.table, pos.x);
        dragtable.ColumnStopIndex = targetCol;

        if (targetCol != -1 && targetCol != dragObj.startCol) {
            dragtable.moveColumn(dragObj.table, dragObj.startCol, targetCol);
            // KiemNH
            dragtable.TableDragMoveColumn();

            //if (dragObj.table.id && dragtable.cookiesEnabled() && -- mặc định
            //if (dragtable.user!="default" && dragtable.cookiesEnabled() && //đã  chỉnh sửa theo mục đích
            //              dragObj.table.className.search(/\bforget-ordering\b/) == -1) {
            //    //dragtable.rememberDrag(dragObj.table.id, dragObj.startCol, targetCol); -- mặc định
            //    dragtable.rememberDrag(dragObj.startCol, targetCol); //đã chỉnh sửa theo mục đích

            //}-------------------------------------------------------------------------------------------

            //lưu thứ tự các column vào cookie
            var colShow = "";
            var cols = jQuery("th", dragObj.table);
            for (var i = 0; i < cols.length; i++) {
                var cls = cols[i].getAttribute("class");
                if (cls != null) {
                    colShow += cls + ",";
                }
            }
            dragtable.createCookie("colShow_" + dragtable.user, colShow, 365);
        }
    },

    // Which column does the x value fall inside of? x should include scrollLeft.
    findColumn: function (table, x) {
        var header = table.tHead.rows[0].cells;
        var div_scroll = 0;
        var qqq1 = $(".dataTables_scrollHead").scrollLeft();
        var qqq2 = $(".tdragTable").parent().scrollLeft();
        // Nếu cái bảng chỉ có makeDrag thì lấy thẻ cha cái bảng
        if (qqq1 == null)
            div_scroll = qqq2;
        else {
            div_scroll = qqq1;
        }

        for (var i = 0; i < header.length; i++) {
            //var left = header[i].offsetLeft;
            var w = header[i].offsetWidth;
            var pos = dragtable.absolutePosition(header[i]);
            //if (left <= x && x <= left + header[i].offsetWidth) {
            if (pos.x - div_scroll <= x && x <= pos.x + header[i].offsetWidth - div_scroll) {
                return i;
            }
        }
        return -1;
    },

    // KiemNH ngày 10-07-2015
    // Hàm của nó bên dưới chi move đc cái bảng header fixed thôi
    // Move in table
    TableDragMoveColumn: function () {
        $(".dataTables_scrollBody .tdragTable").find("tr").each(function () {
            _column = $(this).find("th, td");
            if (dragtable.ColumnStartIndex > dragtable.ColumnStopIndex)
                _column.eq(dragtable.ColumnStartIndex).detach().insertBefore(_column.eq(dragtable.ColumnStopIndex));
            else
                _column.eq(dragtable.ColumnStartIndex).detach().insertAfter(_column.eq(dragtable.ColumnStopIndex));
        });
    },

    // Move a column of table from start index to finish index.
    // Based on the "Swapping table columns" discussion on comp.lang.javascript.
    // Assumes there are columns at sIdx and fIdx
    moveColumn: function (table, sIdx, fIdx) {
        var row, cA;
        var i = table.rows.length;
        while (i--) {
            row = table.rows[i]
            var x = row.removeChild(row.cells[sIdx]);
            if (fIdx < row.cells.length) {
                row.insertBefore(x, row.cells[fIdx]);
            } else {
                row.appendChild(x);
            }
        }

        // For whatever reason, sorttable tracks column indices this way.
        // Without a manual update, clicking one column will sort on another.
        var headrow = table.tHead.rows[0].cells;
        for (var i = 0; i < headrow.length; i++) {
            headrow[i].sorttable_columnindex = i;
        }
    },

    // Are cookies enabled? We should not attempt to set cookies on a local file.
    //cookiesEnabled: function () {
    //    return (window.location.protocol != 'file:') && navigator.cookieEnabled;
    //},

    //luu tài khoản của từng user đăng nhập( tài khoản này sẽ được dùng để lưu bảng dữ liệu trong cookie theo tung user)
    user: "default",
    setUsers: function (user) {
        dragtable.user = user;
    },

    // Store a column swap in a cookie for posterity.
    //rememberDrag: function(id, a, b) {
    rememberDrag: function (a, b) {
        //var cookieName = "dragtable-" + id;
        var cookieName = dragtable.user;
        var prev = dragtable.readCookie(cookieName);
        var new_val = "";
        if (prev) new_val = prev + ",";
        new_val += a + "/" + b;
        dragtable.createCookie(cookieName, new_val, dragtable.cookieDays);
    },

    // Replay all column swaps for a table.
    replayDrags: function (table) {
        if (!dragtable.cookiesEnabled()) return;
        var dragstr = dragtable.readCookie(dragtable.user);
        if (!dragstr) return;
        var drags = dragstr.split(',');
        for (var i = 0; i < drags.length; i++) {
            var pair = drags[i].split("/");
            if (pair.length != 2) continue;
            var a = parseInt(pair[0]);
            var b = parseInt(pair[1]);
            if (isNaN(a) || isNaN(b)) continue;
            dragtable.moveColumn(table, a, b);
        }
    },

    // Cookie functions based on http://www.quirksmode.org/js/cookies.html
    // Cookies won't work for local files.
    //cookiesEnabled: function () {
    //    return (window.location.protocol != 'file:') && navigator.cookieEnabled;
    //},

    createCookie: function (name, value, days) {
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            var expires = "; expires=" + date.toGMTString();
        }
        else var expires = "";
        //var path = document.location.pathname;
        //document.cookie = name + "=" + value + expires + "; path=" + path //luu cookie có path, ở 2 path khác nhau có thể có các cookie cùng tên
        document.cookie = name + "=" + value + expires;
    },

    readCookie: function (name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    },

    eraseCookie: function (name) {
        dragtable.createCookie(name, "", -1);
    }

}

/* ******************************************************************
   Supporting functions: bundled here to avoid depending on a library
   ****************************************************************** */

// Dean Edwards/Matthias Miller/John Resig
// has a hook for dragtable.init already been added? (see below)
var dgListenOnLoad = false;

/* for Mozilla/Opera9 */
if (document.addEventListener) {
    dgListenOnLoad = true;
    document.addEventListener("DOMContentLoaded", dragtable.init, false);
}

/* for Internet Explorer */
/*@cc_on @*/
/*@if (@_win32)
  dgListenOnLoad = true;
  document.write("<script id=__dt_onload defer src=//0)><\/script>");
  var script = document.getElementById("__dt_onload");
  script.onreadystatechange = function() {
    if (this.readyState == "complete") {
      dragtable.init(); // call the onload handler
    }
  };
/*@end @*/

/* for Safari */
if (/WebKit/i.test(navigator.userAgent)) { // sniff
    dgListenOnLoad = true;
    var _dgtimer = setInterval(function () {
        if (/loaded|complete/.test(document.readyState)) {
            dragtable.init(); // call the onload handler
        }
    }, 10);
}

/* for other browsers */
/* Avoid this unless it's absolutely necessary (it breaks sorttable) */
if (!dgListenOnLoad) {
    window.onload = dragtable.init;
}

// Dean's forEach: http://dean.edwards.name/base/forEach.js
/*
  forEach, version 1.0
  Copyright 2006, Dean Edwards
  License: http://www.opensource.org/licenses/mit-license.php
*/

// array-like enumeration
if (!Array.forEach) { // mozilla already supports this
    Array.forEach = function (array, block, context) {
        for (var i = 0; i < array.length; i++) {
            block.call(context, array[i], i, array);
        }
    };
}

// generic enumeration
Function.prototype.forEach = function (object, block, context) {
    for (var key in object) {
        if (typeof this.prototype[key] == "undefined") {
            block.call(context, object[key], key, object);
        }
    }
};

// character enumeration
String.forEach = function (string, block, context) {
    Array.forEach(string.split(""), function (chr, index) {
        block.call(context, chr, index, string);
    });
};

// globally resolve forEach enumeration
var forEach = function (object, block, context) {
    if (object) {
        var resolve = Object; // default
        if (object instanceof Function) {
            // functions have a "length" property
            resolve = Function;
        } else if (object.forEach instanceof Function) {
            // the object implements a custom forEach method so use that
            object.forEach(block, context);
            return;
        } else if (typeof object == "string") {
            // the object is a string
            resolve = String;
        } else if (typeof object.length == "number") {
            // the object is array-like
            resolve = Array;
        }
        resolve.forEach(object, block, context);
    }
};

function hideColumn(pClass) {
    //lay ten class cua the a 
    var tp = document.getElementById(pClass.id).checked;
    if (tp == false) {
        var name = $(pClass).attr('class').valueOf();
        tagName = name;
        //xóa column dua theo ten class cua column (ten class cua the a = ten class cua column tuong ung)
        $("th." + name + "").hide();
        $("td." + name + "").hide();
    } else {
        //$("th." + pClass.className + "").css("display", "block");
        $("th." + pClass.className + "").show();
        $("td." + pClass.className + "").show();
    }
}

////tagTablesId: ID của tables cần lấy
//function saveShowCol(tagTablesId) {
//    try {
//        var colShow = "";
//        var table = document.getElementById(tagTablesId);
//        var cols = table.getElementsByTagName("th");
//        for (var i = 0; i < cols.length; i++) {
//            var cls = cols[i].getAttribute("class");
//            var cssat = $(cols[i]).css("display");
//            if (cls != null) {
//                if (cssat == "none") {
//                    cssat = "0";
//                } else {
//                    cssat = "1";
//                }
//                colShow += cls + "|" + cssat +",";
//            }
//        }
//        //đẩy vào controller khi export dữ liệu ra 
//        return colShow;
//        //setCookie(tagTablesId, colShow, 365);
//    } catch (e) {
//        alert(e.message);
//        return "";
//    }
//}


function initDrag() {
    arguments.callee.done = true;
    if (_dgtimer) clearInterval(_dgtimer);
    if (!document.createElement || !document.getElementsByTagName) return;

    dragtable.dragObj.zIndex = 0;
    dragtable.browser = new dragtable.Browser();
    forEach(document.getElementsByTagName('table'), function (table) {
        if (table.className.search(/\bdraggable\b/) != -1) {
            //dragtable.makeDraggable(table);
        }

    });
}

//Create by Sangdd
//Date: 02-Feb-2015
//Quan ly thao tac voi Cookies

//=====================Cách lấy trong Controller =====================================
//if (Request.Cookies[cName] != null) {
//    string values = Request.Cookies[cName].Value;
//}
//=============================================================================

//Lưu Cookies trên trình duyệt giá trị lưu kiểu Hashtables Key -> Value
//Name: tên cần lưu đóng vai trò là Key
//Value: Giá trị cần lưu (value)
//days: Số ngày lưu trên trình duyệt quá số ngày này sẽ hết hạn
function setCookie(name, value, days) {
    try {
        var d = new Date();
        d.setTime(d.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = d.toUTCString();
        document.cookie = name + "=" + value + ";expires=" + expires;
    } catch (e) {
        alert(e.message);
    }
}

//Lấy thông tin từ Cookies
//cname : Key truyền vào lấy ra giá trinh
function getCookie(cname) {
    try {
        var name = cname + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            //lấy lần lượt các chuỗi cookie(bao gồm key và value) trong tập hợp cookies
            var c = ca[i];
            //xóa các khoảng trống đầu tiên có trong chuỗi(nếu có)
            while (c.charAt(0) == ' ') c = c.substring(1);
            //nếu key = cname thì trả sẽ về value của cookie tương ứng
            if (c.indexOf(name) != -1) return c.substring(name.length, c.length);
        }
        return "";
    } catch (e) {
        alert(e.message);
        return "";
    }
}

//xóa cookie trên trình duyệt
function removeCookie(name) {
    try {
        setCookie(name, "", -1);
    } catch (e) {
        alert(e.message);
    }
}

//Check Cookies
function checkCookie() {
    var username = getCookie("username");
    if (username != "") {
        alert("Welcome again " + username);
    } else {
        username = prompt("Please enter your name:", "");
        if (username != "" && username != null) {
            setCookie("username", username, 365);
        }
    }
}

//function SendConfigToExport() {
//    var _SearchId = $("#_txtId").val(); //id cua thang searchmaster
//    var _config=saveShowCol("tblthanhvien");
//    $.ajax({
//        type: "Get",
//        url: "/ModuleDynamicSearch/NewSearchView/GetConfigToExport",
//        dataType: "json",
//        data: { pConfig: _config, p_SearchId: _SearchId },
//        contentType: "application/json;charset:utf-8",
//        success: function (data) {
//            if (data.Rel == true) {
//                alert("Save File Successfull");
//            } else {
//                alert("Cannot save this file");
//            }
//        },
//        error: function () {
//            alert("Some Error!");
//        }
//    });
//}