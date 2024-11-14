using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ILessonRepository : IRepository<Lesson>
    {
        Task<bool> IsExistLessonName(string name, int? id = 0);
    }
    public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
    {
        public LessonRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService) : base(dbContext, userContextService)
        {
        }

        public Task<bool> IsExistLessonName(string name, int? id = 0)
        {
            return _dbContext.Lessons.AnyAsync(x => x.LessonName == name && x.Id != id && x.IsDeleted != true);
        }
    }
}
