using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Constants;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;
using System.Linq.Expressions;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<bool> IsExistExamName(string name, int? lessonId, int? id = 0);
        Task<List<ExamItem>> GetAll();

        Task<bool> IsExistCode(string code, int? id = 0);

        Task<Pagination<ExamItem>> GetExamGrid(
            Expression<Func<Exam, bool>> filter = null,
            Func<IQueryable<Exam>, IOrderedQueryable<Exam>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null);

        Task<ExamItem> GetDetail(int id);
        Task<Pagination<ExamItem>> GetExamByLessonGrid(int lessonId, int? pageIndex, int? pageSize);
        Task<Pagination<ExamItem>> GetExamBySubjectGrid(int subjectId, int? pageIndex, int? pageSize);
        Task<List<ExamItem>> GetExamByLesson(int lessonId);
        Task<List<ExamItem>> GetExamBySubject(int subjectId);
        Task<ApiResponse<ExamResultResponse>> ExamFinish(int examId, List<CandidateAnswer> answers);
    }
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        //private readonly IGPTService _gptService;
        private readonly IExamAttemptRepository _attemptRepository;
        private readonly IExamResultRepository _resultRepository;
        public ExamRepository(
            //IGPTService gptService,
            IExamAttemptRepository attemptRepository,
            IExamResultRepository examResultRepository,
            MultipleChoiceTestDbContext dbContext,
            IUserContextService userContextService,
            IMapper mapper) : base(dbContext, userContextService, mapper)
        {
            //_gptService = gptService;
            _attemptRepository = attemptRepository;
            _resultRepository = examResultRepository;
        }
        public Task<bool> IsExistCode(string code, int? id = 0)
        {
            return _dbContext.Exams.AnyAsync(x => x.Code == code && x.Id != id && x.IsDeleted != true);
        }

        public async Task<List<ExamItem>> GetAll()
        {
            var list = await _dbContext.Exams.Include(x => x.Subject).Include(x => x.Lesson).Where(x => x.IsDeleted != true).ToListAsync();
            return _mapper.Map<List<ExamItem>>(list);
        }

        public Task<bool> IsExistExamName(string name, int? lessonId, int? id = 0)
        {
            return _dbContext.Exams.AnyAsync(x => x.ExamName == name && x.LessonId == lessonId && x.Id != id && x.IsDeleted != true);
        }

        public async Task<Pagination<ExamItem>> GetExamGrid(
            Expression<Func<Exam, bool>> filter = null,
            Func<IQueryable<Exam>, IOrderedQueryable<Exam>> orderBy = null,
            string includeProperties = "",
            int? pageIndex = null,
            int? pageSize = null)
        {
            IQueryable<Exam> query = _dbContext.Exams;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var totalItemsCount = await query.CountAsync();

            if (pageIndex.HasValue && pageIndex.Value == -1)
            {
                pageSize = totalItemsCount; // Set pageSize to total count
                pageIndex = 0; // Reset pageIndex to 0
            }
            else if (pageIndex.HasValue && pageSize.HasValue)
            {
                int validPageIndex = pageIndex.Value > 0 ? pageIndex.Value : 0;
                int validPageSize = pageSize.Value > 0 ? pageSize.Value : 10; // Assuming a default pageSize of 10 if an invalid value is passed

                query = query.Skip(validPageIndex * validPageSize).Take(validPageSize);
            }

            var items = await query.ToListAsync();

            return new Pagination<ExamItem>
            {
                TotalItemsCount = totalItemsCount,
                PageSize = pageSize ?? totalItemsCount,
                PageIndex = pageIndex ?? 0,
                Items = _mapper.Map<List<ExamItem>>(items)
            };
        }

        public async Task<ExamItem> GetDetail(int id)
        {
            var exam = await _dbContext.Exams.Include(x => x.Subject).Include(x => x.Lesson).SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);
            return _mapper.Map<ExamItem>(exam);
        }

        public async Task<Pagination<ExamItem>> GetExamByLessonGrid(int lessonId, int? pageIndex, int? pageSize)
        {
            var exams = await GetGridAsync(
                filter: e => e.LessonId == lessonId,
                includeProperties: "Lesson,Subject",
                pageIndex: pageIndex,
                pageSize: pageSize);

            return _mapper.Map<Pagination<ExamItem>>(exams);
        }

        public async Task<Pagination<ExamItem>> GetExamBySubjectGrid(int subjectId, int? pageIndex, int? pageSize)
        {
            var exams = await GetGridAsync(
                filter: e => e.SubjectId == subjectId,
                includeProperties: "Lesson,Subject",
                pageIndex: pageIndex,
                pageSize: pageSize);

            return _mapper.Map<Pagination<ExamItem>>(exams);
        }

        public async Task<List<ExamItem>> GetExamByLesson(int lessonId)
        {
            var exams = await _dbContext.Exams
                .Where(x => x.LessonId == lessonId && x.IsDeleted != true)
                .Include(x => x.Subject)
                .Include(x => x.Lesson)
                .ToListAsync();
            return _mapper.Map<List<ExamItem>>(exams);
        }

        public async Task<List<ExamItem>> GetExamBySubject(int subjectId)
        {
            var exams = await _dbContext.Exams
                .Where(x => x.SubjectId == subjectId && x.IsDeleted != true)
                .Include(x => x.Subject)
                .Include(x => x.Lesson)
                .ToListAsync();
            return _mapper.Map<List<ExamItem>>(exams);
        }

        public async Task<ApiResponse<ExamResultResponse>> ExamFinish(int examId, List<CandidateAnswer> answers)
        {
            // lấy question
            int correct = 0;
            int inCorrect = 0;
            int unanswered = 0;
            var examInfo = await _dbContext.Exams.FindAsync(examId);
            if (examInfo == null)
                return ApiResponse<ExamResultResponse>.ErrorResponse<ExamResultResponse>("Bài thi không tồn tại");
            foreach (var an in answers)
            {
                var question = await _dbContext.Questions.FindAsync(an.QuestionId);
                //if (question == null)
                //    return;
                ExamAttempt result = null;
                if (an.QuestionTypeId == (int)QuestionTypeConstant.Type.MultipleChoice || an.QuestionTypeId == (int)QuestionTypeConstant.Type.Audio)
                {
                    result = new ExamAttempt()
                    {
                        UserId = _userContext.GetCurrentUserId(),
                        ExamId = examId,
                        QuestionId = an.QuestionId,
                        Answer = an.AnswerText,
                        IsCorrect = an.AnswerText == question.CorrectAnswer.Trim(),
                    };
                    an.IsCorrect = result.IsCorrect ?? false;
                }
                else if (an.QuestionTypeId == (int)QuestionTypeConstant.Type.Essay)
                {
                    result = new ExamAttempt()
                    {
                        UserId = _userContext.GetCurrentUserId(),
                        ExamId = examId,
                        QuestionId = an.QuestionId,
                        Answer = an.AnswerText,
                        IsCorrect = false,
                    };

                    //await _gptService.GradeEssay(question.QuestionText, question.AnswerExplanation, an.AnswerText)
                    an.IsCorrect = result.IsCorrect ?? false;
                }

                if (result != null)
                {
                    if (result.IsCorrect == true)
                    {
                        correct++;
                    }
                    else if (!string.IsNullOrEmpty(an.AnswerText) && result.IsCorrect != true)
                    {
                        inCorrect++;
                    }
                    else
                    {
                        unanswered++;
                    }
                    await _attemptRepository.AddAsync(result);
                }
                else
                {
                    return ApiResponse<ExamResultResponse>.ErrorResponse<ExamResultResponse>("Lỗi không tìm thấy loại câu hỏi");
                }
            }

            decimal score = (decimal)(((correct * 1.0) / (examInfo.TotalQuestions * 1.0)) * 10);
            var examResult = new ExamResult()
            {
                ExamId = examId,
                UserId = _userContext.GetCurrentUserId(),
                CompletionTime = DateOnly.FromDateTime(DateTime.Now),
                CorrectAnswersCount = correct,
                IncorrectAnswersCount = inCorrect,
                UnansweredQuestionsCount = unanswered,
                Score = score,
                Rank = score >= 9 ? "Xuất sắc" :
                        score >= 8 ? "Giỏi" :
                        score >= 7 ? "Khá" :
                        score >= 5 ? "Trung bình" :
                        "Kém"
            };
            try
            {
                await _resultRepository.AddAsync(examResult);
                return ApiResponse<ExamResultResponse>.SuccessWithData(new ExamResultResponse()
                {
                    ExamResult = _mapper.Map<ExamResultItem>(examResult),
                    Answers = answers
                });
            }
            catch (
            Exception ex)
            {
                return ApiResponse<ExamResultResponse>.ErrorResponse<ExamResultResponse>(ex.Message);
            }
        }
    }
}
