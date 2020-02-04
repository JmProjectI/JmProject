
//回填选中项目（科室、职务）
function chooseKS() {
    if ($("#HiddenKS").val().length == 0) {
        //$.show_alert('系统提示', '请至少选择一项');
        $("#dlgKS").dialog("close");
        return;
    }
    inputKS.val($("#HiddenKS").val().substr(1));
    inputKS.textbox('setValue', $("#HiddenKS").val().substr(1));
    $("#dlgKS").dialog("close");
};

//关闭 弹出的选择框（科室、职务）
function closeKS() {
    $("#dlgKS").dialog("close");
    inputKS.focus();
};

//弹出 选科室对话框
function showKS(e) {
    inputKS = $(e.data.target);
    $("input:checkbox[name='cheKS']").removeAttr('checked');
    if (inputKS.val().length > 0) {
        var thisKS = inputKS.val().split("、");
        $.each(thisKS, function (index, value) {
            $("input:checkbox[name='cheKS'][value='" + value + "']").prop("checked", "checked");
        });
        $("#HiddenKS").val("、" + inputKS.val());
    }
    else {
        $("#HiddenKS").val("");
    }
    $("#ulKS").show();
    $("#ulZW").hide();
    $("#dlgKS").dialog("open").dialog('setTitle', '选择科室');
}

//弹出 选职务对话框
function showZW(e) {
    inputKS = $(e.data.target);
    $("input:checkbox[name='cheZW']").removeAttr('checked');
    if (inputKS.val().length > 0) {
        var thisZW = inputKS.val().split("、");
        $.each(thisZW, function (index, value) {
            $("input:checkbox[name='cheZW'][value='" + value + "']").prop("checked", "checked");
        });
        $("#HiddenKS").val("、" + inputKS.val());
    }
    else {
        $("#HiddenKS").val("");
    }
    $("#ulKS").hide();
    $("#ulZW").show();
    $("#dlgKS").dialog("open").dialog('setTitle', '选择职务');
}

function setHeight() {
    $(".stepContainer").height($(window).height() - 68);
    $("div.content").height($(window).height() - 80);
    //$("stepContainer").width($(window).width() - 600);
    $("div.content").width($(window).width() - 200);
    $("div.actionBar").width($(window).width() - 20);

    $("#zzdiv").height($(window).height() - 68);
    $("#zzdiv").width($(window).width() - 200);
};

$(function () {

    $("input[jm='com_zw']").textbox({
        iconWidth: 22,
        icons: [{
            iconCls: 'icon-add1',
            handler: showZW
        }]
    });

    $("input[jm='com']").textbox({
        iconWidth: 22,
        icons: [{
            iconCls: 'icon-add1',
            handler: showKS
        }]
    });

    setEvent();

    $('#wizard').smartWizard({
        transitionEffect: 'slide',
        labelNext: "下一步",
        labelPrevious: '上一步',
        labelFinish: '提交',
        onShowStep: showStepCallback,
        onLeaveStep: nextStepCallback,
        onFinish: FinishCallback
    });

    //激活 向导 所有页
    $('#wizard ul a').attr("isDone", 1).attr("class", "done");
    //默认选中第一页 向导
    $('#wizard [rel=1]').attr("isDone", 1).attr("class", "selected");


    if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1") {
        $('.actionBar a.buttonFinish').hide(); //提交按钮变灰
    }

    Init();


    $("input[type='text']").tooltip({
        content: '',
        onShow: function () {
            var msg = $(this).attr("placeholder");
            if (msg.length == 0) {
                msg = "无提示信息";
            }
            var t = $(this);
            t.tooltip('update', msg);
            t.tooltip('tip').unbind().bind('mouseenter', function () {
                t.tooltip('show');
            }).bind('mouseleave', function () {
                t.tooltip('hide');
            });
        }
    });
});

var inputKS; //弹出选择的文本框

function setEvent() {

    $(window).resize(function () {
        setHeight();
    });

    $(".easyui-textbox").each(function (i) {
        var span = $(this).siblings("span")[0];
        var targetInput = $(span).find("input:first");
        if (targetInput) {
            $(targetInput).attr("placeholder", $(this).attr("placeholder"));
        }
    });

    //    $("#txtscqtbm").on({
    //        "blur": function () {
    //            if ($("#txtnbkzgzxzzzqt01").val().length==0) {
    //                $("#txtnbkzgzxzzzqt01").val($(this).val());
    //            }            
    //        }
    //    });

    //    $("#txtscxzbm").on({
    //        "blur": function () {
    //            if ($("#txtnbkzgzxzzzcy01").val().length == 0) {
    //                $("#txtnbkzgzxzzzcy01").val($(this).val());
    //            }
    //        }
    //    });

    //科室对话框 选中checkbox时 组合内容
    $("#ulKS").on("click", "input:checkbox[name='cheKS']", function () {
        if ($(this).is(':checked')) {
            var ks = $("#HiddenKS").val() + "、" + $(this).val();
            $("#HiddenKS").val(ks);
        }
        else {
            var ks = $("#HiddenKS").val().replace("、" + $(this).val(), "");
            $("#HiddenKS").val(ks);
        }
    });

    //职务对话框 多选时组合内容
    $("#ulZW").on("click", "input:checkbox[name='cheZW']", function () {
        if ($(this).is(':checked')) {
            var ks = $("#HiddenKS").val() + "、" + $(this).val();
            $("#HiddenKS").val(ks);
        }
        else {
            var ks = $("#HiddenKS").val().replace("、" + $(this).val(), "");
            $("#HiddenKS").val(ks);
        }
    });

    ///////////////党组织机构///////////////////////
    $('input:radio[name="dzzjgmc"]').change(function () {
        var dzmc = $("input[name='dzzjgmc']:checked").val();
        if (dzmc != "无") {
            $("#spanzzzwDY").html('是否' + dzmc + '成员');
            $('#divzzzwDY').show();
            $("#spanfzzwDY").html('是否' + dzmc + '成员');
            $('#divfzzwDY').show();

            var rowindex = 2;
            $('#table_fz tr').each(function (i) {
                if (i % 2 == 1) {
                    $("#spanfzzwDY" + rowindex).html('是否' + dzmc + '成员');
                    $('#divfzzwDY' + rowindex).show();
                    rowindex += 1;
                }
            });
        }
        else {
            $("#spanzzzwDY").html('');
            $('#divzzzwDY').hide();
            $("#spanfzzwDY").html('');
            $('#divfzzwDY').hide();

            var rowindex = 2;
            $('#table_fz tr').each(function (i) {
                if (i % 2 == 1) {
                    $("#spanfzzwDY" + rowindex).html('');
                    $('#divfzzwDY' + rowindex).hide();
                    rowindex += 1;
                }
            });
        }
    });

    /////////不需要社会统一信用代码///////////////////
    $('#check_tyxy').change(function () {
        if ($(this).is(':checked')) {
            $("#TexttyxyID").attr("disabled", "disabled");
            $("#TexttyxyID").val('');
        }
        else {
            $("#TexttyxyID").removeAttr("disabled");
        }
    });

    ///////////////考勤时间///////////////////////
    $('#Checkbox_dxkq').change(function () {
        SetKqsj($(this).is(':checked'));
    });

    ///////////////业务内容///////////////////////
    $("input[name='checkboxYW']").on({
        "click": function () {
            if ($(this).attr("id") == "Checkboxh") {
                if ($(this).is(':checked')) {
                    $("#Radio_gwkzd1").prop('checked', true);
                    set_gwk(false);
                }
                else {
                    $("#Radio_gwkzd2").prop('checked', true);
                    set_gwk(true);
                }
            }
            var value = "梳理";
            $("input[name='checkboxYW']:checkbox:checked").each(function (i) {
                if ($(this).val() == 1) {
                    value += "预算编制与执行管理业务；";
                }
                else if ($(this).val() == 2) {
                    value += "收入业务；";
                }
                else if ($(this).val() == 3) {
                    value += "支出管理业务；";
                }
                else if ($(this).val() == 4) {
                    value += "财政票据管理业务；";
                }
                else if ($(this).val() == 5) {
                    value += "政府采购管理业务；";
                }
                else if ($(this).val() == 6) {
                    value += "资产管理业务；";
                }
                else if ($(this).val() == 7) {
                    value += "建设项目管理业务；";
                }
                else if ($(this).val() == 8) {
                    value += "合同管理业务；";
                }
                else if ($(this).val() == 9) {
                    value += "印章管理业务；";
                }
                else if ($(this).val() == "a") {
                    value += "非税收入业务；";
                }
                else if ($(this).val() == "b") {
                    value += "债务业务；";
                }
                else if ($(this).val() == "c") {
                    value += "对外投资业务；";
                }
            });
            if (value != "梳理") {
                value = value.substring(0, value.length - 1);
                $("#txtywslzdmc").textbox('setValue', value + "。");
            }
            else {
                $("#txtywslzdmc").textbox('setValue', '');
            }
        }
    });

    //本单位收入包括哪些内容（行政单位 、 事业单位）切换
    $('input:radio[name="radiodwxz"]').change(function () {
        if ($("input[name='radiodwxz']:checked").val() == "xzdw") {
            $("#txtbdwsrbk").textbox('setValue', '财政拨款收入、非税收入、其他收入');
        }
        else {
            $("#txtbdwsrbk").textbox('setValue', '财政补助收入、上级补助收入、事业收入、其他收入');
        }
    });

    ///////////////专项资金管理办法 ///////////////////////
    $('input:radio[name="zxzjgl"]').change(function () {
        if ($("input[name='zxzjgl']:checked").val() == "1") {

            $("#txtczzxzjgkks").textbox({ disabled: false })
            var span = $("#txtczzxzjgkks").siblings("span")[0];
            var targetInput = $(span).find("input:first");
            if (targetInput) {
                $(targetInput).attr("placeholder", $("#txtczzxzjgkks").attr("placeholder"));
            }

        }
        else {

            $("#txtczzxzjgkks").textbox('setValue', '');
            $("#txtczzxzjgkks").textbox({ disabled: true })
            var span = $("#txtczzxzjgkks").siblings("span")[0];
            var targetInput = $(span).find("input:first");
            if (targetInput) {
                $(targetInput).attr("placeholder", $("#txtczzxzjgkks").attr("placeholder"));
            }

        }
    });
    //////////////////////////////////////////////////

};

