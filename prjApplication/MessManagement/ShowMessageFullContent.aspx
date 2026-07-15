<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowMessageFullContent.aspx.cs"
    Inherits="prjApplication.MessManagement.ShowMessageFullContent" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        .fGea {
            width: 80%;
            height: 400px;
        }

        ul > li {
            cursor: pointer;
            color: blue;
        }

            ul > li.active {
                cursor: pointer;
                color: red;
            }

        .mMessage {
            height: 400px;
            width: 80%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div style="width: 100%">
            <table style="width: 100%">
                <tr>
                    <td style="width: 250px">
                        <fieldset class="fGea">
                            <legend>GEA</legend>
                            <div id="divReferent">
                            </div>
                        </fieldset>
                    </td>
                    <td>
                        <fieldset class="mMessage">
                            <legend>Message</legend>
                            <label for="txtDate">Date</label><input id="txtDate" type="text" />
                            From<input id="txtGea" type="text" />
                            <div id="divMessageContent">
                                <table>
                                    <tr>
                                        <td>
                                            <label for="txtMessage">Message</label></td>
                                        <td>
                                            <textarea id="txtMessage" rows="20" class="wid_800px" cols="20"></textarea></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label for="txtReference">Reference</label></td>
                                        <td>
                                            <textarea id="txtReference" class="wid_800px" rows="3" cols="20"></textarea>
                                        </td>
                                    </tr>
                                </table>


                            </div>
                        </fieldset>
                    </td>
                </tr>
            </table>
<button type="button" id="test" onclick="FressKeyTap();">s</button>


        </div>
    </form>

    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>

        function renderGea(id, ftype, cc) {
            if (cc != '') {
                var re = cc.split('\n');
                var el = $('<ul>');
                $.each(re, function () {
                    var li = $("<li><a onclick='getContentMessage(\"" + this + "\")'>" + this + "</a></li>");
                    el.append(li);
                })
                $('#divReferent').append(el);
                $('#txtReference').val(cc.replace(/%0A/gi, '\n'));
                return;
            }
            var obj = { P_ID: id, P_TYPE: ftype };
            var $request = $.ajax({
                async: false,
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteReturnInt?packageName=RECEIVELOGFILE_PKG&storeName=getReferenceByIdPerm",
                data: JSON.stringify(obj),
            }).always(function (data) {
                if (data.ListValue == -1) return;
                $('#txtReference').val(data.ListValue);
                var a = data.ListValue.split('\n');
                var el = $('<ul>');
                $.each(a, function (c, d) {
                    var li = $("<li><a onclick='getContentMessage(\"" + d + "\")'>" + d + "</a></li>");
                    el.append(li);
                })
                $('#divReferent').append(el);
            });
        }
        function getContentMessage(vl) {
            clearValue();
            try {
                var ge = '',
                d = '';
                vl = vl.trim();
                ge = vl.split(' ')[0];
                d = vl.split(' ')[1].replace(/^(\d{2})(\d{2})(\d{2})$/, '20$3-$2-$1');
                var obj = { P_GEA: ge, P_DATE: new Date(d) };
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=RECEIVELOGFILE_PKG&storeName=getBy_Gea_Date",
                data: JSON.stringify(obj),
            }).always(function (data) {
                try {
                    $('#txtMessage').val(data.ListValue[0].CONTENT);
                    $('#txtGea').val(data.ListValue[0].NBR);
                    $('#txtDate').val(new Date(data.ListValue[0].LETTERNBR_PK).format('dd/mm/yyyy HH:MM'));
                } catch (e) { }

            });
        } catch (e) {

        }

    }
    function getValueByParam(pa) {
        var url_string = window.location.href;
        var url = new URL(url_string);
        var c = url.searchParams.get(pa);
        return c;
    }
    function clearValue() {
        $('#txtDate').val('');
        $('#txtGea').val('');
        $('#txtMessage').val('');
    }
    </script>
    <script>
        function loaddata() {
            var id = getValueByParam('id');
            var flightType = getValueByParam('fType');
            var ct = getValueByParam('content');
            renderGea(id, flightType, ct);
        }
        loaddata();
        $('#divReferent ul li').each(function (a, b) {
            $(b).on('click', function () {
                $('#divReferent ul li').removeClass('active');
                $(b).addClass('active');
            })
        })
        $('#divReferent ul li').first().click();
    </script>
<script>
    function FressKeyTap() {
        var e = new Event('keydown');
        e.key = 'tab';
        e.keyCode = e.key.charCodeAt(0);
        e.which = e.keyCode;
        e.altKey = false;
        e.ctrlKe = true;
        e.shiftKey = false;
        e.metaKey = false;
        e.bubbles = true;
        document.dispatchEvent(e);
    }
</script>
</body>
</html>
