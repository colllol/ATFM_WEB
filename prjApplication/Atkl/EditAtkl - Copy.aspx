<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Masters/ATFM.Master" CodeBehind="EditAtkl.aspx.cs" Inherits="prjApplication.Atkl.EditAtkl" %>

<%@ Register Assembly="CustomControl" Namespace="CustomControl" TagPrefix="cc1" %>
<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <%--<link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/autocomplete.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />

    <style>
        td {
            text-align: center; /* center checkbox horizontally */
            vertical-align: middle; /* center checkbox vertically */
        }

        input[type=checkbox] {
            transform: scale(1.5);
        }

        .cssHetHan {
            color: red !important;
        }

            .cssHetHan input {
                color: red !important;
            }

        #tblSource input, select {
            color: black;
        }

        .xdsoft_autocomplete_dropdown {
            min-width: 130px !important;
        }

        .sInput {
            /*border-width: 0px !important;*/
            border: 0px;
            width: 100%;
        }

        input[type=text] {
            border: 0px;
        }

        .success {
            background-color: aquamarine;
        }

        .rowCreate {
            background-color: mediumvioletred;
        }

        input {
            text-transform: uppercase;
        }

        textarea {
            text-transform: uppercase;
        }
    </style>
    <style>
        .preloader {
            display: inline-block;
            padding: 0px;
            border-radius: 100%;
            border: 2px solid;
            border-top-color: rgba(0,0,0, 0.65);
            border-bottom-color: rgba(0,0,0, 0.15);
            border-left-color: rgba(0,0,0, 0.65);
            border-right-color: rgba(0,0,0, 0.15);
            -webkit-animation: preloader 0.8s linear infinite;
            animation: preloader 0.8s linear infinite;
        }

        @keyframes preloader {
            from {
                transform: rotate(0deg);
            }

            to {
                transform: rotate(360deg);
            }
        }

        @-webkit-keyframes preloader {
            from {
                -webkit-transform: rotate(0deg);
            }

            to {
                -webkit-transform: rotate(360deg);
            }
        }
    </style>
    <style>
        #tblSource tbody tr.select input {
            background-color: darkseagreen;
        }

        #tblSource tbody tr.select {
            background-color: darkseagreen;
        }

        #tblSource tbody tr.selectdel input {
            background-color: red;
        }

        #tblSource tbody tr.selectdel {
            background-color: red;
        }
    </style>--%>
    <div id="pathFileUpload" hidden="hidden"></div>
    <div id="atkl_id" hidden="hidden"><%= Request.QueryString["ID"]==null?"": Request.QueryString["ID"]%></div>
    <div id="divExcuteScript" hidden="hidden"></div>
    <fieldset class="box-border">
        <div id="divPermMaster">
            <label for="txtPERMNBR_ID" style="display: none;">PERMNBR</label>
            <input id="txtPERMNBR_ID" readonly type="text" class="wid_120px" style="display: none;" />

            <table style="width: 100%; border: 0px solid black;">
                <tr valign="top">
                    <td style="width: 50%">
                        <fieldset class="box-border">
                            <legend class="box-border">An toàn không lưu</legend>
                            <table style="width: 100%; border: 0px solid black;">
                                <tr>
                                    <td>
                                        <label for="txtDATE_EXPLOIT" style="font-size: 12px;">Ngày Khai Thác</label><br />
                                        <input style="height: 25px;" id="txtDATE_EXPLOIT" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtCRAFT_TYPE" style="font-size: 12px;">Loại tàu bay</label><br />
                                        <input id="txtCRAFT_TYPE" maxlength="5" data-minlenght="1" data-control="update" type="text"
                                            class="wid_80px" style="height: 25px;" />
                                    </td>
                                    <td>
                                        <label for="txtREGISTRATION" style="font-size: 12px;">Đăng bạ</label><br />
                                        <input style="height: 25px;" id="txtREGISTRATION" data-minlenght="1" data-control="update" maxlength="1" type="text" class="wid_80px" />
                                    </td>
                                    
                                    <td>
                                        <label for="txtCALLSIGN" style="font-size: 12px;">Tên gọi chuyến bay</label><br />
                                        <input style="height: 25px;" id="txtCALLSIGN" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtFROMAIRP" style="font-size: 12px;">Sân bay đi</label><br />
                                        <input style="height: 25px;" id="txtFROMAIRP" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTOAIRP" style="font-size: 12px;">Sân bay đến</label><br />
                                        <input style="height: 25px;" id="txtTOAIRP" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtATD" style="font-size: 12px;">ATD</label><br />
                                        <input style="height: 25px;" id="txtATD" maxlength="2" data-number="true" data-control="update"
                                            data-minlenght="1" type="text"
                                            class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtATA" style="font-size: 12px;">ATA</label><br />
                                        <input style="height: 25px;" id="txtATA" maxlength="2" data-number="true" data-control="update"
                                            data-minlenght="1" type="text"
                                            class="wid_80px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <label for="txtFROMAIRP" style="font-size: 12px;">Sân bay đi</label><br />
                                        <input style="height: 25px;" id="txtFROMAIRP" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtTOAIRP" style="font-size: 12px;">Sân bay đến</label><br />
                                        <input style="height: 25px;" id="txtTOAIRP" data-minlenght="10" data-control="update" type="text" class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtATD" style="font-size: 12px;">ATD</label><br />
                                        <input style="height: 25px;" id="txtATD" maxlength="2" data-number="true" data-control="update"
                                            data-minlenght="1" type="text"
                                            class="wid_80px" />
                                    </td>
                                    <td>
                                        <label for="txtATA" style="font-size: 12px;">ATA</label><br />
                                        <input style="height: 25px;" id="txtATA" maxlength="2" data-number="true" data-control="update"
                                            data-minlenght="1" type="text"
                                            class="wid_80px" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div class="row" style="text-align: center">
                            <button id="btnUpdatePermMaster" class="btn btn-sm btn-primary" type="button" onclick="btnUpdateOnclick()">Update</button>
                            <a href="<%= Page.ResolveUrl("~/Atkl/Atkl_1.aspx?Menu_ID=" + Page.Request["Menu_ID"].ToString()) %>"
                                class="btn btn-sm btn-primary">
                                <span class="ace-icon fa fa-times"></span>
                                Exit
                            </a>
                        </div>
                    </td>
                </tr>
            </table>
            <script>
                function ReadInfoPerm(data) {
                    var obj = JSON.parse(data.replace(/\n/gi, '<br>').replace(/\\/gi, '\\\\').replace(/\t/gi, '     '));
                    //var obj = JSON.parse(data);
                    txtDATE_EXPLOIT.value = obj['DATE_EXPLOIT'] == null ? null : obj['DATE_EXPLOIT']['DateTime'].replace(/\//g, '-');
                    txtCRAFT_TYPE.value = obj['CRAFT_TYPE'];
                    //txtREFERENCE.value = obj['REFERENCE'] == null ? null : obj['REFERENCE'].replace(/<br>/gi, '\r\n');
                    txtREGISTRATION.value = obj['REGISTRATION'];
                    txtATD.value = obj['ATD'];
                    txtATA.value = obj['ATA'];
                }
                function DisplayResult(resulf, context) {
                    if (context == 'GetOneFlight') {
                        ReadInfoPerm(resulf);
                    }
                    if (context == 'btnUpdateOnclick') {
                        alert(resulf);
                    }
                }
                function ReadObj() {
                    var obj = {
                        ID: $('#atkl_id').html(),
                        DATE_EXPLOIT: txtDATE_EXPLOIT.value.replace(/-/g, '/'),
                        CRAFT_TYPE: txtCRAFT_TYPE.value,
                        REGISTRATION: txtREGISTRATION.value,
                        ATD: txtATD.value,
                        ATA: txtATA.value
                    };
                    return obj;
                }
                function btnUpdateOnclick() {
                    if ($('#atkl_id').html() != '') {
                            GetArgWithPostBack(JSON.stringify(ReadObj()) + '_____btnUpdateOnclick', 'btnUpdateOnclick');
                    }
                    else {
                        alert('Please select Flight');
                        return;
                    }
                }
            </script>
</asp:Content>

