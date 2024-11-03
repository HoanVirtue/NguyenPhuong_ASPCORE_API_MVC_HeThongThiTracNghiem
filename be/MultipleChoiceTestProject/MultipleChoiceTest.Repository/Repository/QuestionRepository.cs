using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IQuestionRepository : IRepository<Question>
	{

	}
	public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
	{
		public QuestionRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
