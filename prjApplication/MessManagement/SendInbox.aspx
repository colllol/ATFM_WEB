<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="SendInbox.aspx.cs" Inherits="prjApplication.Common.SendInbox" %>

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
                                <td style="width: 40%; text-align: left;">                                    
                                </td>

                                <td style="width: 60%; text-align: left;">
                                   
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
                                    FlightDate
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "FlightDate") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           
                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="75%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="75%"></ItemStyle>
                                <HeaderTemplate>
                                    Content
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "Content") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>                                                    
                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>            
        </table>

    </asp:Panel>
    
</asp:Content>
