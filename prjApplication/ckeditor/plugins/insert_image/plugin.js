CKEDITOR.plugins.add('insert_image',
{
    init: function(editor) {
        var pluginName = 'insert_image';
        editor.ui.addButton('insert_image',
            {
                label: 'Chèn Ảnh, Flash, Media...',
                icon: this.path + 'Photo.png',
                command: pluginName,
                click: function(editor) {
                    var vHeight = 600;
                    var vWidth = 900;
                    var sel = editor.getSelection();
                    if (sel != null)
                        var element = sel.getSelectedText();
                    else
                        var element ='';
                      
                    winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                    winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                    winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                    if (element!='')
                        window.open('../ckeditor/plugins/insert_image/InsertImage.aspx?textselect=' + element + '&editorID=' + editor.name, 'ImageGallery', winDef);
                    else
                        window.open('../ckeditor/plugins/insert_image/InsertImage.aspx?editorID=' + editor.name, 'ImageGallery', winDef);
                }
            });
    }
});
