<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="Reports.aspx.cs" Inherits="prjApplication.Common.Reports" %>

<%@ Register TagPrefix="cc1" Namespace="prjComponents.UI.WebControls" Assembly="prjComponents" %>
<%@ Import Namespace="prjComponents" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <table>
        <tr>
            <td>
                <div id="visualization" style="width: 1100px; height: 400px;"></div>
            </td>
                                
        </tr>
          <tr> 
              <td>
                <div id="visualization1" style="width: 1100px; height: 400px;"></div>
            </td>
                          
        </tr>

    </table>
    
</asp:Content>
