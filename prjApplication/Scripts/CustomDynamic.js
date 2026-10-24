var elems = document.body.getElementsByTagName('html');
var classList = [];
var lis = ['mgt', 'mgr', 'mgb', 'mgl', 'pdt', 'pdr', 'pdb', 'pdl', 'wid', 'hei']
var kq = '';
function getAllClassCSS() {
    var elements, i, results = [], curClass;
    elements = document.getElementsByTagName('*');
    for (i = 0; i < elements.length; i++) {
        try {
            curClass = elements[i].getAttribute('class');
            if (curClass != null || typeof (curClass) !== undefined) {
                curClass = curClass.split(" ");
                for (j = 0; j < curClass.length; j++) {
                    if (curClass[j].length > 3)
                        if (lis.indexOf(curClass[j].substring(0, 3)) >= 0) {
                            if (results.indexOf(curClass[j]) < 0)
                                results.push(curClass[j]);
                        }
                }
            }
        } catch (ex) {
        }
    }
    return results;
};
classList = getAllClassCSS();
for (i = 0; i <= classList.length - 1; i++) {
    var cssName = classList[i];
    var value = classList[i].substring(4);
    var value2 = '';
    if (value.indexOf('px') > 0)
        value2 = value;
    else value2 = value + '%';
    var mar = ['mg', 't', 'r', 'b', 'l'];
    var param = ['', 'top', 'right', 'bottom', 'left']
    var txt = '';
    var ali = '';
    if (cssName.substring(0, 3) == 'wid') {
        txt = 'width: ' + value2 + '!important';
        createClassCSS(cssName, txt);

    }
    else if (cssName.substring(0, 3) == 'hei') {
        txt = 'height: ' + value2 + '!important';
        createClassCSS(cssName, txt);

    }
    else ali = param[mar.indexOf(cssName.substring(2, 3))].toString();
    var abs = '';
    if (cssName.substring(3, 4) == '-')
        abs = '-';
    else if (cssName.substring(3, 4) == '_') abs = '';
    //else break;
    if (mar.indexOf(cssName.substring(0, 2)) == 0) {
        txt = 'margin-' + ali + ': ' + abs + value + '!important';
        createClassCSS(cssName, txt);
    }
    else if (cssName.substring(0, 2) == 'pd') {
        txt = 'padding-' + ali + ': ' + abs + value + '!important';
        createClassCSS(cssName, txt)
    }


};

