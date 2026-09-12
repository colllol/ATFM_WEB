<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ImportPermForMonth.aspx.cs" Inherits="prjApplication.Tool.ImportPermForMonth" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        input, textarea {
            text-transform: uppercase;
        }

        .import-perm-page {
            display: grid;
            gap: 16px;
        }

        .import-perm-input-card,
        .import-perm-table-card {
            padding: 18px;
            border: 1px solid #d3e2ed;
            border-radius: 14px;
            background: #fff;
            box-shadow: 0 7px 22px rgba(27, 73, 108, .09);
        }

        .import-perm-input-card textarea {
            display: block;
            width: 100%;
            min-height: 150px;
            max-height: 230px;
            padding: 12px 14px;
            resize: vertical;
            border: 1px solid #bad3e5;
            border-radius: 9px;
            outline: 0;
            background: #fbfdff;
        }

        .import-perm-oper {
            display: flex;
            max-width: 360px;
            align-items: center;
            gap: 10px;
            margin-top: 12px;
            color: #174f78;
            font-weight: 700;
        }

        .import-perm-oper input,
        .import-perm-date-filter input {
            width: 100%;
            height: 38px;
            padding: 7px 10px;
            border: 1px solid #b9d2e4;
            border-radius: 8px;
            outline: 0;
            background: #fff;
        }

        .import-perm-table-card {
            min-width: 0;
            padding: 0;
            overflow: visible;
        }

        .import-perm-table-title {
            margin: 0;
            padding: 16px 18px 12px;
            border-bottom: 1px solid #dce8f1;
            color: #155f91;
            font-size: 19px;
            font-weight: 700;
        }

        .import-perm-toolbar {
            display: flex;
            align-items: flex-end;
            justify-content: space-between;
            gap: 14px;
            padding: 14px 18px;
            background: #f6faff;
        }

        .import-perm-actions,
        .import-perm-date-filter {
            display: flex;
            align-items: flex-end;
            gap: 8px;
            flex-wrap: wrap;
        }

        .import-perm-date-field {
            display: grid;
            min-width: 170px;
            gap: 5px;
            color: #31536d;
            font-size: 12px;
            font-weight: 700;
        }

        .import-perm-toolbar .btn {
            min-height: 38px;
            padding: 8px 15px;
            border: 0;
            border-radius: 8px;
            font-weight: 700;
            box-shadow: 0 4px 10px rgba(28, 105, 159, .16);
        }

        .import-perm-toolbar .btn-danger-soft {
            background: #e74c5e !important;
            color: #fff !important;
        }

        .import-perm-table-scroll {
            width: 100%;
            max-height: clamp(360px, 55vh, 620px) !important;
            margin: 0 !important;
            overflow: auto !important;
            border: 0 !important;
            border-radius: 0 0 14px 14px !important;
        }

        #tblSource {
            width: max-content;
            min-width: 1180px;
            margin: 0;
            table-layout: fixed;
            border-collapse: separate;
            border-spacing: 0;
        }

        #tblSource thead {
            position: static;
            box-shadow: none;
        }

        #tblSource thead th {
            position: sticky;
            padding: 7px 8px;
            vertical-align: middle;
            border-color: #c8dbe8;
        }

        #tblSource thead tr:first-child th {
            top: 0;
            z-index: 32;
            height: 54px;
            background: #eaf4fb;
        }

        #tblSource thead tr:nth-child(2) th {
            top: 54px;
            z-index: 31;
            height: 40px;
            background: #246b9c;
            color: #fff;
            font-weight: 700;
            text-align: center;
        }

        #tblSource thead input {
            width: 100%;
            height: 36px;
            padding: 6px 8px;
            border: 1px solid #b4cee1;
            border-radius: 7px;
            outline: 0;
            background: #fff;
        }

        #tblSource tbody td {
            height: 36px;
            padding: 7px 9px;
            overflow: hidden;
            border-color: #dce6ee;
            background: #fff;
            line-height: 20px;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        #tblSource tbody tr:nth-child(even) td {
            background: #f7fafc;
        }

        #tblSource tbody tr:hover td {
            background: #e9f5fd;
        }

        #tblSource th:nth-child(1), #tblSource td:nth-child(1) { width: 52px; text-align: center; }
        #tblSource th:nth-child(2), #tblSource td:nth-child(2) { width: 140px; }
        #tblSource th:nth-child(3), #tblSource td:nth-child(3) { width: 105px; }
        #tblSource th:nth-child(4), #tblSource td:nth-child(4) { width: 130px; }
        #tblSource th:nth-child(5), #tblSource td:nth-child(5) { width: 130px; }
        #tblSource th:nth-child(6), #tblSource td:nth-child(6),
        #tblSource th:nth-child(7), #tblSource td:nth-child(7) { width: 100px; }
        #tblSource th:nth-child(8), #tblSource td:nth-child(8),
        #tblSource th:nth-child(9), #tblSource td:nth-child(9) { width: 95px; }
        #tblSource th:nth-child(10), #tblSource td:nth-child(10) { width: 100px; }
        #tblSource th:nth-child(11), #tblSource td:nth-child(11) { width: 85px; text-align: center; }

        #tblSource .import-perm-delete {
            color: #dc3545;
            font-weight: 700;
        }

        @media (max-width: 900px) {
            .import-perm-toolbar { align-items: stretch; flex-direction: column; }
            .import-perm-date-filter { width: 100%; }
            .import-perm-date-field { flex: 1 1 160px; }
        }
    </style>

    <div class="import-perm-page">
    <section class="import-perm-input-card">
        <textarea id="txtCONTENT" runat="server" cols="20" rows="15" class="wid_100"></textarea>
        <label class="import-perm-oper">
            <span>Hãng khai thác (OPER)</span>
            <input id="txtOPER" runat="server" value="HVN" data-autocomplete="OPER" data-control="mUpdate"
                data-minlenght="2" type="text" />
        </label>
    </section>

    <section class="import-perm-table-card">
        <h3 class="import-perm-table-title">Dữ liệu tạm</h3>
        <div class="import-perm-toolbar">
            <div class="import-perm-actions">
                <asp:Button ID="btnSearch" Text="Tìm kiếm" runat="server" CssClass="btn btn-primary"
                    OnClick="btnSearch_Click" />
                <asp:Button runat="server" ID="btnImport" Text="Nhập dữ liệu" OnClick="btnImport_Click"
                    CssClass="btn btn-primary" />
            </div>
            <div class="import-perm-date-filter">
                <label class="import-perm-date-field">
                    <span>Từ ngày</span>
                    <input id="txtFromDate" runat="server" type="text" />
                </label>
                <label class="import-perm-date-field">
                    <span>Đến ngày</span>
                    <input id="txtToDate" runat="server" type="text" />
                </label>
                <asp:Button runat="server" ID="btnDeleteAll" OnClientClick="return validDelete();"
                    Text="Xóa theo ngày" CssClass="btn btn-danger-soft" OnClick="btnDeleteAll_Click" />
            </div>
        </div>
        <div class="table-responsive import-perm-table-scroll">
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
                            <td><%# Container.ItemIndex + 1 %>
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
                                    data-id='<%# Eval("ID") %>' OnClick="btnDeleteId_Click" Text="Xóa"
                                    CssClass="import-perm-delete"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
        </div>
    </section>
    </div>



    
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

