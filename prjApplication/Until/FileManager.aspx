<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FileManager.aspx.cs" Inherits="prjApplication.Until.FileManager" %>

<%@ Import Namespace="prjComponents" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quản lý file</title>
    <link rel="Stylesheet" type="text/css" href="scripts/uploadify.css" />

    <script type="text/javascript" src="../Scripts/AutoFormatDatTime.js"></script>

    <script src="scripts/jquery-1.7.2.min.js" type="text/javascript" />

    <script src="jscrop/js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../scripts/watermark/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="../scripts/watermark/jquery-ui-1.8.6.custom.min.js"></script>

    <script type="text/javascript" src="../scripts/watermark/jquery-watermarker-0.3.js"></script>

    <script type="text/javascript" src="jscrop/js/jquery.Jcrop.js"></script>

    <script type="text/javascript" src="scripts/jquery.uploadify-3.1.min.js"></script>

    <script type="text/javascript" src="scripts/jquery.uploadify.js"></script>

    <link type="text/css" rel="Stylesheet" href="../Style/style.css" />
    <link href="jscrop/css/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        div.watermark
        {
            border: 1px dashed #000;
        }
        img.watermark:hover
        {
            cursor: move;
        }
        .watermark
        {
            left: 0;
        }
    </style>

    <script type="text/javascript">
        function getImgSrc(theImagePath, numberAgr, Size, FileExtension) {
            if (theImagePath != "") {
                window.close();
                opener.getPath(theImagePath, numberAgr, Size, FileExtension);

            }
        }
        
        function PreviewImage(theImagePath, numberAgr, Size, FileExtension) {
            if (theImagePath != "") {
                document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + theImagePath + "','" + numberAgr + "','" + Size + "','" + FileExtension + "');\"  src=\"<%=prjComponents.Global.TinPath%><%=prjComponents.Global.UploadPath%>" + theImagePath + "\" />";
                document.getElementById("lblFilename").innerHTML = "Thư mục: <%=prjComponents.Global.UploadPath%>" + theImagePath;
                $('#<%=txt_UrlImage.ClientID %>').val(theImagePath);
            }
        }
        function confirm_deleteFile() {
            var stringMess = "";
            stringMess = 'Bạn có thực sự muốn xóa thư các file đã chọn không?';
            //alert(stringMess);
            if (confirm(stringMess) == true)
                return true;
            else
                return false;
        }

        function positionSetup() {
            $('#imgCrop').Watermarker({
                watermark_img: 'watermark.png',
                opacity: 0.5,
                opacitySlider: $('#sliderdiv'),
                position: 'center',
                onChange: showCoords
            });
        }   
    </script>

    <script type="text/javascript">
        function showCoords(c) {
            //                $('#X1').val(c.x);
            //                $('#Y1').val(c.y);
            $('#X11').val(c.x);
            $('#Y11').val(c.y);
            //                $('#w1').val(c.w);
            //                $('#h1').val(c.h);
            //                $('#a').val(c.opacity);
        };
        //-->
    </script>

    <script type="text/javascript">
        function storeCoords(c) {

            jQuery('#X').val(c.x);

            jQuery('#Y').val(c.y);

            jQuery('#W').val(c.w);

            jQuery('#H').val(c.h);
        };
        function getImageCrop(_img) {
            document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + _img + "','1','666666','.jpg');\"  src=\"<%=prjComponents.Global.UploadPath%>" + _img + "\" />";
            $('#<%=txt_UrlImage.ClientID %>').val(_img);
        };
        function setSelection(x, y) {
            var _rong = 300;
            var _cao = 165;
            var _cropicon = 1; //<%=Request["cropicon"]%>;
            if (_cropicon == 1) {
                _rong = x;
                _cao = y;
            }

            if (document.getElementById("imgCrop").src.length > 40) {
                jQuery('#imgCrop').Jcrop({
                    onSelect: storeCoords,
                    setSelect: [_rong, _cao, 0, 0]
                });
            }
            else {
                alert("Bạn chưa chọn ảnh để crop. Mời bạn chọn ảnh trước sau đó mới lựa chọn crop."); return false;
            }
        }

        function cropimage(UserID) {
            var URL_Get;
            if (document.getElementById("imgCrop").src.length > 40) {
                if (parseInt($("#<%=W.ClientID%>").val()) > 0) {
                    URL_Get = 'CropImage.ashx?user=' + UserID + '&vType=<%=GetvType()%>&img=' + document.getElementById("imgCrop").src + '&x=' + $("#<%=X.ClientID%>").val() + '&y=' + $("#<%=Y.ClientID%>").val() + '&w=' + $("#<%=W.ClientID%>").val() + '&h=' + $("#<%=H.ClientID%>").val();
                    if (URL_Get != undefined) {
                        var xmlHttp = CreateAjax();
                        xmlHttp.open("GET", URL_Get, true);
                        xmlHttp.send(null);
                    }
                    xmlHttp.onreadystatechange = function() {
                        if (xmlHttp.readyState == 4) {
                            window.location = 'FileManager.aspx?vType=2';
                            document.getElementById("imgCrop").style.display = 'block';
                            $('#<%=txt_UrlImage.ClientID %>').val(xmlHttp.responseText);
                        }
                    }
                    return false;

                }
                else { alert("Bạn chưa chọn vùng crop!"); return false; }
            }
            else {
                alert("Bạn chưa chọn ảnh!"); return false;
            }
        }
        function CreateAjax() {
            //#region
            var XmlHttp;
            //Creating object of XMLHTTP in IE
            try {
                XmlHttp = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e) {
                try {
                    XmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (oc) {
                    XmlHttp = null;
                }
            }
            //Creating object of XMLHTTP in Mozilla and Safari
            if (!XmlHttp && typeof XMLHttpRequest != "undefined") {
                XmlHttp = new XMLHttpRequest();
            }
            return XmlHttp;
            //#endregion
        }
    </script>

</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 5px">
    <form id="frmUpLoad" method="post" enctype="multipart/form-data" runat="server">
    <cc2:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
    <table border="0" cellpadding="2" width="840px" cellspacing="0">
        <tr>
            <td>
                <div>
                    <span class="TitlePanel">+ QUẢN LÝ FILES</span>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div>
                    <table style="width: 840px;" cellpadding="2" cellspacing="2" border="0">
                         <!--<tr>
                            <td class="Titlelbl" style="text-align: right;">
                                Chuyên trang:
                            </td>
                            <td class="Titlelbl" style="text-align: left;">
                                <asp:DropDownList ID="Drop_Lang" Width="130px" CssClass="inputtext" runat="server"
                                    AutoPostBack="True" OnSelectedIndexChanged="Drop_Lang_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="Titlelbl" style="text-align: right;">
                               Chuyên mục:
                            </td>
                            <td class="Titlelbl" style="text-align: left;">
                                <asp:DropDownList ID="Drop_Chuyenmuc" runat="server" Width="300px" CssClass="inputtext">
                                </asp:DropDownList>
                            </td>
                        </tr>-->
                        <tr>
                            <td class="Titlelbl" style="text-align: right;">
                                Từ ngày:
                            </td>
                            <td style="text-align: left;">
                                <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../scripts/DatePicker/Images"
                                    Width="100px" ScriptSource="../Scripts/datepicker.js" ID="txt_FromDate" CssClass="inputtext"
                                    runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                    onKeyUp="DateFormat(this,this.value,event,false,'3')" MaxLength="10" onblur="DateFormat(this,this.value,event,true,'3')">
                                </nbc:NetDatePicker>
                            </td>
                            <td class="Titlelbl" style="text-align: right;">
                                Đến ngày:
                            </td>
                            <td style="text-align: left;">
                                <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../Scripts/DatePicker/Images"
                                    CssClass="inputtext" Width="100px" ScriptSource="../Scripts/datepicker.js" ID="txt_ToDate"
                                    runat="server" onKeyPress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                    onKeyUp="DateFormat(this,this.value,event,false,'3')" onblur="DateFormat(this,this.value,event,true,'3')"
                                    MaxLength="10">
                                </nbc:NetDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="Titlelbl" style="text-align: right;">
                                Tên file:
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTenfile" Width="200px" CssClass="inputtext"></asp:TextBox>
                            </td>
                            <td>
                                <asp:DropDownList CssClass="inputtext" runat="server" ID="ddlStock" Width="150px"
                                    AutoPostBack="true" onclick="javascript:run_upload();" OnSelectedIndexChanged="ddlStock_OnSelectedIndexChanged">
                                    <asp:ListItem Value="0">Ảnh hàng ngày</asp:ListItem>
                                    <asp:ListItem Value="1">Ảnh trong kho</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="Button1" runat="server" Text="Tìm kiếm" CssClass="iconFind" OnClick="btnSearch_Click">
                                </asp:Button>
                                <asp:Button ID="Button2" runat="server" Text="Đóng" CssClass="iconThoat" OnClientClick="window.close();">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr style="size: 1; background-color: #f1f1f1" />
                <table border="0" style="width: 100%">
                    <tr>
                        <td style="width: 90%">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
                        </td>
                    </tr>
                </table>
                <div class="classSearchHeader" id="displayContainer">
                    <table style="width: 100%" cellpadding="2" cellspacing="2" border="1">
                        <tr>
                            <td style="text-align: left; width: 30%;">
                                <span class="TitlePanel">+ ẢNH ĐÃ CẬP NHẬT</span>
                            </td>
                            <td>
                                <span class="Title">Chọn vùng Crop:</span> <span>
                                    <input type="button" value="Nổi bật trang chủ" id="btnSetSelection" onclick="setSelection(665,448);"
                                        class="iconCropRegion" />
                                    <input type="button" value="Nổi bật chuyên mục" id="Button3" onclick="setSelection(300,200);"
                                        class="iconCropRegion" />
                                    <input type="button" value="Thumbnail" id="Button4" onclick="setSelection(150,100);"
                                        class="iconCropRegion" />
                                </span>
                                <asp:HiddenField ID="X" runat="server" />
                                <asp:HiddenField ID="Y" runat="server" />
                                <asp:HiddenField ID="W" runat="server" />
                                <asp:HiddenField ID="H" runat="server" />
                                <asp:HiddenField ID="X11" runat="server" />
                                <asp:HiddenField ID="Y11" runat="server" />
                                <input type="button" value="Crop ảnh" id="btnCrop" onclick="cropimage('<%=GetUserID()%>');"
                                    class="iconCrop" /><asp:TextBox ID="txt_UrlImage" runat="server" Style="display: none"></asp:TextBox>
                                <br />
                                <input type="button" value="Chọn vị trí đóng dấu" id="btnPosWaterMark" class="iconWaterMark"
                                    onclick="positionSetup();" />
                               
                                <asp:Button ID="cmd_watermark" runat="server" CssClass="iconWaterMarkSave" Text="Lưu đóng dấu"
                                    OnClick="cmd_watermark_Click" />
                                <!--</span>-->
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left; width: 30%; vertical-align: top">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="showListImages" id="divListImages">
                                            <asp:DataList ID="dlImages" DataKeyField="ID" runat="server" RepeatColumns="3" RepeatDirection="Horizontal"
                                                RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                                ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset" ItemStyle-Width="20%"
                                                ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2" OnEditCommand="dlImages_EditCommand">
                                                <ItemTemplate>
                                                    <table border="0" cellspacing="1" cellpadding="1" width="100%" style="background-color: #f1f1f1">
                                                        <tr>
                                                            <td style="text-align: center; height: 100px; width: 100px; vertical-align: middle">
                                                                <img alt="<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>"
                                                                    src="<%#GetFileURL(DataBinder.Eval(Container.DataItem,"ImgeFilePath"))%>" border="1"
                                                                    width="80px" height="60px" style="cursor: pointer;" title="<%#DataBinder.Eval(Container.DataItem,"ImageFileName")%>"
                                                                    onclick="PreviewImage('<%#UrlPathImage_RemoveUpload(DataBinder.Eval(Container.DataItem,"ImgeFilePathOrizin"))%>','<%=strNumberArg%>','<%#DataBinder.Eval(Container.DataItem,"ImageFileSize")%>','<%#DataBinder.Eval(Container.DataItem,"ImageFileExtension")%>')">
                                                                <br />
                                                                <asp:Label ID="lbl_URL" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ImgeFilePath")%>'></asp:Label>
                                                                <asp:Label ID="lbl_ID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ID")%>'></asp:Label>
                                                                <asp:Label ID="lblFilename" Width="80px" runat="server" Text='<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>'></asp:Label>
                                                                <br />
                                                                <asp:ImageButton ID="ImageButton_delete" OnClientClick="if (confirm('Bạn có chắc muốn xóa. Có thể có bài viết liên quan?')) return true; else return false;"
                                                                CommandName="edit" runat="server" ImageUrl="~/Images/cancel.gif" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:DataList>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="classSearchHeader_Noborder" style="width: 96%; text-align: center">
                                    <asp:Button ID="cmdPrev" runat="server" Text=" << " CssClass="iconNext" OnClick="cmdPrev_Click">
                                    </asp:Button>&nbsp;
                                    <asp:Label ID="lblCurrentPage" runat="server"></asp:Label>
                                    <asp:Button ID="cmdNext" runat="server" Text=" >> " CssClass="iconNext" OnClick="cmdNext_Click">
                                    </asp:Button>
                                </div>
                            </td>
                            <td style="width: 70%; vertical-align: top; text-align: center;">
                                <div class="classSearchHeader" style="width: 97%; text-align: left">
                                    <span class="Titlelbl">
                                        <label id="lblFilename" runat="server">
                                        </label>
                                    </span>
                                </div>
                                <hr style="height: 1px; background-color: #f1f1f1; border: 0px;" />
                                <div id="ContainerCrop" runat="server">
                                    <img id="imgCrop" onclick="getImgSrc(this.src,'2','6666','.jpg');" src="" style="display: none">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>

<script type="text/javascript">
    
    $(window).load(

         run_upload()
        );
    function run_upload() {

        var _stock = document.getElementById("ddlStock");
        var _strStock = _stock.options[_stock.selectedIndex].value;

        $("#<%=FileUpload1.ClientID %>").uploadify({
            'swf': 'scripts/uploadify.swf',
            'buttonText': 'Nhập ảnh',
            'uploader': 'UploadImg.ashx?user=<%=GetUserName()%>&inStock=' + _strStock,
            'folder': 'Upload',
            'fileDesc': 'Images Files',
            'fileExt': '*.jpg;*.png;*.gif;*.jpeg;',
            'multi': true,
            'auto': true,
            'onQueueComplete': function(queueData) {
                  doPostBackAsync('UploadSuccess', '');
            }
        });
    }

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

