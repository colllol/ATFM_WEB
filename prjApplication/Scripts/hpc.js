function CheckboxesAll(objRef, _id) {
    var GridView = document.getElementById(_id);
    var inputList = GridView.getElementsByTagName("input");
    for (var i = 0; i < inputList.length; i++) {
        var row = inputList[i].parentNode.parentNode;
        if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
            if (objRef.checked && inputList[i].disabled == false) {
                inputList[i].checked = true;
            }
            else {
                inputList[i].checked = false;
            }
        }
    }
}
function clickButton(e, buttonid) {
    var evt = e ? e : window.event;
    var bt = document.getElementById(buttonid);
    if (bt) {
        if (evt.keyCode == 13) {
            bt.click();
            return false;
        }
    }
}