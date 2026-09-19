<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ViewImages.aspx.cs" Inherits="prjApplication.Until.ViewImages" %>

<html>
<head>
    <title>Kích thước thật của ảnh</title>

</head>

<script language="javascript">
	function DisableRightClick()
	{
	 document.oncontextmenu = function(){return false} 
	}
	function close_windown()
	{
		var tmp_Window = window.close();
    }
    function FixCenter() {
        if (window.innerWidth) {
            iWidth = window.innerWidth;
            iHeight = window.innerHeight;
        } else {
            iWidth = document.body.clientWidth;
            iHeight = document.body.clientHeight;
        }
        iWidth = document.images[0].width - iWidth;
        iHeight = document.images[0].height - iHeight;
        window.resizeBy(iWidth, iHeight);
        window.moveTo(window.screen.width / 2 - (document.body.clientWidth / 2), window.screen.height / 2 - (document.body.clientHeight / 2));
    }
	
</script>
<body bottommargin="0" leftmargin="0" rightmargin="0" topmargin="0" bgcolor="#e7e8eb" onload="return FixCenter();">
    <form id="Form1" method="post" runat="server">
        <table cellpadding="0" cellspacing="0" border="0" width="100%">
            <tr>
                <td>
                    <a href="javascript:close_windown();">
                        <img src="<%=Imagescr%>" border="0" style="border: 1px solid #a9a9a9"></a>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
