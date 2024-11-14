using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.UnitOfWork;
using System.Security.Claims;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : BaseController
    {
        public LessonsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }


        // GET: api/Lessons
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Lesson>>>> GetLessons()
        {
            var lessons = await _unitOfWork.LessonRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Lesson>>
            {
                Success = lessons != null && lessons.Any(),
                Data = lessons,
                Message = lessons == null || !lessons.Any() ? "không có dữ liệu" : ""
            });
        }

        // GET: api/Lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Lesson>>> GetLesson(int id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<Lesson>
            {
                Success = lesson != null,
                Data = lesson,
                Message = lesson == null ? "Không tìm thấy bài học" : ""
            });
        }

        // PUT: api/Lessons/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Lesson>>> PutLesson([FromBody] CULesson lesson)
        {
            if (await _unitOfWork.LessonRepository.IsExistLessonName(lesson.LessonName, lesson.Id))
            {
                return new ApiResponse<Lesson>()
                {
                    Success = false,
                    Message = "Tên bài học đã tồn tại"
                };
            }
            if (await _unitOfWork.SubjectRepository.GetByIdAsync(lesson.SubjectId) == null)
            {
                return new ApiResponse<Lesson>()
                {
                    Success = false,
                    Message = "Không tìm thấy môn học"
                };
            }
            try
            {
                var lessonUpdate = await _unitOfWork.LessonRepository.GetByIdAsync(lesson.Id);
                if (lessonUpdate == null)
                {
                    return BadRequest(new ApiResponse<Lesson>
                    {
                        Success = false
                    });
                }
                lessonUpdate.LessonName = lesson.LessonName;
                lessonUpdate.SubjectId = lesson.SubjectId;
                lessonUpdate.UpdatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
                await _unitOfWork.LessonRepository.UpdateAsync(lessonUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await LessonExists(lesson.Id))
                {
                    return NotFound(new ApiResponse<Lesson>
                    {
                        Success = false,
                        Message = "Không tìm thấy bài học"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<Lesson>()
            {
                Success = true,
                Data = _mapper.Map<Lesson>(lesson),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/Lessons
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Lesson>>> PostLesson(CULesson lesson)
        {
            if (await _unitOfWork.LessonRepository.IsExistLessonName(lesson.LessonName))
            {
                return new ApiResponse<Lesson>()
                {
                    Success = false,
                    Message = "Tên bài học đã tồn tại"
                };
            }
            lesson.CreatedDate = DateTime.Now;
            lesson.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
            await _unitOfWork.LessonRepository.AddAsync(_mapper.Map<Lesson>(lesson));

            return CreatedAtAction("GetLesson", new { id = lesson.Id }, new ApiResponse<Lesson>
            {
                Success = true,
                Data = _mapper.Map<Lesson>(lesson),
                Message = "Thêm dữ liệu thành công"
            });
        }

        // DELETE: api/Lessons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Lesson>>> DeleteLesson(int id)
        {
            var lesson = await _unitOfWork.LessonRepository.GetByIdAsync(id);
            if (lesson == null)
            {
                return NotFound(new ApiResponse<Lesson>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unitOfWork.LessonRepository.SoftRemoveAsync(lesson);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<Lesson>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> LessonExists(int id)
        {
            return await _unitOfWork.LessonRepository.GetByIdAsync(id) != null;
        }
    }
}
