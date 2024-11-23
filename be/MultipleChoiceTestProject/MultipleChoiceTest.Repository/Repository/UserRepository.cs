using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CheckLogin(Login model);
        Task<bool> IsExistAccountName(string accountName, int? id = 0);
        Task<bool> IsExistEmail(string email, int? id = 0);
    }
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public Task<User> CheckLogin(Login model)
        {
            return _dbContext.Users.SingleOrDefaultAsync(x => x.AccountName == model.AccountName && x.PasswordHash == model.Password);
        }

        public Task<bool> IsExistAccountName(string accountName, int? id = 0)
        {
            return _dbContext.Users.AnyAsync(x=>x.AccountName == accountName && x.IsDeleted != true);
        }

        public Task<bool> IsExistEmail(string email, int? id = 0)
        {
            return _dbContext.Users.AnyAsync(x => x.Email == email && x.IsDeleted != true);
        }
    }
}