function preloadImg(containerId, preId, preWidth, preHeight) {
    if ($('#' + preId)[0] != undefined) return;
    var i = document.createElement('div');
    i.id = preId;
    i.className = 'preloader';
    //var divcha = document.getElementById(containerId);
    //var divcha = $('#' + containerId);
    //$(i).insertAfter($(divcha));
    var divcha = document.getElementById(containerId);
    divcha.appendChild(i);
    $(i).css({ 'width': preWidth, 'height': preHeight });
};
function preloadImgAfterButton(containerId, preId) {
    if ($('#' + preId)[0] != undefined) return;
    var i = document.createElement('span');
    i.id = preId;
    i.className = 'preloader';    
    var divcha = document.getElementById(containerId);
    divcha.after(i);
    $(i).outerWidth($(divcha).outerHeight()).outerHeight($(divcha).outerHeight());
    //$(i).offset({ top: $(divcha).offset().top + ($(divcha).height() / 2) - ($(i).height() / 2) });
    $(i).offset({ top: $(divcha).offset().top });
};
function unLoadingData(ele) {
    $('#'+ele).remove();
}
function createClassCSS(clsName, txt) {
    //if (txt == '') return;
    var style = document.createElement('style');
    style.type = 'text/css';
    style.innerHTML = '.' + clsName + ' {  ' + txt + '; }';
    document.getElementsByTagName('head')[0].appendChild(style);
};
$(document).ready(function () {
    $('[data-number="true"]').keypress(validateNumber);
    $('[data-minlenght]').keyup(validateEmty);
    $('[data-minlenght]').each(function () {
        var el = $(this)[0];
        var val = el.getAttribute('data-minlenght');
        if (el.value.length < val) {
            el.style.border = "red solid 1px";
        }
        else {
            el.style.border = "1px solid #D5D5D5";
        }
    });
});
function checkCustomValidate() {
    var bx = 0;
    var ax = 0;
    $('[data-minlenght]').each(function () {
        var el = $(this)[0];
        var val = el.getAttribute('data-minlenght');
        if (el.value.length < val) {
            el.style.border = "red solid 1px";
            bx++;
        }
        else {
            el.style.border = "1px solid #D5D5D5";
        }
    });
    $('[data-number=true]').each(function () {
        var cx = 0;
        var el2 = $(this)[0];
        for (var i = 0; i < el2.value.length; i++) {
            if (el2.value.charCodeAt(i) < 48 || el2.value.charCodeAt(i) > 57) {
                el2.value = el2.substring(0, i) + el2.substring(i + 1, el2.value.length);
                ax++; cx++;
            }
        }
        if (cx > 0) el2.style.border = "red solid 1px";
        else el2.style.border = "1px solid #D5D5D5";
    });
    if (ax > 0) return false;
    if (bx > 0) return false;
    return true;
}
function validateEmty(event) {
    var el = $(this)[0];
    var val = el.getAttribute('data-minlenght');
    if (el.value.length < val) {
        el.style.border = "red solid 1px";
    } else { try { el.style.border = "1px solid #D5D5D5"; } catch (er) { } }
}
function validateNumber(event) {
    var key = window.event ? event.keyCode : event.which;
    if (event.keyCode === 8 || event.keyCode === 46 || event.keyCode===43) {
        return true;
    } else if (key < 48 || key > 57) {
        return false;
    } else {
        return true;
    }
};

$(document).ready(function () {
    var ax = $('[validatemmty=true]');
    if (ax.length > 0) {
        for (var i = 0; i < ax.length; i++) {
            if (ax[i].value.length == 0) {
                ax[i].style.border = "red solid 1px";
            }
        }
    }
});
function checkValidCustomMinlenght(sVal) {
    var ax = 0;
    $('[data-control="' + sVal + '"]').each(function () {
        
        var el = $(this)[0];
        var val_Lenght = el.getAttribute('data-minlenght');
        if (el.value.length < val_Lenght) {
            ax++;
            el.focus();
            el.style.border = "1px solid red";
            alert('Check validate!');
            return false;
        }
        if ($(el).attr('data-number') == 'true') {
            for (var i = 0; i < el.value.length; i++) {
                if (el.value.charCodeAt(i) === 43) {
                }
                else if (el.value.charCodeAt(i) < 48 || el.value.charCodeAt(i) > 57) {
                    el.focus();
                    el.style.border = "1px solid red";
                    alert('Check validate!');
                    ax++;
                    return false;
                }
            }
        }
    });
    if (ax == 0) return true;
    else return false;
}
function setSelectedValue(idSelected, valueToSet) {
    var selectObj = document.getElementById(idSelected);
    if (valueToSet == null) return;
    if (selectObj.options.length <= 0) return;
    for (var i = 0; i < selectObj.options.length; i++) {
        if (selectObj.options[i].value == valueToSet) {
            selectObj.options[i].selected = true;
            return;
        }
    }
}
function copyToClipboard(valu) {
    var $temp = $("<textarea>");
    var brRegex = /<br\s*[\/]?>/gi;
    $("body").append($temp);
    $temp.val(valu.replace(brRegex, "\r\n")).select();
    try {
        var successful = document.execCommand("copy");
        var msg = successful ? 'Copy success!' : 'Copy error!';
        alert(msg);
        $temp.remove();
    } catch (e) {
        alert('Copy error!');
    }

}
/*dhtml tooltip*/

