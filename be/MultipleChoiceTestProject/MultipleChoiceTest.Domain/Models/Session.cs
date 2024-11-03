namespace MultipleChoiceTest.Domain.Models;

public partial class Session : BaseEntity
{
	public int? UserId { get; set; }

	public DateTime? SessionStart { get; set; }

	public DateTime? SessionEnd { get; set; }

	public bool? IsActive { get; set; }

	public virtual User? User { get; set; }
}
