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
    public class ExamResultsController : BaseController
    {
        public ExamResultsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }
        // GET: api/Exams
        [HttpGet("GetAll")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamResult>>>> GetExamsRf()
        {
            var exams = await _unitOfWork.ExamResultRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExamResult>>
            {
                Success = exams != null && exams.Any(),
                Data = exams,
                Message = exams == null || !exams.Any() ? "không có dữ liệu" : ""
            });
        }
        // DELETE: api/examresult/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<ExamResult>>> DeleteResult(int id)
        {
            var rf = await _unitOfWork.ExamResultRepository.GetByIdAsync(id);
            if (rf == null)
            {
                return NotFound(new ApiResponse<ExamResult>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.ExamResultRepository.SoftRemoveAsync(rf);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<ExamResult>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<ResultItem>>>> GetRf()
        {
            var rf = await _unitOfWork.ExamResultRepository.GetAll();
            return Ok(new ApiResponse<List<ResultItem>>
            {
                Success = rf != null && rf.Any(),
                Data = rf,
                Message = rf == null || !rf.Any() ? "không có dữ liệu" : ""
            });
        }
    }
}
