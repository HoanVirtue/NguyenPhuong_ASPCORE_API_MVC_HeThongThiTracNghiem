using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<QuestionItem>> GetQuestionList();
        Task<QuestionItem> GetDetail(int id);
        Task<bool> CheckQuantityQuestionInLesson(int? quantity, int? lessonId);
        Task<ApiResponse<List<QuestionItem>>> GetDataOfExam(int examId);
        Task<List<QuestionItem>> GetDataByLessonId(int lessonId);
    }
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public async Task<bool> CheckQuantityQuestionInLesson(int? quantity, int? lessonId)
        {
            var questions = await _dbContext.Questions.Where(x => x.LessonId == lessonId && x.IsDeleted != true).ToListAsync();
            return questions.Count >= quantity;
        }

        public async Task<List<QuestionItem>> GetDataByLessonId(int lessonId)
        {
            var questions = await _dbContext.Questions.Where(x => x.LessonId == lessonId && x.IsDeleted != true).ToListAsync();

            return _mapper.Map<List<QuestionItem>>(questions);
        }

        public async Task<ApiResponse<List<QuestionItem>>> GetDataOfExam(int examId)
        {
            var exam = await _dbContext.Exams.FindAsync(examId);
            var questions = await _dbContext.Questions
                .Include(x => x.Subject)
                .Include(x => x.Lesson)
                .Include(x => x.QuestionType)
                .Where(x => x.LessonId == exam.LessonId && x.IsDeleted != true)
                .OrderBy(x => Guid.NewGuid())
                .Take(exam.TotalQuestions ?? 1)
                .ToListAsync();

            if (questions.Count < exam.TotalQuestions)
            {
                return ApiResponse<List<QuestionItem>>.ErrorResponse<List<QuestionItem>>("Số lượng câu trong ngân hàng câu hỏi không đủ");
            }
            int index = 0;
            var result = _mapper.Map<List<QuestionItem>>(questions, opts =>
            {
                opts.AfterMap((src, dest) =>
                {
                    foreach (var item in dest)
                    {
                        item.Index = ++index;
                    }
                });
            });
            return ApiResponse<List<QuestionItem>>.SuccessWithData(result);
        }

        public async Task<QuestionItem> GetDetail(int id)
        {
            var question = await _dbContext.Questions
                .Include(x => x.QuestionType)
                .Include(x => x.Subject)
                .Include(x => x.Lesson)
                .SingleOrDefaultAsync(x => x.Id == id && x.IsDeleted != true);
            return _mapper.Map<QuestionItem>(question);
        }

        public async Task<IEnumerable<QuestionItem>> GetQuestionList()
        {
            var items = from q in _dbContext.Questions
                        join s in _dbContext.Subjects on q.SubjectId equals s.Id
                        join l in _dbContext.Lessons on q.LessonId equals l.Id
                        join qt in _dbContext.QuestionTypes on q.QuestionTypeId equals qt.Id
                        where q.IsDeleted != true
                        select new QuestionItem()
                        {
                            Id = q.Id,
                            QuestionText = q.QuestionText,
                            Choices = q.Choices,
                            CorrectAnswer = q.CorrectAnswer,
                            AnswerExplanation = q.AnswerExplanation,
                            SubjectId = q.SubjectId,
                            SubjectName = s.SubjectName,
                            LessonId = q.LessonId,
                            LessonName = l.LessonName,
                            QuestionTypeId = q.QuestionTypeId,
                            QuestionTypeName = qt.TypeName,
                            AudioFilePath = q.AudioFilePath,
                            CreatedDate = q.CreatedDate,
                            CreatedBy = q.CreatedBy,
                            UpdatedDate = q.UpdatedDate,
                            UpdatedBy = q.UpdatedBy
                        };
            return await items.ToListAsync();
        }
    }
}
