using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ISessionRepository : IRepository<Session>
    {
    }
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        public SessionRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService) : base(dbContext, userContextService)
        {

        }

    }
}
