<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM_New.Master" CodeBehind="Atkl_1.aspx.cs" Inherits="prjApplication.Atkl.Atkl_1" EnableEventValidation="false"%>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
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
            z-index: 100;
            /*Remove below line to remove shadow. Below line should always appear last within this CSS*/
            filter: progid:DXImageTransform.Microsoft.Shadow(color=gray,direction=135);
        }

        .cssHide {
            display: none;
        }

        .cssShow {
            display: block;
        }

        .modal {
            overflow-x: hidden;
            overflow-y: auto;
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
    </style>

    <span class="TitlePanel">+ An toàn không lưu</span>
    <div class="classSearchHeader">
        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
            <tr>
                <td style="width: 7%; text-align: right;">START DATE
                </td>
                <td style="width: 10%; text-align: left;">
                    <input id="txtStartDate" runat="server" data-date-format="dd/mm/yyyy"
                        class="inputControl date-picker mControl"
                        type="text" />

                </td>
                <td style="width: 7%; text-align: right;">END DATE
                </td>
                <td style="width: 10%; text-align: left;">
                    <input id="txtFinishDate" runat="server" data-date-format="dd/mm/yyyy"
                        class="inputControl date-picker mControl"
                        type="text" />

                </td>

                <td style="width: 25%; text-align: left;">

                    <asp:LinkButton runat="server" OnClientClick="return confirm('Do you want confirm');" Style="padding: 8px 24px;" ID="btnimport" OnClick="btnimport_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    GetData
                    </asp:LinkButton>

                    <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                        Font-Bold="true" Text="" Width="40px" Height="38px" OnClick="btnSearch_Click"></asp:Button>
                    <asp:LinkButton Visible="false" runat="server" Style="padding: 8px 24px;" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Pdf
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" ID="btnExportExel" Style="padding: 8px 24px;" OnClick="btnExcel_Click" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt" ></span>
                                                    Excel
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>

    <div class="table-responsive">
        <asp:GridView ID="grdSource" runat="server" CssClass="table table-bordered table-hover"
            AutoGenerateColumns="false">
            <Columns>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div>
                        </div>
                        <tr>
                            <th rowspan="3">STT</th>
                            <th rowspan="3">Ngày khai thác</th>
                            <th rowspan="3">Loại tàu bay</th>
                            <th rowspan="3">Đăng bạ</th>
                            <th rowspan="3">Tên gọi chuyến bay</th>
                            <th rowspan="3">Sân bay khởi hành</th>
                            <th rowspan="3">Sân bay Đến</th>
                            <th colspan="4">Giờ dự kiến theo phép bay</th>
                            <th colspan="2">Giờ theo FPL</th>
                            <th colspan="2">Giờ khai thác thực tế theo điện văn DEP, AAR</th>
                            <th colspan="2">Đường bay</th>
                            <th rowspan="3">Ghi chú</th>
                        </tr>
                        <tr>
                            <td colspan="2">Theo phép cấp lần đầu</td>
                            <td colspan="2">Theo sửa đổi, bổ sung (nếu có)</td>
                            <td rowspan="2">Giờ khởi hành</td>
                            <td rowspan="2">Tổng thời gian đến sân bay đến</td>
                            <td rowspan="2">ATD</td>
                            <td rowspan="2">ATA</td>
                            <td rowspan="2">Theo phép bay</td>
                            <td rowspan="2">Thực tế</td>
                        </tr>
                        <tr>
                            <td>ETD</td>
                            <td>ETA</td>
                            <td>ETD</td>
                            <td>ETA</td>
                        </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Container.DataItemIndex +1 %>
                        <td>
                            <%# DataBinder.Eval(Container.DataItem, "DATE_EXPLOIT","{0:dd/MM/yyyy}") %>

                        </td>
                        <td>
                            <%# Eval("CRAFT_TYPE") %>
                        </td>
                        <td>
                            <%# Eval("REGISTRATION") %>
                        </td>
                        <td>
                            <%# Eval("CALLSIGN") %>
                        </td>
                        <td>
                            <%# Eval("FROM_AIRPORT") %>
                        </td>
                        <td>
                            <%# Eval("TO_AIRPORT") %>
                        </td>
                        <td>
                            <%# Eval("ETD_FIRST") %>
                        </td>
                        <td>
                            <%# Eval("ETA_FIRST") %>
                        </td>
                        <td>
                            <%# Eval("ETD_CHANGE") %>
                        </td>
                        <td>
                            <%# Eval("ETA_CHANGE") %>
                        </td>
                        <td>
                            <%# Eval("FPL_START") %>
                        </td>
                        <td>
                            <%# Eval("FPL_SUM").ToString() %>
                        </td>
                        <td>
                            <%# Eval("ATD") %>
                        </td>
                        <td>
                            <%# Eval("ATA") %>
                        </td>
                         <td>
                            <%# Eval("ROUTE_PERMISSION") %>
                        </td>
                        <td>
                            <%# Eval("ROUTE_REALITY") %>
                        </td>
                        <td>
                            <%# Eval("REMARK") %>
                        </td>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <%--<div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="500" />
    </div>--%>
     <div style="text-align: right; float:left" >
        <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="100" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <div id="dhtmltooltip"></div>
    <script>
        function Edit(id) {
            window.open('EditAtkl.aspx?Menu_ID=<%=(Request["Menu_ID"]!=null)?Request["Menu_ID"].ToString():""%>&ID=' + id, '_parent');
        }
        function ReadObj() {
            var obj = {
                AREA_PARKING: txtAREA_PARKING.value,
                AREA_EOBT: txtAREA_EOBT.value,
                AREA_AOBT: txtAREA_AOBT.value,
                AREA_START_UP: txtAREA_START_UP.value,
                AREA_PUSH_BACK: txtAREA_PUSH_BACK.value,
                AREA_START_RUN: txtAREA_START_RUN.value,
                AREA_TWR_APP: txtAREA_TWR_APP.value
            };
            return obj;
        }
        function ClosePopup(elem) {
            IdSelect = '';
            document.getElementById(elem).className = "modal cssHide";

        }
        function CreateFlight(elem) {

            document.getElementById(elem).className = "modal cssShow";

        }
        function LoadDataGrid() {
            GetArgWithPostBack('LoadDataGrid_____LoadDataGrid', 'LoadDataGrid');
        }
        function DisplayResult(resulf, context) {
            if (context == 'LoadDataGrid') {
                document.getElementById('<%=grdSource.ClientID%>').innerHTML = resulf;
        }
        else if (context == 'btnCreateOnclick') {
            alert(resulf);
            LoadDataGrid();
        }
    }
    function btnCreateOnclick() {
        GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnCreateOnclick', 'btnCreateOnclick');
    }
    function GetContent(id) {
        GetArgWithPostBack(id + '_____GetContent', 'GetContent');
    }
    </script>
</asp:Content>
