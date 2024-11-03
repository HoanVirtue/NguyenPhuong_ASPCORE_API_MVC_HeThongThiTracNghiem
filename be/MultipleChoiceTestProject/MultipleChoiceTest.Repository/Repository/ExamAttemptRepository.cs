using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IExamAttemptRepository : IRepository<ExamAttempt>
	{

	}
	public class ExamAttemptRepository : GenericRepository<ExamAttempt>, IExamAttemptRepository
	{
		public ExamAttemptRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
