<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true" CodeBehind="EditCRAFT.aspx.cs" Inherits="prjApplication.Static.Craft.EditCRAFT" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link href="../../Style/Style_List.css" rel="stylesheet" />
    <div id="body-wrapper">
        <div id="main-content">
            <div class="content-box">
                <div class="content-box-header"><h3>+ EDIT CRAFT</h3></div>
                <div class="content-box-content">
                    <table class="input">
                        <tbody>
                            <tr>
                                <td class="tdinput" style="text-align:right;font-weight:bold;">CRAFT_ICAO:</td>
                                <td class="tdinput tdwidth">
                                    <asp:TextBox ID="txtCraftIcao" runat="server" MaxLength="200" CssClass="text-input" Width="40%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdinput" style="text-align:right;font-weight:bold;">CRAFT_IATA:</td>
                                <td class="tdinput tdwidth">
                                    <asp:TextBox ID="txtCraftIata" runat="server" MaxLength="200" CssClass="text-input" Width="40%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdinput" style="text-align:right;font-weight:bold;">CRAFT_NAME:</td>
                                <td class="tdinput tdwidth">
                                    <asp:TextBox ID="txtCraftName" runat="server" MaxLength="200" CssClass="text-input" Width="60%" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td><label runat="server" id="lblResult" style="font-weight:700;color:red;"></label></td>
                            </tr>
                        </tbody>
                    </table>
                    <div class="clear clearHeight"></div>
                    <div id="khung"><div id="main">
                        <asp:LinkButton runat="server" ID="btnSave" CssClass="btn btn-sm btn-primary btn-bold btn-round" OnClick="btnSave_Click">
                            <i class="ace-icon fa fa-floppy-o bigger-120 white"></i> Save
                        </asp:LinkButton>
                        <asp:LinkButton runat="server" ID="btnExit" CssClass="btn btn-sm btn-warning btn-round" CausesValidation="false" OnClick="btnExit_Click">
                            <i class="ace-icon fa fa-times white"></i> Exit
                        </asp:LinkButton>
                    </div></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
