using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseController
    {
        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var list = await _unitOfWork.UserRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<User>>()
            {
                Success = true,
                Data = list,
                Message = "Get data successfully"
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await _unitOfWork.UserRepository.GetByIdAsync(id);
            return Ok(new ApiResponse<User>()
            {
                Success = result != null,
                Data = result,
                Message = result == null ? "Not found user" : ""
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
    }
}
