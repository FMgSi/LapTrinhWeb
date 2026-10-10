using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class Book
{
    [Key]
    public int BookId { get; set; }

    [Required(ErrorMessage = "Tiêu đề sách không được để trống")]
    [StringLength(200)]
    [Display(Name = "Tên sách")]
    public string Title { get; set; } = null!;

    [StringLength(100)]
    [Display(Name = "Tác giả")]
    public string? Author { get; set; }

    [Display(Name = "Đơn giá (VNĐ)")]
    [Column(TypeName = "decimal(18, 2)")]
    [Range(0, 1000000000, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
    public decimal Price { get; set; }

    [Display(Name = "Số lượng tồn")]
    [Range(0, 1000000, ErrorMessage = "Số lượng phải lớn hơn hoặc bằng 0")]
    public int Quantity { get; set; }

    [Display(Name = "Thể loại")]
    public int CategoryId { get; set; }

    [Display(Name = "Nhà xuất bản")]
    public int PublisherId { get; set; }

    [StringLength(255)]
    [Display(Name = "Ảnh bìa")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Mô tả nội dung")]
    public string? Description { get; set; }

    [Display(Name = "Thể loại")]
    public virtual Category? Category { get; set; }

    [Display(Name = "Nhà xuất bản")]
    public virtual Publisher? Publisher { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
