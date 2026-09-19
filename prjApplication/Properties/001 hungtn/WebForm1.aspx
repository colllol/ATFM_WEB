<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="prjApplication._001_hungtn.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../Style/assets/css/jquery-ui.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
</head>
<body>
    <style>
        #grdSource tr td input {
            border: none;
        }
    </style>


    <form id="form1" runat="server">

        <input id="tags" name="name">
        <input id="tags_code" name="code">
        <input id="test" type="text" />
        <div id="formText">

            <div class="form-group wid_250px">
                <input type="text" id="txtDate" class="wid_250px" onblur="checkInputDate(this)" name="txtDate" />
            </div>
            <i class="glyphicon glyphicon-ok"></i>

            <% for (int i = 0; i < 10; i++)%>
            <%{%>
            <select>
                <%= _SanBay %>
            </select>
            <%} %>
            <select id="ddlFrom">
                <%= _SanBay%>
            </select>
            <script>
                setSelectedValue('ddlFrom', 'UUDD');
            </script>
            <asp:GridView runat="server" ID="grdSource" AutoGenerateColumns="false">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            1
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="ABC">
                        <ItemTemplate>
                            <%--<select id='<%# "ddlFrom" + Container.DataItemIndex %>'>
                           <%= _SanBay %>                           
                       </select>--%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            asdasd<br>
            asdasd<br>
            asdasd<br>
            asdasd<br>
            asdasd<br>
            <a href="#txtDate">aa</a>
        </div>


    </form>

    <script src="../Style/assets/js/jquery-1.11.3.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/jquery.autocomplete.js"></script>
    <script>
        var grd = $('#<%=grdSource.ClientID%>');
        var $tr = $('#<%=grdSource.ClientID%> tr');
        var oldVal = ''; var newVal = '';
        var isUpdate = false;
        $('#<%=grdSource.ClientID%> tr').each(function () {
            $(this).bind('blur', function () {
                alert('1');
            });
        });
        <%--$('#<%=grdSource.ClientID%> td').each(function () {
            $(this).bind('dblclick', function () {
                oldVal = $(this).html();
                $(this).html('');
                var txt = '<input id=\'idAX\' onblur=\'ChangeValueUpdate(this)\' type=\'text\' value=\'' + oldVal.trim() + '\' />';
                //$(ele).text('');
                $(this).append(txt);
                $('#idAX').focus();
            })
        });--%>

        function EditMe(ele) {
            oldVal = $(ele).text();
            var $pa = $(ele).parent();
            $(ele).parent().html('');
            var txt = '<input id=\'idAX\' onblur=\'ChangeValueUpdate(this)\' type=\'text\' value=\'' + $(ele).text() + '\' />';
            //$(ele).text('');
            $(ele).append(txt);
            $('#idAX').focus();
        }
        function ChangeValueUpdate(ele) {
            newVal = $(ele).val();
            if (oldVal != newVal) {
                isUpdate = true;
            }
            oldVal = newVal;
            $(ele).parent().html(newVal);
            $(ele).remove();
        }
        var lisSB = ["VVTS", "VVNB", "VVDN"];
        var listSanBay = '<%= _ListSanBay%>'.split(',');
        function CheckListSanBay(ele) {
            if (listSanBay.indexOf($(ele).val().toUpperCase()) == -1) {
                $(ele).val('');
                $(ele).css('border-color: red;');
            } else {
                $(ele).css('border-color: \'\'');
                $(ele).val($(ele).val().toUpperCase());
            }
        }

        var availableTags = ["ActionScript", "AppleScript", "Asp"];
        var availableTagsCode = ["1", "2", "3"];
        $("#tags").autocomplete({
            source: [listSanBay],
        }).on('blur', function (e, datum) {
            CheckListSanBay(this);
        });;


    </script>

    <tr id="_102" data-isinsert="true" onmouseover="rowId=$(this).prop('id')" onmouseout="rowId=0;" class="rowCreate">
        <td style="width: 30px"></td>
        <td id="b_102">101</td>
        <td>
            <input type="checkbox" id="chk_102103abc" value="13"></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtPERMNBR102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtOPER_ID102103abc" value="" onfocusin="binAutocomplete(this,&quot;OPER&quot;)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="4" maxlength="8" class="sInput" id="txtFLIGHTNBR102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtREGISTRATION102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="2" class="sInput" id="txtFROM_AIRP102103abc" value="" onfocusin="binAutocomplete(this,&quot;AERO&quot;)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="2" class="sInput" id="txtTO_AIRP102103abc" value="" onfocusin="binAutocomplete(this,&quot;AERO&quot;)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" maxlength="10" data-checkdate="true" class="sInput" id="txtFLIGHTDATE102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="4" maxlength="6" class="sInput" id="txtETD102103abc" value="" data-number="true" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtETA102103abc" value="" data-number="true" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtATD102103abc" value="" data-number="true" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtATA102103abc" value="" data-number="true" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtPERMCRAFT102103abc" data-craftid="0" value="" onfocusin="binAutocomplete(this,&quot;CRAFT&quot;)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtREALCRAFT102103abc" value="" data-craftid="10" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtPURPOSE102103abc" onfocusin="binAutocomplete(this,&quot;PURPOSE&quot;)" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="2" class="sInput" id="txtVALIDHOURS102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtFLIGHT_TYPE102103abc" value="" maxlength="2" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtPERMTYPE102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtVIA102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtFPLVIA102103abc" value="" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtREMARK102103abc" value="" data-contenttip=""></td>
    </tr>



    <tr id="3293423" data-isupdate="false" onmouseover="rowId=3293423;" onmouseout="rowId=0;">
        <td style="width: 30px">
            <div class="action-buttons"><i id="btnDeleteRemark3293423" class="ace-icon fa fa-trash-o bigger-130" onclick="btnDeleteOnclick(3293423);"></i></div>
        </td>
        <td style="width: 30px" id="b_109">
            <input style="width: 30px!important; padding: 0px!important;" type="text" id="txtStt9" value="9"></td>
        <td>
            <input type="checkbox" id="chk_3293423"></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtPERMNBR8" data-oldvalue="O/F 00111/S/CHK/2019" value="O/F 00111/S/CHK/2019" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtOPER_ID8" data-oldvalue="TLM" value="TLM" onfocusin="binAutocomplete(this,&quot;OPER&quot;)" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="4" maxlength="8" class="sInput" id="txtFLIGHTNBR8" data-oldvalue="TLM973" value="TLM973" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtREGISTRATION8" data-oldvalue="HSLTY" value="HSLTY" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="2" class="sInput" id="txtFROM_AIRP8" data-oldvalue="ZSNJ" onfocusin="binAutocomplete(this,&quot;AERO&quot;)" value="ZSNJ" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="2" class="sInput" id="txtTO_AIRP8" data-oldvalue="VTSP" onfocusin="binAutocomplete(this,&quot;AERO&quot;)" value="VTSP" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" maxlength="10" class="sInput" id="txtFLIGHTDATE8" data-oldvalue="12-04-2019" value="12-04-2019" onblur="checkIsUpdate(this)" data-contenttip="format dd,dd....,ddmmyy" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="4" maxlength="6" class="sInput" id="txtETD8" data-oldvalue="1725" data-number="true" value="1725" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtETA8" data-oldvalue="" data-number="true" value="" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtATD8" data-oldvalue="" data-number="true" value="" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtATA8" data-oldvalue="" data-number="true" value="" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtPERMCRAFT8" data-craftid="11" data-oldvalue="B739" value="B739" onfocusin="binAutocomplete(this,&quot;CRAFT&quot;)" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="6" class="sInput" id="txtREALCRAFT8" data-oldvalue="A330" value="A330" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtPURPOSE8" data-oldvalue="PAX" onfocusin="binAutocomplete(this,&quot;PURPOSE&quot;)" value="PAX" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" maxlength="2" class="sInput" id="txtVALIDHOURS8" data-oldvalue="72" value="72" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" data-minlenght="1" class="sInput" id="txtFLIGHT_TYPE8" data-oldvalue="SC" value="SC" maxlength="2" onblur="checkIsUpdate(this)" data-contenttip="" style="border: 0px;"></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtPERMTYPE8" data-oldvalue="O/F" value="O/F" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtVIA8" data-oldvalue="" value="" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtFPLVIA8" data-oldvalue="R474" value="R474" onblur="checkIsUpdate(this)" data-contenttip=""></td>
        <td>
            <input type="text" data-control="_updateAll" class="sInput" id="txtREMARK8" data-oldvalue="B/737" value="B/737" onblur="checkIsUpdate(this)" data-contenttip=""></td>
    </tr>

</body>
</html>
