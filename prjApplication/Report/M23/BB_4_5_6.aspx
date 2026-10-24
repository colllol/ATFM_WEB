<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BB_4_5_6.aspx.cs" Inherits="prjApplication.Report.M23.BB_4_5_6" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU BAY CÁC HÃNG HÀNG KHÔNG QUỐC NỘI CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MB THÁNG </title>
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
                            <td class="tg-s6z2">Hà nội, ngày……tháng…….năm 20…….</td>
                        </tr>
                        <tr>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">
                                BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU BAY CÁC HÃNG HÀNG KHÔNG QUỐC NỘI<br />
                                CẤT HẠ CÁNH TẠI CÁC SÂN BAY KHU VỰC MB THÁNG…./20..

                            </td>                                                     
                        </tr>
                    </table>



                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="2">Hãng/sân bay</th>
                            <th class="tg-s6z2" colspan="3">HVN</th>
                            <th class="tg-s6z2" colspan="3">PIC</th>
                            <th class="tg-s6z2" colspan="1">VFC</th>
                            <th class="tg-s6z2" colspan="1"></th>
                            <th class="tg-s6z2" colspan="1"></th>
                            <th class="tg-s6z2" colspan="1">…</th>
                            <th class="tg-s6z2" colspan="1">…</th>
                            <th class="tg-s6z2" colspan="3">Tổng</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">QN đi QN</td>
                            <td class="tg-s6z2">QN đi QT</td>
                            <td class="tg-s6z2">QT về</td>
                            <td class="tg-s6z2">QN đi QN</td>
                            <td class="tg-s6z2">QN đi QT</td>
                            <td class="tg-s6z2">QT về</td>
                            <td class="tg-s6z2">QN đi QN</td>
                            <td class="tg-s6z2">QN đi QT</td>
                            <td class="tg-s6z2">QT về</td>
                            <td class="tg-s6z2"></td>
                            <td class="tg-s6z2"></td>
                            <td class="tg-s6z2">QN đi QN</td>
                            <td class="tg-s6z2">QN đi QT</td>   
                            <td class="tg-s6z2">QT về</td>                             
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
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVDH</td>
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
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">VVVH</td>
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
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                         <tr>
                            <td class="tg-s6z2">VVTX</td>
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
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                            <td class="tg-031e"></td>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">TỔNG</td>
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
                            <th class="tg-s6z2" colspan="2">
                                ĐẠI DIỆN TRUNG TÂM QLLKL
                            </th>
                            <th class="tg-s6z2" colspan="2">
                                ĐẠI DIỆN CÔNG TY QLBMB
                            </th>
                        </tr>
                        <tr>
                            <td class="tg-s6z2">
                                TRƯỞNG TT <br />HĐB&ĐPLKL
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC <br />TRUNG TÂM
                            </td>
                            <td class="tg-s6z2">
                                    TRƯỞNG TT <br />KS ĐƯỜNG DÀI
                            </td>
                            <td class="tg-s6z2">
                                GIÁM ĐỐC<br />CÔNG TY
                            </td>                            
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 80px;"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Nơi nhận:</b>

                </td>                
            </tr>
            <tr>
                <td>
                    <b>-Như kính gửi;</b>

                </td>                
            </tr>
            <tr>
                <td>
                    <b>-Lưu VT,ĐPL</b>

                </td>                
            </tr>
        </table>
        </div>

    </form>
</body>
</html>


