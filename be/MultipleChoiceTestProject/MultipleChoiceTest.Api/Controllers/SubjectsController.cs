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
    public class SubjectsController : BaseController
    {
        public SubjectsController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Subject>>>> GetSubjects()
        {
            var subjects = await _unitOfWork.SubjectRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Subject>>
            {
                Success = subjects != null && subjects.Any(),
                Data = subjects,
                Message = subjects == null || !subjects.Any() ? "Không có dữ liệu" : ""
            });
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Subject>>> GetSubject(int id)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<Subject>
            {
                Success = subject != null,
                Data = subject,
                Message = subject == null ? "Không tìm thấy môn học" : ""
            });
        }

        // PUT: api/Subjects/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Subject>>> PutSubject([FromBody] CUSubject subject)
        {
            if (await _unitOfWork.SubjectRepository.IsExistSubjectName(subject.SubjectName))
            {
                return new ApiResponse<Subject>()
                {
                    Success = false,
                    Message = "Tên môn học đã tồn tại"
                };
            }
            try
            {
                var subjectUpdate = await _unitOfWork.SubjectRepository.GetByIdAsync(subject.Id);
                subjectUpdate.SubjectName = subject.SubjectName;
                subjectUpdate.UpdatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
                await _unitOfWork.SubjectRepository.UpdateAsync(subjectUpdate);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SubjectExists(subject.Id))
                {
                    return NotFound(new ApiResponse<Subject>
                    {
                        Success = false,
                        Message = "Không tìm thấy môn học"
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
                Data = _mapper.Map<Subject>(subject),
                Message = "Cập nhật dữ liệu thành công"
            };
        }

        // POST: api/Subjects
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Subject>>> PostSubject(CUSubject subject)
        {
            if (await _unitOfWork.SubjectRepository.IsExistSubjectName(subject.SubjectName))
            {
                return new ApiResponse<Subject>()
                {
                    Success = false,
                    Message = "Tên môn học đã tồn tại"
                };
            }

            subject.CreatedBy = User.FindFirst(ClaimTypes.Name)?.Value;
            await _unitOfWork.SubjectRepository.AddAsync(_mapper.Map<Subject>(subject));

            return CreatedAtAction("GetSubject", new { id = subject.Id }, new ApiResponse<Subject>
            {
                Success = true,
                Data = _mapper.Map<Subject>(subject),
                Message = "Thêm dữ liệu thành công"
            });
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<Subject>>> DeleteSubject(int id)
        {
            var subject = await _unitOfWork.SubjectRepository.GetByIdAsync(id);
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
                await _unitOfWork.SubjectRepository.SoftRemoveAsync(subject);
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
            return await _unitOfWork.SubjectRepository.GetByIdAsync(id) != null;
        }
    }
}
