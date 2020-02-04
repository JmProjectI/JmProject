var moneyTest;
var typeName;
function saveZJ() {
    $(moneyTest).next().attr(typeName + "money", $("#txtjineT").numberbox("getValue"));
    $(moneyTest).next().attr(typeName + "type", $("#dlljineC").combobox("getValue"));
    $(moneyTest).html($(moneyTest).attr("jtext") + $("#txtjineT").numberbox("getValue") + "元" + $("#dlljineC").combobox("getValue") + "的，由");
    $("#dlgZJ").dialog("close");
};

$(function () {

    //弹出金额修改对话框
    $("#divzjsp").on("click", ".moneyText", function () {
        typeName = "jk";
        if ($(this).attr("jtext").indexOf("单笔") >= 0) {
            typeName = "zc";
        }
        else if ($(this).attr("jtext").indexOf("报销") >= 0) {
            typeName = "bx";
        }
        moneyTest = this;
        $("#txtjineT").numberbox("setValue", $(this).next().attr(typeName + "money"));
        $("#dlljineC").combobox("setValue", $(this).next().attr(typeName + "type"));
        $("#dlgZJ").dialog("open").dialog('setTitle', '信息维护');
    });

    //政府采购合同审批设置
    $('input:radio[name="Radio_cghtsq"]').change(function () {
        if ($("input[name='Radio_cghtsq']:checked").val() == "1") {
            $("#tr_cghtsq1").hide();
            $("#tr_cghtsq2").show();
            $("#tr_cghtsq3").show();
        }
        else if ($("input[name='Radio_cghtsq']:checked").val() == "0") {
            $("#tr_cghtsq1").show();
            $("#tr_cghtsq2").hide();
            $("#tr_cghtsq3").hide();
        }
        else {
            $("#tr_cghtsq1").hide();
            $("#tr_cghtsq2").hide();
            $("#tr_cghtsq3").hide();
        }
    });

    //自行采购合同审批设置
    $('input:radio[name="Radio_zxcgsq"]').change(function () {
        if ($("input[name='Radio_zxcgsq']:checked").val() == "1") {
            $("#tr_zxcgsq1").hide();
            $("#tr_zxcgsq2").show();
            $("#tr_zxcgsq3").show();
        }
        else if ($("input[name='Radio_zxcgsq']:checked").val() == "0") {
            $("#tr_zxcgsq1").show();
            $("#tr_zxcgsq2").hide();
            $("#tr_zxcgsq3").hide();
        }
        else {
            $("#tr_zxcgsq1").hide();
            $("#tr_zxcgsq2").hide();
            $("#tr_zxcgsq3").hide();
        }
    });

    //预算内资金支付
    $('input:radio[name="Radio_zjzf"]').change(function () {
        if ($("input[name='Radio_zjzf']:checked").val() == "1") {
            $("#tr_zjzf1").hide();
            $("#tr_zjzf2").show();
            $("#tr_zjzf3").show();
        }
        else {
            $("#tr_zjzf1").show();
            $("#tr_zjzf2").hide();
            $("#tr_zjzf3").hide();
        }
    });

    //财政专项资金支付
    $('input:radio[name="Radio_czzxzj"]').change(function () {
        if ($("input[name='Radio_czzxzj']:checked").val() == "1") {
            $("#tr_czzxzj1").hide();
            $("#tr_czzxzj2").show();
            $("#tr_czzxzj3").show();
        }
        else if ($("input[name='Radio_czzxzj']:checked").val() == "0") {
            $("#tr_czzxzj1").show();
            $("#tr_czzxzj2").hide();
            $("#tr_czzxzj3").hide();
        }
        else {
            $("#tr_czzxzj1").hide();
            $("#tr_czzxzj2").hide();
            $("#tr_czzxzj3").hide();
        }
    });

    //非财政专项资金支付
    $('input:radio[name="Radio_fczzxzj"]').change(function () {
        if ($("input[name='Radio_fczzxzj']:checked").val() == "1") {
            $("#tr_fczzxzj1").hide();
            $("#tr_fczzxzj2").show();
            $("#tr_fczzxzj3").show();
        }
        else if ($("input[name='Radio_fczzxzj']:checked").val() == "0") {
            $("#tr_fczzxzj1").show();
            $("#tr_fczzxzj2").hide();
            $("#tr_fczzxzj3").hide();
        }
        else {
            $("#tr_fczzxzj1").hide();
            $("#tr_fczzxzj2").hide();
            $("#tr_fczzxzj3").hide();
        }
    });

    //借款审核
    $('input:radio[name="Radio_jksh"]').change(function () {
        if ($("input[name='Radio_jksh']:checked").val() == "1") {
            $("#tr_jksh1").hide();
            $("#tr_jksh2").show();
            $("#tr_jksh3").show();
        }
        else if ($("input[name='Radio_jksh']:checked").val() == "0") {
            $("#tr_jksh1").show();
            $("#tr_jksh2").hide();
            $("#tr_jksh3").hide();
        }
        else {
            $("#tr_jksh1").hide();
            $("#tr_jksh2").hide();
            $("#tr_jksh3").hide();
        }
    });

    //报销业务
    $('input:radio[name="Radio_bxsh"]').change(function () {
        if ($("input[name='Radio_bxsh']:checked").val() == "1") {
            $("#tr_bxsh1").hide();
            $("#tr_bxsh2").show();
            $("#tr_bxsh3").show();
        }
        else {
            $("#tr_bxsh1").show();
            $("#tr_bxsh2").hide();
            $("#tr_bxsh3").hide();
        }
    });

    //零星支出
    $("#CheckboxXJZF").on({ "click": function () {
        if ($("#CheckboxXJZF").is(':checked')) {
            $("#txtjine041509").numberbox('setValue', '0');
            $("#tr_XjzfLxzc").hide();
        }
        else {
            $("#txtjine041509").numberbox('setValue', '');
            $("#tr_XjzfLxzc").show();
        }
    }
    });
});

