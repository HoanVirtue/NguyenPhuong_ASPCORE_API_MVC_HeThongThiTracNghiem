using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CreateUser
    {
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Giới tính không được bỏ trống")]
        public string Gender { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Phone { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string PasswordHash { get; set; }


        public bool IsAdmin { get; set; }
    }
}
