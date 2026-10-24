<%@ Page Title="" MasterPageFile="~/Masters/ATFM.Master" Language="C#" AutoEventWireup="true" CodeBehind="CatHaThucTe.aspx.cs" Inherits="prjApplication.Report.ReportCatHaThucTe.CatHaThucTe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <script language="Javascript" type="text/javascript">
        function f_CallReports(valuePath) {
            var _fromDate = document.getElementById("ctl00_MainContent_txtBEGINDATE").value;
            var _toDate = document.getElementById("ctl00_MainContent_txtENDDATE").value;
            if ((_fromDate != '__/__/____') && (_toDate != '__/__/____')) {
                SubmitImage('../' + valuePath + '?FromDate=' + _fromDate + '&ToDate=' + _toDate + '', 1200, 580);
            }
            else {
                alert("Bạn chưa nhập khoảng thời gian !");
            }

        }
    </script>
    <style type="text/css">
        .tgTHSL {
            border-collapse: collapse;
            border-spacing: 0;
        }

            .tgTHSL td {
                font-family: Arial, sans-serif;
                font-size: 14px;
                padding: 10px 5px;
                border-style: solid;
                border-width: 1px;
                overflow: hidden;
                word-break: normal;
            }

            .tgTHSL th {
                font-family: Arial, sans-serif;
                font-size: 14px;
                font-weight: normal;
                padding: 10px 5px;
                border-style: solid;
                border-width: 1px;
                overflow: hidden;
                word-break: normal;
            }

            .tgTHSL .tg-baqh {
                text-align: center;
                vertical-align: top;
            }

            .tgTHSL .tg-hgcj {
                font-weight: bold;
                text-align: center;
            }

            .tgTHSL .tg-amwm {
                font-weight: bold;
                text-align: center;
                vertical-align: top;
            }

            .tgTHSL .tg-yw4l {
                vertical-align: top;
            }
    </style>
    <div class="alert alert-block alert-success">
        <i class="ace-icon fa fa-check green"></i>
        <strong class="green">BÁO CÁO TỔNG HỢP SỐ LIỆU BAY THỰC TẾ</strong>
    </div>
    <div class="classSearchHeader">
        <table class="tblHeader" style="border: none; text-align: right; width: 100%; color: white;">
            <tr>
                <td style="width: 20%; text-align: center;"></td>

                <td style="width: 60%; text-align: center;">
                    <asp:linkbutton runat="server" id="btnExportExel" cssclass="btn btn-primary" onclick="btnExcel_Click">
                        <span class="glyphicon glyphicon-download-alt"></span>
                        Export data into Excel
                    </asp:linkbutton>
                    <asp:button id="btnOption" runat="server" onclick="btnSelect_Click" text="Select" class="btn btn-primary" type="button"></asp:button>
                </td>
                <td style="width: 20%; text-align: center;"></td>
            </tr>
        </table>
    </div>
    <div class="hr"></div>
    <div class="row">
        <div class="col-sm-12">
            <div class="form-horizontal">
                <div class="form-group">
                    <div class="col-sm-2">
                        <div class="input-group">
                            <span class="lbl">SÂN BAY</span>
                            <asp:DropDownList ID="ddlAERO" CssClass="text-input" runat="server" Width="60%"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-sm-2">
                        <div class="lbl">FROM DATE</div>
                        <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                            style="width: 50%" onkeypress='return check_num(this,14,event)' type="text" />
                    </div>
                    <div class="col-sm-2">
                        <div class="lbl">TO DATE</div>
                        <input id="txtENDDATE" class="datepicker" autocomplete="off" runat="server"
                            style="width: 50%" onkeypress='return check_num(this,14,event)' type="text" />
                    </div>
                    <div class="col-sm-2">
                        <div>HOURBEGIN</div>
                        <input id="txtHOURBEGIN" type="text" runat="server" maxlength="4" data-number="true" style="width: 80px" />
                    </div>
                    <div class="col-sm-2">
                        <div>HOUR END</div>
                        <input id="txtHOUREND" type="text" runat="server" maxlength="4" data-number="true" style="width: 80px" />
                    </div>
                    <div class="col-sm-2">
                        <div>TYPE</div>
                        <label>
                            <select id="lblET" runat="server" style="width: 60px">
                                <option value="ATD">ATD</option>
                                <option value="ATA">ATA</option>
                            </select>
                        </label>
                    </div>
                </div>
            </div>
            
        </div>

    </div>
    <div class="hr"></div>
    <div>
        <asp:DataGrid runat="server" ID="grdBCTHTT" AutoGenerateColumns="false" Width="100%" CssClass="Grid">
            <ItemStyle CssClass="GridItem"></ItemStyle>
            <AlternatingItemStyle CssClass="GridAltItem" />
            <HeaderStyle CssClass="GridHeader"></HeaderStyle>
            <Columns>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        Khung Giờ
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container.DataItem, "KHUNGGIO") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        Tổng Số Cất Cánh
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("SFROM") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        Tổng Số Hạ Cánh
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Eval("sto") %>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn>
                    <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                    <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                    <HeaderTemplate>
                        Tổng Số Cất Hạ
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# Convert.ToInt32(Eval("SFROM").ToString()) + Convert.ToInt32(Eval("STO").ToString()) %>
                    </ItemTemplate>
                </asp:TemplateColumn>
            </Columns>
        </asp:DataGrid>
    </div>

    <script language="javascript" type="text/javascript">

        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });
        $('#<%= txtENDDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });
    </script>
    <script src="../../Scripts/CustomDynamic.js"></script>
</asp:Content>