///////////////公务卡制度///////////////////////
function set_gwk(isgwk) {
    $("#txtgwkglks").textbox({ disabled: isgwk })
    var span = $("#txtgwkglks").siblings("span")[0];
    var targetInput = $(span).find("input:first");
    if (targetInput) {
        $(targetInput).attr("placeholder", $("#txtgwkglks").attr("placeholder"));
    }

    $("#txtgwkjdks").textbox({ disabled: isgwk })
    span = $("#txtgwkjdks").siblings("span")[0];
    targetInput = $(span).find("input:first");
    if (targetInput) {
        $(targetInput).attr("placeholder", $("#txtgwkjdks").attr("placeholder"));
    }
}

//绑定所有科室
function Bind_KS() {
    $("#ulKS").empty();
    if ($("#txtbhks").val().length > 0) {
        var allKS = $("#txtbhks").val().split("、");
        $.each(allKS, function (index, value) {
            var html = '<li style="display:inline-block;line-height:20px;width:28%;"><input id="CheckboxKS' + index + '" name="cheKS" type="checkbox" value="' + value + '" /><label for="CheckboxKS' + index + '">' + value + '</label></li>';
            $("#ulKS").append(html);
        });
    }
};

//绑定所有副职
function Bind_FZ() {
    $("#ulZW").empty();
    if ($("#txtzzzwmc").val().length > 0) {
        var html = '<li style="display:inline-block;line-height:20px;width:28%;"><input id="CheckboxZW_zz" name="cheZW" type="checkbox" value="' + $("#txtzzzwmc").val() + $("#txtldzzmc").val() + '" /><label for="CheckboxZW_zz">' + $("#txtzzzwmc").val() + $("#txtldzzmc").val() + '</label></li>';
        $("#ulZW").append(html);
    }
    if ($("#txtfzzwmc1").val().length > 0) {
        var html = '<li style="display:inline-block;line-height:20px;width:28%;"><input id="CheckboxZW_fz" name="cheZW" type="checkbox" value="' + $("#txtfzzwmc1").val() + $("#txtldfzmc1").val() + '" /><label for="CheckboxZW_fz">' + $("#txtfzzwmc1").val() + $("#txtldfzmc1").val() + '</label></li>';
        $("#ulZW").append(html);
    }
    var rowNum = 2;
    $('#table_fz tr').each(function (i) {
        if (i % 2 == 1) {
            if ($("#txtfzzwmc" + rowNum).val().length > 0) {
                var html = '<li style="display:inline-block;line-height:20px;width:28%;"><input id="CheckboxZW' + rowNum + '" name="cheZW" type="checkbox" value="' + $("#txtfzzwmc" + rowNum).val() + $("#txtldfzmc" + rowNum).val() + '" /><label for="CheckboxZW' + rowNum + '">' + $("#txtfzzwmc" + rowNum).val() + $("#txtldfzmc" + rowNum).val() + '</label></li>';
                $("#ulZW").append(html);
            }
            rowNum += 1;
        }
    });
}

var step_num_now = "1";

//当现实该步骤时回调该函数，一般用于当前步骤的初始化
function showStepCallback(stepObj) {
    step_num_now = stepObj.attr('rel');
    if (step_num_now == "9") {
        $("div.actionBar").find("a.buttonsave").remove();
        $("#zzdiv").css('display', 'none'); 
    }
    else {
        if ($("#HiddenFlag").val() == "" || $("#HiddenFlag").val() == "1" || $("#HiddenManager").val() == "1") {
            if ($("div.actionBar").find("a.buttonsave").length == 0) {
                $("div.actionBar").append('<a id="SBC" href="#" onclick="SaveStep();" class="buttonsave">保存</a>');
            }
            $("#zzdiv").css('display', 'none');
        }
        else {
            $("#zzdiv").css('display', 'block'); 
        }

        if (step_num_now == "1") {
            setHeight();
        }
        else if (step_num_now == "7") {
            uploader.refresh(); //刷新上传控件
        }
        else if (step_num_now=="8") {
            $('textarea[autoHeight]').autoHeight();
        }

    }
};

function nextStepCallback(stepObj, curstep) {
    if (curstep.toStep < curstep.fromStep) {
        return true;
    }
    var step_num = stepObj.attr('rel');
    switch (step_num) {
        case '1':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                Bind_KS();
                return true;
            }
            break;
        case '2':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                Bind_FZ();
                return true;
            }
            break;
        case '3':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        case '4':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        case '5':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        case '6':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        case '7':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        case '8':
            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
                return true;
            }
            else {
                return true;
            }
            break;
        default:
            break;
    }
};

function FinishCallback(stepObj) {
    $("#zzdiv").show();
    SaveStepEnd();
};

function Init() {

    //加载考勤时间
    SetKqsj($('#Checkbox_dxkq').is(':checked'));
    //自定义副职数量
    fzcount = $("#table_fz tbody").children("tr").length / 2 + 1;
    //绑定科室
    Bind_KS();
    //绑定赋值
    Bind_FZ();
};

//考勤时间
function SetKqsj(dx) {
    if (dx) {
        $('#dxkqsj').show();
        $('#spankqsjsw').html("夏令时：上午");
        $('#spankqsjxw').html("夏令时：下午");
    }
    else {
        $('#dxkqsj').hide();
        $('#spankqsjsw').html("上午");
        $('#spankqsjxw').html("下午");
    }
};

