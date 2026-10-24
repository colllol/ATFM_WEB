<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    EnableEventValidation="false"
    CodeBehind="ListAero.aspx.cs" Inherits="prjApplication.Static.AERO.ListAero" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        input, textarea {
            text-transform: uppercase;
        }
    </style>
    <link href="../../Style/Style_List.css" rel="stylesheet" />

    <span style="font-weight: bold;">+ APT LIST</span>
    <table border="0" style="width: 100%" class="table table-condensed">
        <tr>
            <td style="text-align: right;">
                <div class="classSearchHeader">
                    <table border="0" style="text-align: right; width: 100%; color: white;">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search CODE"
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
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                APT
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# Eval( "AE_CODE") %>'
                                    Enabled='<%#_Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                IATA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_IATA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                ZONE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_ZONE") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                INTER
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnINTER" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "AE_INTER").ToString())%>'
                                    ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                ISOVERSEA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnISOVERSEA" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "AD_ISOVERSEA").ToString())%>'
                                    ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
                                <%--<%# Eval( "AD_ISOVERSEA") %>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                SUMMER TIME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "SUMMER_TIME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                WINTER TIME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "WINTER_TIME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" ToolTip="Delete Country" CommandName="DeleteAero"
                                    CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Do you want delete?');"
                                    BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
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
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                AE CODE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_CODE") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                Country Name
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "CTR_ENAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                ZONE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_ZONE") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                INTER
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_INTER") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                IATA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AE_IATA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                ISOVERSEA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "AD_ISOVERSEA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                SUMMER TIME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "SUMMER_TIME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                WINTER TIME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval( "WINTER_TIME") %>
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
                <cc2:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
            </td>

        </tr>
    </table>

</asp:Content>
