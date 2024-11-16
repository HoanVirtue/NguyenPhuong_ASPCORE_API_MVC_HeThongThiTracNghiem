using AutoMapper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IAccessStatisticRepository : IRepository<AccessStatistic>
    {

    }
    public class AccessStatisticRepository : GenericRepository<AccessStatistic>, IAccessStatisticRepository
    {
        public AccessStatisticRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }
    }
}
