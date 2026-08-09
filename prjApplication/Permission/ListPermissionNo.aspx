<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ListPermissionNo.aspx.cs" Inherits="prjApplication.Permission.ListPermissionNo" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .sInput {
            width: 100%;
        }

        input, textarea {
            text-transform: uppercase;
        }

        #popupPermDetailNoHistory .history-dialog {
            width: min(1180px, calc(100% - 30px));
            max-width: 1180px;
            margin: 30px auto;
        }

        #popupPermDetailNoHistory .modal-body {
            max-height: calc(100vh - 190px);
            overflow: auto;
            padding: 12px;
        }

        #tblPermDetailNoActionHistory {
            width: 100%;
            margin-bottom: 0;
            background: #fff;
        }

        #tblPermDetailNoActionHistory thead th {
            position: sticky;
            top: 0;
            z-index: 2;
            padding: 8px 6px;
            color: #fff;
            text-align: center;
            vertical-align: middle;
            background: #337ab7;
        }

        #tblPermDetailNoActionHistory tbody td {
            padding: 7px 6px;
            vertical-align: top;
            border: 1px solid #d7e3ef;
        }

        #tblPermDetailNoActionHistory .history-center {
            text-align: center;
            white-space: nowrap;
        }

        #tblPermDetailNoActionHistory .history-date {
            min-width: 145px;
        }

        #tblPermDetailNoActionHistory .history-change-detail {
            min-width: 320px;
            line-height: 1.55;
            overflow-wrap: anywhere;
        }

        #tblPermDetailNoActionHistory .history-action-insert {
            color: #2e7d32;
            font-weight: 600;
        }

        #tblPermDetailNoActionHistory .history-action-update {
            color: #1565c0;
            font-weight: 600;
        }

        #tblPermDetailNoActionHistory .history-action-delete,
        #tblPermDetailNoActionHistory .history-error {
            color: #c62828;
            font-weight: 600;
        }

        #tblPermDetailNoActionHistory .history-empty,
        #tblPermDetailNoActionHistory .history-error,
        #tblPermDetailNoActionHistory .history-loading {
            padding: 24px;
            text-align: center;
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
            z-index: 999999;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }
    </style>
    <style>
        .cssHide {
            display: none;
        }

        .cssShow {
            display: block;
        }

        .modal {
            background-color: rgba(0, 0, 0, 0.7);
            overflow-x: hidden;
            overflow-y: auto;
            position: fixed;
            font-family: Arial, Helvetica, sans-serif;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            z-index: 99999;
            opacity: 1;
            -webkit-transition: opacity 400ms ease-in;
            -moz-transition: opacity 400ms ease-in;
            transition: opacity 400ms ease-in;
            pointer-events: visible;
        }
        /*.modal .modal-content{position:relative; pointer-events:visible;}*/
        .table td i:hover {
            cursor: pointer;
        }

        .form-group {
            margin-bottom: 7px;
        }

        .rowRed {
            color: red !important;
        }

        .modal-dialog {
            width: 80%;
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
        .mControl {
            width: 200px !important;
            height: 23px !important;
        }

        select.form-control {
            padding: 1px 2px;
            font-size: 12px;
        }

        .mLable {
            width: 120px !important;
        }

        #popupEditFlight > .modal-dialog {
            width: 75% !important;
        }

        #MultiAdd > .modal-dialog {
            width: 100%;
        }

        #MultiAdd label, input, select > option {
            font-size: 12px;
        }
    </style>

    <div>
        <%--<div style="text-align: left">
            <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="100" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
        </div>--%>
        <div>
            <asp:Button ID="btnSearch" OnClick="btnSearch_Click" CssClass="btn btn-sm btn-primary" runat="server"
                Text="Search" />
           <!-- <button id="btnOpenSearch" data-toggle="modal" data-target="#searchExtension" class="btn btn-sm btn-primary"
                type="button">
                Exten search</button>-->
            <asp:LinkButton runat="server" ID="btnAddNew" CssClass="btn btn-sm btn-primary"
                OnClick="btnAddNew_Click">
                                                    <span class="glyphicon glyphicon-plus"></span>
                                                    Create
            </asp:LinkButton>
            <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                OnClick="btnExcel_Click">
                                            <span class="glyphicon glyphicon-download-alt"></span>
                                            Export data into Excel
            </asp:LinkButton>
            <a id="btnSearchPermissionAdv" class="btn btn-sm btn-primary"
                href="<%= Page.ResolveUrl("~/Permission/SearchPermissionAdv.aspx?Menu_ID=" + Request.QueryString["Menu_ID"]) %>">
                <span class="glyphicon glyphicon-search"></span> Search Permission Adv
            </a>
        </div>

    </div>

    <table id="tblSource" class="table table-bordered">
        <thead>
            <tr>
                <th></th>
                <th>
                    <input runat="server" class="sInput" type="text" id="txtSearchPERMNBR_ID" />
                </th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchAUTHOR" />-->
                        </th>
                <th>
                    <input runat="server" class="sInput" type="text" id="txtSearchTYPE" /></th>
                <th></th>
                <th>
                    <input runat="server" class="sInput" type="text" id="txtSearchNUMBER" /></th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchVERSION" />-->
                        </th>
                <th>
                    <input runat="server" class="sInput" type="text" id="txtSearchDATE" /></th>
                <th>
                    <input runat="server" class="sInput" type="text" id="txtSearchOPER" /></th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchREFERENCE" />-->
                        </th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchVALIDHOURS" />-->
                </th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchBILLINGADDRESS" />-->
                </th>
                <th>
                    <!--<input runat="server" class="sInput" type="text" id="txtSearchPERMCONTENT" />-->
                </th>
            </tr>
            <tr>
                <th></th>
                <th>PERM</th>
                <th>AUTHOR</th>
                <th>PTYPE</th>
                <th>FTYPE</th>
                <th>NUMBER</th>
                <th>VERSION</th>
                <th>DATE</th>
                <th>OPER</th>
                <th>REFERENCE</th>
                <th>VALID</th>
                <th>ADDRESS</th>
                <th>CONTENT</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater runat="server" ID="rptSource">
                <ItemTemplate>
                    <tr >
                        <td style="width: 70px; white-space: nowrap;">
                            <div class="action-buttons" id="divattribute" runat="server">
                                <a data-toggle="tooltip" title='<%# "Edit: " + Eval("PERM_ID") %>' runat="server"
                                    visible='true'>
                                    <i class="ace-icon fa fa-pencil bigger-130" onclick="Edit('<%# Eval("PERM_ID") %>')"></i>
                                </a>
                                <a href="#" class="bigger-140 show-details-btn"
                                    data-perm-id='<%# Eval("PERM_ID") %>'
                                    data-perm-nbr='<%# System.Web.HttpUtility.HtmlAttributeEncode(Convert.ToString(Eval("PERMNBR_ID"))) %>'
                                    title="Show history" aria-label="Show history"
                                    onclick="return ShowPermDetailNoActionHistory(this);">
                                    <i class="ace-icon fa fa-angle-double-down"></i>
                                </a>
                                <asp:LinkButton runat="server" ID="lnkDelete" CssClass="ace-icon fa fa-trash-o bigger-130"
                                    Visible='<%# _Role.R_Del %>' data-id='<%# Eval("PERM_ID") %>' data-toggle="tooltip"
                                    title='<%# "Delete: " + Eval("PERMNBR_ID") %>'
                                    OnClick="lnkDelete_Click"></asp:LinkButton>
                            </div>
                        </td>
                        <td style="white-space: nowrap;"><%# Eval("PERMNBR_ID") %></td>
                        <td style="width: 90px"><%# Eval("AUTHOR_NAME") %></td>
                        <td style="width: 50px"><%# Eval("PERMTYPE") %></td>
                        <td style="width: 50px"><%# Eval("FLIGHTTYPE") %></td>
                        <td style="width: 60px" onclick="EditNo('<%# Eval("PERM_ID") %>')"><%# Eval("PERMNBR") %></td>
                        <td style="width: 45px"><%# Eval("VERSION") %></td>
                        <td style="white-space: nowrap;"><%# Eval("PERMDATE", "{0:dd/MM/yyyy}") %></td>
                        <td style="width: 40px"><%# Eval("OPER_ID") %></td>
                        <td style="width: 120px"><%# Eval("REFERENCE") %></td>
                        <td style="width: 40px"><%# Eval("VALIDHOURS") %></td>
                        <td style="width: 220px">
                            <span ondblclick="copyToClipboard('<%# Eval("BILLINGADDRESS").ToString().Replace("\n","<br>") %>')"
                                onmouseout="hideddrivetip();" onmouseover="ddrivetip('<%# Eval("BILLINGADDRESS").ToString().Replace("\n", "</br>") %>', 'yellow', 900);">
                                <%# Eval("BILLINGADDRESS").ToString().Length>40? Eval("BILLINGADDRESS").ToString().Substring(0,40)+"..."  : Eval("BILLINGADDRESS").ToString() %></span>
                        </td>
                        <td style="width: 220px"><span ondblclick="copyToClipboard('<%# Eval("PERMCONTENT").ToString().Replace("\n","<br>") %>')"
                            onmouseout="hideddrivetip();" onmouseover="ddrivetip('<%# Eval("PERMCONTENT").ToString().Replace("\n", "</br>") %>', 'yellow', 1250);">
                            <%# Eval("PERMCONTENT").ToString().Length>40? Eval("PERMCONTENT").ToString().Substring(0,40)+"..."  : Eval("PERMCONTENT").ToString() %></span>
                        </td>
                    </tr>
                    <%--<tr class="detail-row">
                        <td colspan="11" class="text-left">
                            <%# rListHistoryFlightDetails(new PermMasterNoDAL().GetHistoryById(Eval("ID").ToString()), Eval("ID").ToString()) %>
                        </td>
                    </tr> --%>
                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
    <div style="text-align: right; float:left">
        <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="100" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
    </div>

    <div id="popupPermDetailNoHistory" class="modal cssHide" role="dialog" tabindex="-1"
        aria-hidden="true" aria-labelledby="permDetailNoHistoryTitle">
        <div class="modal-dialog history-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" aria-label="Close"
                        onclick="ClosePermDetailNoActionHistory();">×</button>
                    <h4 id="permDetailNoHistoryTitle" class="blue bigger">
                        Lịch sử thay đổi chi tiết phép bay NO
                    </h4>
                </div>
                <div class="modal-body">
                    <table id="tblPermDetailNoActionHistory" class="table table-bordered table-striped">
                        <thead>
                            <tr>
                                <th>STT</th>
                                <th>Thao tác</th>
                                <th>Detail ID</th>
                                <th>Flight PK</th>
                                <th>Callsign</th>
                                <th>Người thực hiện</th>
                                <th>Thời gian</th>
                                <th>Thông tin thay đổi</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td colspan="8" class="history-loading">Đang tải lịch sử...</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary"
                        onclick="ClosePermDetailNoActionHistory();">
                        <i class="ace-icon fa fa-times"></i> Đóng
                    </button>
                </div>
            </div>
        </div>
    </div>

