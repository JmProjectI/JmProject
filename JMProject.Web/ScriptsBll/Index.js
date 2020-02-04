$(function () {
    InitMenu();

    //安全退出
    $('#btnExit').click(function () {
        $.ajax({
            url: "/Account/Exit",
            type: "Post",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data.type == 1) {
                    window.location = "/Admin";
                }
            }
        });
    });

    //个人信息
    $('#btnMy').click(function () {
        $("#dlgMy").dialog("open").dialog('setTitle', '个人信息');
        $("#fmMy").form("clear");
        $.ajax({
            url: "/Account/GetUsers",
            type: "Post",
            data: {},
            dataType: "json",
            success: function (data) {
                if (data != null) {
                    $("#Id").val(data.Id);
                    $("input[name='Name']").val(data.Name);
                    $("input[name='ZsName']").val(data.ZsName);
                    $("input[name='Phone']").val(data.Phone);
                    $("input[name='Tel']").val(data.Tel);
                    $("input[name='IcCard']").val(data.IcCard);
                    $("#Birthday").datebox('setValue', data.Birthday);
                    $("input[name='Address']").val(data.Address);
                    $("textarea[name='Remake']").val(data.Remake);
                }
            }
        });
    });

    //修改密码
    $('#btnPwd').click(function () {
        $("#dlgPWD").dialog("open").dialog('setTitle', '修改密码');
        $("#fmPWD").form("clear");
    });

    /*为选项卡绑定右键*/
    $(document).on('contextmenu', ".tabs li", function (e) {
        /*选中当前触发事件的选项卡 */
        var subtitle = $(this).text();
        $('#mainTab').tabs('select', subtitle);
        //显示快捷菜单
        $('#tab_menu').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
        return false;
    });

    /* 左侧菜单事件 */
    $(document).on("click", "a.xj-menu", function () {
        var url = $(this).attr('url');
        var icon = $(this).attr('iconcls');
        var tabTitle = $(this).find(".l-btn-text").html();
        addTab(tabTitle, url, icon);
    });

    $('#tab_menu-tabrefresh').click(function () {
        /*重新设置该标签 */
        var url = $(".tabs-panels .panel").eq($('.tabs-selected').index()).find("iframe").attr("src");
        $(".tabs-panels .panel").eq($('.tabs-selected').index()).find("iframe").attr("src", url);
    });
    //关闭当前
    $('#tab_menu-tabclose').click(function () {
        var currtab_title = $('.tabs-selected .tabs-inner span').text();
        $('#mainTab').tabs('close', currtab_title);
        if ($(".tabs li").length == 0) {
            //open menu
            $(".layout-button-right").trigger("click");
        }
    });
    //全部关闭
    $('#tab_menu-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            if ($(this).parent().next().is('.tabs-close')) {
                var t = $(n).text();
                $('#mainTab').tabs('close', t);
            }
        });
        //open menu
        $(".layout-button-right").trigger("click");
    });
    //关闭除当前之外的TAB
    $('#tab_menu-tabcloseother').click(function () {
        var currtab_title = $('.tabs-selected .tabs-inner span').text();
        $('.tabs-inner span').each(function (i, n) {
            if ($(this).parent().next().is('.tabs-close')) {
                var t = $(n).text();
                if (t != currtab_title)
                    $('#mainTab').tabs('close', t);
            }
        });
    });
    //关闭当前右侧的TAB
    $('#tab_menu-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            $.messager.alert('提示', '前面没有了!', 'warning');
            return false;
        }
        nextall.each(function (i, n) {
            if ($('a.tabs-close', $(n)).length > 0) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#mainTab').tabs('close', t);
            }
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#tab_menu-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            $.messager.alert('提示', '后面没有了!', 'warning');
            return false;
        }
        prevall.each(function (i, n) {
            if ($('a.tabs-close', $(n)).length > 0) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#mainTab').tabs('close', t);
            }
        });
        return false;
    });
});

//修改密码
function UpPass() {
    var validate = $("#fmPWD").form('validate');
    if (validate == false) {
        return false;
    };

    var postData = {
        opassword: $("#opassword").val()
        , password: $("#password").val()
    };
    //异步实现添加信息
    $.post("/Account/UpdatePWD", postData, function (result) {
        if (result.type == "1") {
            $.messager.show({   // show error message 
                title: '系统提示',
                msg: result.message
            });
            $("#dlgPWD").dialog("close");
        }
        else {
            $.messager.alert("提示信息", result.message);
        }
    });
}

//修改个人信息
function UpMy() {
    var validate = $("#fmMy").form('validate');
    if (validate == false) {
        return false;
    };

    var postData = {
        Id: $("#Id").val(),
        Name: $("input[name='Name']").val(),
        ZsName: $("input[name='ZsName']").val(),
        Phone: $("input[name='Phone']").val(),
        Tel: $("input[name='Tel']").val(),
        IcCard: $("input[name='IcCard']").val(),
        Birthday: $("#Birthday").val(),
        Address: $("input[name='Address']").val(),
        Remake: $("textarea[name='Remake']").val()
    };
    //异步实现添加信息
    $.post("/Account/UpdateMy", postData, function (result) {
        if (result.type == "1") {
            $.messager.show({   // show error message 
                title: '系统提示',
                msg: result.message
            });
            $("#dlgMy").dialog("close");
        }
        else {
            $.messager.alert("提示信息", result.message);
        }
    });
}


//加载菜单
function InitMenu() {
    //清楚所有
    var pnl = $("#AccMenu").accordion("panels");
    var titles = '';
    if (pnl) {
        $.each(pnl, function (i) {
            var title = pnl[i].panel("options").title;
            titles += title + ',';
        })
    }
    var arr_title = new Array();
    arr_title = titles.split(",");
    for (i = 0; i < arr_title.length; i++) {
        if (arr_title[i] != "") {
            $('#AccMenu').accordion("remove", arr_title[i]);
        }
    }
    //重新添加
    $.post("/System/GetTree_SysModule", {}, function (data) {
        var fname = "";
        var ficon = "";
        var buttons = "";
        $.each(data.parent, function (i, e) {
            fname = e.Name;
            ficon = e.Icon;
            buttons = "";
            $.each(data.child, function (j, ec) {
                if (ec._parentId == e.Id) {
                    buttons += '<a class="easyui-linkbutton xj-menu" plain="true" iconCls="' + ec.Icon + '" Url="' + ec.Url + '">' + ec.Name + '</a>';
                }
            });

            $('#AccMenu').accordion('add', {
                title: fname,
                content: buttons,
                selected: false,
                iconCls: ficon
            });
        });
        $.parser.parse();

    }, 'json');
};

function addTab(subtitle, url, icon) {
    if (!$("#mainTab").tabs('exists', subtitle)) {
        $("#mainTab").tabs('add', {
            title: subtitle,
            content: createFrame(url),
            closable: true,
            icon: icon
        });
    } else {
        $("#mainTab").tabs('select', subtitle);
        $("#tab_menu-tabrefresh").trigger("click");
    }
    //自动折叠
    //$(".layout-button-left").trigger("click");
}

function createFrame(url) {
    var s = '<iframe name="Listframe" frameborder="0" src="' + url + '" scrolling="auto" style="width:100%; height:99%; overflow-x:hidden;"></iframe>';
    return s;
}