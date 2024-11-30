using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
