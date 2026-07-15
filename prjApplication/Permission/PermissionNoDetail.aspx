<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="PermissionNoDetail.aspx.cs" Inherits="prjApplication.Permission.PermissionNoDetail" %>

<%@ Import Namespace="prjInfo" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
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

        .cssVisble {
            visibility: visible;
            display: block;
        }

        .cssVisbleHide {
            visibility: hidden;
            display: none;
        }

        .modal {
            background-color: rgba(0, 0, 0, 0.7);
        }

        .table td i:hover {
            cursor: pointer;
        }

        .inputControl {
            width: 300px !important;
        }

        .pagination {
            padding-right: 5px;
            margin-top: -5px;
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

        .modal-dialog {
            width: 100% !important;
        }

        #MultiAdd.table label, input, select > option {
            font-size: 12px;
        }
    </style>
    <span class="TitlePanel">+ Flight LIST</span>
    <div class="well well-sm">
        <asp:TextBox ID="txtSearch_UserName" Width="35%" CssClass="inputtext" runat="server"
            placeholder="Search ..."
            onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
        <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
            Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px">
        </asp:Button>
        <button type="button" data-toggle="modal" data-target="#MultiAdd" id="btnCreateNew"
            class="btn btn-sm btn-primary">
            <i class="glyphicon glyphicon-plus"></i>
            Create
        </button>
        <asp:LinkButton runat="server" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold"
            OnClick="btnPDF_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
        </asp:LinkButton>
        <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
            OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
        </asp:LinkButton>
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div class="table-responsive">
        <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyNames="ID"
            Width="100%" OnRowDataBound="grdSource_RowDataBound"
            CssClass="table table-striped table-bordered table-hover">
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="4%"></ItemStyle>
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div class="action-buttons" style="width: 65px">
                            <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Edit %>' title="Edit">
                                <i class="ace-icon fa fa-pencil bigger-130" data-toggle="modal" data-target="#popupEditFlightDetail"
                                    onclick="ShowpopupEditFlightDetail(<%# Eval("ID") %>);"></i>
                            </a>
                            <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Del %>' title="Delete">
                                <i class="ace-icon fa fa-trash-o bigger-130" onclick="btnDeleteOnclick(<%# Eval("ID") %>);">
                                </i>
                            </a>
                            <a href="#" class="bigger-140 show-details-btn" title="Show history">
                                <i class="ace-icon fa fa-angle-double-down"></i>
                            </a>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                    <HeaderTemplate>
                        PERMNBR_ID
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("PERMNBR_ID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        CRAFT
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("CRAFT_NAME") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        MTOW
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("MTOW") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        DAYSFLIGHT
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("DAYSFLIGHT") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        MAX_DATE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("MAX_DATE", "{0:dd/MM/yyyy}") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        REMARK
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("REMARK") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        REGISTRATION
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("REGISTRATION") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        FLIGHTNBR
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("FLIGHTNBR") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        TO_AIRP
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("TO_AIRP") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        FROM_AIRP
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("FROM_AIRP") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        ETD
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("ETD") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        PURPOSE_ID
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("PURPOSE_ID") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        ETA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("ETA") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                    <HeaderTemplate>
                        VIA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("VIA") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
    </div>
    <div id="popupEditFlightDetail" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEditFlightDetddlCRAFT_IDail');" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit Permisson No detail</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtPERM_ID">PERM_ID</label>
                                    <input id="txtPERM_ID" type="text" readonly class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlCRAFT_ID">CRAFT</label>
                                    <div class="col-xs-6">
                                        <select id="ddlCRAFT_ID" class="form-control inputControl row">
                                            <% foreach (var item in new CraftTypeDAL().GetAllCraftType())
                                                { %>
                                            <option data-taitrong="<%= item.TAITRONG.ToString() %>" value="<%= item.CRAFT_ID.ToString() %>">
                                                <%= item.MA %></option>
                                            <% } %>
                                        </select>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <%--<label class="col-lg-4 control-label" for="txtMTOW">MTOW</label>
                                    <input id="txtMTOW" type="text" readonly class="inputControl" />--%>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtREGISTRATION">REGISTRATION</label>
                                    <input id="txtREGISTRATION" maxlength="20" data-control="update" data-minlenght="1"
                                        type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtDAYSFLIGHT">DAYSFLIGHT</label>
                                    <input id="txtDAYSFLIGHT" type="text" maxlength="200" class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtFLIGHTNBR">FLIGHTNBR</label>
                                    <input id="txtFLIGHTNBR" maxlength="20" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlTO_AIRP">TO_AIRP</label>
                                    <div class="col-xs-10 col-sm-6 col-lg-6">
                                        <asp:DropDownList ID="ddlTO_AIRP" runat="server" CssClass="form-control row inputControl">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlFROM_AIRP">FROM_AIRP</label>
                                    <div class="col-xs-10 col-sm-6 col-lg-6">
                                        <asp:DropDownList ID="ddlFROM_AIRP" runat="server" CssClass="form-control row inputControl">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtETD">ETD</label>
                                    <input id="txtETD" type="text" data-control="update" data-minlenght="1" maxlength="5"
                                        class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlPURPOSE_ID">PURPOSE_NAME</label>
                                    <div class="col-xs-10 col-sm-6 col-lg-6">
                                        <asp:DropDownList ID="ddlPURPOSE_ID" runat="server" CssClass="form-control row inputControl">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtETA">ETA</label>
                                    <input id="txtETA" type="text" data-control="update" data-minlenght="1" maxlength="5"
                                        class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtMAX_DATE">MAX_DATE</label>
                                    <input id="txtMAX_DATE" data-minlenght="1" data-date-format="dd/mm/yyyy" data-control="update"
                                        type="text"
                                        class="inputControl date-picker" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtREMARK">REMARK</label>
                                    <textarea id="txtREMARK" rows="2"
                                        class="inputControl"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtREMARK_SEND">REMARK SEND</label>
                                    <textarea id="txtREMARK_SEND" rows="2"
                                        class="inputControl"></textarea>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtVIA">VIA</label>
                                    <input id="txtVIA" type="text" data-minlenght="1" maxlength="500" data-control="update"
                                        class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnUpdate" type="button" onclick="btnUpdateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Update
                        </button>
                        <button id="btnCancel" type="button" onclick="ClosePopup('popupEditFlightDetail');"
                            runat="server"
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
                    <h4 class="modal-title" id="headerLabel">Multi add detail flight</h4>
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
                                        <th>
                                        </th>
                                        <th></th>
                                        <th>PERM
                                        </th>
                                        <th><select id="mCRAFT_ID" onchange="mCRAFT_ID_Onchange()" class="form-control mControl">
                                            <% foreach (var item in new CraftTypeDAL().GetAllCraftType())
                                                { %>
                                            <option data-taitrong="<%= item.TAITRONG.ToString() %>" value="<%= item.CRAFT_ID.ToString() %>"><%= item.MA %>
                                            </option>
                                            <%} %>
                                        </select>
                                        </th>
                                        <th><label id="lblMTOW"></label>
                                        </th>
                                        <th><input id="mDAYSFLIGHT" maxlength="200" name="DAYSFLIGHT"
                                            class="form-control mControl"
                                            type="text" />
                                        </th>
                                        <th><input id="mFLIGHTNBR" type="text" maxlength="20"
                                            name="mFLIGHTNBR"
                                            class="form-control mControl" />
                                        </th>
                                        <th><input id="mREGISTRATION" maxlength="20" data-control="mUpdate" data-minlenght="1"
                                            name="mREGISTRATION"
                                            class="form-control mControl" type="text" />
                                        </th>
                                        <th><asp:DropDownList runat="server" CssClass="form-control mControl" ID="mFROM_AIRP">
                                        </asp:DropDownList>
                                        </th>
                                        <th><asp:DropDownList runat="server" CssClass="form-control mControl" ID="mTO_AIRP">
                                        </asp:DropDownList>
                                        </th>
                                        <th><input id="mETD" maxlength="5" data-control="mUpdate" data-minlenght="1" class="form-control mControl"
                                            name="mETD"
                                            type="text" />
                                        </th>
                                        <th><input id="mETA" data-control="mUpdate" maxlength="5" data-minlenght="1" class="form-control mControl"
                                            name="mETA"
                                            type="text" />
                                        </th>
                                        <th><input id="mVIA" name="mVIA" maxlength="500" data-control="mUpdate" data-minlenght="1"
                                            class="form-control mControl"
                                            type="text" />
                                        </th>                                        
                                        <th><asp:DropDownList runat="server" CssClass="form-control mControl" ID="mPURPOSE_ID">
                                        </asp:DropDownList>
                                        </th>
                                        <th><input id="mMAX_DATE" data-minlenght="1" data-control="mUpdate" name="mMAX_DATE"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                        </th>
                                        <th><textarea id="mREMARK"  name="mREMARK"
                                            class="form-control wid_200px"
                                            rows="3"></textarea>
                                        </th>
                                        <th><textarea id="mREMARK_SEND" name="mREMARK_SEND"
                                            class="form-control wid_200px"
                                            rows="3"></textarea>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th>Number
                                        </th>
                                        <th></th>
                                        <th>PERM
                                        </th>
                                        <th>CRAFT
                                        </th>
                                        <th>MTOW
                                        </th>
                                        <th>DAYSFLIGHT
                                        </th>
                                        <th>FLIGHTNBR
                                        </th>
                                        <th>REGISTRATION
                                        </th>
                                        <th>FROM_AIRP
                                        </th>
                                        <th>TO_AIRP
                                        </th>
                                        <th>ETD
                                        </th>
                                        <th>ETA
                                        </th>
                                        <th>VIA
                                        </th>
                                        <%--<th>STATUS
                                        </th>
                                        <th>LASTMODIFY
                                        </th>
                                        <th>LASTUSER
                                        </th>--%>
                                        <th>PURPOSE_ID
                                        </th>
                                        <th>MAX_DATE
                                        </th>
                                        <th>REMARK
                                        </th>
                                        <th>REMARK SEND
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
                                        <td></td>
                                        <%--<td></td>
                                        <td></td>
                                        <td></td>--%>
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
                    <button type="button" class="btn btn-primary">Save changes</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <div id="dhtmltooltip"></div>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>

<script>
    
</script>

    <script>
        var IdSelect = '0';
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtPERM_ID = document.getElementById('txtPERM_ID');
        //var txtMTOW = document.getElementById('txtMTOW');
        var ddlCRAFT_ID = document.getElementById('ddlCRAFT_ID');
        var txtDAYSFLIGHT = document.getElementById('txtDAYSFLIGHT');
        var txtREMARK = document.getElementById('txtREMARK');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var ddlTO_AIRP = document.getElementById('<%= ddlTO_AIRP.ClientID%>');
        var ddlFROM_AIRP = document.getElementById('<%=ddlFROM_AIRP.ClientID%>');
        var txtETD = document.getElementById('txtETD');
        var ddlPURPOSE_ID = document.getElementById('<%=ddlPURPOSE_ID.ClientID%>');
        var txtETA = document.getElementById('txtETA');
        var txtVIA = document.getElementById('txtVIA');
        //var txtLASTUSER = document.getElementById('txtLASTUSER');
        //var txtLASTMODIFY = document.getElementById('txtLASTMODIFY');
        var txtREMARK = document.getElementById('txtREMARK');
        var txtREMARK_SEND = document.getElementById('txtREMARK_SEND');
        var txtMAX_DATE = document.getElementById('txtMAX_DATE');
        //var txtSTATUS = document.getElementById('txtSTATUS');
        var btnUpdate = document.getElementById('btnUpdate');
        txtPERM_ID.value = '<%= IDCHA %>';
        function ReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);
            //txtMTOW.value = obj['MTOW'];
            setSelectedValue(ddlCRAFT_ID.id, obj['CRAFT_ID']);
            txtDAYSFLIGHT.value = obj['DAYSFLIGHT'];
            txtETA.value = obj['ETA'];
            txtREGISTRATION.value = obj['REGISTRATION'];
            txtFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(ddlTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(ddlFROM_AIRP.id, obj['FROM_AIRP']);
            txtETD.value = obj['ETD'];
            txtVIA.value = obj['VIA'];
            //txtLASTUSER.value = obj['LASTUSER'];
            //txtLASTMODIFY.value = obj['LASTMODIFY'] == null ? null : obj['LASTMODIFY']['DateTime'];
            txtREMARK.value = obj['REMARK'];
            txtREMARK_SEND.value = obj['REMARK_SEND'];
            txtMAX_DATE.value = obj['MAX_DATE'] == null ? null : obj['MAX_DATE']['DateTime'];
            //txtSTATUS.value = obj['STATUS'];
        }
        function GetObjectInfo() {
            var _obj = _objRender;
            _obj['PERM_ID'] = '<%= IDCHA %>';
            _obj['CRAFT_ID'] = ddlCRAFT_ID.value;
            //_obj['MTOW'] = txtMTOW.value;
            _obj['DAYSFLIGHT'] = txtDAYSFLIGHT.value;
            _obj['FLIGHTNBR'] = txtFLIGHTNBR.value;
            _obj['REGISTRATION'] = txtREGISTRATION.value;
            _obj['FROM_AIRP'] = ddlFROM_AIRP.value;
            _obj['TO_AIRP'] = ddlTO_AIRP.value;
            _obj['ETD'] = txtETD.value;
            _obj['ETA'] = txtETA.value;
            _obj['VIA'] = txtVIA.value;
            //_obj['STATUS'] = txtSTATUS.value;
            _obj['LASTUSER'] = '<%= _user.UserID.ToString() %>'
            _obj['PURPOSE_ID'] = ddlPURPOSE_ID.value;
            _obj['MAX_DATE'] = txtMAX_DATE.value;
            _obj['REMARK'] = txtREMARK.value;
            _obj['REMARK_SEND'] = txtREMARK_SEND.value;
            _obj['ID'] = IdSelect;
            return _obj;
        }
        function ShowpopupEditFlightDetail(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetOneObject', 'GetOneObject');
        }
        function ShowDetailNoEdit(id) {
            GetArgWithPostBack(id + '_____GetOneObjectNoUpdate', 'GetOneObjectNoUpdate');
        }
        function ClearValue() {
            //txtMTOW.value = '';
            txtDAYSFLIGHT.value = '';
            txtREMARK.value = '';
            txtREGISTRATION.value = '';
            txtFLIGHTNBR.value = '';
            txtETD.value = '';
            txtVIA.value = '';
            //txtLASTUSER.value = '';
            //txtLASTMODIFY.value = '';
            txtREMARK_SEND.value = '';
            txtREMARK.value = '';
            txtMAX_DATE.value = '';
            //txtSTATUS.value = '';
        }
        function ClosePopup(elem) {
            ClearValue();
        }
        function ShowPopup(elem) {
            //document.getElementById(elem).className = "modal cssShow";
        }

        //    function btnCreateOnclick() {
        //        GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnCreateOnclick',
        //'btnCreateOnclick');
        //    }
        function btnDeleteOnclick(id) {
            var result = confirm("Do you want delete?");
            if (result)
                GetArgWithPostBack(id + '_____btnDeleteOnclick', 'btnDeleteOnclick');
        }
        function btnUpdateOnclick() {
            if (IdSelect != '')
                GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnUpdateOnclick',
    'btnUpdateOnclick');
            else {
                alert('Please select Flight');
                return;
            }
        }
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'GetOneObject') {
                if (resulf != '') {
                    ReadInfoFlightDetail(resulf);
                    checkCustomValidate();
                }
            }
            if (context == 'GetOneObjectNoUpdate') {
                if (resulf != '') {
                    ReadInfoFlightDetail(resulf);
                    //document.getElementById('popupEditFlightDetail').className = "modal cssShow";
                    btnUpdate.setAttribute('style', 'display: none');
                }
            }
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'btnDeleteOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            //if (context == 'btnCreateOnclick') {
            //    alert(resulf);
            //    LoadDataGrid();
            //}
            if (context == 'mbtnAddNewFlightDetailOnclick') {
                if (resulf != '-1' && resulf != '-99') {
                    alert('Insert sussess!');
                    mAddrowDynamic(resulf);
                } else alert('Insert error!');
            }
            if (context == 'mEditFlightDetailByID') {
                if (resulf != '') {
                    mReadInfoFlightDetail(resulf);
                    mbtnAddNewFlightDetail.setAttribute('disabled', 'disabled');
                    mbtnCancelFlightDetail.removeAttribute('disabled');
                    mbtnUpdateFlightDetail.removeAttribute('disabled');
                }
            }
            if (context == 'mDeleteRowOnclick') {
                if (resulf != 'OK') {
                    alert("Delete error!");
                }
                else {
                    var mtblMultiAdd = document.getElementById('tblMultiAdd');
                    document.getElementById('tr' + numberRowDelete).remove();
                    if (mtblMultiAdd.tBodies[0].rows.length == 0) {
                        mtblMultiAdd.tBodies[0].appendChild(mdefaulfRow);
                        iRowAdd = 1;
                    }
                }
            }
            if (context == 'mShowDetail') {
                mReadInfoFlightDetail(resulf);
                checkCustomValidate();
                mbtnAddNewFlightDetail.setAttribute('disabled', 'disabled');
                mbtnUpdateFlightDetail.removeAttribute('disabled');
                mbtnCancelFlightDetail.removeAttribute('disabled');
            }
            if (context == 'mbtnUpdateFlightDetailOnclick') {
                if (resulf != "OK") {
                    alert("Update error!");
                }
                else {
                    alert("Update sussess!");
                    mUpdateValueUpdate();
                }
            }
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
            }
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
        }
    }
    $('.show-details-btn').on('click', function (e) {
        e.preventDefault();
        $(this).closest('tr').next().toggleClass('open');
        $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
    });
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
        
        var mCRAFT_ID = document.getElementById('mCRAFT_ID');
        var mDAYSFLIGHT = document.getElementById('mDAYSFLIGHT');
        var mREMARK = document.getElementById('mREMARK');
        var mREGISTRATION = document.getElementById('mREGISTRATION');
        var mFLIGHTNBR = document.getElementById('mFLIGHTNBR');
        var mTO_AIRP = document.getElementById('<%= mTO_AIRP.ClientID%>');
        var mFROM_AIRP = document.getElementById('<%=mFROM_AIRP.ClientID%>');
        var mETD = document.getElementById('mETD');
        var mPURPOSE_ID = document.getElementById('<%=mPURPOSE_ID.ClientID%>');
        var mETA = document.getElementById('mETA');
        var mVIA = document.getElementById('mVIA');
        var mREMARK = document.getElementById('mREMARK');
        var mREMARK_SEND = document.getElementById('mREMARK_SEND');
        var mMAX_DATE = document.getElementById('mMAX_DATE');
        function mGetObjectInfo() {
            var _obj = _objRender;
            _obj['ID'] = IdSelect;
            _obj['PERM_ID'] = '<%= IDCHA %>';
            _obj['CRAFT_ID'] = mCRAFT_ID.value;
            //_obj['MTOW'] = mMTOW.value;
            _obj['DAYSFLIGHT'] = mDAYSFLIGHT.value;
            _obj['FLIGHTNBR'] = mFLIGHTNBR.value;
            _obj['REGISTRATION'] = mREGISTRATION.value;
            _obj['FROM_AIRP'] = mFROM_AIRP.value;
            _obj['TO_AIRP'] = mTO_AIRP.value;
            _obj['ETD'] = mETD.value;
            _obj['ETA'] = mETA.value;
            _obj['VIA'] = mVIA.value;
            _obj['LASTUSER'] = '<%= _user.UserID.ToString() %>'
            _obj['PURPOSE_ID'] = mPURPOSE_ID.value;
            _obj['MAX_DATE'] = mMAX_DATE.value;
            _obj['REMARK'] = mREMARK.value;
            _obj['REMARK_SEND'] = mREMARK_SEND.value;
            return _obj;
        }

        function mbtnAddNewFlightDetailOnclick() {
            if (checkValidCustomMinlenght('mUpdate'))
                GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnAddNewFlightDetailOnclick', 'mbtnAddNewFlightDetailOnclick');
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
            if (checkValidCustomMinlenght('mUpdate'))
                GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnUpdateFlightDetailOnclick', 'mbtnUpdateFlightDetailOnclick');
        }
        function mCRAFT_ID_Onchange() {
            document.getElementById('lblMTOW').innerText = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].getAttribute('data-taitrong');
        } mCRAFT_ID_Onchange();
        function mAddrowDynamic(id) {
            var new_row = mdefaulfRow.cloneNode(true);
            new_row.cells[0].innerHTML = idIndetiny;
            new_row.cells[1].innerHTML = '<div class="action-buttons"><a title="Edit"><i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(' + id + ');"></i></a><a title="Delete"><i class="ace-icon fa fa-trash-o bigger-130" onclick="mDeleteRowOnclick(' + id + ')"></i></a></div>';
            new_row.cells[2].innerHTML = '<%= IDCHA %>';
            new_row.cells[3].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].text;
            new_row.cells[4].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].getAttribute('data-TaiTrong');
            new_row.cells[5].innerHTML = mDAYSFLIGHT.value;
            new_row.cells[6].innerHTML = mFLIGHTNBR.value;
            new_row.cells[7].innerHTML = mREGISTRATION.value;
            new_row.cells[8].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            new_row.cells[9].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            new_row.cells[10].innerHTML = mETD.value;
            new_row.cells[11].innerHTML = mETA.value;
            new_row.cells[12].innerHTML = mVIA.value;
