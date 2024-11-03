namespace MultipleChoiceTest.Domain.Models;

public partial class Lesson : BaseEntity
{
	public string? LessonName { get; set; }

	public int? SubjectId { get; set; }


	public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

	public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

	public virtual Subject? Subject { get; set; }
}
