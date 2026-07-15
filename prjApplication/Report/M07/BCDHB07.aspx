<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCDHB07.aspx.cs" Inherits="prjApplication.Report.M07.BCDHB07" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BÁO CÁO DANH SÁCH CÁC CHUYẾN BAY ĐỘT XUẤT QUÁ CẢNH</title>
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
                            <td class="tg-asmn" colspan="3">BÁO CÁO DANH SÁCH CÁC CHUYẾN BAY ĐỘT XUẤT QUÁ CẢNH
                                <br />THÁNG <%=_month %>  NĂM <%=_year %> <br /><%=_datekhoang %>


                            </td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3"></td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2" colspan="3">Kính gửi :  Ban Tài chính Tổng Công ty</td>
                        </tr>
                    </table>


                    
                    <table class="tg" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-031e">STT</th>
                            <th class="tg-yw4l">NGÀY BAY</th>
                            <th class="tg-yw4l">SỐ HIỆU</th>
                            <th class="tg-yw4l">LOẠI MÁY BAY</th>
                            <th class="tg-yw4l">ĐĂNG KÝ</th>
                            <th class="tg-yw4l">FROM</th>
                            <th class="tg-yw4l">TO</th>
                            <th class="tg-yw4l">ĐƯỜNG BAY</th>
                            <th class="tg-yw4l">GIỜ VÀO</th>
                            <th class="tg-yw4l">GIỜ RA</th>
                            <th class="tg-yw4l">SỐ PHÉP BAY</th>
                        </tr>
                        
                        <%=_content %>
                        <tr>
                            <td class="tg-031e">TỔNG</td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
                            <td class="tg-yw4l"></td>
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
