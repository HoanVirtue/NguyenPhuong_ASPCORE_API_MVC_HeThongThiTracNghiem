using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionTypesController : BaseController
    {
        public QuestionTypesController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        // GET: api/QuestionTypes
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<QuestionType>>>> GeDatatQuestionTypes()
        {
            var questionTypes = await _unitOfWork.QuestionTypeRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<QuestionType>>
            {
                Success = questionTypes != null && questionTypes.Any(),
                Data = questionTypes,
                Message = questionTypes == null || !questionTypes.Any() ? "không có dữ liệu" : ""
            });
        }
    }
}
