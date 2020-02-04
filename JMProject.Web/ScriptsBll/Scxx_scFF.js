var applicationPath = window.applicationPath === "" ? "" : window.applicationPath || "../../";
var GUID = WebUploader.Base.guid(); //一个GUID
var uploader;
$(function () {
    InitWebUpload(); //加载上传控件
});

function InitWebUpload() {
    var $ = jQuery;
    var $list = $('#thelist');
    uploader = WebUploader.create({
        // 选完文件后，是否自动上传。
        auto: true,
        // swf文件路径
        swf: applicationPath + 'webuploader-0.1.5/Uploader.swf',
        // 文件接收服务端。
        server: applicationPath + 'Nksc/Upload_Swf',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        multiple: true,
        chunked: true, //开始分片上传
        chunkSize: 2048000, //每一片的大小
        formData: {
            guid: GUID //自定义参数，待会儿解释
        },
        accept: {
            title: 'Pdf文件',
            extensions: 'pdf',
            mimeTypes: 'application/pdf'//'application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document'
            //title: 'Flash文件',
            //extensions: 'swf',
            //mimeTypes: 'application/x-shockwave-flash'//
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
        $.post('/Sale/Merge_Swf', { guid: GUID, fileName: file.name }, function (data) {
            if (data.type == 0) {
                $.messager.alert("提示信息", data.message);
            }
            else {
                var id = file.id;
                var filename = file.name
                var filetext = '<li id="' + id + '"><a href="javascript:void(0)" title="' + filename + '"><img class="fjimg" alt="' + filename + '" src="../../image/Office.png" /><span style="font-size:12px;">' + filename + '</span></a></li>';
                $list.append(filetext);
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

    //开始上传
    $("#ctlBtn").click(function () {
        uploader.upload();
    });
};