<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="SendMessAirport.aspx.cs" Inherits="prjApplication.Common.SendMessAirport" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />    
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
     <style type="text/css">
        .w3-example {
        background-color: #f1f1f1;
       
    }
       
   </style>
   <script type="text/javascript" src="../Scripts/jsapi.js"></script>
   <script type="text/javascript" src="../Scripts/uds_api_contents.js"></script>     
     <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ MESS LIST AIRPORT</span>

         <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>
                                <td style="width: 20%; text-align: left;">  
                                   
                                     <asp:TextBox ID="txtSearch_UserName" Width="90%" CssClass="inputtext" runat="server"
                                    placeholder="Search Content"
                                   ></asp:TextBox>                                  
                                </td>
                                <td style="width: 20%; text-align: left;">  
                                    <asp:TextBox ID="txtd" Width="90%" CssClass="inputtext" runat="server"
                                    placeholder="Search date"
                                   ></asp:TextBox> 
                                                                      
                                </td>
                                <td style="width: 60%; text-align: left;">
                                   
                                    <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Search
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
                                <HeaderStyle HorizontalAlign="Center" Width="8%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="8%"></ItemStyle>
                                <HeaderTemplate>
                                    FlightDate
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# Convert.ToDateTime(DataBinder.Eval(Container.DataItem, "FlightDate")).ToString("dd-mm-yyyy") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    Part_No
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "Part_No") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Mess_type
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "Mess_type") %>
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
<script language="javascript" type="text/javascript">

        $('#<%= txtd.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });       
    </script>
    </asp:Panel>
    
</asp:Content>
