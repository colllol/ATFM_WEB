<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SearchExtension.aspx.cs" Inherits="prjApplication.Permission.SearchExtension" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />

    <style>
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
    <style>
        input {
            text-transform: uppercase;
        }

        #tblSource {
            font-size: 12px !important;
        }
    </style>

    <table id="tblSearch">
        <tr>
            <td>CallSign</td>
            <td>From</td>
            <td>To</td>
            <td>Craft</td>
            <td>Via</td>
            <td>From date</td>
            <td>To date</td>
            <td>Perm number</td>
            <td>Oper</td>
            <td>Flight type</td>
            <td>Type</td>
            <td>Etd</td>
            <td>Purpose</td>
            <td>Remark</td>
        </tr>
        <tr>
            <td>
                <input id="sCallSign" maxlength="8" class="wid_100px" type="text" /></td>
            <td>
                <input id="sFrom_Airp" class="wid_60px"  type="text" />
            </td>
            <td>
                <input id="sTo_Airp" class="wid_60px"  type="text" />
            </td>
            <td>
                <input id="sCraft" class="wid_75px" data-autocomplete="CRAFT" type="text" />
            </td>
            <td>
                <input id="sVia" maxlength="200" type="text" /></td>
            <td>
                <input data-checkdate="true" id="sToDatePerm" class="wid_90px" type="text" />
            </td>
            <td>
                <input data-checkdate="true" id="sFromDatePerm" class="wid_90px" type="text" />
            </td>
            <td>
                <input id="sPermNbr" maxlength="5" class="wid_80px" type="text" /></td>
            <td>
                <input id="sOper" data-autocomplete="OPER" class="wid_60px" type="text" /></td>
            <td>
                <input id="sFlightType" data-autocomplete="FLIGHTTYPE" class="wid_60px" type="text" />
            </td>
            <td style="width: 90px">
                <input id="sPermType" data-autocomplete="PERMTYPE" class="wid_60px" type="text" />
            </td>
            <td>
                <input id="sEtd" type="text" data-number="true" maxlength="4" class="wid_50px" />
            </td>
            <td>
                <input id="sPurpose" data-autocomplete="PURPOSE" type="text" class="wid_60px" />
            </td>
            <td>
                <input id="sRemark" maxlength="200" type="text" class="wid_100px" /></td>
        </tr>
    </table>


    <button type="button" id="btnSearchExtension" class="btn btn-sm btn-primary" onclick="btnSearchExtension_Click()">
        Search</button>
    <button type="button" id="btnClearSearchExten" class="btn btn-sm btn-primary" onclick="btnClearSearchExten_Click()">
        Clear</button>
    <table id="tblSource" class="table table-bordered">
        <thead>
            <tr>
                <th></th>
                <th>Permission number</th>
                <th>Permission date</th>
                <th>Daily</th>
                <th>ValidHours</th>
                <th>ValidDate</th>
                <th>Callsign</th>
                <th>Registrator</th>
                <th>From</th>
                <th>To</th>
                <th>Etd</th>
                <th>Type</th>
                <th>Flight type</th>
                <th>Oper</th>
                <th>Purpose</th>
                <th>Craft</th>
                <th>Via</th>
                <th>Remark</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>


    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script>
        function btnSearchExtension_Click() {
            $('#tblSource').attr('data-pageindex', '1');
            $('#tblSource').attr('data-total', 0);
            LoadDataBySearch();
        }
        function LoadDataBySearch() {
            	console.log(getObjectSearch());
	var $request = $.ajax({
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/SearchPermExtension/GetPermBySearch",
                data: getObjectSearch(),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove();
                    preloadImg('tblSource', 'loadingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loadingData');
                    $('#tblSource').paging({
                        onClickButton: 'LoadDataBySearch',
                    });
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unLoadingData('loadingData');
                    $('#tblSource').attr('data-total', '0');
                    return;
                }
                $('#tblSource tbody tr').remove();
                $('#tblSource').attr('data-total', data.ListValue[0]['RECORD_SUM']);
                var strAppend = '';
                $.each(data.ListValue, function (a, b) {
                    $('#tblSource tbody').append("<tr>"
                    + "<td>" + b.RNUM + "</td>"
                    + "<td style='white-space: nowrap;'><a style='cursor: pointer;' onclick=\"getcontentPerm(\'" + returnEmpty(b.PERM_ID) + "\',\'" + returnEmpty(b.FTYPE) + "\',\'" + returnEmpty(b.DAYLY) + "\' )\">" + returnEmpty(b.PERMNBR_ID) + "</a></td>"
                    + "<td>" + returnEmpty(new Date(b.PERMDATE).format('dd-mm-yyyy')) + "</td>"
                    + "<td style='white-space: nowrap;'>" + returnEmpty(b.DAYLY) + "</td>"
                    + "<td style='text-align:center; white-space: nowrap;'>" + returnEmpty(b.VALIDDATE) + "</td>"
                    + "<td style='text-align:center; white-space: nowrap;'>" + returnEmpty(b.VALIDDATEPER) + "</td>"
                    + "<td>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                    + "<td>" + returnEmpty(b.REGISTRATION) + "</td>"
                    + "<td>" + returnEmpty(b.FROM_AIRP) + "</td>"
                    + "<td>" + returnEmpty(b.TO_AIRP) + "</td>"
                    + "<td>" + returnEmpty(b.ETD) + "</td>"
                    + "<td>" + returnEmpty(b.PERMTYPE) + "</td>"
                    + "<td>" + returnEmpty(b.FTYPE) + "</td>"
                    + "<td>" + returnEmpty(b.OPER_ID) + "</td>"
                    + "<td>" + returnEmpty(b.PURPOSE_ID) + "</td>"
                    + "<td>" + returnEmpty(b.CRAFT) + "</td>"
                    + "<td>" + returnEmpty(b.VIA) + "</td>"
                    + "<td>" + returnEmpty(b.REMARK) + "</td>"
                    + "</tr>");
                });
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }
        function getObjectSearch() {
            if ($('#tblSource').attr('data-pageSize') == null) $('#tblSource').attr('data-pageSize', 500);
            if ($('#tblSource').attr('data-pageIndex') == null) $('#tblSource').attr('data-pageIndex', 1);
            var _obj = new Object();
            _obj['FLIGHTNBR'] = $('#sCallSign').val();
            _obj['FROM_AIRP'] = $('#sFrom_Airp').val();
            _obj['TO_AIRP'] = $('#sTo_Airp').val();
            _obj['CRAFT'] = $('#sCraft').val();
            _obj['VIA'] = $('#sVia').val();
            _obj['PERMNBR'] = $('#sPermNbr').val();
            _obj['OPER'] = $('#sOper').val();
            _obj['SEASION'] = '';
            _obj['FLIGHT_TYPE'] = $('#sFlightType').val();
            _obj['PERMTYPE'] = $('#sPermType').val();
            _obj['ETD'] = $('#sEtd').val();
            _obj['REMARK'] = $('#sRemark').val();
            if ($('#sToDatePerm').val() != '')
                _obj['SDATE'] = new Date($('#sToDatePerm').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
            if ($('#sFromDatePerm').val() != '')
                _obj['FDATE'] = new Date($('#sFromDatePerm').val().replace(/^(\d{2})\-(\d{2})\-(\d{4})$/, '$3/$2/$1')).format('yyyy-mm-dd');
            _obj['PURPOSE'] = $('#sPurpose').val();
            _obj['PAGESIZE'] = $('#tblSource').attr('data-pageSize');
            _obj['PAGEINDEX'] = parseInt($('#tblSource').attr('data-pageIndex')) - 1;
            return _obj;
        }
        function returnEmpty(val) {
            return val == null ? "" : val;
        }
        function btnClearSearchExten_Click() {
            $('#sCallSign').val('');
            $('#sFrom_Airp').val('');
            $('#sTo_Airp').val('');
            $('#sCraft').val('');
            $('#sVia').val('');
            $('#sPermNbr').val('');
            $('#sDatePerm').val('');
            $('#sOper').val('');
            $('#sFlightType').val('');
            $('#sPermType').val('');
            $('#sToDatePerm').val('');
            $('#sFromDatePerm').val('');
            $('#sEtd').val('');
            $('#sRemark').val('');
            $('#sPurpose').val('');
        }
        $('#tblSearch input[type="text"]').each(function () {
            $(this).ValidateTip();
        })
        function getcontentPerm(a,b,c) {
            //22-MAY-2023
           var parsedDate = Date.parse(c);
           if (isNaN(c) && !isNaN(parsedDate)) {
                console.log(isNaN(c));
            }
           if (b == 'SC')
           
                window.open('<%= Page.ResolveUrl("~/Permission/View_PermSC.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] + "&ID=" %>' + a.trim(), '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight);
            else
                window.open('<%= Page.ResolveUrl("~/Permission/View_PermNo.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] + "&ID=" %>' + a.trim(), '_blank', 'toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight);
        }
       
    </script>

</asp:Content>

