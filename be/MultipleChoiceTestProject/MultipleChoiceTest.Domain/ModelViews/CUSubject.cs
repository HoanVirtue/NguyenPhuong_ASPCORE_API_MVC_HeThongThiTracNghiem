using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUSubject
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên môn không được để trống")]
        public string? SubjectName { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
