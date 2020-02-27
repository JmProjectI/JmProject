function today() {
    var today = new Date();
    var h = today.getFullYear();
    var m = today.getMonth() + 1;
    var d = today.getDate();
    m = m < 10 ? "0" + m : m;   //  这里判断月份是否<10,如果是在月份前面加'0'
    d = d < 10 ? "0" + d : d;        //  这里判断日期是否<10,如果是在日期前面加'0'
    return h + "-" + m + "-" + d;
}

$(function () {
    bindGroup();
    reload();

    Event()
});

function Event() {    
    //查看报告信息
    $("#btnNksc").click(function () {
        ChaJcXx(); //查看手册
    });
    //查看报告历史工作
    $("#btnGzls").click(function () {
        ChaHistory();
    });    
    //手册状态
    $("#btnFlag1").click(function () {
        qishen(); //弃审
    });
    $("#btnFlagA").click(function () {
        chushen(); //A初审
    });
    $("#btnFlagB").click(function () {
        paifa(); //B派工
    });
    $("#btnFlagC").click(function () {
        tijiao(); //C提交
    });
    $("#btnFlagD").click(function () {
        yijiao(); //D移交客户
    });
    $("#btnFlagE").click(function () {
        fasong(); //E发送报告
    });
    $("#btnFlagF").click(function () {
        lingqu(); //F领取
    });
    $("#btnFlagJ").click(function () {
        daiding(); //J待定
    });
    //其他
    $("#btnDesc").click(function () {
        UpTsyq(); //特殊要求描述
    });
}

function bindGroup() {
    $("#DiQuS").combobox({
        url: '/Basic/Get_CombCity?pid=',
        multiple: true,
        panelHeight: 500,
        valueField: 'ID',
        textField: 'Name',
        groupField: 'CityPLName'
    }).combobox('clear');
    
    //业务员
    $('#userS').combobox({
        url: '/System/GetComb_Users?All=true',
        valueField: 'Id',
        textField: 'ZsName',
        onLoadSuccess: function (node, data) {
        }
    });

    //派发人
    $('#pfr').combobox({
        url: '/System/GetComb_Users?All=false',
        valueField: 'Id',
        textField: 'ZsName',
        onLoadSuccess: function (node, data) {
        }
    });

    //移交人
    $('#yjr').combobox({
        url: '/System/GetComb_Users?All=false',
        valueField: 'Id',
        textField: 'ZsName',
        onLoadSuccess: function (node, data) {
        }
    });
}

var cmenu;
function createColumnMenu() {
    cmenu = $('<div/>').appendTo('body');
    cmenu.menu({
        onClick: function (item) {
            if (item.iconCls == 'icon-ok') {
                //隐藏列
                $.cookie('NkReport_' + item.name, 'true');
                $('#grid').datagrid('hideColumn', item.name);
                cmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-empty'
                });
            } else {
                //显示列
                $.cookie('NkReport_' + item.name, null);
                $('#grid').datagrid('showColumn', item.name);
                cmenu.menu('setIcon', {
                    target: item.target,
                    iconCls: 'icon-ok'
                });
            }
        }
    });

    var fields = $('#grid').datagrid('getColumnFields');
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        if (field == "ck") {
            continue;
        }
        var col = $('#grid').datagrid('getColumnOption', field);
        var isHide = $.cookie('NkReport_' + field);
        if (isHide == "true") {
            cmenu.menu('appendItem', {
                text: col.title,
                name: field,
                iconCls: 'icon-empty'
            });
        }
        else {
            cmenu.menu('appendItem', {
                text: col.title,
                name: field,
                iconCls: 'icon-ok'
            });
        }
    }
}

function initCloumn() {
    var fields = $('#grid').datagrid('getColumnFields');
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        if (field == "ck") {
            continue;
        }
        var isHide = $.cookie('NkReport_' + field);
        if (isHide == "true") {
            $('#grid').datagrid('hideColumn', field);
        }
    }
}

/*刷新内控报告列表*/
function reload() {
    var queryData = {
        type: 'select',
        DiQuS: $("#DiQuS").combobox("getValues") + "",
        NameS: $("#NameS").val(),
        flag: $("#ddlflag").combobox("getValue"),
        NkscSBDateS: $("#txtNkscSBDateS").val(),
        NkscSBDateE: $("#txtNkscSBDateE").val(),
        Uname: $("#userS").combobox("getValue")
    };
    InitGrid(queryData);
    $('#grid').datagrid('uncheckAll');
}

