CKEDITOR.plugins.add('InsertNewsRelates',
{
    init: function(editor) {
    var pluginName = 'InsertNewsRelates';
    editor.ui.addButton('InsertNewsRelates',
            {
                label: 'Chèn bài liên quan',
                icon: this.path + 'editnews.gif',
                command: pluginName,
                click: function(editor) {
                var vHeight = 580;
                var vWidth = 1000;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                window.open('../ckeditor/plugins/InsertNewsRelates/InsertNewsRelates.aspx?editorID=' + editor.name, 'InsertNewsRelates', winDef); 
                }
            });
    }
});