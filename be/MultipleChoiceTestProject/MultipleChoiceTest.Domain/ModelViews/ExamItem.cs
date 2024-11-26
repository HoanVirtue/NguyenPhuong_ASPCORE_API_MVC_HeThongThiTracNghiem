namespace MultipleChoiceTest.Domain.ModelViews
{
    public class ExamItem
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? ExamName { get; set; }

        public int? Duration { get; set; }

        public int? TotalQuestions { get; set; }
        public string? LessonId { get; set; }
        public string? LessonName { get; set; }
        public int? SubjectId { get; set; }
        public string? SubjectName { get; set; }

        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
