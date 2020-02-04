
$(function () {
    $("#mytabs").tabs({
        width: $("#mytabs").parent().width(),
        height: $(window).height() - 5
    });

    $('#mytabs').tabs('getTab', "001").panel('options').tab.hide();
    //$('#mytabs').tabs('getTab', "002").panel('options').tab.hide();

//    for (var i = parseInt($('#HiddenVs').val()); i < parseInt($('#HiddenVe').val()); i++) {
//        var title = ("000" + i).substr(-3);
//        alert(title);
//    }
});