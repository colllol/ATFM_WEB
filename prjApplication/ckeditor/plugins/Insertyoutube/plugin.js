CKEDITOR.plugins.add('Insertyoutube',
{
    init: function(editor) {
    var pluginName = 'Insertyoutube';
    editor.ui.addButton('Insertyoutube',
            {
                label: 'Chèn Youtube vào nội dung',
                icon: this.path + 'FVideo-icon.png',
                command: pluginName,
                click: function(editor) {
                var vHeight = 280;
                var vWidth = 400;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                window.open('../ckeditor/plugins/Insertyoutube/Insertyoutube.aspx?editorID=' + editor.name, 'Insertyoutube', winDef); 
                }
            });
    }
});