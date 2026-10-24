<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ATMMN.aspx.cs" Inherits="prjApplication.ATM.ATMMN" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-weight: bold;">+ ATM LIST FILE</span>
     <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />    
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
     <style type="text/css">
        .w3-example {
        background-color: #f1f1f1;
       
    }
       
   </style>
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td>
                                Select Date
                                 <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" style="width:90px;" /> 
                            </td>

                            <td style="width: 60%; text-align: left;">
                                 <asp:Button ID="btnCapNhat" runat="server" OnClick="Button1_Click" Text="Search" />
                               

                                

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
                 <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
                 <tr>
                      <td style="width:20%" valign="top">
                        Chọn năm:<asp:DropDownList runat="server" ID="cboNam" style="width:70px;" >
                        </asp:DropDownList>
                          <br />
                          <asp:TreeView ID="TreeView1" runat="server" ImageSet="XPFileExplorer" NodeIndent="15"  
                              onselectednodechanged="TreeViewTabs_SelectedNodeChanged"                           
                            style="float:left;padding-left:40px;"   >
                        <HoverNodeStyle Font-Underline="True" ForeColor="#6666AA" />
                        <NodeStyle Font-Names="Tahoma" Font-Size="10pt" ForeColor="Black" HorizontalPadding="2px"
                            NodeSpacing="0px" VerticalPadding="2px"></NodeStyle>
                        <ParentNodeStyle Font-Bold="False" />
                        <SelectedNodeStyle BackColor="#B5B5B5" Font-Underline="False" HorizontalPadding="0px"
                           ForeColor="Red"  VerticalPadding="0px" />
                    </asp:TreeView>                            

                     </td>
                     <td style="width:80%" valign="top">
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
                                <a href='<%# System.Configuration.ConfigurationManager.AppSettings["ATMPATH"] %>MN/<%# Eval("Dateupdate").ToString().Replace("_","/") %>/<%# Eval("Name") %>' title="Download Document">
                                <img style='height: 40px;'src='<%# Global.ApplicationPath %>/images/Icons/Download.gif'/></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                                                                                                       
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>
                               
                                
                               <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/Icons/delete.gif"
                                    ImageAlign="AbsMiddle" ToolTip="Download" CommandName="DeleteByID" Enabled='<%#_Role.R_Edit %>'
                                    CommandArgument='<%# Eval("Name") %>' BorderStyle="None"></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView></td>
                 </tr>
                 </table>                              
            </td>
        </tr>
        <tr>
            <td style="height: 10px"></td>
        </tr>
        
    </table>
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