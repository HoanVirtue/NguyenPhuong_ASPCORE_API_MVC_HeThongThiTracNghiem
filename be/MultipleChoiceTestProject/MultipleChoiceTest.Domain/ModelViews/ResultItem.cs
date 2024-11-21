using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class ResultItem
    {
        public int Id { get; set; }
        public string? ExamId { get; set; }
        public string? ExamName { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public DateOnly? CompletionTime { get; set; }
        public int? CorrectAnswersCount { get; set; }
        public int? IncorrectAnswersCount { get; set; }
        public int? UnansweredQuestionsCount { get; set; }
        public decimal? Score { get; set; }
        public string? Rank { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
