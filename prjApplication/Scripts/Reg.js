var result;
var currYear = (new Date()).getFullYear();
var dates = {
    nowStr: function () {
        var date = new Date();
        var Month = date.getMonth() + 1;
        return date.getDate() + "/" + (Month < 10 ? "0" + Month : Month) + "/" + date.getFullYear();
    },

    dateStr: function (date) {
        var month = date.getMonth() + 1;
        var day = date.getDate();
        return (day < 10 ? "0" + day : day) + "/" + (month < 10 ? "0" + month : month) + "/" + date.getFullYear();
    },

    nowStrTime: function () {
        var date = new Date();
        var Month = date.getMonth() + 1;
        return dates.nowStr() + ' ' + date.getHours() + ':' + date.getMinutes();
    },

    convert: function (d) {
        return (
            d.constructor === Date ? d :
            d.constructor === Array ? new Date(d[0], d[1], d[2]) :
            d.constructor === Number ? new Date(d) :
            d.constructor === String ? new Date(d) :
            typeof d === "object" ? new Date(d.year, d.month, d.date) :
            NaN
        );
    },

    convertF: function (d) {
        if (d.constructor === String) {
            t = d.split('/');
            d = t[1] + '/' + t[0] + '/' + t[2];
        }

        return (
            d.constructor === Date ? d :
            d.constructor === Array ? new Date(d[0], d[1], d[2]) :
            d.constructor === Number ? new Date(d) :
            d.constructor === String ? new Date(d) :
            typeof d === "object" ? new Date(d.year, d.month, d.date) :
            NaN
        );
    },

    compare: function (a, b) {
        // Compare two dates (could be of any type supported by the convert
        // function above) and returns:
        //  -1 : if a < b
        //   0 : if a = b
        //   1 : if a > b
        // NaN : if a or b is an illegal date
        // NOTE: The code inside isFinite does an assignment (=).
        return (
            isFinite(a = this.convert(a).valueOf()) &&
            isFinite(b = this.convert(b).valueOf()) ?
            (a > b) - (a < b) :
            NaN
        );
    },

    inRange: function (d, start, end) {
        // Checks if date in d is between dates in start and end.
        // Returns a boolean or NaN:
        //    true  : if d is between start and end (inclusive)
        //    false : if d is before start or after end
        //    NaN   : if one or more of the dates is illegal.
        // NOTE: The code inside isFinite does an assignment (=).
        return (
             isFinite(d = this.convert(d).valueOf()) &&
             isFinite(start = this.convert(start).valueOf()) &&
             isFinite(end = this.convert(end).valueOf()) ?
             start <= d && d <= end :
             NaN
         );
    }
};

function addDays(startDate, numberOfDays) {
    return new Date(startDate.getTime() + (numberOfDays * 24 * 60 * 60 * 1000));
}

function formatDate(d) {
    var dd = d.getDate()
    if (dd < 10) dd = '0' + dd

    var mm = d.getMonth() + 1
    if (mm < 10) mm = '0' + mm

    var yy = d.getFullYear()
    if (yy < 10) yy = '0' + yy

    return dd + '/' + mm + '/' + yy;
}

function isDate(txtDate, rxDatePattern) {
    if (currVal == '') {
        return true;
    } else {
        var currVal = txtDate;
        //var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
        if (!currVal.match(rxDatePattern)) {
            return false;
        } else {
            var dtArray = currVal.match(rxDatePattern); // is format OK?

            if (dtArray == null)
                return false;

            //Checks for mm/dd/yyyy format.
            dtDay = dtArray[1];
            dtMonth = dtArray[3];
            dtYear = dtArray[5];
            if (!dtDay || !dtMonth || !dtYear) {
                return false;
            } else {
                if (dtMonth < 1 || dtMonth > 12)
                    return false;
                else if (dtDay < 1 || dtDay > 31)
                    return false;
                else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
                    return false;
                else if (dtMonth == 2) {
                    var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                    if (dtDay > 29 || (dtDay == 29 && !isleap))
                        return false;
                }
            }
        }
        return true;
    }
}


