<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="Dashboard.aspx.cs" Inherits="prjApplication.Common.Dashboard" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <script type="text/javascript" src="../Scripts/jsapi.js"></script>
   <script type="text/javascript" src="../Scripts/uds_api_contents.js"></script>     
     <asp:Panel ID="pnlList" runat="server">
      

        <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid runat="server" ID="grdListUser" AutoGenerateColumns="false" DataKeyField="UserID"
                        OnEditCommand="grdListUser_EditCommand" Width="100%" OnItemDataBound="grdListUser_ItemDataBound"
                        CssClass="Grid">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="UserID">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="12%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="12%"></ItemStyle>
                                <HeaderTemplate>
                                    Tên tài khoản
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "UserName") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    Họ và tên
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "UserFullName") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                                <HeaderTemplate>
                                    Địa chỉ truy cập
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "ADDRESS") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Trạng thái Online
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnIsReporter" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "isonline").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái online" Width="25px" 
                                        BorderStyle="None" />
                                </ItemTemplate>
                            </asp:TemplateColumn>  
                                                      
                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>            
        </table>

    </asp:Panel>
    
</asp:Content>
