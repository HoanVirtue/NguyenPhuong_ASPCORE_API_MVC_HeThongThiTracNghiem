namespace MultipleChoiceTest.Domain.Models;

public partial class Exam : BaseEntity
{

    public string? ExamName { get; set; }

    public int? Duration { get; set; }

    public int? TotalQuestions { get; set; }

    public int? SubjectId { get; set; }

    public int? LessonId { get; set; }

    public string? Code { get; set; }
    public virtual ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();

    public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

    public virtual Lesson? Lesson { get; set; }

    public virtual Subject? Subject { get; set; }
}
