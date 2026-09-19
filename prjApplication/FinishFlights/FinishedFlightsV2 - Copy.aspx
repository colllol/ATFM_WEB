<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="FinishedFlightsV2.aspx.cs" Inherits="prjApplication.FinishFlights.FinishedFlightsV2" %>

<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />--%>
    <link href="../Style/Style_List.css" rel="stylesheet" />
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

        .sControl {
            width: 100px;
            height: 23px;
            color: #777777;
        }

        #tblSource tr td:first-child + td {
            white-space: nowrap;
        }

            #tblSource tr td:first-child + td + td {
                white-space: nowrap;
            }
    </style>

    <span class="TitlePanel">+ FINISHED FILGHTS</span>
    <div class="well well-sm">
        <div class="classSearchHeader">
            <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td class="Titlelbl" style="text-align: right">START DATE:
                    </td>
                    <td style="text-align: left;width:14%;">
                        <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputtext date-picker"
                            type="text" style="width: 80%;" />
                    </td>
                    <td class="Titlelbl" style="text-align: right">END DATE:
                    </td>
                    <td style="text-align: left;width:14%;">
                        <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                            class="inputtext date-picker"
                            type="text" style="width: 80%;" />
                    </td>
                    
                    <td style="text-align: left;width:20%;">
                        <select id="ddlSelect" runat="server" style="width:80%;">
                            <option value="0">--</option>
                            <option value="1">Nối chuyến</option>
                            <option value="2">Bay chậm</option>
                            <option value="3">Bay chuyển sân</option>
                            <option value="4">Bay khác đường bay</option>
                            <option value="5">Trùng CallSigh</option>
                            <option value="6">Gần giống CallSign</option>                            
                        </select>
                    </td>
                   <td style="text-align: left;width:20%;">
                       <asp:Literal ID="lit" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>

        <div style="text-align: right; margin-right: 20px;">
            <asp:LinkButton ID="lnkDelete" OnClick="lnkShowDelete_Click" Visible="false" data-Value="1"
                runat="server">
                <i class="glyphicon glyphicon-trash" title="List delete"></i>
            </asp:LinkButton>
            <asp:LinkButton ID="LinkHideDelete" OnClick="lnkShowDelete_Click" data-Value="-1"
                runat="server" Visible="false">
                <i class="glyphicon glyphicon-th-list" title="List delete"></i>
            </asp:LinkButton>
        </div>
        <div class="table-responsive">
            <table id="tblSource" class="table table-bordered table-responsive">
                <thead>
                    <tr>
                        <th style="background-color: white;"><asp:Button ID="btnSearch" runat="server" Width="40px" Height="38px" CssClass="iconFind"
                            OnClick="btnSearch_Click" /></th>

                        <%--<th></th>--%>
                        <th>
                            <input type="text" class="sControl" id="txtPERMNBR" runat="server" /></th>
                        <th>
                            <asp:DropDownList CssClass="sControl" runat="server" ID="ddlOPER"></asp:DropDownList>
                        </th>
                        <th>
                            <select id="ddlPERMTYPE" cssclass="sControl" class="sControl" runat="server">
                                <option value="">--</option>
                                <option value="LD">LD</option>
                                <option value="O/F">O/F</option>
                            </select></th>
                        <th>
                            <select id="ddlFLIGHT_TYPE" runat="server" class="sControl">
                                <option value="">--</option>
                                <option value="SC">SC</option>
                                <option value="NO">NO</option>
                            </select></th>
                        <th>
                            <asp:DropDownList CssClass="sControl" ID="ddlPURPOSE" runat="server"></asp:DropDownList>
                        </th>
                        <th><input type="text" class="sControl" id="txtValidateHour" maxlength="2" data-number="true" runat="server" /></th>
                        <th><input type="text" class="sControl" id="txtFlightDate" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtFLIGHTNBR" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtREGISTRATION" runat="server" /></th>
                        <th>
                            <asp:DropDownList CssClass="sControl" ID="ddlFROM_AIRP" runat="server"></asp:DropDownList>
                        </th>
                        <th>
                            <asp:DropDownList CssClass="sControl" ID="ddlTO_AIRP" runat="server"></asp:DropDownList>
                        </th>
                        <th>
                            <input type="text" class="sControl" id="txtETD" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtETA" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtATD" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtATA" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtVIA" runat="server" /></th>
                        <th>
                            <asp:DropDownList CssClass="sControl" ID="ddlCRAFT_TYPE" runat="server"></asp:DropDownList>
                        </th>
                        <th>
                            <input type="text" class="sControl" id="txtREMARK" runat="server" /></th>
                        <th>
                            <input type="text" class="sControl" id="txtFPL_VIA" runat="server" /></th>
                        <th>
                            <asp:DropDownList CssClass="sControl" ID="ddlREAL_CRAFT_TYPE" runat="server"></asp:DropDownList>
                        </th>
                    </tr>
                    <tr>
                        <th>STT</th>
                        <%--<th>Edit</th>--%>

                        <th>PERMNBR</th>
                        <th>OPER_ID</th>
                        <th>PERMTYPE</th>
                        <th>FLIGHT_TYPE</th>
                        <th>PURPOSE</th>
                        <th>VALIDHOURS</th>
                        <th>FLIGHTDATE</th>
                        <th>FLIGHTNBR</th>
                        <th>REGISTRATION</th>
                        <th>FROM_AIRP</th>
                        <th>TO_AIRP</th>
                        <th>ETD</th>
                        <th>ETA</th>
                        <th>ATD</th>
                        <th>ATA</th>
                        <th>VIA</th>
                        <th>CRAFT_TYPE</th>
                        <th>REMARK</th>
                        <th>FPL_VIA</th>
                        <th>REAL_CRAFT_TYPE</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="grdSource1" runat="server" OnItemDataBound="grdSource1_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("RNUM") %></td>
                                <%--<td>
                                    <div class="action-buttons">
                                        <a data-toggle="tooltip" id="btnEdit" href="<%# Page.ResolveUrl("~/FinishFlights/EditFinishFlight.aspx") + "?Menu_ID="+ Request.Params["Menu_Id"]+ "&ID=" + Eval("FLIGHT_ID") %>"
                                            title="Edit">
                                            <i class="glyphicon glyphicon-pencil"></i>
                                        </a>
                                        <asp:LinkButton runat="server" ID="lnkDelete" data-toggle="tooltip"
                                            title="Delete" CommandArgument='<%# Eval("FLIGHT_ID") %>' OnClick="lnkDelete_Click">
                                        <i class="glyphicon glyphicon-trash"></i>
                                        </asp:LinkButton>
                                        <a href="#" data-toggle="tooltip" class="bigger-140 show-details-btn" title="Show history">
                                            <i class="ace-icon fa fa-angle-double-down"></i>
                                        </a>
                                    </div>
                                </td>--%>

                                <td><%# Eval("PERMNBR") %></td>
                                <td>
                                    <%# Eval("OPER_ID") %>
                                </td>
                                <td><%# Eval("PERMTYPE") %></td>
                                <td>
                                    <%# Eval("FLIGHT_TYPE") %>
                                </td>
                                <td>
                                    <%# Eval("PURPOSE") %>
                                </td>
                                <td><%# Eval("VALIDHOURS") %></td>
                                <td><%# Eval("FLIGHTDATE", "{0:dd/MM/yyyy}") %></td>
                                <td><%# Eval("FLIGHTNBR") %></td>
                                <td><%# Eval("REGISTRATION") %></td>
                                <td><%# Eval("FROM_AIRP") %></td>
                                <td><%# Eval("TO_AIRP") %></td>
                                <td><%# Eval("ETD") %></td>
                                <td><%# Eval("ETA") %></td>
                                <td><%# Eval("ATD") %></td>
                                <td><%# Eval("ATA") %></td>
                                <td><%# Eval("VIA") %></td>
                                <td><%# Eval("CRAFT_TYPE") %></td>
                                <td><%# Eval("REMARK") %></td>
                                <td><%# Eval("FPL_VIA") %></td>
                                <td><%# Eval("REAL_CRAFT_TYPE") %></td>
                            </tr>
                            <asp:Label ID="lblId" runat="server" Text='<%# Eval("FLIGHT_ID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblHis" runat="server" Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
        <div style="text-align: right">
            <cc1:PhanTrang ID="PhanTrang1" runat="server" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange"
                NumberViewPage="7" PageSize="10" />
        </div>

        <script src="../Style/assets/js/jquery-1.11.3.min.js"></script>
        <script src="../Style/assets/js/bootstrap.min.js"></script>
        <script src="../Style/assets/js/ace.min.js"></script>
        <script src="../Style/assets/js/ace.min.js"></script>
        <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
        <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
        <script src="../Scripts/CustomDynamic.js"></script>
        <script src="../Scripts/validate.js"></script>
        <script>
            var phanCach = '<%= _phanCach%>';
            function LoadDataGrid() {
                GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
            }
            function DisplayResult(resulf, context) {
                if (context == 'LoadDataGrid') {
                    $('#tblSource tbody tr').remove();
                    $('#tblSource tbody').append(resulf);
                }
                if (context == 'GetContent') {
                    document.getElementById('vContent').innerHTML = resulf;
                }
                if (context == 'btnDelete_Onclick') {
                    alert(resulf);
                }
                if (context == 'RestoreHistory') {
                    if (resulf != 'NOK') {
                        alert('Restore sussess!');
                        LoadDataGrid();
                    } else alert('Restore error!');
                }
            }
            function RestoreHistory(id, ver, UserID) {
                GetArgWithPostBack(id + phanCach + ver + phanCach + UserID + '_____RestoreHistory', 'RestoreHistory');
            }
            function LoadDataGrid() {
                GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
            }
            function GetContent(id) {
                GetArgWithPostBack(id + '_____GetContent', 'GetContent');
            }
            function btnDelete_Onclick(id) {
                GetArgWithPostBack(id + '_____btnDelete_Onclick', 'btnDelete_Onclick');
            }
            $('.show-details-btn').on('click', function (e) {
                e.preventDefault();
                $(this).closest('tr').next().toggleClass('open');
                $(this).find(ace.vars['.icon']).toggleClass('fa-angle-double-down').toggleClass('fa-angle-double-up');
            });
        </script>
</asp:Content>
