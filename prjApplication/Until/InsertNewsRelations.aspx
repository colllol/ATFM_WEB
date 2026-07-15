<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertNewsRelations.aspx.cs"
    Inherits="prjApplication.Until.InsertNewsRelations" %>

<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="REFRESH" content="1800" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <meta name="robots" content="INDEX,FOLLOW" />
    <title>Chèn tin bài liên quan</title>
    <link type="text/css" rel="Stylesheet" href="../Style/style.css" />

    <script type="text/javascript">        
        function Send_NewRelation() {
            var strBuild1 = "";            
            var _break1 = " ,";
            var elm = document.FormNews.elements;
            for (i = 0; i < elm.length; i++) {
                if (elm[i].type == "checkbox" && elm[i].id != 'dgr_tintuc1_ctl01_chkAll' && elm[i].id != 'chkNewsIsBaidinh' && elm[i].id != 'chkNewsIsFocus' && elm[i].id != 'chkNewTieudiem' && elm[i].id != 'chkNewFocusParent' && elm[i].id != 'chkNewFocusChild' && elm[i].id != 'chkVideoIsFocus' && elm[i].id != 'chkImageIsFocus' && elm[i].id != 'chkHosoIsFocus' && elm[i].id != 'chDisplayMobi') {
                    if (elm[i].checked) {
                        var ID = 'iStr' + elm[i].name;                      
                        strBuild1 += elm[i].name + _break1;

                    }
                }
            }
            strBuild1 += 0;            
            InsertNewRelation(strBuild1);
        }
        function InsertNewRelation(strText1) {           
            var browserName = navigator.appName;
            window.opener.InsertNewRelation(strText1);
            window.close();
        }
    </script>

    <script language="javascript" type="text/javascript" src="../Scripts/hpc.js"></script>

    <script language="javascript" type="text/javascript" src="../Scripts/Lib.js"></script>

