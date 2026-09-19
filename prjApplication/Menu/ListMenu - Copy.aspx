<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ListMenu.aspx.cs" Inherits="prjApplication.Menu.ListMenu" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="cp_ListMenu" ContentPlaceHolderID="MainContent" runat="server">

    <span style="font-weight: bold;">+ MENU LIST</span>

    <table cellpadding="0" cellspacing="0" border="0" width="100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader" style="float: left; width: 100%;">
                    <table style="float: left; width: 100%;">
                        <tr>

                            <td style="width: 40%" align="left">
                                <asp:TextBox ID="txtSearch_Menu" Width="80%" CssClass="inputtext" runat="server"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');" placeholder="Tên chức năng"></asp:TextBox><asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                        Font-Bold="true" Text="" Width="40px" Height="38px" OnClick="linkSearch_Click"></asp:Button>
                            </td>

                            <td style="width: 60%; text-align: left;">

                                <asp:LinkButton runat="server" ID="btnAddMenu" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnAddMenu_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="Excel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Export Excel
                                </asp:LinkButton>
                            </td>
                            <td style="width: 60%; text-align: left;">

                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 4px" align="left"></td>
        </tr>
        <tr>
            <td align="left">
                <asp:DataGrid runat="server" ID="gdListMenu" AutoGenerateColumns="false" DataKeyField="ID"
                    Width="100%" CssClass="Grid" OnEditCommand="gdListMenu_EditCommand" OnItemDataBound="gdListMenu_ItemDataBound">
                    <ItemStyle CssClass="GridItem"></ItemStyle>
                    <AlternatingItemStyle CssClass="GridAltItem" />
                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                    <Columns>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                Chức năng hệ thống
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkEdit" Text='<%# DataBinder.Eval(Container.DataItem, "MenuName")%>'
                                    ToolTip="<%$ Resources:Strings, SYSTEM_GridTiitleEdit %>" CommandName="Edit"
                                    Enabled='<%#_Role.R_Edit%>' CommandArgument="Edit">
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="Literal23" runat="server" Text="<%$ Resources:Strings, SYSTEM_GridTiitleDuongDan %>"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"MenuURL")%>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                        <asp:TemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Strings, SYSTEM_GridTiitleDelete %>"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                              <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" ToolTip="<%$ Resources:Strings, SYSTEM_GridTiitleDelete %>"
                                    CommandName="Edit" Visible='<%#_Role.R_Del%>' CommandArgument="Delete" BorderStyle="None"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateColumn>
                    </Columns>
                </asp:DataGrid>
                <asp:GridView Visible="false" Style="border-collapse: collapse;" runat="server"
                         ID="GridView" AutoGenerateColumns="false" DataKeyField="UserID"
                   
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                MenuName
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "MenuName") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                MenuURL
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "MenuURL") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="text-align: right" class="pageNavTotal">
                <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
            </td>
        </tr>
    </table>

</asp:Content>
