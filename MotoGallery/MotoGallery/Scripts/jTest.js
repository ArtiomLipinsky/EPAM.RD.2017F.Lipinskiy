$(document).ready(function () {

    var previewId = "#preview";
    var fileId = "#file";
    var submitId = "#submit";
    var messageClass = ".message";

    var fileInfo = {};

    $(fileId).change(function () {
        var file = this.files[0];
        var reader = new FileReader();

        reader.onload = function (event) {
            var fileData = event.target.result;
            $(previewId).attr("src", fileData);
            fileInfo.filename = file.name;
            fileInfo.data = fileData;
        };

        reader.readAsDataURL(file);
    });

    $(submitId).click(function (e) {
        $(submitId).prop('disabled', true);
        $.ajax({
            url: "/Image/AddImageAjax",
            type: 'POST',
            data: {
                fileName: fileInfo.filename,
                data: fileInfo.data
            },
            success: function (data) {
                $(submitId).prop('disabled', false);
                $(previewId).attr("src", "");
                $(fileId).val("");

                $(messageClass).hide();
                $(messageClass).text('Upload completed');
                $(messageClass).show(500);
                $(messageClass).hide(2000);
            }
        });

        e.preventDefault();
    });
});