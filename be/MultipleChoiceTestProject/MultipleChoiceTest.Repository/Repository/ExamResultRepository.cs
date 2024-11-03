using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IExamResultRepository : IRepository<ExamResult>
	{

	}
	public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
	{
		public ExamResultRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
