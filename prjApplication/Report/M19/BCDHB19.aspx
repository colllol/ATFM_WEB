<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BCDHB19.aspx.cs" Inherits="prjApplication.Report.M19.BCDHB19" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC PHÍA BẮC</title>
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
                              BÁO CÁO SẢN LƯỢNG CẤT HẠ CÁNH TẠI KHU VỰC PHÍA BẮC
                            </td>                                                     
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">(<%=_datekhoang %>)</td>
                        </tr> 
                        <tr>
                            <td class="tg-asmn" colspan="3">Kính gửi: Ban Tài chính-Tổng công ty Quản lý bay Việt nam</td>
                        </tr>
                    </table>



                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                         <tr>
                            <th class="tg-hgcj" rowspan="2">Ngày</th>
                            <th class="tg-hgcj" colspan="2">Nội Bài</th>
                            <th class="tg-hgcj" colspan="2">Điện Biên</th>
                            <th class="tg-hgcj" colspan="2">Cát Bi</th>
                            <th class="tg-hgcj" colspan="2">Vinh</th>
                            <th class="tg-hgcj" colspan="2">Đồng Hới</th>
                            <th class="tg-hgcj" colspan="2">Thọ xuân</th>
                            <th class="tg-hgcj" colspan="2">Vân Đồn</th>
                            <th class="tg-hgcj" colspan="2">Tổng</th>
                          </tr>
                        <tr>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td> 
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>
                            <td class="tg-s6z2">Đi</td>
                            <td class="tg-s6z2">Đến</td>                         
                          </tr>
                          <%=_content %>
                                                                             
                          <tr>
                            <td class="tg-s6z2">CỘNG</td>
                            <td class="tg-031e"><B><%=_tong01 %></B></td>
                            <td class="tg-031e"><B><%=_tong02 %></B></td>
                            <td class="tg-031e"><B><%=_tong03 %></B></td>
                            <td class="tg-031e"><B><%=_tong04 %></B></td>
                            <td class="tg-031e"><B><%=_tong05 %></B></td>
                            <td class="tg-031e"><B><%=_tong06 %></B></td>
                            <td class="tg-031e"><B><%=_tong07 %></B></td>
                            <td class="tg-031e"><B><%=_tong08 %></B></td>
                            <td class="tg-031e"><B><%=_tong09 %></B></td>
                            <td class="tg-031e"><B><%=_tong10 %></B></td>
                            <td class="tg-031e"><B><%=_tong11 %></B></td>
                            <td class="tg-031e"><B><%=_tong12 %></B></td>
                            <td class="tg-031e"><B><%=_tong15 %></B></td>
                            <td class="tg-031e"><B><%=_tong16 %></B></td>
                            <td class="tg-031e"><B><%=_tong13 %></B></td>
                            <td class="tg-031e"><B><%=_tong14 %></B></td>
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

