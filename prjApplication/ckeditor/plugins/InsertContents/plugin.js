CKEDITOR.plugins.add('InsertContents',
{
    init: function(editor) {
    var pluginName = 'InsertContents';
    editor.ui.addButton('InsertContents',
            {
                label: 'Chèn nội dung mẫu',
                icon: this.path + 'Copy-icon.png',
                command: pluginName,
                click: function(editor) {
                var vHeight = 560;
                var vWidth = 900;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                window.open('../ckeditor/plugins/InsertContents/InsertContents.aspx?editorID=' + editor.name, 'InsertContents', winDef); 
                }
            });
    }
});