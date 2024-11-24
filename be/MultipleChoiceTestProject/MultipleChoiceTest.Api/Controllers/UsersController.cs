using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.UnitOfWork;
using System.Collections.Generic;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }
        // GET: api/Users
        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse<IEnumerable<User>>>> GeData()
        {
            var user = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<User>>
            {
                Success = user != null && user.Any(),
                Data = user,
                Message = user == null || !user.Any() ? "không có dữ liệu" : ""
            });
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<UserItem>>>> GetUsers()
        {
            var lessons = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(new ApiResponse<List<UserItem>>
            {
                Success = lessons != null && lessons.Any(),
                Data = _mapper.Map<List<UserItem>>(lessons),
                Message = lessons == null || !lessons.Any() ? "không có dữ liệu" : ""
            });
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> GetUsers(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<User>
            {
                Success = user != null,
                Data = user,
                Message = user == null ? "Không tìm thấy người dùng" : ""
            });
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<User>>> PostUser(CUUser user)
        {
            if (await _unitOfWork.UserRepository.IsExistAccountName(user.AccountName))
            {
                return new ApiResponse<User>()
                {
                    Success = false,
                    Message = "Tên tài khoản đã tồn tại"
                };
            }
            if(await _unitOfWork.UserRepository.IsExistEmail(user.Email))
            {
                return new ApiResponse<User>()
                {
                    Success = false,
                    Message = "Email này đã tồn tại"
                };
            }

            await _unitOfWork.UserRepository.AddAsync(_mapper.Map<User>(user));

            return Ok(
                new ApiResponse<User>
                {
                    Success = true,
                    Data = _mapper.Map<User>(user),
                    Message = "Thêm dữ liệu thành công"
                });
        }
       
        // PUT: api/Users/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<User>>> PutUsers([FromBody] CUUser user)
        {
            if (await _unitOfWork.UserRepository.IsExistAccountName(user.AccountName ,user.Id))
            {
                return new ApiResponse<User>()
                {
                    Success = false,
                    Message = "Tên tài khoản đã tồn tại"
                };
            }
            if (await _unitOfWork.UserRepository.IsExistEmail(user.Email, user.Id))
            {
                return new ApiResponse<User>()
                {
                    Success = false,
                    Message = "Email này đã tồn tại"
                };
            }
            try
            {
                var userUpdate = await _unitOfWork.UserRepository.GetByIdAsync(user.Id);
                if (userUpdate == null)
                {
                    return BadRequest(new ApiResponse<User>
                    {
                        Success = false
                    });
                }
                userUpdate.AccountName = user.AccountName;
                userUpdate.Email = user.Email;
                userUpdate.Gender = user.Gender;
                userUpdate.DateOfBirth = user.DateOfBirth;
                userUpdate.Phone = user.Phone;
                userUpdate.UserName = user.UserName;
                userUpdate.PasswordHash = user.PasswordHash;
                userUpdate.IsAdmin= user.IsAdmin;
                await _unitOfWork.UserRepository.UpdateAsync(userUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserExits(user.Id))
                {
                    return NotFound(new ApiResponse<User>
                    {
                        Success = false,
                        Message = "Không tìm thấy người dùng"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<User>()
            {
                Success = true,
                Data = _mapper.Map<User>(user),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // DELETE: api/users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<User>>> DeleteUser(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new ApiResponse<User>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.UserRepository.SoftRemoveAsync(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<User>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> UserExits(int id)
        {
            return await _unitOfWork.UserRepository.GetByIdAsync(id) != null;
        }
    }
}
