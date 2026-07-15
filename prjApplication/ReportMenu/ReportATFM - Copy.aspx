<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ReportATFM.aspx.cs" Inherits="prjApplication.ReportMenu.ReportATFM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <script language="Javascript" type="text/javascript">
        function f_CallReports_old(valuePath) {
            var _fromDate = document.getElementById("ctl00_MainContent_txtBEGINDATE").value;
            var _toDate = document.getElementById("ctl00_MainContent_txtENDDATE").value;
            if ((_fromDate != '__/__/____') && (_toDate != '__/__/____')) {
                SubmitImage('../' + valuePath + '?FromDate=' + _fromDate + '&ToDate=' + _toDate + '', 1200, 580);
            }
            else {
                alert("Bạn chưa nhập khoảng thời gian !");
            }

        }

        function f_CallReports(valuePath) {
            SubmitImage('../' + valuePath + '', 1200, 580);
        }
    </script>

    <style>
        ul, #myUL {
            list-style-type: none;
        }

        #myUL {
            margin: 0;
            padding: 0;
        }

        .box {
            cursor: pointer;
            -webkit-user-select: none; /* Safari 3.1+ */
            -moz-user-select: none; /* Firefox 2+ */
            -ms-user-select: none; /* IE 10+ */
            user-select: none;
            font-size: 14px;
            font-weight: bold;
            margin-bottom: 10px;
        }

            .box::before {
                content: "\2610";
                color: black;
                display: inline-block;
                margin-right: 6px;
            }

        .check-box::before {
            content: "\2611";
            color: dodgerblue;
        }

        .nested {
            display: none;
        }

        .active {
            display: block;
        }

        .textlink {
            margin-top: 10px;
            margin-bottom: 10px;
        }
    </style>



    <div class="alert alert-block alert-success">
        <i class="ace-icon fa fa-check green"></i>
        <strong class="green">BÁO CÁO ĐIỀU HÀNH BAY									
        </strong>
    </div>

    <div class="row">
        <!--
        <div class="col-sm-3">

            <div class="widget-box">

                <div class="widget-body">
                    <div class="widget-main">
                        <div class="input-group">
                            <span class="lbl">BEGIN DATE</span>
                            <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                style="width: 80%" onkeypress='return check_num(this,14,event)' type="text" />

                        </div>
                        <div class="hr"></div>
                        <div class="input-group">
                            <span class="lbl">END DATE</span>
                            <input id="txtENDDATE" class="datepicker" autocomplete="off" runat="server"
                                style="width: 80%" onkeypress='return check_num(this,14,event)' type="text" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12">
            <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false"
                OnRowDataBound="grdSource_RowDataBound"
                Width="100%"
                CssClass="Grid table table-striped table-bordered">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="90%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="left" Width="90%"></ItemStyle>
                        <HeaderTemplate>
                            REPORT NAME
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="#" class="linkGridForm" onclick="f_CallReports('<%# Eval("ADDRESS_FILE") %>');"><%# Eval("NAME_REPORT") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>-->


        <div class="col-sm-12">
            <ul id="myUL">
                <li class="linkGridForm"><span class="box">I. BÁO CÁO BAN TÀI CHÍNH THEO QĐ 1024</span>
                    <ul class="nested active">
                        <li class="linkGridForm"><span class="box">+ LD</span>
                            <ul class="nested active">
                                <li class="linkGridForm"><span class="box">- QUỐC NỘI</span>
                                    <ul class="nested">
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_BAYNGAY.aspx');">1. TỔNG HỢP SỐ LIỆU BAY</a></li>
										<li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_BAYNGAYExt.aspx');">1. TỔNG HỢP SỐ LIỆU BAY - FULL</a></li>
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_Hang_QN.aspx');">2. BÁO CÁO SỐ LIỆU CÁC HÃNG HK TRONG NƯỚC</a></li>
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_Hang_QN.aspx');">3. BÁO CÁO SỐ LIỆU ĐHB THEO HÃNG CÁC HÃNG HK QUỐC NỘI</a></li>
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_CatHa_Mien.aspx');">4. BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC</a></li>
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/BDC_SL_Bay_Hang_QN.aspx');">5. BẢNG ĐỐI CHIẾU SỐ LIỆU CÁC HÃNG HÀNG KHÔNG TRONG NƯỚC CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC</a></li>

                                    </ul>
                                </li>
                                <li><span class="box">- QUỐC TẾ</span>
                                    <ul class="nested">
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_HangQT.aspx');">6. BÁO CÁO SỐ LIỆU ĐHB CÁC HÃNG HK QUỐC TẾ ĐI ĐẾN</a></li>
                                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_HangQT_LD.aspx');">7. BÁO CÁO SỐ LIỆU ĐHB CÁC HÃNG HK QUỐC TẾ ĐI ĐẾN THEO HÃNG</a></li>

                                    </ul>
                                </li>
                            </ul>
                        </li>

                        <li class="linkGridForm"><span class="box">+ OF</span>
                            <ul class="nested">
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_OF.aspx');">8. BÁO CÁO SỐ LIỆU BAY QUÁ CẢNH</a></li>
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_OF_DOTXUAT.aspx');">9. BÁO CÁO CHUYỂN BAY QUÁ CẢNH ĐỘT XUẤT</a></li>
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_VIP_QS.aspx');">10. BÁO CÁO CHUYẾN BAY VIP, QUÂN SỰ</a></li>
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_Via_2FirHN_HCM.aspx');">11. BÁO CÁO SỐ LIỆU BAY QUA 2 FIR HÀ NỘI VÀ HCM</a></li>
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_VIA_2FIR_BY_OPER.aspx');">12. BÁO CÁO CÁC CHUYẾN BAY QUA FIR HÀ NỘI VÀ FIR HCM THEO HÃNG</a></li>
                                <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_VIA_2FIR_BY_CRAFT.aspx');">13. BÁO CÁO SỐ LIỆU BAY QUA FIR HÀ NỘI VÀ FIR HCM THEO LOẠI MÁY BAY</a></li>

                            </ul>
                        </li>
                    </ul>
                </li>
                <li class="linkGridForm"><span class="box">II. BÁO CÁO THEO QĐ 2816</span>
                    <ul class="nested">
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SLHK_Chung.aspx');">14. BÁO CÁO SỐ LIỆU BAY HÀNG KHÔNG CHUNG</a></li>
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_BY_VIA.aspx');">15. BÁO CÁO SỐ LIỆU ĐHB THEO ĐƯỜNG BAY</a></li>
                    </ul>
                </li>
                <li class="linkGridForm"><span class="box">III. BÁO CÁO KHÁC</span>
                    <ul class="nested">
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_BC_SoLieu.aspx');">16. BÁO CÁO SỐ LIỆU BAY</a></li>
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_CatHa_TT.aspx');">17. BÁO CÁO THEO KHUNG GIỜ BAY THEO SL THỰC TẾ </a></li>
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_BieuDo_KhungGio.aspx');">18. BIỂU ĐỒ THỐNG KÊ ĐÁNH GIÁ NĂNG LỰC KHAI THÁC VÙNG TRỜI</a></li>
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_CatHa_KHB.aspx');">19. BÁO CÁO THEO KHUNG GIỜ BAY THEO SL KHB</a></li>
                        <li class="textlink"><a href="#" class="linkGridForm" onclick="f_CallReports('RpDHB/RpDHB_SL_DuThao_ThucTe.aspx');">20. THỐNG KÊ ĐỐI CHIẾU SỐ LIỆU DỰ THẢO / THỰC TẾ</a></li>
                    </ul>
                </li>
            </ul>
        </div>
    </div>
    <script>
        var toggler = document.getElementsByClassName("box");
        var i;

        for (i = 0; i < toggler.length; i++) {
            toggler[i].addEventListener("click", function () {
                this.parentElement.querySelector(".nested").classList.toggle("active");
                this.classList.toggle("check-box");
            });
        }
    </script>






    <script language="javascript" type="text/javascript">

        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });
        $('#<%= txtENDDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });
    </script>
</asp:Content>
