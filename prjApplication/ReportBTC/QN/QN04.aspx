<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QN01.aspx.cs" Inherits="prjApplication.ReportBTC.QN.QN01" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BẢNG XÁC NHẬN THÔNG TIN CHUYẾN BAY HÃNG HK QUỐC NỘI BAY QUỐC TẾ</title>
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
                            <th class="tg-s6z2">TỔNG CÔNG TY QUẢN LÝ BAY VIỆT NAM</th>
                            <th class="tg-031e"></th>
                            <th class="tg-hgcj">CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM</th>
                        </tr>
                        <tr>
                            <td class="tg-hgcj">BAN TÀI CHÍNH<br>
                                ______________</td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Độc lập-Tự do- Hạnh phúc<br>
                                ______________</td>
                        </tr>
                        <tr>
                            <td class="tg-s6z2"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-s6z2">Hà nội, ngày……tháng…….năm 20…….</td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                                Kính gửi: Trung tâm hiệp đồng điều hành bay.
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                                BẢNG XÁC NHẬN THÔNG TIN CHUYẾN BAY<br />
                                HÃNG HK QUỐC NỘI BAY QUỐC TẾ<br />
                                THÁNG…..NĂM 20....
                            </td>                                                    
                        </tr>
                        <tr>
                            <td class="tg-031e">
                                1.	Hóa đơn số:
                            </td>
                        </tr> 
                         <tr>
                            <td class="tg-031e">
                                2.	Chi tiết chuyến bay:
                            </td>
                        </tr>                                                                         
                    </table>



                    <table class="tblNoidung" style="undefined;table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-hgcj">STT</th>
                            <th class="tg-hgcj">NGÀY</th>
                            <th class="tg-hgcj">SỐ HIỆU</th>
                            <th class="tg-hgcj">LOẠI MÁY BAY</th>
                            <th class="tg-hgcj">ĐĂNG BẠ</th>
                            <th class="tg-hgcj">FROM</th>
                            <th class="tg-hgcj">TO</th>
                            <th class="tg-hgcj">LÝ DO CHỐI TỪ</th>
                            <th class="tg-hgcj">XÁC NHẬN CỦA TT HĐ ĐHB</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">1</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">2</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">3</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">4</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">5</td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
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
                                BAN TÀI CHÍNH TỔNG CÔNG TY
                            </th>
                            <th class="tg-s6z2" colspan="3">
                                TRUNG TÂM HĐ – ĐHB
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TỔ TRƯỞNG TỔ <br /> TTT ĐHB<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG BAN TÀI CHÍNH<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG PHÒNG<br />(Ký,ghi rõ họ tên)
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

