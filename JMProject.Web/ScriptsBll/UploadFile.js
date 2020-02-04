var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../../";
var GUID = WebUploader.Base.guid(); //一个GUID

$(function () {
    //加载上传控件
    InitWebUpload();

    $("#btnUpload").click(function () {
        var filename = $("#spanfile").html();
        if (filename.length == 0) {
            $.messager.alert("提示信息", "请选择要导入的Excel文件");
            return;
        }

        var sheetname = $("#ddlSheetName").combobox("getText");
        if (sheetname.length == 0) {
            $.messager.alert("提示信息", "请选择SheetName");
            return;
        }

        $.ajax({
            url: "/Sale/Import",
            type: "Post",
            data: { filename: filename, sheetname: sheetname },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                load("正在导入数据，请稍候...");
                $("span.sucMsg").html('');
                $("span.errMsg").html('');
            },
            complete: function (XMLHttpRequest, textStatus) {
                disLoad();
            },
            success: function (data) {
                if (data.type == 1) {
                    disLoad();
                    $("span.sucMsg").html(data.message);
                    window.parent.reloadTabpage($("#td_wordText").html());
                }
                else {
                    $("span.errMsg").html(data.message);
                }
            }
        });
    });
});

function InitWebUpload() {
    var $ = jQuery;
    var $list = $('#thelist');
    var uploader = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        // swf文件路径
        swf: applicationPath + 'webuploader-0.1.5/Uploader.swf',
        // 文件接收服务端。
        server: applicationPath + 'Sale/UploadXls',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        multiple: false,
        chunked: true, //开始分片上传
        chunkSize: 2048000, //每一片的大小 (B)
        formData: {
            guid: GUID //自定义参数，待会儿解释
        },
        accept: {
            title: 'Excel文件',
            extensions: 'xls,xlsx',
            mimeTypes: 'application/vnd.ms-excel,application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
        },
        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false
    });

    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {

    });

    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {

    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {
        $.post('/Sale/MergeXls', { guid: GUID, fileName: file.name }, function (data) {
            if (data.type == 0) {
                $.messager.alert("提示信息", data.message);
            }
            else {
                var id = file.id;
                var filename = file.name;
                $("#spanfile").html(filename);
                $("#ddlSheetName").combobox({
                    url: '/Sale/GetExcelSheetName?filename=' + filename,
                    valueField: 'id',
                    textField: 'name'
                });

                $.messager.show({ title: '系统提示', msg: '上传成功' });
            }
        });
    });

    // 文件上传失败，显示上传出错。
    uploader.on('uploadError', function (file) {
        //$('#' + file.id).find('p.state').text('上传出错');
        $.messager.alert("提示信息", '上传出错');
    });

    // 完成上传完了，成功或者失败，先删除进度条。
    uploader.on('uploadComplete', function (file) {
        //$('#' + file.id).find('.progress').fadeOut();
    });

    //所有文件上传完毕
    uploader.on("uploadFinished", function () {
        //提交表单
    });

};