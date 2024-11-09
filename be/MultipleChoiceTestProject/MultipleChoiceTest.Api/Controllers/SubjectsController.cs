using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.UnitOfWork;
using System.Security.Claims;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public SubjectsController(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Subject>>>> GetSubjects()
        {
            var subjects = await _unit.SubjectRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Subject>>
            {
                Success = subjects != null && subjects.Any(),
                Data = subjects,
                Message = subjects == null || !subjects.Any() ? "No Subjects found" : ""
            });
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Subject>>> GetSubject(int id)
        {
            var subject = await _unit.SubjectRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<Subject>
            {
                Success = subject != null,
                Data = subject,
                Message = subject == null ? "Subject not found" : ""
            });
        }

        // PUT: api/Subjects/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Subject>>> PutSubject([FromBody] Subject subject)
        {
            if (await _unit.SubjectRepository.IsExistSubjectName(subject.SubjectName))
            {
                return new ApiResponse<Subject>()
                {
                    Success = false,
                    Message = "Tên môn học đã tồn tại"
                };
            }
            try
            {
                subject.UpdatedDate = DateTime.Now;
                subject.UpdatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
                await _unit.SubjectRepository.UpdateAsync(subject);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubjectExists(subject.Id))
                {
                    return NotFound(new ApiResponse<Subject>
                    {
                        Success = false,
                        Message = "Subject not found"
                    });
                }
                else
                {
                    throw;
                }
            }

            return new ApiResponse<Subject>()
            {
                Success = true,
                Data = subject,
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Subject>>> PostSubject(Subject subject)
        {
            if (await _unit.SubjectRepository.IsExistSubjectName(subject.SubjectName))
            {
                return new ApiResponse<Subject>()
                {
                    Success = false,
                    Message = "Tên môn học đã tồn tại"
                };
            }
            subject.CreatedDate = DateTime.Now;
            subject.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
            await _unit.SubjectRepository.AddAsync(subject);

            return CreatedAtAction("GetSubject", new { id = subject.Id }, new ApiResponse<Subject>
            {
                Success = true,
                Data = subject,
                Message = "Subject created successfully"
            });
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Subject>>> DeleteSubject(int id)
        {
            var subject = await _unit.SubjectRepository.GetByIdAsync(id);
            if (subject == null)
            {
                return NotFound(new ApiResponse<Subject>
                {
                    Success = false,
                    Message = "Không tìm thấy dữ liệu"
                });
            }

            try
            {
                await _unit.SubjectRepository.SoftRemoveAsync(subject);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<Subject>
            {
                Success = true,
                Message = "Xóa dữ liệu thành công"
            });
        }

        private async Task<bool> SubjectExists(int id)
        {
            return await _unit.SubjectRepository.GetByIdAsync(id) != null;
        }
    }
}
