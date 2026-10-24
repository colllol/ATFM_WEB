<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM.Master" CodeBehind="MessFlightplan.aspx.cs" Inherits="prjApplication.MessManagement.MessFlightplan" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <style type="text/css">
        .w3-example {
        background-color: #f1f1f1;       
    }       
   </style>
    <div class="container-fluid"> 
     <div class="row w3-example" style="margin-bottom:20px;">
       <div class="col-xs-3"></div>
       <div class="row" style="text-align:center"> 
          
        <%--<asp:Button id="btnOption" runat="server" OnClick="btnSelect_Click" Text="Select" class="btn btn-primary btn-sm" type="button"></asp:Button>--%>
      </div>               
   </div>
       <div class="row">
           
           
           <div class="col-xs-7 w3-example" style="  overflow-y: scroll;height:calc(100% - 50px);" >
                <div class="table-responsive">
                    <table style="width: 100%; border: 0px solid black;">
                         <tr>
            <td style="width:100%">
                <fieldset class="box-border">
                    <legend class="box-border">Message Heading and Addressing Data</legend>
                    <table style="width: 60%; border: 0px solid black;margin:1px;">
                         <tr>
                             <td> <label style="font-size: 12px;" >Priority</label>
                             </td>
                             <td colspan="3">
                                 <input value="FF" style="height: 25px;width:30px" id="idPriority" type="text" class="wid_80px" />
                             </td>
                         </tr>
                        <tr>
                            <td>
                                <label style="font-size: 12px;" >Filling Time</label>
                                
                            </td>
                            <td><input style="height: 25px;" id="idFillingTime" value="110330" type="text" class="wid_80px" /></td>
                            <td>
                                <label style="font-size: 12px;">Originator</label>
                            </td>
                            <td>                                
                                <input style="height: 25px;" id="idOriginator" type="text" class="wid_80px" value="VVVVZGZX" />
                            </td>
                       </tr>
                       <tr>
                           <td><label style="font-size: 12px;">Addresses FF</label></td>                           
                            <td colspan="3">                              
                                <input style="height: 25px;width:523px" id="idAddressesFF" value="" type="text" class="wid_280px" />
                            </td>                           
                        </tr> 
                        <tr>
                             <td> <label style="font-size: 12px;" >Addressing(AD)</label>
                             </td>
                             <td colspan="3">
                                 <input style="height: 25px;width:523px" id="idAddressesAD" value="" type="text" class="wid_280px" />
                             </td>
                         </tr>                                                       
                    </table>
                </fieldset>
            </td>
        </tr>
                        <tr>
                         <td>
                <fieldset class="box-border">
                    <legend class="box-border">Message Fields</legend>
                    <table style="width: 100%; border: 0px solid black;">
                        <tr>
                            <td style="width:20%">
                                <label style="font-size: 12px;">Callsign</label>
                                <input id="idCallsign" maxlength="8" data-minlenght="1" data-control="update" type="text"
                                    class="wid_30px" style="height: 25px;width:80px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">SSR</label>
                                <input id="idSSR" maxlength="8" data-minlenght="1" data-control="update" type="text"
                                    class="wid_30px" style="height: 25px;width:80px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">Rules</label>
                               <input id="idRules" maxlength="8" data-minlenght="1" data-control="update" type="text"
                                    class="wid_30px" style="height: 25px;width:40px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">Type of Flight</label>
                                <input id="idTypeofFlight" maxlength="8" data-minlenght="1" data-control="update" type="text"
                                    class="wid_30px" style="height: 25px;width:40px" />
                            </td>
                         </tr>   
                            <tr>
                            <td>
                                <label style="font-size: 12px;">Number of A/C</label>
                                <input style="height: 25px;width:40px" id="idNumberofAC" type="text" class="wid_80px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">A/C Type</label>
                               <input style="height: 25px;width:70px" id="idACType" type="text" class="wid_80px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">WTC</label>
                                <input style="height: 25px;width:40px" id="idWTC" type="text" class="wid_80px" />
                            </td>
                             <td>
                                <label style="font-size: 12px;">Equipment</label>
                                <input style="height: 25px;width:70px" id="idEquipment" type="text" class="wid_80px" />
                            </td>
                        </tr>
                         <tr>
                            <td>
                                <label style="font-size: 12px;">ADEP</label><br />
                                <input style="height: 25px;" id="idADEP" type="text" class="wid_80px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">EOBT</label>                                
                                <input style="height: 25px;" id="idEOBT" type="text" class="wid_80px" />
                            </td>
                            <td>
                                
                            </td>
                              <td>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label style="font-size: 12px;">ADES</label><br />
                                <input style="height: 25px;width:60px" id="idADES" type="text" class="wid_40px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">EET</label>                               
                                <input style="height: 25px;width:60px" id="idEET" type="text" class="wid_40px" />
                            </td>
                            <td>
                                <label style="font-size: 12px;">ALTN 1</label><br />
                                <input style="height: 25px;width:60px" id="idALTN1" type="text" class="wid_40px" />
                            </td>
                             <td>
                                <label style="font-size: 12px;">ALTN 2</label><br />
                                <input style="height: 25px;width:60px" id="idALTN2" type="text" class="wid_40px" />
                            </td>
                        </tr>
                         <tr>
                            <td colspan="3">
                                <label style="font-size: 12px;">Route</label><br />
                                <textarea id="idRoute" rows="3" style="width:500px"></textarea>
                            </td>                           
                        </tr>

                        <tr>
                         <td colspan="4" align="center">
                             <input type="button" value="Create" onclick="fnCreate();"  class="btn btn-primary btn-sm" />                             
                            </td> 
                        
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>        
    </table>
               </div>               
           </div>
            <div class="col-xs-5  " style="padding-left:10px;">    
                    
                    <textarea id="idContent" runat="server" style="width:100%;height:450px" ></textarea>    
                    <button id="btnSend" onclick="btnOnclick()" class="btn btn-primary btn-sm" type="button">Send</button>     
                </div> 
        </div>
    </div>
     <script type="text/javascript">
        var phanCachArg = '<%= _phanCachArg%>';        
         function btnOnclick() {
             if (confirm('Do you want confirm???')) {
                 var obj = {
                     P_OT_CONT: $('#ctl00_MainContent_idContent').val()
                 };
                 
                 GetArgWithPostBack(JSON.stringify(obj) + phanCachArg + 'btnSend_Onclick', 'btnSend_Onclick');
             }
             else {
                 return false;
             }

         }
         function DisplayResult(resulf, context) {
             if (context == 'btnSend_Onclick') {
                 alert(resulf);
             }            
         }
         function fnCreate() {
             var idPriority = $('#idPriority').val();
             var idFillingTime = $('#idFillingTime').val();
             var idOriginator = $('#idOriginator').val();
             var idAddressesFF = $('#idAddressesFF').val();
             var idAddressesAD = $('#idAddressesAD').val();
             var idCallsign = $('#idCallsign').val();
             var idSSR = $('#idSSR').val();
             var idRules = $('#idRules').val();
             var idTypeofFlight = $('#idTypeofFlight').val();
             var idNumberofAC = $('#idNumberofAC').val();
             var idACType = $('#idACType').val();             
             var idWTC = $('#idWTC').val();
             var idEquipment = $('#idEquipment').val();
             var idADEP = $('#idADEP').val();
             var idEOBT = $('#idEOBT').val();
             var idADES = $('#idADES').val();
             var idEET = $('#idEET').val();
             var idALTN1 = $('#idALTN1').val();
             var idALTN2 = $('#idALTN2').val();
             var idRoute = $('#idRoute').val();
             
             var idContent;

             idContent = idPriority + ' ' + idAddressesFF + '\n';
             if (idAddressesAD !='')
                 idContent = idContent + idPriority + ' ' + idAddressesAD + '\n';
             idContent = idContent + idFillingTime + ' ' + idOriginator + '\n';
             idContent = idContent + '(FPL-' + idCallsign + '-' + idRules + idTypeofFlight + '\n';
             idContent = idContent + '-' + idNumberofAC + ' ' +  idACType + '/' + idWTC + '-' + idEquipment + '\n';
             idContent = idContent + '-' + idADEP + ' ' + idEOBT + '\n';
             idContent = idContent + '-' + idADES + ' ' + idEET + ' ' + idALTN1 + ' ' + idALTN2 + '\n';
             idContent = idContent + '-' + idRoute + ' EQUIPPED)';

             $('#ctl00_MainContent_idContent').val(idContent);
         }
     </script>
     <script language="javascript" type="text/javascript">
         var currentdate = new Date();
         var mon = currentdate.getMonth() + 1;
         var monF
         if (mon < 10)
             monF = '0' + mon;
         else
             monF = mon;
         var datetime = currentdate.getFullYear() + '' 
                         + monF + ''
                         + currentdate.getDate();
         $('#idFillingTime').val(datetime);
        $('#idFillingTime').datetimepicker({
            mask: '39/19/9999',
            timepicker: false,
            formatTime: '',
            format: 'Y/m/d',
            formatDate: 'Y/m/d'
        });       
    </script>
</asp:Content>
