$(function () {
    reload();
    reloadHk();
    reloadCustomTx();
    reloadCustomLlztTx();
});

function reload() {
    var queryData = {
        userS: $('#hiduserS').val()
    };
    InitGrid(queryData);
    $('#grid').datagrid('clearSelections');
};

function reloadHk() {
    var queryData = {
        userS: $('#hiduserS').val()
    };
    InitGridHk(queryData);
    $('#gridHK').datagrid('clearSelections');
};

function reloadCustomTx() {
    var queryData = {
        userS: $('#hiduserS').val()
    };
    InitGridCustomTx(queryData);
    $('#gridCustomTx').datagrid('clearSelections');
};

function reloadCustomLlztTx() {
    var queryData = {
        userS: $('#hiduserS').val()
    };
    InitGridCustomLlztTx(queryData);
    $('#gridBuyTx').datagrid('clearSelections');
}; 

function InitGrid(queryData) {
    $('#grid').datagrid({
        url: '/Home/GetData_Desktop_Visit',
        title: '未读处理意见',
        width: 750,
        methord: 'post',
        height: 350,
        fitColumns: false,
        idField: 'Id',
        sortName: 'Id',
        sortOrder: 'desc',
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        pagination: true,
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,  //显示合计行
        queryParams: queryData,
        columns: [[
                    { field: 'ck', checkbox: true },   //选择
                    {field: 'Name', title: '客户名称', width: 270 }, //客户名称
                    {field: 'AuditDetails', title: '处理意见', width: 420,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.AuditDetails + ">" + row.AuditDetails + "</span>";
                    }
                }, //处理意见
            ]],
        onLoadSuccess: function (data) {
            $("#RowsCount").html(data.total);
        },
        onDblClickRow: function (index, row) {
            $("#modalwindow").html("<iframe width='100%' height='98%' scrolling='no' frameborder='0' src='/Sale/Create_SaleVisit?Id=" + row.Id + "&LookType=true'></iframe>");
            $("#modalwindow").window({ title: '查看', width: 900, height: 670, iconCls: 'icon-edit' }).window('open');
            $.post("/Sale/Update_AuditState?Id=" + row.Id + "&userS=" + $("#hiduserS").val(), function (data) {
                if (data.type == 1) {
                    reload();
                }
            }, "json");
        }
    });
}

function InitGridHk(queryData) {
    $('#gridHK').datagrid({
        url: '/Home/GetData_Desktop_Invoice',
        title: '回款管理',
        width: 750,
        methord: 'post',
        height: 350,
        fitColumns: false,
        idField: 'Id',
        sortName: 'Id',
        sortOrder: 'desc',
        pagination: true,
        pageSize: 20,
        pageList: [10, 20, 30, 40, 50],
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,
        queryParams: queryData,
        columns: [[
                { field: 'Id', title: '编号', width: 100, halign: 'center' },
                { field: 'CustomName', title: '客户名称', width: 270, halign: 'center' },
                { field: 'Key', title: '收款账户', width: 70, halign: 'center' },
                { field: 'Paymentdate', title: '收款日期', width: 90, halign: 'center' },
                { field: 'Paymentmoney', title: '收款金额', width: 90, align: 'right', halign: 'center' },
                { field: 'Remark', title: '描述', width: 100, halign: 'center' }
            ]]
    });
}

function InitGridCustomTx(queryData) {
    $('#gridCustomTx').datagrid({
        url: '/Home/GetData_Desktop_Date',
        title: '近期要联系的客户',
        width: 750,
        methord: 'post',
        height: 350,
        fitColumns: false,
        idField: 'Id',
        sortName: 'Id',
        sortOrder: 'desc',
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        pagination: true,
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,  //显示合计行
        queryParams: queryData,
        columns: [[
                    { field: 'ck', checkbox: true },   //选择
                    {field: 'Name', title: '客户名称', width: 200 }, //客户名称
                    {field: 'NextTime', title: '下次联络时间', width: 120
                     , formatter: function (value, row, index) {
                         if (row.Name == "合计") {
                             return "";
                         }
                         else {
                             return value + " (" + row.EndDay + "日)";
                         }
                     }
                }, //下次联络时间                    
                    {field: 'DemandTypeName', title: '需求分类名称', width: 240,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.DemandTypeName + ">" + row.DemandTypeName + "</span>";
                    }
                }, //需求分类名称
                    {field: 'ContactDate', title: '联络日期', width: 90 }, //联络日期
                    {field: 'YxName', title: '是否意向', width: 80 }, //是否意向
                    {field: 'Amount', title: '预计成交金额', width: 90 }, //预计成交金额
                    {field: 'Offer', title: '本次报价', width: 100 }, //本次报价
                    {field: 'LlfsName', title: '联络方式', width: 80 }, //联络方式
                    {field: 'ContactDetails', title: '联络详情', width: 100,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.ContactDetails + ">" + row.ContactDetails + "</span>";
                    }
                }, //联络详情
                    {field: 'ContactTarget', title: '下次联络目标', width: 100,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.ContactTarget + ">" + row.ContactTarget + "</span>";
                    }
                }, //下次联络目标
                    {field: 'AuditDetails', title: '处理意见', width: 100,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.AuditDetails + ">" + row.AuditDetails + "</span>";
                    }
                }, //处理意见
            ]]
    });
}

function InitGridCustomLlztTx(queryData) {
    $('#gridBuyTx').datagrid({
        url: '/Home/GetData_Desktop_VisitLlztTx',
        title: '计划购买提醒',
        width: 750,
        methord: 'post',
        height: 350,
        fitColumns: false,
        idField: 'Id',
        sortName: 'Id',
        sortOrder: 'desc',
        pageSize: 20,
        pageList: [15, 20, 30, 40, 50],
        pagination: true,
        striped: true, //奇偶行是否区分
        singleSelect: true, //单选模式
        showFooter: true,  //显示合计行
        queryParams: queryData,
        columns: [[
                    { field: 'ck', checkbox: true },   //选择
                    {field: 'Id', title: '编号', width: 80 }, //客户名称
                    {field: 'Name', title: '客户名称', width: 200 }, //客户名称
                    {field: 'ContactDate', title: '联络日期', width: 90 }, //联络日期
                    {field: 'LlztName', title: '联络状态', width: 80 }, //联络方式
                    {field: 'DemandTypeName', title: '需求分类名称', width: 240,
                    formatter: function (value, row, index) {
                        return "<span title=" + row.DemandTypeName + ">" + row.DemandTypeName + "</span>";
                        }
                    }
            ]]
    });
}