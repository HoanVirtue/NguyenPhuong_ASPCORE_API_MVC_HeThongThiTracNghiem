namespace MultipleChoiceTest.Domain.ModelViews
{
    public class ExamResultResponse
    {
        public ExamResultItem ExamResult { get; set; }
        public List<CandidateAnswer> Answers { get; set; }
    }
}
