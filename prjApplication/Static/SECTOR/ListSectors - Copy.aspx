<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListSectors.aspx.cs" Inherits="prjApplication.Static.SECTOR.ListSectors" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ SECTORS LIST</span>

    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search Address"
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

                                <%--<asp:LinkButton runat="server" ID="btnExportPdf" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnPDF_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Pdf
                                </asp:LinkButton>--%>

                                <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click" Visible="false">
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

                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="SECTOR_ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="SECTOR_ID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                SECTOR NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "SECTOR_NAME") %>'
                                    Enabled='<%# _Role.R_Edit %>' CommandArgument='<%# Eval("SECTOR_ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                       
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="35%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="35%"></ItemStyle>
                            <HeaderTemplate>
                                DESCRIPTION
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "NOTE") %>
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
                                    ImageAlign="AbsMiddle" ToolTip="Delete Country" CommandName="DeleteByID"
                                    CommandArgument='<%# Eval("SECTOR_ID") %>' OnClientClick="return confirm('Do you want delete?');" BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
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