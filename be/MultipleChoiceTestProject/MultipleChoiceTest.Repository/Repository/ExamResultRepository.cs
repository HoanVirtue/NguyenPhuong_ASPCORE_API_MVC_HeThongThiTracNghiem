using AutoMapper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamResultRepository : IRepository<ExamResult>
    {

    }
    public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }
    }
}
