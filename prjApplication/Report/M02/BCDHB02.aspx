<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCDHB02.aspx.cs" Inherits="prjApplication.Report.M02.BCDHB02" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO SỐ LIỆU ĐIỀU HÀNH BAY</title>
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
                            <td class="tg-s6z2">Hà nội, ngày <%=_date %>  tháng <%=_month %> năm  <%=_year %></td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">BÁO CÁO SỐ LIỆU SẢN LƯỢNG ĐIỀU HÀNH BAY</td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l" colspan="3">- Kỳ báo cáo: <b> <%=_datekhoang %> </td>
                        </tr>
                        <tr>
                            <td class="tg-yw4l" colspan="3">- Ngày báo cáo: <%=_datetime %></td>
                        </tr>
                    </table>


                    <style type="text/css">
                        .tg {
                            border-collapse: collapse;
                            border-spacing: 0;
                        }

                            .tg td {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                padding: 10px 5px;
                                border-style: solid;
                                border-width: 1px;
                                overflow: hidden;
                                word-break: normal;
                            }

                            .tg th {
                                font-family: Arial, sans-serif;
                                font-size: 14px;
                                font-weight: normal;
                                padding: 10px 5px;
                                border-style: solid;
                                border-width: 1px;
                                overflow: hidden;
                                word-break: normal;
                            }

                            .tg .tg-baqh {
                                text-align: center;
                                vertical-align: top;
                            }

                            .tg .tg-hgcj {
                                font-weight: bold;
                                text-align: center;
                            }

                            .tg .tg-amwm {
                                font-weight: bold;
                                text-align: center;
                                vertical-align: top;
                            }

                            .tg .tg-yw4l {
                                vertical-align: top;
                            }

                            .tg .tg-9hbo {
                                font-weight: bold;
                                vertical-align: top;
                            }
                    </style>
                    <table class="tg" style="undefined; table-layout: fixed; width: 1175px">
                        
                        <tr>
                            <th class="tg-hgcj" rowspan="2">CHỈ TIÊU<br>
                            </th>
                            <th class="tg-hgcj" rowspan="2">Thực<br>
                                hiện Tháng</th>
                            <th class="tg-hgcj" colspan="3">SỐ LIỆU BÁO CÁO THÁNG</th>
                            <th class="tg-amwm" colspan="3">SỐ LIỆU CỘNG DỒN<br>
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-yw4l">Ước TH</td>
                            <td class="tg-yw4l">So với tháng<br>
                                trước</td>
                            <td class="tg-yw4l">So với cùng kỳ<br>
                                năm trước</td>
                            <td class="tg-yw4l">Ước TH</td>
                            <td class="tg-yw4l">So với cùng kỳ<br>
                                năm trước</td>
                            <td class="tg-yw4l">So với KH năm</td>
                        </tr>
                        <tr>
                            <td class="tg-amwm">a</td>
                            <td class="tg-baqh">b</td>
                            <td class="tg-baqh">c</td>
                            <td class="tg-baqh">d</td>
                            <td class="tg-baqh">e</td>
                            <td class="tg-baqh">f</td>
                            <td class="tg-baqh">g</td>
                            <td class="tg-baqh">h</td>
                        </tr>
                        <tr>
                            <td class="tg-9hbo" colspan="8">I. Số chuyến bay điều hành (Lần chuyến)</td>
                        </tr>
                        <%=_content %>
                        
                        <tr>
                            <td class="tg-9hbo">II.Km điều hành quy đổi (nghìn km)</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
                        <tr>
                            <td class="tg-9hbo">III. Doanh thu</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                        </tr>
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



