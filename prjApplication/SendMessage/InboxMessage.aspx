<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="InboxMessage.aspx.cs" Inherits="prjApplication.SendMessage.InboxMessage" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />

        <div class="row">
        <span class="TitlePanel">SEND MESSAGE</span>
    </div>
    <div class="row">
        <label>Group</label>
        <select id="ddlGroupAddress" onchange="ddlGroupAddress_Onchange()">
            <option value=" ">-- [Select] --</option>
            <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                <option value="<%: r["ADDRESS"] %>"><%: r["GROUP_NAME"] %></option>
               <%  } %>
        </select>
        <button id="btnSend" onclick="btnSend_Onclick()" class="btn btn-primary btn-sm" type="button">Send</button>
    </div>
    <div class="row">
        Address
    </div>
    <div class="row">
        <textarea id="txtAddress" class="wid_70" rows="3"></textarea>
    </div>
    <div class="row">
        Origin
    </div>
    <div class="row">
        <input id="txtOrigin" class="wid_250px" value="080901 VTBBZDZX" />
    </div>
    <div class="row">
        Content
    </div>
    <div class="row">
        <textarea id="txtContent" class="wid_70" rows="15"></textarea>
    </div>

    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
        var phanCachArg = '<%= _phanCachArg%>';
        function ddlGroupAddress_Onchange() {
            var v = $('#txtAddress').val()
            if(v=='')v='FF '
            var n = document.getElementById('ddlGroupAddress').options[document.getElementById('ddlGroupAddress').selectedIndex].value
            $('#txtAddress').val(v + ' ' + n )
        }
        function btnSend_Onclick() {
            var obj = {
                P_OT_CONT: $('#txtContent').val()
            };
            GetArgWithPostBack(JSON.stringify(obj) + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
        }
        function DisplayResult(resulf, context) {
            if (context == 'btnSend_Onclick') {
                alert(resulf);
            }
            else if (context == 'RenderTopBarInfo') {
                $('#divExcuteScript').html(resulf);
            }            
        }

    </script>
</asp:Content>

