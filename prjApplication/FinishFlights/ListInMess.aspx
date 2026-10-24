<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListVia.aspx.cs" Inherits="prjApplication.FinishFlights.ListVia" %>

<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        #tblSource input, select {
            color: black;
        }

        .table > thead > tr {
            background-color: #fff;
            background-image: none;
            color: black;
			
        }

        .cssTrung {
            color: seagreen !important;
        }

        #tblSource > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
            padding: unset;
        }
        .table-bordered,th {
			border-radius: 0!important;
			height:35px;background-color: #f5f5f5;
		}
		 .table-bordered,td {
			border-radius: 0!important;
			height:35px;
			
		}
        #tblSource tr > td, tr > th {
            padding: unset;
            vertical-align: middle;
            text-align: center;
        }

        .sInput {
            /*border-width: 0px !important;*/
            border: 0px;
            width: 100%;
        }

        input[type=text] {
            border: 0px;
        }

        .success {
            background-color: blue;
        }

        .rowCreate {
            background-color: mediumvioletred;
        }

        input, select, label, textarea {
            text-transform: uppercase;
        }

        caption {
            text-align: left;
            padding-bottom: 0px;
            padding-top: 0px;
        }
    </style>
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
        #tblSource tbody tr.select input {
            background-color: darkseagreen;
        }

        #tblSource tbody tr.select {
            background-color: darkseagreen;
        }
    </style>
    <div id="abcxyz" class="well well-sm" style="text-align: center;">
        <b>DATE :</b>
        <input id="txtFromDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
		<!--
        <b>TO :</b>
        <input id="txtToDate" data-minlenght="1" data-control="checkAccess" type="text"
            placeholder="select date" class="wid_100px" />
        -->
		<b>CAllSIGN :</b>
		<input id="txt_popCAllSIGN" type="text" class="wid_80px" />
		<b>FROM :</b>
		 <input id="txt_popFROM" type="text" data-autocomplete="AERO" class="wid_80px" />
		<b>TO :</b>
		<input id="txt_popTO" type="text" data-autocomplete="AERO" class="wid_80px" />
		<b>TYPE :</b>
		<select id="ddlSelectPop" class="wid_80px" style="height: 30px;">
                            <option selected value="--">--</option>
                            <option value="FPL">FPL</option>
                            <option value="DEP">DEP</option>
                            <option value="DLA">DLA</option>
                            <option value="CHG">CHG</option>
                            <option value="CNL">CNL</option>
                            <option value="ARR">ARR</option>
                           
                        </select>
		<b>CONTENT :</b>
        <input id="txtContent"  type="text"
            class="wid_200px" />
		<b class="hidden">P_SIZE :</b>
        <select id="ddlPageSize" class="disabled hidden" style="width: 65px;">
            <option value="100">100</option>
            <option value="500">500</option>
            <option value="1000">1000</option>
            <option value="2000">2000</option>            
            <option value="4000">4000</option>
            <option value="6000">6000</option>
            <option value="8000">8000</option>
        </select>
       
        <button type="button" id="btnSearch" class="btn btn-sm btn-primary" style="width: 100px" onclick="btnSearch_OnClick()">
            Search</button>
		<button type="button" id="btnExport" class="btn btn-sm btn-primary hidden" style="width: 135px" onclick="LoadDataGrid_Export()">
            Export Excel</button>
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
   
	<!--FLIGHTNBR: "HVN1194"
				FROM_AIRP: "VVTS"
				LETTERNBR_PK: "2020-02-18T12:40:18"
				LETTER_TYPE: "DEP"
				TEXT: "(DEP-HVN1194/A6012-VVTS1238-VVCI-DOF/200218)"
				TO_AIRP: "VVCI"-->

    <label class="radio-inline">
        <span id="totalsfinished">Tổng số : <b>0</b></span></label>
    <div id="exportid" runat="server">
