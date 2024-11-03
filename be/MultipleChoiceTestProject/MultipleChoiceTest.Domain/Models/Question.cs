namespace MultipleChoiceTest.Domain.Models;

public partial class Question : BaseEntity
{

	public string? QuestionText { get; set; }

	public string? Choices { get; set; }

	public string? CorrectAnswer { get; set; }

	public int? SubjectId { get; set; }

	public int? LessonId { get; set; }

	public virtual ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();

	public virtual Lesson? Lesson { get; set; }

	public virtual Subject? Subject { get; set; }
}
