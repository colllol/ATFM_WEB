<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListCRAFT.aspx.cs" Inherits="prjApplication.Static.Craft.ListCRAFT" EnableEventValidation="false" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ LIST CRAFT</span>

    <table border="0" cellspacing="1" style="width: 100%">
        <tr>
            <td>
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1" style="width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search ICAO, IATA or craft name"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_btnSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="btnSearch" CssClass="iconFind"
                                    OnClick="btnSearch_Click" Text="" Width="40px" Height="38px" />
                            </td>
                            <td style="width: 60%; text-align: left;">
                                <asp:LinkButton runat="server" ID="btnAdd" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnAdd_Click" CausesValidation="false">
                                    <i class="ace-icon fa fa-plus bigger-120 white"></i> Add New
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="btnExport" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExport_Click" CausesValidation="false">
                                    <span class="glyphicon glyphicon-download-alt"></span> Export data into Excel
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
        <tr><td style="height: 10px"></td></tr>
        <tr>
            <td>
                <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%" CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:BoundField DataField="CRAFT_ICAO" HeaderText="CRAFT ICAO" />
                        <asp:BoundField DataField="CRAFT_IATA" HeaderText="CRAFT IATA" />
                        <asp:BoundField DataField="CRAFT_NAME" HeaderText="CRAFT NAME" />
                        <asp:TemplateField HeaderText="Edit">
                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="btn btn-xs btn-primary"
                                    Text="Edit" CommandName="EditByID" CommandArgument='<%# Eval("ID") %>'
                                    Enabled='<%# _Role.R_Edit %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete">
                            <ItemStyle HorizontalAlign="Center" Width="8%" />
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="btn btn-xs btn-danger"
                                    Text="Delete" CommandName="DeleteByID" CommandArgument='<%# Eval("ID") %>'
                                    OnClientClick="return confirm('Do you want to delete this craft?');"
                                    Enabled='<%# _Role.R_Del %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView runat="server" ID="grdExport" AutoGenerateColumns="false" Visible="false">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" />
                        <asp:BoundField DataField="CRAFT_ICAO" HeaderText="CRAFT ICAO" />
                        <asp:BoundField DataField="CRAFT_IATA" HeaderText="CRAFT IATA" />
                        <asp:BoundField DataField="CRAFT_NAME" HeaderText="CRAFT NAME" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr><td style="height: 10px"></td></tr>
        <tr>
            <td style="text-align: right" class="pageNavTotal">
                <cc2:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="7" PageSize="10" />
            </td>
        </tr>
    </table>
</asp:Content>
