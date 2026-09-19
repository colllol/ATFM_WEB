<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DownloadImageFromWebsite.aspx.cs"
    Inherits="prjApplication.Until.DownloadImageFromWebsite" %>

<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Register TagPrefix="anthem" Namespace="Anthem" Assembly="Anthem" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Download Ảnh</title>
    <link rel="Stylesheet" type="text/css" href="CSS/uploadify.css" />

    <script type="text/javascript" src="scripts/jquery-1.3.2.min.js"></script>

    <script type="text/javascript" src="scripts/jquery.uploadify.js"></script>

    <link type="text/css" rel="Stylesheet" href="../Style/style.css" />
    <style>
        BODY
        {
            border-right: 0px;
            border-top: 0px;
            margin: 0px;
            overflow: hidden;
            border-left: 0px;
            border-bottom: 0px;
            background-color: buttonface;
        }
        .button
        {
            font-size: 12px;
            color: #000099;
            font-family: 'Arial';
        }
        .inputtext
        {
            border-right: #cccccc 1px solid;
            border-top: #cccccc 1px solid;
            font-weight: normal;
            font-size: 12px;
            border-left: #cccccc 1px solid;
            cursor: hand;
            color: #000000;
            border-bottom: #cccccc 1px solid;
            font-family: Arial, Helvetica, sans-serif;
        }
        .Time
        {
            font-weight: normal;
            font-size: 11px;
            color: #000000;
            font-family: Arial, Helvetica, sans-serif;
        }
        #displayContainer
        {
            padding-right: 1px;
            padding-left: 1px;
            scrollbar-face-color: #cacaca;
            font-size: 10pt;
            padding-bottom: 1px;
            margin: 0px;
            scrollbar-highlight-color: #cacaca;
            overflow: auto;
            width: 100%;
            scrollbar-shadow-color: #cacaca;
            color: #000000;
            padding-top: 1px;
            font-family: Verdana, Arial, Helvetica, sans-serif;
            height: 350px;
        }
        .pageNav
        {
            font-weight: normal;
            font-size: 12px;
            color: #000099;
            font-family: 'Tahoma';
        }
        TD.currentFolder
        {
            border-right: #cccccc 1px solid;
            border-top: #cccccc 1px solid;
            font-weight: bold;
            font-size: 12px;
            border-left: #cccccc 1px solid;
            color: #000099;
            border-bottom: #cccccc 1px solid;
            font-family: Arial, Helvetica, sans-serif;
            background-color: #f1f1f1;
        }
        .currentFolderText
        {
            font-weight: bold;
            font-size: 12px;
            color: #000099;
            font-family: Arial, Helvetica, sans-serif;
            background-color: #f1f1f1;
        }
        TD.currentFolderContent
        {
            border-right: #cccccc 2px solid;
            border-top: #cccccc 2px solid;
            font-weight: bold;
            font-size: 12px;
            border-left: #cccccc 2px solid;
            color: #000099;
            border-bottom: #cccccc 2px solid;
            font-family: Arial, Helvetica, sans-serif;
            background-color: #ffffff;
        }
        fieldset
        {
            -moz-border-radius: 4px;
            border-radius: 4px;
            -webkit-border-radius: 4px;
            border: solid 1px #d4d4d4;
            padding: 2px;
        }
        legend
        {
            color: black;
            font-size: 100%;
            width: 150px;
        }
    </style>

    <script type="text/javascript" language="javascript">
    function openNewImage(file, imgText) {
            if (file.lang == 'no-popup') return;
            picfile = new Image();
            picfile.src = (file.src);
            width = picfile.width;
            height = picfile.height;

            if (imgText != '' && height > 0) {
                height += 40;
            }
            else if (height == 0) {
                height = screen.height;
            }

            winDef = 'status=no,resizable=yes,scrollbars=no,toolbar=no,location=no,fullscreen=no,titlebar=yes,height='.concat(height).concat(',').concat('width=').concat(width).concat(',');
            winDef = winDef.concat('top=').concat((screen.height - height) / 2).concat(',');
            winDef = winDef.concat('left=').concat((screen.width - width) / 2);
            newwin = open('', '_blank', winDef);

            newwin.document.writeln('<style>a:visited{color:blue;text-decoration:none}</style>');
            newwin.document.writeln('<body topmargin="0" leftmargin="0" marginheight="0" marginwidth="0">');
            newwin.document.writeln('<div style="width:100%;height:100%;overflow:auto;"><a style="cursor:pointer" href="javascript:window.close()"><img src="', file.src, '" border=0></a>');
            if (imgText != '') {
                newwin.document.writeln('<div align="center" style="padding-top:5px;font-weight:bold;font-family:arial,Verdana,Tahoma;color:blue">', imgText, '</div></div>');
            }
            newwin.document.writeln('</body>');
            newwin.document.close();
        }
    </script>

