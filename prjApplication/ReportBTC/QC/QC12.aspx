<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QC12.aspx.cs" Inherits="prjApplication.ReportBTC.QC.QC12" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ XÁC NHẬN LẠI THÔNG TIN CHUYẾN BAY</title>
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
                            <td class="tg-asmn" colspan="3">Kính gửi :  Công ty quản lý bay Miền Bắc</td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                                XÁC NHẬN LẠI THÔNG TIN CHUYẾN BAY
                            </td>                                                    
                        </tr>   
                        <tr>
                            <td class="tg-031e">
                                1-	Hóa đơn (số hóa đơn, tháng, năm) 
                            </td>
                        </tr>
                        <tr>
                            <td class="tg-031e">
                                2-	Loại chuyến bay ( quốc nội, bay qua )
                            </td>
                        </tr> 
                        <tr>
                            <td class="tg-031e">
                                3-	Chi tiết chuyến bay :
                            </td>
                        </tr>                    
                    </table>



                    <table class="tblNoidung" style="undefined;table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-hgcj" colspan="10">CHI TIẾT CHUYẾN BAY</th>
                            <th class="tg-hgcj" rowspan="2">LÝ DO CẦN XÁC NHẬN</th>
                            <th class="tg-hgcj" rowspan="2">XÁC NHẬN CỦA PHÒNG HĐ ĐHB</th>                           
                          </tr>
                          <tr>
                            <td class="tg-hgcj">STT</td>
                            <td class="tg-hgcj">DATE</td>
                            <td class="tg-hgcj">CALLSIGN</td>
                            <td class="tg-hgcj">AIRCRAFT REGIST</td>
                            <td class="tg-hgcj">AIRCRAFT TYPE</td>
                            <td class="tg-hgcj">SC/NO</td>
                            <td class="tg-hgcj">PURPOSE</td>
                            <td class="tg-hgcj">FROM</td>
                            <td class="tg-hgcj">TO</td>
                            <td class="tg-hgcj">ROUTE</td>                          
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
                                CÔNG TY QUẢN LÝ BAY MIỀN BẮC
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TỔ TRƯỞNG TTT ĐHB<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG BAN TÀI CHÍNH<br />(Ký,ghi rõ họ tên,đóng dấu)
                            </td>
                            <td class="tg-s6z2">
                                NGƯỜI LẬP<br />(Ký,ghi rõ họ tên)
                            </td>
                            <td class="tg-s6z2">
                                TRƯỞNG TT HĐB&ĐPLKL<br />(Ký,ghi rõ họ tên)
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

