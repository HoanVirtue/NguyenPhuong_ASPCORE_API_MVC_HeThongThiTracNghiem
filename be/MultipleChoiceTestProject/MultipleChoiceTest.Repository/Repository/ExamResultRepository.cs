using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamResultRepository : IRepository<ExamResult>
    {
        Task<List<ResultItem>> GetAll();
        Task<List<ExamResultItem>> GetDataByUserId(int userId);
    }
    public class ExamResultRepository : GenericRepository<ExamResult>, IExamResultRepository
    {
        public ExamResultRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }
        public async Task<List<ResultItem>> GetAll()
        {
            var list = await _dbContext.ExamResults.Include(x => x.User)
                                                    .Include(x => x.Exam).Where(x => x.IsDeleted != true)
                                                    .ToListAsync();
            return _mapper.Map<List<ResultItem>>(list);
        }

        public async Task<List<ExamResultItem>> GetDataByUserId(int userId)
        {
            var examResults = await _dbContext.ExamResults
                .Include(x => x.Exam)
                .Where(x => x.UserId == userId && x.IsDeleted != true)
            .ToListAsync();

            return _mapper.Map<List<ExamResultItem>>(examResults);
        }
    }
}
