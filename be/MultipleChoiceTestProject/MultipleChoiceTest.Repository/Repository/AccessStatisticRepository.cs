using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IAccessStatisticRepository : IRepository<AccessStatistic>
	{

	}
	public class AccessStatisticRepository : GenericRepository<AccessStatistic>, IAccessStatisticRepository
	{
		public AccessStatisticRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
