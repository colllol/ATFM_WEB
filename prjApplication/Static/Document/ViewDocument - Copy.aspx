<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ViewDocument.aspx.cs" Inherits="prjApplication.Static.Document.ViewDocument" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-weight: bold;">+ Document LIST</span>
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search Name"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>

                            <td style="width: 60%; text-align: left;">

                               

                                

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
                            <HeaderStyle HorizontalAlign="Left" Width="90%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="90%"></ItemStyle>
                            <HeaderTemplate>
                                NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                         
                                           
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                View
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a href='<%# Global.ApplicationPath %>/upload<%# Eval("Path") %>' title="Download Document">
                                    <img src="<%# Global.ApplicationPath %>/images/button/Saves.gif" />
                                </a>
                                <%--<asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/zoom.gif"
                                    ImageAlign="AbsMiddle"  ToolTip="Download Document"
                                     BorderStyle="None" Visible='<%#_Role.R_Del %>'></asp:ImageButton>--%>
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
                <cc1:CustomPaging ID="CustomPaging1" runat="server" NumberViewPage="5" PageSize="10" />
            </td>
        </tr>
    </table>

</asp:Content>