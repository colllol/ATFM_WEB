<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="ImportPermNo.aspx.cs" Inherits="prjApplication.Tool.ImportPermNo" %>

<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        input, textarea {
            text-transform: uppercase;
        }
    </style>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <div id="divTong">
        <div>
            <%-- <textarea id="txtCONTENT" runat="server" cols="20" rows="15" class="wid_100"></textarea>--%>
            <asp:TextBox ID="txtCONTENT" TextMode="MultiLine" Rows="15" cols="20" runat="server" class="wid_100" />
        </div>
        <div>

            <asp:Button ID="btnLoc" runat="server" OnClientClick="if($('#ctl00_MainContent_txtCONTENT').val().trim()=='') {alert('Content no value!');$('#ctl00_MainContent_txtCONTENT').focus();return false;} if($('#ctl00_MainContent_txtOPER').val().trim()=='') {alert('Oper no value!');$('#ctl00_MainContent_txtOPER').focus();return false;}" Text="Convert" CssClass="btn btn-primary"
                OnClick="btnLoc_Click" />
            <asp:Button ID="btnUpdate" runat="server" OnClientClick="return checkValidCustomMinlenght('mUpdate');" Text="Save" CssClass="btn btn-primary"
                OnClick="btnUpdate_Click" />
        </div>
        <table>
            <tr>
                <td>Number</td>
                <td>
                    <input id="txtPERMNBR" runat="server" data-minlenght="2" data-control="mUpdate" type="text" />
                </td>
                <td>Reg</td>
                <td>
                    <input id="txtREGISTRATION" runat="server" type="text" />
                    <select id="ddlFLIGHTTYPE" runat="server" style="width: 70px;">
                        <option value="NO">NO</option>
                        <option value="SC">SC</option>
                    </select>
                </td>
                <td>Perm date</td>
                <td>
                    <input id="txtPERMDATE" data-minlenght="1" data-control="mUpdate" runat="server"
                        type="text" /></td>
                <td>Perm type</td>
                <td>
                    <input id="txtPERMTYPE" data-autocomplete="PERMTYPE" data-minlenght="1" data-control="mUpdate" runat="server" type="text" />
                </td>
            </tr>
            <tr>
                <td>Oper (*)</td>
                <td>
                    <input id="txtOPER" runat="server" data-autocomplete="OPER" data-control="mUpdate" data-minlenght="2" type="text" />
                </td>
                <td>Author</td>
                <td>
                    <select id="ddlAUTHOR" runat="server">
                        <option value="CHK">CUC HANG KHONG</option>
                        <option value="BQF">BO QUOC PHONG</option>
                        <option value="BNG">BO NGOAI GIAO</option>
                        <option value="CSC">THU NGHIEM TESTING</option>
                        <option value="QLB">TRUNG TAM QLB DAN DUNG VIET NAM</option>
                    </select></td>
                <td>Purpose(*)</td>
                <td>
                    <input id="txtPURPOSE" data-autocomplete="PURPOSE" runat="server" type="text" />
                </td>
                <td>Craft(*)</td>
                <td>
                    <input id="txtCraft" data-autocomplete="CRAFT" runat="server" type="text" />
                </td>
            </tr>
            <tr>
                <td>Schedule</td>
                <td colspan="3">
                    <textarea id="txtSCHEDULE" rows="10" data-control="mUpdate" data-minlenght="10" class="wid_100" runat="server"></textarea>
                </td>
                <td>Route</td>
                <td colspan="3">
                    <textarea id="txtVIA" rows="10" class="wid_100" runat="server"></textarea>
            </tr>
            <tr>
            </tr>
            <tr>
                <td>Remark</td>
                <td colspan="3">
                    <textarea id="txtREMARK" rows="3" class="wid_100" runat="server"></textarea>
                    <td>Billing</td>
                    <td colspan="3">
                        <textarea id="txtBILLINGADDRESS" rows="3" class="wid_100" runat="server"></textarea>
            </tr>
        </table>
    </div>


    <fieldset>
        <legend></legend>
        <input type="button" value="acbtest" onclick="btnAccess_Click();" />
        <asp:Button runat="server" ID="btnDeleteAll" OnClientClick="return confirm('Do you want delete all?')" Text="Delete all" CssClass="btn btn-primary" OnClick="btnDeleteAll_Click" />
        <asp:Button runat="server" ID="btnAccess" OnClientClick="if($('#ctl00_MainContent_txtOPER').val().trim()=='') {alert('Oper no value!');$('#ctl00_MainContent_txtOPER').focus();return false;}"  Text="Commit to schedule" 
            CssClass="btn btn-primary" /> <span style="font-size:16px;color:red; font-weight:bold;margin:5px;">Hiện mỗi lần commit sẽ thực hiện 50 chuyến.Các anh chị lưu ý ạ.</span>
        <table id="tblSource" class="table table-bordered">
            <thead>
                <tr>
                    <td>No</td>
                    <td>Flight date</td>
                    <td>Craft</td>
                    <td>CallSign</td>
                    <td>From</td>
                    <td>To</td>
                    <td>Etd</td>
                    <td>Eta</td>
                    <td>Etd old</td>
                    <td>Eta old</td>
                    <td>Flight date old</td>
                    <td>Purpose</td>
                    <td>Loai</td>
                    <td>Perm type</td>
                    <td>PERM_ID</td>
                    <td></td>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater runat="server" ID="rptSource">
                    <HeaderTemplate>
				 <div>
				 <div style="font-size:16px;font-weight:bold;margin:5px;text-align:center;">
						Tổng số chuyến:		<%# Convert.ToString(((System.Data.DataTable)((Repeater)Container.Parent).DataSource).Rows.Count) %>
				 </div>
				 </div>
 </HeaderTemplate>
					<ItemTemplate>
                        <tr>
                            <td><%# Container.ItemIndex +1 %></td>
                            <td><%# Eval("FlightDate_New","{0:dd-MM-yyyy}") %></td>
                            <td><%# Eval("Craft") %></td>
                            <td><%# Eval("Flightnbr") %></td>
                            <td><%# Eval("From_Airp") %></td>
                            <td><%# Eval("To_Airp") %></td>
                            <td><%# Eval("Etd_New") %></td>
                            <td><%# Eval("Eta_New") %></td>
                            <td><%# Eval("Etd_Old") %></td>
                            <td><%# Eval("Eta_Old") %></td>
                            <td><%# Eval("FlightDate_Old","{0:dd-MM-yyyy}") %></td>
                            <td><%# Eval("Purpose") %></td>
                            <td><%# Eval("Loai") %></td>
                            <td><%# Eval("PermType") %></td>
                            <td><%# Eval("PERMNBR") %></td>
                            <td>
                                <asp:LinkButton runat="server" ID="btnDeleteId" OnClientClick="return confirm('Do you want delete?');" data-id='<%# Eval("ID") %>' OnClick="btnDeleteId_Click" Text="Delete"></asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </fieldset>


    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        function btnAccess_Click() {
             document.getElementById("<%= btnAccess.ClientID %>").click();
        }
       
        //OnClick = "btnAccess_Click"
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
                        $ele.focus();
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
        $('#<%= txtPERMDATE.ClientID%>').multiDate();
        
    </script>
</asp:Content>
