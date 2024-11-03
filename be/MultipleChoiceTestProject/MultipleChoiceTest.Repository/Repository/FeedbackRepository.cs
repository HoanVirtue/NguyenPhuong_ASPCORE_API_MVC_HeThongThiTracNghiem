using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IFeedbackRepository : IRepository<Feedback>
	{

	}
	public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
	{
		public FeedbackRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
