
$(document).ready(function () {
    var apiAddress = "https://localhost:7047/api/";
    $('.select-subject').on('change', function () {
        var val = $(this).val();
        if (val !== "") {
            loadLesson(val);
        }
    });

    function loadLesson(subjectId) {
        $.ajax({
            type: 'GET',
            url: `${apiAddress}Lessons/GetBySubjectId/${subjectId}`,
            success: function (response) {
                if (response != "") {
                    var lessonDropdown = $('.select-lesson');
                    lessonDropdown.empty();
                    lessonDropdown.append('<option selected disabled>--Chọn bài học--</option>');
                    if (response.data != "") {
                        response.data.forEach(les => {
                            lessonDropdown.append(`<option value="${les.id}">${les.lessonName}</option>`);
                        })
                    }
                } else {
                    console.error("lỗi load lesson");
                }
            },
            error: errorCallback
        });
    }

    function errorCallback() {
        bootbox.alert("Đã có lỗi xảy ra! Vui lòng thử lại sau");
    }
});