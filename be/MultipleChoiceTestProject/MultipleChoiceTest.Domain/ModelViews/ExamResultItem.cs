namespace MultipleChoiceTest.Domain.ModelViews
{
    public class ExamResultItem
    {
        public int Id { get; set; }
        public int? ExamId { get; set; }
        public string? ExamName { get; set; }
        public int? UserId { get; set; }

        public DateOnly? CompletionTime { get; set; }

        public int? CorrectAnswersCount { get; set; }

        public int? IncorrectAnswersCount { get; set; }

        public int? UnansweredQuestionsCount { get; set; }

        public decimal? Score { get; set; }

        public string? Rank { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
