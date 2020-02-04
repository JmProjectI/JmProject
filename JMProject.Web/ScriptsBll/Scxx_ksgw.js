var ksdata = []; //科室二维表
var kscount = 0; //科室数量

$(function () {

    var trList = $("#table_ksgwzz tbody").children(".kstr");
    kscount = trList.length / 3;
    var ksNum = 1;
    for (var i = 0; i < trList.length; i++) {
        if (i % 3 == 2) {
            var gwNum = 1;
            $('#table_ksgwzr' + ksNum + ' tr').each(function (i) {
                ksdata.push({ zz: ksNum, zr: gwNum });
                gwNum += 1;
            });
            ksNum += 1;
        }
    }

});

function addks() {
    kscount += 1;
    var trHTML = '<tr class="kstr">' +
                 '    <td class="dec">' +
                 '        科室名称' +
                 '    </td>' +
                 '    <td class="con">' +
                 '        <input id="txtksmc' + kscount + '" name="ksmc' + kscount + '" type="text" value="" placeholder="如填写“办公室”" class="easyui-validatebox" style="font-size: 12px;" />' +
                 '        <input id="btnAdd" type="button" onclick="addgw(\'table_ksgwzr' + kscount + '\')" value="添加岗位与责任" /><input id="btnRemove" type="button" onclick="removegw(\'table_ksgwzr' + kscount + '\')" value="删除岗位与责任" />' +
                 '    </td>' +
                 '</tr>' +
                 '<tr class="kstr">' +
                 '    <td class="dec">' +
                 '        科室职能' +
                 '    </td>' +
                 '    <td class="con">' +
                 '        <textarea id="txtkszn' + kscount + '" name="kszn' + kscount + '" rows="3" placeholder="如填写“负责组织协调局机关日常工作”,请详细填写科室职能内容，不能一句话概况" class="easyui-validatebox" style="height: 120px;width: 1018px; font-size: 12px;"></textarea>' +
                 '    </td>' +
                 '</tr>' +
                 '<tr class="kstr" id="tr' + kscount + '">' +
                 '    <td colspan="2">' +
                 '        <table id="table_ksgwzr' + kscount + '" class="tab_form" cellpadding="0" cellspacing="0" style="width: 100%">' +
                 '          <tr>' +
                 '            <td class="dec">' +
                 '                科室岗位（更多的岗位 <br/>请点击“添加岗位与责任”按钮！）' +
                 '           </td>' +
                 '            <td class="con">' +
                 '                <input id="txtksgw_' + kscount + '_1" name="ksgw_' + kscount + '_1" type="text" value="" placeholder="如填写“主任”" class="easyui-validatebox" style="font-size: 12px;" />' +
                 '            </td>' +
                 '            <td class="dec">' +
                 '                岗位责任' +
                 '            </td>' +
                 '            <td class="con">' +
                 '                <textarea id="txtgwzr_' + kscount + '_1" name="gwzr_' + kscount + '_1" rows="3" placeholder="如填写“主持办公室的日常行政工作”,请详细填写每个岗位职责内容，不能一句话概况" class="easyui-validatebox" style="height: 120px;width: 620px; font-size: 12px;"></textarea>' +
                 '            </td>' +
                 '          </tr>' +
                 '        </table>' +
                 '    </td>' +
                 '</tr>';
    $("#table_ksgwzz").append(trHTML);
    ksdata.push({ zz: kscount, zr: 1 });
}

function removeks() {
    if (kscount < 1) {
        alert('没有可删除的行');
        return;
    }
    $("#tr" + kscount).remove();
    $("#table_ksgwzz tr:last").remove();
    $("#table_ksgwzz tr:last").remove();
    for (var i = ksdata.length - 1; i >= 0; i--) {
        if (ksdata[i].zz == kscount) {
            ksdata.splice(i, 1);
        }
    }
    kscount -= 1;
    //arrayObj.pop(); //移除最后一个元素并返回该元素值
    //arrayObj.shift(); //移除最前一个元素并返回该元素值，数组中元素自动前移
    //arrayObj.splice(deletePos, deleteCount); //删除从指定位置deletePos开始的指定数量deleteCount的元素，数组形式返回所移除的元素
}

function addgw(tbname) {
    //var ksc = tbname.substr(tbname.length - 1, 1)
    var ksc = tbname.replace("table_ksgwzr", "")
    //alert(ksc);
    var maxzr = 1;
    for (var i = ksdata.length - 1; i >= 0; i--) {
        if (ksdata[i].zz == ksc) {
            if (ksdata[i].zr > maxzr) {
                maxzr = ksdata[i].zr;
            }
        }
    }
    maxzr += 1;
    var trHTML = '<tr>' +
                 '  <td class="dec">' +
                 '      科室岗位（更多的岗位 <br/>请点击“添加岗位与责任”按钮！）' +
                 ' </td>' +
                 '  <td class="con">' +
                 '      <input id="txtksgw_' + ksc + '_' + maxzr + '" name="ksgw_' + ksc + '_' + maxzr + '" type="text" value="" class="easyui-validatebox" style="font-size: 12px;" />' +
                 '  </td>' +
                 '  <td class="dec">' +
                 '      岗位责任' +
                 '  </td>' +
                 '  <td class="con">' +
                 '      <textarea id="txtgwzr_' + ksc + '_' + maxzr + '" name="gwzr_' + ksc + '_' + maxzr + '" rows="3" class="easyui-validatebox" style="height: 120px;width: 620px; font-size: 12px;"></textarea>' +
                 '  </td>' +
                 '</tr>';

    $("#" + tbname + "").append(trHTML);
    ksdata.push({ zz: ksc, zr: maxzr });
}

function removegw(tbname) {
    //var ksc = tbname.substr(tbname.length - 1, 1)
    var ksc = tbname.replace("table_ksgwzr", "");
    var maxzr = 1;
    for (var i = ksdata.length - 1; i >= 0; i--) {
        if (ksdata[i].zz == ksc) {
            if (ksdata[i].zr > maxzr) {
                maxzr = ksdata[i].zr;
            }
        }
    }

    if (maxzr == 1) {
        alert('没有可删除的行');
        return;
    }

    $("#" + tbname + " tr:last").remove();
    for (var i = ksdata.length - 1; i >= 0; i--) {
        if (ksdata[i].zz == ksc && ksdata[i].zr == maxzr) {
            ksdata.splice(i, 1);
        }
    }
}