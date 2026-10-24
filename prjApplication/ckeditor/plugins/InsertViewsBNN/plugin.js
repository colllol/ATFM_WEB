CKEDITOR.plugins.add('InsertViewsBNN',
{
    init: function(editor) {
        var pluginName = 'InsertViewsBNN';
        editor.ui.addButton('InsertViewsBNN',
            {
                label: 'Xem Trước Bài Viết',
                icon: this.path + 'edit-find.png',
                command: pluginName,
                click: function(editor) {
                    chitiettin();
                }
            });
    }
});