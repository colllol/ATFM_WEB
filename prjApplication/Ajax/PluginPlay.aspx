<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PluginPlay.aspx.cs" Inherits="prjApplication.Ajax.PluginPlay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src='../Scripts/jwplayer/jwplayer.js'></script>

    <script language="javascript" type="text/javascript">        jwplayer.key = "4cFrCsBdTSWp87XH5zQW4VsWi+mFFzQIIqiC4kpnEoU="</script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="MediaPlayer">
        <div id="liveTVContent">
        </div>

        <script type="text/javascript">
            jwplayer("liveTVContent").setup({
                image: '<%=_urlImage%>',
                file: '<%=_urlFile%>',
                width: 480,
                height: 330,
                primary: "flash"
            });
                     
        </script>

    </div>
    </form>
</body>
</html>
