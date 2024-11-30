using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUQuestion
    {
        public int Id { get; set; }
        public string? QuestionText { get; set; }

        public string? Choices { get; set; }

        public string? CorrectAnswer { get; set; }

        public string? AnswerExplanation { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn môn học")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn bài học")]
        public int LessonId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn loại câu hỏi")]
        public int QuestionTypeId { get; set; }

        public string? AudioFilePath { get; set; }
        //public IFormFile? AudioFile { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }

    public class ImportQuestionMessage
    {
        public int? Row { get; set; }
        public string? Message { get; set; }
        public ImportQuestionMessage(int? row, string? message)
        {
            Row = row;
            Message = message;
        }
        public ImportQuestionMessage() { }
    }
}
