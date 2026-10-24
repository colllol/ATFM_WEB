<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WaterMark.aspx.cs" Inherits="prjApplication.ckeditor.plugins.WaterMark.WaterMark" %>

<%@ Import Namespace="prjComponents" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Đóng dấu ảnh</title>

    <script src="../../../Until/scripts/jquery-1.7.2.min.js" type="text/javascript" />

    <script src="../../../Until/jscrop/js/jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" src="../../../Scripts/watermark/jquery-1.4.2.min.js"></script>

    <script type="text/javascript" src="../../../Scripts/watermark/jquery-ui-1.8.6.custom.min.js"></script>

    <script type="text/javascript" src="../../../Scripts/watermark/jquery-watermarker-0.3.js"></script>

    <script type="text/javascript" src="../../../Until/jscrop/js/jquery.Jcrop.js"></script>

    <link type="text/css" rel="Stylesheet" href="../../../Style/style.css" />
    <link href="../../../Until/jscrop/css/jquery.Jcrop.css" rel="stylesheet" type="text/css" />
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
                stringImg = "<img id=\"" + numberAgr + "\" style=\"cursor-pointer\" hspace=\"3\" vspace=\"3\" src=\"<%=prjComponents.Global.UploadPath%>" + theImagePath + "\" border =\"0\">";
                window.close();
                InsertImg(stringImg);

            }
        }
        function InsertImg(strText) {
            var browserName = navigator.appName;
            var id = '<%=Request["editorID"] %>';
            window.opener.InsertImage(id, strText);
            window.close();
        }
        function setLink(url) {
            var _pathNew = '<%=prjComponents.Global.UploadPath%>' + url;
            window.close();
            window.opener.CKEDITOR.tools.callFunction(2, _pathNew, function() {
                // Get the reference to a dialog window.
                var element, dialog = this.getDialog();
                // Check if this is the Image dialog window.
                if (dialog.getName() == 'link') {
                    // Get the reference to a text field that holds the "title" attribute.
                    element = dialog.getContentElement('advanced', 'advTitle');
                    if (element) {
                        element.setValue(trimTitle($('f_title').value));
                    }
                }
            });

        }
        function BackImage(theImagePath, numberAgr, Size, FileExtension) {
            if (theImagePath != "") {
                document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + theImagePath + "','" + numberAgr + "','" + Size + "','" + FileExtension + "');\"  src=\"<%=prjComponents.Global.UploadPath%>" + theImagePath + "\" />";
                document.getElementById("lblFilename").innerHTML = "Thư mục: <%=prjComponents.Global.UploadPath%>" + theImagePath;
                $('#<%=txt_UrlImage.ClientID %>').val(theImagePath);
            }
        }
        function PreviewImageAjax() {
            var URL_Get;
            var X11 = $('#<%=X11.ClientID %>').val();            
            var Y11 = $('#<%=Y11.ClientID %>').val();            
            URL_Get = '../../../Until/ImagePropertiesWark.ashx?user=<%=GetUserID()%>&vType=<%=GetvType()%>&img=' + document.getElementById("imgCrop").src + '&vposX=' + X11 + '&vposY=' + Y11;
            if (URL_Get != undefined) {
                var xmlHttp = CreateAjax();
                xmlHttp.open("GET", URL_Get, true);
                xmlHttp.send(null);
            }
            xmlHttp.onreadystatechange = function() {
                if (xmlHttp.readyState == 4) {                    
                    document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + xmlHttp.responseText + "','1','666666','.jpg');\"  src=\"<%=prjComponents.Global.UploadPath%>" + xmlHttp.responseText + "\" />";
                    document.getElementById("lblFilename").innerHTML = "Thư mục: <%=prjComponents.Global.UploadPath%>" + xmlHttp.responseText;
                    $('#<%=txt_UrlImage.ClientID %>').val(xmlHttp.responseText);
                }
            }
            return false;

        }
        function PreviewImage(theImagePath, numberAgr, Size, FileExtension) {
            if (theImagePath != "") {
                document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + theImagePath + "','" + numberAgr + "','" + Size + "','" + FileExtension + "');\"  src=\"<%=prjComponents.Global.UploadPath%>" + theImagePath + "\" />";
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
            if (document.getElementById("imgCrop").src.length > 55) {
                if (parseInt($("#<%=W.ClientID%>").val()) > 0) {
                    URL_Get = '../../../Until/CropImage.ashx?user=' + UserID + '&vType=<%=GetvType()%>&img=' + document.getElementById("imgCrop").src + '&x=' + $("#<%=X.ClientID%>").val() + '&y=' + $("#<%=Y.ClientID%>").val() + '&w=' + $("#<%=W.ClientID%>").val() + '&h=' + $("#<%=H.ClientID%>").val();
                    if (URL_Get != undefined) {
                        var xmlHttp = CreateAjax();
                        xmlHttp.open("GET", URL_Get, true);
                        xmlHttp.send(null);
                    }
                    xmlHttp.onreadystatechange = function() {
                        if (xmlHttp.readyState == 4) {
                            document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + xmlHttp.responseText + "','1','666666','.jpg');\"  src=\"<%=prjComponents.Global.UploadPath%>" + xmlHttp.responseText + "\" />";
                            document.getElementById("lblFilename").innerHTML = "Thư mục: <%=prjComponents.Global.UploadPath%>" + xmlHttp.responseText;
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
            $('#X11').val(c.x);
            $('#Y11').val(c.y);
        };
         
    </script>

</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 5px">
    <form id="frmUpLoad" method="post" enctype="multipart/form-data" runat="server">
    <cc2:ToolkitScriptManager runat="Server" EnablePartialRendering="true" ID="ScriptManager1" />
    <table border="0" cellpadding="2" width="900px" cellspacing="0">
        <tr>
            <td>
                <div>
                    <span class="TitlePanel">+ ĐÓNG DẤU ẢNH</span>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="classSearchHeader" id="displayContainer">
                    <table style="width: 100%" cellpadding="2" cellspacing="2" border="1">
                        <tr>
                            <td colspan="2">
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
                                <!--<span class="Title">Đóng dấu: </span><span>
                                    <asp:DropDownList CssClass="inputtext" runat="server" ID="DropStyle" Width="100px">
                                        <asp:ListItem Value="1">Giữa</asp:ListItem>
                                        <asp:ListItem Value="2">Trên trái</asp:ListItem>
                                        <asp:ListItem Value="3">Trên phải</asp:ListItem>
                                        <asp:ListItem Value="4">Dưới trái</asp:ListItem>
                                        <asp:ListItem Value="5">Dưới phải</asp:ListItem>
                                    </asp:DropDownList>-->
                                <input type="button" value="Chọn vị trí đóng dấu" id="btnPosWaterMark" class="iconWaterMark"
                                    onclick="positionSetup();" />
                                <input id="cmd_watermark" value="Lưu đóng dấu" type="button" onclick="PreviewImageAjax();" class="iconWaterMarkSave" />
                                <!--</span>-->
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; vertical-align: top; text-align: center;">
                                <div id="ContainerSave" style="width: 90px;" runat="server">
                                    <img id="imgSave" src="" style="display: none;" />
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
                                <div id="ContainerCrop" style="width: 880px; height: 500px;" runat="server">
                                    <img id="imgCrop" onclick="getImgSrc(this.src,'1','6666','.jpg');" src="" style="display: none" />
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
    function SelectFile() {
        //var _strPath = window.opener.document.getElementById("cke_122_textInput").value;
        var _path = '<%=Request["objtext"]%>';
        var _strPath = _path; //window.opener.document.getElementById(_path).value;        
        var _strWithout = _strPath.replace("/upload/", "");
        document.getElementById("ContainerCrop").innerHTML = "<img style=\"cursor:pointer;\" id=\"imgCrop\" onclick=\"getImgSrc('" + _strWithout + "','1','666666','.jpg');\"  src=\"" + _strPath + "\" />";
        document.getElementById("lblFilename").innerHTML = "Thư mục: " + _strPath;
        $('#<%=txt_UrlImage.ClientID %>').val(_strWithout);

        document.getElementById("ContainerSave").innerHTML = "<img style=\"cursor:pointer;max-width:90px;\" id=\"imgSave\" onclick=\"BackImage('" + _strWithout + "','1','666666','.jpg');\"  src=\"" + _strPath + "\"/>";
    }

    $(document).ready(function() {

        SelectFile();
    });
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