function AjaxRequestURL(url) {
    var xmlHttp;
    try {
        if (!document.all) { xmlHttp = new XMLHttpRequest() }
        else { try { xmlHttp = new ActiveXObject("Msxml2.XMLHTTP") } catch (e) { xmlHttp = new ActiveXObject("Microsoft.XMLHTTP") } }
    }
    catch (e) {
        alert("Your browser does not support AJAX!");
        return false
    };
    xmlHttp.onreadystatechange = function () {
        if (xmlHttp.readyState == 4) {
            result = xmlHttp.responseText;

        }
    }; xmlHttp.open("GET", url, false); xmlHttp.send(null)
}

function LoadData(url) {
    AjaxRequestURL(url);
}

$(function () {
    $('#txtEmail').change(function () {
        var email = $('#txtEmail').val().trim();
        validateEmail(email);
    });
    $('#txtDienThoai').change(function () {
        var mobile = $('#txtDienThoai').val().trim();
        validateMobile(mobile);
    });
});
/*
$(function () {
    $('#txtClassID').datepicker({
        dateFormat: 'mm/yy'
    });
});
$(function () {
    $('#txtClassID1').datepicker({
        dateFormat: 'mm/yy'

    });
});
*/


$(function () {
    $('#txtClassID').mask("99/9999", { placeholder: "__/____" });
    $('#txtClassID1').mask("99/9999", { placeholder: "__/____" });
    $('#txtNgaySinh').mask("99/99/9999", { placeholder: "__/__/____" });
    $('#txtNgaySinh').datepicker({
        dateFormat: 'dd/mm/yy',
        onSelect: function (date) {
            var fdate = $('#txtNgaySinh').val().trim();
            validateBirthday(fdate);
            var _value = $('#ddlUuTien').val();
            if (_value == 1) {
                var d = new Date();
                var valDate = dates.convertF(fdate);

                var ds = addDays(d, 1);

                $('#txtClassID').val(dates.dateStr(ds).substring(3, fdate.length));
                var age = d.getFullYear() - valDate.getFullYear()
                var ischeck = 18 - age
                if (ischeck >= 0) {
                    var y1 = 0;
                    if (ischeck > 5)
                        y1 = d.getFullYear() + 5;
                    else
                        y1 = d.getFullYear() + ischeck

                    $('#txtClassID1').val("12/" + y1);
                }
                else {
                    var y = d.getFullYear();
                    $('#txtClassID1').val("12/" + y);
                }
            }
            else
                return;
        }

    });
    $('#txtNgaySinh').change(function () {
        var fdate = $('#txtNgaySinh').val().trim();
        validateBirthday(fdate);
        var _value = $('#ddlUuTien').val();

        if (_value == 1) {
            var d = new Date();
            var valDate = dates.convertF(fdate);
            var ds = addDays(d, 1);

            $('#txtClassID').val(dates.dateStr(ds).substring(3, fdate.length));


            var age = d.getFullYear() - valDate.getFullYear()
            var ischeck = 18 - age
            if (ischeck >= 0) {
                var y1 = 0;
                if (ischeck > 5)
                    y1 = d.getFullYear() + 5;
                else
                    y1 = d.getFullYear() + ischeck

                $('#txtClassID1').val("12/" + y1);
            }
            else {
                var y = d.getFullYear();
                $('#txtClassID1').val("12/" + y);
            }
        }
    });

});
/*
$(function () {

    $('#txtNgayNhanThe').mask("99/99/9999", { placeholder: "__/__/____" });
    $('#txtNgayNhanThe').datepicker({
        dateFormat: 'dd/mm/yy',

        onSelect: function (date) {
            var fdate = $('#txtNgayNhanThe').val().trim();
            validateReceiverDate(fdate);
        }
    });
    $('#txtNgayNhanThe').change(function () {
        var fdate = $('#txtNgayNhanThe').val().trim();
        validateReceiverDate(fdate);
    });

});*/
function show(id) {
    $(id).css("display", "block");
}
function hide(id) {
    $(id).css("display", "none");
}
function showhocsinhsv(id) {
    var _value = id.value;
    if (_value == "1") {
        $("#divhocsinhsinhvien").css("display", "block");
        $("#divKhucongnghiep").css("display", "none");
        $("#divbinhthuong").css("display", "none");
        $("#txtClassID").prop("disabled", true);
        $("#txtClassID1").prop("disabled", true);
    }
    else if (_value == "2") {
        $("#divhocsinhsinhvien").css("display", "none");
        $("#divKhucongnghiep").css("display", "none");
        $("#divbinhthuong").css("display", "block");

    }
    else if (_value == "3") {
        $("#divKhucongnghiep").css("display", "block");
        $("#divhocsinhsinhvien").css("display", "none");
        $("#divbinhthuong").css("display", "none");
    }
    else if (_value == "4") {
        $("#divhocsinhsinhvien").css("display", "block");
        $("#divKhucongnghiep").css("display", "none");
        $("#divbinhthuong").css("display", "none");
        $("#txtClassID").prop("disabled", false);
        $("#txtClassID1").prop("disabled", false);
    }
}
function BinhThuong() {
    $("#divhocsinhsinhvien").css("display", "none");
    $("#divbinhthuong").css("display", "block");
    $("#divKhucongnghiep").css("display", "none");
    $("#divuutien").css("display", "none");
}
function UuTien() {
    $("#divuutien").css("display", "block");
    $("#divKhucongnghiep").css("display", "none");
    $("#divhocsinhsinhvien").css("display", "none");
    $("#divbinhthuong").css("display", "none");
}
function NhanThongtin(id) {

    if (id.checked) {

        $("#divNhanThongTin").css("display", "block");
    }
    else {

        $("#divNhanThongTin").css("display", "none");
    }
}
function validateEmail(email) {

    var re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    if (email.length > 0) {
        if (re.test(email) == false) {
            alert("Sai định dạng email"); $('#txtEmail').val(""); $('#txtEmail').focus(); return false;
        }
    }
}
function validateMobile(mobile) {
    var regex_pattern_mobile = /^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$/im;
    if (mobile.length > 0) {
        if (regex_pattern_mobile.test(mobile) == false) {
            alert("Sai định dạng, chỉ nhập số từ 0-9 cho số điện thoại"); $('#txtDienThoai').val(""); $('#txtDienThoai').focus(); return false;
        }
    }
}/*
function validateReceiverDate(fdate) {
    if (fdate.length == 0)
        return;
    else {

        dd = fdate.substr(0, 2).split("_");
        mm = fdate.substr(3, 2).split("_");
        yy = fdate.substr(6, 5);

        if (dd[0] == "") { dd = "0" + dd[1]; if (dd == "00") { dd = "01"; } }
        if (dd[1] == "") { dd = "0" + dd[0]; if (dd == "00") { dd = "01"; } }
        if (mm[0] == "") { mm = "0" + mm[1]; if (mm == "00") { mm = "01"; } }
        if (mm[1] == "") { mm = "0" + mm[0]; if (mm == "00") { mm = "01"; } }
        if (dd == "00") { dd = "01"; }
        if (mm == "00") { mm = "01"; }

        var m = parseInt(mm);
        var d = parseInt(dd);
        var y = parseInt(yy);

        if (parseInt(yy) < currYear) { yy = currYear; }
        if (parseInt(yy) > currYear + 1) { yy = currYear + 1; }
        if (m > 12) { mm = "12"; m = parseInt(mm); }
        if (d > 31) { dd = "31"; d = parseInt(dd); }
        if (m == 2) {
            var isleap = (y % 4 == 0 && (y % 100 != 0 || y % 400 == 0));
            if (isleap && d > 29) { dd = "29"; d = parseInt(dd); }
            if (!isleap && d > 28) { dd = "28"; d = parseInt(dd); }
        }
        if ((m == 4 || m == 6 || m == 9 || m == 11) && d > 30) { dd = "30"; d = parseInt(dd); }
        fdate = dd + "/" + mm + "/" + yy;

        var valDate = dates.convertF(fdate);
        var d = new Date();
        var ds = addDays(d, 1);
        var de = addDays(d, 30);

        if (dates.compare(valDate, ds) < 0)
            $('#txtNgayNhanThe').val(dates.dateStr(ds));
            //$('#txtNgayNhanThe').val(fdate);
        else if (dates.compare(valDate, de) > 0)
            $('#txtNgayNhanThe').val(dates.dateStr(de));
        else
            $('#txtNgayNhanThe').val(fdate);
    }
}
*/

