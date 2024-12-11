$(document).ready(function () {
    var countdown = $('#countdown').data('minute');
    let isExamSubmitted = false;

    //loadQuestion(1);
    countdownTime(parseInt(countdown));

    //$(document).on('click', '.pagination-item-name', function () {
    //    var number = $(this).data('value');
    //    loadQuestion(number);
    //});

    $(document).on('click', '.pagination-item-name', function () {
        var number = $(this).data('value');
        var $parent = $(this).closest('li');
        $('.pagination-active').removeClass('pagination-active');
        $parent.addClass('pagination-active');

        var $question = $('#questionlist').find('#question_' + number);
        $('.question-box').addClass('d-none');
        $($question).removeClass('d-none');
    });

    $(document).on('change', 'input[type="radio"]', function () {
        var number = $(this).attr('name').match(/\d+/)[0];

        $('#question_number_' + number).addClass('item-answed');
    });

    $(document).on('click', 'button[name="submit"]', function () {
        validateData();
    });

    $(document).on('click', '#submit-button', function () {
        sumitExam();
    });

    $('#question_number_1 .pagination-item-name').click();
});


function sumitExam() {
    var examId = $('input[name="exam_id"]').val();
    var answers = [];
    $('input[type="radio"]').each(function () {
        var questionName = $(this).attr('name');
        var questionType = $(this).data('questiontype');
        var questionIndex = questionName.match(/\d+/)[0];
        if (answers.some(a => a.questionIndex == questionIndex)) {
            return;
        }

        var answer = $(`input[name="${questionName}"]:checked`).val();
        if (!answer) {
            answer = "";
        }
        answers.push({
            questionIndex: questionIndex,
            answerText: answer,
            questionTypeId: questionType
        })
    });

    $('.textarea-answer').each(function () {
        var questionName = $(this).attr('name');
        var questionType = $(this).data('questiontype');
        var questionIndex = questionName.match(/\d+/)[0];
        if (answers.some(a => a.questionIndex == questionIndex)) {
            return;
        }

        var answer = $(`textarea[name="${questionName}"]`).val();
        if (!answer) {
            answer = "";
        }
        answers.push({
            questionIndex: questionIndex,
            answerText: answer,
            questionTypeId: questionType
        })
    });


    $.ajax({
        url: '/Exams/SubmitExam',
        type: 'POST',
        contentType: 'application/json',
        dataType: 'json',
        data: JSON.stringify({
            examId: examId,
            answers: answers
        }),
        success: function (res) {
            if (res.success) {
                isExamSubmitted = true;
                window.location.replace("/ExamResults/ExamResult?id=" + res.data.examResult.id);
            } else {
                alert(res.message);
            }
        },
        error: function (err) {
            alert('Đã có lỗi xảy ra! Vui lòng liên hệ trung tâm tư vấn');
        }
    });
}

function validateData() {
    var unansweredQuestion = [];
    $('input[type="radio"]').each(function () {
        var questionName = $(this).attr('name');
        if ($(`input[name="${questionName}"]:checked`).length === 0) {
            var number = questionName.match(/\d+/)[0];
            if (!unansweredQuestion.includes(number)) {
                unansweredQuestion.push(number);
            }
        }
    });
    if (unansweredQuestion.length > 0) {
        $('#test-submit-confirm-msg').text(`Các câu hỏi số ${unansweredQuestion.join(', ')} chưa được trả lời. Bạn chắc chắn muốn nộp bài?`);
        $('#submit-confirm').modal('show');
    } else {
        sumitExam();
    }
}


function countdownTime(minute) {
    var countdownTime = minute * 60;
    var countdownInterval = setInterval(function () {
        var hours = Math.floor(countdownTime / 3600);
        var minutes = Math.floor((countdownTime % 3600) / 60);
        var seconds = countdownTime % 60;

        $('#countdown').find('.jst-hours').text(formatTime(hours) + ":");
        $('#countdown').find('.jst-minutes').text(formatTime(minutes) + ":");
        $('#countdown').find('.jst-seconds').text(formatTime(seconds));

        countdownTime--;

        if (countdownTime < 0) {
            clearInterval(countdownInterval);
            $('#countdown').find('.jst-hours').text("00:");
            $('#countdown').find('.jst-minutes').text("00:");
            $('#countdown').find('.jst-seconds').text("00");
        }
    }, 1000);
}

function formatTime(time) {
    return time < 10 ? "0" + time : time;
}

function loadQuestion(index) {
    $.ajax({
        async: true,
        type: 'GET',
        dataType: 'HTML',
        contentType: 'application/json; charset=utf-8',
        url: '/Exams/UserQuestionAnswer?index=' + index,
        success: function (res) {
            if (res !== "") {
                $('#questionlist').html(res);
            } else {
                alert("Đã có lỗi xảy ra!");
            }
        }, error: function (err) {
            alert(err);
        }
    })
}