<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCDHB06.aspx.cs" Inherits="prjApplication.Report.M06.BCDHB06" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>THỐNG KÊ HOẠT ĐỘNG HÀNG KHÔNG CHUNG</title>
    <link href="../../Style/StyleReports.css" rel="stylesheet" />
</head>
<body style="color: white;">
    <form id="form1" runat="server">
        <div class="classSearchHeader">
            <table class="tblHeader" style="border: none; text-align: right; width: 100%; color: white;">
                <tr>
                    <td style="width: 20%; text-align: center;"></td>
                    <td style="width: 30%; text-align: center;">

                        <asp:LinkButton runat="server" ID="btnExportPdf" CssClass="btn btn-primary"
                            OnClick="btnPDF_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
                        </asp:LinkButton>

                    </td>
                    <td style="width: 30%; text-align: center;">
                        <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-primary"
                            OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                        </asp:LinkButton>
                    </td>
                    <td style="width: 20%; text-align: center;"></td>
                </tr>
            </table>
        </div>
        <div id="divContent" runat="server">
            <table border="0" style="width: 100%; color: white;" class="table table-condensed">

            <tr>
                <td>
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-s6z2">TRUNG TÂM QUẢN LÝ LUỒNG KHÔNG LƯU</th>
                            <th class="tg-031e"></th>
                            <th class="tg-hgcj">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</th>
                        </tr>
                        <tr>
                            <td class="tg-hgcj">TRUNG TÂM HĐB&amp;ĐPLKL<br>
                                ______________</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Độc lập-Tự do- Hạnh phúc<br>
                                ______________</td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">Số……./BC-ĐPL</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Hà nội, ngày <%=_date %> tháng <%=_month %> năm <%=_year %></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">THỐNG KÊ HOẠT ĐỘNG HÀNG KHÔNG CHUNG

                            </td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l" colspan="3">- Kỳ báo cáo:  <%=_datekhoang %></td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l" colspan="3">- Ngày báo cáo:  <%=_date %> tháng <%=_month %> năm <%=_year %></td>
                        </tr>
                    </table>


                    
                    <table class="tg" style="undefined; table-layout: fixed; width: 100%">
                        
                        <tr>
                            <th>Stt</th>
                            <th class="tg-031e">Nhà khai thác tàu bay</th>
                            <th class="tg-yw4l">Số hiệu chuyến bay (callsign)</th>
                            <th class="tg-yw4l">Loại MB (Aircraft type)</th>
                            <th class="tg-yw4l">Mục đích khai thác (purpose)</th>
                            <th class="tg-yw4l">Từ<br>
                                điểm (from)</th>
                            <th class="tg-yw4l">Đến<br>
                                điểm (to)</th>
                            <th class="tg-yw4l">Ngày khai thác (Date flight)</th>
                            <th class="tg-yw4l">Đường<br>
                                Hàng không (ATS route)</th>
                            <th class="tg-yw4l">Giờ<br>
                                cất cánh chính thức (ATD)</th>
                            <th class="tg-yw4l">Giờ hạ cánh chính thức (ATA)</th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">1</td>
                            <td class="tg-baqh">2</td>
                            <td class="tg-baqh">3</td>
                            <td class="tg-baqh">4</td>
                            <td class="tg-baqh">5</td>
                            <td class="tg-baqh">6</td>
                            <td class="tg-baqh">7</td>
                            <td class="tg-baqh">8</td>
                            <td class="tg-baqh">9</td>
                            <td class="tg-baqh">10</td>
                            <td class="tg-baqh">11</td>
                        </tr>
                        <%=_content %>
                    </table>


                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-s6z2">NGƯỜI LẬP<br>
                                ( Ký ,ghi rõ họ tên)</th>
                            <th class="tg-s6z2">TRƯỞNG TT…. ( ghi rõ tên,phòng ),<br>
                                ( Ký, ghi rõ họ tên)</th>
                            <th class="tg-s6z2">GIÁM ĐỐC,<br>
                                ( Ký, ghi rõ họ tên, đóng dấu)</th>
                        </tr>
                        <tr>
                            <td colspan="3" style="height: 80px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </div>

    </form>
</body>
</html>