var offsetxpoint = -60 //Customize x offset of tooltip
var offsetypoint = 20 //Customize y offset of tooltip
var ie = document.all
var ns6 = document.getElementById && !document.all
var enabletip = false
try {
    if (ie || ns6)
        var tipobj = document.all ? document.all["dhtmltooltip"] : document.getElementById ? document.getElementById("dhtmltooltip") : ""
    document.body.appendChild(tipobj)
} catch (e) {

}


function ietruebody() {
    return (document.compatMode && document.compatMode != "BackCompat") ? document.documentElement : document.body
}

function ddrivetip(thetext, thecolor, thewidth) {
    if (ns6 || ie) {
        if (typeof thewidth != "undefined") tipobj.style.width = thewidth + "px"
        if (typeof thecolor != "undefined" && thecolor != "") tipobj.style.backgroundColor = thecolor
        tipobj.innerHTML = thetext
        enabletip = true
        return false
    }
}

function positiontip(e) {
    if (enabletip) {
        var curX = (ns6) ? e.pageX : event.clientX + ietruebody().scrollLeft;
        var curY = (ns6) ? e.pageY : event.clientY + ietruebody().scrollTop;
        //Find out how close the mouse is to the corner of the window
        var rightedge = ie && !window.opera ? ietruebody().clientWidth - event.clientX - offsetxpoint : window.innerWidth - e.clientX - offsetxpoint - 20
        var bottomedge = ie && !window.opera ? ietruebody().clientHeight - event.clientY - offsetypoint : window.innerHeight - e.clientY - offsetypoint - 20

        var leftedge = (offsetxpoint < 0) ? offsetxpoint * (-1) : -1000

        //if the horizontal distance isn't enough to accomodate the width of the context menu
        if (rightedge < tipobj.offsetWidth)
            //move the horizontal position of the menu to the left by it's width
            tipobj.style.left = ie ? ietruebody().scrollLeft + event.clientX - tipobj.offsetWidth + "px" : window.pageXOffset + e.clientX - tipobj.offsetWidth + "px"
        else if (curX < leftedge)
            tipobj.style.left = "5px"
        else
            //position the horizontal position of the menu where the mouse is positioned
            tipobj.style.left = curX + offsetxpoint + "px"

        //same concept with the vertical position
        if (bottomedge < tipobj.offsetHeight)
            tipobj.style.top = ie ? ietruebody().scrollTop + event.clientY - tipobj.offsetHeight - offsetypoint + "px" : window.pageYOffset + e.clientY - tipobj.offsetHeight - offsetypoint + "px"
        else
            tipobj.style.top = curY + offsetypoint + "px"
        tipobj.style.visibility = "visible"
    }
}

function hideddrivetip() {
    if (ns6 || ie) {
        enabletip = false
        tipobj.style.visibility = "hidden"
        tipobj.style.left = "-1000px"
        tipobj.style.backgroundColor = ''
        tipobj.style.width = ''
    }
}

document.onmousemove = positiontip

function copyToClipboard(valu) {
    var $temp = $("<textarea>");
    var brRegex = /<br\s*[\/]?>/gi;
    $("body").append($temp);
    $temp.val(valu.replace(brRegex, "\r\n")).select();
    try {
        var successful = document.execCommand("copy");
        var msg = successful ? 'Copy success!' : 'Copy error!';
        alert(msg);
        $temp.remove();
    } catch (e) {
        alert('Copy error!');
    }

}
$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})
try {
    $('.date-picker').datepicker({
        autoclose: true,
        todayHighlight: true
    }).on('change', function (ev) {
        var el = $(this)[0];
        try {
            if (el.getAttribute('data-minlenght').length > 0) {
                if (el.value.length > 0) {
                    el.style.border = "1px solid #D5D5D5";
                }
            }
        } catch (e) {

        }

    });
} catch (e) {

}

