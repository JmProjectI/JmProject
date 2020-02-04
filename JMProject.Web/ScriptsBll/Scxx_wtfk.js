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
        server: applicationPath + 'Nksc/Upload_wtfk',
        // 选择文件的按钮。可选。
        // 内部根据当前运行是创建，可能是input元素，也可能是flash.
        pick: '#picker',
        multiple: false,
        chunked: true, //开始分片上传
        chunkSize: 2048000, //每一片的大小
        formData: {
            guid: GUID //自定义参数，待会儿解释
        },
        accept: {
            title: 'Word文件或excel文件',
            extensions: 'doc,docx',
            mimeTypes: 'application/msword,application/vnd.openxmlformats-officedocument.wordprocessingml.document'//
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
        $.post('/Nksc/Merge_wtfk', { guid: GUID, fileName: response.message }, function (data) {
            if (data.type == 0) {
                $.messager.alert("提示信息", data.message);
            }
            else {
                
                var id = data.value;
                var filename = data.message
                var exp = "docx";
                var filetext = '<li id="' + id + '"><img alt="删除" onclick="deleteFJ(\'' + id + '\',\'' + filename + '\')" style="cursor:pointer; position:absolute;right:15px;top:10px;" src="../../image/close.png" /><a href="javascript:;" title="' + filename + '"><img class="fjimg" alt="' + filename + '" src="../../image/' + exp + '.png" /><span style="font-size:12px;">' + filename + '</span></a></li>';
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
        fkid: id,
        name: name
    }
    $.ajax({
        type: "post",
        url: "/Nksc/Delete_wtfk",
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

function saveup() {
    var ts = '<div style="font-size:16px; color:red; ">确认要 [提交] 问题反馈吗? <br /><br />提交后将不能再填写问题反馈。</div>';
    $.messager.confirm('系统提示', ts, function (yes) {
        if (yes) {

            var postDataS = {};

            //异步实现提交信息
            $.post("/Nksc/UpdateWtfkZT", postDataS, function (data) {
                if (data.type == 1) {
                    $.messager.show({ title: '系统提示', msg: '提交成功' });
                    window.location.reload(); 
                }
                else {
                    $.messager.alert("提示信息", data.message);
                }
            });
        }
    });
}