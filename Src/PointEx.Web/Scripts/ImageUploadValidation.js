$('input:file').on('fileloaded', function (event, file, previewId, index, reader) {
    $("#btnSubmit").removeAttr("disabled");
});

$('input:file').on('fileclear', function (event) {
    $("#btnSubmit").removeAttr("disabled");
});

$("input:file").on('fileuploaderror', function (event, data, previewId, index) {
    $("#btnSubmit").attr("disabled", "true");
});

