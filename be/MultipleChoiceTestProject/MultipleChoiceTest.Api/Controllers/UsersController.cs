using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unit;
        public UsersController(IUnitOfWork unit)
        {
            _unit = unit;
        }
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var list = await _unit.UserRepository.GetAllAsync();
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
            var result = await _unit.UserRepository.GetByIdAsync(id);
            return Ok(new ApiResponse<User>()
            {
                Success = result != null,
                Data = result,
                Message = result == null ? "Not found user" : ""
            });

        }

        //[HttpPost]
        //public IActionResult Post(User user)
        //{
        //    var result = _unit.UserRepository.GetByIdAsync();
        //}
    }
}
