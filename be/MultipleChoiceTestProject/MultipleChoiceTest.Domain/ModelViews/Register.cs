using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
	public class Register
	{
		[Required(ErrorMessage = "Họ tên không được để trống")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "Email không được để trống")]
		public string Email { get; set; }

		public string Gender { get; set; }

		public DateOnly? DateOfBirth { get; set; }

		public string? Phone { get; set; }
		[Required(ErrorMessage = "Tên đăng nhập không được để trống")]
		public string AccountName { get; set; }
		[Required(ErrorMessage = "Mật khẩu không được để trống")]
		public string PasswordHash { get; set; }
		[Required(ErrorMessage = "Nhập lại khẩu không được để trống")]
		public string EnterPassword { get; set; }
	}
}
