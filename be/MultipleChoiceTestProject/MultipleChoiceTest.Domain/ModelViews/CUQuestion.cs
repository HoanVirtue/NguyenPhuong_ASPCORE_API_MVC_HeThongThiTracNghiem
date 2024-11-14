namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUQuestion
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }
        public string? Choices { get; set; }
        public string? CorrectAnswer { get; set; }
        public int? SubjectId { get; set; }
        public int? LessonId { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
