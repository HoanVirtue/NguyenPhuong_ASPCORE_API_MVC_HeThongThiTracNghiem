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
    public class QuestionsController : BaseController
    {
        public QuestionsController(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Question>>>> GetQuestions()
        {
            var questions = await _unitOfWork.QuestionRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Question>>
            {
                Success = questions != null && questions.Any(),
                Data = questions,
                Message = questions == null || !questions.Any() ? "không có dữ liệu" : ""
            });
        }

        // GET: api/Questions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Question>>> GetQuestion(int id)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<Question>
            {
                Success = question != null,
                Data = question,
                Message = question == null ? "Không tìm thấy câu hỏi" : ""
            });
        }

        //PUT: api/Questions/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Question>>> PutQuestion([FromBody] CUQuestion question)
        {
            var validateFK = await CheckValidateFK(question);
            if (!validateFK.Success)
                return validateFK;
            try
            {
                var questionUpdate = await _unitOfWork.QuestionRepository.GetByIdAsync(question.Id);
                if (questionUpdate == null)
                {
                    return BadRequest(new ApiResponse<Question>
                    {
                        Success = false
                    });
                }
                questionUpdate.QuestionText = question.QuestionText;
                questionUpdate.Choices = question.Choices;
                questionUpdate.CorrectAnswer = question.CorrectAnswer;
                questionUpdate.AnswerExplanation = question.AnswerExplanation;
                questionUpdate.SubjectId = question.SubjectId;
                questionUpdate.LessonId = question.LessonId;
                questionUpdate.QuestionTypeId = question.QuestionTypeId;
                questionUpdate.AudioFilePath = question.AudioFilePath;
                await _unitOfWork.QuestionRepository.UpdateAsync(questionUpdate);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await QuestionExists(question.Id))
                {
                    return NotFound(new ApiResponse<Question>
                    {
                        Success = false,
                        Message = "Không tìm thấy câu hỏi"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<Question>()
            {
                Success = true,
                Data = _mapper.Map<Question>(question),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/Questions
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Question>>> PostQuestion(CUQuestion question)
        {
            var validateFK = await CheckValidateFK(question);
            if (!validateFK.Success)
                return validateFK;

            await _unitOfWork.QuestionRepository.AddAsync(_mapper.Map<Question>(question));

            return CreatedAtAction("GetQuestion", new { id = question.Id }, new ApiResponse<Question>
            {
                Success = true,
                Data = _mapper.Map<Question>(question),
                Message = "Thêm dữ liệu thành công"
            });
        }

        // DELETE: api/Questions/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Question>>> DeleteQuestion(int id)
        {
            var question = await _unitOfWork.QuestionRepository.GetByIdAsync(id);
            if (question == null)
            {
                return NotFound(new ApiResponse<Question>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.QuestionRepository.SoftRemoveAsync(question);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<Question>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> QuestionExists(int id)
        {
            return await _unitOfWork.QuestionRepository.GetByIdAsync(id) != null;
        }

        private async Task<ApiResponse<Question>> CheckValidateFK(CUQuestion question)
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
            if (await _unitOfWork.QuestionTypeRepository.GetByIdAsync(question.QuestionTypeId) == null)
            {
                isSuccess = false;
                message = string.Join("Không tìm thấy loại câu hỏi", ",");
            }
            return new ApiResponse<Question>
            {
                Success = isSuccess,
                Message = message
            };
        }
    }
}