/*加载内控报告列表*/
function InitGrid(queryData) {
    $('#grid').datagrid({   //定位到Table标签，Table标签的ID是grid
        url: '/NkReport/Report_Data',   //指向后台的Action来获取当前菜单的信息的Json格式的数据
        iconCls: 'icon-view',
        width: $(window).width() - 10,
        height: $(window).height() - 70,
        nowrap: true,
        autoRowHeight: false,
        striped: true,
        collapsible: true,
        pagination: true,
        pageSize: 20,
        pageList: [10, 20, 25, 30, 50],
        rownumbers: true,
        sortName: 'OrderId',    //根据某个字段给easyUI排序
        sortOrder: 'desc',
        remoteSort: true,
        idField: 'id',
        singleSelect: true,
        queryParams: queryData,  //异步查询的参数
        columns: [[
                    { field: 'ck', checkbox: true}   //选择
                    , { field: 'Years', title: '报告年度', sortable: false, width: 60 }
                    , { field: 'Id', title: '报告编号', sortable: false, width: 250,hidden:true }
                    , { field: 'OrderId', title: '订单编号', sortable: false, width: 100 }
                    , { field: 'Invoice', title: '客户名称', sortable: false, width: 250 }
                    , { field: 'Flag', title: '报告状态', sortable: false, width: 95,
                        formatter: function (value, row, index) {
                            if (value == '1') {
                                return "未提交";
                            }
                            else if (value == '2') {
                                return "已提交";
                            }
                            else if (value == '3') {
                                return "资料不全";
                            }
                            else if (value == '4') {
                                return "已审核";
                            }
                            else if (value == '5') {
                                return "制作中";
                            }
                            else if (value == '6') {
                                return "二次修改";
                            }
                            else if (value == '7') {
                                return "已完成"
                            }
                            else if (value == "8") {
                                return "移交客户";
                            }
                            else if (value == "9") {
                                return "已发报告";
                            }
                            else if (value == "10") {
                                return "待定";
                            }
                            else {
                                return "";
                            }
                        }
                    }
                    , { field: 'Tjrq', title: '提交日期', sortable: false, width: 100 }
                    , { field: 'Tsyqtext', title: '特殊描述', sortable: false, width: 100 }
                    , { field: 'Shrq', title: '审核日期', sortable: false, width: 100 }
                    , { field: 'ShrName', title: '审核人', sortable: false, width: 100 }
                    , { field: 'Zzrq', title: '制作日期', sortable: false, width: 100 }
                    , { field: 'ZzrName', title: '制作人', sortable: false, width: 100 }
                    , { field: 'Yjrq', title: '移交日期', sortable: false, width: 100 }
                    , { field: 'YjrName', title: '移交人', sortable: false, width: 100 }
                    , { field: 'Fsrq', title: '发送日期', sortable: false, width: 100 }
                    , { field: 'FsrName', title: '发送人', sortable: false, width: 100 }
                    , { field: 'Wcrq', title: '完成日期', sortable: false, width: 100 }
                    , { field: 'bz', title: '描述', sortable: false, width: 100 }
                    , { field: 'Lsr', title: '历史制作人', sortable: false, width: 100 }
                ]],
        onDblClickRow: function (rowIndex, rowData) {
            $('#grid').datagrid('uncheckAll');
            $('#grid').datagrid('checkRow', rowIndex);
            ShowEditOrViewDialog();
        },
        onHeaderContextMenu: function (e, field) {
            e.preventDefault();
            if (!cmenu) {
                createColumnMenu();
            }
            cmenu.menu('show', {
                left: e.pageX,
                top: e.pageY
            });
        },
        onLoadSuccess: function (data) {
            initCloumn();
        }
    })
};

/*刷新内控历史工作列表*/
function reloadReport(Rid) {
    var queryData = {
        Id: Rid
    };
    InitGridReport(queryData);
    $('#gridReport').datagrid('clearSelections');
};

/*加载内控历史工作列表*/
function InitGridReport(queryData) {
    $('#gridReport').datagrid({
        url: '/NkReport/SysReport_Data',
        width: 480,
        methord: 'post',
        height: 555,
        fitColumns: false,
        idField: 'Id',
        sortName: 'date',
        sortOrder: 'desc',
        pagination: true,
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,  //显示合计行
        queryParams: queryData,
        columns: [[
                { field: 'date', title: '操作日期', width: 150, halign: 'center' },
                { field: 'FlagName', title: '操作状态', width: 120, halign: 'center' },
                { field: 'CzrName', title: '操作人', width: 150, halign: 'center' }
         ]]
    });
}

