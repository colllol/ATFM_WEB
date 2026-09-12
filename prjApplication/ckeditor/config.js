/*
Copyright (c) 2003-2013, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config )
{
	 config.uiColor = '#F0F0F0';
    config.enterMode = CKEDITOR.ENTER_BR;
    config.shiftEnterMode = CKEDITOR.ENTER_P;
    CKEDITOR.config.toolbar_Basic = [
                  ['Source', '-', 'PasteText', '-', 'Bold', 'Italic', 'Underline', '-', 'Subscript', 'Superscript']
                ]

    config.toolbar_Tomtat = [
		      ['Source', 'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Bold', 'Italic', 'Underline', 'Undo', 'Redo', 'TextColor', 'BGColor', 'JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
		     , '/', ['Font', 'FontSize', 'RemoveFormat']

    ];
    CKEDITOR.config.toolbar_Sapo = [
                  ['Source', '-', 'PasteText', '-', 'Bold', 'Italic', 'Underline', '-', 'Subscript', 'Superscript', 'InsertNewsRelates']
                ]
    CKEDITOR.config.toolbar_Comment = [
                  ['Source', '-', 'PasteText', '-', 'Bold', 'Italic', 'Underline', '-', 'Subscript', 'Superscript']
                ]
    config.toolbar_Noidung = [
		      ['Source'],
              ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print'],
              ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],              
              ['Bold', 'Italic', 'Underline', 'StrikeThrough', '-', 'Subscript', 'Superscript'],
              ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
              '/',
              ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
              ['Link', 'Unlink'],
              ['Table', 'HorizontalRule', 'SpecialChar', 'PageBreak'],              
              ['Styles', 'Format'], ['TextColor', 'BGColor'],              
              ['Font', 'FontSize', 'Maximize'],
              ['insert_image', 'Insert_Variables', 'InsertNewsRelates', 'InsertContents', 'InsertAds', 'Insertyoutube', 'Insertslideimage', 'InsertViewsBNN', 'WaterMark', 'InsertImage2', 'InsertAdsContent'] //Button added to the toolbar
    ];
    config.removePlugins = 'iframe';
    config.toolbar = 'Basic';
    config.entities = false;
    config.indentUnit = 'em';
    config.indentOffset = 5;
    config.extraPlugins = 'insert_image,Insert_Variables,InsertNewsRelates,InsertContents,InsertAds,Insertyoutube,Insertslideimage,InsertViewsBNN,WaterMark,InsertImage2,InsertAdsContent';
    config.protectedSource.push(/<ins[\s|\S]+?<\/ins>/g); //Add thẻ ins quang cao google
};