<!--letternbr_pk, letter_type,flightnbr,from_airp,to_airp,TEXT-->
        <table id="tblSource" class="table table-bordered">
           <thead style="color: red"><tr> 
		   <th class="wid_20px">NO</th>
		   <th class="wid_80px">FLIGHTDATE</th>
		   <th class="wid_40px">TYPE</th>
		   <th class="wid_80px">FLIGHTNBR</th>
		   <th class="wid_50px">FROM</th>
		   <th class="wid_50px">TO</th>
		   <th>TEXT</th>
		   <th class="wid_50px">DATERECEVIE</th>
		   </tr></thead>
            <tbody>
			
			</tbody>
        </table>

    </div>
    <br />
    <asp:HiddenField ID="hfGridHtml" runat="server" />
    <iframe id="txtArea1" style="display: none"></iframe>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>

    <script>
        var isTarget = true;
        var qEdit = '<%= _Role.R_Edit %>';
        var qDel = '<%= _Role.R_Del %>';       
        var ddlPageSize = document.getElementById('ddlPageSize');       
        var isSearch = false;
        var rowId = 0;
        var pageSize = parseInt(ddlPageSize.value);
        $('#tblSource').paging({ pageSize: pageSize });
       

     
       

        function returnEmpty(val) {
            return val == null ? "" : val;
        }
		/*
		FLIGHTNBR: "HVN1194"
				FROM_AIRP: "VVTS"
				LETTERNBR_PK: "2020-02-18T12:40:18"
				LETTER_TYPE: "DEP"
				TEXT: "(DEP-HVN1194/A6012-VVTS1238-VVCI-DOF/200218)"
				TO_AIRP: "VVCI"
		*/
        function LoadDataGrid() {
			//console.log(JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtFromDate').val(), P_CONT: $('#txtContent').val(),P_LETTER_TYPE:$('#ddlSelectPop').val() })); 
            var _urlPath = "";
           
            _urlPath= "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetInMess"
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtFromDate').val(), P_CONT: $('#txtContent').val(),P_LETTER_TYPE:$('#ddlSelectPop').val(),P_FLIGHTNBR:$('#txt_popCAllSIGN').val() ,P_FROM_AIRP:$('#txt_popFROM').val(),P_TO_AIRP:$('#txt_popTO').val()}),
                beforeSend: function () {
                    
                },
                
            }).always(function (data) {
                //console.log(data);
				if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }
				//console.log(data.ListValue.length);
					$('#tblSource tbody tr').remove();
                    $('#tblSource').attr('data-total', 0);
                   // preloadImg('tblSource', 'loaddingData', '25px', '25px');
					
               $('#tblSource').attr('data-total', data.ListValue.length);
				var strAppend = RenderTableKhExport(data);
				
						
              
               $('#tblSource tbody').append(strAppend);
               $("#totalsfinished").html("Tổng số : <b>" + data.ListValue.length + "</b>");
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }

      
        
        function reloadCheckValid() {
            $(document).ready(function () {
                $('[data-number="true"]').keypress(validateNumber);
                $('[data-minlenght]').keyup(validateEmty1);
                $('[data-minlenght]').each(function () {
                    var el = $(this)[0];
                    var val = el.getAttribute('data-minlenght');
                    if (el.value.length < val) {
                        el.style.border = "red solid 1px";
                    }
                    else {
                        el.style.border = "0px";
                    }
                });
            });
            try {
                $('[data-CheckDate="true"]').each(function () {
                    $(this).on('blur', function () {
                        checkInputDate1($(this));
                    });
                });
            } catch (e) {

            }
        }
        function validateEmty1(event) {
            var el = $(this)[0];
            var val = el.getAttribute('data-minlenght');
            if (el.value.length < val) {
                el.style.border = "red solid 1px";
            } else { try { el.style.border = "0px"; } catch (er) { } }
        }
        function checkInputDate1(ele) {
            var $ele = $(ele);
            var v = $(ele).val();
            if (v.length < 8) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 8)
                v = $ele.val().replace(/^(\d{2})(\d{2})(\d{4})$/, '$1/$2/$3');
            if (v.length > 10 || v.length == 9) {
                $ele.css('border', '1px solid red');
                $ele.val('');
                return;
            }
            if (v.length == 10) {
                v = v.replace(/-/g, '/');
                var d = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$1');
                var m = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$2');
                var y = v.replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3');
                var chk = new Date(y + '/' + m + '/' + d);
                if (chk == 'Invalid Date') {
                    $ele.css('border', '1px solid red');
                    $ele.val('');
                }
                else {
                    $ele.css('border', '');
                    $ele.val(chk.format('dd-mm-yyyy'));
                }
            }
        }

        function onmouseoverInput(id) {
            inputId = id;
        }
        function onmouseoutInput(id) {
            inputId = '';
        }
        function checkIsUpdate(id) {
            if ($(id).attr('data-isInsert') != undefined) return;
            $inputs = $(id).closest('tr').find('[data-oldValue]');
            var ci = 0;
            $.each($inputs, function (a, b) {
                switch ($(b).attr('type')) {
                    case 'text':
                        if ($(b).attr('data-oldValue').toUpperCase() != $(b).val().toUpperCase()) { ci++; };
                        break;
                    case 'checkbox':
                        if ($(b).attr('data-oldValue') != ($(b).prop('checked') ? '1' : '0')) ci++;
                        break;
                }
            })
            if (ci > 0) { $(id).closest('tr').attr('data-isUpdate', 'true'); $(id).closest('tr').addClass('success'); }
            else { $(id).closest('tr').attr('data-isUpdate', 'false'); $(id).closest('tr').removeClass('success'); }
        }
        
    </script>

    <script>
        function btnSearch_OnClick() {
			LoadDataGrid();
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

    </script>
    
    <script>
        
		$('#txtFromDate').val(dateFormat(new Date().setDate(new Date().getDate()), 'dd-mm-yyyy'));
        $('#txtFromDate').multiDate();
        $('#txtToDate').val(dateFormat(new Date().setDate(new Date().getDate()), 'dd-mm-yyyy'));
        $('#txtToDate').multiDate();	
		//LoadDataGrid();   		
    </script>
    
    
    <script type="text/javascript">
        
        function generate_excel(html) {

            //OK
            exportExel(html);

        }

        function exportExel(_html) {

            var dt = new Date();
            var day = dt.getDate();
            var month = dt.getMonth() + 1;
            var year = dt.getFullYear();
            var hour = dt.getHours();
            var mins = dt.getMinutes();
            var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;

            var textToSave = _html;
            var textToSaveAsBlob = new Blob([textToSave], { type: "text/plain" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var fileNameToSaveAs = 'exported_day_flight_' + postfix + '.xls';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();

        }
        function destroyClickedElement(event) {
            document.body.removeChild(event.target);
        }
        function RenderTableKhExport(data) {
            var kq = '';
            var idx = parseInt($('#tblSource').attr('data-pageindex'));
            var pz = parseInt(ddlPageSize.value);
            //var stt = parseInt(((idx - 1) * pz) + 1);
			//letternbr_pk, letter_type,flightnbr,from_airp,to_airp,TEXT
			/*FLIGHTNBR: "HVN1194"
				FROM_AIRP: "VVTS"
				LETTERNBR_PK: "2020-02-18T12:40:18"
				LETTER_TYPE: "DEP"
				TEXT: "(DEP-HVN1194/A6012-VVTS1238-VVCI-DOF/200218)"
				TO_AIRP: "VVCI"
			*/
			var stt = 1;
            if (data.ListValue == null) return '';
            $.each(data.ListValue, function (a, b) {
               kq += "<tr>"
                    + "<td style=\'text-align: center;\'>" + stt + "</td>"
					 + "<td style=\'text-align: center;\'>" + returnEmpty(b.flightdate) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.LETTER_TYPE) + "</td>"
                    + "<td style=\'text-align: center;\'>" + returnEmpty(b.FLIGHTNBR) + "</td>"
                 + "<td style=\'text-align: center;\'>" + returnEmpty(b.FROM_AIRP) + "</td>"
                      + "<td style=\'text-align: center;\'>" + returnEmpty(b.TO_AIRP) + "</td>"                
                   + "<td style=\'text-align: left;\'>" + returnEmpty(b.TEXT) + "</td>"  
				   + "<td style=\'text-align: center;\'>" + returnEmpty(b.LETTERNBR_PK) + "</td>"
                    + "</tr>";
                stt++;
            });
            return kq;
        }
        function LoadDataGrid_Export() {
            var _urlPath = "";
           
            _urlPath= "api/ApiExtension/ExcuteTable?packageName=A_TEST_SEARCH&storeName=GetDayFlight2021"
            var $request = $.ajax({
                method: "PUT",
                url: urlApi + _urlPath,
                data: JSON.stringify({ P_DATE: $('#txtFromDate').val(), P_EDATE: $('#txtToDate').val(), P_FROM_AIRP: $('#txtFromAir').val(),P_TO_AIRP: $('#txtToAir').val() ,P_OPER_ID: $('#txtOPER_ID').val() }),
                beforeSend: function () {
                    $('#tblSource tbody tr').remove(); 
                    $('#tblSource').attr('data-total', 0);
                    preloadImg('tblSource', 'loaddingData', '25px', '25px');
                },
                complete: function () {
                    unLoadingData('loaddingData');
                },
            }).always(function (data) {
                if (data.ListValue == null) {
                    unLoadingData('loaddingData');
                    return;
                }


                var strAppend = RenderTableKhExport(data);
                 var strHtml = "<table id='tblSource' class='table table-bordered'>";
                strHtml += "<thead style='color: red'>"
                        + "<tr>"
                        + "<th>NO</th>"
						+ "<th>PERMNBR</th>" 
                        + "<th>OPER</th>"
                        + "<th>CALLSIGN</th>"
                        + "<th>CRAFT</th>"
                        + "<th>PURPOSE</th>"
                        + "<th>P_TYPE</th>"
                        + "<th>FROM</th>"
                        + "<th>TO</th>"
                        + "<th>FLIGHTDATE</th>"
                        + "<th>ETD</th>"
                        + "<th>ETA</th>"
                        + "<th>VIA</th>" 
						
                        + "<th>REMARK</th>"         
                        + "</tr>"
                        + "</thead>"
                        + "<tbody>"
						+ strAppend
                        + "</tbody>"
                        + "</table>";
                generate_excel(strHtml);
            });
            $request.onreadystatechange = null;
            $request.abort = null;
            $request = null;
        }





    </script>


</asp:Content>
