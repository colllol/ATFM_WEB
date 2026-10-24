<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CTTsau02.aspx.cs" Inherits="prjApplication.ReportBTC.QTDD.CTTsau02" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BÁO CÁO SỐ LIỆU THU TIỀN ĐHB CÁC HÃNG HKQT BAY ĐI ĐẾN THƯỜNG LỆ</title>
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
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 70%">

                        <tr>
                            <th class="tg-s6z2">TỔNG CÔNG TY QUẢN LÝ BAY VIỆT NAM</th>
                            <th class="tg-031e"></th>
                            <th class="tg-hgcj"></th>
                        </tr>
                        <tr>
                            <td class="tg-hgcj">CÔNG TY QUẢN LÝ BAY MIỀN….</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2"></td>
                        </tr>

                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">BÁO CÁO SỐ LIỆU THU TIỀN ĐHB CÁC HÃNG HKQT BAY ĐI ĐẾN THƯỜNG LỆ

                                <br />
                                (Từ ngày      /     /            đến ngày      /     /           )


                            </td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3"></td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2" colspan="3">Đơn vị: VND</td>
                        </tr>
                    </table>




                    <table class="tgBCQTDD" style="undefined; table-layout: fixed; width: 70%">

                        <tr>
                            <th class="tg-hgcj">STT</th>
                            <th class="tg-amwm">TÊN HÃNG</th>
                            <th class="tg-amwm">SỐ CHUYẾN</th>
                            <th class="tg-amwm">SỐ TIỀN</th>
                        </tr>
                        <tr>
                            <td class="tg-baqh">1</td>
                            <td class="tg-yw4l">Hãng A</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-baqh">2</td>
                            <td class="tg-yw4l">Hãng B</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-baqh">3</td>
                            <td class="tg-yw4l">Hãng C</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-amwm" colspan="2">CỘNG</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 70%">
                        <tr>
                            <td colspan="3" style="height: 40px; text-align: right;">Ngày lập</td>
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
                </td>
            </tr>

        </table>
        </div>

    </form>
</body>
</html>

