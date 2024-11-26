﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamRepository : IRepository<Exam>
    {
        Task<bool> IsExistExamName(string name, int? lessonId, int? id = 0);
        Task<List<ExamItem>> GetAll();

        Task<bool> IsExistCode(string code, int? id = 0);
    }
    public class ExamRepository : GenericRepository<Exam>, IExamRepository
    {
        public ExamRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }
        public Task<bool> IsExistCode(string code, int? id = 0)
        {
            return _dbContext.Exams.AnyAsync(x => x.Code == code && x.Id != id && x.IsDeleted != true);
        }

        public async Task<List<ExamItem>> GetAll()
        {
            var list = await _dbContext.Exams.Include(x => x.Subject).Include(x => x.Lesson).Where(x => x.IsDeleted != true).ToListAsync();
            return _mapper.Map<List<ExamItem>>(list);
        }

        public Task<bool> IsExistExamName(string name, int? lessonId, int? id = 0)
        {
            return _dbContext.Exams.AnyAsync(x => x.ExamName == name && x.LessonId == lessonId && x.Id != id && x.IsDeleted != true);
        }
    }
}
