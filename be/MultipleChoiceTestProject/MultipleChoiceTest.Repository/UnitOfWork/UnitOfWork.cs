using MultipleChoiceTest.Repository.Repository;

namespace MultipleChoiceTest.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAccessStatisticRepository AccessStatisticRepository { get; }
        IExamAttemptRepository ExamAttemptRepository { get; }
        IExamRepository ExamRepository { get; }
        IExamResultRepository ExamResultRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ILessonRepository LessonRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        ISessionRepository SessionRepository { get; }
        ISubjectRepository SubjectRepository { get; }
        IUserRepository UserRepository { get; }
        IQuestionTypeRepository QuestionTypeRepository { get; }
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MultipleChoiceTestDbContext _context;
        private readonly IAccessStatisticRepository _accessStatisticRepository;
        private readonly IExamAttemptRepository _examAttemptRepository;
        private readonly IExamRepository _examRepository;
        private readonly IExamResultRepository _examResultRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IUserRepository _userRepository;
        private readonly IQuestionTypeRepository _questionTypeRepository;

        public UnitOfWork(MultipleChoiceTestDbContext context,
            IAccessStatisticRepository accessStatisticRepository,
            IExamAttemptRepository examAttemptRepository,
            IExamRepository examRepository,
            IExamResultRepository examResultRepository,
            IFeedbackRepository feedbackRepository,
            ILessonRepository lessonRepository,
            IQuestionRepository questionRepository,
            ISessionRepository sessionRepository,
            ISubjectRepository subjectRepository,
            IUserRepository userRepository,
            IQuestionTypeRepository questionTypeRepository)
        {
            _context = context;
            _accessStatisticRepository = accessStatisticRepository;
            _examAttemptRepository = examAttemptRepository;
            _examRepository = examRepository;
            _examResultRepository = examResultRepository;
            _feedbackRepository = feedbackRepository;
            _lessonRepository = lessonRepository;
            _questionRepository = questionRepository;
            _sessionRepository = sessionRepository;
            _subjectRepository = subjectRepository;
            _userRepository = userRepository;
            _questionTypeRepository = questionTypeRepository;
        }
        public IAccessStatisticRepository AccessStatisticRepository => _accessStatisticRepository;

        public IExamAttemptRepository ExamAttemptRepository => _examAttemptRepository;

        public IExamRepository ExamRepository => _examRepository;

        public IExamResultRepository ExamResultRepository => _examResultRepository;

        public IFeedbackRepository FeedbackRepository => _feedbackRepository;

        public ILessonRepository LessonRepository => _lessonRepository;

        public IQuestionRepository QuestionRepository => _questionRepository;

        public ISessionRepository SessionRepository => _sessionRepository;

        public ISubjectRepository SubjectRepository => _subjectRepository;

        public IUserRepository UserRepository => _userRepository;

        public IQuestionTypeRepository QuestionTypeRepository => _questionTypeRepository;

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
