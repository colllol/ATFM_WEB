<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="Inbox.aspx.cs" Inherits="prjApplication.Receive_LogFile.Inbox" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .select {
        background-color: cadetblue;
        }
    </style>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    
        <div>
        <label for="txtFromDateSearch">From:</label> 
        <input id="txtFromDateSearch" style="width:100px" type="text" />
        <label for="txtToDateSearch">to:</label><input id="txtToDateSearch" style="width:100px" type="text" />
        <label for="txtNbrSearch">Nbr:</label>
        <input id="txtNbrSearch" type="text" />
        <label for="txtOriginSearch">Origin</label>
        <input id="txtOriginSearch" type="text" />
        <label for="txtContentSearch">Content:</label>
        <input id="txtContentSearch" type="text" />
        <button id="btnSeacrch" type="button" class="btn btn-primary" onclick="btnSeacrch_OnClick()">Search</button>
    </div>
    <hr />
    
    <div style="position: relative; width: 100%; height:500px">
        <label for="lblNbr">CID/CSN</label>
        <div style="position: relative;max-width: 150px;overflow-y: scroll; left: 0; right: 0; max-height:100%; border: 1px black solid;">
            
            <div id="lblNbr"></div>
        </div>
        <div style="position: absolute; left: 175px; top: 20px;width:80%;">
            <div>
                <div>Address</div>
            <textarea rows="3" style="width:100%" id="txtAddress"></textarea>
            </div>
            <div>
                Time receive
                <input id="txtTimeReceive" type="text" />
                Origin
                <input id="txtOrigin" disabled="disabled" type="text" />
                <!--Filter by Origin
                <select id="ddlFilterOrigin"></select>
                Desciption-->
                <input id="txtDesciption" disabled="disabled" type="text" />
                <button id="btnSend" type="button" class="btn btn-primary" onclick="btnSend_OnClick()">Forward</button>
                <button id="btnSendAMHS" type="button" class="btn btn-primary" onclick="btnSendAMHS_OnClick()">Forward AMHS</button>
                <button id="btnPrint" type="button" class="btn btn-primary" onclick="btnPrint_OnClick()">Print</button>
            </div>
            <div>
                Content
                <textarea style="width:100%; height:350px;" id="txtContent">
                </textarea>
            </div>
        </div>
    </div>


    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        $(document).ready(function () {
            $('#txtFromDateSearch').val(new Date().format('dd-mm-yyyy'));
            $('#txtToDateSearch').val(new Date().format('dd-mm-yyyy'));
            $('#txtFromDateSearch').multiDate();
            $('#txtToDateSearch').multiDate();
            //$('#btnSeacrch').click();
            LoadData();
        })
    </script>
    <script>
        function btnSend_OnClick() {
            var id = $('#txtDesciption').val();
	    console.log($('#txtDesciption').val());
            SubmitImage('../SendMessage/SendMessageFlight.aspx?Menu_ID=89&id='+id, 1200, 580);
            
        }
        function btnSendAMHS_OnClick() {
            var id = $('#txtDesciption').val();
            SubmitImage('../SendMessage/SendMessageFlightAMHS.aspx?Menu_ID=89&id=' + id, 1200, 580);

        }
        function btnPrint_OnClick() {
            var content = $('#txtContent').val();

            var newline = String.fromCharCode(13, 10);

            content = content.replace('\r', '<br>');

            content = content.replace('NNNN', '<br>' + '<br>' + '<br>' + '<br>' + '<br>' + '<br>' + 'NNNN');

            Popup(content);
        }
        function Popup(data) {
            var mywindow = window.open('', 'print page', 'height=800,width=600');
            mywindow.document.write('<html><head><title>print page</title>');
            /*optional stylesheet*/ //mywindow.document.write('<link rel="stylesheet" href="main.css" type="text/css" />');
            mywindow.document.write('</head><body >');
            mywindow.document.write('<textarea style="width:100%; height:100%;border:0px;" id="txtContent">' + data + '</textarea>');

            mywindow.document.write('</body></html>');

            mywindow.print();
            mywindow.close();

            return true;
        }
        function LoadData() {
            $('#lblNbr').html('');
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GetInboxBySearch",
                data: JSON.stringify({ P_FROMDATE: new Date($('#txtFromDateSearch').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')), P_TODATE: new Date($('#txtToDateSearch').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')), P_NBR: $('#txtNbrSearch').val(), P_ORIGIN: $('#txtOriginSearch').val(), P_CONTENT: $('#txtContentSearch').val() })
            }).always(function (data) {
                if (data.ListValue[0] != null) {
                    $(data.ListValue).each(function (a, b) {
                        var c = "<div onclick=\"lblNbr_OnRowClick('" + b.TYPE + "', " + b.ID + "); $(this).addClass('select');\">" + b.NBR + "</div>";
                        $('#lblNbr').append(c);
                    })
                }
            });
        }

        function LoadDataLogFile() {
            $('#lblNbr').html('');
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GetInboxBySearchLogFile",
                data: JSON.stringify({ P_FROMDATE: new Date($('#txtFromDateSearch').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')), P_TODATE: new Date($('#txtToDateSearch').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')), P_NBR: $('#txtNbrSearch').val(), P_ORIGIN: $('#txtOriginSearch').val(), P_CONTENT: $('#txtContentSearch').val() })
            }).always(function (data) {
                if (data.ListValue[0] != null) {
                    $(data.ListValue).each(function (a, b) {
                        var c = "<div onclick=\"lblNbr_OnRowClickLogFile('" + b.TYPE + "', " + b.ID + "); $(this).addClass('select');\">" + b.NBR + "</div>";
                        $('#lblNbr').append(c);
                    })
                }
            });
        }



        function btnSeacrch_OnClick() {
            var _date1 = new Date().format('mm-yyyy');
            var _date2 = $('#txtFromDateSearch').val().substring(3,($('#txtFromDateSearch').val().length));
            
            if (_date1 == _date2)
            {                
                LoadData();
            }                
            else
            {                
                LoadDataLogFile();
            }
               
        }
        function lblNbr_OnRowClick(ty, id) {
            
            $('#lblNbr div').removeClass('select');
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GetInboxDetailBy",
                data: JSON.stringify({ P_TYPE: ty, P_ID: id })
            }).always(function (data) {
                if (data.ListValue[0] != null) {
                    var ct = data.ListValue[0].CONTENT;
                    $('#txtAddress').val(getAddress(ct));
                    $('#txtTimeReceive').val(new Date(data.ListValue[0].LETTERNBR_PK).format('dd-mm-yyyy HH:MM'));
                    $('#txtOrigin').val(data.ListValue[0].FROM_PL);
                    $('#txtContent').val(data.ListValue[0].CONTENT);
                    $('#txtDesciption').val(id);
                    
                }
            });
        }


        function lblNbr_OnRowClickLogFile(ty, id) {            
            $('#lblNbr div').removeClass('select');
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + "api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GetInboxDetailByLogFile",
                data: JSON.stringify({ P_TYPE: ty, P_ID: id })
            }).always(function (data) {
                if (data.ListValue[0] != null) {
                    var ct = data.ListValue[0].CONTENT;
                    $('#txtAddress').val(getAddress(ct));
                    $('#txtTimeReceive').val(new Date(data.ListValue[0].LETTERNBR_PK).format('dd-mm-yyyy HH:MM'));
                    $('#txtOrigin').val(data.ListValue[0].FROM_PL);
                    $('#txtContent').val(data.ListValue[0].CONTENT);
                    $('#txtDesciption').val(id);

                }
            });
        }

        function getAddress(ct) {
            var kq = '',
                fi = 0,
                ta = 0
                tn = '';
            fi = ct.substring(ct.indexOf('\n') + 1, ct.indexOf(' ', ct.indexOf('\n') + 1));
            if (fi.length == 2) {
                fi = ct.indexOf(' ', ct.indexOf('\n') -2);
                ta = ct.indexOf('\n', fi) + 1;
                tn = ct.substring(ta, ct.indexOf(' ', ta));
                while (tn.length != 6) {
                    ta = ct.indexOf('\n', ta) + 1;
                    tn = ct.substring(ta, ct.indexOf(' ', ta));
                }
                kq = ct.substring(fi+3, ta-1);
            }
            return kq;
        }
    </script>
    <script>
        (function ($) {
            var $cssStyle = $('<style id="checktipCssstyle">'
                        + '.ABC{ position: relative; display: inline-block;}'
                        + '.ABCD {position: absolute; top: 0px; right: 3px; color: red; font-size: 12px; z-index: 1; white-space: nowrap; height: 10px; width: 10px; tabindex="-1" }'
                        + '</style>');
            if ($('#checktipCssstyle')[0] == undefined)
                $('body').append($cssStyle);
            function preIns(that) {
                var createTip = function (ele) {

                    var $ele = $(ele),
                        _hasDiv = $ele.parent().hasClass('ABC'),
                        _hasSpan = $ele.next().hasClass('ABCD'),
                        isNumber = false, isDate = false, isMinlength = false, isMaxlength = false;
                    var $div = _hasDiv ? $ele.parent() : $('<div class="ABC">');
                    var $span = _hasSpan ? $ele.next() : $('<span class="ABCD glyphicon glyphicon-remove">');
                    if ($ele.attr('data-contenttip') != undefined) {
                        $span.attr('data-toggle', 'tooltip');
                        $span.attr('title', $ele.attr('data-contenttip'));
                        $span.tooltip();
                    } else $span.attr('title', '');
                    if (!_hasDiv) { $ele.wrap($div); $ele.focus(); }
                    if (!_hasSpan) { $ele.after($span); $ele.focus(); }
                }
                var removeTip = function (ele) {
                    if ($(ele).next().hasClass('ABCD')) {
                        $(ele).next().remove();
                        $(ele).focus();
                    }

                }
                $(that).keyup(function (e) {
                    var $ele = $(that);
                    $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                    if (e.keyCode == 13) {
                        var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                        v = v.replace(/JAN/gi, '01');
                        v = v.replace(/FEB/gi, '02');
                        v = v.replace(/MAR/gi, '03');
                        v = v.replace(/APR/gi, '04');
                        v = v.replace(/MAY/gi, '05');
                        v = v.replace(/JUN/gi, '06');
                        v = v.replace(/JUL/gi, '07');
                        v = v.replace(/AUG/gi, '08');
                        v = v.replace(/SEP/gi, '09');
                        v = v.replace(/OCT/gi, '10');
                        v = v.replace(/NOV/gi, '11');
                        v = v.replace(/DEC/gi, '12');
                        $ele.val(v);
                        var ds = $ele.val().trim().split(',');
                        $(ds).each(function (a, b) {
                            if (b.length == 8) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 6) {
                                b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 2) {
                                var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                                var mmyy = chuan.substring(2, 6);
                                b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                                if (!isValidDate(b)) {
                                    c = 1;
                                } else
                                    _new.push(b);
                            }
                            else if (b.length == 10) {
                                if (!isValidDate(b)) {
                                    c = 1;
                                }
                                else _new.push(b);
                            }
                            else {
                                c = 1;
                            }
                        });
                        if (c == 0) {
                            $ele.val('');
                            $(_new).each(function () {
                                $ele.val($ele.val() + this + ',');
                            })
                            $ele.val($ele.val().substring(0, $ele.val().length - 1));
                            $(this).attr('data-contenttip', '');
                            $ele.css({ 'color': '' })
                            removeTip($ele);
                        } else {
                            $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                            $ele.css({ 'color': 'red' });
                            createTip($ele);

                        }
                    }
                })
                $(that).bind('blur', function () {
                    var $ele = $(that);
                    var _new = [],
                            c = 0,
                            v = $ele.val().toUpperCase();
                    v = v.replace(/JAN/gi, '01');
                    v = v.replace(/FEB/gi, '02');
                    v = v.replace(/MAR/gi, '03');
                    v = v.replace(/APR/gi, '04');
                    v = v.replace(/MAY/gi, '05');
                    v = v.replace(/JUN/gi, '06');
                    v = v.replace(/JUL/gi, '07');
                    v = v.replace(/AUG/gi, '08');
                    v = v.replace(/SEP/gi, '09');
                    v = v.replace(/OCT/gi, '10');
                    v = v.replace(/NOV/gi, '11');
                    v = v.replace(/DEC/gi, '12');
                    $ele.val(v);
                    var ds = $ele.val().trim().split(',');
                    $(ds).each(function (a, b) {
                        if (b.length == 8) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{4})$/, '$1-$2-$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 6) {
                            b = b.replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 2) {
                            var chuan = ds[ds.findIndex(item => item.length == 6 && ds.findIndex(x=>x == item) > a)];
                            var mmyy = chuan.substring(2, 6);
                            b = (b + mmyy).replace(/^(\d{2})(\d{2})(\d{2})$/, '$1-$2-20$3');
                            if (!isValidDate(b)) {
                                c = 1;
                            } else
                                _new.push(b);
                        }
                        else if (b.length == 10) {
                            if (!isValidDate(b)) {
                                c = 1;
                            }
                            else _new.push(b);
                        }
                        else {
                            c = 1;
                        }
                    });
                    if (c == 0) {
                        $ele.val('');
                        $(_new).each(function () {
                            $ele.val($ele.val() + this + ',');
                        })
                        $ele.val($ele.val().substring(0, $ele.val().length - 1));
                        $(this).attr('data-contenttip', '');
                        $ele.css({ 'color': '' })
                        removeTip($ele);
                    } else {
                        $ele.attr('data-contenttip', 'format dd,dd....,ddmmyy');
                        $ele.css({ 'color': 'red' });
                        //$ele.focus();
                    }
                })
            }
            $.fn.multiDate = function () {
                if ($('#checktipCssstyle')[0] == undefined)
                    $('body').append($cssStyle);
                $(this).attr('data-contenttip', 'format dd,dd....,ddmmyy');
                preIns(this);
            };
        }(jQuery));
        window.onkeydown = function (e) {
            var charCode = (e.which) ? e.which : e.keyCode;
            
            // len 38 xuong 40
            if (charCode == 38) {
                try {
                    var el = $('#lblNbr div.select');
                    el.removeClass('select');
                    el.focus();
                    el.prev().click();
                } catch (x) { }
            }
            if (charCode == 40) {
                try {
                    var el = $('#lblNbr div.select');
                    el.removeClass('select');
                    el.focus();
                    el.next().click();
                } catch (x) { }
            }
        }
    </script>
</asp:Content>
