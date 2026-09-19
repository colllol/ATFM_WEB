<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM_New.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table id="tblSource" class="table table-bordered table-hover">
        <caption>
            <label>
                <input id="ckIsCheck" checked type="radio" onchange="chkIsCheck();" name="optradio" />
                MessIsCheck
            </label>
            <label>
                <input id="ckNotCheck" type="radio" onchange="chkNotCheck();" name="optradio" />
                MessNotCheck
            </label>
            <button type="button" id="btnSearch" class="btn btn-sm btn-primary" onclick="btnSearch_OnClick()">
                Search</button>
            <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" onclick="btnClearSearch_OnClick()">
                Clear search</button>
        </caption>
        <thead>
            <tr style="background-color: unset">
                <th></th>
                <th>
                    <input id="txtFLIGHTNBR" class="wid_75px" type="text" /></th>
                <th>
                    <input id="txtREGISTRATION" class="wid_70px" type="text" /></th>
                <th>
                    <input id="txtFROM_AIRP" data-autocomplete="AERO" class="wid_60px" type="text" />
                </th>
                <th>
                    <input id="txtTO_AIRP" data-autocomplete="AERO" class="wid_60px" type="text" />
                </th>
                <th>
                    <input id="txtETD" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtATD" class="wid_50px" />
                </th>
                <th>
                    <input id="txtETA" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtATA" class="wid_50px" />
                </th>
                <th>
                    <input id="sctLETTERTYPE" class="wid_50px" data-autocomplete="LETTER_TYPE" type="text" />
                </th>
                <th>
                    <input id="txtFLIGHTDATE" onblur="checkInputDate(this)" data-date-format="dd/mm/yyyy" class="wid_85px" type="text" /></th>
                <th>
                    <input id="txtCRAFT_TYPE" data-autocomplete="CRAFT" class="wid_50px" type="text" />
                </th>
                <th>
                    <input id="txtVIA" class="wid_200px" type="text" /></th>
                <th>
                    <input id="txtREMARK" class="wid_200px" type="text" /></th>
            </tr>
            <tr style="color: white">
                <%--<th></th>--%>
                <th>STT</th>
                <th>CallSign</th>
                <th>Regis</th>
                <th>From</th>
                <th>To</th>
                <th>Etd</th>
                <th>Atd</th>
                <th>Eta</th>
                <th>Ata</th>
                <th>Letter Type</th>
                <th>Perm date</th>
                <th>Craft</th>
                <th>Via</th>
                <th>Remark</th>
            </tr>
        </thead>
        <tbody></tbody>
    </table>
    <script>
        LoadDataGrid();

        var _urlAPI = 'http://localhost/qlb.api/';
        function Render2Table(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';

            $.each(data.ListValue, function (a, b) {
                kq += "<tr id='" + b.ID + "' data-isUpdate='false' onmouseover='rowId=" + b.ID + ";' onmouseout='rowId=0;'>"
                    + "<td>" + stt + "</td>"
                    + "<td>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td>" + b.FROM_AIRP + "</td>"
                    + "<td>" + b.TO_AIRP + "</td>"
                    + "<td>" + returnEmpty(b.ETD) + "</td>"
                    + "<td>" + returnEmpty(b.ATD) + "</td>"
                    + "<td>" + returnEmpty(b.ETA) + "</td>"
                    + "<td>" + returnEmpty(b.ATA) + "</td>"
                    + "<td>" + returnEmpty(b.LETTER_TYPE) + "</td>"
                    + "<td>" + new Date(b.FLIGHTDATE).format('dd-mm-yyyy') + "</td>"
                    + "<td>" + returnEmpty(b.CRAFT_TYPE) + "</td>"
                    + "<td>" + returnEmpty(b.VIA).replace(/\n/gi, '') + "</td>"
                    + "<td>" + returnEmpty(b.REMARK) + "</td>"
                    + "</tr>";
                stt++;
            });
            return kq;
        }
        function returnEmpty(val) {
            return val == null ? "" : val;
        }

        function LoadDataGrid() {
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                //url: urlApi + "api/ApiExtension/ExcuteTable?packageName=PERM_PKG&storeName=validFlightNbr",
                //data: JSON.stringify({ P_FLIGHT_TYPE: 'NO', P_FLIGHTNBR: $('#txtPERMNBR').val().toUpperCase(), P_PERMTYPE: $('#ddlPERMTYPE').val().toUpperCase() }),
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=GroupAddress_GetAll",
                //url: _urlAPI + "api/PlanMessage/MessIsCheck",
                data: {},
                //data: GetObjectSearch(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataGrid',
                        pageSize: pageSize,
                    });
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = Render2Table(data);
                $('#tblSource tbody').append(strAppend);
            });
        }

    </script>
</asp:Content>
