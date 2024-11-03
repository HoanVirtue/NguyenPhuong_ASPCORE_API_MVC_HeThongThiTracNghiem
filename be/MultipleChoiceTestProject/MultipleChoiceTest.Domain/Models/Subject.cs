namespace MultipleChoiceTest.Domain.Models;

public partial class Subject : BaseEntity
{
	public string? SubjectName { get; set; }


	public virtual ICollection<Exam> Exams { get; set; } = new List<Exam>();

	public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

	public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
