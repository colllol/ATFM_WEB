CKEDITOR.plugins.add('WaterMark', {
    init: function(editor) {
        editor.addCommand('callcommand', {
            exec: function(editor) {
                //alert("VÀO ĐÂY");
                var vHeight = 650;
                var vWidth = 1000;
                winDef = 'scrollbars=yes,scrolling=yes,location=no,toolbar=no,height='.concat(vHeight).concat(',').concat('width=').concat(vWidth).concat(',');
                winDef = winDef.concat('top=').concat((screen.height - vHeight) / 2).concat(',');
                winDef = winDef.concat('left=').concat((screen.width - (vWidth)) / 2);
                
                editor.focus();
                var selection = editor.document.getSelection();
                var selectedContent = '';
                if (selection.getType() == CKEDITOR.SELECTION_ELEMENT) {
                   
                    var start_element = editor.getSelection().getStartElement();
                    for (i = 0; i < start_element.$.attributes.length; i++) {
                        if (start_element.$.attributes[i].name.indexOf('src') > -1) {
                            selectedContent = start_element.$.attributes[i].value;
                        }
                    }

                } 
                  //else if (selection.getType() == CKEDITOR.SELECTION_TEXT) {

//                    if (CKEDITOR.env.ie) {
//                        selection.unlock(true);
//                        selectedContent = selection.getNative().createRange().text;
//                    } else {
//                        selectedContent = selection.getNative();
//                    }
//                }
                if (selectedContent!='')
                    window.open('../ckeditor/plugins/WaterMark/WaterMark.aspx?objtext=' + selectedContent + '&editorID=' + editor.name, 'WaterMark', winDef);
            }
        });

        if (editor.addMenuItem) {
            editor.addMenuGroup('testgroup');
            editor.addMenuItem('testitem', {
                label: 'Đóng dấu ảnh',
                command: 'callcommand',
                icon: this.path + 'map-magnify-icon.png',
                group: 'testgroup'

            });

        }

        if (editor.contextMenu) {
            editor.contextMenu.addListener(function(element, selection) {
                return { testitem: CKEDITOR.TRISTATE_ON };
            });
        }

    }
});