//添加副职
var fzcount = 1; //副职数量
function addfz() {
    //副职职务名称1
    var txt_fzzwmc = $("#txtfzzwmc1").val();
    //副职领导姓名1
    var txt_ldfzmc = $("#txtfzzwmc1").val();
    //副职领导1分管科室
    var txt_ldfzfg = $("#txtfzzwmc1").val();

    if (txt_fzzwmc != "" && txt_fzzwmc != null && txt_fzzwmc != "undefined"
        && txt_ldfzmc != "" && txt_ldfzmc != null && txt_ldfzmc != "undefined"
        && txt_ldfzfg != "" && txt_ldfzfg != null && txt_ldfzfg != "undefined") {
        fzcount += 1;
        var trHTML = '<tr>' +
        '<td class="dec"><input id="check_fz' + fzcount + '" type="checkbox" name="check_fz" /><label for="check_fz' + fzcount + '">副职职务名称' + fzcount + '</label></td>' +
        '<td class="con"><input id="txtfzzwmc' + fzcount + '" name="fzzwmc' + fzcount + '" type="text" value="" class="easyui-validatebox" /></td>' +
        '<td class="dec">副职领导姓名' + fzcount + '</td>' +
        '<td class="con"><input id="txtldfzmc' + fzcount + '" name="ldfzmc' + fzcount + '" type="text" value="" class="easyui-validatebox" /></td>';
        var dzmc = $("input[name='dzzjgmc']:checked").val();
        if (dzmc != "无" && dzmc != "" && typeof (dzmc) != "undefined") {
            trHTML = trHTML + '<td class="dec"><span id="spanfzzwDY' + fzcount + '">是否' + dzmc + '成员</span></td>' +
            '<td class="con" style="min-width:206px;">' +
            '    <div id="divfzzwDY' + fzcount + '" style="">' +
            '        <input id="Radio_fzzwDY1' + fzcount + '" type="radio" name="fzzwDY' + fzcount + '" value="1" /><label for="Radio_fzzwDY1' + fzcount + '">是</label>' +
            '        <input id="Radio_fzzwDY2' + fzcount + '" type="radio" name="fzzwDY' + fzcount + '" value="0" checked="checked" /><label for="Radio_fzzwDY2' + fzcount + '">否</label>' +
            '    </div>' +
            '</td>';
        }
        else {
            trHTML = trHTML + '<td class="dec"><span id="spanfzzwDY' + fzcount + '"></span></td>' +
            '<td class="con" style="min-width:206px;">' +
            '    <div id="divfzzwDY' + fzcount + '" style="display:none;">' +
            '        <input id="Radio_fzzwDY1' + fzcount + '" type="radio" name="fzzwDY' + fzcount + '" value="1" /><label for="Radio_fzzwDY1' + fzcount + '">是</label>' +
            '        <input id="Radio_fzzwDY2' + fzcount + '" type="radio" name="fzzwDY' + fzcount + '" value="0" checked="checked" /><label for="Radio_fzzwDY2' + fzcount + '">否</label>' +
            '    </div>' +
            '</td>';
        }
        trHTML = trHTML + '</tr>' +
        '<tr>' +
        '<td class="dec">副职领导' + fzcount + '分管科室</td>' +
        '<td class="con" colspan="5"><input jm="com" id="txtldfzfg' + fzcount + '" name="ldfzfg' + fzcount + '" type="text" value="" class="easyui-validatebox" style="width:1050px" /></td>' +
        '</tr>';
        $("#table_fz").append(trHTML);
    }
    else {
        alert('请先填写副职职务名称1、副职领导姓名1、副职领导1分管科室');
    }
};

//删除副职
function removefz() {
    if (fzcount == 1) {
        alert('没有可删除的行');
        return;
    }
    //    $("input:checkbox[name='check_fz']:checked").each(function (i) {
    //    })
    $("#table_fz tr:last").remove();
    $("#table_fz tr:last").remove();
    fzcount -= 1;
};

function CheckStep1() {
    var bl_step1 = true;
    if ($("#txtdwjc").val() == "") {
        $.messager.alert("提示信息", "请输入 颁发日期");
        bl_step1 = false;
    }
    if ($("#txtdwqc").val() == "") {
        $.messager.alert("提示信息", "请输入 单位全称");
        bl_step1 = false;
    }
    if (!$("#check_tyxy").is(':checked')) {
        if ($("#TexttyxyID").val() == "") {
            $.messager.alert("提示信息", "请输入 社会统一信用代码");
            bl_step1 = false;
        }
        if ($("#TexttyxyID").val().length != 18) {
            $.messager.alert("提示信息", "社会统一信用代码必须为18位");
            bl_step1 = false;
        }
    }
    if ($("#TextName").val() == "") {
        $.messager.alert("提示信息", "请输入 发票收件人");
        bl_step1 = false;
    }
    if ($("#TextTel").val() == "") {
        $.messager.alert("提示信息", "请输入 手机号");
        bl_step1 = false;
    }
    if ($("#TextQQ").val() == "") {
        $.messager.alert("提示信息", "请输入 QQ号");
        bl_step1 = false;
    } 
    if ($("#TextAddress").val() == "") {
        $.messager.alert("提示信息", "请输入 邮寄地址");
        bl_step1 = false;
    }
    if ($("#txtkqsjswS").val() == "") {
        $.messager.alert("提示信息", "请输入 上午上班时间");
        bl_step1 = false;
    }
    if ($("#txtkqsjswE").val() == "") {
        $.messager.alert("提示信息", "请输入 上午下班时间");
        bl_step1 = false;
    }
    if ($("#txtkqsjxwS").val() == "") {
        $.messager.alert("提示信息", "请输入 下午上班时间");
        bl_step1 = false;
    }
    if ($("#txtkqsjxwE").val() == "") {
        $.messager.alert("提示信息", "请输入 下午下班时间");
        bl_step1 = false;
    }

    if ($('#Checkbox_dxkq').is(':checked')) {

        if ($("#txtkqsjswSd").val() == "") {
            $.messager.alert("提示信息", "请输入 冬季上午上班时间");
            bl_step1 = false;
        }
        if ($("#txtkqsjswEd").val() == "") {
            $.messager.alert("提示信息", "请输入 冬季上午下班时间");
            bl_step1 = false;
        }
        if ($("#txtkqsjxwSd").val() == "") {
            $.messager.alert("提示信息", "请输入 冬季下午上班时间");
            bl_step1 = false;
        }
        if ($("#txtkqsjxwEd").val() == "") {
            $.messager.alert("提示信息", "请输入 冬季下午下班时间");
            bl_step1 = false;
        }
    }
    //if ($("#txtbhks").val() == "") {
    //    $.messager.alert("提示信息", "请输入 本单位包含所有科室");
    //    bl_step1 = false;
    //}
    if ($("#txtdwjj").val() == "") {
        $.messager.alert("提示信息", "请输入 单位简介");
        bl_step1 = false;
    }
    return bl_step1;
};

function SaveStep1() {
    //本手册适用范围
    var syfw0415 = '';
    $("input:checkbox[name='checkboxscfw']:checked").each(function (i) {
        if (0 == i) {
            syfw0415 = $(this).val();
        } else {
            syfw0415 += '、' + $(this).val();
        }
    })

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step1"
        , dwjc: $("#txtdwjc").val()//手册颁发日期
        , dwqc: $("#txtdwqc").val()//单位全称 发票抬头（Invoice）  
        , Invoice: $("#txtInvoice").val()//单位全称 发票抬头（Invoice）      
        , Code: $("#TexttyxyID").val()//社会统一信用代码
        , Lxr: $("#TextName").val()//发票收件人
        , Phone: $("#TextTel").val()//手机号
        , QQ: $("#TextQQ").val()//手机号
        , Address: $("#TextAddress").val()//邮寄地址
        , syfw0415: syfw0415 //本手册适用范围
        //考勤时间
        , kqsjswS: $("#txtkqsjswS").val()
        , kqsjswE: $("#txtkqsjswE").val()
        , kqsjxwS: $("#txtkqsjxwS").val()
        , kqsjxwE: $("#txtkqsjxwE").val()
        , kqsjswSd: $("#txtkqsjswSd").val()
        , kqsjswEd: $("#txtkqsjswEd").val()
        , kqsjxwSd: $("#txtkqsjxwSd").val()
        , kqsjxwEd: $("#txtkqsjxwEd").val()
        , bhks: $("#txtbhks").val()//本单位包含所有科室
        , dwjj: $("#txtdwjj").val()//单位简介
    };
    return Save(postData);
}

