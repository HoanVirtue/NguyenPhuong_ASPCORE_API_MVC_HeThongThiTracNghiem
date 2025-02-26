﻿using AutoMapper;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IExamAttemptRepository : IRepository<ExamAttempt>
    {

    }
    public class ExamAttemptRepository : GenericRepository<ExamAttempt>, IExamAttemptRepository
    {
        public ExamAttemptRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }
    }
}
