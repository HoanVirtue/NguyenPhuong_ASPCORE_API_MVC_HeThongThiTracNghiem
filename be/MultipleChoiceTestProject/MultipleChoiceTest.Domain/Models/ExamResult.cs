namespace MultipleChoiceTest.Domain.Models;

public partial class ExamResult : BaseEntity
{

	public int? ExamId { get; set; }

	public int? UserId { get; set; }

	public DateOnly? CompletionTime { get; set; }

	public int? CorrectAnswersCount { get; set; }

	public int? IncorrectAnswersCount { get; set; }

	public int? UnansweredQuestionsCount { get; set; }

	public decimal? Score { get; set; }

	public string? Rank { get; set; }


	public virtual Exam? Exam { get; set; }

	public virtual User? User { get; set; }
}