function validateBirthday(fdate) {
    if (fdate.length == 0)
        return;
    else {
        dd = fdate.substr(0, 2).split("_");
        mm = fdate.substr(3, 2).split("_");
        yy = fdate.substr(6, 5);

        if (dd[0] == "") { dd = "0" + dd[1]; if (dd == "00") { dd = "01"; } }
        if (dd[1] == "") { dd = "0" + dd[0]; if (dd == "00") { dd = "01"; } }
        if (mm[0] == "") { mm = "0" + mm[1]; if (mm == "00") { mm = "01"; } }
        if (mm[1] == "") { mm = "0" + mm[0]; if (mm == "00") { mm = "01"; } }
        if (dd == "00") { dd = "01"; }
        if (mm == "00") { mm = "01"; }

        var m = parseInt(mm);
        var d = parseInt(dd);
        var y = parseInt(yy);

        if (parseInt(yy) > (currYear - 6)) { yy = (currYear - 6); }
        if (parseInt(yy) < (currYear - 100)) { yy = (currYear - 100); }
        if (m > 12) { mm = "12"; m = parseInt(mm); }
        if (d > 31) { dd = "31"; d = parseInt(dd); }
        if (m == 2) {
            var isleap = (y % 4 == 0 && (y % 100 != 0 || y % 400 == 0));
            if (isleap && d > 29) { dd = "29"; d = parseInt(dd); }
            if (!isleap && d > 28) { dd = "28"; d = parseInt(dd); }
        }
        if ((m == 4 || m == 6 || m == 9 || m == 11) && d > 30) { dd = "30"; d = parseInt(dd); }

        fdate = dd + "/" + mm + "/" + yy;

        $('#txtNgaySinh').val(fdate);
    }
}