function UpdateFlag(flag, postdata) {
    //当不等于弃审时候
    if (postdata.flag != "1") {
        //状态装订  且  要修改的状态不为 装订修改、发送PDF 、领取手册
        if (flag == "7" && postdata.flag != "8" && postdata.flag != "9") {
            $.messager.alert('系统提示', '报告已完成，不能操作！', 'warning');
            return false;
        }
    }

    $.post('/NkReport/SysNkReportFlag', postdata, function (result) {
        if (result.type == "1") {
            $.messager.show({ title: '系统提示', msg: result.message });
            reload();
            if (postdata.flag == "5") {
                $("#dlg").dialog("close");
            }
            else if (postdata.flag == "7") {
                $("#dlgydg").dialog("close");
            }
            else if (postdata.flag == "8") {
                $("#dlgyzd").dialog("close");
            }
        } else {
            $.messager.alert('系统提示', result.message, 'warning');
        }
    }, 'json');
};

/*查看报告基础信息*/
function ChaJcXx() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        window.parent.addTab(row.Name, "/NkReport/Index?Id=" + row.Id, "pic_1")
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*查看报告历史工作*/
function ChaHistory() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#dlgReport").dialog("open").dialog('setTitle', row.Name + ' 报告历史工作');
        reloadReport(row.Id);
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*弃审*/
function qishen() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [弃审] 内控报告吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.Id
                    , flag: '1'
                };
                UpdateFlag(row.Flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*A初审*/
function chushen() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [初审] 内控报告吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.Id
                    , flag: '4'
                };
                UpdateFlag(row.Flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*B派工*/
function paifa() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#dlg").dialog("open").dialog('setTitle', '派发用户');
        $("#pfr").combobox('setValue', row.Zzr);
        //$("input[name='pfr']").val(row.Zzr);
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*B派工保存*/
function save() {
    var validate = $("#fm").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.Id
        , flag: '5'
        , pfName: $("input[name='pfr']").val()
    };
    UpdateFlag(row.Flag, postdata);
}

/*C提交*/
function tijiao() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [提交] 内控报告吗?', function (yes) {
            if (yes) {
                $("#fmydg").form("clear");
                $("#dlgydg").dialog("open").dialog('setTitle', '提交');
                $("#fmydg").form("load", row);
                $("#bzid").val(row.bz);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*C提交保存*/
function savetj() {
    var validate = $("#fmydg").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.Id
        , flag: '7'
        //定稿描述
        , txtbz: $("#bzid").val()
    };
    UpdateFlag(row.Flag, postdata);
}

/*D移交客户*/
function yijiao() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#dlg").dialog("open").dialog('setTitle', '移交客户');
        $("#yjr").combobox('setValue', row.Yjr);
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*D移交客户保存*/
function saveyj() {
    var validate = $("#fmyzd").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.Id
        , flag: '8'
        , pfName: $("input[name='yjr']").val()
    };
    UpdateFlag(row.Flag, postdata);
}

/*E发送报告*/
function fasong() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [发送] 内控报告吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.Id
                    , flag: '9'
                };
                UpdateFlag(row.Flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*F领取*/
function lingqu() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [领取] 内控报告吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.Id
                    , flag: 'F'
                };
                $.post('/NkReport/SysNkReportFlag', postdata, function (result) {
                    if (result.type == "1") {
                        $.messager.show({ title: '系统提示', msg: result.message });
                        reload();
                    } else {
                        $.messager.alert('系统提示', result.message, 'warning');
                    }
                }, 'json');
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*特殊要求描述*/
function UpTsyq() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#dlgtsyq").dialog("open").dialog('setTitle', '特殊要求描述');
        $("#tsyqid").val(row.tsyqtext)
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}
/*特殊要求描述 保存*/
function savetsyq() {
    var validate = $("#fmtsyq").form('validate');
    if (validate == false) {
        return false;
    };

    var postData = {
        id: $("#grid").datagrid("getSelected").Id
        , TsyqName: $("#tsyqid").val()
    };

    //异步实现添加信息
    $.post("/NkReport/UpdateTsyq", postData, function (result) {
        if (result.type == "1") {
            $.messager.show({ title: '系统提示', msg: result.message });
            $("#dlgtsyq").dialog("close");
            reload();
        } else {
            $.messager.alert('系统提示', result.message, 'warning');
        }
    });
}

/*J待定*/
function daiding() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [待定] 内控报告吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.Id
                    , flag: '10'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};