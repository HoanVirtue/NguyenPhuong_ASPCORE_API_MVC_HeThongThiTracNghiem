namespace MultipleChoiceTest.Domain.Models;

public partial class QuestionType : BaseEntity
{
    public string? TypeName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
