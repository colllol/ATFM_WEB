<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM_New.Master" CodeBehind="ListAirportRoute.aspx.cs" Inherits="prjApplication.Static.AirportRoute.ListAirportRoute" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ AIRPORT ROUTE LIST</span>
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search ROUTE"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>

                            <td style="width: 60%; text-align: left;">
                                <asp:LinkButton runat="server" ID="btnAddObject" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnAddObject_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Add New
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
            <td>               
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                               ROUTE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "ROUTE") %>'
                                    Enabled='<%# _Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                FROM_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<%# Eval( "FROM_AIRP") %>--%>
                                <asp:LinkButton ID="btnEdit1" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "FROM_AIRP") %>'
                                    Enabled='<%# _Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                TO_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "TO_AIRP") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                 IS_OVERSEA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnIS_OVERSEA" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "IS_OVERSEA").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "DOMESTIC") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                INTERNATIONAL
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "INTERNATIONAL") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                IS_DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnIS_DOMESTIC" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "IS_DOMESTIC").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                SUMMARY
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "SUMMARY") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" ToolTip="Delete Country" CommandName="DeleteRoute"
                                    CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Do you want delete?');" BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="GridView1" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                               ROUTE
                            </HeaderTemplate>
                            <ItemTemplate>                                
                                <%# Eval( "ROUTE") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                FROM_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "FROM_AIRP") %>                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                TO_AIRP
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "TO_AIRP") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                 IS_OVERSEA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "IS_OVERSEA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "DOMESTIC") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                INTERNATIONAL
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "INTERNATIONAL") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                IS_DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "IS_DOMESTIC") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                SUMMARY
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "SUMMARY") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>      
        <tr>
            <td style="text-align: right" class="pageNavTotal">
                <cc2:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
            </td>
        </tr>
    </table>
</asp:Content>