//审批流程
function splc(div_id) {
    $('#' + div_id).combobox({
        panelHeight: 'auto',
        url: '/Basic/GetComb_SpLcDictionaryItem?DicID=027',
        valueField: 'ItemName',
        textField: 'ItemName',
        onLoadSuccess: function (node, data) {
            //$("#" + div_id).combobox('setValue', '@Model.BM');
        }
    });
}

//政府采购合同start
//政府采购tr行数（审批行数）
var tr_cg_count = 0;
function addcghtsq() {
    if ($("#txtjine_cghtsq").val() == "") {
        $.messager.alert("提示信息", "请输入金额");
        return;
    }
    if ($("#dlljine_cghtsqT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入控制范围");
        return;
    }
    var rowcount = $("#table_cghtsq tr").length;
    if (tr_cg_count == 0) {
        tr_cg_count = rowcount;
    }
    else {
        tr_cg_count++;
    }
    var trHTML = '<tr id="table_tr_cghtsq' + tr_cg_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_zxcgsq_DelB" onclick="delcghtsq(\'table_tr_cghtsq' + tr_cg_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_cghtsq_AddS" onclick="addcghtsqs(\'div_cghtsq' + tr_cg_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_cghtsq_DelS" onclick="delcghtsqs(\'div_cghtsq' + tr_cg_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="金额在" style="float:left;margin-top:3px;">金额在' + $("#txtjine_cghtsq").val() + '元' + $("#dlljine_cghtsqT").combobox('getValue') + '的，由</span>' +
                 '    <div jkmoney="' + $("#txtjine_cghtsq").val() + '" jktype="' + $("#dlljine_cghtsqT").combobox('getValue') + '" id="div_cghtsq' + tr_cg_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_cghtsq' + tr_cg_count + '_0" name="divsplc" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_cghtsq").append(trHTML);
    splc("div_cghtsq" + tr_cg_count + "_0");
}

//删除步骤
function delcghtsq(cgtrid) {
    if ($("#table_cghtsq tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_cghtsq tr").length;
    if (tr_cg_count == 0) {
        tr_cg_count = rowcount;
    }
    $("#" + cgtrid).remove();
    //$("#table_cghtsq tr:last").remove();
}

//添加审核审批人
function addcghtsqs(divid) {
    var len = $("#" + divid + " input[name='divsplc']").length;
    var combHtml = '<span>、</span>' +
    '<input id="' + divid + '_' + len + '" name="divsplc" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delcghtsqs(divid) {
    if ($("#" + divid + " input[comboname='divsplc']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divsplc']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
//采购合同end

//自行采购合同start
//自行采购tr行数（审批行数）
var tr_zxcg_count = 0;
function addzxcghtsq() {
    if ($("#txtjine_zxcgsq").val() == "") {
        $.messager.alert("提示信息", "请输入金额");
        return;
    }
    if ($("#dlljine_zxcgsqT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入控制范围");
        return;
    }
    var rowcount = $("#table_zxcgsq tr").length;
    if (tr_zxcg_count == 0) {
        tr_zxcg_count = rowcount;
    }
    else {
        tr_zxcg_count++;
    }
    var trHTML = '<tr id="table_tr_zxcghtsq' + tr_zxcg_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_zxcgsq_DelB" onclick="delzxcghtsq(\'table_tr_zxcghtsq' + tr_zxcg_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_zxcgsq_AddS" onclick="addzxcghtsqs(\'div_zxcghtsq' + tr_zxcg_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_zxcgsq_DelS" onclick="delzxcghtsqs(\'div_zxcghtsq' + tr_zxcg_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="金额在" style="float:left;margin-top:3px;">金额在' + $("#txtjine_zxcgsq").val() + '元' + $("#dlljine_zxcgsqT").combobox('getValue') + '的，由</span>' +
                 '    <div jkmoney="' + $("#txtjine_zxcgsq").val() + '" jktype="' + $("#dlljine_zxcgsqT").combobox('getValue') + '" id="div_zxcghtsq' + tr_zxcg_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_zxcghtsq' + tr_zxcg_count + '_0" name="divzxsplc" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_zxcgsq").append(trHTML);
    splc("div_zxcghtsq" + tr_zxcg_count + "_0");
}

//删除步骤
function delzxcghtsq(zxcgtrid) {
    if ($("#table_zxcgsq tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_zxcgsq tr").length;
    if (tr_zxcg_count == 0) {
        tr_zxcg_count = rowcount;
    }
    $("#" + zxcgtrid).remove();
    //$("#table_zxcgsq tr:last").remove();
}

//添加审核审批人
function addzxcghtsqs(divid) {
    var len = $("#" + divid + " input[name='divzxsplc']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divzxsplc" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delzxcghtsqs(divid) {
    if ($("#" + divid + " input[comboname='divzxsplc']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divzxsplc']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
//自行采购合同end

/*预算内资金支付审核strart*/

//添加步骤
var tr_zjzf_count = 0;
function addzjzf() {

    if ($("#txtjine_zjzf").val() == "") {
        $.messager.alert("提示信息", "请输入资金支付金额");
        return;
    }
    if ($("#dlljine_zjzfT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入资金支付控制范围");
        return;
    }
    var rowcount = $("#table_zjzf tr").length;
    if (tr_zjzf_count == 0) {
        tr_zjzf_count = rowcount;
    }
    else {
        tr_zjzf_count++;
    }
    var trHTML = '<tr id="table_tr_zjzf' + tr_zjzf_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_zjzf_DelB" onclick="delzjzf(\'table_tr_zjzf' + tr_zjzf_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_zjzf_AddS" onclick="addzjzfs(\'div_ysnzjzf' + tr_zjzf_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_zjzf_DelS" onclick="delzjzfs(\'div_ysnzjzf' + tr_zjzf_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="单笔支出金额在" style="float:left;margin-top:3px;">单笔支出金额在' + $("#txtjine_zjzf").val() + '元' + $("#dlljine_zjzfT").combobox('getValue') + '的，由</span>' +
                 '    <div zcmoney="' + $("#txtjine_zjzf").val() + '" zctype="' + $("#dlljine_zjzfT").combobox('getValue') + '" id="div_ysnzjzf' + tr_zjzf_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_ysnzjzf' + tr_zjzf_count + '_0" name="divysnzjzf" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_zjzf").append(trHTML);
    splc("div_ysnzjzf" + tr_zjzf_count + "_0");
}

//删除步骤
function delzjzf(zjzftrid) {
    if ($("#table_zjzf tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_zjzf tr").length;
    if (tr_zjzf_count == 0) {
        tr_zjzf_count = rowcount;
    }
    $("#" + zjzftrid).remove();
    //$("#table_zjzf tr:last").remove();
}

//添加审核审批人
function addzjzfs(divid) {
    var len = $("#" + divid + " input[name='divysnzjzf']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divysnzjzf" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delzjzfs(divid) {
    if ($("#" + divid + " input[comboname='divysnzjzf']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divysnzjzf']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
/*预算内资金支付审核end*/

/*财政专项资金支付审核strart*/

//添加步骤
var tr_czzjzf_count = 0;
function addczzjzf() {

    if ($("#txtjine_czzxzj").val() == "") {
        $.messager.alert("提示信息", "请输入资金支付金额");
        return;
    }
    if ($("#dlljine_czzxzjT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入资金支付控制范围");
        return;
    }
    var rowcount = $("#table_czzxzj tr").length;
    if (tr_czzjzf_count == 0) {
        tr_czzjzf_count = rowcount;
    }
    else {
        tr_czzjzf_count++;
    }
    var trHTML = '<tr id="table_tr_czzxzj' + tr_czzjzf_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_czzxzj_DelB" onclick="delczzjzf(\'table_tr_czzxzj' + tr_czzjzf_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_czzxzj_AddS" onclick="addczzjzfs(\'div_czzxzj' + tr_czzjzf_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_czzxzj_DelS" onclick="delczzjzfs(\'div_czzxzj' + tr_czzjzf_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="单笔支出金额在" style="float:left;margin-top:3px;">单笔支出金额在' + $("#txtjine_czzxzj").val() + '元' + $("#dlljine_czzxzjT").combobox('getValue') + '的，由</span>' +
                 '    <div zcmoney="' + $("#txtjine_czzxzj").val() + '" zctype="' + $("#dlljine_czzxzjT").combobox('getValue') + '" id="div_czzxzj' + tr_czzjzf_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_czzxzj' + tr_czzjzf_count + '_0" name="divczzxzj" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_czzxzj").append(trHTML);
    splc("div_czzxzj" + tr_czzjzf_count + "_0");
}

//删除步骤
function delczzjzf(czzjzftrid) {
    if ($("#table_czzxzj tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_czzxzj tr").length;
    if (tr_czzjzf_count == 0) {
        tr_czzjzf_count = rowcount;
    }
    $("#" + czzjzftrid).remove();
    //$("#table_czzxzj tr:last").remove();
}

//添加审核审批人
function addczzjzfs(divid) {
    var len = $("#" + divid + " input[name='divczzxzj']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divczzxzj" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delczzjzfs(divid) {
    if ($("#" + divid + " input[comboname='divczzxzj']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divczzxzj']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
/*财政专项资金支付审核end*/

/*非财政专项资金支付审核strart*/

//添加步骤
var tr_fczzjzf_count = 0;
function addfczzjzf() {

    if ($("#txtjine_fczzxzj").val() == "") {
        $.messager.alert("提示信息", "请输入资金支付金额");
        return;
    }
    if ($("#dlljine_fczzxzjT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入资金支付控制范围");
        return;
    }
    var rowcount = $("#table_fczzxzj tr").length;
    if (tr_fczzjzf_count == 0) {
        tr_fczzjzf_count = rowcount;
    }
    else {
        tr_fczzjzf_count++;
    }
    var trHTML = '<tr id="table_tr_fczzxzj' + tr_fczzjzf_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_fczzxzj_DelB" onclick="delfczzjzf(\'table_tr_fczzxzj' + tr_fczzjzf_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_fczzxzj_AddS" onclick="addfczzjzfs(\'div_fczzxzj' + tr_fczzjzf_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_fczzxzj_DelS" onclick="delfczzjzfs(\'div_fczzxzj' + tr_fczzjzf_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="单笔支出金额在" style="float:left;margin-top:3px;">单笔支出金额在' + $("#txtjine_fczzxzj").val() + '元' + $("#dlljine_fczzxzjT").combobox('getValue') + '的，由</span>' +
                 '    <div zcmoney="' + $("#txtjine_fczzxzj").val() + '" zctype="' + $("#dlljine_fczzxzjT").combobox('getValue') + '" id="div_fczzxzj' + tr_fczzjzf_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_fczzxzj' + tr_fczzjzf_count + '_0" name="divfczzxzj" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_fczzxzj").append(trHTML);
    splc("div_fczzxzj" + tr_fczzjzf_count + "_0");
}

//删除步骤
function delfczzjzf(fczzjzftrid) {
    if ($("#table_fczzxzj tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_fczzxzj tr").length;
    if (tr_fczzjzf_count == 0) {
        tr_fczzjzf_count = rowcount;
    }
    $("#" + fczzjzftrid).remove();
    //$("#table_fczzxzj tr:last").remove();
}

//添加审核审批人
function addfczzjzfs(divid) {
    var len = $("#" + divid + " input[name='divfczzxzj']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divfczzxzj" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delfczzjzfs(divid) {
    if ($("#" + divid + " input[comboname='divfczzxzj']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divfczzxzj']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
/*非财政专项资金支付审核end*/

//借款
var tr_jksh_count = 0;
function addjksh() {
    if ($("#txtjine_jkshf").val() == "") {
        $.messager.alert("提示信息", "请输入资金支付金额");
        return;
    }
    if ($("#dlljine_jkshT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入资金支付控制范围");
        return;
    }
    var rowcount = $("#table_jksh tr").length;
    if (tr_jksh_count == 0) {
        tr_jksh_count = rowcount;
    }
    else {
        tr_jksh_count++;
    }
    var trHTML = '<tr id="table_tr_jksh' + tr_jksh_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_jksh_DelB" onclick="deljksh(\'table_tr_jksh' + tr_jksh_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_jksh_AddS" onclick="addjkshs(\'div_jksh' + tr_jksh_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_jksh_DelS" onclick="deljkshs(\'div_jksh' + tr_jksh_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="借款金额在" style="float:left;margin-top:3px;">借款金额在' + $("#txtjine_jksh").val() + '元' + $("#dlljine_jkshT").combobox('getValue') + '的，由</span>' +
                 '    <div jkmoney="' + $("#txtjine_jksh").val() + '" jktype="' + $("#dlljine_jkshT").combobox('getValue') + '" id="div_jksh' + tr_jksh_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_jksh' + tr_jksh_count + '_0" name="divjksh" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_jksh").append(trHTML);
    splc("div_jksh" + tr_jksh_count + "_0");
}

//删除步骤
function deljksh(jkshtrid) {
    if ($("#table_jksh tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_jksh tr").length;
    if (tr_jksh_count == 0) {
        tr_jksh_count = rowcount;
    }
    $("#" + jkshtrid).remove();
    //$("#table_jksh tr:last").remove();
}

//添加审核审批人
function addjkshs(divid) {
    var len = $("#" + divid + " input[name='divjksh']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divjksh" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function deljkshs(divid) {
    if ($("#" + divid + " input[comboname='divjksh']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divjksh']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
//借款

//报销
var tr_bxsh_count = 0;
function addbxsh() {

    if ($("#txtjine_bxshf").val() == "") {
        $.messager.alert("提示信息", "请输入资金支付金额");
        return;
    }
    if ($("#dlljine_bxshT").combobox('getValue') == "") {
        $.messager.alert("提示信息", "请输入资金支付控制范围");
        return;
    }
    var rowcount = $("#table_bxsh tr").length; 
    if (tr_bxsh_count == 0) {
        tr_bxsh_count = rowcount;
    }
    else {
        tr_bxsh_count++;
    }
    var trHTML = '<tr id="table_tr_bxsh' + tr_bxsh_count + '">' +
                 '  <td style="border: solid #ccc; border-width: 0px 1px 1px 0px; width:100px;">' +
                 '      <input name="btn_bxsh_DelB" onclick="delbxsh(\'table_tr_bxsh' + tr_bxsh_count + '\')" value="删除步骤" type="button" style="margin-left: 10px;" />' +
                 '  </td>' +
                 '  <td class="con" style="min-width:240px;width:240px">' +
                 '      <input name="btn_bxsh_AddS" onclick="addbxshs(\'div_bxsh' + tr_bxsh_count + '\')" value="增加审核审批人" type="button" />' +
                 '      <input name="btn_bxsh_DelS" onclick="delbxshs(\'div_bxsh' + tr_bxsh_count + '\')" value="删除审核审批人" type="button" />' +
                 '  </td>' +
                 '  <td class="con" colspan="5">' +
                 '    <span class="moneyText" jtext="报销金额在" style="float:left;margin-top:3px;">报销金额在' + $("#txtjine_bxsh").val() + '元' + $("#dlljine_bxshT").combobox('getValue') + '的，由</span>' +
                 '    <div bxmoney="' + $("#txtjine_bxsh").val() + '" bxtype="' + $("#dlljine_bxshT").combobox('getValue') + '" id="div_bxsh' + tr_bxsh_count + '" style="float:left; margin-left:3px; margin-right:3px;">' +
                 '      <input id="div_bxsh' + tr_bxsh_count + '_0" name="divbxsh" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />' +
                 '    </div>' +
                 '    <span style="float:left;margin-top:3px;">审核审批。</span>' +
                 '  </td>' +
                 '</tr>';
    $("#table_bxsh").append(trHTML);
    splc("div_bxsh" + tr_bxsh_count + "_0");
}

//删除步骤
function delbxsh(bxshtrid) {
    if ($("#table_bxsh tr").length <= 0) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    var rowcount = $("#table_bxsh tr").length;
    if (tr_bxsh_count == 0) {
        tr_bxsh_count = rowcount;
    }
    $("#" + bxshtrid).remove();
    //$("#table_bxsh tr:last").remove();
}

//添加审核审批人
function addbxshs(divid) {
    var len = $("#" + divid + " input[name='divbxsh']").length;
    var combHtml = '<span>、</span>' +
        '<input id="' + divid + '_' + len + '" name="divbxsh" type="combobox" style="width: 160px;" class="easyui-combobox" maxlength="30" editable="false" />';
    $("#" + divid).append(combHtml);
    splc(divid + "_" + len);
}

//删除审核审批人
function delbxshs(divid) {
    if ($("#" + divid + " input[comboname='divbxsh']").length <= 1) {
        $.messager.alert("提示信息", "没有可删除项！");
        return;
    }
    $("#" + divid + " input[comboname='divbxsh']").last().remove();
    $("#" + divid + " span.combo").last().remove();
    $("#" + divid + " span").last().remove();
}
//报销