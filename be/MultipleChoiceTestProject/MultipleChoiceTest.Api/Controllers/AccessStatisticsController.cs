using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultipleChoiceTest.Domain.ApiModel;
using MultipleChoiceTest.Domain.Models;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessStatisticsController : ControllerBase
    {
        private readonly IUnitOfWork _unit;

        public AccessStatisticsController(IUnitOfWork unitOfWork)
        {
            _unit = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<AccessStatistic>>>> GetAccessStatistics()
        {
            var statistics = await _unit.AccessStatisticRepository.GetAllAsync();
            return Ok(new ApiResponse<IEnumerable<AccessStatistic>>
            {
                Success = statistics != null && statistics.Any(),
                Data = statistics,
                Message = statistics == null || !statistics.Any() ? "No access statistics found" : ""
            });
        }

        // GET: api/AccessStatistics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<AccessStatistic>>> GetAccessStatistic(int id)
        {
            var accessStatistic = await _unit.AccessStatisticRepository.GetByIdAsync(id);

            return Ok(new ApiResponse<AccessStatistic>
            {
                Success = accessStatistic != null,
                Data = accessStatistic,
                Message = accessStatistic == null ? "Not found access statistic" : ""
            });
        }

        // PUT: api/AccessStatistics/5
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<AccessStatistic>>> PutAccessStatistic(int id, AccessStatistic accessStatistic)
        {
            if (id != accessStatistic.Id)
            {
                return BadRequest(new ApiResponse<AccessStatistic>
                {
                    Success = false,
                    Message = "ID mismatch"
                });
            }


            try
            {
                await _unit.AccessStatisticRepository.UpdateAsync(accessStatistic);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await AccessStatisticExists(id))
                {
                    return NotFound(new ApiResponse<AccessStatistic>
                    {
                        Success = false,
                        Message = "Not found access statistic"
                    });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/AccessStatistics
        [HttpPost]
        public async Task<ActionResult<ApiResponse<AccessStatistic>>> PostAccessStatistic(AccessStatistic accessStatistic)
        {
            await _unit.AccessStatisticRepository.AddAsync(accessStatistic);

            return CreatedAtAction("GetAccessStatistic", new { id = accessStatistic.Id }, new ApiResponse<AccessStatistic>
            {
                Success = true,
                Data = accessStatistic,
                Message = "Access statistic created successfully"
            });
        }

        // DELETE: api/AccessStatistics/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<AccessStatistic>>> DeleteAccessStatistic(int id)
        {
            var accessStatistic = await _unit.AccessStatisticRepository.GetByIdAsync(id);
            if (accessStatistic == null)
            {
                return NotFound(new ApiResponse<AccessStatistic>
                {
                    Success = false,
                    Message = "Not found access statistic"
                });
            }

            try
            {
                await _unit.AccessStatisticRepository.SoftRemoveAsync(accessStatistic);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok(new ApiResponse<AccessStatistic>
            {
                Success = true,
                Message = "Access statistic deleted successfully"
            });
        }

        private async Task<bool> AccessStatisticExists(int id)
        {
            return await _unit.AccessStatisticRepository.GetByIdAsync(id) != null;
        }
    }
}
