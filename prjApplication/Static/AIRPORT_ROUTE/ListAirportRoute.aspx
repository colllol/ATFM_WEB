<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    EnableEventValidation="false"
    CodeBehind="ListAirportRoute.aspx.cs" Inherits="prjApplication.Static.AIRPORT_ROUTE.ListAirportRoute" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td></td>
            <td style="text-align: left">
                <span class="TitlePanel">+ AIRPORT ROUTE LIST</span>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td style="text-align: center">
                <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
                    <tr>
                        <td style="text-align: right">
                            <div class="classSearchHeader">
                                <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                                    <tr>
                                        <td style="width: 40%; text-align: left;">
                                            <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                                placeholder="Search ..."
                                                onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                            <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                                Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px">
                                            </asp:Button>
                                        </td>

                                        <td style="width: 60%; text-align: left;">
                                            <asp:LinkButton runat="server" ID="btnAddObject" CssClass="btn btn-sm btn-primary btn-bold"
                                                OnClick="btnAddObject_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
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

                            <asp:DataGrid runat="server" ID="grdAirportRoute" AutoGenerateColumns="false" DataKeyField="ID"
                                OnEditCommand="grdAirportRoute_EditCommand" OnDeleteCommand="grdAirportRoute_DeleteCommand"
                                Width="100%"
                                CssClass="Grid">
                                <ItemStyle CssClass="GridItem"></ItemStyle>
                                <AlternatingItemStyle CssClass="GridAltItem" />
                                <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                <Columns>
                                    <asp:BoundColumn Visible="False" DataField="ID">
                                        <HeaderStyle Width="1%"></HeaderStyle>
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                        <HeaderTemplate>
                                            ROUTE
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "ROUTE") %>'
                                                Visible='<%#_Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                        <HeaderTemplate>
                                            FROM_AIRP
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "FROM_AIRP") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                        <HeaderTemplate>
                                            TO_AIRP
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "TO_AIRP") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                        <HeaderTemplate>
                                            INTERNATIONAL
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "INTERNATIONAL") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                        <HeaderTemplate>
                                            DOMESTIC
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "DOMESTIC") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                        <HeaderTemplate>
                                            IS_OVERSEA
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "IS_OVERSEA") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>                                 
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                        <HeaderTemplate>
                                            IS_DOMESTIC
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "IS_DOMESTIC") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                        <HeaderTemplate>
                                            SUMMARY
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# DataBinder.Eval(Container.DataItem, "SUMMARY") %>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                    <asp:TemplateColumn>
                                        <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                        <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                        <HeaderTemplate>
                                            Delete
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                                ImageAlign="AbsMiddle" ToolTip="Delete Country" CommandName="Delete"
                                                CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Do you want delete?');"
                                                BorderStyle="None" Visible='<%#_Role.R_Del %>'></asp:ImageButton>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>

                        </td>
                    </tr>
                    <tr>
                        <td style="height: 10px"></td>
                    </tr>
                    <tr>
                        <td style="text-align: right" class="pageNavTotal">
                            <cc2:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="20" />
                        </td>
                    </tr>
                </table>
            </td>
            <td></td>
        </tr>
        <tr>
            <td></td>
            <td></td>
            <td></td>
        </tr>
    </table>

</asp:Content>
