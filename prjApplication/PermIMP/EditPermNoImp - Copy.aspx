<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true" CodeBehind="EditPermNoImp.aspx.cs" Inherits="prjApplication.PermIMP.EditPermNoImp" %>
<%@ Register TagPrefix="nbc" Namespace="prjComponents.UI" Assembly="prjComponents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/style_input.css" rel="stylesheet" />
    <link href="../Style/Style_List.css" rel="stylesheet" />
    <script src="../Style/assets/js/wizard.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
    <script src="../Scripts/validate.js"></script>
    <script src="../Scripts/DateTimeFomat.js"></script>
    <table width="100%" cellpadding="0" cellspacing="0" align="center">
        <tr align="center">
            <td align="center">

                <div id="body-wrapper">
                    <div id="main-content">
                        <div class="content-box">
                            <!-- Start Content Box -->
                            <div class="content-box-header">
                                <h3 style="cursor: s-resize; margin-top: 0px; font-size: 12px;">+ EDIT PERM NO IMP</h3>

                                <div class="clear">
                                </div>
                            </div>
                            <!-- End .content-box-header -->
                            <div class="content-box-content">

                                <div class="center">
                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtPERMNBR">CALLSIGN </label>
                                                    <input id="txtCALLSIGN" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <label class="col-lg-4 control-label" for="txtBEGINDATE">PERMDATE</label>
                                                    <input id="txtPERMDATE" runat="server" type="text" class="col-sm-5" />
                                            </div>
                                        </div>
                                    </div>
                                    


                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    
                                                    <label class="col-lg-4 control-label" for="txtPERMTYPE">PERMTYPE</label>
                                                    <div class="col-xs-5">
                                                        <asp:DropDownList  ID="ddlPERMTYPE" CssClass="form-control row col-sm-5" runat="server"
                                                            Width="90%">
                                                            <asp:ListItem Value="LD" Text="Landing" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Value="O/F" Text="Over Flight"></asp:ListItem>
                                                        </asp:DropDownList>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtCRAFT">CRAFT</label>
                                                    <div class="col-xs-5">
                                                        <asp:DropDownList ID="ddlCRAFT" CssClass="form-control row col-sm-5" runat="server"
                                                            Width="90%">                                                            
                                                        </asp:DropDownList>                                                       
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtFROM_AIRP">FROM_AIRP</label>
                                                    <input id="txtFROM_AIRP" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtTO_AIRP">TO_AIRP</label>
                                                    <input id="txtTO_AIRP" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtETD">ETD</label>
                                                    <input id="txtETD" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtETA">ETA</label>
                                                    <input id="txtETA" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                    <label class="col-lg-4 control-label" for="txtVIA">VIA</label>
                                                    <input id="txtVIA" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                     <label class="col-lg-4 control-label" for="txtPERMNBR">REMARK</label>
                                                    <input id="txtREMARK" runat="server" type="text" class="col-sm-5" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                     <label runat="server" id="lblResuft" style="font-weight: 700; color: red;"></label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-lg-5">
                                            <div class="form-horizontal">
                                                <div class="form-group">
                                                   
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                   

                                </div>



                                <div class="clear clearHeight">
                                </div>
                                <div id="khung">
                                    <div id="main">
                                        <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold btn-round"
                                            OnClick="btnSave_Click">
                                                     <i class="ace-icon fa fa-floppy-o bigger-120 white"></i>
                                                        Save
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" ID="btnExit" CssClass="btn btn-sm btn-warning btn-round"
                                            CausesValidation="false"
                                            OnClick="btnExit_Click">
                                                     <i class="ace-icon fa fa-times white"></i>
                                                        Exit
                                        </asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <!-- End .content-box-content -->
                        </div>

                    </div>
                </div>
            </td>
        </tr>
    </table>

    <script src="../Scripts/CustomDynamic.js"></script>
</asp:Content>