<%--            new_row.cells[13].innerHTML = mSTATUS.value;
            new_row.cells[13].innerHTML = mLASTMODIFY.value;
            new_row.cells[14].innerHTML = '<%= _user.UserName %>';--%>
            new_row.cells[13].innerHTML = mPURPOSE_ID.options[mPURPOSE_ID.selectedIndex].text;
            new_row.cells[14].innerHTML = mMAX_DATE.value;
            new_row.cells[15].innerHTML = mREMARK.value.length > 40 ? mREMARK.value.substring(0, 40) + '...' : mREMARK.value;
            if (mREMARK.value.length > 40) {
                new_row.cells[15].setAttribute("onmouseover", "ddrivetip('" + mREMARK.value.replace(/\r?\n|\r/g, '<br>') + "', 'yellow', 500);");
                new_row.cells[15].setAttribute('onmouseout', 'hideddrivetip();');
                new_row.cells[15].setAttribute("ondblclick", "copyToClipboard('" + mREMARK.value.replace(/\r?\n|\r/g, '<br>') + "');");
            }
            new_row.cells[16].innerHTML = mREMARK_SEND.value.length > 40 ? mREMARK_SEND.value.substring(0, 40) + '...' : mREMARK_SEND.value;
            if (mREMARK_SEND.value.length > 40) {
                new_row.cells[16].setAttribute("onmouseover", "ddrivetip('" + mREMARK_SEND.value.replace(/\r?\n|\r/g, '<br>') + "', 'yellow', 500);");
                new_row.cells[16].setAttribute('onmouseout', 'hideddrivetip();');
                new_row.cells[16].setAttribute("ondblclick", "copyToClipboard('" + mREMARK_SEND.value.replace(/\r?\n|\r/g, '<br>') + "');");
            }
            new_row.id = 'tr' + id;
            tblMultiAdd.tBodies[0].appendChild(new_row);
            if (iRowAdd == 1)
                tblMultiAdd.tBodies[0].rows[0].remove();
            idIndetiny++;
            iRowAdd = 0;
        }
        function mClearValueControl() {
            mETA.value = ''; mETD.value = ''; mFLIGHT_PK.value = ''; mFLIGHTNBR.value = ''; mDAY1.selected = false; mDAY2.selected = false; mDAY3.selected = false; mDAY4.selected = false; mDAY5.selected = false; mDAY6.selected = false; mDAY7.selected = false;
            //mMTOW.value = '';
            mREGISTRATION.value = '';
            mVIA.value = ''; mREMARK.value = ''; mBEGINDATE = ''; mENDDATE.value = '';
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
            //mMTOW.value = obj['MTOW'];
            setSelectedValue(mCRAFT_ID.id, obj['CRAFT_ID']);
            mDAYSFLIGHT.value = obj['DAYSFLIGHT'];
            mREGISTRATION.value = obj['REGISTRATION'];
            mFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(mTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(mFROM_AIRP.id, obj['FROM_AIRP']);
            mETD.value = obj['ETD'];
            mETA.value = obj['ETA'];
            mVIA.value = obj['VIA'];
           
            
            mREMARK.value = obj['REMARK'];
            mMAX_DATE.value = obj['MAX_DATE'] == null ? null : obj['MAX_DATE']['DateTime'];
            mREMARK_SEND.value = obj['REMARK_SEND'];
        }
        function mShowDetail(id) {
            iRowSelect = 'tr' + id;
            IdSelect = id;
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        function mUpdateValueUpdate() {
            var uRow = document.getElementById(iRowSelect);
            uRow.cells[2].innerHTML = '<%= IDCHA%>';
            uRow.cells[3].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].text;
            uRow.cells[4].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].getAttribute('data-TaiTrong');
            uRow.cells[5].innerHTML = mDAYSFLIGHT.value;
            uRow.cells[6].innerHTML = mFLIGHTNBR.value;
            uRow.cells[7].innerHTML = mREGISTRATION.value;
            uRow.cells[8].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            uRow.cells[9].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            uRow.cells[10].innerHTML = mETD.value;
            uRow.cells[11].innerHTML = mETA.value;
            uRow.cells[12].innerHTML = mVIA.value;
<%--            uRow.cells[13].innerHTML = mSTATUS.value;
            uRow.cells[14].innerHTML = mLASTMODIFY.value;
            uRow.cells[15].innerHTML = '<%= _user.UserName %>';--%>
            uRow.cells[13].innerHTML = mPURPOSE_ID.options[mPURPOSE_ID.selectedIndex].text;
            uRow.cells[14].innerHTML = mMAX_DATE.value;
            uRow.cells[15].innerHTML = mREMARK.value.length > 40 ? mREMARK.value.substring(0, 40) + '...' : mREMARK.value;
            if (mREMARK.value.length > 40) {
                uRow.cells[15].setAttribute("onmouseover", "ddrivetip('" + mREMARK.value.replace(/\r?\n|\r/g, '<br>') + "', 'yellow', 500);");
                uRow.cells[15].setAttribute('onmouseout', 'hideddrivetip();');
                uRow.cells[15].setAttribute('ondblclick', 'copyToClipboard(' + mREMARK.value + ');');
            }
            uRow.cells[16].innerHTML = mREMARK_SEND.value.length > 40 ? mREMARK_SEND.value.substring(0, 40) + '...' : mREMARK_SEND.value;
            if (mREMARK_SEND.value.length > 40) {
                uRow.cells[16].setAttribute("onmouseover", "ddrivetip('" + mREMARK_SEND.value.replace(/\r?\n|\r/g, '<br>') + "', 'yellow', 500);");
                uRow.cells[16].setAttribute('onmouseout', 'hideddrivetip();');
                uRow.cells[16].setAttribute('ondblclick', 'copyToClipboard(' + mREMARK_SEND.value + ');');
            }
        }
    </script>
    <%--/-Script for multi add--%>
</asp:Content>
