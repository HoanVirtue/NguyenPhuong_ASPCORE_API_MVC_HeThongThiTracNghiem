using System.ComponentModel.DataAnnotations;

namespace MultipleChoiceTest.Domain.ModelViews
{
    public class CUUser
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Họ tên không được bỏ trống")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email không được bỏ trống")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Giới tính không được bỏ trống")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được bỏ trống")]
        public DateOnly? DateOfBirth { get; set; }
        [Required(ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Tên đăng nhập không được bỏ trống")]
        public string AccountName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được bỏ trống")]
        public string PasswordHash { get; set; }


        public bool IsAdmin { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
