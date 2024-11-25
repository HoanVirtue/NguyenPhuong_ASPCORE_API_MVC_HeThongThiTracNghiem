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
    public class ExamsController : BaseController
    {
        public ExamsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        // GET: api/Exams
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Exam>>>> GetExams()
        {
            var exams = await _unitOfWork.ExamRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Exam>>
            {
                Success = exams != null && exams.Any(),
                Data = exams,
                Message = exams == null || !exams.Any() ? "không có dữ liệu" : ""
            });
        }

        // GET: api/Exams/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Exam>>> GetExam(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(id);


            return Ok(new ApiResponse<Exam>
            {
                Success = exam != null,
                Data = exam,
                Message = exam == null ? "Không tìm thấy bài thi" : ""
            });
        }

        // PUT: api/Exams/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Exam>>> PutExam([FromBody] CUExam exam)
        {
            if (await _unitOfWork.ExamRepository.IsExistExamName(exam.ExamName, exam.Id))
            {
                return new ApiResponse<Exam>()
                {
                    Success = false,
                    Message = "Tên bài thi đã tồn tại"
                };
            }
            var validateFK = await CheckValidateFK(exam);
            if (!validateFK.Success)
                return validateFK;
            try
            {
                var examUpdate = await _unitOfWork.ExamRepository.GetByIdAsync(exam.Id);
                if (examUpdate == null)
                {
                    return BadRequest(new ApiResponse<Exam>
                    {
                        Success = false
                    });
                }
                examUpdate.ExamName = exam.ExamName;
                examUpdate.Duration = exam.Duration;
                examUpdate.TotalQuestions = exam.TotalQuestions;
                examUpdate.SubjectId = exam.SubjectId;
                examUpdate.LessonId = exam.LessonId;
                await _unitOfWork.ExamRepository.UpdateAsync(examUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExamExists(exam.Id))
                {
                    return NotFound(new ApiResponse<Exam>
                    {
                        Success = false,
                        Message = "Không tìm thấy bài thi"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<Exam>()
            {
                Success = true,
                Data = _mapper.Map<Exam>(exam),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/Exams
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Exam>>> PostExam(CUExam exam)
        {
            if (await _unitOfWork.ExamRepository.IsExistExamName(exam.ExamName))
            {
                return new ApiResponse<Exam>()
                {
                    Success = false,
                    Message = "Tên bài thi đã tồn tại"
                };
            }
            var validateFK = await CheckValidateFK(exam);
            if (!validateFK.Success)
                return validateFK;

            await _unitOfWork.ExamRepository.AddAsync(_mapper.Map<Exam>(exam));

            return CreatedAtAction("GetExam", new { id = exam.Id }, new ApiResponse<Exam>
            {
                Success = true,
                Data = _mapper.Map<Exam>(exam),
                Message = "Thêm dữ liệu thành công"
            });
        }

        // DELETE: api/Exams/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Exam>>> DeleteExam(int id)
        {
            var exam = await _unitOfWork.ExamRepository.GetByIdAsync(id);
            if (exam == null)
            {
                return NotFound(new ApiResponse<Exam>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.ExamRepository.SoftRemoveAsync(exam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<Exam>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> ExamExists(int id)
        {
            return await _unitOfWork.ExamRepository.GetByIdAsync(id) != null;
        }

        private async Task<ApiResponse<Exam>> CheckValidateFK(CUExam question)
        {
            string message = "";
            bool isSuccess = true;
            if (await _unitOfWork.SubjectRepository.GetByIdAsync(question.SubjectId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy môn học", ",");
            }
            if (await _unitOfWork.LessonRepository.GetByIdAsync(question.LessonId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy bài học", ",");
            }
            return new ApiResponse<Exam>
            {
                Success = isSuccess,
                Message = message
            };
        }
    }
}
