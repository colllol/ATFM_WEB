<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="DaylyFlight.aspx.cs" Inherits="prjApplication.Day_Flights.DaylyFlight" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <link href="../Style/spectrum.css" rel="stylesheet" />
    <link href="../Style/assets/css/ace-skins.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/ace-rtl.min.css" rel="stylesheet" />

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
    <style type="text/css">
        #dhtmltooltip {
            position: absolute;
            width: 150px;
            border: 2px solid black;
            padding: 2px;
            background-color: lightyellow;
            visibility: hidden;
            z-index: 100;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }
    </style>
    <style>
        table {
            border-collapse: separate;
            border-spacing: 0;
        }

        #infoTotal {
            position: absolute;
            width: 250px;
            font-size: 11px;
            top: -15px;
        }

        #topBar1 {
            position: absolute;
            left: 250px;
            width: 1000px;
            text-align: center;
            top: -20px;
        }

            #topBar1 span {
                background-color: blue;
            }

        #topBar2 {
            position: absolute;
            left: 250px;
            text-align: center;
            width: 1000px;
            top: 7px;
            font-size: 20px;
        }

            #topBar2 span {
                border: solid black 1px;
                width: 70px;
            }

        #topBar3 {
            position: absolute;
            bottom: 2px;
            right: 20px;
        }

        #lblTimeRefresh {
            position: absolute;
            right: 60px;
            bottom: 0px;
            width: 120px;
        }

        #grdSource span {
            /*border: 1px solid blue;
            border-radius: 4px;
            color: forestgreen;
            padding: 2px 3px;*/
            font-weight: 550;
        }

            #grdSource span:hover {
                cursor: pointer;
            }

        #grdSource > tr.active {
            background-color: red;
        }

        #grdSource input:focus {
            width: 180px !important;
        }

        #divStatusIcon {
            font-size: 13px;
            width: 10px;
        }

        #grdSource .tdIconStatus {
            white-space: nowrap;
            background-color: ghostwhite;
            color: chocolate;
            width: 10px !important;
        }

        #grdSource .iconSearch {
            cursor: pointer;
        }

        #grdSource thead tr td {
            text-transform: uppercase;
            font-weight: 700;
            color: black;
        }

        #grdSource > tbody > tr > td {
            line-height: 0.7;
        }

        #grdSource {
            font-size: 12px;
        }

        #ddlPERMTYPE {
            position: absolute;
            right: 520px;
            height: 25px;
            width: 90px;
            font-size: 12px;
            bottom: 0px;
        }
         #ddlSelect {
            position: absolute;
            right: 420px;
            height: 25px;
            width: 90px;
            font-size: 12px;
            bottom: 0px;
        }
        #btnExport {
            position: absolute;
            right: 140px;
            height: 28px;
            width: 100px;
            font-size: 12px;
            bottom: 0px;
        }


        #grdSource tr td:first-child + td + td {
            white-space: nowrap;
        }

        #grdSource tr td:last-child {
            display: none;
        }

        #grdSource tr th:last-child {
            display: none;
        }

        .divHeader {
            position: relative;
            border-bottom: dotted 1px black;
            height: 80px;
            margin-bottom: 8px;
        }

        .btn.active {
            background-color: red !important;
        }

        .table > thead > tr.Spec {
            background-position: 0% 0%;
            color: #707070;
            font-weight: 400;
            background-image: unset;
            background-color: unset;
            background-repeat: repeat-x;
            background-attachment: scroll;
        }

        .table-bordered > thead > tr > td.headSpec {
            border: unset;
        }

        .table-bordered {
            border: unset;
        }

        .tableTip {
            font-size: 12px;
            background-color: aquamarine;
            text-align: left;
        }
        /*.dControl{font-size: 12px;
            width: 50px;
            height: 20px;}*/
        .sControl {
            font-size: 12px;
            width: 50px;
            height: 20px;
            font-weight: 100;
            color: black !important;
        }

        #divSelectDate {
            position: absolute;
            left: -700px;
            top: -10px;
        }
    </style>
    <style id="styCSS">
        
    </style>
    <style>
        .icon-quay {
            -webkit-animation: spin 1000ms infinite linear;
            animation: spin 1000ms infinite linear;
        }

        @-webkit-keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }

        @keyframes spin {
            20% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            40% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            60% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }

            80% {
                -webkit-transform: rotate(-45deg);
                transform: rotate(-45deg);
            }

            100% {
                -webkit-transform: rotate(45deg);
                transform: rotate(45deg);
            }
        }

        .modal {
            position: fixed;
            top: 10%;
            left: 10%;
            **overflow-y:scroll**;
        }

        .vertical-alignment-helper {
            display: table;
            height: 100%;
            width: 100%;
        }

        .vertical-align-center {
            /* To center vertically */
            display: table-cell;
            vertical-align: middle;
        }

        .modal-content {
            /* Bootstrap sets the size of the modal in the modal-dialog class, we need to inherit it */
            width: inherit;
            height: inherit;
            /* To center horizontally */
            margin: 0 auto;
        }

        dt {
            font-weight: 300;
        }
    </style>
    <div id="sound"></div>
    <div id="optionColor">
        <div class="ace-settings-container" id="ace-settings-container">
            <div class="btn btn-app btn-xs btn-warning ace-settings-btn" onclick="$('#ace-settings-box').toggleClass('open');"
                id="ace-settings-btn">
                <i class="ace-icon fa fa-cog bigger-130"></i>
            </div>
            <div class="ace-settings-box clearfix" id="ace-settings-box">
                <div class="pull-left width_50">
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar">
                            Sound notification
                            <input id="chkAutoSound" type="checkbox" onchange="isSound = ($('#chkAutoSound').prop('checked'));runtimeSoundNotification();" /></label>

                    </div>
                    <% foreach (DataRow r in new DayFlightSetColorDAL().GetTableColor(_user.UserID.ToString()).Rows)%>
                    <%  {%>
                    <% if (!r["CSSCLASSNAME"].ToString().Contains("FontSize")) %>
                    <%{ %>
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar"><%: r["ALIASNAME"] %></label>
                        <input id="txtMc<%:r["CSSCLASSNAME"] %>" type="text" data-classname="<%: r["CSSCLASSNAME"] %>"
                            data-modecolor="true" data-setcolor="<%: r["COLORCUSTOM"] %>" data-colordefault="<%: r["DEFAULTCOLOR"] %>" />
                        <a style="cursor: pointer" onclick="SetDefaultColor('<%: r["CSSCLASSNAME"] %>', <%: _user.UserID %>)">Default</a>
                    </div>
                    <%} %>
                    <% else %>
                    <%{ %>
                    <div class="ace-settings-item">
                        <label class="lbl" for="ace-settings-navbar"><%: r["ALIASNAME"] %></label>
                        <input id="txtFc<%:r["CSSCLASSNAME"] %>" type="number" data-classname="<%: r["CSSCLASSNAME"] %>"
                            data-modefontsize="true" class="wid_55px hei_25px" onblur="UpdateColorBy('<%= _user.UserID%>', $(this).val(), $(this).attr('data-className'));reStyleColor();"
                            value="<%: r["COLORCUSTOM"] %>" data-setfontsize="<%: r["COLORCUSTOM"] %>" data-fontsizedefault="<%: r["DEFAULTCOLOR"] %>" />
                        <%--<a style="cursor: pointer" onclick="SetDefaultFontSize('<%: r["CSSCLASSNAME"] %>', <%: _user.UserID %>)">
                            Default</a>--%>
                    </div>
                    <% } %>
                    <% } %>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="divHeader">
            <div id="infoTotal">
                <%= RenderTopBarInfo() %>
            </div>
            <div id="topBar1">
                <select id="ddlSelectViewColum" class="sControl wid_60px">
                    <option value="">--</option>
                    <option selected value="ETD">ETD</option>
                    <option value="ETA">ETA</option>
                    <option value="ATD">ATD</option>
                    <option value="ATA">ATA</option>
                    <option value="EOBT">EOBT</option>
                </select>
                <% for (int i = 0; i <= 23; i++)%>
                <% {%>
                <span class="btn btn-default btn-xs" onclick="$(this).toggleClass('active');">
                    <%= i %></span>
                <% } %>
            </div>
            <div id="topBar2">
                <span id="timeUS"></span>
                <span id="totql"></span>
            </div>

            <div id="topBar3">
                <div id="divSelectDate">
                </div>
                <div class="checkbox" style="font-weight: 100; width: 120px!important; position: absolute; right: 260px; bottom: -10px;">
                    <label>
                        <input type="checkbox" id="chkAutoRefresh" onchange="isRefresh = ($('#chkAutoRefresh').prop('checked')); if(isRefresh){cd= timeRefresh;runtimeCountDown();}" />Auto
                        refresh</label>
                </div>
              
                <select id="ddlPERMTYPE">
                    <option value="">All</option>
                    <option value="LD">Landing</option>
                    <option value="O/F">OverFlight</option>
                </select>
                <select id="ddlSelect">
                    <option value="0">-All-</option>
                    <option value="QN">QN</option>
                    <option value="QT">QT</option>
                </select>
                <span id="lblTimeRefresh"></span>

                <div id="topbarNotificon"></div>
                <div>
                    <a href="#" onclick="window.open('<%= Page.ResolveUrl("~/SendMessage/SendMessageFlight.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)"
                        data-toggle="tooltip" title="Send message">
                        <i class="glyphicon glyphicon-send"></i>
                    </a>
                    <%--<a data-toggle="tooltip" href="#" id="urlSendMessage" title="Message arising in the day">
                        <i class="ace-icon fa fa-bell icon-quay"></i><span id="lbltopbarNotificon_TotalMessage"
                            class="badge badge-important"></span>
                    </a>--%>
                    <a href="#" data-toggle="tooltip" title="Flight not permission" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightNotPerm.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="glyphicon glyphicon-remove"></i><span id="lbltopbarNotificon_TotalFlight"
                            class="badge badge-success"></span>
                    </a>
                   <%-- <a href="#" data-toggle="tooltip" title="Flight not complate" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightNotATD.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="ace-icon fa fa-fighter-jet"></i><span id="lbltopbarNotComplate_TotalFlight"
                            class="badge badge-success"></span>
                    </a>--%>

                    <a href="#" data-toggle="tooltip" title="Update status of message" onclick="window.open('<%= Page.ResolveUrl("~/Day_Flights/DaylyFlightCancel.aspx") + "?Menu_Id=" + Request.Params["Menu_ID"] %>','_blank','toolbar=yes,scrollbars=yes,resizable=yes').resizeTo(window.screen.availWidth, window.screen.availHeight).moveTo(0,0)">
                        <i class="glyphicon glyphicon-comment"></i><span id="lbltopbarNo_TotalFlight"
                            class="badge badge-success"></span>
                    </a>
                    <a href="#" data-toggle="tooltip" title="Export Flight Finished" onclick="LoadDataGrid_Finished()">
                        <i class="glyphicon glyphicon-cloud-download"></i><span id="lbltopbarNo_TotalFlight"
                            class="badge badge-success"></span>
                    </a>
                </div>
            </div>
        </div>
    </div>

    <div class="row">

        <div class="table-responsive">
            <table id="grdSource" class="table table-bordered">
                <thead>
                    <tr class="Spec">
                        <td style="width: 10px" class="headSpec"></td>
                        <td class="headSpec">
                            <div class="action-buttons wid_45px">
                                <a data-toggle="tooltip" id="btnSearch" onclick="btnSearch_Onclick()" title="Search">
                                    <i class="glyphicon glyphicon-search"></i>
                                </a>
                                <a id="btnClearInput" onclick="btnClearInput_Onclick();" data-toggle="tooltip" title="Clear value search">
                                    <i class="glyphicon glyphicon-trash"></i>
                                </a>
                            </div>
                        </td>
                        <%--<td class="headSpec">
                            <select id="ddlStatus" class="sControl">
                                <option value="0">--</option>
                                <option value="1">1</option>
                                <option value="2">2</option>
                                <option value="3">3</option>
                            </select>
                        </td>--%>
                        <td class="headSpec">
                            <input id="txtPERMNBR" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input type="text" class="sControl" id="txtREGISTRATION" /></td>
                        <td class="headSpec">
                            <input id="txtFLIGHTNBR" type="text" class="sControl" /></td>

                        <td class="headSpec">
                            <input id="ddlFROM_AIRP" type="text" data-autocomplete="AERO" class="sControl" />
                        </td>
                        <td class="headSpec">
                            <input id="ddlTO_AIRP" type="text" data-autocomplete="AERO" class="sControl" />
                        </td>
                        <td class="headSpec">
                            <input id="txtETD" type="text" class="sControl" style="display: none;" />
                        </td>
                        <td class="headSpec">
                            <input id="txtPtd" type="text" class="sControl" style="display: none;" /></td>
                        <td class="headSpec">
                            <input id="txtETA" type="text" class="sControl" style="display: none;" />
                        </td>

                        <td class="headSpec">
                            <input id="txtAtd" type="text" class="sControl" style="display: none;" /></td>
                        <td class="headSpec">
                            <input id="txtATA" type="text" class="sControl" style="display: none;" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtROUTE_TP" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="ddlCRAFT_T" type="text" class="sControl" data-autocomplete="CRAFT" />
                        </td>
                        <td class="headSpec">
                            <input id="ddlCRAFT_TP" type="text" class="sControl" data-autocomplete="CRAFT" />

                        </td>

                        <td class="headSpec">
                            <input id="txtPURPOSE" type="text" class="sControl" /></td>
                        <td class="headSpec">
                            <input id="txtREMARK" type="text" class="sControl" /></td>
                        <!--<td class="headSpec">
                            <input id="txtFLIGHTDATE" type="text" data-date-format="dd/mm/yyyy" class="sControl date-picker" />
                        </td>-->

                        <td class="headSpec">
                            <input id="txtVIA" type="text" class="sControl" /></td>
                    </tr>
                    <tr>
                        <th style="width: 10px"></th>
                        <th>
                            <select id="ddlLetter_Type" class="sControl">
                                <option value="">--</option>
                                <option value="FPL">FPL</option>
                                <option value="DEP">DEP</option>
                                <option value="DLA">DLA</option>
                                <option value="CHG">CHG</option>
                                <option value="CNL">CNL</option>
                                <option value="ARR">ARR</option>
                                <%--<option value="NO">No Permission</option>--%>
                            </select>
                        </th>
                        <%--<th>Status</th>--%>
                        <th data-sort="1" onclick="sortOnclick(this);">PERM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">REGIS</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CallSign</th>

                        <th data-sort="1" onclick="sortOnclick(this);">FROM</th>
                        <th data-sort="1" onclick="sortOnclick(this);">TO</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">EOBT</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ETA</th>

                        <th data-sort="1" onclick="sortOnclick(this);">ATD</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ATA</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ROUTE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">ROUTE_FPL</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CRAFT_T</th>
                        <th data-sort="1" onclick="sortOnclick(this);">CRAFT_P</th>

                        <th data-sort="1" onclick="sortOnclick(this);">PUPOSE</th>
                        <th data-sort="1" onclick="sortOnclick(this);">REMARK</th>
                        <th data-sort="1" onclick="sortOnclick(this);">VIA</th>
                    </tr>
                </thead>
                <tbody>
                    <%= grdSourceLoadFirt() %>
                </tbody>

            </table>
        </div>
    </div>

    <div id="popPerm" class="modal fade" role="dialog" tabindex="-1" aria-labelledby="myModal"
        data-backdrop="false" aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">x</button>
                    <h4 class="blue bigger">Flight Permission</h4>
                </div>
                <div class="modal-body">


                    <button id="btnCancelpopPerm" type="button"
                        data-dismiss="modal" class="btn btn-sm btn-primary">
                        <i class="ace-icon fa fa-times"></i>
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="popFlightInfoExtension" class="modal fade" role="dialog" tabindex="-1"
        aria-labelledby="myModalLabel"
        data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Flight extension</h4>
                </div>
                <div class="modal-body" id="txtContentExtensionInfoFlight">
                    <div class="wid_50">
                        <!--<div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Change:
                                </label>
                                <label class="mLable text-info" id="txtChange"></label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtValidateTime" class="mLable control-label">
                                    Time
    valid:
                                </label>
                                <label class="mLable text-danger" id="txtValidateTime"></label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtHasPermission" class="mLable control-label">
                                    Has
    permission:
                                </label>
                                <label class="mLable text-primary" style="font-weight: 100"
                                    id="txtHasPermission">
                                </label>
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtContentChange" class="mLable control-label">
                                    Content
    change:
                                </label>
                                <label class="mLable text-uppercase" id="txtContentChange"></label>
                            </div>
                        </div>-->

                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Regis:
                                </label>
                                <input id="txtRegis" type="text" class="wid_100px" />
                                <label for="txtChange" class="mLable control-label">
                                    Remark:
                                </label>
                                <input id="txtRemark" type="text" class="wid_230px" />
                            </div>
                        </div>
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label for="txtChange" class="mLable control-label">
                                    Eobt:
                                </label>
                                <input id="txtEobt" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Atd:
                                </label>
                                <input id="txtAtd2" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Ata:
                                </label>
                                <input id="txtAta" data-minlenght="1" data-control="checkAccess" type="text"
                                    placeholder="time" class="wid_50px" maxlength='4' data-number='true' />
                                <label for="txtChange" class="mLable control-label">
                                    Status   :
                                </label>

                                <select id="ddlSelectView" class="wid_60px" style="height: 30px;">
                                    <option selected value="--">--</option>
                                    <option value="FPL">FPL</option>
                                    <option value="DEP">DEP</option>
                                    <option value="DLA">DLA</option>
                                    <option value="CHG">CHG</option>
                                    <option value="CNL">CNL</option>
                                    <option value="ARR">ARR</option>
                                </select>
                                <button id="btnUpdateStatusLetter" class="btn btn-sm btn-primary" type="button">
                                    Update
                                </button>

                            </div>
                        </div>



                    </div>
                    <textarea id="txtContentMessage" style="margin-top: -100%; margin-left: 50.5%; height: 100%; width: 50%;"
                        rows="7">                            
                        </textarea>
                    <div style="border-top: 1px dotted black;"></div>
                    <div>
                        <fieldset>
                            <legend>Permission</legend>
                            <table>
                                <tr>
                                    <td>
                                        <dl>
                                            <dt>Author</dt>
                                            <dt>
                                                <input id="txtAuthorPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Oper</dt>
                                            <dt>
                                                <input id="txtOperPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Number</dt>
                                            <dt>
                                                <input id="txtFlightNbrPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Type</dt>
                                            <dt>
                                                <input id="txtPermTypePerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Version</dt>
                                            <dt>
                                                <input id="txtVersionPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Season</dt>
                                            <dt>
                                                <input id="txtSeasonPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Date</dt>
                                            <dt>
                                                <input id="txtDatePerm" type="text" class="wid_90px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <dl>
                                            <dt>Hours</dt>
                                            <dt>
                                                <input id="txtHoursPerm" type="text" class="wid_60px" /></dt>
                                        </dl>
                                    </td>
                                    <td>
                                        <label style="font-weight: 300;" id="lblLinkFile"></label>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered" id="tblPermission"></table>
                        </fieldset>
                    </div>

                    <div style="border-top: 1px dotted black;"></div>
                    <div class="row">
                        <div class="table-responsive">
                            <table id="tblDienVan" class="table table-bordered">
                                <caption class="text-left">History Message</caption>
                                <thead>
                                    <tr>
                                        <th>Hour</th>
                                        <th>Date</th>
                                        <th>Type</th>
                                        <th>CallSign</th>
                                        <th>Registration</th>
                                        <th>From</th>
                                        <th>To</th>
                                        <!--<th>ETD</th>
                                        <th>ETA</th>-->
                                        <th>Via</th>
                                        <th>Content</th>
                                        <th>FROM_PL</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr></tr>
                                </tbody>
                            </table>

                        </div>
                    </div>
                    <div class="row" style="text-align: center;">
                        <button id="btnAcceseeInfoChange" class="btn btn-sm btn-primary"
                            style="display: none;">
                            <i class="ace-icon fa fa-times"></i>
                            Access
                        </button>
                        <button id="btnCancel" type="button"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="dhtmltooltip"></div>
    <div id="divExcuteScript"></div>
    <div id="divMessegeChange" hidden="hidden"></div>
    <div id="divAlarm" hidden="hidden"></div>
    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
    <script src="../Scripts/spectrum.js"></script>
    <script src="../Style/assets/js/ace-elements.min.js"></script>
    <script src="../Style/assets/js/ace.min.js"></script>
    <script src="../Scripts/tableHeadFixer.js"></script>
    <script src="../Scripts/CustumStaticdata.js"></script>
    <script>
        var d = new Date();
        $('#divSelectDate').html("<select id='ddlDateFlight' onchange='btnSearch_Onclick();'><option value='-1'>"
                    + dateFormat(new Date().setDate(new Date().getDate() - 1), 'dd/mm/yyyy')
    + "</option>"
                    + "<option selected value='0'>" + dateFormat(new Date(), 'dd/mm/yyyy')
    + "</option><option value='1'>" + dateFormat(new Date().setDate(new Date().getDate()
    + 1), 'dd/mm/yyyy') + "</option></select>");
        function SelectedDateFlight_Change() {
            btnSearch_Onclick();
        }
    </script>
    <script>
        function checkTime(m) {
            if (m < 10)
                m = "0" + m	// add zero in front of minutes < 10
            return m
        }
        function cKhungGio1(m) {
            var today = new Date();
            if (m < 10)
                m = "0" + m + "00"	// add zero in front of minutes < 10
            else m = m + '00'
            return m
        }
        function cKhungGio2(m) {
            if (m < 10)
                m = "0" + m + "59"	// add zero in front of minutes < 10
            else m = m + '59'
            return m
        }
        function start() {
            var today = new Date();
            var h = today.getHours();
            var m = today.getMinutes();
            var s = today.getSeconds();
            var today2 = new Date();
            var utc = today2.getTime() + (today2.getTimezoneOffset() * 60000);
            var nd = new Date(utc + (3600000 * 0)); /* timezone + 9*/
            var h2 = nd.getHours();
            var m2 = nd.getMinutes();
            var s2 = nd.getSeconds();
            //document.getElementById('timeVN').innerHTML = checkTime(h) + ":" + checkTime(m) + ":" + checkTime(s)
            document.getElementById('timeUS').innerHTML = checkTime(h2) + ":" + checkTime(m2) + ":" + checkTime(s2);
            var t = setTimeout(start, 1 * 1000);
        }
        var _hide_details = false
        var count = 0
        start();
    </script>
    <script>
        $('body').attr('onSubmit', 'return false;');
    </script>
    <script>
        var isSound = false;
        var appenLoad = 50;
        var phanCach = '<%= _phanCach%>';
        var phanCachArg = '<%= _phanCachArg %>';
        var khungGio = [0, 23];
        var loadTopbarInfo = true;
        var loadGrdSource = false;
        var loadTopbar3 = false;
        var isRefresh = false;
        var isSearch = false;
        var isSroll = false;
        var timeRefresh = 20;
        var cd = timeRefresh;
        var sObj = JSON.parse('<%= _ObjSearch%>');
        var btnSearch = document.getElementById('btnSearch');
        var ddlStatus = document.getElementById('ddlStatus');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var txtFROM_AIRP = document.getElementById('ddlFROM_AIRP');
        var txtTO_AIRP = document.getElementById('ddlTO_AIRP');
        var txtETD = document.getElementById('txtETD');
        var txtETA = document.getElementById('txtETA ');
        var txtPtd = document.getElementById('txtPtd');
        var txtAtd = document.getElementById('txtAtd');
        var txtATA = document.getElementById('txtATA');
        var ddlCRAFT_TP = document.getElementById('ddlCRAFT_TP');
        var ddlCRAFT_T = document.getElementById('ddlCRAFT_T');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var txtPURPOSE = document.getElementById('txtPURPOSE');
        var ddlMTOW = document.getElementById('ddlMTOW');
        var txtFLIGHTDATE = document.getElementById('txtFLIGHTDATE');
        var txtFLIGHT_TYPE = document.getElementById('txtFLIGHT_TYPE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtDATE_OLD = document.getElementById('txtDATE_OLD');
        var txtROUTE = document.getElementById('txtROUTE');

        $("#grdSource tbody tr td").attr('valign', 'center');
        function btnClearInput_Onclick() {
            $('#grdSource input').val('')
            //$('#ddlStatus').prop('selectedIndex', 0);
            //$('#ddlCRAFT_TP').prop('selectedIndex', 0);
            //$('#ddlCRAFT_T').prop('selectedIndex', 0);
            $('#ddlMTOW').prop('selectedIndex', 0);
            //$('#ddlFROM_AIRP').prop('selectedIndex', 0);
            //$('#ddlTO_AIRP').prop('selectedIndex', 0);
            $('#ddlLetter_Type').prop('selectedIndex', 0);
        }
        function getValueSearchDaylyFlight() {
            var khungGio = [0, 23];
            var ax = document.getElementById('topBar1').getElementsByClassName('active');
            if (ax.length == 1)
                khungGio = [ax[0].innerText, ax[0].innerText];
            else if (ax.length == 0)
                khungGio = [0, 23];
            else khungGio = [ax[0].innerText, ax[ax.length - 1].innerText];



            var _obj = {
                //STATUS: document.getElementById('ddlStatus').value,
                PERMNBR: $('#txtPERMNBR').val(),
                FLIGHTNBR: $('#txtFLIGHTNBR').val(),
                FROM_AIRP: $('#ddlFROM_AIRP').val(),
                TO_AIRP: $('#ddlTO_AIRP').val(),
                ETA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETA' ? 'ETA'
    : ''),
                ETD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ETD' ? 'ETD'
    : ''),
                ATD: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATD' ? 'ATD'
    : ''),
                ATA: isRefresh ? '' : ($('#ddlSelectViewColum').val() == 'ATA' ? 'ATA'
    : ''),
                CRAFT_ID: $('#ddlCRAFT_T').attr('data-craftid'),
                CRAFT_TYPE: $('#ddlCRAFT_TP').val(),
                REGISTRATION: $('#txtREGISTRATION').val(),
                PURPOSE: $('#txtPURPOSE').val(),
                //FLIGHTDATE: $('#txtREMARK').val(),
                REMARK: $('#txtREMARK').val(),
                FLIGHT_TYPE: $('#ddlSelect').val(),
                PERMTYPE: $('#ddlPERMTYPE').val(),
                KHUNGGIO1: cKhungGio1(khungGio[0]),
                KHUNGGIO2: cKhungGio2(khungGio[1]),
                LETTER_TYPE: $('#ddlLetter_Type').val(),
                RowStart: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + 1) : 0,
                RowFinish: isSroll ? (parseInt($('#grdSource tbody tr').last().attr('data-RowNumber'))
    + appenLoad) : 20,
                OptionDate: $('#ddlDateFlight').val(),
                ROUTE: $('#txtROUTE').val(),
                ROUTE_TT: $('#txtROUTE_TP').val()
            }
            sObj = _obj;
            return _obj;
        }
        function DisplayResult(resulf, context) {
            if (context == 'btnSearch_Onclick') {
                var array = resulf.split('&&&&');

                //alert(array[1])

                $("#totql").html("Total : <b>" + array[1] + "</b>");

                $('#grdSource tbody tr').remove();
                $('#grdSource tbody').append(array[0]);
                $("#grdSource").tableHeadFixer({ "head": true, "left": 4 });
                unPreLoadData();
            }
            if (context == 'RenderTopBarInfo') {
                $('#divExcuteScript').html(resulf);
                isFirtLoadAlarm = false;
                if ($('#divMessegeChange').html() != '')
                    notifyMe($('#divMessegeChange').html().replace(/<br>/gi, '\n'));
            }
            if (context == 'LoadGrdSourceScroll') {
                $('#grdSource tr').last().after(resulf).fadeIn();
                $("#grdSource").tableHeadFixer({ "head": true, "left": 4 });
                unPreLoadData();
            }
            if (context == 'viewPopupInfoExtension') {
                $('#divExcuteScript').html(resulf);
                $('#txtContentMessage').val($('#txtContentMessage').val().replace(/<br>/gi,
    '\n'));
                $('#divExcuteScript').html('');
                $('#popFlightInfoExtension').modal('show');
                $('#tblPermission tbody tr td').each(function () {
                    var $el = $(this);
                    if ($($el).text() == 'null')
                        $($el).text('');
                });
            }
            if (context == 'btnAccessInfoChange_Onclick') {
                $('#divExcuteScript').html(resulf);
                btnSearch_Onclick();
            }
            if (context == 'UpdateColorBy') {
                alert(resulf);
            }
            if (context == 'SetDefaultColor') {
                alert(resulf);
            }
            if (context == 'GetMessageAlert') {
                $('#divExcuteScript').html(resulf);
            }
            if (context == 'RuntimeSoundNotification') {
                if (resulf == "1") {
                    playSound('../Sound/DayFlight/sNotifi');
                }
            }
            textColorChange();
        }
        function btnSearch_Onclick() {
            isSearch = true;
            if (isSearch) {
                preLoadData();
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'btnSearch_Onclick', 'btnSearch_Onclick');
            }
            isSearch = false;
        }
        function runtimeCountDown() {
            if (!isRefresh) {
                $('#lblTimeRefresh').text(''); cd = timeRefresh; $('#ddlSelectViewColum').removeAttr('disabled');
                $('#ddlSelectViewColum').prop('selectedIndex', 1); return;
            };
            $('#topBar1 span').removeClass('active');
            $('#ddlSelectViewColum').prop('selectedIndex', 0);
            $('#ddlSelectViewColum').attr('disabled', 'disabled');
            cd--;
            if (cd > 0) {
                setTimeout(runtimeCountDown, 1000);
            } else {
                runtimeRefresh(); setTimeout(runtimeCountDown, 1000); cd = timeRefresh;
            }
            $('#lblTimeRefresh').text('Time refresh: ' + cd);
        }
        function runtimeGetTopBarInfo() {
            if (loadTopbarInfo) {
                GetArgWithPostBack('RenderTopBarInfo' + phanCachArg + 'RenderTopBarInfo',
    'RenderTopBarInfo');
                setTimeout(runtimeGetTopBarInfo, timeRefresh * 1130);
            } else return;
        }
        function runtimeMessageAlert() {
            notifyMe($('#divMessegeChange').html().replace(/<br>/gi, '\n'));
            setTimeout(runtimeMessageAlert, 13200);
        }
        function runtimeSoundNotification() {
            if (isSound) {
                GetArgWithPostBack('RuntimeSoundNotification' + phanCachArg + 'RuntimeSoundNotification',
    'RuntimeSoundNotification');
                var t = setTimeout(runtimeGetTopBarInfo, timeRefresh * 1010);
            } else return;
        }
        function runtimeGetTopbar3() {
            if (loadTopbar3) {
                GetArgWithPostBack('RenderTopBarInfo' + phanCachArg + 'RenderTopBarInfo',
    'RenderTopBarInfo');
                setTimeout(runtimeGetTopBarInfo, timeRefresh * 1020);
            } else return;
        }
        function runtimeRefresh() { //auto load 20 record and default data serach
            if (isRefresh) {
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'btnSearch_Onclick', 'btnSearch_Onclick');
                //setTimeout(runtimeRefresh, timeRefresh * 1000);
            }
        }
        function runtimeAlarm() {
            if ($('#divAlarm').html() != '') {
                notifyMe_New($('#divAlarm').html().replace(/<br>/gi, '\n'));
            } var t = setTimeout(runtimeAlarm, isFirtLoadAlarm ? 5430 : 1800000);
        }
        function preLoadData() {
            preloadImg('grdSource', 'PreLoadData', '30px', '30px');
        }
        function unPreLoadData() {
            unLoadingData('PreLoadData');
        }
        function runtimeRefreshAll() { //auto load 20 record and default data serach
            preLoadData();
            GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg + 'btnSearch_Onclick',
            'btnSearch_Onclick');
            setTimeout(runtimeRefreshAll, timeRefresh * 1030);
        }
        function sortTable(iCol) {
            var table, rows, switching, i, x, y, shouldSwitch, s, indd, span;
            table = document.getElementById("grdSource");
            s = $(iCol).attr('data-sort');
            $(iCol).attr('data-sort', s == 'false' ? 'true' : 'false');
            indd = $(iCol.cellIndex)[0];
            span = s == 'true' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(iCol).append(span);
            rows = table.getElementsByTagName("TR");
            switching = true;
            while (switching) {
                switching = false;
                for (i = 2; i < (rows.length - 1) ; i++) {
                    shouldSwitch = false;
                    x = rows[i].getElementsByTagName("TD")[indd];
                    y = rows[i + 1].getElementsByTagName("TD")[indd];
                    if (s == 'true') {
                        if (x.innerHTML.toLowerCase() > y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                    else {
                        if (x.innerHTML.toLowerCase() < y.innerHTML.toLowerCase()) {
                            shouldSwitch = true;
                            break;
                        }
                    }
                }
                if (shouldSwitch) {
                    rows[i].parentNode.insertBefore(rows[i + 1], rows[i]);
                    switching = true;
                }
            }
        }
    </script>
    <script>
        $(window).scroll(function () { //detact scroll
            if ($(window).scrollTop() + $(window).height() >= $(document).height()) { //scrolled to bottom of the page
                isSroll = true;
                LoadGrdSourceScroll();
                isSroll = false;
            }
        });
        function LoadGrdSourceScroll() {
            if (isSroll) {
                GetArgWithPostBack(JSON.stringify(getValueSearchDaylyFlight()) + phanCachArg
    + 'LoadGrdSourceScroll', 'LoadGrdSourceScroll');
                preLoadData()
            }
        }
        function sortTable(f, n) {
            var rows = $('#grdSource tbody  tr').get();
            rows.sort(function (a, b) {
                var A = getVal(a);
                var B = getVal(b);
                if (A < B) {
                    return -1 * f;
                }
                if (A > B) {
                    return 1 * f;
                }
                return 0;
            });
            function getVal(elm) {
                var v = $(elm).children('td').eq(n).text().toUpperCase();
                if ($.isNumeric(v)) {
                    v = parseInt(v, 10);
                }
                return v;
            }

            $.each(rows, function (index, row) {
                $('#grdSource').children('tbody').append(row);
            });
        }
        function sortOnclick(ele) {

            var s = parseInt($(ele).attr('data-sort'));
            s *= -1;
            $(ele).attr('data-sort', s);
            var span = s == '1' ? '<p class="glyphicon glyphicon-triangle-bottom"></p>'
    : '<p class="glyphicon glyphicon-triangle-top"></p>';
            $('#grdSource TR').eq(1).find('p').remove();
            $(ele).append(span);

            var n = $(ele).prevAll().length;
            sortTable(s, n);
        }
    </script>
    <script>
        <%--var urlApi = '<%= ConfigurationManager.AppSettings["ApplicationPath.API"]%>';--%>
        function viewPopupInfoExtension(ele) {
            $('#txtStatusLetter').val('');
            var ax = $(ele);
            $('#grdSource tr').removeClass('active');
            ax.addClass('active');
            GetArgWithPostBack(ax.attr('data-id') + phanCach + ax.attr('data-timeM')
    + phanCach + ax.attr('data-CallSign') + phanCachArg + 'viewPopupInfoExtension',
    'viewPopupInfoExtension');

            /*
            get permission info
            */
            $('#lblLinkFile').html('');
            $('#txtAuthorPerm').val('');
            $('#txtOperPerm').val('');
            $('#txtFlightNbrPerm').val('');
            $('#txtPermTypePerm').val('');
            $('#txtVersionPerm').val('');
            $('#txtSeasonPerm').val('');
            $('#txtDatePerm').val('');
            $('#txtHoursPerm').val('');
            var $permNbr = $($(ele).find('td')[2]).text();
            //if ($permNbr == 'NoPerm') return;
            var kq = '';
            var url = urlApi + '/api/DayFlights/GetPermBy?ID=' + ax.attr('data-id');
            $.ajax({
                method: "GET",
                url: url,
            }).always(function (data) {
                $('#tblPermission tr').remove();
                if (data.ListValue == null) return;
                $('#txtAuthorPerm').val(data.ListValue[0]['AUTHOR_ID']);
                $('#txtOperPerm').val(data.ListValue[0]['OPER_ID']);
                $('#txtFlightNbrPerm').val(data.ListValue[0]['PERMNBR']);
                $('#txtPermTypePerm').val(data.ListValue[0]['PERMTYPE']);
                $('#txtVersionPerm').val(data.ListValue[0]['VERSION']);
                $('#txtSeasonPerm').val(data.ListValue[0]['SEASON']);
                $('#txtDatePerm').val(new Date(data.ListValue[0]['PERMDATE']).format('dd-mm-yyyy'));
                $('#txtHoursPerm').val(data.ListValue[0]['VALIDHOURS']);
                getLinkfile(urlApi + '/api/DayFlights/GetLinkFile', ax.attr('data-id'), 'SC');
                if (data.ListValue[0]['FLIGHTTYPE'] == 'NO')
                    kq += '<thead><tr><th>Call sign</th><th>Registration</th><th>From</th><th>To</th><th>Etd</th><th>Eta</th><th>Day flight</th><th>Craft</th><th>Purpose</th><th>Via</th><th>Remark</th><th>LastModify</th><th></th></tr></thead>';
                else kq += '<thead><tr><th>Call Sign</th><th>Registration</th><th>From</th><th>To</th><th>Etd</th><th>Eta</th><th>DAY</th><th>Craft</th><th>Begin date</th><th>End date</th><th>Purpose</th><th>Via</th><th>Remark</th></tr>';
                kq += '<tbody>';
                $.each(data.ListValue, function (a, b) {
                    kq += '<tr>';
                    kq += '<td>' + b['FLIGHTNBR'] + '</td>';
                    kq += '<td>' + b['REGISTRATION'] + '</td>';
                    kq += '<td>' + b['FROM_AIRP'] + '</td>';
                    kq += '<td>' + b['TO_AIRP'] + '</td>';
                    kq += '<td>' + b['ETD'] + '</td>';
                    kq += '<td>' + b['ETA'] + '</td>';
                    if (b['FLIGHT_TYPE'] = 'SC')
                        kq += '<td>' + (b['DAY1'] + b['DAY2'] + b['DAY3'] + b['DAY4'] + b['DAY5'] + b['DAY6'] + b['DAY7']).replace(/0/gi, '.') + '</td>';
                    else kq += '<td>' + b['DAYFLIGHT'] + '</td>';
                    kq += '<td>' + b['MA'] + '</td>';
                    if (b['FLIGHT_TYPE'] = 'SC') {
                        kq += '<td style=\"white-space: nowrap;\">' + (b['BEGINDATE'] == null ? '' : new Date(b['BEGINDATE']).format('dd-mm-yyyy')) + '</td>';
                        kq += '<td style=\"white-space: nowrap;\">' + (b['ENDDATE'] == null ? '' : new Date(b['ENDDATE']).format('dd-mm-yyyy')) + '</td>';
                    }
                    kq += '<td>' + b['PURPOSE_ID'] + '</td>';
                    kq += '<td style=\"word-break: break-all;\">' + b['VIA'] + '</td>';
                    kq += '<td style=\"word-break: break-all;\">' + b['REMARK'] + '</td>';
                    kq += '</tr>'
                })
                kq += '</tbody>';
                $('#tblPermission').append(kq);
            });
        }
        function getLinkfile(url, id, ty) {
            var url1 = url + '?id=' + id + '&permtype=' + ty;
            $.ajax({
                method: "GET",
                url: url1,
            }).always(function (ms) {
                if (ms.ListValue == null) return;
                $.each(ms.ListValue, function (a, b) {
                    $('#lblLinkFile').append('<a href="' + b['URLPATH'] + b['FILENAME'] + '">' + b['FILENAME'] + '</a></br>');

                })
            });

        }
        function btnUpdateStatusLetter_OnClick(id) {
            if ($('#txtStatusLetter').val() == '') return;
            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=FLIGHT_DAYFLIGHT&storeName=updateFlightBy_LetterType";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_ID: id, P_USER: '<%= _user.UserName%>', P_LETTER_TYPE: $('#ddlSelectView').val().toUpperCase(), P_REMARK: $('#txtRemark').val(), P_REGISTRATION: $('#txtRegis').val(), P_EOBT: $('#txtEobt').val(), P_ATD: $('#txtAtd2').val(), P_ATA: $('#txtAta').val() }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
            })
        }
        function LoadDataGrid_Finished() {

            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_STRING: 'TEST' }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
            })
        }
    </script>
    <script>
        function textColorChange() {
            $('#grdSource .cssTextColChange').each(function () {
                var ele = $(this);
                var span = $(ele).find('span').addClass('cssTextColChange');
            });
        }
        $('[data-ModeColor="true"]').each(function () {
            $(this).spectrum({
                color: $(this).attr('data-setcolor'),
                change: function (color) {
                    reStyleColor();
                    UpdateColorBy('<%= _user.UserID%>', color.toHexString(), $(this).attr('data-className'));
                },
                chooseText: 'Select',
                cancelText: 'Cancel',
            })
        });
        function reStyleColor() {
            $('#styCSS').html('');
            $('[data-ModeColor="true"]').each(function () {
                var cR = '.' + $(this).attr('data-className') + '{' + ($(this).attr('data-className').indexOf('cssText')
    == 0 ? 'color: ' : 'background-color: ') + $(this).spectrum('get').toHexString()
    + '!important;} ';
                $('#styCSS').append(cR);
            });
            $('[data-modefontsize="true"]').each(function () {
                var el = $(this).val() + 'px';
                $('#grdSource tbody').css('font-size', el);
            });
            textColorChange();
        }
        function UpdateColorBy(u, c, cl) {
            GetArgWithPostBack(u + phanCach + c + phanCach + cl + phanCachArg + 'UpdateColorBy',
    'UpdateColorBy');
        }
        function SetDefaultColor(cl, u) {
            GetArgWithPostBack(cl + phanCach + u + phanCachArg + 'SetDefaultColor',
    'SetDefaultColor');
            $('#txtMc' + cl).spectrum('set', $('#txtMc' + cl).attr('data-ColorDefault'));
            reStyleColor();
        }
        reStyleColor();
    </script>
    <script>
        function playSound(filename) {
            document.getElementById("sound").innerHTML = '<audio autoplay="autoplay"><source src="' + filename + '.mp3" type="audio/mpeg" /><source src="' + filename + '.ogg" type="audio/ogg" /><embed hidden="true" autostart="true" loop="false" src="' + filename
            + '.mp3" /></audio>';
        }
    </script>
    <script>
        $(document).ready(function () {
            $("#grdSource").tableHeadFixer({ "head": true, "left": 4 });
        });

        function LoadDataGrid_Finished() {

            var url = urlApi + "api/ApiExtension/ExcuteReturnInt?packageName=MAKE_FINISHED&storeName=make_finished_flights_news";
            $.ajax({
                async: false,
                method: "PUT",
                url: url,
                data: JSON.stringify({ P_STRING: 'TEST' }),
            }).always(function (data) {
                if (data.ListValue == null || data.ListValue == -1) {
                    alert('Update error!');
                } else alert('Update success!');
            })
        }
    </script>

</asp:Content>