function checkInputDate(ele) {
    var $ele = $(ele);
    var v = $(ele).val();
    if (v.length < 8) {
        //$ele.css('border-color', 'red');
        $ele.val('');
    }
    if (v.length == 8)
        v = $ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
    if (v.length > 10 || v.length == 9) {
        //$ele.css('border-color', 'red');
        $ele.val('');
    }
    if (v.length == 10) {
        v = v.replace(/-/g, '/');
        var d = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$1');
        var m = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$2');
        var y = v.replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3');
        var chk = isValidDate(v);
        if (!chk) {
            //$ele.css('border-color', 'red');
            $ele.val('');
        }
        else {
            //$ele.css('border-color', '');
            //$ele.val(new Date(y,m,d).format('dd-mm-yyyy'));
        }
    }
}

var dateFormat = function () {
    var token = /d{1,4}|m{1,4}|yy(?:yy)?|([HhMsTt])\1?|[LloSZ]|"[^"]*"|'[^']*'/g,
        timezone = /\b(?:[PMCEA][SDP]T|(?:Pacific|Mountain|Central|Eastern|Atlantic) (?:Standard|Daylight|Prevailing) Time|(?:GMT|UTC)(?:[-+]\d{4})?)\b/g,
        timezoneClip = /[^-+\dA-Z]/g,
        pad = function (val, len) {
            val = String(val);
            len = len || 2;
            while (val.length < len) val = "0" + val;
            return val;
        };

    // Regexes and supporting functions are cached through closure
    return function (date, mask, utc) {
        var dF = dateFormat;

        // You can't provide utc if you skip other args (use the "UTC:" mask prefix)
        if (arguments.length == 1 && Object.prototype.toString.call(date) == "[object String]" && !/\d/.test(date)) {
            mask = date;
            date = undefined;
        }

        // Passing date through Date applies Date.parse, if necessary
        date = date ? new Date(date) : new Date;
        if (isNaN(date)) throw SyntaxError("invalid date");

        mask = String(dF.masks[mask] || mask || dF.masks["default"]);

        // Allow setting the utc argument via the mask
        if (mask.slice(0, 4) == "UTC:") {
            mask = mask.slice(4);
            utc = true;
        }

        var _ = utc ? "getUTC" : "get",
            d = date[_ + "Date"](),
            D = date[_ + "Day"](),
            m = date[_ + "Month"](),
            y = date[_ + "FullYear"](),
            H = date[_ + "Hours"](),
            M = date[_ + "Minutes"](),
            s = date[_ + "Seconds"](),
            L = date[_ + "Milliseconds"](),
            o = utc ? 0 : date.getTimezoneOffset(),
            flags = {
                d: d,
                dd: pad(d),
                ddd: dF.i18n.dayNames[D],
                dddd: dF.i18n.dayNames[D + 7],
                m: m + 1,
                mm: pad(m + 1),
                mmm: dF.i18n.monthNames[m],
                mmmm: dF.i18n.monthNames[m + 12],
                yy: String(y).slice(2),
                yyyy: y,
                h: H % 12 || 12,
                hh: pad(H % 12 || 12),
                H: H,
                HH: pad(H),
                M: M,
                MM: pad(M),
                s: s,
                ss: pad(s),
                l: pad(L, 3),
                L: pad(L > 99 ? Math.round(L / 10) : L),
                t: H < 12 ? "a" : "p",
                tt: H < 12 ? "am" : "pm",
                T: H < 12 ? "A" : "P",
                TT: H < 12 ? "AM" : "PM",
                Z: utc ? "UTC" : (String(date).match(timezone) || [""]).pop().replace(timezoneClip, ""),
                o: (o > 0 ? "-" : "+") + pad(Math.floor(Math.abs(o) / 60) * 100 + Math.abs(o) % 60, 4),
                S: ["th", "st", "nd", "rd"][d % 10 > 3 ? 0 : (d % 100 - d % 10 != 10) * d % 10]
            };

        return mask.replace(token, function ($0) {
            return $0 in flags ? flags[$0] : $0.slice(1, $0.length - 1);
        });
    };
}();

