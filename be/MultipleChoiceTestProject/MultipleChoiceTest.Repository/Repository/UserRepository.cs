using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IUserRepository : IRepository<User>
	{

	}
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		public UserRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
