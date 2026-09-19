<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM.Master" AutoEventWireup="true"
    CodeBehind="EditFinishFlight.aspx.cs" Inherits="prjApplication.FinishFlights.EditFinishFlight" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../Style/assets/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
    <link href="../Style/assets/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
    <style>
        body {
            font-size: 12px;
        }

        .mControl {
            width: 200px !important;
            position: absolute;
            top:0px;
            right: 0px;
        }
    </style>
    <span class="TitlePanel">+ EDIT FINISHED FILGHTS</span>
    <div class="row">
        <div class="form-inline" role="form">

            
            <div class="col-lg-3">
                <div class="form-group">
                    <label for="txtPERMNBR" class="mLable control-label">PERMNBR</label>
                    <input id="txtPERMNBR" runat="server" data-control="_Update" maxlength="30" class="form-control mControl"
                        type="text" />
                </div>
            </div>
            
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtPERMTYPE" class="mLable control-label">PERMTYPE</label>
                        <select id="ddlPERMTYPE" runat="server" class="form-control mControl">
                            <option value="LD">Landing</option>
                            <option value="O/F">Over Flight</option>
                        </select>

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="ddlFLIGHT_TYPE" class="mLable control-label">FLIGHT_TYPE</label>
                        <select id="ddlFLIGHT_TYPE" runat="server" class="form-control mControl">
                            <option value="SC">SC</option>
                            <option value="NO">NO</option>
                        </select>
                    </div>

                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for='<%= ddlPURPOSE.ClientID %>' class="mLable control-label">
                            PURPOSE                                
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlPURPOSE">
                        </asp:DropDownList>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="form-inline" role="form">
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for='<%= ddlCRAFT_ID.ClientID %>' class="mLable control-label">
                            CRAFT                              
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control mControl" ID="ddlCRAFT_ID">
                        </asp:DropDownList>

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">


                        <label for="txtDOF" class="mLable control-label">REAL_CRAFT</label>
                        <asp:DropDownList ID="ddlREAL_CRAFT_TYPE" runat="server" CssClass="form-control mControl">
                        </asp:DropDownList>


                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtVALIDHOURS" class="control-label mLable">VALIDHOURS</label>
                        <input id="txtVALIDHOURS" runat="server" type="text" data-number="true" maxlength="2"
                            name="mVALIDHOURS"
                            class="form-control mControl" />

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtFLIGHTDATE" class="mLable control-label">FLIGHTDATE</label>
                        <input id="txtFLIGHTDATE" runat="server" data-control="_Update" name="mFLIGHTDATE"
                            data-date-format="dd/mm/yyyy"
                            class="inputControl date-picker mControl" />

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="form-inline" role="form">
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtFLIGHTNBR" class="control-label mLable">FLIGHTNBR</label>
                        <input id="txtFLIGHTNBR" runat="server" data-control="_Update"
                            maxlength="20"
                            data-minlenght="1"
                            name="mFLIGHTNBR"
                            type="text" class="form-control mControl" />

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtREGISTRATION" class="mLable control-label">REGISTRATION</label>
                        <input id="txtREGISTRATION" runat="server" maxlength="20" data-control="_Update"
                            data-minlenght="1"
                            name="mREGISTRATION"
                            class="form-control mControl" type="text" />

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for='<%= ddlFROM_AIRP.ClientID %>' class="mLable control-label">
                            FROM_AIRP                                
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control mControl"
                            ID="ddlFROM_AIRP">
                        </asp:DropDownList>

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for='<%= ddlTO_AIRP.ClientID %>' class="mLable control-label">
                            TO_AIRP                                
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control mControl"
                            ID="ddlTO_AIRP">
                        </asp:DropDownList>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="form-inline" role="form">
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtETD" class="mLable control-label">ETD</label>
                        <input id="txtETD" runat="server" name="mETD" maxlength="4" data-control="_Update"
                            class="form-control mControl"
                            type="text" />

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">

                        <label for="txtETA" class="mLable control-label">
                            ETA
                        </label>
                        <input id="txtETA" runat="server" name="mETA" maxlength="4" data-control="_Update"
                            class="form-control mControl"
                            type="text" />

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        
                            <label for="txtATD" class="mLable control-label">ATD</label>
                            <input id="txtATD" runat="server" name="mATD"
                                class="form-control mControl" />
                        
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        <%--<div class="form-group">--%>
                        <label for="txtATA" class="mLable control-label">ATA</label>
                        <input id="txtATA" runat="server" maxlength="4" data-control="_Update"
                            data-minlenght="1"
                            name="mATA"
                            class="form-control mControl"
                            type="text" />
                        <%--</div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="form-inline" role="form">
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        <%--<div class="form-group">--%>
                        <label for="txtVIA" class="mLable control-label">VIA</label>
                        <input id="txtVIA" runat="server" name="mVIA"
                            class="form-control mControl"
                            type="text" />
                        <%--</div>--%>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        <%--<div class="form-group">--%>
                        <label for="txtCODE" class="mLable control-label">FPL_VIA</label>
                        <input id="txtFPL_VIA" runat="server" maxlength="500" data-control="_Update"
                            data-minlenght="1"
                            name="mCODE"
                            class="form-control mControl"
                            type="text" />
                        <%--</div>--%>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        <%--<div class="form-group">--%>
                        <label for="txtREMARK" class="mLable control-label">REMARK</label>
                        <input id="txtREMARK" runat="server" name="mREMARK"
                            class="form-control mControl" />
                        <%--</div>--%>
                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="form-group">
                    <div class="form-horizontal" role="form">
                        <%--<div class="form-group">--%>
                        <label for='<%= ddlOPER_ID.ClientID %>' class="mLable control-label">
                            OPER_ID                                
                        </label>
                        <asp:DropDownList runat="server" CssClass="form-control mControl"
                            ID="ddlOPER_ID">
                        </asp:DropDownList>
                        <%--</div>--%>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row" style="margin-top: 30px">
        <%--<asp:Button ID="btnCreate" runat="server" Text="Create" Visible="<%# !string.IsNullOrEmpty(Request.QueryString["Menu_ID"])
    %>" OnClick="btnCreate_Click" CssClass="btn btn-group-xs btn-primary"/>
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Visible="<%#
    string.IsNullOrEmpty(Request.QueryString["Menu_ID"]) %>" Text="Update" CssClass="btn
    btn-group-xs btn-primary"/>
        <a href="<%= Page.ResolveUrl("~/FinishFlights/FinishedFlights.aspx") + "?Menu_ID="
    + Request.QueryString["Menu_ID"] %>" class="btn btn-group-xs btn-primary"></a>--%>
        <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_Click"
            CssClass="btn btn-group-xs btn-primary" />
        <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="Update"
            CssClass="btn btn-group-xs btn-primary" />
        <a href="<%= Page.ResolveUrl("~/FinishFlights/FinishedFlights.aspx") + "?Menu_ID="
    + Request.QueryString["Menu_ID"] %>"
            class="btn btn-group-xs btn-primary">Exit</a>
    </div>

    <script src="../Style/assets/js/jquery-2.1.4.min.js"></script>
    <script src="../Style/assets/js/bootstrap.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datetimepicker.min.js"></script>
    <script src="../Style/assets/js/bootstrap-datepicker.min.js"></script>
    <script src="../Scripts/CustomDynamic.js"></script>
</asp:Content>