</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 5px">
    <form id="frmUpLoad" method="post" enctype="multipart/form-data" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td class="datagrid_top_left">
            </td>
            <td class="datagrid_top_center">
                <span class="TitlePanel">+ DOWNLOAD ẢNH TỪ WEBSITE</span>
            </td>
            <td class="datagrid_top_right">
            </td>
        </tr>
        <tr>
            <td class="datagrid_content_left">
            </td>
            <td style="text-align: left">
                <table cellpadding="1" cellspacing="1" border="0" width="100%">
                    <tr>
                        <td align="left" colspan="3">
                            Nguồn download :
                            <asp:DropDownList ID="Drop_nguon" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Drop_nguon_SelectedIndexChanged">
                                <asp:ListItem Value="1">Từ website
                                </asp:ListItem>
                                <asp:ListItem Value="2">Từ nguồn khác</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                     <tr>
                        <td align="left" style="height: 10px" colspan="3">
                        </td>
                    </tr>
                    <tr>
                        <td align="left" style="width: 20%" colspan="3">
                            <asp:Panel ID="panel_website" runat="server" Visible="true">
                                <table width="100%">
                                    <tr>
                                        <td align="left">
                                            Địa chỉ Website :
                                            <asp:TextBox ID="fileWebURL" onkeypress="return clickButton(event,'ctl00_MainContent_cmd_getImage');" runat="server" Width="60%"></asp:TextBox>
                                            <asp:Button ID="cmd_getImage" runat="server" Text="Xem ảnh" OnClick="cmd_getImage_Click" />
                                        </td>
                                    </tr>
                                     <tr>
                                        <td align="left"  style="height: 10px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="Panel_webservice" runat="server" Visible="false">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Từ ngày:
                                            <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../scripts/DatePicker/Images"
                                                CssClass="inputtext" Width="100px" ScriptSource="../Scripts/datepicker.js" ID="txt_FromDate"
                                                runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                                onKeyUp="DateFormat(this,this.value,event,false,'3')" onBlur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                        </td>
                                        <td>
                                            Đến ngày:
                                            <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../Scripts/DatePicker/Images"
                                                CssClass="inputtext" Width="100px" ScriptSource="../Scripts/datepicker.js" ID="txt_ToDate"
                                                runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                                onKeyUp="DateFormat(this,this.value,event,false,'3')" onBlur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="height: 10px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            Tên file:
                                            <asp:TextBox ID="fileName" onkeypress="return clickButton(event,'ctl00_MainContent_cmd_Getwebservice');" runat="server" CssClass="inputtext" Width="70%"></asp:TextBox>
                                            <asp:Button ID="cmd_Getwebservice" runat="server" Text="Xem ảnh" OnClick="cmd_getImage_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2" style="height: 10px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="text-align: left; height: 25px; padding-left: 4px" bgcolor="#cccccc"
                            class="TitlePanel">
                            + DANH SÁCH CÁC ẢNH ĐƯỢC TÌM THẤY
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center" colspan="3">
                            <div id="displayContainer">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:DataList ID="dlImages" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                            RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                            ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset" ItemStyle-Width="20%"
                                            ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2">
                                            <ItemTemplate>
                                                <table border="0" cellspacing="1" cellpadding="1" width="100%" style="background-color: #f1f1f1">
                                                    <tr>
                                                        <td style="text-align: center; height: 100px; width: 100px; vertical-align: middle">
                                                            <img alt="<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"URLImage"))%>" src="<%#DataBinder.Eval(Container.DataItem,"URLImage")%>"
                                                                title="<%#DataBinder.Eval(Container.DataItem,"Title")%>" border="1" width="80px"
                                                                height="60px" style="cursor: pointer;" onclick="return openNewImage(this, '')" />
                                                            <br />
                                                            (<%#DataBinder.Eval(Container.DataItem,"Size")%>)
                                                            <br />
                                                            <asp:Label ID="lbl_Warning" runat="server" Text="" Visible="false" ForeColor="Red"></asp:Label>
                                                            <asp:Label ID="lbl_URL" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"URLImage")%>'></asp:Label>
                                                            <asp:CheckBox ID="checkbox_download" runat="server" /><br />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:DataList>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="height: 15px;">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 70%">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td align="left">
                                                Chuyên trang :
                                                <asp:DropDownList ID="Drop_Lang" runat="server" AutoPostBack="True" OnSelectedIndexChanged="Drop_Lang_SelectedIndexChanged"
                                                    Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left">
                                                Chuyên mục :
                                                <asp:DropDownList ID="Drop_Chuyenmuc" runat="server" AutoPostBack="True" Width="150px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                        <td>
                            <asp:Button ID="cmd_Download" runat="server" Text="Download ảnh" OnClick="cmd_Download_Click" />
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
    </form>
</body>
</html>
