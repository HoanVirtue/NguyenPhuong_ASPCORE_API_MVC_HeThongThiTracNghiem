using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<bool> IsExistSubjectName(string name, int? id = 0);
    }
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        // e thấy ở đây khác gì nào chả khác j

        // nhìn kĩ hơn xem ?, e nhìn vào code ý, mới nhìn hàm thì có gì khá cđâu hàm không viết bằng code à, nhìn lõi, thằng subject nó đang check là với name bằng với name truyền vào, id thì khác với id truyền vào
        // thằng usẻ có thế đâu, id vẫn k IsDeleted cái này là j, isDeled là 1 cái trường nó giả dạng xóa
        // tức là khi mình xóa 1 đối tượng nhưng thực tế nó vẫn tồn tại chỉ là mình không hiển thị nó ra thôi
        public Task<bool> IsExistSubjectName(string name, int? id = 0)
        {
            return _dbContext.Subjects.AnyAsync(x => x.SubjectName == name && x.Id != id && x.IsDeleted != true);
        }
        
    }
}
