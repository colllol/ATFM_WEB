<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="EmergencyFlight.aspx.cs" Inherits="prjApplication.FlightManagement.EmergencyFlight" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
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
            position: fixed;
            font-family: Arial, Helvetica, sans-serif;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background: rgba(0,0,0,0.8);
            z-index: 99999;
            opacity: 1;
            -webkit-transition: opacity 400ms ease-in;
            -moz-transition: opacity 400ms ease-in;
            transition: opacity 400ms ease-in;
            pointer-events: visible;
        }

        .table td i:hover {
            cursor: pointer;
        }

        .inputControl {
            width: 300px !important;
        }
    </style>

    <style>
        .mControl {
            width: 200px !important;
            height: 24px !important;
        }

        .mLable {
            width: 120px !important;
        }

        .modal-dialog {
            width: 100% !important;
        }

        #MultiAdd label, input, select > option {
            font-size: 12px;
        }

        #MultiAdd .modal-footer button {
            height: 25px;
        }
    </style>

    <div class="table-responsive">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td></td>
                <td style="text-align: left">
                    <span class="TitlePanel">+ Flight LIST</span>
                </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td style="text-align: center">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
                        <tr>
                            <td style="text-align: right">
                                <div class="classSearchHeader">
                                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                                        <tr>
                                            <td style="width: 40%; text-align: left;">
                                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                                    placeholder="Search ..."
                                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px">
                                                </asp:Button>
                                            </td>

                                            <td style="width: 60%; text-align: left;">

                                                <%--<asp:LinkButton runat="server" ID="btnAddCountry" CssClass="btn btn-sm btn-primary btn-bold"
                                                OnClick="btnAddCountry_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
                                            </asp:LinkButton>--%>
                                                <button type="button" data-toggle="modal" data-target="#MultiAdd" id="btnCreateNew"
                                                    class="btn btn-sm btn-primary">
                                                    <i class="glyphicon glyphicon-plus"></i>
                                                    Create
                                                </button>
                                                <asp:Literal ID="lit" runat="server"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>

                                <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyNames="ID"
                                    Width="100%" OnRowDataBound="grdSource_RowDataBound"
                                    CssClass="table table-striped table-bordered table-hover">
                                    <%--<ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>--%>
                                    <Columns>
                                        <asp:BoundField Visible="False" DataField="ID">
                                            <HeaderStyle Width="1%"></HeaderStyle>
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="4%"></ItemStyle>
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div class="action-buttons" style="width: 65px">
                                                    <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Edit %>' title="Edit">
                                                        <i class="ace-icon fa fa-pencil bigger-130" onclick="ShowpopupEditFlightDetail(<%# Eval("ID") %>);">
                                                        </i>
                                                    </a>
                                                    <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Del %>' title="Delete">
                                                        <i class="ace-icon fa fa-trash-o bigger-130" onclick="btnDeleteOnclick(<%# Eval("ID") %>);">
                                                        </i>
                                                    </a>
                                                    <a href="#" data-toggle="tooltip" class="bigger-140 show-details-btn" title="Show history">
                                                        <i class="ace-icon fa fa-angle-double-down"></i>
                                                    </a>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                            <HeaderTemplate>
                                                Begin date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("BEGINDATE", "{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                End date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("ENDDATE", "{0:dd/MM/yyyy}") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--<asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                CALENDAR_ID
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("FlightPemission_NAME") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
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
                                                CARAFT_ID
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("CARAFT_ID") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                FLIGHT_PK
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("FLIGHT_PK") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY1
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY1") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY2
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY2") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY3
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY3") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY4
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY4") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY5
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY5") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY6
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY6") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="25%"></ItemStyle>
                                            <HeaderTemplate>
                                                DAY7
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# Eval("DAY7") %>
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
                                                <%# Eval("PURPOSE_NAME") %>
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
                                <table runat="server" id="tblA"></table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td></td>
                <td></td>
            </tr>
        </table>
    </div>
    <div id="popupEditFlightDetail" class="modal cssHide">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEditFlightDetail');" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit flight detail</h4>
                </div>
                <div class="modal-body">

                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtBEGINDATE">BEGINDATE</label>
                                    <input id="txtBEGINDATE" type="text" data-date-format="dd/mm/yyyy" class="inputControl date-picker" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtENDDATE">ENDDATE</label>
                                    <input id="txtENDDATE" type="text" data-date-format="dd/mm/yyyy" class="inputControl date-picker" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlCALENDAR_ID">CALENDAR_ID</label>
                                    <div class="col-lg-6">
                                        <asp:DropDownList ID="ddlCALENDAR_ID" runat="server" CssClass="form-control inputControl row">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtMTOW">MTOW</label>
                                    <input id="txtMTOW" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlCARAFT_ID">CARAFT_ID</label>
                                    <div class="col-xs-6">
                                        <asp:DropDownList ID="ddlCARAFT_ID" runat="server" CssClass="form-control inputControl row">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtFLIGHT_PK">FLIGHT_PK</label>
                                    <input id="txtFLIGHT_PK" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-12">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-2 control-label">DAY FLIFGT</label>
                                    <div class="col-lg-10">
                                        <label class="checkbox-inline">
                                            <input id="chkDay1" type="checkbox" />
                                            DAY 1
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay2" type="checkbox" />
                                            DAY 2
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay3" type="checkbox" />
                                            DAY 3
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay4" type="checkbox" />
                                            DAY 4
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay5" type="checkbox" />
                                            DAY 5
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay6" type="checkbox" />
                                            DAY 6
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="chkDay7" type="checkbox" />
                                            DAY 7
                                        </label>


                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtREMARK">REMARK</label>
                                    <input id="txtREMARK" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtREGISTRATION">REGISTRATION</label>
                                    <input id="txtREGISTRATION" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtFLIGHTNBR">FLIGHTNBR</label>
                                    <input id="txtFLIGHTNBR" type="text" class="inputControl" />
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
                                    <input id="txtETD" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="ddlPURPOSE_ID">PURPOSE_ID</label>
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
                                    <input id="txtETA" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-lg-4 control-label" for="txtVIA">VIA</label>
                                    <input id="txtVIA" type="text" class="inputControl" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="text-align: center">
                        <button id="btnUpdate" type="button" style="display: none" onclick="btnUpdateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Update
                        </button>
                        <button id="btnCreate" type="button" style="display: none" onclick="btnCreateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
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
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mETA" class="mLable control-label">ETA</label>
                                        <input id="mETA" class="form-control mControl" name="mETA" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%=mCALENDAR_ID.ClientID %>' class="mLable control-label">
                                            CALENDAR_ID                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mCALENDAR_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mETD" class="mLable control-label">ETD</label>
                                        <input id="mETD" class="form-control mControl" name="mETD" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mFLIGHT_PK" class="control-label mLable">FLIGHT_PK</label>
                                        <input id="mFLIGHT_PK" type="text" name="mFLIGHT_PK" class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%=mCARAFT_ID.ClientID %>' class="mLable control-label">
                                            CARAFT_ID                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mCARAFT_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mFLIGHTNBR" class="control-label mLable">FLIGHTNBR</label>
                                        <input id="mFLIGHTNBR" type="text" name="mFLIGHTNBR" class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%=mPURPOSE_ID.ClientID %>' class="mLable control-label">
                                            PURPOSE_ID                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mPURPOSE_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mMTOW" class="control-label mLable">MTOW</label>
                                        <input id="mMTOW" name="mMTOW" type="text" class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%=mFROM_AIRP.ClientID %>' class="mLable control-label">
                                            FROM_AIRP                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mFROM_AIRP">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%=mTO_AIRP.ClientID %>' class="mLable control-label">
                                            TO_AIRP                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mTO_AIRP">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mREGISTRATION" class="mLable control-label">REGISTRATION</label>
                                        <input id="mREGISTRATION" name="mREGISTRATION" class="form-control mControl" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mVIA" class="mLable control-label">VIA</label>
                                        <input id="mVIA" name="mVIA" class="form-control mControl" type="text" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mREMARK" class="mLable control-label">REMARK</label>
                                        <input id="mREMARK" name="mREMARK" class="form-control mControl" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mBEGINDATE" class="mLable control-label">BEGINDATE</label>
                                        <input id="mBEGINDATE" name="mBEGINDATE" data-date-format="dd/mm/yyyy" class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mENDDATE" class="mLable control-label">ENDDATE</label>
                                        <input id="mENDDATE" name="mENDDATE" data-date-format="dd/mm/yyyy" class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label class="control-label mLable">DAY FLIGHT</label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY1" type="checkbox" />
                                            DAY 1
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY2" type="checkbox" />
                                            DAY 2
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY3" type="checkbox" />
                                            DAY 3
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY4" type="checkbox" />
                                            DAY 4
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY5" type="checkbox" />
                                            DAY 5
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY6" type="checkbox" />
                                            DAY 6
                                        </label>
                                        <label class="checkbox-inline">
                                            <input id="mDAY7" type="checkbox" />
                                            DAY 7
                                        </label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-inline">
                                <button id="mbtnAddNewFlightDetail" onclick="mbtnAddNewFlightDetailOnclick();"
                                    type="button"
                                    class="btn btn-primary">
                                    Add new</button>
                                <button id="mbtnUpdateFlightDetail" disabled="disabled" onclick="mbtnUpdateFlightDetailOnclick();"
                                    type="button"
                                    class="btn btn-primary">
                                    Update</button>
                                <button id="mbtnCancelFlightDetail" disabled="disabled" onclick="mbtnCancelFlightDetailOnclick();"
                                    type="button"
                                    class="btn btn-primary">
                                    Cancel</button>
                            </div>
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
                                        <th>Number
                                        </th>
                                        <th></th>
                                        <th>CALENDAR
                                        </th>
                                        <th>ETA
                                        </th>
                                        <th>ETD
                                        </th>
                                        <th>FLIGHT_PK
                                        </th>
                                        <th>CARAFT
                                        </th>
                                        <th>FLIGHTNBR
                                        </th>
                                        <th>PURPOSE
                                        </th>
                                        <th>DAY1
                                        </th>
                                        <th>DAY2
                                        </th>
                                        <th>DAY3
                                        </th>
                                        <th>DAY4
                                        </th>
                                        <th>DAY5
                                        </th>
                                        <th>DAY6
                                        </th>
                                        <th>DAY7
                                        </th>
                                        <th>FROM_AIRP
                                        </th>
                                        <th>TO_AIRP
                                        </th>
                                        <th>MTOW
                                        </th>
                                        <th>REGISTRATION
                                        </th>
                                        <th>VIA
                                        </th>
                                        <th>REMARK
                                        </th>
                                        <th>BEGINDATE
                                        </th>
                                        <th>ENDDATE
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
                    <button type="button" class="btn btn-primary">Save changes</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
        >
    </div>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="http://localhost/prjApplication/Scripts/CustomDynamic.js"></script>
    <script src="http://localhost/prjApplication/Scripts/validate.js"></script>
    <%--Script validate--%>
    <script>

    </script>
    <%--/Script validate--%>

    <script>
        var IdSelect = '0';
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtBEGINDATE = document.getElementById('txtBEGINDATE');
        var txtENDDATE = document.getElementById('txtENDDATE');
        var ddlCALENDAR_ID = document.getElementById('<%=ddlCALENDAR_ID.ClientID%>');
        var txtMTOW = document.getElementById('txtMTOW');
        var ddlCARAFT_ID = document.getElementById('<%=ddlCARAFT_ID.ClientID%>');
        var txtFLIGHT_PK = document.getElementById('txtFLIGHT_PK');
        var chkDAY1 = document.getElementById('chkDay1');
        var chkDAY2 = document.getElementById('chkDay2');
        var chkDAY3 = document.getElementById('chkDay3');
        var chkDAY4 = document.getElementById('chkDay4');
        var chkDAY5 = document.getElementById('chkDay5');
        var chkDAY6 = document.getElementById('chkDay6');
        var chkDAY7 = document.getElementById('chkDay7');
        var txtREMARK = document.getElementById('txtREMARK');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var ddlTO_AIRP = document.getElementById('<%= ddlTO_AIRP.ClientID%>');
        var ddlFROM_AIRP = document.getElementById('<%=ddlFROM_AIRP.ClientID%>');
        var txtETD = document.getElementById('txtETD');
        var ddlPURPOSE_ID = document.getElementById('<%=ddlPURPOSE_ID.ClientID%>');
        var txtETA = document.getElementById('txtETA');
        var txtVIA = document.getElementById('txtVIA');
        var btnCreate = document.getElementById('btnCreate');
        var btnUpdate = document.getElementById('btnUpdate');
        function ReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);
            //btnUpdate.removeAttribute('style');
            txtBEGINDATE.value = obj['BEGINDATE']['DateTime'];
            chkDAY1.checked = obj['DAY1'] == '0' ? false : true;
            chkDAY2.checked = obj['DAY2'] == '0' ? false : true;
            chkDAY3.checked = obj['DAY3'] == '0' ? false : true;
            chkDAY4.checked = obj['DAY4'] == '0' ? false : true;
            chkDAY5.checked = obj['DAY5'] == '0' ? false : true;
            chkDAY6.checked = obj['DAY6'] == '0' ? false : true;
            chkDAY7.checked = obj['DAY7'] == '0' ? false : true;
            txtENDDATE.value = obj['ENDDATE']['DateTime'];
            setSelectedValue(ddlCALENDAR_ID.id, obj['CALENDAR_ID'])
            txtMTOW.value = obj['MTOW'];
            setSelectedValue(ddlCARAFT_ID.id, obj['CARAFT_ID']);
            txtFLIGHT_PK.value = obj['FLIGHT_PK'];
            txtREMARK.value = obj['REMARK'];
            txtREGISTRATION.value = obj['REGISTRATION'];
            txtFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(ddlTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(ddlFROM_AIRP.id, obj['FROM_AIRP']);
            txtETD.value = obj['ETD'];
            setSelectedValue(ddlPURPOSE_ID.id, obj['PURPOSE_ID']);
            txtETA.value = obj['ETA'];
            txtVIA.value = obj['VIA'];
        }
        function GetObjectInfo() {
            var _obj = _objRender;
            _obj['CALENDAR_ID'] = ddlCALENDAR_ID.value;
            _obj['ETA'] = txtETD.value;
            _obj['ETD'] = txtETD.value;
            _obj['FLIGHT_PK'] = txtFLIGHT_PK.value;
            _obj['CARAFT_ID'] = ddlCALENDAR_ID.value;
            _obj['FLIGHTNBR'] = txtFLIGHTNBR.value;
            _obj['PURPOSE_ID'] = ddlPURPOSE_ID.value;
            _obj['DAY1'] = chkDAY1.checked == true ? '1' : '0';
            _obj['DAY2'] = chkDAY2.checked == true ? '2' : '0';
            _obj['DAY3'] = chkDAY3.checked == true ? '3' : '0';
            _obj['DAY4'] = chkDAY4.checked == true ? '4' : '0';
            _obj['DAY5'] = chkDAY5.checked == true ? '5' : '0';
            _obj['DAY6'] = chkDAY6.checked == true ? '6' : '0';
            _obj['DAY7'] = chkDAY7.checked == true ? '7' : '0';
            _obj['FROM_AIRP'] = ddlFROM_AIRP.value;
            _obj['TO_AIRP'] = ddlTO_AIRP.value;
            _obj['MTOW'] = txtMTOW.value;
            _obj['REGISTRATION'] = txtREGISTRATION.value;
            _obj['VIA'] = txtVIA.value;
            _obj['REMARK'] = txtREMARK.value;
            _obj['BEGINDATE'] = txtBEGINDATE.value;
            _obj['ENDDATE'] = txtENDDATE.value;
            return _obj;
        }

        function setSelectedValue(idSelected, valueToSet) {
            var selectObj = document.getElementById(idSelected);
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].value == valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }

        function ShowpopupEditFlightDetail(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetOneObject', 'GetOneObject');
        }
        function ShowDetailNoEdit(id) {
            GetArgWithPostBack(id + '_____GetOneObjectNoUpdate', 'GetOneObjectNoUpdate');
        }
        function ClearValue() {
            txtBEGINDATE.value = '';
            txtENDDATE.value = '';
            txtETA.value = '';
            txtETD.value = '';
            txtFLIGHT_PK.value = '';
            txtFLIGHTNBR.value = '';
            txtMTOW.value = '';
            txtREGISTRATION.value = '';
            txtREMARK.value = '';
            txtVIA.value = '';
            ddlCALENDAR_ID.selectedIndex = 0;
            ddlCARAFT_ID.selectedIndex = 0;
            ddlFROM_AIRP.selectedIndex = 0;
            ddlPURPOSE_ID.selectedIndex = 0;
            ddlTO_AIRP.selectedIndex = 0;
            chkDAY1.checked = false;
            chkDAY2.checked = false;
            chkDAY3.checked = false;
            chkDAY4.checked = false;
            chkDAY5.checked = false;
            chkDAY6.checked = false;
            chkDAY7.checked = false;
            IdSelect = '0';
        }
        function ClosePopup(elem) {
            document.getElementById(elem).className = "modal cssHide";
            btnCreate.setAttribute('style', 'display: none');
            btnUpdate.setAttribute('style', 'display: none');
            ClearValue();
        }
        function ShowPopup(elem) {
            document.getElementById(elem).className = "modal cssShow";
        }

        function btnCreateOnclick() {
            GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnCreateOnclick',
    'btnCreateOnclick');
        }
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
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'GetOneObject') {
                if (resulf != '') {
                    ReadInfoFlightDetail(resulf);
                    document.getElementById('popupEditFlightDetail').className = "modal cssShow";
                    btnUpdate.removeAttribute('style');
                    
                }
            }
            if (context == 'GetOneObjectNoUpdate') {
                if (resulf != '') {
                    ReadInfoFlightDetail(resulf);
                    document.getElementById('popupEditFlightDetail').className = "modal cssShow";
                    btnUpdate.setAttribute('style', 'display: none');
                }
            }
            if (context == 'btnUpdateOnclick') {
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'btnDeleteOnclick'){
                alert(resulf);
                LoadDataGrid();
            }
            if (context == 'btnCreateOnclick'){
                alert(resulf);
                LoadDataGrid();
            }
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
                    if (mtblMultiAdd.tBodies[0].rows.length == 0)
                        {
                        mtblMultiAdd.tBodies[0].appendChild(mdefaulfRow);
                        iRowAdd = 1;
                    }
                }
            }
            if (context == 'mShowDetail') {
                mReadInfoFlightDetail(resulf);
                mbtnAddNewFlightDetail.setAttribute('disabled', 'disabled');
                mbtnUpdateFlightDetail.removeAttribute('disabled');
                mbtnCancelFlightDetail.removeAttribute('disabled');
            }
            if (context == 'mbtnUpdateFlightDetailOnclick') {
                if (resulf != "OK") {
                    alert("Update error!");
                }
                else {
                    mUpdateValueUpdate();
                }
            }
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
            }
        }

        $('.date-picker').datepicker({
            autoclose: true,
            todayHighlight: true
        })
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
        $(function () {
            $('[data-toggle="tooltip"]').tooltip()
        })
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
        var mCALENDAR_ID = document.getElementById('<%= mCALENDAR_ID.ClientID%>');
        var mETA = document.getElementById('mETA');
        var mETD = document.getElementById('mETD');
        var mFLIGHT_PK = document.getElementById('mFLIGHT_PK');
        var mCARAFT_ID = document.getElementById('<%= mCARAFT_ID.ClientID%>');
        var mFLIGHTNBR = document.getElementById('mFLIGHTNBR');
        var mPURPOSE_ID = document.getElementById('<%= mPURPOSE_ID.ClientID%>');
        var mDAY1 = document.getElementById('mDAY1');
        var mDAY2 = document.getElementById('mDAY2');
        var mDAY3 = document.getElementById('mDAY3');
        var mDAY4 = document.getElementById('mDAY4');
        var mDAY5 = document.getElementById('mDAY5');
        var mDAY6 = document.getElementById('mDAY6');
        var mDAY7 = document.getElementById('mDAY7');
        var mFROM_AIRP = document.getElementById('<%= mFROM_AIRP.ClientID%>');
        var mTO_AIRP = document.getElementById('<%= mTO_AIRP.ClientID%>');
        var mMTOW = document.getElementById('mMTOW');
        var mREGISTRATION = document.getElementById('mREGISTRATION');
        var mVIA = document.getElementById('mVIA');
        var mREMARK = document.getElementById('mREMARK');
        var mBEGINDATE = document.getElementById('mBEGINDATE');
        var mENDDATE = document.getElementById('mENDDATE');

        function mGetObjectInfo() {
            var _obj = _objRender;
            _obj['CALENDAR_ID'] = mCALENDAR_ID.value;
            _obj['ETA'] = mETA.value;
            _obj['ETD'] = mETD.value;
            _obj['FLIGHT_PK'] = mFLIGHT_PK.value;
            _obj['CARAFT_ID'] = mCARAFT_ID.value;
            _obj['FLIGHTNBR'] = mFLIGHTNBR.value;
            _obj['PURPOSE_ID'] = mPURPOSE_ID.value;
            _obj['DAY1'] = mDAY1.checked == true ? '1' : '0';
            _obj['DAY2'] = mDAY2.checked == true ? '2' : '0';
            _obj['DAY3'] = mDAY3.checked == true ? '3' : '0';
            _obj['DAY4'] = mDAY4.checked == true ? '4' : '0';
            _obj['DAY5'] = mDAY5.checked == true ? '5' : '0';
            _obj['DAY6'] = mDAY6.checked == true ? '6' : '0';
            _obj['DAY7'] = mDAY7.checked == true ? '7' : '0';
            _obj['FROM_AIRP'] = mFROM_AIRP.value;
            _obj['TO_AIRP'] = mTO_AIRP.value;
            _obj['MTOW'] = mMTOW.value;
            _obj['REGISTRATION'] = mREGISTRATION.value;
            _obj['VIA'] = mVIA.value;
            _obj['REMARK'] = mREMARK.value;
            _obj['BEGINDATE'] = mBEGINDATE.value;
            _obj['ENDDATE'] = mENDDATE.value;            
            return _obj;
        }

        function mbtnAddNewFlightDetailOnclick() {

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
            GetArgWithPostBack(JSON.stringify(mGetObjectInfo()) + '_____mbtnUpdateFlightDetailOnclick', 'mbtnUpdateFlightDetailOnclick');
        }
        function mAddrowDynamic(id) {
            var new_row = mdefaulfRow.cloneNode(true);
            new_row.cells[0].innerHTML = idIndetiny;
            new_row.cells[1].innerHTML = '<div class="action-buttons"><a title="Edit"><i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(' + id + ');"></i></a><a title="Delete"><i class="ace-icon fa fa-trash-o bigger-130" onclick="mDeleteRowOnclick(' + id + ')"></i></a></div>';
            new_row.cells[2].innerHTML = mCALENDAR_ID.options[mCALENDAR_ID.selectedIndex].text;
            new_row.cells[3].innerHTML = mETA.value;
            new_row.cells[4].innerHTML = mETD.value;
            new_row.cells[5].innerHTML = mFLIGHT_PK.value;
            new_row.cells[6].innerHTML = mCARAFT_ID.options[mCARAFT_ID.selectedIndex].text;
            new_row.cells[7].innerHTML = mFLIGHTNBR.value;
            new_row.cells[8].innerHTML = mPURPOSE_ID.options[mPURPOSE_ID.selectedIndex].text;
            new_row.cells[9].innerHTML = mDAY1.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[10].innerHTML = mDAY2.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[11].innerHTML = mDAY3.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[12].innerHTML = mDAY4.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[13].innerHTML = mDAY5.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[14].innerHTML = mDAY6.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[15].innerHTML = mDAY7.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            new_row.cells[16].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            new_row.cells[17].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            new_row.cells[18].innerHTML = mMTOW.value;
            new_row.cells[19].innerHTML = mREGISTRATION.value;
            new_row.cells[20].innerHTML = mVIA.value;
            new_row.cells[21].innerHTML = mREMARK.value;
            new_row.cells[22].innerHTML = mBEGINDATE.value;
            new_row.cells[23].innerHTML = mENDDATE.value;
            new_row.id = 'tr' + id;
            tblMultiAdd.tBodies[0].appendChild(new_row);
            if (iRowAdd == 1)
                tblMultiAdd.tBodies[0].rows[0].remove();
            idIndetiny++;
            iRowAdd = 0;
        }
        function mClearValueControl() {
            mETA.value = ''; mETD.value = ''; mFLIGHT_PK.value = ''; mFLIGHTNBR.value = ''; mDAY1.selected = false; mDAY2.selected = false; mDAY3.selected = false; mDAY4.selected = false; mDAY5.selected = false; mDAY6.selected = false; mDAY7.selected = false;
            mMTOW.value = '';
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
            mBEGINDATE.value = obj['BEGINDATE']['DateTime'];
            mDAY1.checked = obj['DAY1'] == '0' ? false : true;
            mDAY2.checked = obj['DAY2'] == '0' ? false : true;
            mDAY3.checked = obj['DAY3'] == '0' ? false : true;
            mDAY4.checked = obj['DAY4'] == '0' ? false : true;
            mDAY5.checked = obj['DAY5'] == '0' ? false : true;
            mDAY6.checked = obj['DAY6'] == '0' ? false : true;
            mDAY7.checked = obj['DAY7'] == '0' ? false : true;
            mENDDATE.value = obj['ENDDATE']['DateTime'];
            setSelectedValue(mCALENDAR_ID.id, obj['CALENDAR_ID'])
            mMTOW.value = obj['MTOW'];
            setSelectedValue(mCARAFT_ID.id, obj['CARAFT_ID']);
            mFLIGHT_PK.value = obj['FLIGHT_PK'];
            mREMARK.value = obj['REMARK'];
            mREGISTRATION.value = obj['REGISTRATION'];
            mFLIGHTNBR.value = obj['FLIGHTNBR'];
            setSelectedValue(mTO_AIRP.id, obj['TO_AIRP']);
            setSelectedValue(mFROM_AIRP.id, obj['FROM_AIRP']);
            mETD.value = obj['ETD'];
            setSelectedValue(mPURPOSE_ID.id, obj['PURPOSE_ID']);
            mETA.value = obj['ETA'];
            mVIA.value = obj['VIA'];
        }
        function mShowDetail(id) {
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        function mUpdateValueUpdate() {
            var uRow = document.getElementById(iRowSelect);
            uRow.cells[2].innerHTML = mCALENDAR_ID.options[mCALENDAR_ID.selectedIndex].text;
            uRow.cells[3].innerHTML = mETA.value;
            uRow.cells[4].innerHTML = mETD.value;
            uRow.cells[5].innerHTML = mFLIGHT_PK.value;
            uRow.cells[6].innerHTML = mCARAFT_ID.options[mCARAFT_ID.selectedIndex].text;
            uRow.cells[7].innerHTML = mFLIGHTNBR.value;
            uRow.cells[8].innerHTML = mPURPOSE_ID.options[mPURPOSE_ID.selectedIndex].text;
            uRow.cells[9].innerHTML = mDAY1.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[10].innerHTML = mDAY2.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[11].innerHTML = mDAY3.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[12].innerHTML = mDAY4.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[13].innerHTML = mDAY5.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[14].innerHTML = mDAY6.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[15].innerHTML = mDAY7.checked ? '<input type="checkbox" disabled="" checked="true"></input>' : '<input type="checkbox" disabled=""></input>';
            uRow.cells[16].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            uRow.cells[17].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            uRow.cells[18].innerHTML = mMTOW.value;
            uRow.cells[19].innerHTML = mREGISTRATION.value;
            uRow.cells[20].innerHTML = mVIA.value;
            uRow.cells[21].innerHTML = mREMARK.value;
            uRow.cells[22].innerHTML = mBEGINDATE.value;
            uRow.cells[23].innerHTML = mENDDATE.value;
        }
    </script>
    <%--/-Script for multi add--%>
</asp:Content>
