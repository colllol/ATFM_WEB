<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="MessInboxReceive.aspx.cs" Inherits="prjApplication.MessManagement.MessInboxReceive" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .table>thead>tr>th {
            border-color: #ddd;
            background-color: #428bca;
            font-weight: 700;
    </style>
    <style>
        input, textarea {text-transform:uppercase;}
        .preloader {
            display: inline-block;
            padding: 0px;
            border-radius: 100%;
            border: 2px solid;
            border-top-color: rgba(0,0,0, 0.65);
            border-bottom-color: rgba(0,0,0, 0.15);
            border-left-color: rgba(0,0,0, 0.65);
            border-right-color: rgba(0,0,0, 0.15);
            -webkit-animation: preloader 0.8s linear infinite;
            animation: preloader 0.8s linear infinite;
        }

        @keyframes preloader {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        @-webkit-keyframes preloader {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }
    </style>

    <table id="tblSource" class="table table-bordered table-hover">
        <caption> 
            <button type="button" class="btn btn-sm btn-primary btn-bold" id="btnApply" onclick="btnApply_OnClick()">
                Apply
            </button>
            <label>
                <input id="ckkhb" checked type="radio" onchange="chkDienVanKHB_CheckedChanged();" name="optradio"/>
                DienVanKHB
            </label>
            <label>
                <input id="ckSkhb" type="radio" onchange="chkDienVanSaiKHB_CheckedChanged();" name="optradio"/>
                DienVanSaiKHB
            </label>
            <button type="button" id="btnSearch" class="btn btn-sm btn-primary" onclick="btnSearch_OnClick()">
                Search</button>
            <button type="button" id="btnClearSearch" class="btn btn-sm btn-primary" onclick="btnClearSearch_OnClick()">
                Clear search</button>
        </caption>
        <thead>
            <tr style="background-color: unset">
                <th>
                    <input id="chkAll" onchange="ChekAll();" type="checkbox"/>
                </th>
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
                    <input id="txtATD" class="wid_50px"/>
                </th>
                <th>
                    <input id="txtETA" class="wid_50px" type="text" /></th>
                <th>
                    <input id="txtATA" class="wid_50px"/>
                </th>
                <th>
                    <input id="sctLETTERTYPE" class="wid_50px" data-autocomplete="LETTER_TYPE" type="text"/>
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
                <th></th>
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


    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>

    <script>
        var timeRefresh = 20;
        var isSearch = false;
        var rowId = 0;
        var pageSize = 100;
        $('#tblSource').paging({ pageSize: pageSize });
    </script>

    <script>
        function Render2Table(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt($('#tblSource').attr('data-pagesize'));
            var stt = parseInt(((idx - 1) * pz) + 1);
            if (data.ListValue == null) return '';

            $.each(data.ListValue, function (a, b) {
                kq += "<tr id='" +   b.ID + "' data-isUpdate='false' onmouseover='rowId=" + b.ID + ";' onmouseout='rowId=0;'>"
                    //+ "<td><input type='checkbox' ID='chkItem'/></td>"
                    + "<td><input id='chkItem' type=\"checkbox\" " + (b.IS_CHECK == "0" ? "\"\"" : "checked") + " data-oldValue='" + (b.IS_CHECK == '0' ? '0' : '1') + "' onchange='checkIsUpdate(this)' /></td>"
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
                //url: "http://localhost/QLB.API/api/PlanMessage/GetPageRealplanLetter",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/PlanMessage/GetPageRealplanLetter",
                data: GetObjectSearch(),
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
        function btnClearSearch_OnClick() {
            $('#txtFLIGHTNBR').val('');
            $('#txtREGISTRATION').val('');
            $('#txtFROM_AIRP').val('');
            $('#txtTO_AIRP').val('');
            $('#txtETD').val('');
            $('#txtETA').val('');
            $('#txtDATE_OLD').val('');
            $('#txtFLIGHTDATE').val('');
            $('#txtATD').val('');
            $('#txtATA').val('');
            $('#txtCRAFT_TYPE').val('');
            $('#txtVIA').val('');
            $('#txtREMARK').val('');
            $('#sctLETTERTYPE').val('');
            btnSearch_OnClick();
        }
        function btnSearch_OnClick() {
            Load_Data_Search();
        }
        function GetObjectSearch() {
            var _obj = {};
            if (isSearch) {
                $('#tblSource').attr('data-pageIndex', 1);
            }
            _obj['PAGE_SIZE'] = $('#tblSource').attr('data-pageSize');
            _obj['PAGE_INDEX'] = parseInt($('#tblSource').attr('data-pageIndex'));
            _obj['FLIGHTDATE'] = $('#txtFLIGHTDATE').val() == '' ? new Date().format('yyyy-mm-dd') : $('#txtFLIGHTDATE').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1');
            _obj['REGISTRATION'] = $('#txtREGISTRATION').val();
            _obj['FROM_AIRP'] = $('#txtFROM_AIRP').val();
            _obj['TO_AIRP'] = $('#txtTO_AIRP').val();
            _obj['FLIGHTNBR'] = $('#txtFLIGHTNBR').val();
            _obj['VIA'] = $('#txtVIA').val();
            _obj['REMARK'] = $('#txtREMARK').val();
            _obj['ETA'] = $('#txtETA').val();
            _obj['ETD'] = $('#txtETD').val();
            _obj['ATA'] = $('#txtATA').val();
            _obj['ATD'] = $('#txtATD').val();
            _obj['LETTER_TYPE'] = $('#sctLETTERTYPE').val();
            isSearch = false;
            return _obj;
        }
        function chkDienVanKHB_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function chkDienVanSaiKHB_CheckedChanged() {
            $('#tblSource').attr('data-pageIndex', 1);
            Load_Data_Search();
        }
        function Load_Data_Search() {
            var dvSKhb = $('#ckSkhb').prop('checked');
            var dvKhb = $('#ckkhb').prop('checked');
            if (dvKhb) { LoadDataGrid(); }
            if (dvSKhb) { LoadDataGrid_DienVanSaiKHB();}
        }
        function LoadDataGrid_DienVanSaiKHB() {
            var $request = $.ajax({
                //async: false,
                method: "PUT",
                //url: urlApi + "api/PlanMessage/GetPageRealplanLetter",
                url: "http://localhost/QLB.API/api/PlanMessage/RealplanLetterHasChange",
                <%--url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/PlanMessage/RealplanLetterHasChange",--%>
                data: GetObjectSearch(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataGrid_DienVanSaiKHB',
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
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function ChekAll() {
            var $ele = $('#chkAll');
            $('#tblSource input:checkbox').each(function () {
                $(this).prop('checked', $ele.prop('checked'));
            });
        }
        function checkIsUpdate(id) {
            if ($(id).attr('data-isInsert') != undefined) return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci = 0;
            $.each($inputs, function (a, b) {
                switch ($(b).attr('type')) {
                    case 'text':
                        if ($(b).attr('data-oldValue') != $(b).val()) { ci++; };
                        break;
                    case 'checkbox':
                        if ($(b).attr('data-oldValue') != ($(b).prop('checked') ? '1' : '0')) ci++;
                        break;
                }
            })
            if (ci > 0) { $(id).closest('tr').attr('data-isUpdate', 'true'); $(id).closest('tr').addClass('success'); }
            else { $(id).closest('tr').attr('data-isUpdate', 'false'); $(id).closest('tr').removeClass('success'); }

        }
        function btnApply_OnClick() {
            //var $lis = $('tr[data-isUpdate="true"]');
            //var $lisSN = $(('#tblSource input:checkbox').find('[id^="chkItem"]')).prop('checked');
            var $lisSN = $('tr[data-isUpdate="true"]');
            var c = 0;
            $.each($lisSN, function (a, b) {
                var _obj = {};
                _obj['IS_CHECK'] = $($(b).find('[id^="chkItem"]')).prop('checked') == true ? '1' : '0';
                _obj['ID'] = $(b).prop('id');
                var $request = $.ajax({
                    async: false,
                    method: "PUT",
                    url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/PlanMessage/AccessRealplanLetter",                  
                    data: _obj,                    
                }).always(function( data ) {
                    if(data.Value==null)c++;
                });
            })
            Load_Data_Search();
        }
    </script>
    <script>
        LoadDataGrid();
        function runtimeRefresh() {
            LoadDataGrid();
            setTimeout(runtimeRefreshAll, timeRefresh * 1000);
        }
    </script>
</asp:Content>

