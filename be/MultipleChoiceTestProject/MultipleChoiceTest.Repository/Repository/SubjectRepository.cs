using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<bool> IsExistSubjectName(string subjectName);
    }
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService) : base(dbContext, userContextService)
        {
        }

        public Task<bool> IsExistSubjectName(string subjectName)
        {
            return _dbContext.Subjects.AnyAsync(x => x.SubjectName == subjectName && x.IsDeleted != true);
        }
    }
}