</head>
<body style="margin-top: 2px; margin-left: 2px; margin-right: 2px; margin-bottom: 2px">
    <form id="FormNews" method="post" runat="server">
    <div>
        <table border="0" cellpadding="0" width="100%" cellspacing="0">
            <tr>
                <td class="datagrid_top_left">
                </td>
                <td class="datagrid_top_center">
                    <span class="TitlePanel">+ CHÈN TIN LIÊN QUAN VÀO NỘI DUNG CỦA BÀI VIẾT</span>
                </td>
                <td class="datagrid_top_right">
                </td>
            </tr>
            <tr>
                <td class="datagrid_content_left">
                </td>
                <td style="text-align: center">
                    <table style="border: 0" width="100%" cellpadding="2" cellspacing="2">
                        <tr>
                            <td class="Titlelbl" style="text-align: right">
                                Chuyên trang:
                            </td>
                            <td style="text-align: left">
                                <anthem:DropDownList AutoCallBack="true" ID="cboNgonNgu" runat="server" Width="190px"
                                    CssClass="inputtext" DataTextField="Languages_Name" DataValueField="Languages_ID"
                                    OnSelectedIndexChanged="cbo_lanquage_SelectedIndexChanged" TabIndex="1">
                                </anthem:DropDownList>
                            </td>
                            <td class="Titlelbl" style="text-align: right">
                                Chuyên mục:
                            </td>
                            <td style="text-align: left;">
                                <anthem:DropDownList AutoCallBack="true" ID="cbo_chuyenmuc" runat="server" Width="190px"
                                    CssClass="inputtext" DataTextField="tenchuyenmuc" DataValueField="id" TabIndex="5">
                                </anthem:DropDownList>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Bài đinh" ID="chkNewsIsBaidinh" class="ChkBoxTit"
                                    TabIndex="6" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Nổi bật CM cha" ID="chkNewFocusParent" class="ChkBoxTit"
                                    TabIndex="9" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Tin ảnh" ID="chkImageIsFocus" class="ChkBoxTit"
                                    TabIndex="11" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Titlelbl" style="text-align: right">
                                Từ ngày:
                            </td>
                            <td style="text-align: left">
                                <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../Scripts/DatePicker/images/"
                                    CssClass="inputtext" Width="152px" ScriptSource="../Scripts/datepicker.js"
                                    ID="txt_tungay" runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                    onKeyUp="DateFormat(this,this.value,event,false,'3')" onBlur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                <asp:RegularExpressionValidator ID="Regularexpressionvalidator2" runat="server" ControlToValidate="txt_tungay"
                                    ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                            </td>
                            <td class="Titlelbl" style="text-align: right">
                                Đến ngày:
                            </td>
                            <td style="text-align: left">
                                <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../Scripts/DatePicker/Images"
                                    CssClass="inputtext" Width="152px" ScriptSource="../Scripts/datepicker.js"
                                    ID="txt_denngay" runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                    onKeyUp="DateFormat(this,this.value,event,false,'3')" onBlur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                <asp:RegularExpressionValidator ID="Regularexpressionvalidator1" runat="server" ControlToValidate="txt_denngay"
                                    ValidationExpression="^\d{1,2}/\d{1,2}/\d{4}$" Display="Dynamic">Kiểu ngày:dd/MM/yyyy</asp:RegularExpressionValidator>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Tin nóng trang chủ" ID="chkNewsIsFocus" class="ChkBoxTit"
                                    TabIndex="7" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Nổi bật CM con" ID="chkNewFocusChild" class="ChkBoxTit"
                                    TabIndex="10" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Tin ẩn" ID="chkHosoIsFocus" class="ChkBoxTit"
                                    TabIndex="13" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="Titlelbl" style="text-align: right">
                                Từ khóa:
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txt_tieude" TabIndex="1" Width="180px" runat="server" CssClass="inputtext"
                                    onkeypress="return clickButton(event,'cmdSeek');"></asp:TextBox>
                            </td>
                            <td class="Titlelbl" style="text-align: right">
                                Tiêu đề:
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTenbai" TabIndex="1" Width="180px" runat="server" CssClass="inputtext"
                                    onkeypress="return clickButton(event,'cmdSeek');"></asp:TextBox>
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Tin tiêu điểm" ID="chkNewTieudiem" class="ChkBoxTit"
                                    TabIndex="8" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Tin video" ID="chkVideoIsFocus" class="ChkBoxTit"
                                    TabIndex="12" Visible="true" />
                            </td>
                            <td style="text-align: left;">
                                <asp:CheckBox runat="server" Text="Không HT Mobile" ID="chDisplayMobi" class="ChkBoxTit"
                                    TabIndex="12" Visible="true" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 10px;" colspan="5">
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center;" colspan="5">
                                <asp:Button runat="server" ID="cmdSeek" CssClass="iconFind" Style="margin-left: 10px;"
                                    Font-Bold="true" Text="Tìm kiếm" OnClick="cmdSeek_Click" />
                            </td>
                        </tr>
                    </table>
                    <table width="100%" cellspacing="2" cellpadding="2" border="0">
                        <tr>
                            <td align="left" colspan="2">
                                <div class="popup_Body_Fix_width_height">
                                    <asp:DataGrid ID="dgr_tintuc1" runat="server" Width="100%" AutoGenerateColumns="False"
                                        OnItemDataBound="dgr_tintuc1_ItemDataBound" OnEditCommand="dgr_tintuc1_EditCommand"
                                        DataKeyField="News_ID" CssClass="Grid">
                                        <ItemStyle CssClass="GridItem"></ItemStyle>
                                        <AlternatingItemStyle CssClass="GridAltItem" />
                                        <HeaderStyle CssClass="GridHeader"></HeaderStyle>
                                        <Columns>
                                            <asp:TemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" Width="3%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="3%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkAll" onclick="javascript:CheckboxesAll(this,'dgr_tintuc1');"
                                                        runat="server" ToolTip="Chọn tất cả"></asp:CheckBox>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" id="optSelect" name="<%# DataBinder.Eval(Container.DataItem, "News_ID") %>">
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:BoundColumn DataField="News_ID" HeaderText="News_ID" Visible="False"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="<%$ Resources:Strings, BA_QLND_NEWS_NAME %>">
                                                <HeaderStyle HorizontalAlign="Left" Width="25%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                <ItemTemplate>
                                                    <b>
                                                        <%# paintColorSearch(DataBinder.Eval(Container.DataItem, "News_Tittle" )) %></b>
                                                    <input type="hidden" id='iStr<%#Eval("News_ID") %>' value="<%#Eval("News_Tittle")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Chuyên mục">
                                                <HeaderStyle Width="10%" HorizontalAlign="Left"></HeaderStyle>
                                                <ItemStyle Width="10%" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%#prjBusinessLogic.UltilFunc.GetCategoryName(Eval("CAT_ID"))%>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
                                            <asp:TemplateColumn HeaderText="Ngày xuất bản">
                                                <ItemTemplate>
                                                    <%#Eval("News_DatePublished") != System.DBNull.Value ? Convert.ToDateTime(Eval("News_DatePublished")).ToString("dd/MM/yyyy HH:mm") : ""%>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" Width="6%"></HeaderStyle>
                                                <ItemStyle HorizontalAlign="Center" Width="6%"></ItemStyle>
                                            </asp:TemplateColumn>
                                        </Columns>
                                    </asp:DataGrid>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <input type="button" id="btn_OkInsert" class="iconPub" value="Đồng ý" onclick="Send_NewRelation();" />
                                <input class="iconExit" type="button" value="Đóng" onclick="window.parent.close()">
                            </td>
                            <td style="text-align: right" class="pageNav">
                                <cc1:CurrentPage runat="server" ID="CurrentPage2" CssClass="pageNavTotal">
                                </cc1:CurrentPage>
                                <cc1:Pager runat="server" ID="pages" OnIndexChanged="pages_IndexChanged_baidangxuly"></cc1:Pager>
                            </td>
                        </tr>
                    </table>
                </td>
                <td class="datagrid_content_right">
                </td>
            </tr>
            <tr>
                <td class="datagrid_bottom_left">
                </td>
                <td class="datagrid_bottom_center">
                </td>
                <td class="datagrid_bottom_right">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
<script type="text/javascript">

    function doPostBackAsync(eventTarget, eventArgs) {
        var pageReqMgr = Sys.WebForms.PageRequestManager.getInstance();
        if (!Array.contains(pageReqMgr._asyncPostBackControlIDs, eventTarget)) {
            pageReqMgr._asyncPostBackControlIDs.push(eventTarget);
        }
        if (!Array.contains(pageReqMgr._asyncPostBackControlClientIDs, eventTarget)) {
            pageReqMgr._asyncPostBackControlClientIDs.push(eventTarget);
        }
        __doPostBack(eventTarget, eventArgs);
    }
</script>
