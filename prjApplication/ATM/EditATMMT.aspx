<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="EditATMMT.aspx.cs" Inherits="prjApplication.ATM.EditATMMT" %>
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
            <td >
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: center; width: 50%">
                        <tr>
                            <td style="text-align: right; width: 30%;"  >Select Date&nbsp;</td>
                           <td style="text-align: left; " >                                
                                 <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" style="width:90px;" />                                                                                                                          
                           </td>   
                            <td>
                                 <asp:Button ID="btnNew" runat="server" OnClick="Button2_Click" Text="New" Visible="false" />
                                <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" onchange="this.form.submit()"    webkitdirectory directory multiple/>                                  
                            </td>                                                    
                        </tr>                        
                        
                        <tr>
                            <td><asp:Literal ID="lit" runat="server"></asp:Literal></td>
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
                       <td style="width:50%" valign="top">
                          <span style="font-weight: bold;">+ LIST FILE LOCAL</span>   
                            <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdS" AutoGenerateColumns="false" DataKeyField="SlNo"                    
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="SlNo">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                SlNo
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "SlNo") %>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="90%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="90%"></ItemStyle>
                            <HeaderTemplate>
                                NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FileName") %>
                            </ItemTemplate>
                        </asp:TemplateField>                                                                                                                   
                       
                    </Columns>
                </asp:GridView></td>
                        <td style="width:50%" valign="top">
                            <span style="font-weight: bold;">+ LIST FILE SERVER</span> 
                            <asp:GridView Style="border-collapse: collapse;" runat="server" ID="grdSource" AutoGenerateColumns="false" DataKeyField="SlNo"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="SlNo">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="5%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="5%"></ItemStyle>
                            <HeaderTemplate>
                                SlNo
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "SlNo") %>
                            </ItemTemplate>
                        </asp:TemplateField>      
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="90%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="90%"></ItemStyle>
                            <HeaderTemplate>
                                NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "FileName") %>
                            </ItemTemplate>
                        </asp:TemplateField>                                                                                                                   
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>                                                               
                               <asp:ImageButton ID="btnDelete" Width="15px" runat="server" ImageUrl="~/images/Trash-Delete-icon.png"
                                    ImageAlign="AbsMiddle" ToolTip="delete" CommandName="DeleteByID"
                                    OnClientClick="aspnetForm.target ='_blank';" CommandArgument='<%# Eval("FileName") %>' BorderStyle="None"></asp:ImageButton>
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
        <tr>                            
                             <td style="text-align: center; width: 100%">
                                <asp:Button ID="btnCapNhat" runat="server" OnClick="Button1_Click" Text="Upload" />
                                
                            </td>
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

        var phanCachArg = '<%= _phanCachArg%>';
        function Upload() {           
            // GetArgWithPostBack(phanCachArg + 'Upload', 'Upload');
            this.form.submit();
        }
        function DisplayResult(resulf, context) {
            if (context == 'Upload') {
                alert(resulf);
            }            
        }
    </script>
</asp:Content>