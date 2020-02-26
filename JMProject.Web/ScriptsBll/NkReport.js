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
        //wancheng(); //C编制完成
    });
    $("#btnFlagD").click(function () {
        //fenfa(); //D用户核对中
    });
    $("#btnFlagE").click(function () {
        //dinggao(); //E定稿确认
    });
    $("#btnFlagF").click(function () {
        //ZhuangDing(); //F装订完成
    });
    $("#btnFlagG").click(function () {
        //ZDUpdate(); //G装订修改
    });
    $("#btnFlagI").click(function () {
        //ZTUpdate('11'); //I手册已领取
    });
    $("#btnFlagJ").click(function () {
        //daiding(); //J待定
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
        if (flag == "7" && postdata.flag != "8") {
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
            else if (postdata.flag == "6") {
                $("#dlgydg").dialog("close");
            }
            else if (postdata.flag == "8") {
                $("#dlgyzd").dialog("close");
            }
            else if (postdata.flag == "10" || postdata.flag == "11") {
                $("#dlgfslq").dialog("close");
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
                UpdateFlag(row.flag, postdata);
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
                UpdateFlag(row.flag, postdata);
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
        $("#Saler").combobox('setValue', '@Model.Saler');
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
    UpdateFlag(row.flag, postdata);
}

/*生成手册*/
function add(stype, B5, x4) {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要生成内控手册吗?', function (yes) {
            if (yes) {
                $.ajax({
                    type: "post",
                    //url: "/Nksc/BuildWord",
                    url: "/Word/BuildWord",
                    data: { id: row.id, stype: stype, B5: B5, x4: x4 },
                    beforeSend: function () {
                        $.messager.progress({ title: '系统提示', msg: '正在生成手册，请稍候...<br/>(请勿执行其他操作)' });
                    },
                    success: function (result) {
                        $.messager.progress('close');
                        if (result.type == "1") {
                            $.messager.show({ title: '系统提示', msg: result.message });
                            reload();
                        } else {
                            $.messager.alert('系统提示', result.message, 'warning');
                        }
                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        $.messager.progress('close');
                        $.messager.alert('系统提示', '生成失败', 'warning');
                        // 状态码
                        console.log(XMLHttpRequest.status);
                        // 状态
                        console.log(XMLHttpRequest.readyState);
                        // 错误信息   
                        console.log(textStatus);
                    }
                });
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*生成会议纪要*/
function create_hyjy() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要生成会议纪要吗?', function (yes) {
            if (yes) {
                window.location.href = "/Word/BuildWord_Hyjy?id=" + row.id;
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*下载附件*/
function DownFJ() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.post('/Nksc/DownFJ', { cid: row.CustomerID }, function (result) {
            if (result.type == "1") {
                window.location = "../Upload/" + result.message;
            }
            else {
                $.messager.alert('系统提示', result.message, 'warning');
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*C编制完成*/
function wancheng() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [编制完成] 内控手册吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.id
                    , flag: '5'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*D发送客户*/
function fenfa() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [分发] 内控手册吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.id
                    , flag: '4'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*E定稿确认*/
function dinggao() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#fmydg").form("clear");
        $("#dlgydg").dialog("open").dialog('setTitle', '定稿');
        $("#fmydg").form("load", row);
        $("#bzid").val(row.bz);
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}
/*E定稿保存*/
function saveydg() {
    var validate = $("#fmydg").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.id
        , flag: '6'
        //协议装订数量
        , zdsum: $("#txtxyzdsum").numberbox("getValue")
        //定稿描述
        , txtbz: $("#bzid").val()
    };
    UpdateFlag(row.flag, postdata);
}

/*F已装订*/
function ZhuangDing() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#fmyzd").form("clear");
        $("#dlgyzd").dialog("open").dialog('setTitle', '装订');
        $("#fmyzd").form("load", row);
        var sy = parseInt(row.xyzdsum) - parseInt(row.bczdsum);
        $("#txtbzid").val(row.bz);
        $("#txtzddate").val(today());
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*F已装订保存*/
function saveyzd() {
    var validate = $("#fmyzd").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.id
        , flag: '8'
        , zddate: $("#txtzddate").val()
        , bcsum: $("#txtbczdsum").numberbox("getValue")
        , txtbz: $("#txtbzid").val()
    };
    UpdateFlag(row.flag, postdata);
}

/*G装订后有修改*/
function ZDUpdate() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [装订修改] 内控手册吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.id
                    , flag: '9'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/* H已发送PDF I手册已领取 */
function ZTUpdate(ztflag) {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        var title = '';
        if (ztflag == "10") {
            title = '已发送PDF';
        }
        else if (ztflag == "11") {
            title = '手册已领取';
        }
        $("#fmfslq").form("clear");
        $("#dlgfslq").dialog("open").dialog('setTitle', title);
        $("#fmfslq").form("load", row);
        $("#Hidden_fslqflag").val(ztflag);
        $("#txtfslqdate").val(today());
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}
/* H已发送PDF I手册已领取 保存 */
function savefslq() {
    var validate = $("#fmfslq").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postdata = {
        id: row.id
        , flag: $("#Hidden_fslqflag").val()
        , pfName: $("#txtfslqPeo").val()
        , zddate: $("#txtfslqdate").val()
    };
    UpdateFlag(row.flag, postdata);
}

/*J待定*/
function daiding() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [待定] 内控手册吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.id
                    , flag: '13'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
};

/*已生成PDF*/
function ZDUpdatePDF() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $.messager.confirm('系统提示', '确认要 [已生成PDF] 内控手册吗?', function (yes) {
            if (yes) {
                var postdata = {
                    id: row.id
                    , flag: '12'
                };
                UpdateFlag(row.flag, postdata);
            }
        });
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/*弃审问题反馈0  已处理问题反馈2*/
function NkReport_wtfkFlag(flag) {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        text = '';
        if (flag == "0") {
            if (row.wtfkFlag == "0") {
                $.messager.alert('系统提示', '还未提交问题反馈，无须弃审!', 'warning');
                return;
            }
            text = '确认要 [弃审问题反馈] 吗?如果之前反馈问题已修改，清先单击 [已处理问题反馈] 后再 [弃审问题反馈]';
        }
        else if (flag == "2") {
            if (row.wtfkFlag != "1") {
                $.messager.alert('系统提示', '只能处理已反馈数据!', 'warning');
                return;
            }
            text = '确认要 [已处理问题反馈] 吗?';
        }
        $.messager.confirm('系统提示', text, function (yes) {
            if (yes) {
                $.post('/Nksc/NkReport_Wtfk_Flag', { id: row.id, flag: flag }, function (result) {
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

/*获取反馈文档*/
function getWtfk(id, UserName) {
    $("#ulfile").empty();
    $("#dlgFkwd").dialog("open").dialog('setTitle', '反馈文档列表');
    $.post('/Nksc/NkReport_Wtfk', { id: id }, function (result) {
        if (result.length > 0) {
            $.each(result, function (index, item) {
                var text = '';
                var html = '';
                if (item.flags == "0") {
                    text = '未提交';
                    html = '<li>(' + text + ') ' + item.wtFile + '</li>';
                }
                else if (item.flags == "1") {
                    text = '已提交';
                    html = '<li>(' + text + ') <a href="../DownWTFK/' + UserName + '/' + item.wtFile + '">' + item.wtFile + '</a></li>';
                }
                else if (item.flags == "2") {
                    text = '已处理';
                    html = '<li>(' + text + ') ' + item.wtFile + '</li>';
                }

                $("#ulfile").append(html);
            });
        } else {
            //$.messager.alert('系统提示', '获取失败', 'warning');
        }
    }, 'json');
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
        id: $("#grid").datagrid("getSelected").id
        , TsyqName: $("#tsyqid").val()
    };

    //异步实现添加信息
    $.post("/Nksc/UpdateTsyq", postData, function (result) {
        if (result.type == "1") {
            $.messager.show({ title: '系统提示', msg: result.message });
            $("#dlgtsyq").dialog("close");
            reload();
        } else {
            $.messager.alert('系统提示', result.message, 'warning');
        }
    });
}

/* 导出 */
function DaoC() {
    window.location.href = "/Nksc/DaoCExcel?DiQuS=" + $("#DiQuS").combobox("getValues") + "" + "&NameS=" + $("#NameS").val()
                    + "&flag=" + $("#ddlflag").combobox("getValue") + "&fkflag=" + $("#fkflagid").combobox("getValue")
                    + "&NkscDateS=" + $("#txtNkscDateS").val() + "&NkscDateE=" + $("#txtNkscDateE").val()
                    + "&NkscSBDateS=" + $("#txtNkscSBDateS").val() + "&NkscSBDateE=" + $("#txtNkscSBDateE").val()
                    + "&fkpdfflag=" + $("#dllfkpdfflag").combobox("getValue")
                    + "&IsUpdate=" + $("#dllisupdate").combobox("getValue")
                    + "&riqiS=" + $("#txtStart").val() + "&riqiE=" + $("#txtEnd").val();
}

/* 加印手册 */
function jiayin() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#modalwindow").html("<iframe width='100%' height='98%' scrolling='no' frameborder='0' src='/Sale/Create_SaleOrder?AddType=True&CId=" + row.CustomerID + "&nktype=2'></iframe>");
        $("#modalwindow").window({ title: '创建合同-加印手册', width: 1150, height: 510, iconCls: 'icon-add' }).window('open');
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/* 更新手册 */
function gengxin() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        $("#modalwindow").html("<iframe width='100%' height='98%' scrolling='no' frameborder='0' src='/Sale/Create_SaleOrder?AddType=True&CId=" + row.CustomerID + "&nktype=3'></iframe>");
        $("#modalwindow").window({ title: '创建合同-更新手册', width: 1150, height: 510, iconCls: 'icon-add' }).window('open');
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }
}

/* 检查单位 */
function jianchadw() {
    var row = $("#grid").datagrid("getSelected");
    if (row) {
        var title = '检查单位';
        $("#fmjcdw").form("clear");
        $("#dlgjcdw").dialog("open").dialog('setTitle', title);
        $("#fmjcdw").form("load", row);
    }
    else {
        $.messager.alert('系统提示', '请勾选要操作的行!', 'warning');
    }

}

/* 检查单位保存 */
function savejcdw() {
    var validate = $("#fmjcdw").form('validate');
    if (validate == false) {
        return false;
    };
    var row = $("#grid").datagrid("getSelected");
    var postData = {
        id: row.id
        , jcnf: $("#txtjcnf").val()
        , CustomerID: row.CustomerID
    };
    //异步实现添加信息
    $.post("/Nksc/InsertJcdw", postData, function (result) {
        if (result.type == "1") {
            $.messager.show({ title: '系统提示', msg: result.message });
            $("#dlgjcdw").dialog("close");
            reload();
        } else {
            $.messager.alert('系统提示', result.message, 'warning');
        }
    });
}

function ShowSale(Cid, Cname) {
    $("#dlgOrder").dialog("open").dialog('setTitle', Cname + ' 合同明细');
    reloadOrder(Cid);
}

function reloadOrder(Cid) {
    var queryData = {
        SaleCustomId: Cid
    };
    InitGridOrder(queryData);
    $('#gridOrder').datagrid('clearSelections');
};

function InitGridOrder(queryData) {
    $('#gridOrder').datagrid({
        url: '/Sale/GetData_SaleOrder',
        width: 1080,
        methord: 'post',
        height: 455,
        fitColumns: false,
        idField: 'Id',
        sortName: 'OrderDate',
        sortOrder: 'desc',
        pagination: true,
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,  //显示合计行
        queryParams: queryData,
        frozenColumns: [[
        //                { field: 'Id', title: '编号', width: 100, halign: 'center' },
                {field: 'OrderDate', title: '日期', width: 100, halign: 'center' },
        //                { field: 'CityName', title: '地区', width: 80, halign: 'center' },
        //        {field: 'Name', title: '客户/发票抬头', width: 170, halign: 'center' },
                {field: 'ItemNames', title: '合同明细', width: 150, halign: 'center' },
        ]],
        columns: [[
                { field: 'InvoiceFlagName', title: '发票状态', width: 70, halign: 'center' },
                { field: 'PaymentFlagName', title: '回款状态', width: 70, halign: 'center' },
                { field: 'OutStockFlagName', title: '出库状态', width: 70, halign: 'center' },
                { field: 'Finshed', title: '已完成', width: 60, halign: 'center'
                    , formatter: function (value, row, index) {
                        if (row.Name == "合计") {
                            return "";
                        }
                        else if (value) {
                            return "已完成";
                        }
                        else {
                            return "未完成";
                        }
                    }
                },
                { field: 'ItemMoney', title: '合计金额', width: 100, align: 'right', halign: 'center'
                    , formatter: function (value, row, index) {
                        if (value != null) {
                            return parseFloat(value).toFixed(2);
                        }
                    }
                },
                { field: 'Invoicemoney', title: '开票金额', width: 80, align: 'right', halign: 'center'
                    , formatter: function (value, row, index) {
                        if (value != null) {
                            return parseFloat(value).toFixed(2);
                        }
                    }
                },
                { field: 'Receivablemoney', title: '应收金额', width: 80, align: 'right', halign: 'center'
                    , formatter: function (value, row, index) {
                        if (value != null) {
                            return parseFloat(value).toFixed(2);
                        }
                    }
                },
                { field: 'Paymentmoney', title: '回款金额', width: 80, align: 'right', halign: 'center'
                    , formatter: function (value, row, index) {
                        if (value != null) {
                            return parseFloat(value).toFixed(2);
                        }
                    }
                },
                { field: 'ItemCount', title: '合计数量', width: 70, align: 'right', halign: 'center' },
                { field: 'OSCount', title: '出库数量', width: 70, align: 'right', halign: 'center' },
                { field: 'SalerName', title: '业务员', width: 70, halign: 'center' }
         ]]
    });
}

function ShowUpdate(Cid) {
    $("#dlgUpdate").dialog("open").dialog('setTitle', '更新记录');
    $('#gridUpdate').datagrid({
        url: '/Nksc/GetData_NkscUpdate',
        width: 480,
        methord: 'post',
        height: 255,
        fitColumns: false,
        idField: 'Id',
        sortName: 'NkscDate',
        sortOrder: 'desc',
        pagination: true,
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: false,  //显示合计行
        queryParams: { CustomerID: Cid },
        columns: [[
                { field: 'opt', title: '操作', width: 100, align: 'center', halign: 'center',
                    formatter: function (value, row, index) {
                        if (row.UpdateFlag == "0") {
                            var d = '<a href="#" mce_href="#" onclick="delUpdate(\'' + row.id + '\',\'' + Cid + '\')">取消更新</a> ';
                            return d;
                        }
                    }
                },
                { field: 'NkscDate', title: '更新日期', width: 80, halign: 'center' },
                { field: 'versionS', title: '更新前版本', width: 80, halign: 'center' },
                { field: 'versionE', title: '更新后版本', width: 80, halign: 'center' },
                { field: 'UpdateFlag', title: '已完成', width: 80, halign: 'center'
                    , formatter: function (value, row, index) {
                        if (value == "1") {
                            return "已完成";
                        }
                        else {
                            return "未完成";
                        }
                    }
                }
         ]]
    });
}

function delUpdate(id, Cid) {
    $.messager.confirm('系统提示', '确认要取消更新吗？', function (r) {
        if (r) {
            $.post("/Nksc/Delete_NkscUpdate?id=" + id, function (data) {
                if (data.type == 1) {
                    ShowUpdate(Cid);
                }
                else {
                    $.messageBox5s('系统提示', data.message);
                }
            }, "json");
        }
    });
};