// Some common format strings
dateFormat.masks = {
    "default": "ddd mmm dd yyyy HH:MM:ss",
    shortDate: "m/d/yy",
    mediumDate: "mmm d, yyyy",
    longDate: "mmmm d, yyyy",
    fullDate: "dddd, mmmm d, yyyy",
    shortTime: "h:MM TT",
    mediumTime: "h:MM:ss TT",
    longTime: "h:MM:ss TT Z",
    isoDate: "yyyy-mm-dd",
    isoTime: "HH:MM:ss",
    isoDateTime: "yyyy-mm-dd'T'HH:MM:ss",
    isoUtcDateTime: "UTC:yyyy-mm-dd'T'HH:MM:ss'Z'"
};

// Internationalization strings
dateFormat.i18n = {
    dayNames: [
        "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat",
        "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"
    ],
    monthNames: [
        "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
        "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ]
};

// For convenience...
Date.prototype.format = function (mask, utc) {
    return dateFormat(this, mask, utc);
};
function isValidDate(str) {
    var parts = str.split(/[\/ -]/);
    if (parts.length < 3)
        return false;
    else {
        if (isNaN(parts[0]) || isNaN(parts[1]) || isNaN(parts[2])) {
            return false;
        }
        var day = parseInt(parts[0]);
        var month = parseInt(parts[1]);
        var year = parseInt(parts[2]);
        if (isNaN(day) || isNaN(month) || isNaN(year)) {
            return false;
        }
        if (day < 1 || year < 1)
            return false;
        if (month > 12 || month < 1)
            return false;
        if ((month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12) && day > 31)
            return false;
        if ((month == 4 || month == 6 || month == 9 || month == 11) && day > 30)
            return false;
        if (month == 2) {
            if (((year % 4) == 0 && (year % 100) != 0) || ((year % 400) == 0 && (year % 100) == 0)) {
                if (day > 29)
                    return false;
            } else {
                if (day > 28)
                    return false;
            }
        }
        return true;
    }
}
try {
    $('[data-CheckDate="true"]').each(function () {
        $(this).on('blur', function(){
            checkInputDate($(this));
        } );
    });
} catch (e) {

}
(function ($) {
    var $cssStyle = $('<style id="checktipCssstyle">'
    + '.ABC{ position: relative; display: inline-block; width: 100%;}'
    + '.ABCD {position: absolute; top: 0px; right: 3px; color: red; font-size: 12px; z-index: 2; white-space: nowrap; height: 10px; width: 10px; tabindex=-1;cursor: pointer; }'
    + '.tooltip-inner{white-space: nowrap;}'
    + '</style>');

    if ($('#checktipCssstyle')[0] == undefined)
        $('body').append($cssStyle);
    function CreateTip(that) {
        $ele = $(that),
            _hasDiv = $ele.parent().hasClass('ABC'),
            _hasSpan = $ele.next().hasClass('ABCD');
        var $div = _hasDiv ? $ele.parent() : $('<div class="ABC">');
        var $span = _hasSpan ? $ele.next() : $('<span class="ABCD glyphicon glyphicon-remove" onclick="$(\'#'+$ele.prop('id')+'\').val(\'\'); $(\'#'+$ele.prop('id')+'\').focus();">');
        
        if ($ele.attr('data-contenttip') != '') {
            $span.attr('data-toggle', 'tooltip');
            $span.attr('title', $ele.attr('data-contenttip'));
            $span.attr('data-original-title', $ele.attr('data-contenttip'));
            $span.tooltip();
        } else {
            $span.attr('title', '');
        }
        if (!_hasDiv) {
            $ele.wrap($div);
            $ele.focus();
        }
        if (!_hasSpan) $ele.after($span);
        if ($ele.attr('data-contenttip') == '') {
            $div.find('.tooltip').remove();
            $span.remove();
        }
    }
    function getcontentTip(that, v) {
        var va = $(that).attr('data-contenttip');
        if (va.indexOf(v) < 0)
            that.attr('data-contenttip', va + v);
    }
    function removcontentTip(that, v) {
        var va = $(that).attr('data-contenttip');
        if (va.indexOf(v) != -1)
            that.attr('data-contenttip', va.replace(v, ''));
    }
    function preIns(that) {
        var $ele = $(that),
        $minlength = $(that).attr('data-minlenght') != undefined ? parseInt($(that).attr('data-minlenght')) : 0,
        $maxlength = $(that).attr('maxlength') != undefined ? parseInt($(that).attr('maxlength')) : 0,
        $isNumber = $(that).attr('data-number') != undefined ? ($(that).attr('data-number').toLowerCase() == 'true' ? true : false) : false,
        $isDate = $(that).attr('data-checkdate') != undefined ? ($(that).attr('data-checkdate').toLowerCase() == 'true' ? true : false) : false,
        //$valueLength = $ele.val().length,
        $isMin = $(that).attr('data-minlenght') == undefined ? false : true,
        $isMax = $(that).attr('maxlength') == undefined ? false : true,
        _numberTip = 'Value is number!',
        _maxlengthTip = 'Value <=' + $maxlength + 'charater!',
        _DateTip = 'Date format dd-mm-yyyy, ddmmyyyy',
        _minlengthTip = 'Value >' + $minlength + 'charater!',
        _isShowTip = false,
        isMinlength = false, isMaxlength = false, isDate = true, isNumber = false;
        $ele.attr('data-contenttip', '');
        $ele.keypress(function (e) {
            if ($isNumber) {
                var charCode = (typeof e.which == "undefined") ? e.keyCode : e.which;
                if (charCode > 47 && charCode < 58 || charCode==43) {
                    removcontentTip(that, _numberTip);
                }
                else {
                    getcontentTip(that, _numberTip)
                    //e.preventDefault();
                }
            } else { removcontentTip(that, _numberTip); }
            CreateTip(that);
        })
        $ele.keyup(function (e) {
            if ($isMin) {
                if ($ele.val().length < $minlength) {
                    getcontentTip(that, _minlengthTip)
                }
                else removcontentTip(that, _minlengthTip);
            } else { removcontentTip(that, _minlengthTip); }
            //if ($isMax) {
            //    if ($ele.val().length == $maxlength) {
            //        getcontentTip(that, _maxlengthTip)
            //    } else removcontentTip(that, _maxlengthTip);
            //} else { removcontentTip(that, _maxlengthTip); }
            if ($isDate) {
                var v = $ele.val();
                if (v.length == 6) {
                    v = v.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                    $ele.val(v);
                    if (!isValidDate(v)) {
                        getcontentTip(that, _DateTip);
                    }
                    else {
                        removcontentTip(that, _DateTip);
                    }
                }
                if (v.length < 6) {
                    getcontentTip(that, _DateTip);
                }
                if (v.length == 8) {
                    $ele.val($ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3'));
                    v = $ele.val();
                }
                if (v.length > 10 || v.length == 9) {
                    getcontentTip(that, _DateTip);
                }
                if (v.length == 10) {
                    v = v.replace(/-/g, '/');
                    var d = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$1');
                    var m = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$2');
                    var y = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3');
                    var chk = isValidDate(v);
                    if (!chk) {
                        getcontentTip(that, _DateTip);
                        $ele.css({ 'color': 'red' });
                    }
                    else {
                        removcontentTip(that, _DateTip);
                        $ele.css({ 'color': '' });
                    }
                }
            }
            CreateTip(that);
        })

    }
    $.fn.ValidateTip = function () {
        if ($('#checktipCssstyle')[0] == undefined)
            $('body').append($cssStyle);
        preIns(this);
    };
}(jQuery));

