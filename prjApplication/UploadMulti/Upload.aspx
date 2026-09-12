<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="prjApplication.UploadMulti.Upload" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UpLoad</title>
    <link rel="Stylesheet" type="text/css" href="../Until/scripts/uploadify.css" />
    <link type="text/css" rel="Stylesheet" href="../Style/style.css" />

    <script src="../Until/scripts/jquery-1.7.2.min.js" type="text/javascript"></script>

    <script src="../Until/scripts/jquery.uploadify-3.1.min.js" type="text/javascript"></script>

    <script src="../Until/scripts/jquery.uploadify.js" type="text/javascript"></script>

    <script type="text/javascript" src="../Until/scripts/jquery.uploadify.js"></script>

    <script type="text/javascript">
        function getImgSrc(theImagePath, numberAgr, numberID) {            
            if (theImagePath != "") {
                	
                window.close();
                opener.getPath(theImagePath, numberAgr, numberID);
            }
        }

       
    </script>

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
        .inputtextFileUpload
        {
            border-right: #cccccc 1px solid;
            border-top: #cccccc 1px solid;
            font-weight: normal;
            font-size: 12px;
            border-left: #cccccc 1px solid;
            cursor: pointer;
            color: #000000;
            border-bottom: #cccccc 1px solid;
            width: 80%;
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
            font-size: 10pt;
            padding-bottom: 1px;
            margin: 0px;
            overflow: auto;
            width: 100%;
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
</head>
<body style="margin-top: 5px; margin-left: 5px; margin-right: 5px; margin-bottom: 5px">
    <form id="frmUpLoad" method="post" enctype="multipart/form-data" runat="server">
    <table border="0" cellpadding="0" width="100%" cellspacing="0">
        <tr>
            <td class="datagrid_top_left">
            </td>
            <td class="datagrid_top_center">
                <span class="TitlePanel">+ QUẢN LÝ FILES</span>
            </td>
            <td class="datagrid_top_right">
            </td>
        </tr>
        <tr>
            <td class="datagrid_content_left">
            </td>
            <td style="text-align: center">
                <table cellpadding="1" cellspacing="1" border="0" width="100%">
                    <tr>
                        <td align="center" style="width: 100%" colspan="2">
                            <table style="background-color: #F9F9F9; border: solid 1px #F2F2F2; width: 100%;">
                                <tr>
                                    <td class="Titlelbl">
                                        Tên file:
                                    </td>
                                    <td class="Titlelbl">
                                        <asp:TextBox ID="fileName" runat="server" CssClass="inputtext" Width="90%"></asp:TextBox>
                                    </td>
                                    <td class="Titlelbl">
                                        Từ ngày:
                                    </td>
                                    <td>
                                        <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../scripts/DatePicker/Images"
                                            CssClass="inputtextDate" Width="100px" ScriptSource="../Scripts/datepicker.js"
                                            ID="txt_FromDate" runat="server" onkeypress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onblur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                    </td>
                                    <td class="Titlelbl">
                                        Đến ngày:
                                    </td>
                                    <td>
                                        <nbc:NetDatePicker ImageUrl="../Images/events.gif" ImageFolder="../Scripts/DatePicker/Images"
                                            CssClass="inputtextDate" Width="100px" ScriptSource="../Scripts/datepicker.js"
                                            ID="txt_ToDate" runat="server" onkeypress="AscciiDisable()" onfocus="javascript:vDateType='3'"
                                            onkeyup="DateFormat(this,this.value,event,false,'3')" onblur="DateFormat(this,this.value,event,true,'3')"></nbc:NetDatePicker>
                                    </td>
                                    <td class="Time">
                                        <asp:Button ID="Button1" runat="server" Text="Tìm kiếm" CssClass="iconFind" OnClick="btnSearch_Click">
                                        </asp:Button>
                                        <input class="iconThoat" id="btnExit" type="button" value="Đóng" name="btnExit" onclick="javascript: window.close();" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left; height: 25px; padding-left: 4px" bgcolor="#cccccc" class="TitlePanel">
                            + DANH SÁCH CÁC FILE ĐƯỢC CẬP NHẬT
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center">
                            <div id="displayContainer">
                                <asp:DataList ID="dlImages" DataKeyField="ID" runat="server" RepeatColumns="5" RepeatDirection="Horizontal"
                                    RepeatLayout="Table" Width="100%" ItemStyle-HorizontalAlign="Center" ItemStyle-BorderWidth="0"
                                    ItemStyle-BackColor="#ffffff" ItemStyle-BorderStyle="Inset" ItemStyle-Width="20%"
                                    ItemStyle-VerticalAlign="Middle" CellSpacing="2" CellPadding="2" OnEditCommand="dlImages_EditCommand">
                                    <ItemTemplate>
                                        <table border="0" cellspacing="0" cellpadding="0" width="100%" style="background-color: #f1f1f1">
                                            <tr>
                                                <td style="text-align: center; padding: 5px 5px 5px 5px; vertical-align: middle">
                                                    <img alt="<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>"
                                                        src="<%#GetFileURL(DataBinder.Eval(Container.DataItem,"ImgeFilePath"))%>" border="1"
                                                        width="80px" height="60px" style="cursor: pointer;" title="<%#DataBinder.Eval(Container.DataItem,"ImageFileName")%>"
                                                        onclick="getImgSrc('<%#UrlPathImage_RemoveUpload(DataBinder.Eval(Container.DataItem,"ImgeFilePathOrizin"))%>','<%=strNumberArg%>','<%#DataBinder.Eval(Container.DataItem,"ID")%>','<%#DataBinder.Eval(Container.DataItem,"ImageFileExtension")%>')">
                                                    <br />
                                                    <!--<asp:Label ID="lbl_URL" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ImgeFilePath")%>'></asp:Label>
                                                    <asp:Label ID="lbl_ID" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container.DataItem,"ID")%>'></asp:Label>
                                                    <br />-->
                                                    <asp:Label ID="lblFilename" Width="80px" runat="server" Text='<%#Cut_Filename(DataBinder.Eval(Container.DataItem,"ImageFileName"))%>'></asp:Label>
                                                    <asp:ImageButton ID="ImageButton_delete" CommandName="edit" runat="server" ImageUrl="~/Images/cancel.gif" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left">
                            <asp:FileUpload ID="FileUpload1" runat="server" />
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

<script type="text/javascript">

    $(window).load(

         run_upload()
        );
    function run_upload() {

        $("#<%=FileUpload1.ClientID %>").uploadify({
            'swf': '../Until/scripts/uploadify.swf',
            'buttonText': 'Nhập File',
            'uploader': 'Upload.ashx?user=<%=GetUserName()%>',
            'folder': 'Upload',
            'fileDesc': 'Images Files',
            'fileExt': '*.jpg;*.png;*.gif;*.jpeg;*.flv;*.swf;*.flv;*.mp4;*.mpg;*.avi;*.avi;',
            'multi': true,
            'auto': true,
            'onQueueComplete': function (queueData) {
                var url = '../UploadMulti/Upload.aspx?vType=<%=GetvType()%>&vKey=<%=GetvKey()%>';
                window.location = url;
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