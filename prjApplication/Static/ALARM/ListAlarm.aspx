<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="ListAlarm.aspx.cs" Inherits="prjApplication.Static.ALARM.ListAlarm" EnableEventValidation="false" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <span style="font-weight: bold;">+ ALARM LIST</span>
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search Alarm Name"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>

                            <td style="width: 60%; text-align: left;">

                                <asp:LinkButton runat="server" ID="btnAddAlarm" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnAddCountry_Click" OnClientClick="return checkValidCustomMinlenght(formValidate1);" CausesValidation="false">
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
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                EVENT NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEditUser" runat="server" CssClass="linkGridForm" Text='<%# DataBinder.Eval(Container.DataItem, "EVENT_NAME") %>'
                                    ToolTip="Edit Country" Enabled='<%#_Role.R_Edit %>' CommandName="Edit"
                                    CommandArgument='<%# Eval("ID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                EVENT DATE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("EVENT_DATE"))!=DateTime.MinValue?Convert.ToDateTime(Eval("EVENT_DATE")).ToString("dd/MM/yyyy"):"" %>
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                BEGIN VALID
                            </HeaderTemplate>
                            <ItemTemplate> 
                                <%# Convert.ToDateTime(Eval("BEGIN_VALID"))!=DateTime.MinValue?Convert.ToDateTime(Eval("BEGIN_VALID")).ToString("dd/MM/yyyy"):"" %>                              
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                END VALID
                            </HeaderTemplate>
                            <ItemTemplate>  
                                 <%# Convert.ToDateTime(Eval("END_VALID"))!=DateTime.MinValue?Convert.ToDateTime(Eval("END_VALID")).ToString("dd/MM/yyyy"):"" %>                                                          
                                 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                STATUS
                            </HeaderTemplate>
                            <ItemTemplate> 
                                <asp:ImageButton ID="btnIsSTATUS" runat="server" ImageUrl='<%#IsStatusGet(DataBinder.Eval(Container.DataItem, "STATUS").ToString())%>'
                                        ImageAlign="AbsMiddle" ToolTip="Trạng thái" Width="25px" BorderStyle="None" />                              
                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Delete
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                    ImageAlign="AbsMiddle" OnClientClick="return confirm('Do you want delete?');" ToolTip="Delete Country" CommandName="DeleteByID"
                                    CommandArgument='<%# Eval("ID") %>' BorderStyle="None" Enabled='<%#_Role.R_Del %>'></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="GridView1" AutoGenerateColumns="false" DataKeyField="ID"
                    OnRowCommand="grdSource_RowCommand" OnRowDataBound="grdSource_RowDataBound"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:BoundField Visible="False" DataField="ID">
                            <HeaderStyle Width="1%"></HeaderStyle>
                        </asp:BoundField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="25%"></ItemStyle>
                            <HeaderTemplate>
                                EVENT NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Eval("EVENT_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                EVENT DATE
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("EVENT_DATE"))!=DateTime.MinValue?Convert.ToDateTime(Eval("EVENT_DATE")).ToString("dd/MM/yyyy"):"" %>
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                BEGIN VALID
                            </HeaderTemplate>
                            <ItemTemplate> 
                                <%# Convert.ToDateTime(Eval("BEGIN_VALID"))!=DateTime.MinValue?Convert.ToDateTime(Eval("BEGIN_VALID")).ToString("dd/MM/yyyy"):"" %>                              
                               
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="12%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                END VALID
                            </HeaderTemplate>
                            <ItemTemplate>  
                                 <%# Convert.ToDateTime(Eval("END_VALID"))!=DateTime.MinValue?Convert.ToDateTime(Eval("END_VALID")).ToString("dd/MM/yyyy"):"" %>                                                                                          
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                STATUS
                            </HeaderTemplate>
                            <ItemTemplate>
                                  <%# Eval("STATUS") %>                                                            
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
