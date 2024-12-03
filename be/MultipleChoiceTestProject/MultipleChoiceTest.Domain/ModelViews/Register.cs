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
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateOnly? DateOfBirth { get; set; }

        public string? Phone { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Tên đăng nhập chỉ chứa từ 4 - 20 ký tự")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Mật khẩu phải chứa từ 8 - 32 ký tự")]
        public string PasswordHash { get; set; }
        [Required(ErrorMessage = "Lặp lại mật khẩu không được để trống")]
        [StringLength(32, MinimumLength = 8, ErrorMessage = "Lặp lại mật khẩu phải chứa từ 8 - 32 ký tự")]
        [Compare("PasswordHash", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp")]
        public string EnterPassword { get; set; }
    }
}
