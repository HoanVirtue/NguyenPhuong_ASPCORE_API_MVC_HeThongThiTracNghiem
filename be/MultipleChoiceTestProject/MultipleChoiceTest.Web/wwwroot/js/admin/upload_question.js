$(document).ready(function () {
    var input = $('.form-upload').find('input[type="file"]');

    $('.btn-upload-question').on('click', function () {
        $(input).click();
    });

    $(input).on('change', function () {
        let files = $(this);
        if (files != "" && files.length > 0) {
            $(input).closest('.form-upload').submit();

            //var formData = new FormData();
            //formData.append("file", files[0]);
            //$.ajax({
            //    url: "/Admin/Questions/ImportFromExcel",
            //    data: formData,
            //    processData: false,
            //    contentType: false,
            //    type: "POST",
            //    success: function (data) {
            //        alert("Files Uploaded!");
            //    }, error: function (err) {
            //        console.log('upload file: ', err);
            //    }
            //})
        }
    });
});