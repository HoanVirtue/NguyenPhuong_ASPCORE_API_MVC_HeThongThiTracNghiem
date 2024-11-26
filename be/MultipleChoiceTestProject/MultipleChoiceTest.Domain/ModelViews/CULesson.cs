using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CULesson
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên bài học không được bỏ trống")]
        public string? LessonName { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        public int? SubjectId { get; set; }
        [Required(ErrorMessage = "Mã bài học không được để trống")]
        public string? Code { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
