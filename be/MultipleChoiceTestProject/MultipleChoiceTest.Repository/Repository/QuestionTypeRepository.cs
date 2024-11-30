using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IQuestionTypeRepository : IRepository<QuestionType>
    {
        Task<bool> IsExistQuestionTypeName(string name);
    }
    public class QuestionTypeRepository : GenericRepository<QuestionType>, IQuestionTypeRepository
    {
        public QuestionTypeRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public Task<bool> IsExistQuestionTypeName(string name)
        {
            return _dbContext.QuestionTypes.AnyAsync(x => x.TypeName == name && x.IsDeleted != true);
        }

        
    }
}
