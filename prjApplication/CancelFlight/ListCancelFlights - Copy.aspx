<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListCancelFlights.aspx.cs" Inherits="prjApplication.CancelFlight.ListCancelFlights" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/Style_List.css" rel="stylesheet" />
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

    <span class="TitlePanel">+ CANCEL FLIGHT LIST</span>
    <div class="well well-sm" style="text-align: center;">


        <asp:LinkButton runat="server" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
        </asp:LinkButton>

        <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
        </asp:LinkButton>


        <asp:Literal ID="lit" runat="server"></asp:Literal>
    </div>



    <div class="table-responsive table table-condensed">


        <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyNames="FLIGHT_ID"
            Width="100%" OnRowDataBound="grdSource_RowDataBound"
            CssClass="table table-bordered">
            <Columns>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="4%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="4%"></ItemStyle>
                    <HeaderTemplate>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                            Font-Bold="true" OnClick="linkSearch_Click" Visible="false" Text="" Width="40px"
                            Height="38px"></asp:Button>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="8%"></ItemStyle>
                    <HeaderTemplate>
                        PERMNBR
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum01" runat="server" Width="100%" Visible="false" Text='<%# Eval("PERMNBR") %>'></asp:TextBox>
                        <div id="col01" runat="server">
                            <%# Eval("PERMNBR") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        PERMTYPE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum02" runat="server" Width="100%" Visible="false" Text='<%# Eval("PERMTYPE") %>'></asp:TextBox>
                        <div id="col02" runat="server">
                            <%# Eval("PERMTYPE") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        FLIGHT_TYPE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum03" runat="server" Width="100%" Visible="false" Text='<%# Eval("FLIGHT_TYPE") %>'></asp:TextBox>
                        <div id="col03" runat="server">
                            <%# Eval("FLIGHT_TYPE") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        PURPOSE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum04" runat="server" Width="100%" Visible="false" Text='<%# Eval("PURPOSE") %>'></asp:TextBox>
                        <div id="col04" runat="server">
                            <%# Eval("PURPOSE") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        CRAFT_ID
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum05" runat="server" Width="100%" Visible="false" data-number="true" Text='<%# Eval("CRAFT_ID") %>'></asp:TextBox>
                        <div id="col05" runat="server">
                            <%# Eval("CRAFT_ID") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        MTOW
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum06" runat="server" Width="100%" Visible="false" Text='<%# Eval("MTOW") %>'></asp:TextBox>
                        <div id="col06" runat="server">
                            <%# Eval("MTOW") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        VALIDHOURS
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum07" runat="server" Width="100%" maxlength="2" Visible="false"  data-number="true" Text='<%# Eval("VALIDHOURS") %>'></asp:TextBox>
                        <div id="col07" runat="server">
                            <%# Eval("VALIDHOURS") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        DATE_OLD
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum08" runat="server" Width="100%" data-date-format="dd/mm/yyyy"
                            class="date-picker" Visible="false" data-number="true" Text='<%# Eval("DATE_OLD") %>'></asp:TextBox>
                        <div id="col08" runat="server">
                            <%# Eval("DATE_OLD", "{0:dd/MM/yyyy}") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        FLIGHTDATE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum09" runat="server" Width="100%" data-date-format="dd/mm/yyyy"
                            class="date-picker" Visible="false" data-number="true" Text='<%# Eval("FLIGHTDATE") %>'></asp:TextBox>
                        <div id="col09" runat="server">
                            <%# Eval("FLIGHTDATE","{0:dd/MM/yyyy}") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        FLIGHTNBR
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum10" runat="server" Width="100%" Visible="false" Text='<%# Eval("FLIGHTNBR") %>'></asp:TextBox>
                        <div id="col10" runat="server">
                            <%# Eval("FLIGHTNBR") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        REGISTRATION
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum11" runat="server" Width="100%" Visible="false" Text='<%# Eval("REGISTRATION") %>'></asp:TextBox>
                        <div id="col11" runat="server">
                            <%# Eval("REGISTRATION") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        FROM_AIRP
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum12" runat="server" Width="100%" Visible="false" Text='<%# Eval("FROM_AIRP") %>'></asp:TextBox>
                        <div id="col12" runat="server">
                            <%# Eval("FROM_AIRP") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        TO_AIRP
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum13" runat="server" Width="100%" Visible="false" Text='<%# Eval("TO_AIRP") %>'></asp:TextBox>
                        <div id="col13" runat="server">
                            <%# Eval("TO_AIRP") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        ETD
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum14" runat="server" Width="100%" Visible="false" data-number="true" Text='<%# Eval("ETD") %>'></asp:TextBox>
                        <div id="col14" runat="server">
                            <%# Eval("ETD") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        ETA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum15" runat="server" Width="100%" Visible="false" data-number="true" Text='<%# Eval("ETA") %>'></asp:TextBox>
                        <div id="col15" runat="server">
                            <%# Eval("ETA") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        ATD
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum16" runat="server" Width="100%" Visible="false" data-number="true" Text='<%# Eval("ATD") %>'></asp:TextBox>
                        <div id="col16" runat="server">
                            <%# Eval("ATD") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        ATA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum17" runat="server" Width="100%" Visible="false" data-number="true" Text='<%# Eval("ATA") %>'></asp:TextBox>
                        <div id="col17" runat="server">
                            <%# Eval("ATA") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        VIA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum18" runat="server" Width="100%" Visible="false" Text='<%# Eval("VIA") %>'></asp:TextBox>
                        <div id="col18" runat="server">
                            <%# Eval("VIA") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        LASTUSER
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum19" runat="server" Width="100%" Visible="false" Text='<%# Eval("LASTUSER") %>'></asp:TextBox>
                        <div id="col19" runat="server">
                            <%# Eval("LASTUSER") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        LASTMODIFY
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum20" runat="server" Width="100%" data-date-format="dd/mm/yyyy"
                            class="date-picker" Visible="false" data-number="true" Text='<%# Eval("LASTMODIFY") %>'></asp:TextBox>
                        <div id="col20" runat="server">
                            <%# Eval("LASTMODIFY","{0:dd/MM/yyyy}") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        CRAFT_TYPE
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum21" runat="server" Width="100%" Visible="false" Text='<%# Eval("CRAFT_TYPE") %>'></asp:TextBox>
                        <div id="col21" runat="server">
                            <%# Eval("CRAFT_TYPE") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        REMARK
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum22" runat="server" Width="100%" Visible="false" Text='<%# Eval("REMARK") %>'></asp:TextBox>
                        <div id="col22" runat="server">
                            <%# Eval("REMARK") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        FPL_VIA
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="txtColum23" runat="server" Width="100%" Visible="false" Text='<%# Eval("FPL_VIA") %>'></asp:TextBox>
                        <div id="col23" runat="server">
                            <%# Eval("FPL_VIA") %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>




    </div>
    <div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>

    <script>

        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
            }
        }

    </script>


</asp:Content>
