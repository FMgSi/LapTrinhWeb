using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class Customer
{
    [Key]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Họ tên không được để trống")]
    [StringLength(100)]
    [Display(Name = "Họ và tên")]
    public string FullName { get; set; } = null!;

    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(100)]
    [Display(Name = "Địa chỉ Email")]
    public string? Email { get; set; }

    [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
    [StringLength(20)]
    [Display(Name = "Số điện thoại")]
    public string? Phone { get; set; }

    [StringLength(255)]
    [Display(Name = "Địa chỉ liên hệ")]
    public string? Address { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
