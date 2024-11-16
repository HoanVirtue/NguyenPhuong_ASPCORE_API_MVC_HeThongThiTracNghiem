using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamAttemptsController : BaseController
    {
        public ExamAttemptsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        // GET: api/ExamAttempts
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExamAttempt>>>> GetExamAttempts()
        {
            var attempts = await _unitOfWork.ExamAttemptRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<ExamAttempt>>
            {
                Success = attempts != null && attempts.Any(),
                Data = attempts,
                Message = attempts == null || !attempts.Any() ? "không có dữ liệu" : ""
            });
        }

        // GET: api/ExamAttempts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<ExamAttempt>>> GetExamAttempt(int id)
        {
            var attempt = await _unitOfWork.ExamAttemptRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<ExamAttempt>
            {
                Success = attempt != null,
                Data = attempt,
                Message = attempt == null ? "Không tìm thấy dữ liệu" : ""
            });
        }

        // PUT: api/ExamAttempts/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<ExamAttempt>>> PutExamAttempt([FromBody] CUExamAttempt attempt)
        {
            var validateFK = await CheckValidateFK(attempt);
            if (!validateFK.Success)
                return validateFK;
            try
            {
                var attemptUpdate = await _unitOfWork.ExamAttemptRepository.GetByIdAsync(attempt.Id);
                if (attemptUpdate == null)
                {
                    return BadRequest(new ApiResponse<ExamAttempt>
                    {
                        Success = false
                    });
                }
                attemptUpdate.UserId = attempt.UserId;
                attemptUpdate.ExamId = attempt.ExamId;
                attemptUpdate.QuestionId = attempt.QuestionId;
                attemptUpdate.Answer = attempt.Answer;
                await _unitOfWork.ExamAttemptRepository.UpdateAsync(attemptUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExamAttemptExists(attempt.Id))
                {
                    return NotFound(new ApiResponse<ExamAttempt>
                    {
                        Success = false,
                        Message = "Không tìm thấy dữ liệu"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<ExamAttempt>()
            {
                Success = true,
                Data = _mapper.Map<ExamAttempt>(attempt),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/ExamAttempts
        [HttpPost]
        public async Task<ActionResult<ApiResponse<ExamAttempt>>> PostExamAttempt(CUExamAttempt attempt)
        {
            var validateFK = await CheckValidateFK(attempt);
            if (!validateFK.Success)
                return validateFK;
            await _unitOfWork.ExamAttemptRepository.AddAsync(_mapper.Map<ExamAttempt>(attempt));

            return CreatedAtAction("GetExamAttempt", new { id = attempt.Id }, new ApiResponse<ExamAttempt>
            {
                Success = true,
                Data = _mapper.Map<ExamAttempt>(attempt),
                Message = "Thêm dữ liệu thành công"
            });
        }

        // DELETE: api/ExamAttempts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<ExamAttempt>>> DeleteExamAttempt(int id)
        {
            var attempt = await _unitOfWork.ExamAttemptRepository.GetByIdAsync(id);
            if (attempt == null)
            {
                return NotFound(new ApiResponse<ExamAttempt>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.ExamAttemptRepository.SoftRemoveAsync(attempt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<ExamAttempt>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> ExamAttemptExists(int id)
        {
            return await _unitOfWork.ExamAttemptRepository.GetByIdAsync(id) != null;
        }

        private async Task<ApiResponse<ExamAttempt>> CheckValidateFK(CUExamAttempt attempt)
        {
            string message = "";
            bool isSuccess = true;
            if (await _unitOfWork.UserRepository.GetByIdAsync(attempt.UserId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy người dùng", ",");
            }
            if (await _unitOfWork.ExamRepository.GetByIdAsync(attempt.ExamId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy đề thi", ",");
            }
            if (await _unitOfWork.QuestionRepository.GetByIdAsync(attempt.QuestionId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy câu hỏi", ",");
            }
            return new ApiResponse<ExamAttempt>
            {
                Success = isSuccess,
                Message = message
            };
        }
    }
}
