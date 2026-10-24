<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="ImportsPermSC_LDQ.aspx.cs" Inherits="prjApplication.Tool.ImportsPermSC_LDQ" %>


<%@ Register Assembly="prjComponents" Namespace="prjComponents" TagPrefix="cc1" %>
<%@ Import Namespace="prjBusinessLogic" %>
<%@ Import Namespace="prjInfo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link href="../Style/Style_List.css" rel="stylesheet" />
    <!-- PAGE CONTENT BEGINS -->

    <div class="widget-box">
        <div class="widget-header widget-header-blue widget-header-flat">
            <h4 class="widget-title lighter">Import Data</h4>
        </div>

        <div class="widget-body">
            <div class="widget-main">
                <div id="fuelux-wizard-container">

                    <div class="step-content pos-rel">
                        <div class="center">

                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">PERMNBR</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <input id="txtPERMNBR" type="text" runat="server" maxlength="5"  style="width: 100px" />
                                                </label>
                                                <label class="checkbox-inline" style="width:120px">
                                                    PERMDATE
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtENDDATE" type="text"
                                                        data-date-format="dd/mm/yyyy" class="date-picker" runat="server" style="width: 100px" />
                                                </label>
                                                <label class="checkbox-inline" style="width:100px">
                                                    VERSION
                                                </label>
                                                <label class="textbox-inline">
                                                    <input id="txtVersion" type="text" runat="server" maxlength="1" style="width: 50px" />
                                                </label>
                                                <label class="checkbox-inline" style="width:80px">
                                                    AUTHOR
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlAUTHOR" runat="server" CssClass="form-control" Width="100px">
                                                    </asp:DropDownList>
                                                </label>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">OPER</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <label class="textbox-inline">
                                                    <select id="ddlLoaiImport" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="HVN">HVN</option>
                                                        <option value="PIC">PIC</option>
                                                        <option value="VJC">VJC</option>
                                                        <option value="KHV">KHV</option>
                                                        <option value="VFC">VFC</option>
                                                        <option value="JNL">JNL</option>
                                                        <option value="CPA">CPA</option>
                                                        <option value="KA">KA</option>
                                                        <option value="CLX">CLX</option>
                                                        <option value="HKE">HKE</option>
                                                        <option value="CZ">CZ</option>
                                                        <option value="QTR">QTR</option>
                                                        <option value="MAS">MAS</option>
                                                        <option value="AXM">AXM</option>
                                                        <option value="TWB">TWB</option>
                                                        <option value="KRL">KRL</option>
                                                        <option value="FX">FX</option>
                                                        <option value="CES">CES</option>
                                                        <option value="CSN">CSN</option>
                                                        <option value="HDA">HDA</option>
                                                        <option value="KAL">KAL</option>
                                                        <option value="SIA">SIA</option>
                                                        <option value="SQC">SQC</option>
                                                        <option value="TWG">TWG</option>
                                                        <option value="UAE">UAE</option>
                                                        <option value="THA">THA</option>
                                                        <option value="CPASM">CPA SUMMER</option>

                                                        <option value="AXM_WORD">AXM_WORD</option>
                                                        <option value="CAL">CAL</option>
                                                         <option value="UW">UW</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline"style="width:120px">
                                                    PERTYPE
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlPERMTYPE" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="LD">Landing</option>
                                                        <option value="O/F">Over Flight</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width:100px">
                                                    SEASON
                                                </label>
                                                <label class="textbox-inline">
                                                    <select id="ddlSeason" class="form-control" runat="server" style="width: 100px;">
                                                        <option value="W">Winter</option>
                                                        <option value="S">Summer</option>
                                                    </select>
                                                </label>
                                                <label class="checkbox-inline" style="width:80px">
                                                    PURPOSE
                                                </label>
                                                <label class="textbox-inline">
                                                    <asp:DropDownList ID="ddlPURPOSE" runat="server" CssClass="form-control" Width="100px">
                                                    </asp:DropDownList>
                                                </label>
                                            </div>
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
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">VIA</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <asp:TextBox runat="server" ID="txtCraft"  style="width:430px" ></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">SCHEDULE</label>
                                            <div class="col-lg-5" style="text-align: left;">
                                                <textarea class="form-control" id="txtContent" runat="server" style="height: 380px;"
                                                    placeholder="Content"></textarea>
                                            </div>
                                            <label class="col-lg-1 control-label">ROUTES</label>
                                            <div class="col-lg-4" style="text-align: left;">
                                                <textarea class="form-control" id="txtRoutes" runat="server" style="height: 380px;"
                                                    placeholder="Content"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <%--<div class="col-lg-4">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">VIA</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <textarea class="form-control" id="txtvia" runat="server" style="height: 380px;"
                                                    placeholder="Via"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                            <div class="row">
                                <div class="col-lg-10">
                                    <div class="form-horizontal">
                                        <div class="form-group">
                                            <label class="col-lg-2 control-label">REMARK</label>
                                            <div class="col-lg-10" style="text-align: left;">
                                                <textarea class="form-control" id="txtRemark" runat="server" placeholder="Remark"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" style="text-align: center">
                                <div class="col-lg-10">

                                    <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold"
                                        OnClick="linkSave_Click">
                                                <span class="glyphicon glyphicon-download-alt"></span>
                                                IMPORT
                                    </asp:LinkButton>

                                    <a href="#" class="btn btn-sm btn-primary">
                                        <span class="ace-icon fa fa-times"></span>
                                        EXIT

                                    </a>
                                </div>
                            </div>
                        </div>



                    </div>
                </div>


            </div>
            <!-- /.widget-main -->
        </div>
        <!-- /.widget-body -->
    </div>


    <!-- PAGE CONTENT ENDS -->

    <!-- /.row -->
    <!-- basic scripts -->


    <!-- page specific plugin scripts -->

    <script src="<%=Global.ApplicationPath%>/Style/assets/js/wizard.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/CustomDynamic.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/validate.js"></script>
    <script src="<%=Global.ApplicationPath%>/Scripts/DateTimeFomat.js"></script>



</asp:Content>
