using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TvcMember.Models
{
    public class Member
    {
        [DisplayName("Mã số")]
        public int Id { get; set; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = "Tài khoản không được để trống")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Tài khoản có độ dài trong khoảng 3 đến 20 ký tự")]
        public string Username { get; set; } = string.Empty;

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Mật khẩu tối thiểu 8 ký tự")]
        public string Password { get; set; } = string.Empty;

        [DisplayName("Hộp thư Email")]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Số điện thoại")]
        [Required(ErrorMessage = "Bạn chưa nhập điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Điện thoại phải là 10 ký tự số bắt đầu bằng số 0")]
        public string Phone { get; set; } = string.Empty;
    }
}