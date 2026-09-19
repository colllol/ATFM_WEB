<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="StatisticDelayOrCancel.aspx.cs" Inherits="prjApplication.Atkl.StatisticDelayOrCancel" %>

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
        .table,th {
            border: 1px solid black;
            width:100px;
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

                    <%--<asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                            Font-Bold="true" Text="" Width="40px" Height="38px" OnClick="btnSearch_Click"></asp:Button>--%>
                    <asp:LinkButton runat="server" Style="padding: 8px 24px;" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Pdf
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" ID="btnExportExel" Style="padding: 8px 24px;" CssClass="btn btn-sm btn-primary btn-bold">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Excel
                    </asp:LinkButton>
                </td>
            </tr>
        </table>
    </div>
    <div class="table-responsive">
        
        <table class="table">
            <tr class="table">
                <th rowspan="2" class="table">STT</th>
                <th rowspan="2" class="table">Ngày khai thác</th>
                <th rowspan="2" class="table">Số phép bay</th>
                <th rowspan="2" class="table">Loại tàu bay</th>
                <th rowspan="2" class="table">Đăng bạ</th>
                <th rowspan="2" class="table">Tên gọi chuyến bay</th>
                <th rowspan="2" class="table">Sân bay khởi hành</th>
                <th rowspan="2" class="table">Sân bay đến</th>
                <th colspan="2" class="table">Giờ dự kiến theo phép bay(Cấp lần đầu)</th>
                <th colspan="2" class="table">Giờ dự kiến theo phép bay(Sửa đổi, bổ sung nếu có)</th>
                <th rowspan="2" class="table">Giờ khởi hành theo FPL</th>
                <th rowspan="2" class="table">Giờ khởi hành theo FPL điều chỉnh(nếu có)</th>
                <th rowspan="2" class="table">Giờ khởi hành theo điện văn DLA(nếu có)</th>
                <th colspan="2" class="table">Giờ khai thác thực tế theo điện văn DEP, AAR(Áp dụng cho chuyến bay chậm chuyến)</th>
                <th rowspan="2" class="table">Nguyên nhân chậm, hủy chuyến có xác minh của HHK(nếu có)</th>
                <th rowspan="2" class="table">Tình hình thời tiết</th>
                <th rowspan="2" class="table">Tình trạng trang thiết bị, kỹ thuật bảo đảm hoạt động bay</th>
                <th rowspan="2" class="table">Hoạt động quân sự, chuyên cơ... tác động chuyến bay(nếu có)</th>
                <th rowspan="2" class="table">Ghi chú</th>
            </tr>
            <tr class="table"> 
                <td class="table">ETD</td>
                <td class="table">ETA</td>
                <td class="table">ETD</td>
                <td class="table">ETA</td>
                <td class="table">ATD</td>
                <td class="table">ATA</td>
            </tr>
        </table>
    </div>
    <div style="text-align: right">
        <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="20" />
    </div>

    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <div id="dhtmltooltip"></div>
    <%--<script>
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
        if (context == 'GetContent') {
            document.getElementById('vContent').innerHTML = resulf;
        }
    }
    function GetContent(id) {
        GetArgWithPostBack(id + '_____GetContent', 'GetContent');
    }
    </script>--%>
</asp:Content>
