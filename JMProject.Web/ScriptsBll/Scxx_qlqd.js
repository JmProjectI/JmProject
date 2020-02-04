
/**
* 文本框根据输入内容自适应高度
* @param                {HTMLElement}        输入框元素
* @param                {Number}                设置光标与输入框保持的距离(默认0)
* @param                {Number}                设置最大高度(可选)
*/
var autoTextarea = function (elem, extra, maxHeight) {
    extra = extra || 0;
    var isFirefox = !!document.getBoxObjectFor || 'mozInnerScreenX' in window,
        isOpera = !!window.opera && !!window.opera.toString().indexOf('Opera'),
                addEvent = function (type, callback) {
                    elem.addEventListener ?
                                elem.addEventListener(type, callback, false) :
                                elem.attachEvent('on' + type, callback);
                },
                getStyle = elem.currentStyle ? function (name) {
                    var val = elem.currentStyle[name];

                    if (name === 'height' && val.search(/px/i) !== 1) {
                        var rect = elem.getBoundingClientRect();
                        return rect.bottom - rect.top -
                                        parseFloat(getStyle('paddingTop')) -
                                        parseFloat(getStyle('paddingBottom')) + 'px';
                    };

                    return val;
                } : function (name) {
                    return getComputedStyle(elem, null)[name];
                },
                minHeight = parseFloat(getStyle('height'));

    elem.style.resize = 'none';

    var change = function () {
        var scrollTop, height,
                        padding = 0,
                        style = elem.style;

        if (elem._length === elem.value.length) return;
        elem._length = elem.value.length;

        if (!isFirefox && !isOpera) {
            padding = parseInt(getStyle('paddingTop')) + parseInt(getStyle('paddingBottom'));
        };
        scrollTop = document.body.scrollTop || document.documentElement.scrollTop;

        elem.style.height = minHeight + 'px';
        if (elem.scrollHeight > minHeight) {
            if (maxHeight && elem.scrollHeight > maxHeight) {
                height = maxHeight - padding;
                style.overflowY = 'auto';
            } else {
                height = elem.scrollHeight - padding;
                style.overflowY = 'hidden';
            };
            style.height = height + extra + 'px';
            scrollTop += parseInt(style.height) - elem.currHeight;
            document.body.scrollTop = scrollTop;
            document.documentElement.scrollTop = scrollTop;
            elem.currHeight = parseInt(style.height);
        };
    };

    addEvent('propertychange', change);
    addEvent('input', change);
    addEvent('focus', change);
    change();
};

$.fn.autoHeight = function () {

    this.each(function () {
        autoTextarea(this);
    });
}

function CreateQLQD() {
    var postData = {
        nkid: $("#Hiddenid").val()
    }
    $.ajax({
        type: "post",
        url: "/Word/Create_QLQD",
        data: postData,
        dataType: "json",
        success: function (data) {
            if (data.length < 1) {
                $.messager.alert("提示信息", "无可生成新领导");
            }
            var aHtml = '';
            $.each(data, function (index, item) {
                var trHTML = '<tr group="' + item.leder + '"><td class="con" colspan="2">' +
                             '  <input type="checkbox" name="ckbqlqd" id="ckb_qln' + index + '" /><label for="ckb_qln' + index + '">' + item.leder + '</label> 职权清单' +
                             '</td></tr>';
                $.each(item.qlqds, function (idx, qlitem) {
                    trHTML += '<tr group="' + item.leder + '"><td class="dec">' + qlitem.qlsxname + '</td><td class="con">' +
                            '<textarea autoHeight="true" class="textarea" id="' + qlitem.leder + '_' + qlitem.qlsx + '_' + qlitem.qlsxname + '_' + item.leder + '_' + qlitem.qlsort + '" style="font-size: 12px; width: 1050px;" row="1">' + qlitem.qltext + '</textarea>' +
                            '</td></tr>';
                });
                aHtml += trHTML;
            });
            $("#table_qlqd").append(aHtml);

            $('textarea[autoHeight]').autoHeight();
        }
    });
};

function removeQLQD() {
    var checkStr = '';
    $("input[name='ckbqlqd']:checked").each(function (i) {
        var val = $(this).next('label').html();
        checkStr = checkStr + val + "-";
    });
    if (checkStr.length < 1) {
        $.messager.alert("提示信息", '请选择要删除的领导');
        return;
    }
    $.messager.confirm('系统提示', '确定要删除吗？', function (r) {
        if (r) {
            $.post("/Word/Delete_Qlqd?nkid=" + $("#Hiddenid").val() + "&leder=" + checkStr, function (data) {
                if (data.type == 1) {
                    $("input[name='ckbqlqd']:checked").each(function (i) {
                        var val = $(this).next('label').html();
                        $("#table_qlqd tr[group='" + val + "']").remove();
                    });
                }
                else {
                    $.messager.alert("提示信息", data.message);
                }
            }, "json");
        }
    });
};

function removeAll() {
    $.messager.confirm('系统提示', '确定要删除所有权力清单吗？', function (r) {
        if (r) {
            $.post("/Word/Delete_QlqdAll?nkid=" + $("#Hiddenid").val(), function (data) {
                if (data.type == 1) {
                    $("#table_qlqd tr").remove();
                }
                else {
                    $.messager.alert("提示信息", data.message);
                }
            }, "json");
        }
    });
}