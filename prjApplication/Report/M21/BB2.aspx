<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB2.aspx.cs" Inherits="prjApplication.Report.M21.BB2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐIỀU HÀNH BAY QUA CÁC HÃNG HÀNG KHÔNG BAY QUA FIR HỒ CHÍ MINH </title>
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
                            <td class="tg-asmn" colspan="3">
                                BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐIỀU HÀNH BAY QUA <br />CÁC HÃNG HÀNG KHÔNG BAY QUA FIR HỒ CHÍ MINH
                            </td>                                                     
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">(<%=_datekhoang %>)</td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">Giữa Trung tâm hiệp đồng điều hành bay và Công ty QLB Miền Bắc</td>
                        </tr>
                    </table>



                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="2">THÁNG</th>
                            <th class="tg-s6z2" rowspan="2">SỐ CHUYẾN THEO BÁO CÁO</th>
                            <th class="tg-s6z2" colspan="3">SỐ CHUYẾN TĂNG GIẢM</th>
                            <th class="tg-s6z2" rowspan="2">TỔNG SỐ CHUYẾN BAY SAU KHI ĐÃ ĐIỀU CHỈNH TĂNG GIẢM</th>
                            <th class="tg-s6z2" rowspan="2">GHI CHÚ</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">SỐ CHUYẾN TĂNG</td>
                            <td class="tg-s6z2">SỐ CHUYẾN GIẢM</td>
                            <td class="tg-s6z2">TỔNG SỐ</td>                                            
                          </tr>
                          <%=_content %>
                          <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>                        
                          </tr>
                          <tr>
                            <td class="tg-s6z2">TỔNG CỘNG</td>
                            <td class="tg-031e"><%=_total %></td>
                            <td class="tg-031e">0</td>
                            <td class="tg-031e">0</td>
                            <td class="tg-031e">0</td>
                            <td class="tg-031e">0</td>
                            <td class="tg-031e"></td>                            
                          </tr>                        
                    </table>                   
                </td>
            </tr>           
            <tr>
                <td style="height: 10px">
                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">

                        
                        <tr>
                            <th class="tg-s6z2" colspan="3">
                                TRUNG TÂM QUẢN LÝ LUỒNG KHÔNG LƯU
                            </th>
                            <th class="tg-s6z2" colspan="3">
                                CÔNG TY QUẢN LÝ BAY MIỀN BẮC
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG TTHĐB&ĐPLKL<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG TTÂM<br />(ghi rõ tên trung tâm)<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="height: 80px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>

        </table>
        </div>

    </form>
</body>
</html>


