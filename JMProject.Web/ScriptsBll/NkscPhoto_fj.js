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
        server: applicationPath + 'Nksc/NkscFpUpload',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        chunked: true, //开始分片上传
        chunkSize: 2048000, //每一片的大小
        formData: {
            guid: GUID //自定义参数，待会儿解释
        },
        accept: {
            title: '图像文件',
            extensions: 'bmp|dib,gif,png,tif|tiif,jpe|jfif,jpg|jpeg',
            mimeTypes: 'image/bmp,image/gif,image/png,image/tiff,image/jpeg'//
        },
        // 不压缩image, 默认如果是jpeg，文件上传前会压缩一把再上传！
        resize: false
    });
    // 当有文件被添加进队列的时候
    uploader.on('fileQueued', function (file) {
        if (file.size < 1048576) {
            alert("请上传大于等于1MB的封皮图片");
            uploader.cancelFile(file);
        }
    });
    // 文件上传过程中创建进度条实时显示。
    uploader.on('uploadProgress', function (file, percentage) {

    });

    // 文件上传成功，给item添加成功class, 用样式标记上传成功。
    uploader.on('uploadSuccess', function (file, response) {
        //$('#' + file.id).find('p.state').text('已上传');
        $.post('/Nksc/NkscFpMerge', { guid: GUID, fileName: file.name }, function (data) {
            if (data.type == 0) {
                $.messager.alert("提示信息", data.message);
            }
            else {
                var filename = file.name.replace("、", "-");
                var exp = filename;
                $("#thelist li:last").remove();
                var filetext = '<li id="' + file.id + '"><img alt="删除" onclick="deleteFJ(\'' + file.id + '\',\'' + filename + '\')" style="cursor:pointer; position:absolute;right:15px;top:10px;" src="../../image/close.png" /><a href="javascript:;" title="' + filename + '"><img class="fjimg" alt="' + filename + '" src="../../../UploadFengPi/' + data.value + '/' + exp + '" /><span>' + filename + '</span></a></li>';
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

function deleteFJ(id, name) {
    var postData = {
        name: name
    }
    $.ajax({
        type: "post",
        url: "/Nksc/DeleteNkscFp",
        data: postData,
        dataType: "json",
        success: function (data) {
            if (data.type == 1) {
                $("#" + id).remove();
                $.messager.show({ title: '系统提示', msg: '删除成功' });
            }
            else {
                $.messager.alert("提示信息", data.message);
            }
        }
    });
}