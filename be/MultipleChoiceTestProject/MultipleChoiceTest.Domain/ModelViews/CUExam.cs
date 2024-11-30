using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUExam
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên đề thi không được bỏ trống")]
        public string? ExamName { get; set; }

        [Required(ErrorMessage = "Thời gian làm bài không được bỏ trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Thời gian làm bài phải lớn hơn 0")]
        public int? Duration { get; set; }

        [Required(ErrorMessage = "Tổng số câu hỏi không được bỏ trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Tổng số câu hỏi phải lớn hơn 0")]
        public int? TotalQuestions { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        public int? SubjectId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bài học")]
        public int? LessonId { get; set; }
        [Required(ErrorMessage = "Mã bài thi không được bỏ trống")]
        public string? Code { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
