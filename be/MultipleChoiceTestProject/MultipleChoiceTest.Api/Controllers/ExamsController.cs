﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Constants.Api;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Domain.ModelViews;
using MultipleChoiceTest.Repository.Service;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamsController : BaseController
    {
        private readonly IGPTService _gptService;
        public ExamsController(
            IGPTService gptService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration) : base(unitOfWork, mapper, configuration)
        {
            _gptService = gptService;
        }

        //[HttpGet("gpt")]
        //public async Task<IActionResult> TestAsync()
        //{
        //    string apiKey = "";
        //    return Ok(await _gptService.GradeEssay("điện lực là gì", "Điện lực hay điện năng là năng lượng cung cấp bởi dòng điện. Cụ thể, nó là công cơ học thực hiện bởi điện trường lên các điện tích di chuyển trong nó. Năng lượng được sinh ra bởi dòng điện trong một đơn vị thời gian được gọi là công suất điện.", "không biết"));
        //    //return Ok("test");
        //}
        // GET: api/Exams
        [HttpGet("GetAll")]
        //[HttpGet]
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
        public async Task<IActionResult> GetExam(int id, int? type = (int)TypeGetSelectConstant.TypeGetDetail.Get)
        {
            if (type == (int)TypeGetSelectConstant.TypeGetDetail.Get)
            {
                var exam = await _unitOfWork.ExamRepository.GetByIdAsync(id);
                return Ok(new ApiResponse<Exam>
                {
                    Success = exam != null,
                    Data = exam,
                    Message = exam == null ? "Không tìm thấy bài thi" : ""
                });
            }
            else
            {
                var exam = await _unitOfWork.ExamRepository.GetDetail(id);
                return Ok(new ApiResponse<ExamItem>
                {
                    Success = exam != null,
                    Data = exam,
                    Message = exam == null ? "Không tìm thấy bài thi" : ""
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetExam(int? type = (int)TypeGetSelectConstant.TypeGet.GetList, int? pageIndex = 0, int? pageSize = 10)
        {
            if (type == (int)TypeGetSelectConstant.TypeGet.GetList)
            {
                var exam = await _unitOfWork.ExamRepository.GetAll();
                return Ok(new ApiResponse<List<ExamItem>>
                {
                    Success = exam != null && exam.Any(),
                    Data = exam,
                    Message = exam == null || !exam.Any() ? "không có dữ liệu" : ""
                });
            }
            else if (type == (int)TypeGetSelectConstant.TypeGet.GetGrid)
            {
                var examRes = await _unitOfWork.ExamRepository.GetExamGrid(
                    pageIndex: pageIndex,
                    pageSize: pageSize);
                return Ok(new ApiResponse<Pagination<ExamItem>>
                {
                    Success = examRes.Items != null && examRes.Items.Any(),
                    Data = examRes,
                    Message = examRes.Items == null || !examRes.Items.Any() ? "không có dữ liệu" : ""
                });
            }
            var exams = await _unitOfWork.ExamRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<Exam>>
            {
                Success = exams != null && exams.Any(),
                Data = exams,
                Message = exams == null || !exams.Any() ? "không có dữ liệu" : ""
            });
        }

        [HttpGet("GetExamByLesson/{lessonId}")]
        public async Task<IActionResult> GetExamByLessonAsync(int lessonId, int? type = (int)TypeGetSelectConstant.TypeGet.GetList, int? pageIndex = 0, int? pageSize = 10)
        {
            if (type == (int)TypeGetSelectConstant.TypeGet.GetList)
            {
                return Ok(ApiResponse<List<ExamItem>>.SuccessWithData(await _unitOfWork.ExamRepository.GetExamByLesson(lessonId)));
            }
            else if (type == (int)TypeGetSelectConstant.TypeGet.GetGrid)
            {
                var exams = await _unitOfWork.ExamRepository.GetExamByLessonGrid(lessonId, pageIndex, pageSize);
                return Ok(ApiResponse<Pagination<ExamItem>>.SuccessWithData(exams));
            }
            return BadRequest();
        }

        [HttpGet("GetExamBySubject/{subjectId}")]
        public async Task<IActionResult> GetExamBySubjectAsync(int subjectId, int? type = (int)TypeGetSelectConstant.TypeGet.GetList, int? pageIndex = 0, int? pageSize = 10)
        {
            if (type == (int)TypeGetSelectConstant.TypeGet.GetList)
            {
                return Ok(ApiResponse<List<ExamItem>>.SuccessWithData(await _unitOfWork.ExamRepository.GetExamBySubject(subjectId)));
            }
            else if (type == (int)TypeGetSelectConstant.TypeGet.GetGrid)
            {
                var exams = await _unitOfWork.ExamRepository.GetExamBySubjectGrid(subjectId, pageIndex, pageSize);
                return Ok(ApiResponse<Pagination<ExamItem>>.SuccessWithData(exams));
            }
            return BadRequest();
        }

        [HttpGet("GetQuestionByExam/{examId}")]
        public async Task<IActionResult> GetQuestionsByExam(int examId)
        {
            if (await _unitOfWork.ExamRepository.GetByIdAsync(examId) == null)
            {
                return Ok(ApiResponse<List<QuestionItem>>.ErrorResponse<List<QuestionItem>>("Bài thi không tồn tại"));
            }
            var questionRs = await _unitOfWork.QuestionRepository.GetDataOfExam(examId);
            return Ok(questionRs);
        }

        [HttpPost("SubmitExam/{examId}")]
        public async Task<IActionResult> SubmitExamAsync(int examId, [FromBody] List<CandidateAnswer> answers)
        {
            if (examId == 0 || examId == null)
            {
                return Ok(ApiResponse<ExamResultResponse>.ErrorResponse<ExamResultResponse>("Mã bài thi không được bỏ trống"));
            }
            if (answers == null || !answers.Any())
            {
                return Ok(ApiResponse<ExamResultResponse>.ErrorResponse<ExamResultResponse>("Không có câu trả lời"));
            }

            var examResult = await _unitOfWork.ExamRepository.ExamFinish(examId, answers);
            return Ok(examResult);
        }

        // PUT: api/Exams/5
        [HttpPut]
        public async Task<ActionResult<ApiResponse<Exam>>> PutExam([FromBody] CUExam exam)
        {
            if (await _unitOfWork.ExamRepository.IsExistCode(exam.Code, exam.Id))
            {
                return ApiResponse<Exam>.ErrorResponse<Exam>("Mã đề thi đã tồn tại");
            }

            if (await _unitOfWork.ExamRepository.IsExistExamName(exam.ExamName, exam.LessonId, exam.Id))
            {
                return new ApiResponse<Exam>()
                {
                    Success = false,
                    Message = "Tên bài thi đã tồn tại"
                };
            }
            if (!await _unitOfWork.QuestionRepository.CheckQuantityQuestionInLesson(exam.TotalQuestions, exam.LessonId))
            {
                return ApiResponse<Exam>.ErrorResponse<Exam>("Số lượng câu hỏi của bài học không đủ");
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
                examUpdate.Code = exam.Code;
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
            if (await _unitOfWork.ExamRepository.IsExistCode(exam.Code))
            {
                return ApiResponse<Exam>.ErrorResponse<Exam>("Mã đề thi đã tồn tại");
            }

            if (await _unitOfWork.ExamRepository.IsExistExamName(exam.ExamName, exam.LessonId))
            {
                return new ApiResponse<Exam>()
                {
                    Success = false,
                    Message = "Tên bài thi đã tồn tại"
                };
            }

            if (!await _unitOfWork.QuestionRepository.CheckQuantityQuestionInLesson(exam.TotalQuestions, exam.LessonId))
            {
                return ApiResponse<Exam>.ErrorResponse<Exam>("Số lượng câu hỏi của bài học không đủ");
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
