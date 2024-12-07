namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CandidateAnswer
    {
        public int QuestionId { get; set; }
        public int QuestionIndex { get; set; }
        public int QuestionTypeId { get; set; }
        public string AnswerText { get; set; }
    }
}
