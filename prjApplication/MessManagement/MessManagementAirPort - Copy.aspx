<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="MessManagementAirPort.aspx.cs" Inherits="prjApplication.MessManagement.MessManagementAirPort" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="prjBusinessLogic" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/StyleReports.css" rel="stylesheet" />
    <link href="../Scripts/DateTimeJQ/jquery.datetimepicker.css" rel="stylesheet" />    
    <script src="../Scripts/DateTimeJQ/jquery.datetimepicker.js"></script>
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
     <style type="text/css">
        .w3-example {
        background-color: #f1f1f1;
       
    }
       
   </style>
   


   <div class="container-fluid"> 
    <div class="row w3-example" style="margin-bottom:20px;">
       <div class="col-xs-3"></div>
       <div class="row" style="text-align:center"> 
        <input id="txtBEGINDATE" class="datepicker" autocomplete="off" runat="server"
                                 onkeypress='return check_num(this,14,event)' type="text" />          
        <asp:Button id="btnOption" runat="server" OnClick="btnSelect_Click" Text="Select" class="btn btn-primary btn-sm" type="button"></asp:Button>
      </div>               
   </div>
        </div>
       <div class="row">
   <div class="col-xs-2 w3-example" style="width: 220px; height: 700px; overflow-y: scroll;">
     <div class="scroll-area1">
           <div class="table-responsive">
           <table id="tblSource" class="table table-bordered table-responsive">
            <thead>
                <tr>
                 <th>Part No</th>
                  <th>Message Type</th>
                </tr>                
            </thead>
             <tbody>
                  <asp:Repeater ID="grdListPartMessage" runat="server">
                        <ItemTemplate>
                        <tr>
                            <td style="width:20%;text-align:center"><%# Eval("PART_NO") %></td>
                            <td style="width:80%;text-align:center" id="<%# Eval("MESS_TYPE") %><%# Eval("PART_NO") %>">
                                <a style="cursor: pointer; color: <%# Eval("STATUS") %>" onclick="ViewContentNew(<%# Eval("ID") %>,<%# Eval("PART_NO") %>, '<%# Eval("MESS_TYPE") %>')">
                                <%# Eval("MESS_TYPE") %></a></td>
                         </tr>
                        </ItemTemplate>
                </asp:Repeater>
             </tbody>
            </table>
           </div>
      </div>      
   </div>    
    <div class="col-xs-7 " style="padding-left:40px;">            
    
        <div class="row">
        <label>Group</label>
        <select id="ddlGroupAddress" onchange="ddlGroupAddress_Onchange()">
            <option value=" ">-- [Select] --</option>
            <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                <option value="<%: r["ID"] %>"><%: r["GROUP_NAME"] %></option>
               <%  } %>
        </select>
        <button id="btnSend" onclick="btnOnclick()" class="btn btn-primary btn-sm" type="button">Send</button>
        <button id="btnSendAll" onclick="btnOnclickAll()"  class="btn btn-primary btn-sm" type="button">Send All</button>
		<button id="btnExport" onclick="btnExportMess()" class="btn btn-primary btn-sm" type="button">Export</button>
    </div>
        <div class="row">
            Address
        </div>
        <div class="row">
            <textarea id="txtAddress" class="wid_95" rows="5"></textarea>
        </div>
        <div class="row">
            Origin
        </div>
        <div class="row">
            <input id="txtOrigin" class="wid_250px" value="VVVVZGZX" />
        </div>
        <div class="row">
            Content
        </div>
        <div class="row">
            <textarea id="txtContent" class="wid_95" rows="25"></textarea>
        </div>

 </div>
    <div class="col-xs-1 w3-example" style="  overflow-y: scroll;height:calc(100% - 50px);" >
        <div class="scroll-area1">
           <div class="table-responsive">
           <table id="tblSource" class="table table-bordered table-responsive">
            <thead>
                <tr>
                    <th>AirPort North</th>
                   
                </tr>                
            </thead>
             <tbody>
                 <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                 <% if (r["GROUP_NAME"].ToString() == "VVNB" || r["GROUP_NAME"].ToString() == "VVDB"
                                       || r["GROUP_NAME"].ToString() == "VVCI"|| r["GROUP_NAME"].ToString() == "VVVD"  
                                       || r["GROUP_NAME"].ToString() == "VVTX"|| r["GROUP_NAME"].ToString() == "VVVH"
                                       || r["GROUP_NAME"].ToString() == "VVDH"
                                         ) { %>
                 <tr>
                             <td style="width:100%;text-align:center">
                                 
                                <a style="cursor: pointer;" onclick="p_Onchange(<%: r["ID"] %>)">
                                    <%: r["GROUP_NAME"] %></a>
                                   
                                </td>
                             
                         </tr>
                  <%} %>
                 <%  } %>
             </tbody>
            </table>
           </div>
      </div>      
    
       </div>

            <div class="col-xs-1 w3-example" style="  overflow-y: scroll;height:calc(100% - 50px);" >
        <div class="scroll-area1">
           <div class="table-responsive">
           <table id="tblSource" class="table table-bordered table-responsive">
            <thead>
                <tr>
                   
                    <th>AirPort Mid</th>
                    
                </tr>                
            </thead>
             <tbody>
                 <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                   <% if (r["GROUP_NAME"].ToString() == "VVDN" || r["GROUP_NAME"].ToString() == "VVPB"
                                       || r["GROUP_NAME"].ToString() == "VVPC"|| r["GROUP_NAME"].ToString() == "VVPK"  
                                       || r["GROUP_NAME"].ToString() == "VVCA"
                                         ) { %>
                 <tr>
                            
                             <td style="width:100%;text-align:center">
                                
                                <a style="cursor: pointer;" onclick="p_Onchange(<%: r["ID"] %>)">
                                <%: r["GROUP_NAME"] %></a>

                                 
                             </td>
                           
                         </tr>
                  <%} %>
                 <%  } %>
             </tbody>
            </table>
           </div>
      </div>      
    
       </div>

            <div class="col-xs-1 w3-example" style="  overflow-y: scroll;height:calc(100% - 50px);" >
        <div class="scroll-area1">
           <div class="table-responsive">
           <table id="tblSource" class="table table-bordered table-responsive">
            <thead>
                <tr>
                   
                    <th>AirPort South</th>
                </tr>                
            </thead>
             <tbody>
                 <% foreach (DataRow r in new GroupAddressDAL().GetAll().Rows) %>
              <% {%>
                 <% if (r["GROUP_NAME"].ToString() == "VVTS" || r["GROUP_NAME"].ToString() == "VVDL"
                                       || r["GROUP_NAME"].ToString() == "VVRG"|| r["GROUP_NAME"].ToString() == "VVCM"  
                                       || r["GROUP_NAME"].ToString() == "VVCS"|| r["GROUP_NAME"].ToString() == "VVCR"
                                       || r["GROUP_NAME"].ToString() == "VVCT" || r["GROUP_NAME"].ToString() == "VVPQ"
                                       || r["GROUP_NAME"].ToString() == "VVTH" || r["GROUP_NAME"].ToString() == "VVBM"
                                         ) { %>
                 <tr>
                            
                            <td style="width:100%;text-align:center">
                                
                                <a style="cursor: pointer;" onclick="p_Onchange(<%: r["ID"] %>)">
                                <%: r["GROUP_NAME"] %></a>

                                
                            </td>
                         </tr>
                  <%} %>
                 <%  } %>
             </tbody>
            </table>
           </div>
      </div>      
    
       </div>
           
           </div>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script>
         var phanCachArg = '<%= _phanCachArg%>';
        var phanCach = '<%= _phanCach%>';
        var partNo = ' ';
        var messType = ' ';
		
        function ddlGroupAddress_Onchange() {          
            var n = document.getElementById('ddlGroupAddress').options[document.getElementById('ddlGroupAddress').selectedIndex].value
            
            GetArgWithPostBack(n + phanCachArg + 'btnGetAddredd', 'btnGetAddredd');
        }
        function p_Onchange(p_value) {
           
            GetArgWithPostBack(p_value + phanCachArg + 'btnGetAddredd', 'btnGetAddredd');
        }
        function btnOnclickAll() {

            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_OT_CONT: $('#txtContent').val()
                };

                GetArgWithPostBack(partNo + phanCach + messType + phanCach + JSON.stringify(obj) + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
            }
            else {
                return false;
            }
        }
        function checkMaxlength() {
            if ($('#txtAddress').val().length >= 3800) {
                alert('Address long >3800 ky tu!');
                return false;
            }
            if ($('#txtContent').val().length >= 1799) {
                alert('Content long >1800 ky tu!')
                return false;
            }
            return true;
        }
        function btnOnclick() {
            
            //if (confirm('Do you want confirm???')) {
            //    var obj = {
            //        P_OT_CONT: $('#txtContent').val()
            //    };

            //    GetArgWithPostBack(partNo + phanCach + messType + phanCach + JSON.stringify(obj) + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
            //}
            //else {
            //    return false;
            //}
            var bx = checkMaxlength();
            if (!bx) return;
            var ax = checkValidCustomMinlenght('mCheck');
            if (!ax) return;
            if (confirm('Do you want confirm???')) {
                var obj = {
                    P_TOADD: $('#txtAddress').val(),
                    P_CONTENT: $('#txtContent').val()
                };
                GetArgWithPostBack(partNo + phanCach + messType + phanCach + $('#txtAddress').val() + phanCach + $('#txtContent').val() + phanCach + $('#txtOrigin').val() + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
            }
            else {
                return false;
            }

        }
        function DisplayResult(resulf, context) {
            if (context == 'btnGetAddredd') {
                
                $('#txtAddress').val('');
                $('#txtAddress').val('FF ' + resulf);
            }
            else if (context == 'btnSend_Onclick') {
                alert(resulf);
            }
            else if (context == 'RenderTopBarInfo') {
                $('#divExcuteScript').html(resulf);
            }
            else if (context == 'ViewContent') {
                var array = resulf.split('::')
                $('#txtContent').val(array[0]);
                $('#txtAddress').val('FF ' + array[1]);
            }


        }
        function ViewContent(cl, u) {
            document.getElementById(u+cl).style.backgroundColor = '#FF0000';
            partNo = cl;
            messType = u;
			
            GetArgWithPostBack(cl + phanCach + u + phanCachArg + 'ViewContent', 'ViewContent');
        }
        function ViewContentNew(id, cl, u) {
            
			document.getElementById(u + cl).style.backgroundColor = '#FF0000';
            partNo = id;
            messType = u;
            GetArgWithPostBack(id + phanCach + u + phanCachArg + 'ViewContent', 'ViewContent');
        }
		
		function btnExportMess()
		{
			var n = document.getElementById('ddlGroupAddress').options[document.getElementById('ddlGroupAddress').selectedIndex].text;
			var d = document.getElementById('ctl00_MainContent_txtBEGINDATE');
			
            var obj = { P_DATE: d.value, P_MESSTYPE: n };
			
            var $request = $.ajax({
                async: true,
                method: "PUT",
                url: "<%=System.Configuration.ConfigurationManager.AppSettings["ApplicationPath.API"]%>api/ApiExtension/ExcuteTable?packageName=MESSAGE_PKG&storeName=Get_MessageDetailAirPort",
                data: JSON.stringify(obj),
            }).always(function (data) {
                if (data.ListValue == -1) return;
				
				var strAppend = RenderTableKhExport(data);
				

                generate_excel(strAppend);
                //console.log(strHtml);
				
				
            });
			
		}
		function RenderTableKhExport(data) {
            $.each(data.ListValue, function (a, b) {
                kq += b.CONTENT.replace(' ','\n') + '\n\n\n\n'
                
            });
            return kq;
        }
		 function generate_excel(html) {           
            
            //OK
            exportExel(html);
            
        }
		function exportExel(_html) {                  
            
            var dt = new Date();
            var day = dt.getDate();
            var month = dt.getMonth() + 1;
            var year = dt.getFullYear();
            var hour = dt.getHours();
            var mins = dt.getMinutes();
            var postfix = day + "." + month + "." + year + "_" + hour + "." + mins;

            var textToSave = _html;
            var textToSaveAsBlob = new Blob([textToSave], { type: "text/plain" });
            var textToSaveAsURL = window.URL.createObjectURL(textToSaveAsBlob);
            var fileNameToSaveAs = 'exported_mess_airport_' + postfix + '.doc';
            var downloadLink = document.createElement("a");
            downloadLink.download = fileNameToSaveAs;
            downloadLink.innerHTML = "Download File";
            downloadLink.href = textToSaveAsURL;
            downloadLink.onclick = destroyClickedElement;
            downloadLink.style.display = "none";
            document.body.appendChild(downloadLink);
            downloadLink.click();           

        }
		
		function destroyClickedElement(event) {
            document.body.removeChild(event.target);
        }
    </script>
     <script language="javascript" type="text/javascript">

        $('#<%= txtBEGINDATE.ClientID %>').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'd/m/Y',
            formatDate: 'd/m/Y'
        });       
    </script>
</asp:Content>