function CheckStep2() {
    var bl_step2 = true;
    if ($("input[name='dzzjgmc']:checked").length == 0) {
        $.messager.alert("提示信息", "请选择一个【党组织机构名称】");
        bl_step2 = false;
    }
    if ($("#txtzzzwmc").val() == "") {
        $.messager.alert("提示信息", "请输入 正职职务名称");
        bl_step2 = false;
    }
    if ($("#txtldzzmc").val() == "") {
        $.messager.alert("提示信息", "请输入 正职领导姓名");
        bl_step2 = false;
    }
    return bl_step2;
};

function SaveStep2() {

    //副职
    var sort = '';
    var fzzwmc = '';
    var ldfzmc = '';
    var ldfzfg = '';
    var fzzwDY = '';
    var rowNum = 2;
    $('#table_fz tr').each(function (i) {
        if (i % 2 == 1) {
            sort += ',' + rowNum;
            fzzwmc += ',' + $("#txtfzzwmc" + rowNum).val();
            ldfzmc += ',' + $("#txtldfzmc" + rowNum).val();
            ldfzfg += ',' + $("#txtldfzfg" + rowNum).val();
            fzzwDY += ',' + $("input[name='fzzwDY" + rowNum + "']:checked").val();
            rowNum += 1;
        }
    });

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step2"
        , dzzjgmc: $("input[name='dzzjgmc']:checked").val()//党组织机构
        , zzzwmc: $("#txtzzzwmc").val()
        , ldzzmc: $("#txtldzzmc").val()
        , ldzzfg: $("#txtldzzfg").val()
        , zzzwDY: $("input[name='zzzwDY']:checked").val()
        , fzzwmc1: $("#txtfzzwmc1").val()
        , ldfzmc1: $("#txtldfzmc1").val()
        , ldfzfg1: $("#txtldfzfg1").val()
        , fzzwDY: $("input[name='fzzwDY']:checked").val()
        //自定义副职
        , sort: sort.substr(1)
        , fzzwmc: fzzwmc.substr(1)
        , ldfzmc: ldfzmc.substr(1)
        , ldfzfg: ldfzfg.substr(1)
        , fzzwDYY: fzzwDY.substr(1)
    };
    return Save(postData);
}

function CheckStep3() {
    var bl_step3 = true;
    if ($("#txtscqtbm").val() == "") {
        $.messager.alert("提示信息", "请输入 手册编制牵头部门");
        bl_step3 = false;
    }
    if ($("#txtscxzbm").val() == "") {
        $.messager.alert("提示信息", "请输入 手册编制协作部门");
        bl_step3 = false;
    }
    //领导小组
    if ($("#txtnkldxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制领导小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtnkldxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制领导小组：组长");
        bl_step3 = false;
    }
    if ($("#txtnkldxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制领导小组：副组长");
        bl_step3 = false;
    }
    //工作小组
    if ($("#txtnbkzgzxzzzqt01").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制工作小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtnbkzgzxzzzcy01").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制工作小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtnbkzgzxzzz01").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制工作小组：组长");
        bl_step3 = false;
    }
    if ($("#txtnbkzgzxzfzz01").val() == "") {
        $.messager.alert("提示信息", "请输入 内部控制工作小组：副组长");
        bl_step3 = false;
    }
    //风险评估
    if ($("#txtfxpgxzqtks").val() == "") {
        $.messager.alert("提示信息", "请输入 风险评估工作小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtfxpgxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 风险评估工作小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtfxpgxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 风险评估工作小组：组长");
        bl_step3 = false;
    }
    if ($("#txtfxpgxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 风险评估工作小组：副组长");
        bl_step3 = false;
    }
    //预算管理
    if ($("#txtysldxzqdks").val() == "") {
        $.messager.alert("提示信息", "请输入 预算管理领导小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtysldxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 预算管理领导小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtysldxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 预算管理领导小组：组长");
        bl_step3 = false;
    }
    if ($("#txtysldxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 预算管理领导小组：副组长");
        bl_step3 = false;
    }
    //政府采购
    if ($("#txtzfcgxzqdks").val() == "") {
        $.messager.alert("提示信息", "请输入 政府采购领导小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtzfcgxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 政府采购领导小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtzfcgxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 政府采购领导小组：组长");
        bl_step3 = false;
    }
    if ($("#txtzfcgxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 政府采购领导小组：副组长");
        bl_step3 = false;
    }
    //国有资产
    if ($("#txtgyzcxzqdks").val() == "") {
        $.messager.alert("提示信息", "请输入 国有资产管理小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtgyzcxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 国有资产管理小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtgyzcxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 国有资产管理小组：组长");
        bl_step3 = false;
    }
    if ($("#txtgyzcxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 国有资产管理小组：副组长");
        bl_step3 = false;
    }
    //监督检查
    if ($("#txtjdjcxzqdks").val() == "") {
        $.messager.alert("提示信息", "请输入 监督检查工作小组：牵头科室");
        bl_step3 = false;
    }
    if ($("#txtjdjcxzcy").val() == "") {
        $.messager.alert("提示信息", "请输入 监督检查工作小组：成员信息");
        bl_step3 = false;
    }
    if ($("#txtjdjcxzzz").val() == "") {
        $.messager.alert("提示信息", "请输入 监督检查工作小组：组长");
        bl_step3 = false;
    }
    if ($("#txtjdjcxzfzz").val() == "") {
        $.messager.alert("提示信息", "请输入 监督检查工作小组：副组长");
        bl_step3 = false;
    }
    return bl_step3;
};

function SaveStep3() {
    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step3"
        , scqtbm: $("#txtscqtbm").val()
        , scxzbm: $("#txtscxzbm").val()
        , nkldxzcy: $("#txtnkldxzcy").val()
        , nkldxzzz: $("#txtnkldxzzz").val()
        , nkldxzfzz: $("#txtnkldxzfzz").val()
        , nbkzgzxzzz01: $("#txtnbkzgzxzzz01").val()
        , nbkzgzxzfzz01: $("#txtnbkzgzxzfzz01").val()
        , nbkzgzxzzzcy01: $("#txtnbkzgzxzzzcy01").val()
        , nbkzgzxzzzqt01: $("#txtnbkzgzxzzzqt01").val()
        , fxpgxzqtks: $("#txtfxpgxzqtks").val()
        , fxpgxzcy: $("#txtfxpgxzcy").val()
        , fxpgxzzz: $("#txtfxpgxzzz").val()
        , fxpgxzfzz: $("#txtfxpgxzfzz").val()
        , ysldxzcy: $("#txtysldxzcy").val()
        , ysldxzqdks: $("#txtysldxzqdks").val()
        , ysldxzzz: $("#txtysldxzzz").val()
        , ysldxzfzz: $("#txtysldxzfzz").val()
        , zfcgxzcy: $("#txtzfcgxzcy").val()
        , zfcgxzqdks: $("#txtzfcgxzqdks").val()
        , zfcgxzzz: $("#txtzfcgxzzz").val()
        , zfcgxzfzz: $("#txtzfcgxzfzz").val()
        , gyzcxzcy: $("#txtgyzcxzcy").val()
        , gyzcxzqdks: $("#txtgyzcxzqdks").val()
        , gyzcxzzz: $("#txtgyzcxzzz").val()
        , gyzcxzfzz: $("#txtgyzcxzfzz").val()
        , jdjcxzcy: $("#txtjdjcxzcy").val()
        , jdjcxzqdks: $("#txtjdjcxzqdks").val()
        , jdjcxzzz: $("#txtjdjcxzzz").val()
        , jdjcxzfzz: $("#txtjdjcxzfzz").val()
    };
    return Save(postData);
}

