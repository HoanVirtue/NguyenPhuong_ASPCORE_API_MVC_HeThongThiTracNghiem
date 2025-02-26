﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Authorizations;

namespace MultipleChoiceTest.Repository.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> CheckLogin(Login model);
        Task<ApiResponse<User>> RegisterUser(User model);
        Task<bool> IsExistAccountName(string accountName, int? id = 0);
        Task<bool> IsExistEmail(string email, int? id = 0);
        Task<List<UserItem>> GetByUserID(int id);
    }
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MultipleChoiceTestDbContext dbContext, IUserContextService userContextService, IMapper mapper) : base(dbContext, userContextService, mapper)
        {
        }

        public Task<User> CheckLogin(Login model)
        {
            return _dbContext.Users.SingleOrDefaultAsync(x => x.AccountName == model.AccountName && x.PasswordHash == model.Password && x.IsDeleted != true);
        }

        public async Task<List<UserItem>> GetByUserID(int id)
        {
            var user = await _dbContext.Users.Where(x => x.Id == id && x.IsDeleted != true).ToListAsync();
            return _mapper.Map<List<UserItem>>(user);
        }

        public Task<bool> IsExistAccountName(string accountName, int? id = 0)
        {
            return _dbContext.Users.AnyAsync(x => x.AccountName == accountName && x.Id != id && x.IsDeleted != true);
        }

        public Task<bool> IsExistEmail(string email, int? id = 0)
        {
            return _dbContext.Users.AnyAsync(x => x.Email == email && x.Id != id && x.IsDeleted != true);
        }

        public async Task<ApiResponse<User>> RegisterUser(User model)
        {
            string errorMessage = "";
            if (await IsExistEmail(model.Email))
            {
                errorMessage = errorMessage.JoinUnique("Email đã tồn tại");
            }
            if (await IsExistAccountName(model.AccountName))
            {
                errorMessage = errorMessage.JoinUnique("Tên đăng nhập đã tồn tại");
            }

            if (string.IsNullOrEmpty(errorMessage))
            {
                try
                {
                    var user = _mapper.Map<User>(model);
                    await AddAsync(user);
                    return ApiResponse<User>.SuccessWithData<User>(user);
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                }
            }
            return ApiResponse<User>.ErrorResponse<User>(errorMessage);
        }
        
    }
}
