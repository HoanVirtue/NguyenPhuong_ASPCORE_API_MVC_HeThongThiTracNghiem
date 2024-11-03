using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface ISessionRepository : IRepository<Session>
	{

	}
	public class SessionRepository : GenericRepository<Session>, ISessionRepository
	{
		public SessionRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
