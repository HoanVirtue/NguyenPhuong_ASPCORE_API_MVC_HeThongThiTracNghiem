using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;
using NPOI.OpenXmlFormats.Dml;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<bool> IsExistSubjectName(string name, int? id = 0);
        Task<bool> IsExistCode(string code, int? id = 0);
        Task<Subject> GetDataByCode(string code);
        Task<List<SubjectItem>> GetListAsync();
    }
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public async Task<List<SubjectItem>> GetListAsync()
        {
            var result = await _dbContext.Subjects.Where(x => x.IsDeleted != true).ToListAsync();
            return _mapper.Map<List<SubjectItem>>(result);
        }

        public async Task<Subject> GetDataByCode(string code)
        {
            return await _dbContext.Subjects.SingleOrDefaultAsync(x => x.Code == code && x.IsDeleted != true);
        }

        public Task<bool> IsExistCode(string code, int? id = 0)
        {
            return _dbContext.Subjects.AnyAsync(x => x.Code == code && x.Id != id && x.IsDeleted != true);
        }

        public Task<bool> IsExistSubjectName(string name, int? id = 0)
        {
            return _dbContext.Subjects.AnyAsync(x => x.SubjectName == name && x.Id != id && x.IsDeleted != true);
        }

    }
}
