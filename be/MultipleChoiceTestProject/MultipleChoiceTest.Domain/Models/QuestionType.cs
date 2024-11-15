using System;
using System.Collections.Generic;

namespace MultipleChoiceTest.Domain.Models;

public partial class QuestionType
{
    public int Id { get; set; }

    public string? TypeName { get; set; }

    public string? Description { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
}
