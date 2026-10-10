using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class Publisher
{
    [Key]
    public int PublisherId { get; set; }

    [Required(ErrorMessage = "Tên NXB không được để trống")]
    [StringLength(150)]
    [Display(Name = "Tên nhà xuất bản")]
    public string PublisherName { get; set; } = null!;

    [StringLength(255)]
    [Display(Name = "Địa chỉ")]
    public string? Address { get; set; }

    [StringLength(20)]
    [Display(Name = "Số điện thoại")]
    public string? Phone { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
