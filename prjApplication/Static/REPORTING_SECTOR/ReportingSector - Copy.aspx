<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="ReportingSector.aspx.cs" Inherits="prjApplication.Static.REPORTING_SECTOR.ReportingSector" %>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc2" %>
<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Import Namespace="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <span style="font-weight: bold;">+ ROUTE SECTOR LIST</span>

    <table border="0" cellspacing="1px" cellspacing="1px" style="width: 100%">
        <tr>
            <td style="text-align: right">
                <div class="classSearchHeader">
                    <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                        <tr>
                            <td style="width: 40%; text-align: left;">
                                <asp:TextBox ID="txtSearch_UserName" Width="80%" CssClass="inputtext" runat="server"
                                    placeholder="Search ROUTE_NAME"
                                    onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');"></asp:TextBox>
                                <asp:Button CausesValidation="false" runat="server" ID="linkSearch" CssClass="iconFind"
                                    Font-Bold="true" OnClick="linkSearch_Click" Text="" Width="40px" Height="38px"></asp:Button>
                            </td>
                            <td style="width: 60%; text-align: left;">
                               
                            <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                >
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
                                ROUTE_NAME
                            </HeaderTemplate>
                            <ItemTemplate>                                
                                <%# DataBinder.Eval(Container.DataItem, "ROUTE_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="20%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="20%"></ItemStyle>
                            <HeaderTemplate>
                                SECTOR_NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "SECTOR_NAME") %>
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
                                    ImageAlign="AbsMiddle" OnClientClick="return confirm('Do you want delete?');" ToolTip="Delete ReportingSector" CommandName="DeleteReportingSector"
                                    CommandArgument='<%# Eval("ROUTE_ID") %>' BorderStyle="None" Visible='<%#_Role.R_Del %>'></asp:ImageButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                Add Sector
                            </HeaderTemplate>
                            <ItemTemplate>
                                <a runat="server" data-toggle="tooltip" visible='<%#_Role.R_Edit %>' title="Add">
                                    <i class="ace-icon fa fa-pencil bigger-130" data-toggle="modal" data-target="#popupAddSector"
                                        onclick="ShowpopupAddSector(<%# Eval("ROUTE_ID") %>);">
                                    </i>
                                </a>
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
                <cc2:PhanTrang ID="PhanTrang1" runat="server" PageSize="10" NumberViewPage="7" OnPaging_IndexChange="PhanTrang1_Paging_IndexChange" />
            </td>
        </tr>
    </table>
   

    <div id="popupAddSector" class="modal fade" role="dialog" tabindex="-1" data-backdrop="false"
        aria-hidden="true">
        <div class="modal-dialog ">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" onclick="ClosePopup('popupAddSector');" class="close" data-dismiss="modal">
                        ×</button>
                    <h4 class="blue bigger">Add Sector</h4>
                </div>
                <div class="modal-body" id="showTableSector">
                <asp:GridView Style="border-collapse: collapse;" runat="server" ID="GridView2" AutoGenerateColumns="false"
                    Width="100%"
                    CssClass="Grid table-condensed">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="10%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="10%"></ItemStyle>
                            <HeaderTemplate>
                                MARK
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input runat="server" id="chkCheck" type="checkbox" onchange='chkCheck_OnchaneSector(this)' data-sectortid='<%# Eval("SECTOR_ID") %>' checked='<%#  Eval("MARK").ToString()=="1"?true:false %>' />                             
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="80%"></HeaderStyle>
                            <ItemStyle HorizontalAlign="left" Width="80%"></ItemStyle>
                            <HeaderTemplate>
                                SECTOR_NAME
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container.DataItem, "SECTOR_NAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                                                                                                                       
                    </Columns>
                </asp:GridView>
                </div>
                
            </div>
        </div>
    </div>
    <script>
        var IdSelect = 0;
        var eleSelect = '';
        var element;
        
        function ShowpopupAddSector(id) {
            IdSelect = id;
            GetArgWithPostBack(id + '_____GetAllSectorPoint', 'GetAllSectorPoint');
        }
        function DisplayResult(resulf, context) {
            if (resulf == '') return;
            if (context == 'GetAllSectorPoint') {
                document.getElementById('showTableSector').innerHTML = resulf;
            }           
            if (context == 'LoadDataGird') {                
                document.getElementById('<%= grdSource.ClientID%>').innerHTML = resulf;
            }            
            if (context == 'chkCheck_OnchaneSector') {
                if (resulf != 'true') {
                    $(element).prop('checked', !$(element).prop('checked'));
                    alert('Update error!');
                } else alert('Update sussess!');
                LoadDataGird();
            }
            
        }
        
       
        function chkCheck_OnchaneSector(ele) {

            if (IdSelect != 0) {
                element = ele;
                eleSelect = $(ele).prop('id');
                var p = document.getElementById(eleSelect).getAttribute('data-sectortid')
                var c = $(ele).prop('checked') ? '1' : '0'


                GetArgWithPostBack(IdSelect + '::::' + p + '::::' + c + '_____chkCheck_OnchaneSector', 'chkCheck_OnchaneSector');
            }
        }
        function LoadDataGird() {
            GetArgWithPostBack('LoadDataGird_____LoadDataGird', 'LoadDataGird');
        }
        
       
    </script>
</asp:Content>
