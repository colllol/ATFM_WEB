<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginPlayAudio.aspx.cs"
    Inherits="prjApplication.Ajax.PluginPlayAudio" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>

    <script src="../Scripts/Player/jwplayerLive.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MediaPlayer">
        <div id="liveTVContent">
        </div>

        <script type="text/javascript">
            jwplayer("liveTVContent").setup({
                flashplayer: 'http://<%=Request.Url.Host%><%=Request.ApplicationPath%>/Scripts/Player/player.swf',
                image: '<%=_urlImage%>',
                file: '<%=_urlFile%>',
                width: 480,
                height: 20,
                autostart: false,
                allowscriptaccess: true,
                allowfullscreen: true,
                controlbar: 'over'
            });
                     
        </script>

    </div>
    </form>
</body>
</html>
