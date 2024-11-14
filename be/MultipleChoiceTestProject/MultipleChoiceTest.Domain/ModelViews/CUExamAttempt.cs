namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUExamAttempt
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ExamId { get; set; }
        public int? QuestionId { get; set; }
        public string? Answer { get; set; }

        public bool? IsCorrect { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
