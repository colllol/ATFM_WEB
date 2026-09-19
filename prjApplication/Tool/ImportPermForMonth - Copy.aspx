<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="ImportPermForMonth.aspx.cs" Inherits="prjApplication.Tool.ImportPermForMonth" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        input, textarea {
            text-transform: uppercase;
        }
    </style>



    <div>
        <textarea id="txtCONTENT" runat="server" cols="20" rows="15" class="wid_100"></textarea>
    </div>
    <div>
        Oper<input id="txtOPER" runat="server" value="HVN" data-autocomplete="OPER" data-control="mUpdate"
            data-minlenght="2" type="text" />
    </div>
    <fieldset>
        <legend>Bang tam</legend>
        <asp:Button ID="btnSearch" Text="Search" runat="server" CssClass="btn btn-primary"
            OnClick="btnSearch_Click" />
        <asp:Button runat="server" ID="btnImport" Text="Import" OnClick="btnImport_Click"
            CssClass="btn btn-primary" />
        <div style="float: right;">
          From  Date<input id="txtFromDate" runat="server" type="text" />
          To Date  
        <input id="txtToDate" runat="server" type="text"/>
            <asp:Button runat="server" ID="btnDeleteAll" OnClientClick="return validDelete();"
                Text="Delete by date" CssClass="btn btn-primary" OnClick="btnDeleteAll_Click" />
        </div>
        <table id="tblSource" class="table table-bordered">
            <thead>
                <tr>
                    <th></th>
                    <th>
                        <input runat="server" id="txtFLIGHT_DATE" type="text" /></th>
                    <th>
                        <input runat="server" id="txtCRAFT_TYPE" type="text" /></th>
                    <th>
                        <input runat="server" id="txtREGISTER_CRAFT" type="text" /></th>
                    <th>
                        <input runat="server" id="txtCALLSIGN" type="text" /></th>
                    <th>
                        <input runat="server" id="txtFROM_AIRP" type="text" /></th>
                    <th>
                        <input runat="server" id="txtTO_AIRP" type="text" /></th>
                    <th>
                        <input runat="server" id="txtETD" type="text" /></th>
                    <th>
                        <input runat="server" id="txtETA" type="text" /></th>
                    <th>
                        <input runat="server" id="txtOPER_Search" type="text" /></th>
                    <th></th>
                </tr>
                <tr>
                    <th>No</th>
                    <th>Flight date</th>
                    <th>Craft</th>
                    <th>Regis</th>
                    <th>CallSign</th>
                    <th>From</th>
                    <th>To</th>
                    <th>Etd</th>
                    <th>Eta</th>
                    <th>Oper</th>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptSource">
                    <ItemTemplate>
                        <tr>
                            <td><%# (Container.ItemIndex + 1)+(PhanTrang1.PageIndex*PhanTrang1.PageSize)  %>
                            </td>
                            <td><%# Eval("FLIGHT_DATE","{0:dd-MM-yyyy}") %></td>
                            <td><%# Eval("CRAFT_TYPE") %></td>
                            <td><%# Eval("REGISTER_CRAFT") %></td>
                            <td><%# Eval("CALLSIGN") %></td>
                            <td><%# Eval("FROM_AIRP") %></td>
                            <td><%# Eval("TO_AIRP") %></td>
                            <td><%# Eval("ETD") %></td>
                            <td><%# Eval("ETA") %></td>
                            <td><%# Eval("OPER") %></td>
                            <td>
                                <asp:LinkButton runat="server" ID="btnDeleteId" OnClientClick="return confirm('Do you want delete?');"
                                    data-id='<%# Eval("ID") %>' OnClick="btnDeleteId_Click" Text="Delete"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        <div style="text-align: right">
            <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="100" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange">
            </cc1:PhanTrang>

        </div>

    </fieldset>



    
    <script src="../Scripts/CustomDynamic.js"></script>
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

    </script>
    <script>

    </script>
    <script>
        $('#<%= txtFromDate.ClientID%>').multiDate();
        $('#<%= txtToDate.ClientID%>').multiDate();
        function validDelete() {
            if ($('#<%= txtFromDate.ClientID%>').val() == '') {
                alert('Please select date!');
                $('#<%= txtFromDate.ClientID%>').focus();
                return false;
            }
            if ($('#<%= txtToDate.ClientID%>').val() == '') {
                alert('Please select date!');
                $('#<%= txtToDate.ClientID%>').focus();
                return false;
            }
            return confirm('Do you want delete?');
        }
    </script>
</asp:Content>

