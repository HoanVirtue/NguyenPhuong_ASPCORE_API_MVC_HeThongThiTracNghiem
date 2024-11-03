using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface ISubjectRepository : IRepository<Subject>
	{

	}
	public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
	{
		public SubjectRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
