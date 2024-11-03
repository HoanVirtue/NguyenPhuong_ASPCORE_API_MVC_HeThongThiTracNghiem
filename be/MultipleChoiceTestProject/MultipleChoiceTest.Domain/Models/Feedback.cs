namespace MultipleChoiceTest.Domain.Models;

public partial class Feedback : BaseEntity
{

	public string? Title { get; set; }

	public string? FullName { get; set; }

	public string? Email { get; set; }

	public string? Phone { get; set; }

	public string? Address { get; set; }

	public string? Content { get; set; }


}