function validatedate(inputText) {

    if (inputText.value.length > 0) {
        var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/;
        if (inputText.value.match(dateformat)) {


            var opera1 = inputText.value.split('/'); var opera2 = inputText.value.split('-'); lopera1 = opera1.length; lopera2 = opera2.length; if (lopera1 > 1)
            { var pdate = inputText.value.split('/'); }
            else if (lopera2 > 1)
            { var pdate = inputText.value.split('-'); }
            var dd = parseInt(pdate[0]); var mm = parseInt(pdate[1]); var yy = parseInt(pdate[2]); var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]; if (mm == 1 || mm > 2) {
                if (dd > ListofDays[mm - 1])
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); inputText.value = ""; inputText.focus(); return false; }
            }
            if (mm == 2) {
                var lyear = false; if ((!(yy % 4) && yy % 100) || !(yy % 400))
                { lyear = true; }
                if ((lyear == false) && (dd >= 29))
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); inputText.value = ""; inputText.focus(); return false; }
                if ((lyear == true) && (dd > 29))
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); inputText.value = ""; inputText.focus(); return false; }
            }
        }
        else { alert("Sai định dạng ngày tháng (dd/mm/yyyy)!"); inputText.value = ""; inputText.focus(); return false; }
    }
}

function Checkdate(input) {

    var inputText = document.getElementById(input);
    if (inputText.value.length > 0) {
        var dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$/; if (inputText.value.match(dateformat)) {


            var opera1 = inputText.value.split('/'); var opera2 = inputText.value.split('-'); lopera1 = opera1.length; lopera2 = opera2.length; if (lopera1 > 1)
            { var pdate = inputText.value.split('/'); }
            else if (lopera2 > 1)
            { var pdate = inputText.value.split('-'); }
            var dd = parseInt(pdate[0]); var mm = parseInt(pdate[1]); var yy = parseInt(pdate[2]); var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]; if (mm == 1 || mm > 2) {
                if (dd > ListofDays[mm - 1])
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); return false; }
            }
            if (mm == 2) {
                var lyear = false; if ((!(yy % 4) && yy % 100) || !(yy % 400))
                { lyear = true; }
                if ((lyear == false) && (dd >= 29))
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); return false; }
                if ((lyear == true) && (dd > 29))
                { alert('Sai định dạng ngày tháng (dd/mm/yyyy)!'); return false; }
            }
        }
        else { alert("Sai định dạng ngày tháng (dd/mm/yyyy)!"); return false; }
    }
}


