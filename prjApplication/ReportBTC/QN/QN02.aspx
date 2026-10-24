<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QN02.aspx.cs" Inherits="prjApplication.ReportBTC.QN.QN02" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BÁO CÁO SỐ LIỆU ĐHB THEO SÂN BAY CỦA CÁC HÃNG HK TRONG NƯỚC</title>
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
                            <td class="tg-hgcj">TRUNG TÂM QUẢN LÝ LUỒNG KHÔNG LƯU<br>
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
                                BÁO CÁO SỐ LIỆU ĐHB THEO SÂN BAY CỦA CÁC HÃNG HK TRONG NƯỚC 
                            </td>                                                     
                        </tr>                     
                        <tr>
                            <td class="tg-asmn" colspan="3">(Từ ngày……/……/……đến ngày……/……/……)</td>
                        </tr>
                    </table>



                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="3">SÂN BAY</th>
                            <th class="tg-s6z2" colspan="3">HÃNG (HVN)</th>
                            <th class="tg-s6z2" colspan="3">HÃNG…..</th>
                            <th class="tg-s6z2" colspan="2">TỔNG CỘNG</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">SỐ C.BAY</td>
                            <td class="tg-s6z2">SỐ C.BAY</td>
                            <td class="tg-s6z2">CỘNG</td>
                            <td class="tg-s6z2">SỐ C.BAY</td>
                            <td class="tg-s6z2">SỐ C.BAY</td>
                            <td class="tg-s6z2">CỘNG</td>
                            <td class="tg-s6z2">SỐ C.BAY</td>
                            <td class="tg-s6z2">SỐ C.BAY</td>                           
                          </tr>
                          <tr>
                            <td class="tg-s6z2">QUỐC NỘI</td>
                            <td class="tg-s6z2">QUỐC TẾ</td>
                            <td class="tg-s6z2"></td>
                            <td class="tg-s6z2">QUỐC NỘI</td>
                            <td class="tg-s6z2">QUỐC TẾ</td>
                            <td class="tg-s6z2"></td>
                            <td class="tg-s6z2">QUỐC NỘI</td>
                            <td class="tg-s6z2">QUỐC TẾ</td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVCI</td>
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
                            <td class="tg-s6z2">VVDB</td>
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
                            <td class="tg-s6z2">VVNB</td>
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
                            <td class="tg-s6z2">….</td>
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
                            <td class="tg-s6z2">CỘNG</td>
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
                    <table class="tblHeader" style="undefined; table-layout: fixed; width: 100%">

                        <tr>
                            <th class="tg-s6z2">NGƯỜI LẬP<br>
                                ( Ký ,ghi rõ họ tên)</th>
                            <th class="tg-s6z2">TRƯỞNG PHÒNG….<br /> ( ghi rõ tên,phòng )<br>
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


