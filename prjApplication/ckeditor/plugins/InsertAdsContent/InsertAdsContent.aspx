<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertAdsContent.aspx.cs" Inherits="prjApplication.ckeditor.plugins.InsertAdsContent.InsertAdsContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Chèn quảng cáo vào nội dung</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link type="text/css" rel="Stylesheet" href="../../../Style/style.css" />
    <style type="text/css">.cssvalueyoutube{height:200px; width:390px; margin-top:5px;}</style>
    <script type="text/javascript">
        function InsertAds() {
            var strText = document.getElementById('idvalueads').value;
            var id = '<%=Request["editorID"] %>';
            window.opener.InsertImage(id, strText);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div>
            <textarea id="idvalueads" class="cssvalueyoutube" ></textarea>
            <input class="buttonYoutube" type="button" onclick="InsertAds();" />
        </div>
    </center>
    </form>
</body>
</html>
