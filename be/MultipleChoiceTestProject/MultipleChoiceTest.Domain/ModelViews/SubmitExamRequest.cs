namespace MultipleChoiceTest.Domain.ModelViews
{
    public class SubmitExamRequest
    {
        public int ExamId { get; set; }
        public List<CandidateAnswer> Answers { get; set; }
    }
}
