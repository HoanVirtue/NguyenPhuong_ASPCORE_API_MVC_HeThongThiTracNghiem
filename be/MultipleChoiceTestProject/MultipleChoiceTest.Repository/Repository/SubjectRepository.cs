using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<bool> IsExistSubjectName(string subjectName);
    }
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
        {
        }

        public Task<bool> IsExistSubjectName(string subjectName)
        {
            return _dbContext.Subjects.AnyAsync(x => x.SubjectName == subjectName && x.IsDeleted != true);
        }
    }
}
