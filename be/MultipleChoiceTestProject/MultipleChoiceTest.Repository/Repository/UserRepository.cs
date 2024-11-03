using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IUserRepository : IRepository<User>
	{
		Task<User> CheckLogin(Login model);
	}
	public class UserRepository : GenericRepository<User>, IUserRepository
	{
		public UserRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}

		public Task<User> CheckLogin(Login model)
		{
			return _dbContext.Users.SingleOrDefaultAsync(x => x.AccountName == model.AccountName && x.PasswordHash == model.Password);
		}
	}
}
