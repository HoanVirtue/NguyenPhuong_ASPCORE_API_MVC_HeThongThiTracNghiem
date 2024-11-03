using MultipleChoiceTest.Domain.Models;

namespace MultipleChoiceTest.Repository.Repository
{
	public interface ILessonRepository : IRepository<Lesson>
	{

	}
	public class LessonRepository : GenericRepository<Lesson>, ILessonRepository
	{
		public LessonRepository(MultipleChoiceTestDbContext dbContext) : base(dbContext)
		{
		}
	}
}
