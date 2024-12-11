let tabSwitchCount = 0;
$(document).ready(function () {
    $(document).on('visibilitychange', function () {
        if (document.visibilityState === 'hidden') {
            tabSwitchCount++;
            alert('Nếu bạn rời trang sẽ nộp bài ngay lập tức');
            if (tabSwitchCount >= 2) {
                alert('Bạn đã rời khỏi tab quá nhiều lần. Bài thi sẽ được nộp!');
                sumitExam();
            }
        }
    });

    $(document).on('contextmenu', function (e) {
        e.preventDefault();
        alert('Không được mở menu chuột phải!');
    });

    $(document).on('keydown keyup', function (e) {
        if ((e.ctrlKey && (e.key === 't' || e.key === 'n'))) {
            e.preventDefault();
            alert('Không được mở tab hoặc cửa sổ mới, bài thi sẽ được nộp ngay lập tức!');
            sumitExam();
        }

        if (e.ctrlKey && e.shiftKey && e.key === 'N') {
            e.preventDefault();
            alert('Không được mở cửa sổ ẩn danh, bài thi sẽ được nộp ngay lập tức!');
            sumitExam();
        }

        if ((e.ctrlKey && e.key === 'r') || e.key === 'F5' || (e.ctrlKey && e.shiftKey && e.key === 'r')) {
            e.preventDefault();
            alert('Không được làm mới trang, bài thi sẽ được nộp ngay lập tức!');
            sumitExam();
        }
    });

    $(window).on('beforeunload', function (e) {
        if (isExamSubmitted) {
            return undefined;
        }

        var confirmationMessage = 'Bạn có chắc chắn muốn rời khỏi bài thi?';
        (e || window.event).returnValue = confirmationMessage;
        return confirmationMessage;
    });
});
