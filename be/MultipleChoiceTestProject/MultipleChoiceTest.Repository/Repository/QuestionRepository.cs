using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IQuestionRepository : IRepository<Question>
    {

    }
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService) : base(dbContext, userContextService)
        {
        }
    }
}
