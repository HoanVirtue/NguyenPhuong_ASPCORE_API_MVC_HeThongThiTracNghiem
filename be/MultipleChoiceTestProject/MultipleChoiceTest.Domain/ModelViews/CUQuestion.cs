namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUQuestion
    {
        public int Id { get; set; }
        public string QuestionText { get; set; } = null!;

        public string? Choices { get; set; }

        public string? CorrectAnswer { get; set; }

        public string? AnswerExplanation { get; set; }

        public int? SubjectId { get; set; }

        public int? LessonId { get; set; }

        public int? QuestionTypeId { get; set; }

        public string? AudioFilePath { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
