<%@ Page Title="" Language="C#" MasterPageFile="~/Masters/ATFM_New.Master" AutoEventWireup="true"
    CodeBehind="Import.aspx.cs" Inherits="prjApplication.ImportData.Import" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .btnChooseFile {
            position: absolute;
            z-index: 2;
            top: 0;
            left: 0;
            filter: alpha(opacity=0);
            -ms-filter: 'progid:DXImageTransform.Microsoft.Alpha(Opacity=0)';
            opacity: 0;
            background-color: transparent;
            color: transparent;
        }
    </style>
    <div id="exTab1" class="container">
        <ul class="nav nav-pills">
            <li class="active">
                <a href="#1a" onclick="tab1_Load();" data-toggle="tab">Import form</a>
            </li>
            <li><a href="#2a" onclick="tab2_Load();" data-toggle="tab">List source</a>
            </li>
            <li><a href="#3a" onclick="tab3_Load();" data-toggle="tab">Attached table-source</a>
            </li>
            <li><a href="#4a" onclick="tab4_Load();" data-toggle="tab">Define form</a>
            </li>
        </ul>

        <div class="tab-content clearfix">
            <div class="tab-pane active" id="1a">
                <h3>Import data dynamic</h3>
                <div class="form-group">
                    <label class="control-label">Select</label>
                    <asp:DropDownList CssClass="form-control form-control-inline" AutoPostBack="true"  OnSelectedIndexChanged="ddlTableName_SelectedIndexChanged" runat="server" ID="ddlTableName">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="control-label">Source</label>
                    <asp:DropDownList CssClass="form-control form-control-inline" OnSelectedIndexChanged="ddlSourceData_SelectedIndexChanged" runat="server" ID="ddlSourceData">
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <label class="control-label">Select file</label>
                    <asp:FileUpload runat="server" ID="fileUpload" />
                    <%--<div style="position: relative;">
                        <a class='btn btn-primary' href='javascript:;'>Choose File...
			                <input type="file" id="fileUpload" runat="server" name="file_source" class="btnChooseFile"
                                onchange='$("#upload-file-info").html($(this).val());'>
                        </a>
                        &nbsp;
		            <span class='label label-info' id="upload-file-info"></span>
                    </div>--%>
                </div>
                <asp:Button ID="btnImport" runat="server" Text="Import data" OnClick="btnImport_Click"/>
                <asp:GridView runat="server" CssClass="table table-bordered" ID="grdNoImport"></asp:GridView>
            </div>
            <div class="tab-pane" id="2a">
                <h3>List source data</h3>

                <div class="col-lg-6">
                    <div class="form-group">
                        <label class="control-label">Name source data</label>
                        <input class="form-control" type="text" id="a2NameSource" />
                    </div>
                </div>
                <div class="row">
                    <button id="a2AddSource" onclick="a2AddSource_Onclick();" type="button">Add</button>
                    <button id="a2UpdateSource" onclick="a2UpdateSource_Onclick();" type="button">Update</button>
                    <button id="a2CancelSource" onclick="a2CancelSource_Onclick();" type="button">Cancel</button>
                </div>
                <div>
                    <h5>List</h5>
                    <asp:GridView runat="server" AutoGenerateColumns="false" CssClass="table table-bordered" ID="a2grdSourceData">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="action-buttons" style="width: 70px">
                                        <a data-toggle="tooltip" title='<%# "Edit source data: " + Eval("SOURCENAME") %>'>
                                            <i class="ace-icon fa fa-pencil bigger-130"
                                                onclick="SelectEdit_a2(<%# Eval("ID") %>);"></i>
                                        </a>
                                        <a data-toggle="tooltip" title='<%# "Delete source data: " + Eval("SOURCENAME") %>'>
                                            <i class="ace-icon fa fa-trash-o bigger-130" onclick="a2DeleteOnclick(<%# Eval("ID") %>);">
                                            </i>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="SOURCENAME" HeaderText="Name source" />
                        </Columns>
                    </asp:GridView>
                </div>

            </div>
            <div class="tab-pane" id="3a">
                <h3>Table data - Source data</h3>
                <div>
                    <div class="col-lg-8">
                        <div class="form-inline" role="form">
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label class="control-label">Select</label>
                                    <asp:DropDownList runat="server" ID="a3TableName" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label class="control-label">Select</label>
                                    <asp:DropDownList runat="server" ID="a3SourceData" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-8">
                        <div class="form-horizontal" role="form">
                            <div class="form-group">
                                <label class="control-label">Alias table name</label>
                                <input class="form-control" id="a3Alias" type="text" />
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-lg-8">
                        <button id="a3btnAddSource" onclick="a3AddSource_Onclick();" type="button">Add</button>
                        <button id="a3btnUpdateSource" onclick="a3UpdateSource_Onclick();" type="button">Update</button>
                        <button id="a3btnCancelSource" onclick="a3CancelSource_Onclick();" type="button">
                            Cancel</button>
                    </div>
                </div>
                <div>
                    <h5>List</h5>
                    <asp:GridView runat="server" ID="a3grd" AutoGenerateColumns="false" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <div class="action-buttons" style="width: 70px">
                                        <a data-toggle="tooltip" title="Edit">
                                            <i class="ace-icon fa fa-pencil bigger-130"
                                                onclick="SelectEdit_a3(<%# Eval("ID") %>);"></i>
                                        </a>
                                        <a data-toggle="tooltip" title="Delete">
                                            <i class="ace-icon fa fa-trash-o bigger-130" onclick="a3DeleteOnclick(<%# Eval("ID") %>);">
                                            </i>
                                        </a>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Name table" DataField="VTABLENAME" />
                            <asp:BoundField HeaderText="Name source" DataField="SOURCENAME" />
                            <asp:BoundField HeaderText="Alias" DataField="VTABLENAMEALIAS" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="tab-pane" id="4a">
                <h3>Define data
                </h3>
                <div class="row">
                    <div class="col-lg-8">
                        <div class="form-inline" role="form">
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label class="control-label">Select</label>
                                    <asp:DropDownList runat="server" onchange="a4TableName_change();" ID="a4TableName"
                                        CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-horizontal" role="form">
                                <div class="form-group">
                                    <label class="control-label">Select</label>
                                    <asp:DropDownList runat="server" onchange="a4SourceName_change();" ID="a4SourceName"
                                        CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <button id="a4btnAddSource" onclick="a4btnAddSource_Onclick();" type="button">Add</button>
                    <button id="a4btnUpdateSource" hidden="hidden" type="button">Update</button>
                    <button id="a4btnCancelSource" onclick="a4btnCancelSource_Onclick();" type="button">
                        Clear</button>
                </div>
                <div>
                    <h5>List</h5>
                    <asp:GridView runat="server" AutoGenerateColumns="false" ID="a4grd" CssClass="table table-bordered">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Column name" DataField="sColName" />
                            <asp:BoundField HeaderText="Data type" DataField="sDataType" />
                            <asp:TemplateField HeaderText="Alias">
                                <ItemTemplate>
                                    <input id="a4sColAlias" data-data-type='<%# Eval("sDataType") %>' data-columnname='<%# Eval("sColName") %>'
                                        runat="server" value='<%# Eval("VCOLNAMEALIAS") %>' class="form-control" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <!--##-->
    <script>
        var objNguonDuLieu = JSON.parse('<%= _objNguonDuLieu%>');
        var objTable = JSON.parse('<%= _objTable%>');
        var phancach = '<%= _PhanCach%>';
        var phancachArg = '<%= _PhanCachArg%>';

        function DisplayResult(resulf, context) {
            if (context == 'SelectEdit_a2') {
                objNguonDuLieu = JSON.parse(resulf);
                a2NameSource.value = objNguonDuLieu['SOURCENAME'];
            }
            if (context == 'a2grdSourceData_Load') {
                a2grdSourceData.innerHTML = resulf;
            }
            if (context == 'a2DeleteOnclick') {
                if (resulf == 'OK') {
                    alert('Delete sussess!');
                }
                else { alert('Delete error!'); }
                a2grdSourceData_Load();
            }
            if (context == 'a2AddSource_Onclick') {
                if (resulf == 'OK') {
                    alert('Insert sussess!');
                }
                else { alert('Insert error!'); }
                a2grdSourceData_Load();
            }
            if (context == 'a2UpdateSource_Onclick') {
                if (resulf == 'OK') {
                    alert('Update sussess!');
                }
                else { alert('Update error!'); }
                a2grdSourceData_Load();
            }
            if (context == 'SelectEdit_a3') {
                a3LoadInfoObject(resulf);

            }
            if (context == 'a3AddSource_Onclick') {
                if (resulf == 'OK') { alert('Insert sussess!') }
                else { alert('Insert error!'); }
                a3grdSourceData_Load();
            }
            if (context == 'a3grdSourceData_Load') {
                a3grd.innerHTML = resulf;
            }
            if (context == 'a3UpdateSource_Onclick') {
                if (resulf = 'OK') { alert('Update sussess!'); }
                else { alert('Update error!'); }
                a3grdSourceData_Load();
            }
            if (context == 'a3DeleteOnclick') {
                if (resulf = 'OK') { alert('Delete sussess!'); }
                else { alert('Delete error!'); }
                a3grdSourceData_Load();
            }
            if (context == 'a3TableName_Load') {
                a3TableName.innerHTML = resulf;
            }
            if (context == 'a3SourceData_Load') {
                a3SourceData.innerHTML = resulf;
            }
            if (context == 'a4SourceName_Load') {
                if (resulf != '')
                    a4SourceName.innerHTML = resulf;
                a4SourceName_change();
            }
            if (context == 'a4grd_Load') {
                if (resulf != '')
                    a4grd.innerHTML = resulf;
            }
            if (context == 'a4btnAddSource_Onclick') {
                if (resulf == 'OK') alert('Update sussess!');
                else alert('Update error!');
            }
            if (context == 'a4TableName_Load') {
                a4TableName.innerHTML = resulf;
            }
        }
        function setSelectedValue(idSelected, valueToSet) {
            var selectObj = document.getElementById(idSelected);
            if (valueToSet == null) return;
            if (selectObj.options.length <= 0) return;
            for (var i = 0; i < selectObj.options.length; i++) {
                if (selectObj.options[i].value === valueToSet || selectObj.options[i].text === valueToSet) {
                    selectObj.options[i].selected = true;
                    return;
                }
            }
        }
        function tab1_Load() {
            location.reload();            
        }
        function tab2_Load() {
            a2IDSelect = 0;
            a2grdSourceData_Load();
        }
        function tab3_Load() {
            a3IDSelect = 0;
            a3TableName_Load();
            a3SourceData_Load();
            a3grdSourceData_Load();
        }
        function tab4_Load() {
            a4TableName_Load();
            a4TableName_change();
        }

    </script>
    <!--#1-->
    <script>
        var ddlTableName = document.getElementById('<%= ddlTableName.ClientID%>');
        var ddlSourceData = document.getElementById('<%= ddlSourceData.ClientID%>');
        var btnImport = document.getElementById('btnImport');
        var fileUpload = document.getElementById('<%=fileUpload.ClientID%>');
        function ImportFile() {
            if (ddlTableName.value == '' || ddlSourceData.value == '') {
                alert('Please select table & source!');
                return;
            }                
            if (_path.innerText != '')
                GetArgWithPostBack(ddlTableName.value + phancach + ddlSourceData.value + phancach + _path + phancachArg + 'ImportFile', 'ImportFile');
            else alert('Please select file!');
        }
    </script>
    <!-- #2 -->
    <script>
        var a2NameSource = document.getElementById('a2NameSource');
        var a2AddSource = document.getElementById('a2AddSource');
        var a2UpdateSource = document.getElementById('a2UpdateSource');
        a2UpdateSource.setAttribute('disabled', 'disabled');
        var a2CancelSource = document.getElementById('a2CancelSource');
        var a2grdSourceData = document.getElementById('<%= a2grdSourceData.ClientID%>');
        var a2IDSelect = 0;
        function SelectEdit_a2(_id) {
            a2IDSelect = _id;
            a2UpdateSource.removeAttribute('disabled');
            a2AddSource.setAttribute('disabled', 'disabled');
            GetArgWithPostBack(_id + phancachArg + 'SelectEdit_a2', 'SelectEdit_a2');
        }
        function a2CancelSource_Onclick() {
            a2IDSelect = 0;
            a2NameSource.value = '';
            a2AddSource.removeAttribute('disabled');
            a2UpdateSource.setAttribute('disabled', 'disabled');
        }
        function a2AddSource_Onclick() {
            GetArgWithPostBack(a2NameSource.value + phancachArg + 'a2AddSource_Onclick', 'a2AddSource_Onclick');
        }
        function a2UpdateSource_Onclick() {
            objNguonDuLieu['ID'] = a2IDSelect;
            objNguonDuLieu['SOURCENAME'] = a2NameSource.value;
            GetArgWithPostBack(JSON.stringify(objNguonDuLieu) + phancachArg + 'a2UpdateSource_Onclick', 'a2UpdateSource_Onclick');
        }
        function a2DeleteOnclick(_id) {
            var rs = confirm('Do you want delete row?');
            if (rs) {
                GetArgWithPostBack(_id + phancachArg + 'a2DeleteOnclick', 'a2DeleteOnclick');
            }
        }
        function a2grdSourceData_Load() {
            GetArgWithPostBack('asd' + phancachArg + 'a2grdSourceData_Load', 'a2grdSourceData_Load');
        }
    </script>

    <!--#3-->
    <script>
        var a3TableName = document.getElementById('<%= a3TableName.ClientID%>');
        var a3SourceData = document.getElementById('<%= a3SourceData.ClientID%>');
        var a3Alias = document.getElementById('a3Alias');
        var a3btnAddSource = document.getElementById('a3btnAddSource');
        var a3btnUpdateSource = document.getElementById('a3btnUpdateSource');
        a3btnUpdateSource.setAttribute('disabled', 'disabled');
        var a3btnCancelSource = document.getElementById('a3btnCancelSource');
        var a3grd = document.getElementById('<%= a3grd.ClientID%>');
        var a3IDSelect = 0;
        function a3TableName_Load() {
            GetArgWithPostBack('a3TableName_Load' + phancachArg + 'a3TableName_Load', 'a3TableName_Load');
        }
        function a3SourceData_Load() {
            GetArgWithPostBack('a3SourceData_Load' + phancachArg + 'a3SourceData_Load', 'a3SourceData_Load');
        }
        function SelectEdit_a3(_id) {
            a3IDSelect = _id;
            a3btnUpdateSource.removeAttribute('disabled');
            a3btnAddSource.setAttribute('disabled', 'disabled');
            GetArgWithPostBack(_id + phancachArg + 'SelectEdit_a3', 'SelectEdit_a3');
        }
        function a3CancelSource_Onclick() {
            a3IDSelect = 0;
            a3Alias.value = '';
            a3btnAddSource.removeAttribute('disabled');
            a3btnUpdateSource.setAttribute('disabled', 'disabled');
        }
        function a3AddSource_Onclick() {
            objTable['VTABLENAME'] = a3TableName.options[a3TableName.selectedIndex].text;
            objTable['VTABLENAMEALIAS'] = a3Alias.value;
            objTable['NSOURCEDATA'] = a3SourceData.value;
            GetArgWithPostBack(JSON.stringify(objTable) + phancachArg + 'a3AddSource_Onclick', 'a3AddSource_Onclick');
        }
        function a3UpdateSource_Onclick() {
            objTable['ID'] = a3IDSelect;
            objTable['VTABLENAME'] = a3TableName.value;
            objTable['VTABLENAMEALIAS'] = a3Alias.value;
            objTable['NSOURCEDATA'] = a3SourceData.value;
            GetArgWithPostBack(JSON.stringify(objTable) + phancachArg + 'a3UpdateSource_Onclick', 'a3UpdateSource_Onclick');
        }
        function a3DeleteOnclick(_id) {
            var rs = confirm('Do you want delete row?');
            if (rs) {
                GetArgWithPostBack(_id + phancachArg + 'a3DeleteOnclick', 'a3DeleteOnclick');
            }
        }
        function a3grdSourceData_Load() {
            GetArgWithPostBack('asd' + phancachArg + 'a3grdSourceData_Load', 'a3grdSourceData_Load');
        }
        function a3LoadInfoObject(val) {
            objTable = JSON.parse(val);
            setSelectedValue(a3TableName.id, objTable['VTABLENAME']);
            setSelectedValue(a3SourceData.id, objTable['NSOURCEDATA']);
            a3Alias.value = objTable['VTABLENAMEALIAS'];
        }
    </script>

    <!--#4-->
    <script>
        var objDinhNghia = JSON.parse('<%= _objDinhNghia%>');
        var a4TableName = document.getElementById('<%= a4TableName.ClientID%>');
        var a4SourceName = document.getElementById('<%= a4SourceName.ClientID%>');
        var a4btnAddSource = document.getElementById('a4btnAddSource');
        var a4btnUpdateSource = document.getElementById('a4btnUpdateSource');
        var a4grd = document.getElementById('<%= a4grd.ClientID%>');
        function a4btnCancelSource_Onclick() {
            var elems = a4grd.getElementsByTagName('input');
            for (var i = 0; i < elems.length; i++) {
                $(elems[i]).val('');
            }
        }
        function a4TableName_Load() {
            GetArgWithPostBack('a4TableName_Load' + phancachArg + 'a4TableName_Load', 'a4TableName_Load');
        }
        function a4SourceName_Load(idTable) {
            GetArgWithPostBack(idTable + phancachArg + 'a4SourceName_Load', 'a4SourceName_Load');
        }
        function a4TableName_change() {
            a4SourceName_Load(a4TableName.options[a4TableName.selectedIndex].text);            
        }
        function a4SourceName_change() {
            a4grd_Load(a4TableName.options[a4TableName.selectedIndex].text, a4SourceName.value);
        }
        function a4grd_Load(idtable, idsource) {
            GetArgWithPostBack(idtable + phancach + idsource + phancachArg + 'a4grd_Load', 'a4grd_Load');
        }
        function a4btnAddSource_Onclick() {
            GetArgWithPostBack(a4TableName.options[a4TableName.selectedIndex].text + phancach + a4SourceName.value + phancachArg + 'objDinhNghia_Delete', 'objDinhNghia_Delete');
            var elems = a4grd.getElementsByTagName('input');
            for (var i = 0; i < elems.length; i++) {
                var ele = elems[i];
                if (ele.value != '') {
                    objDinhNghia['NIDTABLE'] = a4TableName.options[a4TableName.selectedIndex].text;
                    objDinhNghia['NSOURCEDATA'] = a4SourceName.value;
                    objDinhNghia['VCOLNAME'] = ele.getAttribute('data-ColumnName');
                    objDinhNghia['VCOLNAMEALIAS'] = ele.value;
                    objDinhNghia['VCOLDATATYPE'] = ele.getAttribute('data-data-type');
                    GetArgWithPostBack(JSON.stringify(objDinhNghia) + phancachArg + 'a4btnAddSource_Onclick', 'a4btnAddSource_Onclick');
                }
            }
        }
    </script>
</asp:Content>
