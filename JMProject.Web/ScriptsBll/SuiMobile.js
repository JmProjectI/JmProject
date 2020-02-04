$(function () {

    // 加载flag
    var loading = false;
    // 每次加载添加多少条目
    var itemsPerLoad = 10;

    function addItems(number, pageIndex) {

        var postdata = {
            Name: $("#customerName").val()
            , rows: number
            , page: pageIndex
            , sort: 'Cdate'
            , order: 'desc'
        }

        $.ajax({
            url: "/Mobile/GetCustom",
            type: "Post",
            data: postdata,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.total < 0) {
                    window.location.href = '/P';
                }
                else if (data.total > 0) {
                    // 生成新条目的HTML
                    var html = '';
                    $.each(data.rows, function (index, value) {
                        html += '<li>'
                                 + '  <div class="item-content">'
                                 + '    <div class="item-inner">'
                                 + '        <div class="item-title-row">'
                                 + '            <div class="item-title">' + value.Name + '</div>'
                                 + '        </div>'
                                 + '        <div class="item-subtitle">' + value.Lxr + ' ' + value.Address + '</div>'
                                 + '        <div class="item-text">' + value.Phone + ' 合：' + value.ItemMoney + ' 发：' + value.Invoicemoney + ' 到：' + value.Paymentmoney + '</div>'
                                 + '        <div class="card-footer no-border mynew">'
                                 + '            <a href="Tel://' + value.Phone + '" jid="' + value.ID + '" class="link icon icon-phone"></a>'
                                 + '            <a href="#" jid="' + value.ID + '" class="link icon icon-edit create-actions"></a>'
                                 + '        </div>'
                                 + '    </div>'
                                 + '  </div>'
                                 + '</li>';
                    });
                    // 添加新条目
                    $('.infinite-scroll-bottom ul').append(html);
                    //总数
                    if (pageIndex == 1) {
                        $('.infinite-scroll-bottom ul').attr("maxItems", data.total);
                    }
                    //页数
                    $('.infinite-scroll-bottom ul').attr("jmpage", parseInt(pageIndex) + 1);
                }
            }
        });
    };

    function addVisits(number, pageIndex) {

        var postdata = {
            Name: $("#customerName").val()
            , rows: number
            , page: pageIndex
            , sort: 'Id'
            , order: 'desc'
        }

        $.ajax({
            url: "/Mobile/GetVisit",
            type: "Post",
            data: postdata,
            dataType: "json",
            async: false,
            success: function (data) {
                if (data.total < 0) {
                    window.location.href = '/P';
                }
                else if (data.total > 0) {
                    // 生成新条目的HTML
                    var html = '';
                    $.each(data.rows, function (index, value) {
                        var yjtext = '';
                        if (value.ShRen) {
                            if (value.ShztName=="未读") {
                                yjtext = '<span style="color:red;">*</span>';
                            }                            
                        }
                        html += '<li>'
                                 + '  <div class="item-content">'
                                 + '    <div class="item-inner">'
                                 + '        <div class="item-title-row">'
                                 + '            <div class="item-title">' + yjtext + ' ' + value.Name + '</div>'
                                 + '        </div>'
                                 + '        <div class="item-subtitle">' + value.DemandTypeName + '</div>'
                                 + '        <div class="item-text">' + value.YxName + ' 预：' + value.Amount + ' 报：' + value.Offer + ' ' + value.NextTime + '</div>'
                                 + '        <div class="card-footer no-border mynew">'
                        //+ '            <a href="Tel://' + value.Phone + '" jid="' + value.Id + '" class="link icon icon-phone"></a>'
                                 + '            <a href="#" jid="' + value.Id + '" sid="' + value.SaleCustomID + '" class="link icon icon-edit create-actions"></a>'
                                 + '        </div>'
                                 + '    </div>'
                                 + '  </div>'
                                 + '</li>';
                    });
                    // 添加新条目
                    $('.infinite-scroll-bottom ul').append(html);
                    //总数
                    if (pageIndex == 1) {
                        $('.infinite-scroll-bottom ul').attr("maxItems", data.total);
                    }
                    //页数
                    $('.infinite-scroll-bottom ul').attr("jmpage", parseInt(pageIndex) + 1);
                }
            }
        });
    };

    //查询
    $(document).on('click', '.search-customer', function () {
        $('.infinite-scroll-bottom ul').empty();
        $.closePanel("#panel-left-search");
        addItems(itemsPerLoad, 1);
        $.attachInfiniteScroll($('.infinite-scroll'));
        $('.infinite-scroll-preloader').show();
    });

    //查询
    $(document).on('click', '.search-visit', function () {
        $('.infinite-scroll-bottom ul').empty();
        $.closePanel("#panel-left-search");
        addVisits(itemsPerLoad, 1);
        $.attachInfiniteScroll($('.infinite-scroll'));
        $('.infinite-scroll-preloader').show();
    });

    //加载主页
    $(document).on("pageInit", "#page-main", function (e, id, page) {
        //页码
        var pageIndex = $('.infinite-scroll-bottom ul').attr("jmpage");
        //预先加载20条
        addItems(itemsPerLoad, pageIndex);

        // 注册'infinite'事件处理函数
        $(page).on('infinite', '.infinite-scroll-bottom', function () {
            // 如果正在加载，则退出
            if (loading) return;
            // 设置flag
            loading = true;
            // 模拟1s的加载过程
            setTimeout(function () {
                //重置加载flag
                loading = false;
                var lastIndex = $('.infinite-scroll-bottom ul li').length;
                var maxItems = parseInt($('.infinite-scroll-bottom ul').attr("maxItems"));
                if (lastIndex >= maxItems) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($('.infinite-scroll'));
                    // 删除加载提示符
                    $('.infinite-scroll-preloader').hide(); //.remove();
                    $.toast("暂无更多数据");
                    return;
                }
                //页码
                var pageIndex = $('.infinite-scroll-bottom ul').attr("jmpage");
                //添加新条目
                addItems(itemsPerLoad, pageIndex);
                //容器发生改变,如果是js滚动，需要刷新滚动
                $.refreshScroller();
            }, 1000);
        });

        //弹出按钮窗口
        $(page).on('click', '.create-actions', function () {
            var Id = $(this).attr("jid");
            var buttons1 = [
                    {
                        text: '请选择',
                        label: true
                    },
                    {
                        text: '修改客户',
                        onClick: function () {
                            window.location.href = "/Mobile/CreateCustomer?Id=" + Id;
                        }
                    },
                    {
                        text: '创建合同',
                        onClick: function () {
                            window.location.href = "/Mobile/CreateOrder?Id=&CId=" + Id;
                        }
                    },
                    {
                        text: '沟通拜访',
                        bold: true,
                        color: 'danger',
                        onClick: function () {
                            window.location.href = "/Mobile/CreateVisit?Id=&CId=" + Id;
                        }
                    }
                ];
            var buttons2 = [
                    {
                        text: '取消',
                        bg: 'danger'
                    }
                ];
            var groups = [buttons1, buttons2];
            $.actions(groups);
        });
    });

    //加载客户维护页
    $(document).on("pageInit", "#page-customer", function (e, id, page) {

        var Finances = ['省', '市', '县', '区'];
        //加载财政局
        $("#picker_Finance").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择财政局</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances
                }
            ]
        });

        var sf = "吉林省";
        var sj = "吉林省";
        var qy = "吉林省";
        if ($("#hid_id").val().length > 0) {
            sf = $("#picker_Re").attr("sf");
            sj = $("#picker_Re").attr("sj");
            qy = $("#picker_Re").attr("dq");
        };

        $("#picker_Re").cityPicker({
            value: [sf, sj, qy]
        });

        //保存
        $(page).on('click', '.create-customers', function () {

            if ($("#Cdate").val() == "") {
                $.alert("日期不允许为空!");
                return;
            }

            var ywys = ""; //业务员
            var ywynames = ""; //业务员
            $(".chkyyw:checked").each(function (i) {
                ywys += "," + $(this).val();
                ywynames += "," + $(this).attr("jvalue");
            })
            if (ywys == "") {
                $.alert("请勾选业务员!");
                return;
            }
            if ($("#Name").val() == "") {
                $.alert("客户全称不允许为空!");
                return;
            }
            if ($("#Lxr").val() == "") {
                $.alert("联系人不允许为空!");
                return;
            }
            if ($("#Phone").val() == "") {
                $.alert("联系方式不允许为空!");
                return;
            }
            //if ($("#UserName").val() == "") {
            //    $.alert("用户名不允许为空!");
            //    return;
            //}
            //if ($("#UserPwd").val() == "") {
            //    $.alert("密码不允许为空!");
            //    return;
            //}
            if ($("#picker_Finance").val() == "") {
                $.alert("财政局不允许为空!");
                return;
            }
            var postData = {
                ID: $("#hid_id").val()
                  , CDate: $("#Cdate").val()
                  , Ywy: ywys.substr(1)
                  , YwyName: ywynames.substr(1)
                  , Name: $("#Name").val()
                  , Lxr: $("#Lxr").val()
                  , Phone: $("#Phone").val()
                  , UserName: $("#UserName").val()
                  , UserPwd: $("#UserPwd").val()
                  , CzjName: $("#picker_Finance").val()
                  , QyName: $("#picker_Re").val()
                  , Remark: $("textarea[name='Remark']").val()
            };

            $.ajax({
                url: "/Mobile/Create_Customer",
                type: "Post",
                data: postData,
                dataType: "json",
                success: function (data) {
                    if (data.type == 1) {
                        $("#Name").val('');
                        $("#Lxr").val('');
                        $("#Phone").val('');
                        $("#UserName").val('');
                        $("#UserPwd").val('');
                        $("textarea[name='Remark']").val('');
                        $.toast(data.message);
                    }
                    else {
                        $.alert(data.message);
                    }
                }
            });
        });
    });

    //加载创建合同
    $(document).on("pageInit", "#page-createorder", function (e, id, page) {
        var Finances = ['新购', '服务费'];
        //加载财政局
        $("#picker_OrderType").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择合同类别</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances
                }
            ]
        });

        $.ajax({
            url: "/Mobile/Get_FinProductTypeLast",
            type: "Post",
            data: [],
            dataType: "json",
            success: function (data) {
                //加载财政局
                $("#picker_product").picker({
                    toolbarTemplate: '<header class="bar bar-nav">\
                          <button class="button button-link pull-right close-picker">确定</button>\
                          <h1 class="title">请选择合同产品</h1>\
                          </header>',
                    cols: [
                        {
                            textAlign: 'center',
                            values: data
                        }
                    ]
                });
            }
        });

        //保存
        $(page).on('click', '.create-orders', function () {

            if ($("#OrderDate").val() == "") {
                $.alert("日期不允许为空!");
                return;
            }

            var salers = ""; //业务员
            $(".chkSaler:checked").each(function (i) {
                salers += "," + $(this).val();
            })
            if (salers == "") {
                $.alert("请勾选业务员!");
                return;
            }

            if ($("#picker_OrderType").val() == "") {
                $.alert("合同类别不允许为空!");
                return;
            }
            if ($("#picker_product").val() == "") {
                $.alert("合同产品不允许为空!");
                return;
            }
            if ($("#Name").val() == "") {
                $.alert("客户不允许为空!");
                return;
            }
            if ($("#ItemCount").val() == "") {
                $.alert("数量不允许为空!");
                return;
            }
            if ($("#ItemPrice").val() == "") {
                $.alert("单价不允许为空!");
                return;
            }

            var postData = {
                Id: $("#hid_id").val()
              , SaleCustomId: $("#hid_cid").val()
              , OrderDate: $("#OrderDate").val()
              , Saler: salers.substr(1)
              , Name: $("#Name").val()
              , OrderType: $("#picker_OrderType").val()
              , ItemNames: $("#picker_product").val()
              , ItemCount: $("#ItemCount").val()
              , ItemPrice: $("#ItemPrice").val()
              , PresentMoney: $("#PresentMoney").val()
              , OtherMoney: $("#OtherMoney").val()
              , Remark: $("textarea[name='Remake']").val()
            };

            $.ajax({
                url: "/Mobile/Create_SaleOrder",
                type: "Post",
                data: postData,
                dataType: "json",
                success: function (data) {
                    if (data.type == 1) {
                        $("#ItemCount").val('');
                        $("#ItemPrice").val('');
                        $("#PresentMoney").val('');
                        $("#OtherMoney").val('');
                        $("textarea[name='Remake']").val('');
                        $.toast(data.message);
                    }
                    else {
                        $.alert(data.message);
                    }
                }
            });
        });
    });

    //创建沟通拜访
    $(document).on("pageInit", "#page-createvisit", function (e, id, page) {

        $(page).on('click', '#history', function () {
            var postData = {
                Id: $("#hid_cid").val()
            };
            $.ajax({
                url: "/Sale/GetLxrName",
                type: "Post",
                data: postData,
                dataType: "json",
                success: function (data) {
                    var resulttext = '';
                    if (data.rows.length < 1) {
                        resulttext = '无可显示数据';
                    }
                    else {
                        resulttext = '<div class="list-block media-list"><ul>';
                        for (var i = 0; i < data.rows.length; i++) {
                            var html = ' <li>' +
                                       '     <div class="item-inner">' +
                                       '         <div class="item-title-row">' +
                                       '             <div class="item-title" style="margin-left: 0.5em;">' + data.rows[i].Offer + '</div>' +
                                       '             <div class="item-after">' + data.rows[i].ContactDate + '</div>' +
                                       '         </div>' +
                                       '         <div class="item-text" style="margin-left: 0.5em;height:auto;text-align:left;">' + data.rows[i].ContactDetails + '</div>' +
                                       '     </div>' +
                                       ' </li>';
                            resulttext += html;
                        }
                        resulttext += '</ul></div>';
                    }
                    $.modal({
                        title: '历史联络情况',
                        text: resulttext,
                        extraClass: 'kkkk',
                        buttons: [
                            {
                                text: '确定',
                                bold: true
                            }]
                    });
                    //overflow-y: scroll;height: 300px;
                }
            });
        });

        $("#datetime-picker").datetimePicker({
            value: [$("#hid_year").val(), $("#hid_mon").val(), $("#hid_day").val(), $("#hid_h").val(), $("#hid_m").val()]
        });


        var Finances = ['上门', '电话', '邮件'];
        //加载财政局
        $("#picker_Llfs").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择联络方式</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances
                }
            ]
        });

        var Finances1 = ['1个月内计划购买', '3个月内计划购买', '6个月内计划购买', '暂不购买'];
        //加载财政局
        $("#picker_Llzt").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择联络状态</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances1
                }
            ]
        });

        var Finances2 = ['我方对产品/服务，优惠政策进行明确、生动的介绍', '客户提出具体功能性需求', '客户提出商务性需求', '包含1-2-3', '包含1-2', '包含1-3'];
        //加载财政局
        $("#picker_Llqk").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择联络情况</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances2
                }
            ]
        });

        var Finances3 = ['有意向', '无意向'];
        //加载财政局
        $("#picker_yx").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择是否意向</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances3
                }
            ]
        });

        var Finances4 = ['接洽中', '达成销售待签单', '完成签单', '已回款'];
        //加载财政局
        $("#picker_jd").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择进度</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances4
                }
            ]
        });

        var Finances5 = ['新建', '待处理', '转订单'];
        //加载财政局
        $("#picker_zt").picker({
            toolbarTemplate: '<header class="bar bar-nav">\
                              <button class="button button-link pull-right close-picker">确定</button>\
                              <h1 class="title">请选择状态</h1>\
                              </header>',
            cols: [
                {
                    textAlign: 'center',
                    values: Finances5
                }
            ]
        });

        //保存
        $(page).on('click', '.create-visit', function () {

            if ($("#datetime-picker").val() == "") {
                $.alert("联络时间不允许为空!");
                return;
            }

            var xqs = ""; //需求编号
            var xqtexts = ""; //需求内容
            $(".chkxq:checked").each(function (i) {
                xqs += "," + $(this).val();
                xqtexts += "," + $(this).next().html();
            })
            if (xqs == "") {
                $.alert("请勾选需求分类!");
                return;
            }
            if ($("#picker_Llfs").val() == "") {
                $.alert("联络方式不允许为空!");
                return;
            }
            if ($("#picker_Llzt").val() == "") {
                $.alert("联络状态不允许为空!");
                return;
            }
            if ($("#picker_Llqk").val() == "") {
                $.alert("联络情况不允许为空!");
                return;
            }
            if ($("#picker_yx").val() == "") {
                $.alert("是否意向不允许为空!");
                return;
            }
            else if ($("#picker_yx").val() == "有意向") {
                if ($("#txtAmount").val() == "") {
                    $.alert("预计成交金额不允许为空!");
                    return;
                }
            }
            if ($("#picker_jd").val() == "") {
                $.alert("进度不允许为空!");
                return;
            }
            if ($("#txtOffer").val() == "") {
                $.alert("本次报价不允许为空!");
                return;
            }
            if ($("textarea[name='RemakeDetails']").val() == "") {
                $.alert("本次联络情况不允许为空!");
                return;
            }
            if ($("textarea[name='RemakeTarget']").val() == "") {
                $.alert("下次联络目标不允许为空!");
                return;
            }
            if ($("#txtNextTime").val() == "") {
                $.alert("下次联络时间不允许为空!");
                return;
            }
            if ($("#picker_zt").val() == "") {
                $.alert("状态不允许为空!");
                return;
            }
            var postData = {
                Id: $("#hid_id").val()
              , SaleCustomId: $("#hid_cid").val()
              , ContactDate: $("#datetime-picker").val()
              , DemandType: xqs.substr(1)
              , DemandTypeName: xqtexts.substr(1)
              , LlfsName: $("#picker_Llfs").val()
              , LlztName: $("#picker_Llzt").val()
              , LlqkName: $("#picker_Llqk").val()
              , YxName: $("#picker_yx").val()
              , Amount: $("#txtAmount").val()
              , JdName: $("#picker_jd").val()
              , Offer: $("#txtOffer").val()
              , ContactDetails: $("textarea[name='RemakeDetails']").val()
              , ContactTarget: $("textarea[name='RemakeTarget']").val()
              , NextTime: $("#txtNextTime").val()
              , ZtName: $("#picker_zt").val()
            };

            $.ajax({
                url: "/Mobile/Create_SaleVisit",
                type: "Post",
                data: postData,
                dataType: "json",
                success: function (data) {
                    if (data.type == 1) {
                        $("#txtAmount").val('');
                        $("#Offer").val('');
                        $("textarea[name='RemakeDetails']").val('');
                        $("textarea[name='RemakeTarget']").val('');
                        $.toast(data.message);
                    }
                    else {
                        $.alert(data.message);
                    }
                }
            });
        });
    });

    //加载拜访沟通页面
    $(document).on("pageInit", "#page-Visit", function (e, id, page) {
        //页码
        var pageIndex = $('.infinite-scroll-bottom ul').attr("jmpage");
        //预先加载20条
        addVisits(itemsPerLoad, pageIndex);

        // 注册'infinite'事件处理函数
        $(page).on('infinite', '.infinite-scroll-bottom', function () {
            // 如果正在加载，则退出
            if (loading) return;
            // 设置flag 
            loading = true;
            // 模拟1s的加载过程
            setTimeout(function () {
                //重置加载flag
                loading = false;
                var lastIndex = $('.infinite-scroll-bottom ul li').length;
                var maxItems = parseInt($('.infinite-scroll-bottom ul').attr("maxItems"));
                if (lastIndex >= maxItems) {
                    // 加载完毕，则注销无限加载事件，以防不必要的加载
                    $.detachInfiniteScroll($('.infinite-scroll'));
                    // 删除加载提示符
                    $('.infinite-scroll-preloader').hide(); //.remove();
                    $.toast("暂无更多数据");
                    return;
                }
                //页码
                var pageIndex = $('.infinite-scroll-bottom ul').attr("jmpage");
                //添加新条目
                addVisits(itemsPerLoad, pageIndex);
                //容器发生改变,如果是js滚动，需要刷新滚动
                $.refreshScroller();
            }, 1000);
        });

        //弹出按钮窗口
        $(page).on('click', '.create-actions', function () {
            var Id = $(this).attr("jid");
            var CId = $(this).attr("sid");
            var buttons1 = [
                    {
                        text: '请选择',
                        label: true
                    },
                    {
                        text: '创建',
                        onClick: function () {
                            window.location.href = "/Mobile/CreateVisit?Id=&CId=" + CId;
                        }
                    },
                    {
                        text: '修改',
                        onClick: function () {
                            window.location.href = "/Mobile/CreateVisit?Id=" + Id + "&CId=" + CId;
                        }
                    },
                    {
                        text: '查看意见',
                        bold: true,
                        color: 'danger',
                        onClick: function () {
                            $.ajax({
                                url: "/Mobile/GetYiJian",
                                type: "Post",
                                data: { Id: Id },
                                dataType: "text",
                                success: function (data) {
                                    $.modal({
                                        title: '处理意见',
                                        text: data,
                                        buttons: [
                                            {
                                                text: '确定',
                                                bold: true
                                            }]
                                    });
                                }
                            });
                        }
                    }
                ];
            var buttons2 = [
                    {
                        text: '取消',
                        bg: 'danger'
                    }
                ];
            var groups = [buttons1, buttons2];
            $.actions(groups);
        });
    });

    $.init();
});