namespace MultipleChoiceTest.Domain.Models;

public partial class User : BaseEntity
{
	public string? UserName { get; set; }

	public string? Email { get; set; }

	public string? Gender { get; set; }

	public DateOnly? DateOfBirth { get; set; }

	public string? Phone { get; set; }

	public string? AccountName { get; set; }

	public string? PasswordHash { get; set; }

	public bool? IsAdmin { get; set; }

	public virtual ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();

	public virtual ICollection<ExamResult> ExamResults { get; set; } = new List<ExamResult>();

	public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
}
