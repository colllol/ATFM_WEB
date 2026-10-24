<%@ Page Language="C#" AutoEventWireup="true" Codebehind="FileManagerment.aspx.cs"
    Inherits="prjApplication.UploadMulti.FileManagerment" %>
<%@ Register Assembly="FlashUpload" Namespace="FlashUpload" TagPrefix="FlashUpload" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>FileManagerment</title>

    <script type="text/javascript">
			function getImgSrc(theImagePath,numberAgr,Size,FileExtension)
			{
				//alert(theImagePath + ' ' + numberAgr);
				//return;
				if (theImagePath !="")
				{
					//frmUpLoad.txt_Src.value = theImagePath;
					//alert(theImagePath);
					window.close();
					opener.getPath(theImagePath,numberAgr,Size,FileExtension);
									
				}	
			}
			function confirm_deleteFile()
			{
				var stringMess="";
				stringMess ='Bạn có thực sự muốn xóa thư các file đã chọn không?';
				//alert(stringMess);
				if (confirm(stringMess)==true)
					return true;
				else
					return false;
			}
    </script>

    <style type="text/css">
    BODY { BORDER-RIGHT: 0px; BORDER-TOP: 0px; MARGIN: 0px; OVERFLOW: hidden; BORDER-LEFT:
    0px; BORDER-BOTTOM: 0px; BACKGROUND-COLOR: buttonface } .button { FONT-SIZE: 12px;
    COLOR: #000099; FONT-FAMILY: 'Arial' } .inputtext { BORDER-RIGHT: #cccccc 1px solid;
    BORDER-TOP: #cccccc 1px solid; FONT-WEIGHT: normal; FONT-SIZE: 12px; BORDER-LEFT:
    #cccccc 1px solid; CURSOR: hand; COLOR: #000000; BORDER-BOTTOM: #cccccc 1px solid;
    FONT-FAMILY: Arial, Helvetica, sans-serif } .Time { FONT-WEIGHT: normal; FONT-SIZE:
    11px; COLOR: #000000; FONT-FAMILY: Arial, Helvetica, sans-serif } #displayContainer
    { PADDING-RIGHT: 1px; PADDING-LEFT: 1px; SCROLLBAR-FACE-COLOR: #cacaca; FONT-SIZE:
    10pt; PADDING-BOTTOM: 1px; MARGIN: 0px; SCROLLBAR-HIGHLIGHT-COLOR: #cacaca; OVERFLOW:
    auto; WIDTH: 100%; SCROLLBAR-SHADOW-COLOR: #cacaca; COLOR: #000000; SCROLLBAR-3DLIGHT-COLOR:
    #cacaca; SCROLLBAR-ARROW-COLOR: #000000; PADDING-TOP: 1px; SCROLLBAR-TRACK-COLOR:
    #cacaca; FONT-FAMILY: Verdana, Arial, Helvetica, sans-serif; SCROLLBAR-DARKSHADOW-COLOR:
    #cacaca; HEIGHT: 260px } .pageNav { FONT-WEIGHT: normal; FONT-SIZE: 12px; COLOR:
    #000099; FONT-FAMILY: 'Tahoma' } TD.currentFolder { BORDER-RIGHT: #cccccc 1px solid;
    BORDER-TOP: #cccccc 1px solid; FONT-WEIGHT: bold; FONT-SIZE: 12px; BORDER-LEFT:
    #cccccc 1px solid; COLOR: #000099; BORDER-BOTTOM: #cccccc 1px solid; FONT-FAMILY:
    Arial, Helvetica, sans-serif; BACKGROUND-COLOR: #f1f1f1 } .currentFolderText{FONT-WEIGHT:
    bold; FONT-SIZE: 12px; COLOR: #000099; FONT-FAMILY: Arial, Helvetica, sans-serif;
    BACKGROUND-COLOR: #f1f1f1 } TD.currentFolderContent { BORDER-RIGHT: #cccccc 2px
    solid; BORDER-TOP: #cccccc 2px solid; FONT-WEIGHT: bold; FONT-SIZE: 12px; BORDER-LEFT:
    #cccccc 2px solid; COLOR: #000099; BORDER-BOTTOM: #cccccc 2px solid; FONT-FAMILY:
    Arial, Helvetica, sans-serif; BACKGROUND-COLOR: #ffffff } </style>
</head>
<body>
    <form id="frmUpLoad" method="post" enctype="multipart/form-data" runat="server">
        <table style="height: 100%" cellspacing="1" cellpadding="1" border="0" width="100%">
            <tr>
                <td style="width: 100%" valign="top">
                    <fieldset style="padding-right: 5px; padding-left: 5px; padding-bottom: 5px; width: 100%;
                        padding-top: 5px; text-align: left">
                        <legend style="font-weight: bold; font-size: 10px; width: 120px; font-family: Arial;
                            height: 16px" name="lgd1">Thông tin tìm kiếm ảnh</legend>
                        <table cellpadding="1" cellspacing="1" border="0" width="100%">
                            <tr>
                                <td align="right">
                                    <table border="0" cellpadding="2" cellspacing="2">
                                        <tr>
                                            <td class="Time">
                                                Ngày</td>
                                            <td class="Time">
                                                <asp:DropDownList CssClass="inputtext" runat="server" ID="combo_Ngay">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Time">
                                                Tháng</td>
                                            <td class="Time">
                                                <asp:DropDownList CssClass="inputtext" runat="server" ID="cbo_Thang">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Time">
                                                Năm</td>
                                            <td class="Time">
                                                <asp:DropDownList CssClass="inputtext" runat="server" ID="cbo_Nam">
                                                </asp:DropDownList>
                                            </td>
                                            <td class="Time">
                                                <asp:Button ID="btnSearchFolder" runat="server" Text="Tìm kiếm" CssClass="button"></asp:Button>
                                            </td>
                                            <td>
                                                <asp:Button ID="btnDeleteFile" OnClientClick="return confirm('Bạn có chắc muốn xóa file không?')" runat="server" Text="Xóa file" CssClass="button" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="1" colspan="10" bgcolor="#ffffff">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" width="100%">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td class="currentFolder" style="height: 25px" width="100%">
                                                <table border="0" cellpadding="1" cellspacing="1">
                                                    <tr>
                                                        <td>
                                                            <img alt="" src="../images/folder.png" align="left" /></td>
                                                        <td class="currentFolderText">
                                                            <asp:Label ID="lblFolder2Upload" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="1">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="currentFolderContent">
                                                <div id="displayContainer">
                                                    <asp:DataList ID="dlFolder" DataKeyField="FilePath" runat="server" RepeatColumns="4"
                                                        RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-BorderWidth="1" ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset"
                                                        ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2">
                                                        <ItemTemplate>
                                                            <table border="0" cellspacing="1" cellpadding="1">
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:ImageButton ID="btnEdit" runat="server" ImageUrl="~/images/folder_closed.png"
                                                                            ImageAlign="AbsMiddle" CommandName="Edit"></asp:ImageButton></td>
                                                                </tr>
                                                                <tr>
                                                                    <td class="Time" align="center">
                                                                        <a href="">
                                                                            <%#DataBinder.Eval(Container.DataItem,"FileName")%>
                                                                        </a>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                    <asp:DataList ID="dlImages" DataKeyField="FilePath" runat="server" RepeatColumns="4"
                                                        RepeatDirection="Horizontal" RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center"
                                                        ItemStyle-BorderWidth="0" ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset"
                                                        ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2">
                                                        <ItemTemplate>
                                                            <div>
                                                                <img alt="<%#DataBinder.Eval(Container.DataItem,"FileName")%>" src="<%#UrlPathImage_Display(DataBinder.Eval(Container.DataItem,"FileExtension"))%>"
                                                                    border="0" width="100px" style="cursor: pointer;" height="80px" title="<%#DataBinder.Eval(Container.DataItem,"FileName")%>"
                                                                    onclick="getImgSrc('<%#UrlPathImage_RemoveUpload(DataBinder.Eval(Container.DataItem,"FileView"))%>','<%=strKeyLogo%>','<%#DataBinder.Eval(Container.DataItem,"Size")%>','<%#DataBinder.Eval(Container.DataItem,"FileExtension")%>')">
                                                                <asp:Label ID="lblFilePath" runat="server" Text='<%# Bind("FilePath") %>' Visible="false"></asp:Label>
                                                            </div>
                                                            <div>
                                                                <asp:CheckBox ID="chkDelete" runat="server" />
                                                            </div>
                                                            <!--<div>
                                                                <%#DataBinder.Eval(Container.DataItem,"FileName")%>
                                                            </div>-->
                                                        </ItemTemplate>
                                                    </asp:DataList>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" width="100%" height="1" bgcolor="#ffffff">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="10" width="100%">
                                    <fieldset style="padding-right: 5px; padding-left: 5px; padding-bottom: 5px; width: 100%;
                                        padding-top: 5px; height: 100%" align="left">
                                        <legend style="font-weight: bold; font-size: 11px; width: 120px; font-family: Arial;
                                            height: 16px" name="lgd1">Cập nhật ảnh lên Server .... </legend>
                                        <table cellpadding="1" cellspacing="1" border="1" width="100%">
                                            <tr>
                                                <td class="Time">
                                                    <asp:Label ID="lblResult" runat="server"></asp:Label></td>
                                            </tr>
                                          
                                            <tr>
                                                <td>
                                                   <input class="inputtext" id="txtFile" style="width: 80%; height: 22px" type="file"
                                                        size="52" name="txtFile" runat="server" />
                                                    <asp:Button CssClass="inputtext" ID="btnUpload" Text="Upload" runat="server" Width="67"
                                                        Height="24"></asp:Button>
                                                    <input class="inputtext" id="btnExit" style="width: 67px; height: 30px" type="button"
                                                        value="Đóng" name="btnExit" onclick="javascript:window.close();" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
