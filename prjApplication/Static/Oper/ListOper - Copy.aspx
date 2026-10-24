<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ListOper.aspx.cs" Inherits="prjApplication.Static.Oper.ListOper" EnableEventValidation="false" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ LIST OPER</span>

    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search OPER_ICAO ..."
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
            <td style="height: 10px"></td>
        </tr>
        <tr>
            <td>

                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="ID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_ICAO
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "OPER_ICAO") %>'
                                    Enabled='<%# _Role.R_Edit %>' CommandArgument='<%# Eval("ID") %>' CommandName="Edit"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_IATA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_IATA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_ADDRESS
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_ADDRESS") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                IS_DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<%# DataBinder.Eval(Container.DataItem,"IS_DOMESTIC") %>--%>
                                <asp:ImageButton ID="btnIS_DOMESTIC" runat="server" ImageUrl='<%#IsStatusGet(Eval("IS_DOMESTIC").ToString()==""? "0" : Eval("IS_DOMESTIC").ToString())%>'
                                    ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />
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
                                    CommandArgument='<%# Eval("ID") %>' OnClientClick="return confirm('Do you want delete?');" BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView Visible="false" Style="border-collapse: collapse;" runat="server" ID="GridView1" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="ID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_ICAO
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_ICAO") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_IATA
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_IATA") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                OPER_ADDRESS
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "OPER_ADDRESS") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                IS_DOMESTIC
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem,"IS_DOMESTIC") %>                                
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
