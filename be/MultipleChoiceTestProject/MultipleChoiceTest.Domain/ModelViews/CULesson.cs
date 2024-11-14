namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CULesson
    {
        public int Id { get; set; }
        public string? LessonName { get; set; }
        public int? SubjectId { get; set; }


        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
