<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="Log_Export.aspx.cs" Inherits="prjApplication.Common.Log_Export" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript" src="../Scripts/jsapi.js"></script>
   <script type="text/javascript" src="../Scripts/uds_api_contents.js"></script>     
     <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ LOG LIST</span>

         <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>
                                <td style="width: 10%; text-align: left;" class="Titlelbl">Name:</td>
                                <td style="width: 20%; text-align: left;">
                                    <asp:TextBox ID="txtName" runat="server" CssClass="text-input" Width="100%" MaxLength="100" placeholder="Tìm theo Name" />
                                </td>
                                <td style="width: 10%; text-align: left;" class="Titlelbl">SQLCODE:</td>
                                <td style="width: 15%; text-align: left;">
                                    <asp:TextBox ID="txtSqlCode" runat="server" CssClass="text-input" Width="100%" MaxLength="50" placeholder="Tìm theo SQLCODE" />
                                </td>
                                <td style="width: 8%; text-align: left;" class="Titlelbl">ERR:</td>
                                <td style="width: 22%; text-align: left;">
                                    <asp:TextBox ID="txtErr" runat="server" CssClass="text-input" Width="100%" MaxLength="500" placeholder="Tìm theo ERR" />
                                </td>
                                <td style="width: 15%; text-align: left;">
                                    <asp:LinkButton runat="server" ID="btnSearch" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="btnSearch_Click" CausesValidation="false">
                                        <span class="glyphicon glyphicon-search"></span> Tìm kiếm
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" ID="btnAddMenu" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="btnAddMenu_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Delete
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                                </asp:LinkButton>
                                    <asp:Literal ID="lit" runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid runat="server" ID="grdListUser" AutoGenerateColumns="false" 
                        Width="100%" 
                        CssClass="Grid">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>                            
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Name
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "Name") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    SQLCODE
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "SQLCODE") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    ERR
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "ERR") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>                                                    
                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>            
        </table>

    </asp:Panel>
    
</asp:Content>
