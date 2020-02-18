$(function () {
    setEvent();
    InitWizard();
});

function setHeight() {

    $("#hisNkReport").datalist({ height: $(window).height() - 10 });

    $("#wizard").width($(window).width() - 180);

    $(".stepContainer").height($(window).height() - 68);
    $("div.content").height($(window).height() - 80);
    //$("stepContainer").width($(window).width() - 600);
    $("div.content").width($(window).width() - 200 - 180);
    $("div.actionBar").width($(window).width() - 20 - 180);

    $("#zzdiv").height($(window).height() - 68);
    $("#zzdiv").width($(window).width() - 200);
};

function setEvent() {

    $(window).resize(function () {
        setHeight();
    });
};

function InitWizard() {

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


    //    if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1") {
    //        $('.actionBar a.buttonFinish').hide(); //提交按钮变灰
    //    };
};

var step_num_now = "1";

//当现实该步骤时回调该函数，一般用于当前步骤的初始化
function showStepCallback(stepObj) {
    step_num_now = stepObj.attr('rel');
    if (step_num_now == "9") {
        $("div.actionBar").find("a.buttonsave").remove();
        $("#zzdiv").css('display', 'none');
    }
    else {
        //        if ($("#HiddenFlag").val() == "" || $("#HiddenFlag").val() == "1" || $("#HiddenManager").val() == "1") {
        if ($("div.actionBar").find("a.buttonsave").length == 0) {
            $("div.actionBar").append('<a id="SBC" href="#" onclick="SaveStep();" class="buttonsave">保存</a>');
        }
        //            $("#zzdiv").css('display', 'none');
        //        }
        //        else {
        //            $("#zzdiv").css('display', 'block');
        //        }

        if (step_num_now == "1") {
            setHeight();
            Bind_MJLSGX();
        }
        //        else if (step_num_now == "7") {
        //            uploader.refresh(); //刷新上传控件
        //        }
        //        else if (step_num_now == "8") {
        //            $('textarea[autoHeight]').autoHeight();
        //        }

    }
};

function nextStepCallback(stepObj, curstep) {
    if (curstep.toStep < curstep.fromStep) {
        return true;
    }
    var step_num = stepObj.attr('rel');
    switch (step_num) {
        case '1':
            //            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
            //                return true;
            //            }
            //            else {
            //                Bind_KS();
            //                return true;
            //            }
            return true;
            break;
        case '2':
            //            if ($("#HiddenFlag").val() != "" && $("#HiddenFlag").val() != "1" && $("#HiddenManager").val() == "0") {
            //                return true;
            //            }
            //            else {
            //                Bind_FZ();
            //                return true;
            //            }
            return true;
            break;
        default:
            break;
    }
};

function FinishCallback(stepObj) {
    //$("#zzdiv").show();
    //SaveStepEnd();
};

function Bind_MJLSGX() {
    //隶属关系
    $('#cboLSGX').combotree({
        url: "/NkReport/GetTree_MJLSGX?tName=1",
        panelHeight: 400,
        panelWidth: 350,
        onSelect: function (node) {
        }
    });

    //单位所在地区
    $('#cboDWSZDQ').combotree({
        url: "/NkReport/GetTree_MJLSGX?tName=2",
        panelHeight: 400,
        panelWidth: 350,
        onSelect: function (node) {
            //返回树对象  
            var tree = $(this).tree;
            //选中的节点是否为叶子节点,如果不是叶子节点,清除选中  
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                //清除选中  
                $('#cboDWSZDQ').combotree('clear');
            }
        }
    });
    
    //支出功能分类
    $('#cboZCGNFL').combotree({
        url: "/NkReport/GetTree_MJLSGX?tName=3",
        panelHeight: 400,
        panelWidth: 350,
        onSelect: function (node) {
            //返回树对象  
            var tree = $(this).tree;
            //选中的节点是否为叶子节点,如果不是叶子节点,清除选中  
            var isLeaf = tree('isLeaf', node.target);
            if (!isLeaf) {
                //清除选中  
                $('#cboZCGNFL').combotree('clear');
            }
        }
    });

    //归属部门
    $('#cboBMBSDM').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=1',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 400,
        panelWidth:350,
        onLoadSuccess: function (node, data) {

        }
    });

    //单位预算管理级次
    $('#cboDWYSJC').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=2',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 300,
        onLoadSuccess: function (node, data) {

        }
    });

    //单位基本性质
    $('#cboDWJBXZ').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=3',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 200,
        onLoadSuccess: function (node, data) {

        }
    });

    //预算管理级次
    $('#cboYSGLJC').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=4',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 200,
        onLoadSuccess: function (node, data) {

        }
    });

    //内部控制体系建设的开展进度
    $('#cboJSJD').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=5',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 200,
        onLoadSuccess: function (node, data) {

        }
    });

    //内部控制适用的管理业务领域
    $('#cboSYLY').combobox({
        url: '/NkReport/GetTree_MJBMBS?tName=6',
        valueField: 'Id',
        textField: 'Name',
        panelHeight: 200,
        multiple: true, 
        separator: ';',
        onLoadSuccess: function (node, data) {

        }
    });

};
