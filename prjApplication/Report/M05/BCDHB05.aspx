<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCDHB05.aspx.cs" Inherits="prjApplication.Report.M05.BCDHB05" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO TỔNG HỢP TÌNH HÌNH HOẠT ĐỘNG BAY TRONG NGÀY</title>
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
                                <td class="tg-asmn" colspan="3">BÁO CÁO TỔNG HỢP TÌNH HÌNH HOẠT ĐỘNG BAY TRONG NGÀY..
                                    <br />
                                    (Từ 00h00 đến 23h59 ngày <%=_dateFrom %> tính theo giờ UTC)

                                </td>
                            </tr>
                            <tr>
                                <td class="tg-yw4l" colspan="3">- Kỳ báo cáo:</td>
                            </tr>
                            <tr>
                                <td class="tg-yw4l" colspan="3">- Ngày báo cáo: <%=_dateFrom %> </td>
                            </tr>
                        </table>



                        <span style="font-weight: bold;">1.Số liệu chỉ huy </span>
                        <table class="tgNoiDung" style="undefined; table-layout: fixed; width: 100%">

                            <tr>
                                <th class="tg-e3zv"><b>TỔNG SỐ</b></th>
                                <th class="tg-031e"></th>
                            </tr>
                            <tr>
                                <td class="tg-e3zv"><b>Số chuyến bay VIP</b></td>
                                <td class="tg-031e"></td>
                            </tr>
                            <tr>
                                <td class="tg-e3zv"><b>Bay Quá cảnh</b></td>
                                <td class="tg-031e"></td>
                            </tr>
                            <tr>
                                <td class="tg-e3zv"><b>FIR HAN</b></td>
                                <td class="tg-031e"><%=_firHN %></td>
                            </tr>
                            <tr>
                                <td class="tg-e3zv"><b>FIR HCM</b></td>
                                <td class="tg-031e"><%=_firHCM %></td>
                            </tr>
                            <tr>
                                <td class="tg-e3zv"><b>1. Hãng hàng không Việt nam</b></td>
                                <td class="tg-031e"></td>
                            </tr>
                            <%=_content1 %>
                            <tr>
                                <td class="tg-9hbo">2.Hãng hàng không nước ngoài</td>
                                <td class="tg-yw4l"></td>
                            </tr>
                             <%=_content2 %>
                        </table>
                        <span style="font-weight: bold;">2.	Tình hình thời tiết trong ngày L </span>
                        <br />
                        <span style="font-weight: bold;">3.	Tình trạng hoạt động của hệ thống thiết bị bảo đảm hoạt động bay</span><br />
                        <span style="font-weight: bold;">4.	Các sự cố trong lĩnh vực quản lý bảo đảm hoạt động bay</span><br />
                        <span style="font-weight: bold;">5.	Các lưu ý và đề nghị nếu có </span>
                        <br />

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

