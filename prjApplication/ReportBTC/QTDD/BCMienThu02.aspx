<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCMienThu02.aspx.cs" Inherits="prjApplication.ReportBTC.QTDD.BCMienThu02" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BÁO CÁO CÁC CHUYẾN BAY QUỐC TẾ ĐI ĐẾN MIỄN THU</title>
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
           

                    <table class="tgHearderList table table-condensed" style="table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-e3zv">TỔNG CÔNG TY QUẢN LÝ BAY VIỆT NAM</th>
                            <th class="tg-031e" colspan="2"></th>
                            <th class="tg-031e"></th>
                        </tr>
                        <tr>
                            <td class="tg-e3zv">CÔNG TY QUẢN LÝ BAY MIỀN…..</td>
                            <td class="tg-hgcj" colspan="2">BÁO CÁO CÁC CHUYẾN BAY QUỐC TẾ ĐI ĐẾN MIỄN THU</td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e" colspan="2"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e" colspan="2"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e">Thời gian: Từ                             Tới</td>
                            <td class="tg-031e" colspan="2"></td>
                            <td class="tg-031e">Currency: USD</td>
                        </tr>
                    </table>

                    <table class="tgListContent" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-hgcj">STT</th>
                            <th class="tg-hgcj">Hóa đơn</th>
                            <th class="tg-hgcj">Ngày bay</th>
                            <th class="tg-e3zv">Số hiệu</th>
                            <th class="tg-amwm">Loại máy bay</th>
                            <th class="tg-amwm">Mục đích bay</th>
                            <th class="tg-amwm">Kiểu bay</th>
                            <th class="tg-amwm">Giờ cất cánh</th>
                            <th class="tg-amwm">Giờ hạ cánh</th>
                            <th class="tg-amwm">Từ</th>
                            <th class="tg-amwm">Tới</th>
                            <th class="tg-amwm">Đường bay</th>
                            <th class="tg-amwm">Km điều hành</th>
                            <th class="tg-amwm">Giá cơ bản</th>
                            <th class="tg-amwm">Giảm giá</th>
                            <th class="tg-amwm">VAT</th>
                            <th class="tg-amwm">Phí điều hành</th>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-baqh"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-amwm" colspan="13">Total</td>
                            <td class="tg-amwm"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                    </table>
               
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">
                        <tr>
                            <td colspan="3" style="height: 40px; text-align: right;">Ngày…….</td>
                        </tr>
                         <tr>
                            <th class="tg-s6z2">Người lập<br>
                                ( Ký ,ghi rõ họ tên)</th>
                            <th class="tg-s6z2">Trưởng phòng tài chính..,<br>
                                ( Ký, ghi rõ họ tên)</th>
                            <th class="tg-s6z2">Giám đốc<br>
                                ( Ký, ghi rõ họ tên, đóng dấu)</th>
                        </tr>
                        <tr>
                            <td colspan="3" style="height: 80px;"></td>
                        </tr>
                    </table>
             
        </div>

    </form>
</body>
</html>

