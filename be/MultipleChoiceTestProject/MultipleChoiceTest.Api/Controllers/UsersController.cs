using Microsoft.AspNetCore.Mvc;
using MultipleChoiceTest.Repository.UnitOfWork;

namespace MultipleChoiceTest.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUnitOfWork _unit;
		public UsersController(IUnitOfWork unit)
		{
			_unit = unit;
		}
		[HttpGet]
		public async Task<IActionResult> GetAsync()
		{
			return Ok(await _unit.UserRepository.GetAllAsync());
		}
	}
}