<%--    <div id="searchExtension" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Search extension</h4>
                </div>
                <div class="modal-body" style="text-align: left">
                    <div class="form-group">
                        <label>CallSign</label>
                        <input id="sCallSign" maxlength="8" class="wid_100px" type="text" />
                    </div>
                    <div class="form-group">
                        <label>From</label>
                        <input id="sFrom_Airp" class="wid_60px" data-autocomplete="AERO" type="text" />
                    </div>
                    <div class="form-group">
                        <label>To</label>
                        <input id="sTo_Airp" class="wid_60px" data-autocomplete="AERO" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Craft</label>
                        <input id="sCraft" class="wid_120px" data-autocomplete="CRAFT" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Via</label>
                        <input id="sVia" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Date perm</label>
                        <input data-checkdate="true" id="sDatePerm" class="wid_90px" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Perm</label>
                        <input id="sPermNbr" class="wid_150px" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Oper</label>
                        <input id="sOper" data-autocomplete="OPER" class="wid_60px" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Flight Type</label>
                        <input id="sFlightType" data-autocomplete="FLIGHTTYPE" class="wid_60px" type="text" />
                    </div>
                    <div class="form-group">
                        <label>Perm Type</label>
                        <input id="sPermType" data-autocomplete="PERMTYPE" class="wid_60px" type="text" />
                    </div>
                    <button type="button" id="btnSearchExtension" class="btn btn-sm btn-primary" onclick="btnSearchExtension_Click()">
                        Search</button>
                    <button type="button" id="btnClearSearchExten" class="btn btn-sm btn-primary" onclick="btnClearSearchExten_Click()">
                        Clear</button>
                    <table id="tblSearchExtension">
                        <thead>
                            <tr>
                                <th>Permnbr</th>
                                <th>Author</th>
                                <th>Type</th>
                                <th>Number perm</th>
                                <th>Version</th>
                                <th>Date perm</th>
                                <th>Operator</th>
                                <th>Reference</th>
                                <th>ValidHour</th>
                                <th>Seasion</th>
                                <th>Flight type</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>

    </div>--%>

    <div id="popupEditFlight" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEditFlight');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit permission</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-horizontal">
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtCALENDAR_ID">PERMNBR_ID</label>
                                <input id="txtPERMNBR_ID" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for='<%# ddlAUTHOR_ID.ClientID %>'>AUTHOR_ID</label>
                                <div class="col-xs-10 col-sm-5 col-lg-5">
                                    <asp:DropDownList ID="ddlAUTHOR_ID" runat="server" CssClass="form-control row wid_106">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="ddlPERMTYPE">PERMTYPE</label>
                                <div class="col-xs-10 col-sm-5 col-lg-5">
                                    <select id="ddlPERMTYPE" class="form-control row wid_106">
                                        <option value="LD">Landing</option>
                                        <option value="O/F">Over Flight</option>
                                    </select>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtVERSION">VERSION</label>
                                <input id="txtVERSION" maxlength="1" type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtPERMDATE">PERMDATE</label>
                                <input id="txtPERMDATE" data-date-format="dd/mm/yyyy" data-minlenght="1" data-control="update"
                                    type="text" class="col-xs-10 col-sm-5 col-lg-5 date-picker" />
                            </div>

                            <div class="form-group">
                                <label class="col-lg-4 control-label" for='<%# ddlOPER_ID.ClientID %>'>OPER_ID</label>
                                <div class="col-xs-10 col-sm-5 col-lg-5">
                                    <asp:DropDownList ID="ddlOPER_ID" runat="server" CssClass="form-control row wid_106">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtREFERENCE">REFERENCE</label>
                                <input id="txtREFERENCE" maxlength="4000" data-minlenght="1" data-control="update"
                                    type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtVALIDHOURS">VALIDHOURS</label>
                                <input id="txtVALIDHOURS" maxlength="2" data-number="true" data-control="update"
                                    data-minlenght="1" type="text"
                                    class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtPERMNBR">PERMNBR</label>
                                <input id="txtPERMNBR" maxlength="5" data-minlenght="1" data-control="update" type="text"
                                    class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtSTATUS">STATUS</label>
                                <input id="txtSTATUS" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTUSER">LASTUSER</label>
                                <input id="txtLASTUSER" readonly type="text" class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtLASTMODIFY">LASTMODIFY</label>
                                <input id="txtLASTMODIFY" type="text" readonly
                                    class="col-xs-10 col-sm-5 col-lg-5" />
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtBillingAddress">BILLING ADDRESS</label>
                                <textarea id="txtBillingAddress" rows="3"
                                    class="col-xs-10 col-sm-5 col-lg-5"></textarea>
                            </div>
                            <div class="form-group">
                                <label class="col-lg-4 control-label" for="txtPermContent">PERM CONTENT </label>
                                <textarea id="txtPermContent" rows="3"
                                    class="col-xs-10 col-sm-5 col-lg-5"></textarea>
                            </div>

                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnUpdate" type="button" onclick="btnUpdateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Update
                        </button>
                        <button id="btnCreate" type="button" style="display: none" onclick="btnCreateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
                        </button>
                        <button id="btnCancel" type="button" onclick="ClosePopup('popupEditFlight');" runat="server"
                            data-dismiss="modal" class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-times"></i>
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="MultiAdd" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="headerLabel"
        aria-hidden="true" data-backdrop="false">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        <span aria-hidden="true">&times;</span><span
                            class="sr-only">Close</span></button>
                    <h4 class="modal-title" id="headerLabel">Multi add Permission</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-inline">
                            <button id="mbtnAddNewFlightDetail" onclick="mbtnAddNewFlightDetailOnclick();"
                                type="button"
                                class="btn btn-sm btn-primary">
                                Add new</button>
                            <button id="mbtnUpdateFlightDetail" disabled="disabled" onclick="mbtnUpdateFlightDetailOnclick();"
                                type="button"
                                class="btn  btn-sm btn-primary">
                                Update</button>
                            <button id="mbtnCancelFlightDetail" disabled="disabled" onclick="mbtnCancelFlightDetailOnclick();"
                                type="button"
                                class="btn  btn-sm btn-primary">
                                Cancel</button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="heading">
                            <h4>List add new</h4>
                        </div>
                        <div class="table-responsive">
                            <table id="tblMultiAdd" class="table table-bordered">
                                <thead>
                                    <tr>
                                        <th>No</th>
                                        <th></th>
                                        <th>
                                            <input id="mPERMNBR_ID" readonly class="form-control mControl" name="mPERMNBR_ID"
                                                type="text" />
                                        </th>
                                        <th>
                                            <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mAUTHOR_ID">
                                            </asp:DropDownList>
                                        </th>
                                        <th>
                                            <select id="mPERMTYPE" class="form-control mControl">
                                                <option value="LD">Landing</option>
                                                <option value="O/F">Over flight</option>
                                            </select>
                                        </th>
                                        <th>
                                            <input id="mPERMNBR" maxlength="5" data-minlenght="1" data-control="mCheck" type="text"
                                                name="mPERMNBR" class="form-control mControl" />
                                        </th>
                                        <th>
                                            <input id="mVERSION" type="text" maxlength="1" class="form-control mControl" name="mVERSION" />
                                        </th>
                                        <th>
                                            <input id="mPERMDATE" type="text" data-minlenght="1" data-control="mCheck" data-date-format="dd/mm/yyyy"
                                                class="inputControl mControl date-picker" />
                                        </th>
                                        <th>
                                            <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mOPER_ID">
                                            </asp:DropDownList>
                                        </th>
                                        <th>
                                            <input id="mREFERENCE" data-control="mCheck" maxlength="4000" data-minlenght="1"
                                                name="mREFERENCE"
                                                type="text" class="form-control mControl" />
                                        </th>
                                        <th>
                                            <input id="mVALIDHOURS" data-control="mCheck" maxlength="2" data-minlenght="1"
                                                name="mREFERENCE" data-number="true"
                                                type="text" class="form-control mControl" />
                                        </th>

                                        <th>
                                            <textarea id="mBillingAddress" data-control="mCheck" data-minlenght="1"
                                                name="mBillingAddress"
                                                rows="3" class="form-control wid_350px"></textarea>
                                        </th>
                                        <th>
                                            <textarea id="mPermContent"
                                                name="mPermContent"
                                                rows="3" class="form-control wid_350px"></textarea>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>No</th>
                                        <th></th>
                                        <th>PERMNBR_ID
                                        </th>
                                        <th>AUTHOR_ID
                                        </th>
                                        <th>PERMTYPE
                                        </th>
                                        <th>PERMNBR
                                        </th>
                                        <th>VERSION
                                        </th>
                                        <th>PERMDATE
                                        </th>
                                        <th>OPER_ID
                                        </th>
                                        <th>REFERENCE
                                        </th>
                                        <th>VALIDHOURS
                                        </th>
                                        <th>BILLING ADDRESS
                                        </th>
                                        <th>PERM CONTENT
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>

    </div>

    <div id="dhtmltooltip"></div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/CustomPaging.js"></script>
    <%--<script src="../Scripts/CustumStaticdata.js"></script>--%>

    <script>
        function copyToClipboard(valu) {
            var $temp = $("<textarea>");
            var brRegex = /<br\s*[\/]?>/gi;
            $("body").append($temp);
            $temp.val(valu.replace(brRegex, "\r\n")).select();
            try {
                var successful = document.execCommand("copy");
                var msg = successful ? 'Copy success!' : 'Copy error!';
                alert(msg);
                $temp.remove();
            } catch (e) {
                alert('Copy error!');
            }

        }
    </script>

    <script>
        //GetArgWithPostBack
        var phanCach = '<%= _phanCach %>';
        var IdSelect = '0';
        var _objRef;
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtPERMNBR_ID = document.getElementById("txtPERMNBR_ID");
        var ddlAUTHOR_ID = document.getElementById('<%= ddlAUTHOR_ID.ClientID %>');
        var ddlPERMTYPE = document.getElementById('ddlPERMTYPE');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtVERSION = document.getElementById('txtVERSION');
        var txtPERMDATE = document.getElementById('txtPERMDATE');
        var ddlOPER_ID = document.getElementById('<%= ddlOPER_ID.ClientID %>');
        var txtREFERENCE = document.getElementById('txtREFERENCE');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtSTATUS = document.getElementById('txtSTATUS');
        var txtLASTUSER = document.getElementById('txtLASTUSER');
        var txtLASTMODIFY = document.getElementById('txtLASTMODIFY');
        var btnUpdate = document.getElementById('btnUpdate');
        var btnCreate = document.getElementById('btnCreate');
        var btnCancel = document.getElementById('btnCancel');
        var txtBillingAddress = document.getElementById('txtBillingAddress');
        var txtPermContent = document.getElementById('txtPermContent');
        var permDetailNoHistoryTrigger = null;

        function ShowPermDetailNoActionHistory(trigger) {
            var permId = trigger.getAttribute('data-perm-id');
            var permNbr = trigger.getAttribute('data-perm-nbr') || '';
            var popup = document.getElementById('popupPermDetailNoHistory');
            var tbody = document.querySelector('#tblPermDetailNoActionHistory tbody');

            permDetailNoHistoryTrigger = trigger;
            var openIcon = trigger.querySelector('i');
            if (openIcon) {
                openIcon.classList.remove('fa-angle-double-down');
                openIcon.classList.add('fa-angle-double-up');
            }
            document.getElementById('permDetailNoHistoryTitle').textContent =
                'Lịch sử thay đổi - ' + permNbr + ' (PERM_ID: ' + permId + ')';
            tbody.innerHTML =
                '<tr><td colspan="8" class="history-loading">Đang tải lịch sử...</td></tr>';
            popup.className = 'modal cssShow';
            popup.setAttribute('aria-hidden', 'false');
            popup.focus();

            GetArgWithPostBack(
                permId + '_____LoadPermDetailNoActionHistory',
                'LoadPermDetailNoActionHistory'
            );
            return false;
        }

        function ClosePermDetailNoActionHistory() {
            var popup = document.getElementById('popupPermDetailNoHistory');

            if (permDetailNoHistoryTrigger) {
                var closeIcon = permDetailNoHistoryTrigger.querySelector('i');
                if (closeIcon) {
                    closeIcon.classList.remove('fa-angle-double-up');
                    closeIcon.classList.add('fa-angle-double-down');
                }
                permDetailNoHistoryTrigger.focus();
                permDetailNoHistoryTrigger = null;
            }

            popup.className = 'modal cssHide';
            popup.setAttribute('aria-hidden', 'true');
        }

        function ClosePopup(elem) {
            IdSelect = 0;
            document.getElementById(elem).className = "modal cssHide";
            btnCreate.setAttribute('style', 'display: none');
            //btnUpdate.setAttribute('style', 'display: none');
        }
        function ShowPopup(elem) {
            document.getElementById(elem).className = "modal cssShow";
        }
        function ShowpopupEditFlight(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetOneFlight', 'GetOneFlight');
        }
        function ShowPopup(elem) {
            document.getElementById(elem).className = "modal cssShow";
        }
        function CreateFlight(elem) {
            //document.getElementById(elem).className = "modal cssShow";
            //btnCreate.removeAttribute('style');
            //btnUpdate.setAttribute('style', 'display: none');
        }

        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');

        }
        function btnUpdateOnclick() {
            if (IdSelect != '0') {
                if (checkValidCustomMinlenght('update'))
                    GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
                else return;
            }
            else {
                alert('Please select Flight');
                return;
            }
        }
        function btnCreateOnclick() {
            GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
        }
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result)
                GetArgWithPostBack(id + '_____btnDeleteOnclick', 'btnDeleteOnclick');
        }
        function ReadObj() {
            var obj = {
                ID: IdSelect,
                PERMNBR_ID: createPermNBRID(ddlAUTHOR_ID.value, ddlPERMTYPE.value, txtPERMNBR.value, txtPERMDATE.value),
                AUTHOR_ID: ddlAUTHOR_ID.value,
                PERMTYPE: ddlPERMTYPE.value,
                PERMNBR: txtPERMNBR.value,
                VERSION: txtVERSION.value,
                OPER_ID: ddlOPER_ID.value,
                REFERENCE: txtREFERENCE.value,
                VALIDHOURS: txtVALIDHOURS.value,
                STATUS: txtSTATUS.value,
                PERMDATE: txtPERMDATE.value,
                BILLINGADDRESS: txtBillingAddress.value,
                PERMCONTENT: txtPermContent.value
            };
            return obj;
        }
        function DisplayResult(resulf, context) {
            if (resulf == '') return;
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'btnCreateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'GetOneFlight') {
                ReadInfoPerm(resulf);
                checkCustomValidate();
            }
            if (context == 'LoadDataGrid') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
            }
            if (context == 'btnDeleteOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'mbtnAddNewFlightDetailOnclick') {
                if (resulf != '-1' && resulf != '-99') {
                    alert('Insert sussess!');
                    mAddrowDynamic(resulf);
                } else alert('Insert error!');
            }
            if (context == 'uGetObjectInfo') {
                if (resulf != '' && resulf != null) {
                    _objRef = JSON.parse(resulf);
                }
            }
            if (context == 'mShowDetail') {
                mReadInfoFlightDetail(resulf);
            }
            if (context == 'mDeleteRowOnclick') {
                if (resulf == 'true') {
                    alert('Delete sussess!');
                    var mtblMultiAdd = document.getElementById('tblMultiAdd');
                    document.getElementById('tr' + numberRowDelete).remove();
                    if (mtblMultiAdd.tBodies[0].rows.length == 0) {
                        mtblMultiAdd.tBodies[0].appendChild(mdefaulfRow);
                        iRowAdd = 1;
                    }
                } else { alert('Error detele'); }
            }
            if (context == 'mbtnUpdateFlightDetailOnclick') {
                if (resulf == 'true') {
                    mUpdateValueUpdate();
                    alert('Update sussess!');
                }
                else alert('Error update!');
            }
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'LoadPermDetailNoActionHistory') {
                document.querySelector('#tblPermDetailNoActionHistory tbody').innerHTML = resulf;
            }
            if (context == 'btnSearchExtension_Click') {
                if (resulf == '0') {
                    unLoadingData('preloadSearchExten');
                    $('#tblSearchExtension tbody tr').remove();
                }
                else {
                    unLoadingData('preloadSearchExten');
                    $('#tblSearchExtension tbody tr').remove();
                    $('#tblSearchExtension tbody').append(resulf);
                }
            }
        }
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }
        function ReadInfoPerm(data) {
            var obj = JSON.parse(data);
            txtLASTMODIFY.value = obj['LASTMODIFY'] == null ? null : obj['LASTMODIFY']['DateTime'];
            txtPERMDATE.value = obj['PERMDATE'] == null ? null : obj['PERMDATE']['DateTime'];
            txtLASTUSER.value = obj['LASTUSER'];
            txtPERMNBR_ID.value = obj['PERMNBR_ID'];
            txtREFERENCE.value = obj['REFERENCE'];
            txtSTATUS.value = obj['STATUS'];
            txtVALIDHOURS.value = obj['VALIDHOURS'];
            txtPERMNBR.value = obj['PERMNBR'];
            txtVERSION.value = obj['VERSION'];
            setSelectedValue(ddlAUTHOR_ID.id, obj['AUTHOR_ID']);
            setSelectedValue(ddlOPER_ID.id, obj['OPER_ID']);
            setSelectedValue(ddlPERMTYPE.id, obj['PERMTYPE']);
            //txtBeginDate.value = obj['BEGIN_DATE'] == null ? null : obj['BEGIN_DATE']['DateTime'];
            txtBillingAddress.value = obj['BILLINGADDRESS'];
            txtPermContent.value = obj['PERMCONTENT'];
        }
    </script>

    <%--Script for multi add--%>
    <script>
        var idIndetiny = 1;
        var numberRowDelete = 0;
        var iRowAdd = 1; // 0 - no add; 1 add row defaulf
        var iRowSelect = '';
        var mdefaulfRow = document.getElementById('tblMultiAdd').tBodies[0].rows[0];
        var mbtnCancelFlightDetail = document.getElementById('mbtnCancelFlightDetail');
        var mbtnUpdateFlightDetail = document.getElementById('mbtnUpdateFlightDetail');
        var mbtnAddNewFlightDetail = document.getElementById('mbtnAddNewFlightDetail');
        var mTable = document.getElementById('tblMultiAdd');

        var mPERMNBR_ID = document.getElementById("mPERMNBR_ID");
        var mPERMNBR = document.getElementById('mPERMNBR');
        var mAUTHOR_ID = document.getElementById('<%= mAUTHOR_ID.ClientID %>');
        var mPERMTYPE = document.getElementById('mPERMTYPE');
        var mVERSION = document.getElementById('mVERSION');
        var mPERMDATE = document.getElementById('mPERMDATE');
        var mOPER_ID = document.getElementById('<%= mOPER_ID.ClientID %>');
        var mREFERENCE = document.getElementById('mREFERENCE');
        var mVALIDHOURS = document.getElementById('mVALIDHOURS');
        //var mSTATUS = document.getElementById('mSTATUS');
        //var mLASTUSER = document.getElementById('mLASTUSER');
        //var mLASTMODIFY = document.getElementById('mLASTMODIFY');
        var mBillingAddress = document.getElementById('mBillingAddress');
        var mPermContent = document.getElementById('mPermContent');
        function uGetObjectInfo(id) {
            GetArgWithPostBack(id + '_____uGetObjectInfo', 'uGetObjectInfo');
        }
        function mGetObjectInfo() {
            var _obj = _objRender;
            _obj['ID'] = IdSelect;
            _obj['PERMNBR_ID'] = mPERMNBR_ID.value;
            _obj['AUTHOR_ID'] = mAUTHOR_ID.value;
            _obj['PERMTYPE'] = mPERMTYPE.value;
            _obj['PERMNBR'] = mPERMNBR.value;
            _obj['VERSION'] = mVERSION.value;
            _obj['OPER_ID'] = mOPER_ID.value;
            _obj['REFERENCE'] = mREFERENCE.value;
            _obj['VALIDHOURS'] = mVALIDHOURS.value;
            _obj['PERMDATE'] = mPERMDATE.value;
            _obj['PERMNBR_ID'] = createPermNBRID(mAUTHOR_ID.value, mPERMTYPE.value, mPERMNBR.value, mPERMDATE.value);
            _obj['BILLINGADDRESS'] = mBillingAddress.value;
            _obj['PERMCONTENT'] = mPermContent.value;
            return _obj;
        }
        function mbtnAddNewFlightDetailOnclick() {
            if (checkValidCustomMinlenght('mCheck'))
                GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnAddNewFlightDetailOnclick', 'mbtnAddNewFlightDetailOnclick');
        }
        function returnObject(sdata) {
            var obj = JSON.stringify(sdata);
            return obj;
        }
        function mEditFlightDetailByID(id) {
            iRowSelect = 'tr' + id;
            GetArgWithPostBack(id + '_____mEditFlightDetailByID', 'mEditFlightDetailByID');
        }
        function mbtnCancelFlightDetailOnclick() {
            mbtnAddNewFlightDetail.removeAttribute('disabled');
            mbtnCancelFlightDetail.setAttribute('disabled', 'disabled');
            mbtnUpdateFlightDetail.setAttribute('disabled', 'disabled');
            mClearValueControl();
            iRowSelect = '';
        }
        function mbtnUpdateFlightDetailOnclick() {
            if (checkValidCustomMinlenght('mCheck'))
                GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnUpdateFlightDetailOnclick', 'mbtnUpdateFlightDetailOnclick');
        }
        function mAddrowDynamic(id) {
            var new_row = mdefaulfRow.cloneNode(true);
            new_row.cells[0].innerHTML = idIndetiny;
            new_row.cells[1].innerHTML = '<div class="action-buttons"><a title="Edit"><i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(' + id + ');"></i></a><a title="Delete"><i class="ace-icon fa fa-trash-o bigger-130" onclick="mDeleteRowOnclick(' + id + ')"></i></a></div>';
            //new_row.cells[2].innerHTML = mCALENDAR_ID.options[mCALENDAR_ID.selectedIndex].text;
            new_row.cells[2].innerHTML = createPermNBRID(mAUTHOR_ID.value, mPERMTYPE.value, mPERMNBR.value, mPERMDATE.value);
            new_row.cells[3].innerHTML = mAUTHOR_ID.options[mAUTHOR_ID.selectedIndex].text;
            new_row.cells[4].innerHTML = mPERMTYPE.options[mPERMTYPE.selectedIndex].text;
            new_row.cells[5].innerHTML = mPERMNBR.value;
            new_row.cells[6].innerHTML = mVERSION.value;
            new_row.cells[7].innerHTML = mPERMDATE.value;
            new_row.cells[8].innerHTML = mOPER_ID.options[mOPER_ID.selectedIndex].text;
            new_row.cells[9].innerHTML = mREFERENCE.value;
            new_row.cells[10].innerHTML = mVALIDHOURS.value;
            //new_row.cells[11].innerHTML = mSTATUS.value;
            //new_row.cells[12].innerText = '<%= _user.UserName %>';
            //new_row.cells[13].innerHTML = new Date().format('dd/mm/yyyy hh:mm:ss');
            new_row.cells[11].innerHTML = mBillingAddress.value.length > 40 ? mBillingAddress.value.substring(0, 40) + '...' : mBillingAddress.value;
            new_row.cells[12].innerHTML = mPermContent.value.length > 40 ? mPermContent.value.substring(0, 40) + '...' : mPermContent.value;
            if (mBillingAddress.value.length > 40) {
                new_row.cells[11].setAttribute("onmouseover", "ddrivetip('" + mBillingAddress.value.replace(/\r?\n|\r/g, '<br>') + "', 'yellow', 500);");
                new_row.cells[11].setAttribute('onmouseout', 'hideddrivetip();');
                new_row.cells[11].setAttribute('ondblclick', 'copyToClipboard(' + mBillingAddress.value + ');');
            }
            if (mPermContent.value.length > 40) {
                new_row.cells[12].setAttribute("onmouseover", "ddrivetip('" + mPermContent.value.replace(/\r?\n|\r/g, "<br>") + "', 'yellow', 500);");
                new_row.cells[12].setAttribute('onmouseout', 'hideddrivetip();');
                new_row.cells[12].setAttribute('ondblclick', 'copyToClipboard(' + mPermContent.value + ');');
            }
            new_row.id = 'tr' + id;
            tblMultiAdd.tBodies[0].appendChild(new_row);
            if (iRowAdd == 1)
                tblMultiAdd.tBodies[0].rows[0].remove();
            idIndetiny++;
            iRowAdd = 0;
        }
        function mClearValueControl() {
            m.mPERMNBR_ID = ''; mVERSION.value = ''; mPERMDATE.value = ''; mREFERENCE.value = '';
            mVALIDHOURS.value = '';
            //mSTATUS.value = '';
            mLASTUSER.value = '';
            iRowSelect = '';
        }
        function mDeleteRowOnclick(id) {
            var rs = confirm('Do you want delete row?');
            if (rs) {
                numberRowDelete = id;
                GetArgWithPostBack(id + '_____mDeleteRowOnclick', 'mDeleteRowOnclick');
            }
        }
        function mReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);
            //mLASTMODIFY.value = obj['LASTMODIFY'] == null ? null : obj['LASTMODIFY']['DateTime'];
            mPERMDATE.value = obj['PERMDATE'] == null ? null : obj['PERMDATE']['DateTime'];
            //mLASTUSER.value = obj['LASTUSER'];
            mPERMNBR_ID.value = obj['PERMNBR_ID'];
            mREFERENCE.value = obj['REFERENCE'];
            //mSTATUS.value = obj['STATUS'];
            mPERMNBR.value = obj['PERMNBR'];
            mVALIDHOURS.value = obj['VALIDHOURS'];
            mVERSION.value = obj['VERSION'];
            setSelectedValue(mAUTHOR_ID.id, obj['AUTHOR_ID']);
            setSelectedValue(mOPER_ID.id, obj['OPER_ID']);
            setSelectedValue(mPERMTYPE.id, obj['PERMTYPE']);
            mBillingAddress.value = obj['BILLINGADDRESS'];
            mPermContent.value = obj['PERMCONTENT'];
        }
        function mShowDetail(id) {
            IdSelect = id;
            iRowSelect = 'tr' + id;
            mbtnAddNewFlightDetail.setAttribute('disabled', 'disabled');
            mbtnUpdateFlightDetail.removeAttribute('disabled');
            mbtnCancelFlightDetail.removeAttribute('disabled');
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        function mUpdateValueUpdate() {
            var uRow = document.getElementById(iRowSelect);
            uRow.cells[2].innerHTML = createPermNBRID(mAUTHOR_ID.value, mPERMTYPE.value, mPERMNBR.value, mPERMDATE.value);
            uRow.cells[3].innerHTML = mAUTHOR_ID.options[mAUTHOR_ID.selectedIndex].text;
            uRow.cells[4].innerHTML = mPERMTYPE.options[mPERMTYPE.selectedIndex].text;
            uRow.cells[5].innerHTML = mPERMNBR.value;
            uRow.cells[6].innerHTML = mVERSION.value;
            uRow.cells[7].innerHTML = mPERMDATE.value;
            uRow.cells[8].innerHTML = mOPER_ID.options[mOPER_ID.selectedIndex].text;
            uRow.cells[9].innerHTML = mREFERENCE.value;
            uRow.cells[10].innerHTML = mVALIDHOURS.value;
            //uRow.cells[11].innerHTML = mSTATUS.value;
            //uRow.cells[12].innerText = '<%= _user.UserName %>';
            //uRow.cells[13].innerHTML = new Date().format('dd/mm/yyyy hh:mm:ss');
            uRow.cells[11].innerHTML = mBillingAddress.value.length > 40 ? mBillingAddress.value.substring(0, 40) + '...' : mBillingAddress.value;
            uRow.cells[12].innerHTML = mPermContent.value.length > 40 ? mPermContent.value.substring(0, 40) + '...' : mPermContent.value;
            if (mBillingAddress.value.length > 40) {
                uRow.cells[11].setAttribute("onmouseover", "ddrivetip('" + mBillingAddress.value.replace(/\r?\n|\r/g, '</br>') + "', 'yellow', 500);");
                uRow.cells[11].setAttribute('onmouseout', 'hideddrivetip();');
                uRow.cells[11].setAttribute('ondblclick', 'copyToClipboard(' + mBillingAddress.value + ');');
            }
            if (mPermContent.value.length > 40) {
                uRow.cells[12].setAttribute("onmouseover", "ddrivetip('" + mPermContent.value.replace(/\r?\n|\r/g, '</br>') + "', 'yellow', 500);");
                uRow.cells[12].setAttribute('onmouseout', 'hideddrivetip();');
                uRow.cells[12].setAttribute('ondblclick', 'copyToClipboard(' + mPermContent.value + ');');
            }
        }
        function createPermNBRID(au, typ, nbr, ye) {
            var y = '';
            if (ye == null || ye == '')
                y = new Date().format('dd/mm/yyyy');
            return typ + ' ' + LPAD(nbr, 5, '0') + '/' + au + '/' + ye.split('/')[2];
        }
        function LPAD(nbr, iStart, sAlias) {
            var ax = nbr;
            if (nbr.length < iStart) {
                ax = LPAD(sAlias + nbr, iStart, sAlias);
            } else {
                return ax.substring(0, iStart);
            }
            return ax;
        }
        function Edit(id) {
            window.open('Edit_PermNo.aspx?Menu_ID=<%=(Request["Menu_ID"]!=null)?Request["Menu_ID"].ToString():""%>&ID=' + id, '_parent');
        }
        function EditNo(id) {
            window.open('EditPermNo.aspx?Menu_ID=<%=(Request["Menu_ID"]!=null)?Request["Menu_ID"].ToString():""%>&ID=' + id, '_parent');
        }
    </script>

    <%--/-Script for multi add--%>

    <script>
        $('[data-checkValid]').each(function () {
            var $ele = $(this);
            var d = new Date();
            try {
                var cd = new Date($ele.attr('data-checkValid').replace(/^(\d{2})\/(\d{2})\/(\d{4})$/, '$3/$2/$1'));
                if (cd < d) {
                    $($ele).closest('tr').addClass('rowRed');
                }
            } catch (e) {
            }

        });
    </script>

   <%-- <script>
        var listOper = '<%= _ListOper%>'.split(',');
        var listCraft = JSON.parse('<%= _ListCraft%>');
        var listPurpose = '<%= _ListPurpose%>'.split(',');
        var listAero = '<%= _ListAero%>'.split(',');
        var listPermType = ['LD', 'O/F'];
        var listFlightType = ['SC', 'NO'];
        function CheckListSanBay(ele) {
            if (listAero.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckListOper(ele) {
            if (listOper.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckListAero(ele) {
            if (listAero.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckPermType(ele) {
            if (listPermType.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckFlightType(ele) {
            if (listFlightType.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }
        function CheckCraft(ele) {
            if (listCraft.findIndex(item=>item.name == $(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
                $(ele).attr('data-craftid', listCraft[listCraft.findIndex(item=>axx = (item.name == $(ele).val().toUpperCase()))]['craftid']);
            }
        }
        function CheckPurpose(ele) {
            if (listPurpose.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }

        $('[data-AutoComplete="OPER"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listOper],
            }).on('blur', function (e, datum) {
                CheckListOper($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="PERMTYPE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listPermType],
            }).on('blur', function (e, datum) {
                CheckPermType($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="FLIGHTTYPE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listFlightType],
            }).on('blur', function (e, datum) {
                CheckFlightType($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="PURPOSE"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listPurpose],
            }).on('blur', function (e, datum) {
                CheckPurpose($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="AERO"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                source: [listAero],
            }).on('blur', function (e, datum) {
                CheckListAero($('#' + $ele.prop('id')));
            });
        });
        $('[data-AutoComplete="CRAFT"]').each(function () {
            var $ele = $(this);
            $('#' + $ele.prop('id')).autocomplete({
                titleKey: 'name',
                valueKey: 'name',
                source: [{
                    data: listCraft,
                }],
            }).on('selected.xdsoft', function (e, data) {
                $('#' + $ele.prop('id')).val(data.name);
            }).on('blur', function (e, datum) {
                CheckCraft($('#' + $ele.prop('id')));
            });
        });
    </script>--%>

    <script>
        function getObjectSearchExten() {
            var obj = {
                P_FLIGHTNBR: $('#sCallSign').val(),
                P_FROM_AIRP: $('#sFrom_Airp').val(),
                P_TO_AIRP: $('#sTo_Airp').val(),
                P_CRAFT: $('#sCraft').val(),
                P_VIA: $('#sVia').val(),
                P_PERMNBR: $('#sPermNbr').val(),
                P_PERMDATE: '',
                P_OPER: $('#sOper').val(),
                P_SEASION: '',
                P_FLIGHT_TYPE: $('#sFlightType').val(),
                P_PERMTYPE: $('#sPermType').val()
            }
            return obj;
        }
        function btnClearSearchExten_Click() {
            $('#sCallSign').val('');
            $('#sFrom_Airp').val('');
            $('#sTo_Airp').val('');
            $('#sCraft').val('');
            $('#sVia').val('');
            $('#sPermNbr').val('');
            $('#sOper').val('');
            $('#sFlightType').val('');
            $('#sPermType').val('');
            $('#tblSearchExtension tbody tr').remove();
        }
        function btnSearchExtension_Click() {
            preloadImg('tblSearchExtension', 'preloadSearchExten', '30px', '30px');
            GetArgWithPostBack(JSON.stringify(getObjectSearchExten()) + '_____btnSearchExtension_Click', 'btnSearchExtension_Click');

        }

    </script>
</asp:Content>

