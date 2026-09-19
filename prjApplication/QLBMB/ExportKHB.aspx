<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="ExportKHB.aspx.cs" Inherits="prjApplication.QLBMB.ExportKHB" %>

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
     <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ EXPORT</span>

         <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>
                                <td style="width: 40%; text-align: left;">                                    
                                </td>

                                <td style="width: 60%; text-align: left;">

                                    <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" style="width:90px;" /> 
                                    <asp:Button id="btnOption" runat="server" OnClick="btnSelect_Click" Text="Select" class="btn btn-primary btn-sm" type="button"></asp:Button>
                                  
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
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    FPPID
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "FPPID") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="8%"></ItemStyle>
                                <HeaderTemplate>
                                    ValidFrom
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "ValidFrom") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                           <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="8%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="8%"></ItemStyle>
                                <HeaderTemplate>
                                    ValidTo
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "ValidTo") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>                                                    
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    CallSign
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "CallSign") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>  
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    DEP
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "DEP") %>
                                </ItemTemplate>
                            </asp:TemplateColumn> 

                            
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    EOBT
                                </HeaderTemplate>
                                <ItemTemplate>
                                   &nbsp;<%# DataBinder.Eval(Container.DataItem, "EOBT").ToString() %>
                                </ItemTemplate>
                            </asp:TemplateColumn> 
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                                <HeaderTemplate>
                                    DES
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "DES") %>
                                </ItemTemplate>
                            </asp:TemplateColumn> 
                             
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    FlightDate
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# DataBinder.Eval(Container.DataItem, "FlightDate","{0:dd/MM/yyyy}") %>
                                </ItemTemplate>
                            </asp:TemplateColumn> 
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Route 1
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# CutRoute(DataBinder.Eval(Container.DataItem, "ROUTEFULL").ToString(),0) %>
                                </ItemTemplate>
                            </asp:TemplateColumn> 
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Route 2
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# CutRoute(DataBinder.Eval(Container.DataItem, "ROUTEFULL").ToString(),1) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                             <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Route 3
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# CutRoute(DataBinder.Eval(Container.DataItem, "ROUTEFULL").ToString(),2) %>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                                <HeaderTemplate>
                                    Route 4
                                </HeaderTemplate>
                                <ItemTemplate>
                                   <%# CutRoute(DataBinder.Eval(Container.DataItem, "ROUTEFULL").ToString(),3).Replace("/"," ") %>
                                </ItemTemplate>
                            </asp:TemplateColumn>                           
                        </Columns>
                    </asp:DataGrid>

                </td>
            </tr>            
        </table>

    </asp:Panel>
      <script language="javascript" type="text/javascript">

        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });       
    </script>
</asp:Content>
