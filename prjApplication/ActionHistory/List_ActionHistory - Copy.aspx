    <%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="List_ActionHistory.aspx.cs" Inherits="prjApplication.ActionHistory.List_ActionHistory" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />
    <script src="../Scripts/DateTimeJQ/jquery-1.8.3.js"></script>
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>

    <span style="font-weight: bold;">+ NHẬT KÝ THAO TÁC HỆ THỐNG</span>
    <%--<link href="../../Style/Style_List.css" rel="stylesheet" />--%>
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <table border="0" cellspacing="1px" cellspacing="1px"  style="width: 100%">
        <%--<tr>
            <td style="width: 10%; text-align: left;" class="Titlelbl">Tên truy cập:
            </td>
            <td style="width: 25%; text-align: left">
                <asp:DropDownList ID="ddlTenTruyCap" TabIndex="3" runat="server" Width="100%" CssClass="inputtext">
                </asp:DropDownList>
            </td>
            <td style="width: 10%; text-align: right" class="Titlelbl">Từ ngày(<span class="req_Field">*</span>):
            </td>
            <td style="width: 15%; text-align: left">
                <input id="txtTungay" runat="server" class="datepicker text-input" autocomplete="off"
                    style="width: 100%" onkeypress='return check_num(this,14,event)' />
            </td>
            <td style="width: 10%; text-align: right" class="Titlelbl">Đến ngày(<span class="req_Field">*</span>):
            </td>
            <td style="width: 15%; text-align: left">
                <input id="txtDenngay" runat="server" class="datepicker text-input" autocomplete="off"
                    style="width: 100%" onkeypress='return check_num(this,14,event)' />
            </td>          
        </tr>--%>

        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 5%; text-align: left;" class="Titlelbl">Tên truy cập:
                            </td>
                            <td style="width: 10%; text-align: left">
                                <asp:DropDownList ID="ddlTenTruyCap" CssClass="wid_40" Width="100%" runat="server"></asp:DropDownList>                                          
                             </td>
                            <%--<td style="width: 15%; text-align: left">
                                <asp:DropDownList ID="ddlTenTruyCap" TabIndex="3" runat="server" Width="100%" CssClass="inputtext">
                                </asp:DropDownList>
                            </td>--%>
                            <%--<td style="width: 10%; text-align: right" class="Titlelbl">Từ ngày(<span class="req_Field">*</span>):
                            </td>
                            <td style="width: 15%; text-align: left">
                                <input id="txtTungay" runat="server" class="datepicker text-input" autocomplete="off"
                                    style="width: 100%" onkeypress='return check_num(this,14,event)' />
                            </td>
                            <td style="width: 10%; text-align: right" class="titlelbl">đến ngày(<span class="req_field">*</span>):
                            </td>   
                            <td style="width: 15%; text-align: left">
                                <input id="txtDenngay" runat="server" class="datepicker text-input" autocomplete="off"
                                    style="width: 100%" onkeypress='return check_num(this,14,event)' />
                            </td> --%>    
                            <td style="width: 40%; text-align: left;">
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind" 
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                                  <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                                </asp:LinkButton>
                            </td>                                              
                        </tr>
                    </table>
                </div>
            </td>
        </tr>

        <tr>
            <td>
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false"
                    OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Tên người dùng
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Eval("FULLNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Nhật ký thao tác
                            </HeaderTemplate>
                            <ItemTemplate>                               
                                    <%# DataBinder.Eval(Container.DataItem, "ACTIONSCODE") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                Thời gian thực hiện
                            </HeaderTemplate>
                            <ItemTemplate>                               
                                    <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "DATEMODIFY")).ToString("dd/MM/yyyy HH:mm:ss") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                HostIP
                            </HeaderTemplate>
                            <ItemTemplate>                               
                                    <%# DataBinder.Eval(Container.DataItem, "HostIP") %>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                    </Columns>
                </asp:GridView>

            </td>
        </tr>
        <tr>
            <td style="height: 10px"></td>
        </tr>
        <tr>
            <td style="text-align: right" class="pageNavTotal">
                <cc1:PhanTrang ID="PhanTrang1" runat="server" PageSize="30" NumberViewPage="7" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange"/>
            </td>
        </tr>               
    </table>
 
    <script language="javascript" type="text/javascript">

        <%--$('#<%= txtTungay.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });

        $('#<%= txtDenngay.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });--%>

    </script>
</asp:Content>
