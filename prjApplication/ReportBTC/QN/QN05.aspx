<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QN05.aspx.cs" Inherits="prjApplication.ReportBTC.QN.QN05" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>+ BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐHB THEO THÁNG CỦA CÁC HÃNG HK TRONG NƯỚC</title>
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
                                BIÊN BẢN ĐỐI CHIẾU SỐ LIỆU ĐHB THEO THÁNG<br /> 
                                CỦA CÁC HÃNG HK TRONG NƯỚC
                            </td>                                                     
                        </tr>
                        <tr>
                            <td class="tg-asmn" colspan="3">(Từ ngày……/……/……đến ngày……/……/……)</td>
                        </tr>                        
                    </table>

                    <table class="tblNoidung" style="undefined; table-layout: fixed; width: 100%">
                          <tr>
                            <th class="tg-s6z2" rowspan="2">THÁNG</th>
                            <th class="tg-s6z2" colspan="4">BAY QUỐC NỘI</th>
                            <th class="tg-s6z2" colspan="4">BAY QUỐC TẾ</th>
                            <th class="tg-s6z2" rowspan="2">TỔNG CỘNG</th>
                          </tr>
                          <tr>
                            <td class="tg-s6z2">SỐ CHUYẾN THEO BÁO CÁO</td>
                            <td class="tg-s6z2">SỐ CHUYẾN TĂNG</td>
                            <td class="tg-s6z2">SỐ CHUYẾN GIẢM</td>
                            <td class="tg-s6z2">CỘNG</td>  
                            <td class="tg-s6z2">SỐ CHUYẾN THEO BÁO CÁO</td>
                            <td class="tg-s6z2">SỐ CHUYẾN TĂNG</td>
                            <td class="tg-s6z2">SỐ CHUYẾN GIẢM</td>
                            <td class="tg-s6z2">CỘNG</td>                                             
                          </tr>
                          <tr>
                            <td class="tg-s6z2">THÁNG 1</td>
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
                            <td class="tg-s6z2">THÁNG 2</td>
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
                            <td class="tg-s6z2">THÁNG 3</td>
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
                            <td class="tg-s6z2">THÁNG 4</td>
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
                            <td class="tg-s6z2">THÁNG 5</td>
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
                            <td class="tg-s6z2">THÁNG 6</td>
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
                            <td class="tg-s6z2">TỔNG CỘNG</td>
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
                <td class="tg-031e">
                    <b>
                        ( Kèm theo biên bản đối chiếu số liệu của MB,MN,MT với TT HĐĐHB )
                    </b>                    
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
                                TỔ TRƯỞNG TỔ <br />TTT ĐHB<br />(Ký,ghi rõ họ tên)
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


