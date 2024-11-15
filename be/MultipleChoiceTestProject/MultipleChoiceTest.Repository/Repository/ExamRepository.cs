using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<bool> IsExistExamName(string name, int? id = 0);
    }
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService) : base(dbContext, userContextService)
        {
        }

        public Task<bool> IsExistExamName(string name, int? id = 0)
        {
            return _dbContext.Exams.AnyAsync(x => x.ExamName == name && x.Id != id && x.IsDeleted != true);
        }
    }
}
