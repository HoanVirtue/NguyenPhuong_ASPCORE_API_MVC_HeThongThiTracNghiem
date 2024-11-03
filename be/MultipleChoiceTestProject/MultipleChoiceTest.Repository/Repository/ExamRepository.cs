using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface IExamRepository : IRepository<Exam>
	{

	}
	public class ExamRepository : GenericRepository<Exam>, IExamRepository
	{
		public ExamRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
