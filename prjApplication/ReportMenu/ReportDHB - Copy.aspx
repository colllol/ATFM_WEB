<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ReportDHB.aspx.cs" Inherits="prjApplication.ReportMenu.ReportDHB" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <script language="Javascript" type="text/javascript">
        function f_CallReports(valuePath) {
            var _operid = ""; var _craptid = "";
            var _cbooperid = document.getElementById('ctl00_MainContent_ddlOper');
            _operid = _cbooperid.options[_cbooperid.selectedIndex].value;

            var _cbocraptid = document.getElementById('ctl00_MainContent_ddlCraft');
            _craptid = _cbocraptid.options[_cbocraptid.selectedIndex].value;
           
            var _fromDate = document.getElementById("ctl00_MainContent_txtBEGINDATE").value;
            var _toDate = document.getElementById("ctl00_MainContent_txtENDDATE").value;
            if ((_fromDate != '__/__/____') && (_toDate != '__/__/____')) {
                SubmitImage('../' + valuePath + '?Oper=' + _operid + '&Craft=' + _craptid + '&FromDate=' + _fromDate + '&ToDate=' + _toDate + '', 1200, 580);
            }
            else {
                alert("Bạn chưa nhập khoảng thời gian !");
            }

        }
    </script>
    <div class="alert alert-block alert-success">
        <i class="ace-icon fa fa-check green"></i>
        <strong class="green">BÁO CÁO ĐIỀU HÀNH BAY									
        </strong>
    </div>
    <div class="row">

        <div class="col-sm-3">

            <div class="widget-box">

                <div class="widget-body">
                    <div class="widget-main">
                        <div class="input-group">
                            <span class="lbl">BEGIN DATE</span>
                            <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                style="width: 80%" onkeypress='return check_num(this,14,event)' type="text" />

                        </div>
                        <div class="hr"></div>
                        <div class="input-group">
                            <span class="lbl">END DATE</span>
                            <input id="txtENDDATE" class="datepicker" autocomplete="off" runat="server"
                                style="width: 80%" onkeypress='return check_num(this,14,event)' type="text" />

                        </div>
                        <div class="hr"></div>
                         <div class="input-group">
                            <span class="lbl">OPER</span>
                          <asp:DropDownList ID="ddlOper" CssClass="text-input" runat="server"
                                                            Width="80%">
                                                 </asp:DropDownList>

                        </div>
                        <div class="hr"></div>
                         <div class="input-group">
                            <span class="lbl">CRAFT</span>
                          <asp:DropDownList ID="ddlCraft" CssClass="text-input" runat="server"
                                                            Width="80%">
                                                 </asp:DropDownList>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-9">
            <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
                OnRowDataBound="grdSource_RowDataBound"
                Width="100%"
                CssClass="Grid table table-striped table-bordered">
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <%# Container.DataItemIndex +1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="90%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="left" Width="90%"></ItemStyle>
                        <HeaderTemplate>
                            REPORT NAME
                        </HeaderTemplate>
                        <ItemTemplate>
                            <a href="#" class="linkGridForm" onclick="f_CallReports('<%# Eval("ADDRESS_FILE") %>');"><%# Eval("NAME_REPORT") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
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
</asp:Content>
