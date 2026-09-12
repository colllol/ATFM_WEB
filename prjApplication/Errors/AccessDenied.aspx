<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccessDenied.aspx.cs" Inherits="prjApplication.Errors.AccessDenied" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<%@ import namespace="prjComponents" %>
<head>
    <title>THÔNG BÁO</title>

    <script language="javascript">
        function Back() {
            top.location = "<%=Global.ApplicationPath%>";
        }
    </script>

    <style>
        
        .linkThongBao
        {
            text-decoration: none;
            color: Blue;
        }
        .linkALL
        {
            font: 15px/22px arial,sans-serif;
        }
        fieldset
        {
            width:50%;
            -moz-border-radius: 5px;
            border-radius: 5px;
            -webkit-border-radius: 5px;
            margin: 10% auto 0;
            padding: 30px 0 15px;
        }
       
        #t{color:Red;font: 18px/22px arial,sans-serif;}
       
    </style>
</head>
<body class="linkALL" onload="setTimeout('Back()',5000);">
    <fieldset>
      
        <table style="border: solid 0px; width: 100%">
            <tr>
                <td >
                    <span style="font-weight: bold; color: Red; text-align:right;"></span>
                    <p>
                        <span id="t">THÔNG BÁO</span><br />
                        Có lỗi xảy ra hoặc tài nguyên của bạn bị từ chối truy cập<br />
                        Click vào link sau quay về trang đăng nhập nếu bạn không muốn đợi lâu<br />
                        <br />
                        <a class="linkThongBao" href="<%=Global.ApplicationPath%>">Trang chủ</a>
                </td>
                <td style="vertical-align: baseline;">
                    <img src="<%=Global.ApplicationPath%>/Images/IconHPC/accessDenied.jpg" />
                </td>
            </tr>
            <td>
            </td>
        </table>
    </fieldset>
</body>
</html>

