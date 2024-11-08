using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public ExamsController(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }

        // GET: api/Exams
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Exam>>>> GetExams()
        {
            var exams = await _unit.ExamRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Exam>>
            {
                Success = exams != null && exams.Any(),
                Data = exams,
                Message = exams == null || !exams.Any() ? "No exams found" : ""
            });
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Exam>>> GetExam(int id)
        {
            var exam = await _unit.ExamRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<Exam>
            {
                Success = exam != null,
                Data = exam,
                Message = exam == null ? "Exam not found" : ""
            });
        }

        // PUT: api/Exams/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Exam>>> PutExam(int id, Exam exam)
        {
            if (id != exam.Id)
            {
                return BadRequest(new ApiResponse<Exam>
                {
                    Success = false,
                    Message = "ID mismatch"
                });
            }

            try
            {
                await _unit.ExamRepository.UpdateAsync(exam);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExamExists(id))
                {
                    return NotFound(new ApiResponse<Exam>
                    {
                        Success = false,
                        Message = "Exam not found"
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Exams
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Exam>>> PostExam(Exam exam)
        {
            await _unit.ExamRepository.AddAsync(exam);

            return CreatedAtAction("GetExam", new { id = exam.Id }, new ApiResponse<Exam>
            {
                Success = true,
                Data = exam,
                Message = "Exam created successfully"
            });
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Exam>>> DeleteExam(int id)
        {
            var exam = await _unit.ExamRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound(new ApiResponse<Exam>
                {
                    Success = false,
                    Message = "Exam not found"
                });
            }

            try
            {
                await _unit.ExamRepository.SoftRemoveAsync(exam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<Exam>
            {
                Success = true,
                Message = "Exam deleted successfully"
            });
        }

        private async Task<bool> ExamExists(int id)
        {
            return await _unit.ExamRepository.GetByIdAsync(id) != null;
        }
    }
}