function CheckStep4() {
    var bl_step4 = true;
    var bhyw = ''; //已有业务
    $("input:checkbox[name='checkboxYW']:checked").each(function (i) {
        if (0 == i) {
            bhyw = $(this).val();
        } else {
            bhyw += ',' + $(this).val();
        }
    })

    if (bhyw.length == 0) {
        $.messager.alert("提示信息", "请勾选 本单位包含业务内容");
        bl_step4 = false;
    }
    if ($("#txtywslzdmc").val() == "") {
        $.messager.alert("提示信息", "请输入 业务梳理制度的包括业务流程名称");
        bl_step4 = false;
    }
    //负责科室 行1
    if ($("#txtnbsjks").val() == "") {
        $.messager.alert("提示信息", "请输入 内部审计负责科室名称");
        bl_step4 = false;
    }
    if ($("#txtzdjcsshpjks").val() == "") {
        $.messager.alert("提示信息", "请输入 重大决策实施后评价负责科室");
        bl_step4 = false;
    }
    if ($("#txtbxrgwzdks").val() == "") {
        $.messager.alert("提示信息", "请输入 不相容岗位分离制度解释科室");
        bl_step4 = false;
    }
    //行2
    if ($("#txtbzndlgjhks").val() == "") {
        $.messager.alert("提示信息", "请输入 编制年度岗位轮岗计划的科室");
        bl_step4 = false;
    }
    if ($("#txtzdbgdgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 重大事项内部报告的归口管理科室");
        bl_step4 = false;
    }
    //行3
    if ($("#txtzwxxgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 政务信息公开制度牵头科室");
        bl_step4 = false;
    }
    if ($("#txtxxgkzrjjks").val() == "") {
        $.messager.alert("提示信息", "请输入 信息公开责任追究归口科室");
        bl_step4 = false;
    }
    if ($("#txtfzxxglxtks").val() == "") {
        $.messager.alert("提示信息", "请输入 负责单位信息管理系统的科室");
        bl_step4 = false;
    }
    //行4
    if ($("#txtxxxcgzqtks").val() == "") {
        $.messager.alert("提示信息", "请输入 新闻宣传工作牵头管理科室");
        bl_step4 = false;
    }
    if ($("#txtksglks").val() == "") {
        $.messager.alert("提示信息", "请输入 考勤管理的归口科室");
        bl_step4 = false;
    }
    if ($("#txtlzjmytgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 廉政诫勉约谈的归口科室");
        bl_step4 = false;
    }
    //行5
    if ($("#txtbdwsrbk").val() == "") {
        $.messager.alert("提示信息", "请输入 本单位收入包括哪些内容");
        bl_step4 = false;
    }
    //行6
    if ($("#txtsrywgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 收入业务的归口管理部门");
        bl_step4 = false;
    }
    if ($("#txtjfzcgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 经费支出的归口管理部门");
        bl_step4 = false;
    }
    if ($("#txtzfcgzlgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 政府采购资料的保管归口科室");
        bl_step4 = false;
    }
    //行7
    if ($("#txtrsglzdgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 人事管理制度的归口科室");
        bl_step4 = false;
    }
    if ($("#txtrsglhbks").val() == "") {
        $.messager.alert("提示信息", "请输入 人事管理的回避规定监督归口科室");
        bl_step4 = false;
    }
    if ($("#txtnzkhgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 年终考核,开展民主评议的归口科室");
        bl_step4 = false;
    }
    //行8
    if ($("#txtzdbgcdks").val() == "") {
        $.messager.alert("提示信息", "请输入 重大事项报告存档科室");
        bl_step4 = false;
    }
    if ($("#txthtgkks1").val() == "") {
        $.messager.alert("提示信息", "请输入 合同管理的归口科室");
        bl_step4 = false;
    }
    if ($("#txtgwglgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 公文管理归口科室");
        bl_step4 = false;
    }
    //行9
    if ($("#txtgdzccz").val() == "") {
        $.messager.alert("提示信息", "请输入 固定资产处置的归口科室");
        bl_step4 = false;
    }
    if ($("#txtgdzcdb").val() == "") {
        $.messager.alert("提示信息", "请输入 固定资产调拨的归口科室");
        bl_step4 = false;
    }
    if ($("#txtgdzcgz").val() == "") {
        $.messager.alert("提示信息", "请输入 固定资产购置的归口科室");
        bl_step4 = false;
    }
    //行10
    if ($("#txtgdzcqc").val() == "") {
        $.messager.alert("提示信息", "请输入 固定资产清查的归口科室");
        bl_step4 = false;
    }
    if ($("#txtbgypglgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 办公用品管理归口科室");
        bl_step4 = false;
    }
    if ($("#txtyzglgkks").val() == "") {
        $.messager.alert("提示信息", "请输入 印章管理归口科室");
        bl_step4 = false;
    }
    //行11
//    if ($("input[name='gwkzd']:checked").val() == "1") {
//        if ($("#txtgwkglks").val() == "") {
//            $.messager.alert("提示信息", "请输入 公务卡制度管理归口科室");
//            bl_step4 = false;
//        }
//        if ($("#txtgwkjdks").val() == "") {
//            $.messager.alert("提示信息", "请输入 公务卡监督执行归口科室");
//            bl_step4 = false;
//        }
//    }
    //行12
    if (typeof ($("input[name='EngineRoom']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 是否有机房!', 'warning');
        bl_step4 = false;
    }
    if ($("input[name='zxzjgl']:checked").val() == "1") {
        if ($("#txtczzxzjgkks").val() == "") {
            $.messager.alert("提示信息", "请输入 财政专项资金归口科室");
            bl_step4 = false;
        }
    }
    //行13
    //    if ($("#txtjsxmgkks01").val() == "") {
    //        $.messager.alert("提示信息", "请输入 建设项目归口主管科室");
    //        return;
    //    }
    //    if ($("#txtjsxmjxpjks01").val() == "") {
    //        $.messager.alert("提示信息", "请输入 建设项目绩效评价管理科室");
    //        return;
    //    }
    return bl_step4;
};

function SaveStep4() {

    var bhyw = ''; //已有业务
    $("input:checkbox[name='checkboxYW']:checked").each(function (i) {
        if (0 == i) {
            bhyw = $(this).val();
        } else {
            bhyw += ',' + $(this).val();
        }
    })

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step4"
        , bhyw: bhyw//本单位包含业务内容
        , ywslzdmc: $("#txtywslzdmc").val()//业务梳理制度的包括业务流程名称 
        , nbsjks: $("#txtnbsjks").val()//内部审计负责科室名称
        , zdjcsshpjks: $("#txtzdjcsshpjks").val()//重大决策实施后评价负责科室
        , bxrgwzdks: $("#txtbxrgwzdks").val()//不相容岗位分离制度解释科室
        , bzndlgjhks: $("#txtbzndlgjhks").val()//编制年度岗位轮岗计划的科室
        , bnlgdgwmc: $("#txtbnlgdgwmc").val()//不能轮岗的岗位包括哪些?
        , zdbgdgkks: $("#txtzdbgdgkks").val()//重大事项内部报告的归口管理科室
        , zwxxgkks: $("#txtzwxxgkks").val()//政务信息公开制度牵头科室
        , xxgkzrjjks: $("#txtxxgkzrjjks").val()//信息公开责任追究归口科室
        , fzxxglxtks: $("#txtfzxxglxtks").val()//负责单位信息管理系统的科室
        , xxxcgzqtks: $("#txtxxxcgzqtks").val()//新闻宣传工作牵头管理科室
        , ksglks: $("#txtksglks").val()//考勤管理的归口科室
        , lzjmytgkks: $("#txtlzjmytgkks").val()//廉政诫勉约谈的归口科室 
        , bdwsrbk: $("#txtbdwsrbk").val()//本单位收入包括哪些内容
        , srywgkks: $("#txtsrywgkks").val()//收入业务的归口管理部门
        , jfzcgkks: $("#txtjfzcgkks").val()//经费支出的归口管理部门 
        , zfcgzlgkks: $("#txtzfcgzlgkks").val()//政府采购资料的保管归口科室
        , rsglzdgkks: $("#txtrsglzdgkks").val()//人事管理制度的归口科室
        , rsglhbks: $("#txtrsglhbks").val()//人事管理的回避规定监督归口科室
        , nzkhgkks: $("#txtnzkhgkks").val()//年终考核,开展民主评议的归口科室
        , zdbgcdks: $("#txtzdbgcdks").val()//重大事项报告存档科室
        , htgkks1: $("#txthtgkks1").val()//合同管理的归口科室
        , gwglgkks: $("#txtgwglgkks").val()//公文管理归口科室
        , gdzccz: $("#txtgdzccz").val()//固定资产处置的归口科室
        , gdzcdb: $("#txtgdzcdb").val()//固定资产调拨的归口科室
        , gdzcgz: $("#txtgdzcgz").val()//固定资产购置的归口科室
        , gdzcqc: $("#txtgdzcqc").val()//固定资产清查的归口科室
        , bgypglgkks: $("#txtbgypglgkks").val()//办公用品管理归口科室
        , yzglgkks: $("#txtyzglgkks").val()//印章管理归口科室
        , gwkzd: $("#Checkboxh").is(':checked') ? "1" : "0"//公务卡制度
        , gwkglks: $("#txtgwkglks").val()//公务卡制度管理归口科室
        , gwkjdks: $("#txtgwkjdks").val()//公务卡监督执行归口科室
        , EngineRoom: $("input[name='EngineRoom']:checked").val()//是否有机房
        , zxzjgl: $("input[name='zxzjgl']:checked").val()//是否需要财政专项资金管理办法
        , czzxzjgkks: $("#txtczzxzjgkks").val()//财政专项资金归口科室
        , jsxmgkks01: $("#txtjsxmgkks01").val()//建设项目归口主管科室
        , jsxmjxpjks01: $("#txtjsxmjxpjks01").val()//建设项目绩效评价管理科室
    };
    return Save(postData);
};

function CheckStep5() {
    //政府采购合同授权审批设置
    var bl_step5 = true;
    var blcght = true;
    if ($("input[name='Radio_cghtsq']:checked").val() == "1") {
        if ($('#table_cghtsq tr').length < 1) {
            $.messager.alert('系统提示', "请输入政府采购合同授权审批流程", 'warning');
            bl_step5 = false;
        }
        $('#table_cghtsq tr').each(function (j) {
            $("div input[comboname='divsplc']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    blcght = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_cghtsq']:checked").val() == "0") {
        $("#div_cghtsq input[comboname='divsplc']").each(function (i) {
            if ($(this).val() == "") {
                blcght = false;
            }
        });
    }

    if (!blcght) {
        $.messager.alert('系统提示', "请选择政府采购合同授权审核审批人", 'warning');
        bl_step5 = false;
    }

    //自行采购合同授权审批设置
    var zxblcght = true;
    if ($("input[name='Radio_zxcgsq']:checked").val() == "1") {
        if ($('#table_zxcgsq tr').length < 1) {
            $.messager.alert('系统提示', "请输入自行采购合同授权审批流程", 'warning');
            bl_step5 = false;
        }
        $('#table_zxcgsq tr').each(function (j) {
            $("div input[comboname='divzxsplc']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    zxblcght = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_zxcgsq']:checked").val() == "0") {
        $("#div_zxcghtsq input[comboname='divzxsplc']").each(function (i) {
            if ($(this).val() == "") {
                zxblcght = false;
            }
        });
    }

    if (!zxblcght) {
        $.messager.alert('系统提示', "请选择自行采购合同授权审核审批人", 'warning');
        bl_step5 = false;
    }

    //预算内资金支付
    var blzc = true;
    if ($("input[name='Radio_zjzf']:checked").val() == "1") {
        if ($('#table_zjzf tr').length < 1) {
            $.messager.alert('系统提示', "请输入资金支付审批流程", 'warning');
            bl_step5 = false;
        }
        $('#table_zjzf tr').each(function (j) {
            $("div input[comboname='divysnzjzf']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    blzc = false;
                }
            });
        });
    }
    else {
        $("#div_ysnzjzf input[comboname='divysnzjzf']").each(function (i) {
            if ($(this).val() == "") {
                blzc = false;
            }
        });
    }

    if (!blzc) {
        $.messager.alert('系统提示', "请选择支出业务审核审批人", 'warning');
        bl_step5 = false;
    }

    //财政专项资金支付
    var czblzc = true;
    if ($("input[name='Radio_czzxzj']:checked").val() == "1") {
        if ($('#table_czzxzj tr').length < 1) {
            $.messager.alert('系统提示', "请输入财政专项资金支付审批流程", 'warning');
            bl_step5 = false;
        }
        $('#table_czzxzj tr').each(function (j) {
            $("div input[comboname='divczzxzj']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    czblzc = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_czzxzj']:checked").val() == "0") {
        $("#div_czzxzj input[comboname='divczzxzj']").each(function (i) {

            if ($(this).val() == "") {
                czblzc = false;
            }
        });
    }

    if (!czblzc) {
        $.messager.alert('系统提示', "请选择财政专项资金支付业务审核审批人", 'warning');
        bl_step5 = false;
    }

    //非财政专项资金支付
    var fczblzc = true;
    if ($("input[name='Radio_fczzxzj']:checked").val() == "1") {
        if ($('#table_fczzxzj tr').length < 1) {
            $.messager.alert('系统提示', "请输入非财政专项资金支付审批流程", 'warning');
            bl_step5 = false;
        }
        $('#table_fczzxzj tr').each(function (j) {
            $("div input[comboname='divfczzxzj']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    fczblzc = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_fczzxzj']:checked").val() == "0") {
        $("#div_fczzxzj input[comboname='divfczzxzj']").each(function (i) {
            if ($(this).val() == "") {
                fczblzc = false;
            }
        });
    }

    if (!fczblzc) {
        $.messager.alert('系统提示', "请选择非财政专项资金业务审核审批人", 'warning');
        bl_step5 = false;
    }

    //借款
    var bljk = true;
    if ($("input[name='Radio_jksh']:checked").val() == "1") {
        if ($('#table_jksh tr').length < 1) {
            $.messager.alert('系统提示', "请输入借款审批流程", 'warning');
            return false;
        }
        $('#table_jksh tr').each(function (j) {
            $("div input[comboname='divjksh']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    bljk = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_jksh']:checked").val() == "0") {
        $("#div_jksh input[comboname='divjksh']").each(function (i) {
            if ($(this).val() == "") {
                bljk = false;
            }
        });
    }

    if (!bljk) {
        $.messager.alert('系统提示', "请选择借款业务审核审批人", 'warning');
        bl_step5 = false;
    }

    //报销
    var blbx = true;
    if ($("input[name='Radio_bxsh']:checked").val() == "1") {
        if ($('#table_bxsh tr').length < 1) {
            $.messager.alert('系统提示', "请输入报销审批流程", 'warning');
            return false;
        }
        $('#table_bxsh tr').each(function (j) {
            $("div input[comboname='divbxsh']", $(this)).each(function (i) {
                if ($(this).val() == "") {
                    blbx = false;
                }
            });
        });
    }
    else if ($("input[name='Radio_bxsh']:checked").val() == "0") {
        $("#div_bxsh input[comboname='divbxsh']").each(function (i) {
            if ($(this).val() == "") {
                blbx = false;
            }
        });
    }

    if (!blbx) {
        $.messager.alert('系统提示', "请选择报销业务审核审批人", 'warning');
        bl_step5 = false;
    }

    if ($("#txtjine041509").val() == "") {
        $.messager.alert('系统提示', '请输入 本单位现金支付授权范围中内容!', 'warning');
        bl_step5 = false;
    }
    if ($("#txtjine0407").val() == "" || $("#txtjine0408").val() == "") {
        $.messager.alert('系统提示', '请输入 固定资产标准：实物资产制度、固定资产制度中内容!', 'warning');
        bl_step5 = false;
    }
    return bl_step5;
};

function SaveStep5() {
    //政府采购合同授权审批设置
    var cghtMoney = '', cghtType = '', cghtSpr = '';
    if ($("input[name='Radio_cghtsq']:checked").val() == "1") {
        if ($('#table_cghtsq tr').length > 0) {
            $('#table_cghtsq tr').each(function (j) {
                if (0 == j) {
                    cghtMoney = $("div", $(this)).attr('jkmoney');
                    cghtType = $("div", $(this)).attr('jktype');
                }
                else {
                    cghtSpr += '∮';
                    cghtMoney += '∮' + $("div", $(this)).attr('jkmoney');
                    cghtType += '∮' + $("div", $(this)).attr('jktype');
                }
                var len = $(this).find("div input[comboname='divsplc']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        cghtSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        cghtSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else if ($("input[name='Radio_cghtsq']:checked").val() == "0") {
        var len = $("#div_cghtsq input[comboname='divsplc']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                cghtSpr += $("#div_cghtsq_" + i).combobox("getValue");
            } else {
                cghtSpr += '、' + $("#div_cghtsq_" + i).combobox("getValue");
            }
        }
    }

    //自行采购合同授权审批设置
    var zxcghtMoney = '', zxcghtType = '', zxcghtSpr = '';
    if ($("input[name='Radio_zxcgsq']:checked").val() == "1") {
        if ($('#table_zxcgsq tr').length > 0) {
            $('#table_zxcgsq tr').each(function (j) {
                if (0 == j) {
                    zxcghtMoney = $("div", $(this)).attr('jkmoney');
                    zxcghtType = $("div", $(this)).attr('jktype');
                }
                else {
                    zxcghtSpr += '∮';
                    zxcghtMoney += '∮' + $("div", $(this)).attr('jkmoney');
                    zxcghtType += '∮' + $("div", $(this)).attr('jktype');
                }
                var len = $(this).find("div input[comboname='divzxsplc']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        zxcghtSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        zxcghtSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else if ($("input[name='Radio_zxcgsq']:checked").val() == "0") {
        var len = $("#div_zxcghtsq input[comboname='divzxsplc']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                zxcghtSpr += $("#div_zxcghtsq_" + i).combobox("getValue");
            } else {
                zxcghtSpr += '、' + $("#div_zxcghtsq_" + i).combobox("getValue");
            }
        }
    }

    //预算内支出
    var zcywMoney = '', zcywType = '', zcywSpr = '';
    if ($("input[name='Radio_zjzf']:checked").val() == "1") {
        if ($('#table_zjzf tr').length > 0) {
            $('#table_zjzf tr').each(function (j) {
                if (0 == j) {
                    zcywMoney = $("div", $(this)).attr('zcmoney');
                    zcywType = $("div", $(this)).attr('zctype');
                }
                else {
                    zcywSpr += '∮';
                    zcywMoney += '∮' + $("div", $(this)).attr('zcmoney');
                    zcywType += '∮' + $("div", $(this)).attr('zctype');
                }
                var len = $(this).find("div input[comboname='divysnzjzf']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        zcywSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        zcywSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else {
        var len = $("#div_ysnzjzf input[comboname='divysnzjzf']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                zcywSpr += $("#div_ysnzjzf_" + i).combobox("getValue");
            } else {
                zcywSpr += '、' + $("#div_ysnzjzf_" + i).combobox("getValue");
            }
        }
    }

    //财政专项资金支出
    var czzcywMoney = '', czzcywType = '', czzcywSpr = '';
    if ($("input[name='Radio_czzxzj']:checked").val() == "1") {
        if ($('#table_czzxzj tr').length > 0) {
            $('#table_czzxzj tr').each(function (j) {
                if (0 == j) {
                    czzcywMoney = $("div", $(this)).attr('zcmoney');
                    czzcywType = $("div", $(this)).attr('zctype');
                }
                else {
                    czzcywSpr += '∮';
                    czzcywMoney += '∮' + $("div", $(this)).attr('zcmoney');
                    czzcywType += '∮' + $("div", $(this)).attr('zctype');
                }
                var len = $(this).find("div input[comboname='divczzxzj']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        czzcywSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        czzcywSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else {
        var len = $("#div_czzxzj input[comboname='divczzxzj']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                czzcywSpr += $("#div_czzxzj_" + i).combobox("getValue");
            } else {
                czzcywSpr += '、' + $("#div_czzxzj_" + i).combobox("getValue");
            }
        }
    }

    //非财政专项资金支出
    var fczzcywMoney = '', fczzcywType = '', fczzcywSpr = '';
    if ($("input[name='Radio_fczzxzj']:checked").val() == "1") {
        if ($('#table_fczzxzj tr').length > 0) {
            $('#table_fczzxzj tr').each(function (j) {
                if (0 == j) {
                    fczzcywMoney = $("div", $(this)).attr('zcmoney');
                    fczzcywType = $("div", $(this)).attr('zctype');
                }
                else {
                    fczzcywSpr += '∮';
                    fczzcywMoney += '∮' + $("div", $(this)).attr('zcmoney');
                    fczzcywType += '∮' + $("div", $(this)).attr('zctype');
                }
                var len = $(this).find("div input[comboname='divfczzxzj']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        fczzcywSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        fczzcywSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else {
        var len = $("#div_fczzxzj input[comboname='divfczzxzj']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                fczzcywSpr += $("#div_fczzxzj_" + i).combobox("getValue");
            } else {
                fczzcywSpr += '、' + $("#div_fczzxzj_" + i).combobox("getValue");
            }
        }
    }

    //借款
    var jkywMoney = '', jkywType = '', jkywSpr = '';
    if ($("input[name='Radio_jksh']:checked").val() == "1") {
        if ($('#table_jksh tr').length > 0) {
            $('#table_jksh tr').each(function (j) {
                if (0 == j) {
                    jkywMoney = $("div", $(this)).attr('jkmoney');
                    jkywType = $("div", $(this)).attr('jktype');
                }
                else {
                    jkywSpr += '∮';
                    jkywMoney += '∮' + $("div", $(this)).attr('jkmoney');
                    jkywType += '∮' + $("div", $(this)).attr('jktype');
                }
                var len = $(this).find("div input[comboname='divjksh']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        jkywSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        jkywSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else if ($("input[name='Radio_jksh']:checked").val() == "0") {
        var len = $("#div_jksh input[comboname='divjksh']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                jkywSpr += $("#div_jksh_" + i).combobox("getValue");
            } else {
                jkywSpr += '、' + $("#div_jksh_" + i).combobox("getValue");
            }
        }
    }

    //报销
    var bxywMoney = '', bxywType = '', bxywSpr = '';
    if ($("input[name='Radio_bxsh']:checked").val() == "1") {
        if ($('#table_bxsh tr').length > 0) {
            $('#table_bxsh tr').each(function (j) {
                if (0 == j) {
                    bxywMoney = $("div", $(this)).attr('bxmoney');
                    bxywType = $("div", $(this)).attr('bxtype');
                }
                else {
                    bxywSpr += '∮';
                    bxywMoney += '∮' + $("div", $(this)).attr('bxmoney');
                    bxywType += '∮' + $("div", $(this)).attr('bxtype');
                }
                var len = $(this).find("div input[comboname='divbxsh']").length;
                var divid = $(this).find("div").attr("id");
                for (var i = 0; i < len; i++) {
                    if (0 == i) {
                        bxywSpr += $("#" + divid + "_" + i).combobox("getValue");
                    } else {
                        bxywSpr += '、' + $("#" + divid + "_" + i).combobox("getValue");
                    }
                }
            });
        }
    }
    else if ($("input[name='Radio_bxsh']:checked").val() == "0") {
        var len = $("#div_bxsh input[comboname='divbxsh']").length;
        for (var i = 0; i < len; i++) {
            if (0 == i) {
                bxywSpr += $("#div_bxsh_" + i).combobox("getValue");
            } else {
                bxywSpr += '、' + $("#div_bxsh_" + i).combobox("getValue");
            }
        }
    }

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step5"
        //政府采购合同授权审批设置
        , Radio_cghtsq: $("input[name='Radio_cghtsq']:checked").val()
        , cghtMoney: cghtMoney
        , cghtType: cghtType
        , cghtSpr: cghtSpr
        //自行采购合同授权审批设置
        , Radio_zxcgsq: $("input[name='Radio_zxcgsq']:checked").val()
        , zxcghtMoney: zxcghtMoney
        , zxcghtType: zxcghtType
        , zxcghtSpr: zxcghtSpr
        //资金支付审批权限：支出管理制度
        , Radio_zjzf: $("input[name='Radio_zjzf']:checked").val()
        , zcywMoney: zcywMoney
        , zcywType: zcywType
        , zcywSpr: zcywSpr
        //财政专项资金支付审批权限：支出管理制度
        , Radio_czzxzj: $("input[name='Radio_czzxzj']:checked").val()
        , czzcywMoney: czzcywMoney
        , czzcywType: czzcywType
        , czzcywSpr: czzcywSpr
        //非财政专项资金支付审批权限：支出管理制度
        , Radio_fczzxzj: $("input[name='Radio_fczzxzj']:checked").val()
        , fczzcywMoney: fczzcywMoney
        , fczzcywType: fczzcywType
        , fczzcywSpr: fczzcywSpr
        //借款审批权限：支出管理制度 
        , Radio_jksh: $("input[name='Radio_jksh']:checked").val()
        , jkywMoney: jkywMoney
        , jkywType: jkywType
        , jkywSpr: jkywSpr
        //报销审批权限：资金支出审批管理办法 
        , Radio_bxsh: $("input[name='Radio_bxsh']:checked").val()
        , bxywMoney: bxywMoney
        , bxywType: bxywType
        , bxywSpr: bxywSpr
        , jine041509: $("#txtjine041509").val()//零星支出
        , jine0407: $("#txtjine0407").val()//固定资产 一般设备
        , jine0408: $("#txtjine0408").val()//固定资产 专用设备
    };
    return Save(postData);
};

function SaveStep6() {

    var kssort = '';
    var ksmc = '';
    var kszn = '';
    var kszr = '';
    var ksNum = 1;
    var trList = $("#table_ksgwzz tbody").children(".kstr");

    for (var i = 0; i < trList.length; i++) {
        if (i % 3 == 2) {
            kssort += '∮' + ksNum;
            ksmc += '∮' + $("#txtksmc" + ksNum).val();
            kszn += '∮' + $("#txtkszn" + ksNum).val();

            var zrsort = '';
            var gwmc = '';
            var gwzr = '';
            var gwNum = 1;
            $('#table_ksgwzr' + ksNum + ' tr').each(function (i) {
                zrsort += '∮' + gwNum;
                gwmc += '∮' + $("#txtksgw_" + ksNum + "_" + gwNum).val();
                gwzr += '∮' + $("#txtgwzr_" + ksNum + "_" + gwNum).val();
                gwNum += 1;
            });
            kszr += "☉" + zrsort.substr(1) + "¤" + gwmc.substr(1) + "¤" + gwzr.substr(1);
            ksNum += 1;
        }
    }

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step6"
        , kssort: kssort.substr(1)
        , ksmc: ksmc.substr(1)
        , kszn: kszn.substr(1)
        , kszr: kszr.substr(1)
    };
    return Save(postData);
};

function CheckStep7() {
    var bl_step7 = true;
    if (typeof ($("input[name='Radioclf']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 有无差旅费制度!', 'warning');
        bl_step7 = false;
    }
    if (typeof ($("input[name='Radiohyf']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 有无会议费制度!', 'warning');
        bl_step7 = false;
    }
    if (typeof ($("input[name='Radiopxf']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 有无培训费制度!', 'warning');
        bl_step7 = false;
    }
    if (typeof ($("input[name='Radiogwzdf']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 有无公务招待制度!', 'warning');
        bl_step7 = false;
    }
    if (typeof ($("input[name='Radiobzz']:checked").val()) == "undefined") {
        $.messager.alert('系统提示', '请勾选 财务核算，是否报账制!', 'warning');
        bl_step7 = false;
    }
    return bl_step7;
};

function SaveStep7() {

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step7"
        , Radioclf: $("input[name='Radioclf']:checked").val()
        , Radiohyf: $("input[name='Radiohyf']:checked").val()
        , Radiopxf: $("input[name='Radiopxf']:checked").val()
        , Radiogwzdf: $("input[name='Radiogwzdf']:checked").val()
        , Radiobzz: $("input[name='Radiobzz']:checked").val()
    };
    return Save(postData);
};

function CheckStep8() {
    var bl_step8 = true;
    if ($("#table_qlqd").find("tr").length < 1) {
        $.messager.alert('系统提示', '请填写领导权力清单!', 'warning');
        bl_step8 = false;
    }
    return bl_step8;
};

function SaveStep8() {

    var postData = {
        id: $("#Hiddenid").val()
        , CustomerID: $("#HiddenCustomerID").val()
        , Step: "Step8"
    };

    $("#table_qlqd .textarea").each(function () {
        postData[$(this).attr("ID")] = $(this).val();
    });

    return Save(postData);
};

function SaveStepEnd() {
    if (!CheckStep1()) {
        return false;
    }
    if (!CheckStep2()) {
        return false;
    }
    if (!CheckStep3()) {
        return false;
    }
    if (!CheckStep4()) {
        return false;
    }
    if (!CheckStep5()) {
        return false;
    }
    if (!CheckStep7()) {
        return false;
    }
    if (!CheckStep8()) {
        return false;
    }

    $.messager.confirm('系统提示', '<div style="font-size:19px; color:red; ">确定要 [提交] 基础信息吗? <br /><br />提交后将不能再修改基础信息，不提交点击【取消】按钮</div>', function (yes) {
        if (yes) {
            var postData = {
                id: $("#Hiddenid").val()
                , CustomerID: $("#HiddenCustomerID").val()
                , Step: "StepEnd"
                , flag: "7"
            };
            if (Save(postData)) {
                //默认选中第一页 向导
                //$('.actionBar a.buttonFinish').addClass("buttonDisabled"); //提交按钮变灰  
                //$('#wizard').smartWizard('skipTo', 2);
                $('.actionBar a.buttonFinish').hide(); //提交按钮变灰  
                $.messager.show({ title: '系统提示', msg: '提交成功' });
                window.location.reload();
            }

        }
        return false;
    });
};

function Save(postData) {
    var result = 0;
    $.ajax({
        type: "post",
        url: "/Nksc/addNksc",
        data: postData,
        dataType: "json",
        async: false,
        success: function (data) {
            result = data.type;
            if (data.type == 0) {
                $.messager.alert("提示信息", data.message);
            }
            else {
                $.messager.show({ title: '系统提示', msg: '保存成功' });
            }
        }
    });
    return result == 1;
}

function SaveStep() {
    switch (step_num_now) {
        case '1':
            SaveStep1();
            break;
        case '2':
            SaveStep2();
            break;
        case '3':
            SaveStep3();
            break;
        case '4':
            SaveStep4();
            break;
        case '5':
            SaveStep5();
            break;
        case '6':
            SaveStep6();
            break;
        case '7':
            SaveStep7();
            break;
        case '8':
            SaveStep8();
            break;
        case '9':
            SaveStepEnd();
            break;
        default:
            break;
    }
}

function Print() {
    window.open("/Nksc/NkscPrint?id=" + $("#Hiddenid").val(), "_blank");
}

function LookSwf() {
    window.open("/Sale/SWFView?id=" + $("#Hiddenid").val(), "_blank");
}

//function LookSwf() {
//    window.open("/Nksc/PFDView?id=" + $("#Hiddenid").val(), "_blank");
//}