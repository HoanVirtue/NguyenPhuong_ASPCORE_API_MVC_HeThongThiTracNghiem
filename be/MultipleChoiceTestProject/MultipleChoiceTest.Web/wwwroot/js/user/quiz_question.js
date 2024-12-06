$(document).ready(function () {
    var countdown = $('#countdown').data('minute');
    
    loadQuestion(1);
    countdownTime(parseInt(countdown));
});

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