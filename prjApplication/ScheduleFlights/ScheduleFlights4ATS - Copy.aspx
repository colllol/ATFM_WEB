<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="ScheduleFlights4ATS.aspx.cs" Inherits="prjApplication.ScheduleFlights.ScheduleFlights4ATS" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Import Namespace="prjInfo" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />

    <style>
         table {
  font-family: 'Arial';
  margin: 5px auto;
  border-collapse: collapse;
  border: 1px solid #eee;
  border-bottom: 2px solid #428bca;
  box-shadow:0px 5px 10px rgba(0, 0, 0, 0.05), 0px 10px 10px rgba(0, 0, 0, 0.05), 0px 10px 15px rgba(0, 0, 0, 0.05);
}

table th, table td {
  
  border: 1px solid #eee;
  padding: 1px 0px;
  border-collapse: collapse;
}

table th {
  background: #428bca;
  color: #fff;
  text-transform: uppercase;
  font-size: 12px;
}
table th.last {
  border-right: none;
}
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

        .colorRender {
            color: red;
        }

        .inputControl {
            width: 300px !important;
        }

        .pagination {
            padding-right: 5px;
            margin-top: -5px;
        }

        #tblSource thead tr:first-child {
            height: 30px;
        }

        #tblSource thead tr th {
            padding: 6px;
        }

        .searchControl {
            height: 20px;
            width: 50px;
        }

            .searchControl:focus {
                width: 200px;
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

    <span class="TitlePanel">+ Schedule Flight</span>
    <div class="well well-sm" style="text-align: center;">
        <div>
            <label style="display: none">
                From
            </label>
            <label style="display: none">
                To
                <input id="txtToDate" runat="server" type="text" data-date-format="dd/mm/yyyy"
                    class="date-picker" /></label>
            <asp:Button ID="btnKeHoachBay" Visible="false" runat="server" CssClass="btn btn-sm btn-primary btn-bold"
                Text="Export DailyFlight" OnClick="btnKeHoachBay_Click" />

        </div>
        <div>
            <asp:LinkButton runat="server" ID="btnExportPdf" OnClick="btnExportPdf_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Pdf
            </asp:LinkButton>

            <asp:LinkButton runat="server" ID="btnExportExel" OnClick="btnExportExel_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Excel
            </asp:LinkButton>

            <%--<asp:LinkButton runat="server" ID="lbnCreate" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Create
        </asp:LinkButton>--%>
           
            <input id="txtFromDate" runat="server" type="text" Visible="false" data-date-format="dd/mm/yyyy"
                class="date-picker" placeholder="select date" />
           
        </div>
        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>
    <div style="text-align: right; margin-right: 20px;">
        <asp:LinkButton ID="lnkDelete" OnClick="lnkDelete_Click" Visible="false" data-Value="1"
            runat="server">
                <i class="glyphicon glyphicon-trash" title="List delete"></i>
        </asp:LinkButton>
        <asp:LinkButton ID="LinkHideDelete" OnClick="lnkDelete_Click" data-Value="-1" runat="server">
                <i class="glyphicon glyphicon-th-list" title="List delete"></i>
        </asp:LinkButton>
    </div>
    <div class="table-responsive table table-condensed">
        <table id="tblSource">
            <thead>
                <tr>
                    <th></th>
                    <th></th>
                    <th>PERMNBR</th>
                    <th>PERMTYPE</th>
                    <th>FLIGHT_TYPE</th>
                    <th>PURPOSE</th>
                    <th>CRAFT</th>
                    <th>MTOW</th>
                    <th>VALIDHOURS</th>
                    <th>DATE_OLD</th>
                    <th>FLIGHTDATE</th>
                    <th>FLIGHTNBR</th>
                    <th>REGISTRATION</th>
                    <th>FROM_AIRP</th>
                    <th>TO_AIRP</th>
                    <th>ETD</th>
                    <th>ETA</th>
                    <%--<th>ATD</th>
                    <th>ATA</th>--%>
                    <th>VIA</th>
                    <th>LETTERNBR_PK</th>
                    <th>NBR</th>
                    <th>OPER</th>
                    <th>REMARK</th>
                    <th>DOF</th>
                </tr>
                <tr>
                    <th>
                        <input id="chkAll" onchange="ChekAll();" type="checkbox" /></th>
                    <th style="background-color: unset">
                        <asp:LinkButton runat="server" ID="lnkSearch" OnClick="linkSearch_Click">
                            <i class="glyphicon glyphicon-search"></i>
                        </asp:LinkButton>
                        <%--<asp:LinkButton runat="server" ID="lnkClear" OnClick="lnkClear_Click">
                            <i class="glyphicon glyphicon-trash"></i>
                        </asp:LinkButton>--%>
                        <a onclick="ClearValueSearch();" class="action-buttons">
                            <i class="glyphicon glyphicon-trash"></i>
                        </a>
                    </th>
                    <th>
                        <asp:TextBox ID="txtPERMNBR_Search" runat="server" CssClass="searchControl wid_150px"></asp:TextBox>
                    </th>
                    <th>
                        <%--<asp:TextBox ID="txtPERMTYPE_Search" runat="server" CssClass="searchControl"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlPERMTYPE_Search" runat="server" CssClass="searchControl wid_70px"></asp:DropDownList>
                    </th>
                    <th>
                        <%--<asp:TextBox ID="txtFLIGHT_TYPE_Search" runat="server" CssClass="searchControl ­wid_40px"></asp:TextBox>--%>
                        <asp:DropDownList ID="ddlFLIGHT_TYPE_Search" runat="server" CssClass="searchControl wid_70px"></asp:DropDownList>
                    </th>
                    <th>
                        <asp:DropDownList ID="ddlPURPOSE_Search" runat="server" CssClass="searchControl wid_70px">
                        </asp:DropDownList>
                    <th>
                        <asp:DropDownList ID="ddlCRAFT_Search" runat="server" CssClass="searchControl wid_70px">
                        </asp:DropDownList>

                    </th>
                    <th>
                        <asp:TextBox ID="txtMTOW_Search" ReadOnly data-number="true" runat="server" CssClass="searchControl"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtVALIDHOURS_Search" ReadOnly data-number="true" runat="server"
                            CssClass="searchControl"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtDATE_OLD_Search"  onblur="checkInputDate(this);" data-date-format="dd/mm/yyyy"
                            class="date-picker" runat="server" CssClass="searchControl wid_120px"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtFLIGHTDATE_search" data-date-format="dd/mm/yyyy" onblur="checkInputDate(this);"
                            class="date-picker" runat="server" CssClass="searchControl wid_120px"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtFLIGHTNBR_Search" runat="server" CssClass="searchControl wid_100px"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtREGISTRATION_Search" runat="server" CssClass="searchControl wid_150px"></asp:TextBox>
                    </th>
                    <th>
                        <asp:DropDownList runat="server" ID="ddlFROM_AIRP_Search" CssClass="searchControl">
                        </asp:DropDownList>

                    </th>
                    <th>

                        <asp:DropDownList ID="ddlTO_AIRP_Search" runat="server" CssClass="searchControl">
                        </asp:DropDownList>
                    </th>
                    <th>
                        <asp:TextBox ID="txtETD_Search" data-number="true" runat="server" CssClass="searchControl"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtETA_Search" data-number="true" runat="server" CssClass="searchControl"></asp:TextBox>
                    </th>
                    <%--<th>
                        <asp:TextBox ID="txtATD_Search" data-number="true" runat="server" Width="100%"></asp:TextBox></th>--%>
                    <%--<th>
                        <asp:TextBox ID="txtATA_Search" data-number="true" runat="server" Width="100%"></asp:TextBox></th>--%>
                    <th>
                        <asp:TextBox ID="txtVIA_Search" runat="server" CssClass="searchControl wid_200px"></asp:TextBox>
                    </th>
                    <th>
                        <%--<asp:TextBox ID="txtLETTERNBR_PK_Search" runat="server" Width="100%"></asp:TextBox>--%>
                    </th>
                    <th>
                        <asp:TextBox ID="txtNBR_Search" runat="server" CssClass="searchControl wid_70px"></asp:TextBox>
                    </th>
                    <th></th>
                    <th>
                        <asp:TextBox ID="txtREMARK_Search" runat="server" CssClass="searchControl wid_150px"></asp:TextBox>
                    </th>
                    <th>
                        <asp:TextBox ID="txtDOF_Search" data-date-format="dd/mm/yyyy"
                            class="date-picker" runat="server" CssClass="searchControl wid_120px"></asp:TextBox>
                    </th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpSourceBody" runat="server" OnItemDataBound="rpSourceBody_ItemDataBound">
                    <ItemTemplate>
                        <tr class='<%# Eval("ISRENDER").ToString()=="1"? "colorRender":"" %>'>
                            <td style="text-align: center">
                                <asp:CheckBox Visible='<%# Eval("ISRENDER").ToString()=="1"?false:true %>' runat="server"
                                    ID="chkItem" data-Id='<%# Eval("ID") %>' />
                            </td>
                            <td>
                                <!--<asp:Label ID="lblId" runat="server" Visible="false" Text='<%# Eval("ID") %>'></asp:Label>
                                <div class="action-buttons" style="width: 65px" id="divattribute" runat="server">
                                    <a runat="server" visible='<%# isShowDelete? false:true%>' data-toggle="tooltip"
                                        title="Edit">
                                        <i class="ace-icon fa fa-pencil bigger-130" data-toggle="modal" data-target="#popupEditFlightDetail"
                                            onclick="ShowpopupEditFlightDetail(<%# Eval("ID") %>);"></i>
                                    </a>
                                    <a runat="server" visible='<%# isShowDelete? false:true%>' data-toggle="tooltip"
                                        title="Delete">
                                        <i class="ace-icon fa fa-trash-o bigger-130" onclick="btnDeleteOnclick(<%# Eval("ID") %>);">
                                        </i>
                                    </a>
                                    <a href="#" data-toggle="tooltip" class="bigger-140 show-details-btn" title="Show history">
                                        <i class="ace-icon fa fa-angle-double-down"></i>
                                    </a>
                                </div>-->
                            </td>
                            <td><%# Eval("PERMNBR") %></td>
                            <td><%# Eval("PERMTYPE") %></td>
                            <td><%# Eval("FLIGHT_TYPE") %></td>
                            <td><%# Eval("PURPOSE") %></td>
                            <td><%# Eval("CRAFT_ID") %></td>
                            <td><%# Eval("MTOW") %></td>
                            <td><%# Eval("VALIDHOURS") %></td>
                            <td><%# Eval("DATE_OLD", "{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("FLIGHTDATE","{0:dd/MM/yyyy}") %></td>
                            <td><%# Eval("FLIGHTNBR") %></td>
                            <td><%# Eval("REGISTRATION") %></td>
                            <td><%# Eval("FROM_AIRP") %></td>
                            <td><%# Eval("TO_AIRP") %></td>
                            <td><%# Eval("ETD") %></td>
                            <td><%# Eval("ETA") %></td>
                            <%--<td><%# Eval("ATD") %></td>
                            <td><%# Eval("ATA") %></td>--%>
                            <td><%# Eval("VIA") %></td>
                            <td><%# (DateTime)Eval("LETTERNBR_PK")==DateTime.MinValue?null: ((DateTime)Eval("LETTERNBR_PK")).ToString("dd/MM/yyyy")%></td>
                            <td><%# Eval("NBR") %></td>
                            <td><%# Eval("OPER_ID") %></td>
                            <td><%# Eval("REMARK") %></td>
                            <td><%# (DateTime)Eval("DOF")==DateTime.MinValue?null: ((DateTime)Eval("DOF")).ToString("dd/MM/yyyy") %></td>
                        </tr>
                        <asp:Label ID="lblHis" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>


    </div>
    <div style="text-align: right">
        <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="30" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange">
        </cc1:PhanTrang>

    </div>

    <div id="popupEditFlightDetail" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog wid_90">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupEditFlightDetail');" class="close"
                        data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Edit Day Flight</h4>
                </div>
                <div class="modal-body" style="text-align: left;">
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtPERMNBR" class="mLable control-label">PERMNBR</label>
                                        <input id="txtPERMNBR" data-control="_Update" maxlength="30" class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtPERMTYPE" class="mLable control-label">PERMTYPE</label>
                                        <select id="txtPERMTYPE" class="form-control mControl">
                                            <option value="LD">Landing</option>
                                            <option value="O/F">Over Flight</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtFLIGHT_TYPE" class="mLable control-label">FLIGHT_TYPE</label>
                                        <select id="txtFLIGHT_TYPE" class="form-control mControl">
                                            <option value="SC">SC</option>
                                            <option value="NO">NO</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= ddlPURPOSE.ClientID %>' class="mLable control-label">
                                            PURPOSE                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlPURPOSE">
                                        </asp:DropDownList>
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
                                        <label for='<%= ddlCRAFT_ID.ClientID %>' class="mLable control-label">
                                            CRAFT                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlCRAFT_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtMTOW" class="control-label mLable">MTOW</label>
                                        <input id="txtMTOW" type="text" maxlength="20"
                                            name="mMTOW"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtVALIDHOURS" class="control-label mLable">VALIDHOURS</label>
                                        <input id="txtVALIDHOURS" type="text" data-number="true" maxlength="2"
                                            name="mVALIDHOURS"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtDATE_OLD" class="mLable control-label">DATE_OLD</label>
                                        <input id="txtDATE_OLD" name="mDATE_OLD"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtFLIGHTDATE" class="mLable control-label">FLIGHTDATE</label>
                                        <input id="txtFLIGHTDATE" name="mFLIGHTDATE"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtFLIGHTNBR" class="control-label mLable">FLIGHTNBR</label>
                                        <input id="txtFLIGHTNBR" data-control="_Update" maxlength="20"
                                            data-minlenght="1"
                                            name="mFLIGHTNBR"
                                            type="text" class="form-control mControl" />
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
                                        <label for="txtNBR" class="mLable control-label">NBR</label>
                                        <input id="txtNBR" maxlength="14" data-control="_Update" name="mNBR"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtREGISTRATION" class="mLable control-label">REGISTRATION</label>
                                        <input id="txtREGISTRATION" maxlength="20"
                                            name="mREGISTRATION"
                                            class="form-control mControl" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= ddlFROM_AIRP.ClientID %>' class="mLable control-label">
                                            FROM_AIRP                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlFROM_AIRP">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= ddlTO_AIRP.ClientID %>' class="mLable control-label">
                                            TO_AIRP                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlTO_AIRP">
                                        </asp:DropDownList>
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
                                        <label for="txtETD" class="mLable control-label">ETD</label>
                                        <input id="txtETD" name="mETD" maxlength="4" data-control="_Update"
                                            class="form-control mControl" data-number="true"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtETA" class="mLable control-label">
                                            ETA
                                        </label>
                                        <input id="txtETA" name="mETA" maxlength="4" data-control="_Update" data-number="true"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label style="display: none" for="txtATD" class="mLable control-label">ATD</label>
                                        <input style="display: none" id="txtATD" name="mATD" data-number="true" maxlength="4"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtVIA" class="mLable control-label">VIA</label>
                                        <input id="txtVIA" name="mVIA"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtDOF" class="mLable control-label">DOF</label>
                                        <input id="txtDOF" name="mDOF"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
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
                                        <label for="txtATA" style="display: none" class="mLable control-label">ATA</label>
                                        <input id="txtATA" style="display: none" maxlength="4" data-number="true"
                                            name="mATA"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtLETTERNBR_PK" class="mLable control-label">LETTERNBR_PK</label>
                                        <input id="txtLETTERNBR_PK" name="mLETTERNBR_PK"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= ddlOPER_ID.ClientID %>' class="mLable control-label">
                                            OPER_ID                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlOPER_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtREMARK" class="mLable control-label">REMARK</label>
                                        <input id="txtREMARK" name="mREMARK"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="txtCODE" class="mLable control-label">CODE</label>
                                        <input id="txtCODE" maxlength="100" name="mCODE"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
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
                        <%--<button id="btnCreate" type="button" style="display: none" onclick="btnCreateOnclick();"
                            class="btn btn-sm btn-primary">
                            <i class="ace-icon fa fa-check"></i>
                            Create
                        </button>--%>
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
                <div class="modal-body" style="text-align: left;">
                    <div class="row">
                        <div class="form-inline" role="form">
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mPERMNBR" class="mLable control-label">PERMNBR</label>
                                        <input id="mPERMNBR" data-control="mUpdate" maxlength="30" class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mPERMTYPE" class="mLable control-label">PERMTYPE</label>
                                        <select id="mPERMTYPE" class="form-control mControl">
                                            <option value="LD">Landing</option>
                                            <option value="O/F">Over Flight</option>
                                        </select>
                                        <%--<input id="mPERMTYPE" maxlength="3" data-control="mUpdate" data-minlenght="1" class="form-control mControl"
                                            type="text" />--%>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mFLIGHT_TYPE" class="mLable control-label">FLIGHT_TYPE</label>
                                        <select id="mFLIGHT_TYPE" class="form-control mControl">
                                            <option value="SC">SC</option>
                                            <option value="NO">NO</option>
                                        </select>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= mPURPOSE.ClientID %>' class="mLable control-label">
                                            PURPOSE                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mPURPOSE">
                                        </asp:DropDownList>
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
                                        <label for='<%= mCRAFT_ID.ClientID %>' class="mLable control-label">
                                            CRAFT                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mCRAFT_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mMTOW" class="control-label mLable">MTOW</label>
                                        <input id="mMTOW" type="text" data-number="true" maxlength="20"
                                            name="mMTOW"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mVALIDHOURS" class="control-label mLable">VALIDHOURS</label>
                                        <input id="mVALIDHOURS" type="text" data-number="true" maxlength="2"
                                            name="mVALIDHOURS"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mDATE_OLD" class="mLable control-label">DATE_OLD</label>
                                        <input id="mDATE_OLD" name="mDATE_OLD"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mFLIGHTDATE" class="mLable control-label">FLIGHTDATE</label>
                                        <input id="mFLIGHTDATE" name="mFLIGHTDATE"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mFLIGHTNBR" class="control-label mLable">FLIGHTNBR</label>
                                        <input id="mFLIGHTNBR" data-control="mUpdate" maxlength="20" data-minlenght="1"
                                            name="mFLIGHTNBR"
                                            type="text" class="form-control mControl" />
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
                                        <label for="mNBR" class="mLable control-label">NBR</label>
                                        <input id="mNBR" maxlength="14" data-control="mUpdate" name="mNBR"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mREGISTRATION" class="mLable control-label">REGISTRATION</label>
                                        <input id="mREGISTRATION" maxlength="20"
                                            name="mREGISTRATION"
                                            class="form-control mControl" type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= mFROM_AIRP.ClientID %>' class="mLable control-label">
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
                                        <label for='<%= mTO_AIRP.ClientID %>' class="mLable control-label">
                                            TO_AIRP                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mTO_AIRP">
                                        </asp:DropDownList>
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
                                        <label for="mETD" class="mLable control-label">ETD</label>
                                        <input id="mETD" name="mETD" maxlength="4" data-number="true"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mETA" class="mLable control-label">
                                            ETA
                                        </label>
                                        <input id="mETA" name="mETA" maxlength="4"
                                            class="form-control mControl" data-number="true"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mATD" style="display: none" class="mLable control-label">ATD</label>
                                        <input id="mATD" style="display: none" name="mATD" maxlength="4" data-number="true"
                                            class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mREMARK" class="mLable control-label">REMARK</label>
                                        <input id="mREMARK" name="mREMARK" type="text" class="form-control mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mVIA" class="mLable control-label">VIA</label>
                                        <input id="mVIA" name="mVIA"
                                            class="form-control mControl"
                                            type="text" />
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
                                        <label style="display: none" for="mATA" class="mLable control-label">ATA</label>
                                        <input style="display: none" id="mATA" maxlength="4" name="mATA"
                                            class="form-control mControl" data-number="true"
                                            type="text" />
                                    </div>
                                </div>
                            </div>

                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mCODE" class="mLable control-label">CODE</label>
                                        <input id="mCODE" maxlength="100" name="mCODE"
                                            class="form-control mControl"
                                            type="text" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mLETTERNBR_PK" class="mLable control-label">LETTERNBR_PK</label>
                                        <input id="mLETTERNBR_PK" name="mLETTERNBR_PK"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for='<%= mOPER_ID.ClientID %>' class="mLable control-label">
                                            OPER_ID                                
                                        </label>
                                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="mOPER_ID">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-inline" role="form">



                            <div class="form-group" style="display: none;">
                                <div class="form-horizontal" role="form">
                                    <div class="form-group">
                                        <label for="mDOF" class="mLable control-label">DOF</label>
                                        <input id="mDOF" name="mDOF"
                                            data-date-format="dd/mm/yyyy"
                                            class="inputControl date-picker mControl" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>




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
                                        <th>Number
                                        </th>
                                        <th></th>
                                        <th>PERMNBR
                                        </th>
                                        <th>PERMTYPE
                                        </th>
                                        <th>FLIGHT_TYPE
                                        </th>
                                        <th>PURPOSE
                                        </th>
                                        <th>CRAFT
                                        </th>
                                        <th>MTOW
                                        </th>
                                        <th>VALIDHOURS
                                        </th>
                                        <th>DATE_OLD
                                        </th>
                                        <th>FLIGHTDATE
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
                                        <%--<th>ATD
                                        </th>
                                        <th>ATA
                                        </th>--%>
                                        <th>VIA
                                        </th>
                                        <%--<th>STATUS
                                        </th>--%>
                                        <th>LETTERNBR_PK
                                        </th>
                                        <th>NBR
                                        </th>
                                        <th>OPER
                                        </th>
                                        <%--<th>PLAN_STATUS
                                        </th>--%>
                                        <th>REMARK
                                        </th>
                                        <th>CODE
                                        </th>
                                        <th>DOF
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
                                        <%--<td></td>
                                        <td></td>--%>
                                        <%--<td></td>
                                        <td></td>--%>
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
                    <%--<button type="button" class="btn btn-primary">Save changes</button>--%>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/DateTimeFomat.js"></script>
    <%--<script src="../Scripts/validate.js"></script>--%>
    <script>
        function ClearValueSearch() {
            $('#<%= txtPERMNBR_Search.ClientID%>').val('');            
            $('#<%= txtDATE_OLD_Search.ClientID%>').val('');
            $('#<%= txtFLIGHTDATE_search.ClientID%>').val('');
            $('#<%= txtETD_Search.ClientID%>').val('');
            $('#<%= txtETA_Search.ClientID%>').val('');
            $('#<%= txtVIA_Search.ClientID%>').val('');
            $('#<%= txtNBR_Search.ClientID%>').val('');
            $('#<%= txtREMARK_Search.ClientID%>').val('');
            $('#<%= txtDOF_Search.ClientID%>').val('');
            $('#<%= ddlFLIGHT_TYPE_Search.ClientID%>').prop('selectedIndex', 0);
            $('#<%= ddlPERMTYPE_Search.ClientID%>').prop('selectedIndex', 0);
            $('#<%= ddlCRAFT_Search.ClientID%>').prop('selectedIndex', 0);
            $('#<%= ddlPURPOSE_Search.ClientID%>').prop('selectedIndex', 0);
            $('#<%= ddlFROM_AIRP_Search.ClientID%>').prop('selectedIndex', 0);
            $('#<%= ddlTO_AIRP_Search.ClientID%>').prop('selectedIndex', 0);
        }
    </script>


    <script>
        var phanCach = '<%= _phanCach %>';
        var IdSelect = '0';
        var isShowDelete = '<%= isShowDelete%>';
        var _objRender = JSON.parse('<%= _ObjRender%>');
        var txtPERMNBR = document.getElementById('txtPERMNBR');
        var txtPERMTYPE = document.getElementById('txtPERMTYPE');
        var txtFLIGHT_TYPE = document.getElementById('txtFLIGHT_TYPE');
        var ddlPURPOSE = document.getElementById('<%= ddlPURPOSE.ClientID%>');
        var ddlCRAFT_ID = document.getElementById('<%= ddlCRAFT_ID.ClientID%>');
        var txtMTOW = document.getElementById('txtMTOW');
        var txtVALIDHOURS = document.getElementById('txtVALIDHOURS');
        var txtDATE_OLD = document.getElementById('txtDATE_OLD');
        var txtFLIGHTDATE = document.getElementById('txtFLIGHTDATE');
        var txtFLIGHTNBR = document.getElementById('txtFLIGHTNBR');
        var txtREGISTRATION = document.getElementById('txtREGISTRATION');
        var ddlFROM_AIRP = document.getElementById('<%= ddlFROM_AIRP.ClientID%>');
        var ddlTO_AIRP = document.getElementById('<%= ddlTO_AIRP.ClientID%>');
        var txtETD = document.getElementById('txtETD');
        var txtETA = document.getElementById('txtETA');
        var txtATD = document.getElementById('txtATD');
        var txtATA = document.getElementById('txtATA');
        var txtVIA = document.getElementById('txtVIA');
        //var txtSTATUS = document.getElementById('txtSTATUS');
        var txtLETTERNBR_PK = document.getElementById('txtLETTERNBR_PK');
        var txtNBR = document.getElementById('txtNBR');
        var ddlOPER_ID = document.getElementById('<%= ddlOPER_ID.ClientID%>');
        //var txtPLAN_STATUS = document.getElementById('txtPLAN_STATUS');
        var txtREMARK = document.getElementById('txtREMARK');
        var txtCODE = document.getElementById('txtCODE');
        var txtDOF = document.getElementById('txtDOF');

        function ReadInfoFlightDetail(data) {
            var obj = JSON.parse(data);
            txtPERMNBR.value = obj['PERMNBR'];
            setSelectedValue(txtPERMTYPE.id, obj['PERMTYPE']);
            setSelectedValue(txtFLIGHT_TYPE.id, obj['FLIGHT_TYPE']);
            setSelectedValue(ddlPURPOSE.id, obj['PURPOSE']);
            setSelectedValue(ddlCRAFT_ID.id, obj['CRAFT_ID']);
            txtMTOW.value = obj['MTOW'];
            txtVALIDHOURS.value = obj['VALIDHOURS'];
            txtDATE_OLD.value = obj['DATE_OLD'] == null ? null : obj['DATE_OLD']['DateTime'];
            txtFLIGHTDATE.value = obj['FLIGHTDATE'] == null ? null : obj['FLIGHTDATE']['DateTime'];
            txtFLIGHTNBR.value = obj['FLIGHTNBR'];
            txtREGISTRATION.value = obj['REGISTRATION'];
            setSelectedValue(ddlFROM_AIRP.id, obj['FROM_AIRP']);
            setSelectedValue(ddlTO_AIRP.id, obj['TO_AIRP']);
            txtETD.value = obj['ETD'];
            txtETA.value = obj['ETA'];
            txtATD.value = obj['ATD'];
            txtATA.value = obj['ATA'];
            txtVIA.value = obj['VIA'];
            //txtSTATUS.value = obj['STATUS'];
            txtLETTERNBR_PK.value = obj['LETTERNBR_PK'] == null ? null : obj['LETTERNBR_PK']['DateTime'];
            txtNBR.value = obj['NBR'];
            setSelectedValue(ddlOPER_ID.id, obj['OPER_ID']);
            //txtPLAN_STATUS.value = obj['PLAN_STATUS'];
            txtREMARK.value = obj['REMARK'];
            txtCODE.value = obj['CODE'];
            txtDOF.value = obj['DOF'] == null ? null : obj['DOF']['DateTime'];
            checkCustomValidate();
        }
        function GetObjectInfo() {
            var _obj = _objRender;
            _obj['ID'] = IdSelect;
            _obj["FLIGHT_ID"] = IdSelect;
            _obj['PERMNBR'] = txtPERMNBR.value;
            _obj['PERMTYPE'] = txtPERMTYPE.value;
            _obj['FLIGHT_TYPE'] = txtFLIGHT_TYPE.value;
            _obj['PURPOSE'] = ddlPURPOSE.value;
            _obj['CRAFT_ID'] = ddlCRAFT_ID.value;
            _obj['MTOW'] = txtMTOW.value;
            _obj['VALIDHOURS'] = txtVALIDHOURS.value;
            _obj['DATE_OLD'] = txtDATE_OLD.value;
            _obj['FLIGHTDATE'] = txtFLIGHTDATE.value;
            _obj['FLIGHTNBR'] = txtFLIGHTNBR.value;
            _obj['REGISTRATION'] = txtREGISTRATION.value;
            _obj['FROM_AIRP'] = ddlFROM_AIRP.value;
            _obj['TO_AIRP'] = ddlTO_AIRP.value;
            _obj['ETD'] = txtETD.value;
            _obj['ETA'] = txtETA.value;
            _obj['ATD'] = txtATD.value;
            _obj['ATA'] = txtATA.value;
            _obj['VIA'] = txtVIA.value;
            //_obj['STATUS'] = txtSTATUS.value;
            _obj['LETTERNBR_PK'] = txtLETTERNBR_PK.value;
            _obj['NBR'] = txtNBR.value;
            _obj['OPER_ID'] = ddlOPER_ID.value;
            //_obj['PLAN_STATUS'] = txtPLAN_STATUS.value;
            _obj['REMARK'] = txtREMARK.value;
            _obj['CODE'] = txtCODE.value;
            _obj['DOF'] = txtDOF.value;
            _obj["LASTUSER"] = '<%= _user.UserID.ToString()%>';
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
            txtMTOW.value = '';
            txtREMARK.value = '';
            txtREGISTRATION.value = '';
            txtFLIGHTNBR.value = '';
            txtETD.value = '';
            txtVIA.value = '';
            txtREMARK.value = '';

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
            if (IdSelect != '') {
                if (checkValidCustomMinlenght('_Update'))
                    GetArgWithPostBack(JSON.stringify(GetObjectInfo()) + '_____btnUpdateOnclick',
        'btnUpdateOnclick');
            }
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
            if (context == 'RestoreHistory') {
                if (resulf != 'NOK') {
                    alert('Restore sussess!');
                    LoadDataGrid();
                } else alert('Restore error!');
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
            if (context == 'LoadDataGrid') {
                $('#tblSource tbody tr').remove();
                $('#tblSource tbody').append(resulf);
            }
        }
        function RestoreHistory(id, ver, UserID) {
            GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
        }
        $('.show-details-btn').on('click', function (e) {
            e.preventDefault();
            $(this).closest('tr').next().toggleClass('open');
            $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
        });
        function ChekAll() {
            var $ele = $('#chkAll');
            $('#tblSource input:checkbox').each(function () {
                $(this).prop('checked', $ele.prop('checked'));
            });
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

        var mPERMNBR = document.getElementById('mPERMNBR');
        var mPERMTYPE = document.getElementById('mPERMTYPE');
        var mFLIGHT_TYPE = document.getElementById('mFLIGHT_TYPE');
        var mPURPOSE = document.getElementById('<%=mPURPOSE.ClientID%>');
        var mCRAFT_ID = document.getElementById('<%=mCRAFT_ID.ClientID%>');
        var mMTOW = document.getElementById('mMTOW');
        var mVALIDHOURS = document.getElementById('mVALIDHOURS');
        var mDATE_OLD = document.getElementById('mDATE_OLD');
        var mFLIGHTDATE = document.getElementById('mFLIGHTDATE');
        var mFLIGHTNBR = document.getElementById('mFLIGHTNBR');
        var mREGISTRATION = document.getElementById('mREGISTRATION');
        var mFROM_AIRP = document.getElementById('<%= mFROM_AIRP.ClientID%>');
        var mTO_AIRP = document.getElementById('<%= mTO_AIRP.ClientID%>');
        var mETD = document.getElementById('mETD');
        var mETA = document.getElementById('mETA');
        var mATD = document.getElementById('mATD');
        var mATA = document.getElementById('mATA');
        var mVIA = document.getElementById('mVIA');
        //var mSTATUS = document.getElementById('mSTATUS');
        var mLETTERNBR_PK = document.getElementById('mLETTERNBR_PK');
        var mNBR = document.getElementById('mNBR');
        var mOPER_ID = document.getElementById('<%= mOPER_ID.ClientID%>');
        //var mPLAN_STATUS = document.getElementById('mPLAN_STATUS');
        var mREMARK = document.getElementById('mREMARK');
        var mCODE = document.getElementById('mCODE');
        var mDOF = document.getElementById('mDOF');


        function mGetObjectInfo() {
            var _obj = _objRender;
            _obj['ID'] = IdSelect;
            _obj['FLIGHT_ID'] = IdSelect;
            _obj['PERMNBR'] = mPERMNBR.value;
            _obj['PERMTYPE'] = mPERMTYPE.value;
            _obj['FLIGHT_TYPE'] = mFLIGHT_TYPE.value;
            _obj['PURPOSE'] = mPURPOSE.value;
            _obj['CRAFT_ID'] = mCRAFT_ID.value;
            _obj['MTOW'] = mMTOW.value;
            _obj['VALIDHOURS'] = mVALIDHOURS.value;
            _obj['DATE_OLD'] = mDATE_OLD.value;
            _obj['FLIGHTDATE'] = mFLIGHTDATE.value;
            _obj['FLIGHTNBR'] = mFLIGHTNBR.value;
            _obj['REGISTRATION'] = mREGISTRATION.value;
            _obj['FROM_AIRP'] = mFROM_AIRP.value;
            _obj['TO_AIRP'] = mTO_AIRP.value;
            _obj['ETD'] = mETD.value;
            _obj['ETA'] = mETA.value;
            _obj['ATD'] = mATD.value;
            _obj['ATA'] = mATA.value;
            _obj['VIA'] = mVIA.value;
            //_obj['STATUS'] = mSTATUS.value;
            _obj['LETTERNBR_PK'] = mLETTERNBR_PK.value;
            _obj['NBR'] = mNBR.value;
            _obj['OPER_ID'] = mOPER_ID.value;
            //_obj['PLAN_STATUS'] = mPLAN_STATUS.value;
            _obj['REMARK'] = mREMARK.value;
            _obj['CODE'] = mCODE.value;
            _obj['DOF'] = mDOF.value;
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
        function mAddrowDynamic(id) {
            var new_row = mdefaulfRow.cloneNode(true);
            new_row.cells[0].innerHTML = idIndetiny;
            new_row.cells[1].innerHTML = '<div class="action-buttons"><a title="Edit"><i class="ace-icon fa fa-pencil bigger-130" onclick="mShowDetail(' + id + ');"></i></a><a title="Delete"><i class="ace-icon fa fa-trash-o bigger-130" onclick="mDeleteRowOnclick(' + id + ')"></i></a></div>';

            new_row.cells[2].innerHTML = mPERMNBR.value;
            new_row.cells[3].innerHTML = mPERMTYPE.options[mPERMTYPE.selectedIndex].text;;
            new_row.cells[4].innerHTML = mFLIGHT_TYPE.options[mFLIGHT_TYPE.selectedIndex].text;
            new_row.cells[5].innerHTML = mPURPOSE.options[mPURPOSE.selectedIndex].text;
            new_row.cells[6].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].text;
            new_row.cells[7].innerHTML = mMTOW.value;
            new_row.cells[8].innerHTML = mVALIDHOURS.value;
            new_row.cells[9].innerHTML = mDATE_OLD.value;
            new_row.cells[10].innerHTML = mFLIGHTDATE.value;
            new_row.cells[11].innerHTML = mFLIGHTNBR.value;
            new_row.cells[12].innerHTML = mREGISTRATION.value;
            new_row.cells[13].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            new_row.cells[14].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            new_row.cells[15].innerHTML = mETD.value;
            new_row.cells[16].innerHTML = mETA.value;
            //new_row.cells[17].innerHTML = mATD.value;
            //new_row.cells[18].innerHTML = mATA.value;
            new_row.cells[17].innerHTML = mVIA.value;
            //new_row.cells[20].innerHTML = mSTATUS.value;
            new_row.cells[18].innerHTML = mLETTERNBR_PK.value;
            new_row.cells[19].innerHTML = mNBR.value;
            new_row.cells[20].innerHTML = mOPER_ID.options[mOPER_ID.selectedIndex].text;
            //new_row.cells[24].innerHTML = mPLAN_STATUS.value;
            new_row.cells[21].innerHTML = mREMARK.value;
            new_row.cells[22].innerHTML = mCODE.value;
            new_row.cells[23].innerHTML = mDOF.value;
            new_row.id = 'tr' + id;
            tblMultiAdd.tBodies[0].appendChild(new_row);
            if (iRowAdd == 1)
                tblMultiAdd.tBodies[0].rows[0].remove();
            idIndetiny++;
            iRowAdd = 0;
        }
        function mClearValueControl() {
            mETA.value = ''; mETD.value = ''; mFLIGHTNBR.value = '';
            mMTOW.value = '';
            mREGISTRATION.value = '';
            mVIA.value = ''; mREMARK.value = '';
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
            mPERMNBR.value = obj['PERMNBR'];
            setSelectedValue(mPERMTYPE.id, obj['PERMTYPE']);
            setSelectedValue(mFLIGHT_TYPE.id, obj["FLIGHT_TYPE"]);
            setSelectedValue(mPURPOSE.id, obj['PURPOSE']);
            setSelectedValue(mCRAFT_ID.id, obj['CRAFT_ID']);
            mMTOW.value = obj['MTOW'];
            mVALIDHOURS.value = obj['VALIDHOURS'];
            mDATE_OLD.value = obj['DATE_OLD'] == null ? null : obj['DATE_OLD']['DateTime'];
            mFLIGHTDATE.value = obj['FLIGHTDATE'] == null ? null : obj['FLIGHTDATE']['DateTime'];
            mFLIGHTNBR.value = obj['FLIGHTNBR'];
            mREGISTRATION.value = obj['REGISTRATION'];
            setSelectedValue(mFROM_AIRP.id, obj['FROM_AIRP']);
            setSelectedValue(mTO_AIRP.id, obj['TO_AIRP']);
            mETD.value = obj['ETD'];
            mETA.value = obj['ETA'];
            mATD.value = obj['ATD'];
            mATA.value = obj['ATA'];
            mVIA.value = obj['VIA'];
            //mSTATUS.value = obj['STATUS'];
            mLETTERNBR_PK.value = obj['LETTERNBR_PK'] == null ? null : obj['LETTERNBR_PK']['DateTime'];
            mNBR.value = obj['NBR'];
            setSelectedValue(mOPER_ID.id, obj['OPER_ID']);
            //mPLAN_STATUS.value = obj['PLAN_STATUS'];
            mREMARK.value = obj['REMARK'];
            mCODE.value = obj['CODE'];
            mDOF.value = obj['DOF'] == null ? null : obj['DOF']['DateTime'];
            checkCustomValidate();
        }
        function mShowDetail(id) {
            iRowSelect = 'tr' + id;
            IdSelect = id;
            GetArgWithPostBack(id + '_____mShowDetail', 'mShowDetail');
        }
        function mUpdateValueUpdate() {
            var uRow = document.getElementById(iRowSelect);
            uRow.cells[2].innerHTML = mPERMNBR.value;
            uRow.cells[3].innerHTML = mPERMTYPE.options[mPERMTYPE.selectedIndex].text;;
            uRow.cells[4].innerHTML = mFLIGHT_TYPE.options[mFLIGHT_TYPE.selectedIndex].text;
            uRow.cells[5].innerHTML = mPURPOSE.options[mPURPOSE.selectedIndex].text;
            uRow.cells[6].innerHTML = mCRAFT_ID.options[mCRAFT_ID.selectedIndex].text;
            uRow.cells[7].innerHTML = mMTOW.value;
            uRow.cells[8].innerHTML = mVALIDHOURS.value;
            uRow.cells[9].innerHTML = mDATE_OLD.value;
            uRow.cells[10].innerHTML = mFLIGHTDATE.value;
            uRow.cells[11].innerHTML = mFLIGHTNBR.value;
            uRow.cells[12].innerHTML = mREGISTRATION.value;
            uRow.cells[13].innerHTML = mFROM_AIRP.options[mFROM_AIRP.selectedIndex].text;
            uRow.cells[14].innerHTML = mTO_AIRP.options[mTO_AIRP.selectedIndex].text;
            uRow.cells[15].innerHTML = mETD.value;
            uRow.cells[16].innerHTML = mETA.value;
            //uRow.cells[17].innerHTML = mATD.value;
            //uRow.cells[18].innerHTML = mATA.value;
            uRow.cells[17].innerHTML = mVIA.value;
            //new_row.cells[20].innerHTML = mSTATUS.value;
            uRow.cells[18].innerHTML = mLETTERNBR_PK.value;
            uRow.cells[19].innerHTML = mNBR.value;
            uRow.cells[20].innerHTML = mOPER_ID.options[mOPER_ID.selectedIndex].text;
            //new_row.cells[24].innerHTML = mPLAN_STATUS.value;
            uRow.cells[21].innerHTML = mREMARK.value;
            uRow.cells[22].innerHTML = mCODE.value;
            uRow.cells[23].innerHTML = mDOF.value;
        }
    </script>
    <%--/-Script for multi add--%>
    <script>
        function KiemTraDieuKienRenToDayFlight() {
            if ($('#<%= txtFromDate.ClientID%>').val() == '') {
                alert('Please choise date from: From date');
                $('#<%= txtFromDate.ClientID%>').focus();
                return false;
            }
            return true;
        }
    </script>
</asp:Content>

