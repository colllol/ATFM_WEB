<%@ Page Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" EnableEventValidation="false"
    CodeBehind="ListGroups.aspx.cs" Inherits="prjApplication.Groups.ListGroups" %>

<%--<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>--%>
<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Import Namespace="prjComponents" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel ID="pnlList" runat="server">
        <span style="font-weight: bold;">+ GROUP LIST</span>

        <table border="0" cellspacing="1" cellpadding="1" style="width: 100%">
            <tr>
                <td style="text-align: right">
                    <div class="classSearchHeader" style="width: 100%">
                        <table border="0" cellspacing="1px" cellspacing="1px" style="text-align: right; width: 100%">
                            <tr>

                                <td style="width: 40%; text-align: left;">
                                    <asp:TextBox ID="txtTenNhom" CssClass="inputtext" Width="80%" runat="server" onkeypress="return clickButton(event,'ctl00_MainContent_linkSearch');" placeholder="Tên nhóm"></asp:TextBox>
                                    <asp:Button runat="server" ID="linkSearch" CssClass="iconFind" Font-Bold="true" OnClick="linkSearch_OnClick"
                                        Text="" Width="40px" Height="38px"></asp:Button>
                                </td>

                                <td style="width: 60%; text-align: left;">

                                    <asp:LinkButton runat="server" ID="linkAddNews" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="linkAddNews_Click" CausesValidation="false">
                                                    <i class="ace-icon fa fa-pencil-square-o bigger-120 white"></i>
                                                    Thêm mới
                                    </asp:LinkButton>
                                     <asp:LinkButton runat="server" ID="btnExportExel" CssClass="btn btn-sm btn-primary btn-bold"
                                    OnClick="btnExcel_Click">
                                                    <span class="glyphicon glyphicon-download-alt"></span>
                                                    Export data into Excel
                                </asp:LinkButton>
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
                    <asp:DataGrid runat="server" ID="grdListNhom" AutoGenerateColumns="false" DataKeyField="Group_ID"
                        Width="100%" CssClass="Grid" OnEditCommand="grdListNhom_EditCommand" OnItemDataBound="grdListNhom_ItemDataBound">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>
                            <asp:BoundColumn Visible="False" DataField="Group_ID">
                                <HeaderStyle Width="1%"></HeaderStyle>
                            </asp:BoundColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                <HeaderTemplate>
                                    <asp:Literal ID="Literal12" runat="server" Text="<%$ Resources:Strings, GRIDTITTLETenNhom %>"></asp:Literal>
                                </HeaderTemplate>
                                <ItemStyle Width="25%" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" CssClass="linkEdit" Text='<%# DataBinder.Eval(Container.DataItem, "Group_Name")%>'
                                        runat="server" ToolTip="<%$ Resources:Strings, SYSTEM_GridTiitleEdit %>" CommandName="Edit"
                                        CommandArgument="Edit" Enabled='<%#_Role.R_Edit %>'>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>


                            <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                            <HeaderTemplate>
                                                Quyền xem báo cáo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRoleLang" Width="25px" runat="server" ImageUrl="~/images/language.png"
                                                    ImageAlign="AbsMiddle" ToolTip="Phân quyền xem báo cáo" CommandName="Edit" CommandArgument="btnRoleLang"
                                                    BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                <HeaderTemplate>
                                    Quyền chức năng
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnRole" Width="25px" runat="server" ImageUrl="~/images/function.png"
                                        ImageAlign="AbsMiddle" ToolTip="<%$ Resources:Strings, GRIDTITTLERolePhanQuyen %>"
                                        CommandName="Edit" CommandArgument="Role" BorderStyle="None" Visible='<%#_Role.R_Edit %>'></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <%--<asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                            <HeaderTemplate>
                                                Quyền chuyên mục
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnRoleCate" Width="25px" runat="server" ImageUrl="~/images/list.png"
                                                    ImageAlign="AbsMiddle" ToolTip="Phân quyền chuyên mục" Visible='<%#_Role.R_Edit %>'
                                                    CommandName="Edit" CommandArgument="btnRoleCate" BorderStyle="None"></asp:ImageButton>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>--%>

                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="15%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center" Width="15%"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="Literal22" runat="server" Text="<%$ Resources:Strings, SYSTEM_GridTiitleDelete %>"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnDelete" Width="25px" runat="server" ImageUrl="~/images/delete.png"
                                        ImageAlign="AbsMiddle" ToolTip="<%$ Resources:Strings, SYSTEM_GridTiitleDelete %>"
                                        CommandName="Edit" Visible='<%#_Role.R_Del %>' CommandArgument="Delete" BorderStyle="None"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
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

    </asp:Panel>
    <!--Phan Quyen chuc nang-->
    <asp:Panel ID="plRole" runat="server" Visible="false" CssClass="TitlePanel" BackColor="white"
        BorderStyle="NotSet">
        <script language="Javascript" type="text/javascript">

            function CheckAllandUnCheckAll(objRef) {
                var GridView = document.getElementById('<%=gdListMenu.ClientID%>');
                var inputList = GridView.getElementsByTagName("input");
                for (var i = 0; i < inputList.length; i++) {
                    var row = inputList[i].parentNode.parentNode;
                    if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                        if (objRef.checked && inputList[i].disabled == false) {
                            inputList[i].checked = true;
                        }
                        else {
                            inputList[i].checked = false;
                        }
                    }
                }
            }

        </script>
        <span style="font-weight: bold;">+ + GÁN QUYỀN SỬ DỤNG CHỨC NĂNG CHO NHÓM NGƯỜI DÙNG:</span>
         <asp:Label ID="roleChucNang" runat="server"></asp:Label></span>
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td class="TitlePanel" style="height: 25px; text-align: right"></td>
            </tr>
            <tr>
                <td align="left">
                    <asp:DataGrid runat="server" ID="gdListMenu" OnItemDataBound="gdListMenu_ItemDataBound"
                        AutoGenerateColumns="false" DataKeyField="Menu_ID" CssClass="Grid" Width="100%">
                        <ItemStyle CssClass="GridItem"></ItemStyle>
                        <AlternatingItemStyle CssClass="GridAltItem" />
                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                        <Columns>
                            <asp:TemplateColumn>
                                <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAll" onclick="javascript:CheckAllandUnCheckAll(this);" runat="server"
                                        AutoPostBack="false" ToolTip="Select/Deselect All"></asp:CheckBox>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input type="checkbox" id="optSelect" runat="server" onclick='ChkParrent(this, this.value);'
                                        checked='<%# DataBinder.Eval(Container.DataItem, "Role_Menu") %>' value='<%#Container.ItemIndex+2%>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="30%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="Literal" runat="server" Text="<%$ Resources:Strings, GRIDTITTLERoleTenDanhMuc %>"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# SetNameMenu(DataBinder.Eval(Container.DataItem, "Menu_Name"), DataBinder.Eval(Container.DataItem, "Menu_ID"))%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn>
                                <HeaderStyle Width="30%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources:Strings, GRIDTITTLERoleMoTa %>"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Menu_Desc")%>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText=" <span>Thêm </span>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="6%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkR_Add" runat="server" name="chkR_Add" checked='<%# Eval("R_Add") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<span>Sửa</span>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="6%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkR_Edit" runat="server" name="chkR_Edit" checked='<%# Eval("R_Edit") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<span>Xóa</span>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="6%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkR_Del" runat="server" name="chkR_Del" checked='<%# Eval("R_Del") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="<span>Duyệt</span>" HeaderStyle-HorizontalAlign="Center">
                                <HeaderStyle Width="6%"></HeaderStyle>
                                <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <input type="checkbox" id="chkR_Pub" runat="server" name="chkR_Pub" checked='<%# Eval("R_Pub") %>' />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td>

                    <asp:LinkButton runat="server" ID="linkGroupSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                        OnClick="linkGroupSave_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                    </asp:LinkButton>

                    <asp:LinkButton runat="server" ID="LinkGroupExit" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
                        OnClick="LinkGroupExit_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Thoát
                    </asp:LinkButton>
                </td>
            </tr>
        </table>

    </asp:Panel>
    <!--END-->
    <!--PHAN QUYEN CHUYEN MUC-->
    <asp:Panel runat="server" ID="pnlRoleCategorys" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ GÁN QUYỀN SỬ DỤNG CHUYÊN MỤC CHO NHÓM NGƯỜI DÙNG:
                        <asp:Label ID="lblRoleChuyenMuc" runat="server"></asp:Label></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                    <tr>
                                        <td style="width: 60%; text-align: right; vertical-align: middle" class="Titlelbl">Loại báo:
                                        </td>
                                        <td style="text-align: left; vertical-align: middle; width: 40%">
                                            <asp:DropDownList ID="ddlLang" AutoPostBack="true" Width="90%" CssClass="inputtext"
                                                runat="server" OnSelectedIndexChanged="ddlLang_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <asp:DataGrid runat="server" ID="dgCategory" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="Categorys_ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="10%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderTemplate>
                                                Tên chuyên mục
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container.DataItem, "Category_Name")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>


                                <asp:LinkButton runat="server" ID="linkRoleCateSaves" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                    OnClick="linkRoleCateSaves_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Lưu giữ
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" ID="linkRoleCateExit" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false"
                                    OnClick="linkRoleCateExit_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Thoát
                                </asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--END-->
    <!--PHAN QUYEN NGON NGU-->
    <asp:Panel runat="server" ID="pnlLang" Visible="false" CssClass="TitlePanel" BackColor="white"
        BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ GÁN QUYỀN SỬ DỤNG BÁO CÁO CHO NHÓM NGƯỜI DÙNG:
                        <asp:Label ID="lblRoleNgonNgu" runat="server"></asp:Label></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="TitlePanel" style="height: 25px; text-align: right"></td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="dgListLanguages_ID" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="2%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" runat="server" Checked='<%# Convert.ToBoolean(DataBinder.Eval(Container.DataItem, "role")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="30%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Danh sách báo cáo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#Eval("NAME_REPORT")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="linkSaveLang" OnClick="linkSaveLang_Click" Text="<%$ Resources:Strings, BUTTON_SAVES %>"
                                    CssClass="iconLuu" />
                                <asp:Button runat="server" ID="linkExitLang" CssClass="iconThoat" OnClick="linkExitLang_Click"
                                    Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--END-->
    <!--Phan Quyen Loai bao-->
    <asp:Panel runat="server" ID="panelLoaibao" Visible="false" CssClass="TitlePanel"
        BackColor="white" BorderStyle="NotSet">
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left"></td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel"></span>
                </td>
                <td class="datagrid_top_right"></td>
            </tr>
            <tr>
                <td class="datagrid_content_left"></td>
                <td style="text-align: center">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="TitlePanel" style="height: 25px; text-align: right">
                                <asp:Label ID="lbLoaibao" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <asp:DataGrid runat="server" ID="drgListPaper" OnItemDataBound="gdListMenu_ItemDataBound"
                                    AutoGenerateColumns="false" DataKeyField="Paper_ID" Width="100%" CssClass="Grid">
                                    <ItemStyle CssClass="GridItem"></ItemStyle>
                                    <AlternatingItemStyle CssClass="GridAltItem" />
                                    <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                    <Columns>
                                        <asp:TemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" Width="5%"></HeaderStyle>
                                            <ItemStyle HorizontalAlign="Center" Width="5%"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server"
                                                    ToolTip="Select/Deselect All"></asp:CheckBox>
                                            </HeaderTemplate>
                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="optSelect" runat="server" Checked='<%# DataBinder.Eval(Container.DataItem, "Role") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn>
                                            <HeaderStyle Width="95%"></HeaderStyle>
                                            <HeaderTemplate>
                                                Loại báo
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%#DataBinder.Eval(Container.DataItem, "Paper_Name")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 10px"></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Button runat="server" ID="Button3" OnClick="linkRoleSavesPaper_Click" CssClass="iconLuu"
                                    Text="<%$ Resources:Strings, BUTTON_SAVES %>" />
                                <asp:Button runat="server" ID="Button4" OnClick="linkRoleExitPaper_Click" CssClass="iconThoat"
                                    Text="<%$ Resources:Strings, BUTTON_SIGOUT %>" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right"></td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left"></td>
                <td class="datagrid_bottom_center"></td>
                <td class="datagrid_bottom_right"></td>
            </tr>
        </table>
    </asp:Panel>
    <!--End-->
</asp:Content>
