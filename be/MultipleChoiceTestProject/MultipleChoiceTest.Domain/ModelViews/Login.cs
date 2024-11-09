using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class Login
    {
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string Password { get; set; }
    }
}
