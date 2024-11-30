using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<bool> IsExistLessonName(string name, int? subjectId, int? id = 0);
        Task<List<LessonItem>> GetAll();
        Task<List<Lesson>> GetDataBySubjectId(int subjectId);
        Task<bool> IsExistCode(string code, int? id = 0);
        Task<Lesson> GetDataByCode(string code);
    }
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public async Task<Lesson> GetDataByCode(string code)
        {
            return await _dbContext.Lessons.SingleOrDefaultAsync(x => x.Code == code && x.IsDeleted != true);
        }
        public Task<bool> IsExistCode(string code, int? id = 0)
        {
            return _dbContext.Lessons.AnyAsync(x => x.Code == code && x.Id != id && x.IsDeleted != true);
        }
        public async Task<List<LessonItem>> GetAll()
        {
            var list = await _dbContext.Lessons.Include(x => x.Subject).Where(x => x.IsDeleted != true).ToListAsync();
            return _mapper.Map<List<LessonItem>>(list);
        }

        public Task<List<Lesson>> GetDataBySubjectId(int subjectId)
        {
            return _dbContext.Lessons.Where(x => x.SubjectId == subjectId && x.IsDeleted != true).ToListAsync();
        }

        public Task<bool> IsExistLessonName(string name, int? subjectId, int? id = 0)
        {
            return _dbContext.Lessons.AnyAsync(x => x.LessonName == name && x.SubjectId == subjectId && x.Id != id && x.IsDeleted != true);
        }

        //public async Task<IEnumerable<Lesson>> GetAllAsync()
        //{
        //    return await _dbContext.Lessons.Include(x => x.Subject).ToListAsync();
        //}
    }
}
