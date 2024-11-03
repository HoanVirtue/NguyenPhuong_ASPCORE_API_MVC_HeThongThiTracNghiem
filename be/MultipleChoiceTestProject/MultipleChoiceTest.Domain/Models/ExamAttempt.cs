namespace MultipleChoiceTest.Domain.Models;

public partial class ExamAttempt : BaseEntity
{

	public int? UserId { get; set; }

	public int? ExamId { get; set; }

	public int? QuestionId { get; set; }

	public string? Answer { get; set; }

	public bool? IsCorrect { get; set; }


	public virtual Exam? Exam { get; set; }

	public virtual Question? Question { get; set; }

	public virtual User? User { get; set; }
}