$(function () {
    $.widget("custom.combobox", {
        _create: function () {

            this.wrapper = $("<span>")
              .addClass("custom-combobox")

              .insertAfter(this.element);
            this.element.hide();

            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
              value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
              .appendTo(this.wrapper)
              .val(value)
              .attr("id", "txtInputDiaChi")/*Lưu y trường hợp nếu muốn sử dụng chi tiết cho từng đối tượng*/
              .attr("title", "")
              .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
              .autocomplete({
                  delay: 0,
                  minLength: 0,
                  source: $.proxy(this, "_source")
              })
              .tooltip({
                  classes: {
                      "ui-tooltip": "ui-state-highlight"
                  }
              });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
              wasOpen = false;

            $("<a>")
              .attr("tabIndex", -1)
              .attr("title", "Chọn trong danh sách")
              .tooltip()
              .appendTo(this.wrapper)
              .button({
                  icons: {
                      primary: "ui-icon-triangle-1-s"
                  },
                  text: false
              })
              .removeClass("ui-corner-all")
              .addClass("custom-combobox-toggle ui-corner-right")
              .on("mousedown", function () {
                  wasOpen = input.autocomplete("widget").is(":visible");
              })
              .on("click", function () {
                  input.trigger("focus");

                  // Close if already visible
                  if (wasOpen) {
                      return;
                  }

                  // Pass empty string as value to search for, displaying all results
                  input.autocomplete("search", "");
              });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
              valueLowerCase = value.toLowerCase(),
              valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
              .val("Khác")
              .attr("title", "Địa chỉ  '" + value + " này'" + " không có trong danh sách. Bạn hãy để thông tin là 'Khác'")
              .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
    $("#ddldiachi").combobox();
    $("#toggle").on("click", function () {
        $("#ddldiachi").toggle();
    });
});

$(function () {
    $.widget("custom.combobox", {
        _create: function () {

            this.wrapper = $("<span>")
              .addClass("custom-combobox")

              .insertAfter(this.element);
            this.element.hide();

            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
              value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
              .appendTo(this.wrapper)
              .val(value)
              .attr("id", "txtInputMotTuyen")
              .attr("title", "")
              .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
              .autocomplete({
                  delay: 0,
                  minLength: 0,
                  source: $.proxy(this, "_source")
              })
              .tooltip({
                  classes: {
                      "ui-tooltip": "ui-state-highlight"
                  }
              });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
              wasOpen = false;

            $("<a>")
              .attr("tabIndex", -1)
              .attr("title", "Chọn trong danh sách")
              .tooltip()
              .appendTo(this.wrapper)
              .button({
                  icons: {
                      primary: "ui-icon-triangle-1-s"
                  },
                  text: false
              })
              .removeClass("ui-corner-all")
              .addClass("custom-combobox-toggle ui-corner-right")
              .on("mousedown", function () {
                  wasOpen = input.autocomplete("widget").is(":visible");
              })
              .on("click", function () {
                  input.trigger("focus");

                  // Close if already visible
                  if (wasOpen) {
                      return;
                  }

                  // Pass empty string as value to search for, displaying all results
                  input.autocomplete("search", "");
              });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
              valueLowerCase = value.toLowerCase(),
              valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
              .val("")
               .attr("title", "Tuyến '" + value + " này'" + " không có trong danh sách. Bạn hãy chọn một tuyến có trong danh sách")
              .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
    $("#ddlMotTuyen").combobox();
    $("#toggle").on("click", function () {
        $("#ddlMotTuyen").toggle();
    });
});

$(function () {
    $.widget("custom.combobox", {
        _create: function () {

            this.wrapper = $("<span>")
              .addClass("custom-combobox")

              .insertAfter(this.element);
            this.element.hide();

            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
              value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
              .appendTo(this.wrapper)
              .val(value)
              .attr("id", "txtInputTruongHoc")
              .attr("title", "")
              .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
              .autocomplete({
                  delay: 0,
                  minLength: 0,
                  source: $.proxy(this, "_source")
              })
              .tooltip({
                  classes: {
                      "ui-tooltip": "ui-state-highlight"
                  }
              });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
              wasOpen = false;

            $("<a>")
              .attr("tabIndex", -1)
              .attr("title", "Chọn trong danh sách")
              .tooltip()
              .appendTo(this.wrapper)
              .button({
                  icons: {
                      primary: "ui-icon-triangle-1-s"
                  },
                  text: false
              })
              .removeClass("ui-corner-all")
              .addClass("custom-combobox-toggle ui-corner-right")
              .on("mousedown", function () {
                  wasOpen = input.autocomplete("widget").is(":visible");
              })
              .on("click", function () {
                  input.trigger("focus");

                  // Close if already visible
                  if (wasOpen) {
                      return;
                  }

                  // Pass empty string as value to search for, displaying all results
                  input.autocomplete("search", "");
              });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
              valueLowerCase = value.toLowerCase(),
              valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
              .val("Khác")
              .attr("title", "Trường học/Cơ quan '" + value + " này '" + " không có trong danh sách. Bạn hãy để thông tin là 'Khác'")
              .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
    $("#ddlTruongHoc").combobox();
    $("#toggle").on("click", function () {
        $("#ddlTruongHoc").toggle();
    });
});


$(function () {
    $.widget("custom.combobox", {
        _create: function () {

            this.wrapper = $("<span>")
              .addClass("custom-combobox")

              .insertAfter(this.element);
            this.element.hide();

            this._createAutocomplete();
            this._createShowAllButton();
        },

        _createAutocomplete: function () {
            var selected = this.element.children(":selected"),
              value = selected.val() ? selected.text() : "";

            this.input = $("<input>")
              .appendTo(this.wrapper)
              .val(value)
              .attr("id", "txtInputKhucongnghiep")
              .attr("title", "")
              .addClass("custom-combobox-input ui-widget ui-widget-content ui-state-default ui-corner-left")
              .autocomplete({
                  delay: 0,
                  minLength: 0,
                  source: $.proxy(this, "_source")
              })
              .tooltip({
                  classes: {
                      "ui-tooltip": "ui-state-highlight"
                  }
              });

            this._on(this.input, {
                autocompleteselect: function (event, ui) {
                    ui.item.option.selected = true;
                    this._trigger("select", event, {
                        item: ui.item.option
                    });
                },

                autocompletechange: "_removeIfInvalid"
            });
        },

        _createShowAllButton: function () {
            var input = this.input,
              wasOpen = false;

            $("<a>")
              .attr("tabIndex", -1)
              .attr("title", "Chọn trong danh sách")
              .tooltip()
              .appendTo(this.wrapper)
              .button({
                  icons: {
                      primary: "ui-icon-triangle-1-s"
                  },
                  text: false
              })
              .removeClass("ui-corner-all")
              .addClass("custom-combobox-toggle ui-corner-right")
              .on("mousedown", function () {
                  wasOpen = input.autocomplete("widget").is(":visible");
              })
              .on("click", function () {
                  input.trigger("focus");

                  // Close if already visible
                  if (wasOpen) {
                      return;
                  }

                  // Pass empty string as value to search for, displaying all results
                  input.autocomplete("search", "");
              });
        },

        _source: function (request, response) {
            var matcher = new RegExp($.ui.autocomplete.escapeRegex(request.term), "i");
            response(this.element.children("option").map(function () {
                var text = $(this).text();
                if (this.value && (!request.term || matcher.test(text)))
                    return {
                        label: text,
                        value: text,
                        option: this
                    };
            }));
        },

        _removeIfInvalid: function (event, ui) {

            // Selected an item, nothing to do
            if (ui.item) {
                return;
            }

            // Search for a match (case-insensitive)
            var value = this.input.val(),
              valueLowerCase = value.toLowerCase(),
              valid = false;
            this.element.children("option").each(function () {
                if ($(this).text().toLowerCase() === valueLowerCase) {
                    this.selected = valid = true;
                    return false;
                }
            });

            // Found a match, nothing to do
            if (valid) {
                return;
            }

            // Remove invalid value
            this.input
              .val("Khác")
               .attr("title", "Khu công nghiệp '" + value + " này'" + " không có trong danh sách. Bạn hãy để thông tin là 'Khác'")
              .tooltip("open");
            this.element.val("");
            this._delay(function () {
                this.input.tooltip("close").attr("title", "");
            }, 2500);
            this.input.autocomplete("instance").term = "";
        },

        _destroy: function () {
            this.wrapper.remove();
            this.element.show();
        }
    });
    $("#ddlKhucongnghiep").combobox();
    $("#toggle").on("click", function () {
        $("#ddlKhucongnghiep").toggle();
    });
});

$(function () {
    $.ajax({
        type: "POST",
        url: "RegOnline.aspx/GetRoutes",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlCustomers = $("#ddlMotTuyen");
            //ddlCustomers.empty().append('<option selected="selected" value="0">Chọn tuyến đăng ký...</option>');
            $.each(r.d, function () {
                ddlCustomers.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
});

/*Địa chỉ*/

$(function () {
    $.ajax({
        type: "POST",
        url: "RegOnline.aspx/GetDiaChi",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddldiachi = $("#ddldiachi");
            //ddldiachi.empty().append('<option selected="selected" value="0">Chọn địa chỉ...</option>');
            $.each(r.d, function () {
                ddldiachi.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
});

/*Khu công nghiệp*/

$(function () {
    $.ajax({
        type: "POST",
        url: "RegOnline.aspx/GettblKCN",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlKhucongnghiep = $("#ddlKhucongnghiep");
            //ddlKhucongnghiep.empty().append('<option selected="selected" value="0">Chọn địa chỉ...</option>');
            $.each(r.d, function () {
                ddlKhucongnghiep.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
});

/*Trường học*/

$(function () {
    $.ajax({
        type: "POST",
        url: "RegOnline.aspx/GettblTruongHoc",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlTruongHoc = $("#ddlTruongHoc");
            // ddlTruongHoc.empty().append('<option selected="selected" value="0">Chọn địa chỉ...</option>');
            $.each(r.d, function () {
                ddlTruongHoc.append($("<option></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
});
/*Điểm nhận thẻ*/

$(function () {
    $.ajax({
        type: "POST",
        url: "RegOnline.aspx/GettblStation",
        data: '{}',
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (r) {
            var ddlDiemNhanThe = $("#ddlDiemNhanThe");
            //ddlDiemNhanThe.empty().append('<option selected="selected" value="0">Chọn địa chỉ...</option>');
            $.each(r.d, function () {
                ddlDiemNhanThe.append($("<option selected='selected'></option>").val(this['Value']).html(this['Text']));
            });
        }
    });
});

function Find() {

    var txtMa = $("#txtMa").val();
    if (txtMa == '') {
        alert('Bạn phải nhập mã');
        document.getElementById('txtMa').focus();
        return;
    }
    var param = { 'code': txtMa };
    $.ajax({
        type: "POST",
        url: "Find.aspx/FindTicket",
        data: JSON.stringify(param),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (result) {
            if (result.d == "") {
                $("#txtMa").val("");
                $("#divTrangThaiNhanThe").text("");
                $("#divDiemnhanthe").text("");
                $("#divthoigiannhanthe").text("");
                alert("Không tìm thấy kết quả");
            }
            else {
                var arr = result.d.split(',');
                $("#divTrangThaiNhanThe").text(arr[0]);
                $("#divDiemnhanthe").text(arr[1]);
                $("#divthoigiannhanthe").text(arr[2]);
            }
            console.log(result);
        },

        error: function (xmlhttprequest, textstatus, errorthrown) {
            alert(" conection to the server failed ");
            console.log("error: " + errorthrown);
        }
    });
}
function Save() {
    var DT;
    var DoiTuong;
    if ($("#chkDoiTuong_1").is(":checked"))
    { DoiTuong = "BT"; } else { DoiTuong = "UT"; }
    var _value = $('#ddlUuTien').val();
    if (DoiTuong == "BT") { DT = "0" }
    else { DT = _value };
    var CardTypeID;
    if ($("#chkLoaiThe_1").is(":checked"))
    { CardTypeID = "BT1"; } else { CardTypeID = "BT"; }


    if (DoiTuong == 'BT' && CardTypeID == 'BT1') {
        CardTypeID = 'BT1'
    }
    if (DoiTuong == 'BT' && CardTypeID == 'BT') {
        CardTypeID = 'BT'
    }

    if (DoiTuong == 'UT' && CardTypeID == 'BT1') {
        CardTypeID = 'UT1'
    }
    if (DoiTuong == 'UT' && CardTypeID == 'BT') {
        CardTypeID = 'UT'
    }




    if (DoiTuong == 'UT' && _value == "0") {
        alert('Bạn phải chọn loại đổi tượng');
        document.getElementById('ddlUuTien').focus();
        return;
    }

    var InputMotTuyen = $('#txtInputMotTuyen').val();

    if (CardTypeID == 'BT1') {
        if (InputMotTuyen == "") {
            alert('Bạn phải chọn tuyến');
            document.getElementById('ddlMotTuyen').focus();
            return;
        }
    }

    if (CardTypeID == 'UT1') {
        if (InputMotTuyen == "") {
            alert('Bạn phải chọn tuyến');
            document.getElementById('ddlMotTuyen').focus();
            return;
        }
    }
    var RouteID = '0'
    if ($("#chkLoaiThe_1").is(":checked")) {
        RouteID = $('#ddlMotTuyen').val();
        if (InputMotTuyen == 'Khác') { RouteID = '000'; }
    }


    var CustomerName = $('#txtHoTen').val();
    if (CustomerName == '') {
        alert('Bạn phải nhập đầy đủ họ tên');
        document.getElementById('txtHoTen').focus();
        return;
    }
    var DateBirth = $('#txtNgaySinh').val();
    if (DateBirth == '') {
        alert('Bạn phải nhập ngày sinh');
        document.getElementById('txtNgaySinh').focus();
        return;
    }

    if (DoiTuong == 'BT' && $('#ddldiachi').val() == '0') {
        alert('Bạn chọn địa chỉ');
        document.getElementById('ddldiachi').focus();
        return;
    }

    var OfficeID = '0';
    var PlaceID = '0';
    var Address = '0';
    Address = $('#txtDiaChiKhac').val();
    var ClassID = $('#txtClassID').val();
    var InputTruongHoc = $('#txtInputTruongHoc').val();
    var InputDiaChi = $('#txtInputDiaChi').val();
    var InputKhucongnghiep = $('#txtInputKhucongnghiep').val();


    if (_value == "1") {
        PlaceID = '0';
        OfficeID = $('#ddlTruongHoc').val();
        if (InputTruongHoc == 'Khác') { OfficeID = '000'; }

    }
    else if (_value == "4") {
        PlaceID = '0';
        OfficeID = $('#ddlTruongHoc').val();
        if (InputTruongHoc == 'Khác') { OfficeID = '000'; }
    }
    else if (_value == "2") {
        PlaceID = $('#ddldiachi').val();
        if (InputDiaChi == 'Khác') { PlaceID = '000'; }
        OfficeID = '0'; ClassID = '';
        DoiTuong = 'CT'
    }
    else if (_value == "3") {
        PlaceID = '0';
        OfficeID = $('#ddlKhucongnghiep').val(); ClassID = ''
        if (InputKhucongnghiep == 'Khác') { OfficeID = '000'; }
    }
    else {
        PlaceID = $('#ddldiachi').val(); OfficeID = '0';
        if (InputDiaChi == 'Khác') { PlaceID = '000'; }
    }

    if (_value == "1" && OfficeID == '0') {
        alert('Bạn phải chọn trường học');
        document.getElementById('ddlTruongHoc').focus();
        return;
    }
    if (_value == "2" && PlaceID == '0') {
        alert('Bạn phải chọn địa chỉ');
        document.getElementById('ddlTruongHoc').focus();
        return;
    }
    if (_value == "3" && OfficeID == '0') {
        alert('Bạn phải  chọn KCN');
        document.getElementById('ddlKhucongnghiep').focus();
        return;
    }

    var StationID = $('#ddlDiemNhanThe').val();
    var Mobile = $('#txtDienThoai').val();
    if (Mobile == '') {
        alert('Bạn phải nhập số điện thoại');
        document.getElementById('txtDienThoai').focus();
        return;
    }
    var d = new Date();
    var ds = addDays(d, 1);

    var TimeDelivery = dates.dateStr(ds);// $('#txtNgayNhanThe').val();

    //if (TimeDelivery == '') {
    //    alert('Bạn phải nhập ngày nhận thẻ');
    //    document.getElementById('txtNgayNhanThe').focus();
    //    return;
    //}

    var Email = $('#txtEmail').val();

    var RegType = $('#ddlRegType').val();
    var ClassID1 = $('#txtClassID1').val();


    var url = 'Reg.ashx?sig=' + SigId + '&DoiTuong=' + DoiTuong + '&CardTypeID=' + CardTypeID + '&RouteID=' + RouteID + '&CustomerName=' + CustomerName + '&DateBirth=' + DateBirth + '&PlaceID=' + PlaceID + '&OfficeID=' + OfficeID + '&Address=' + Address + '&StationID=' + StationID + '&Mobile=' + Mobile + '&TimeDelivery=' + TimeDelivery + '&ClassID=' + ClassID + '&Email=' + Email + '&RegType=' + RegType + '&ClassID1=' + ClassID1 + '&DT=' + DT;

    LoadData(url);

    if (result != 'Exists') window.open('RegSuccessful.aspx?obj=' + DT + '&TicketCode=' + result, '_parent');

}


