<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImgShow.aspx.cs" Inherits="prjApplication.Until.ImgShow" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View image</title>
</head>
<body style="text-align: center;">
    <div>
        <img src='<%=Request["img"]%>' style="border: 0px;" />
    </div>
</body>
</html>
