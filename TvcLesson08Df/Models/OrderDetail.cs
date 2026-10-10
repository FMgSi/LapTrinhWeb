using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class OrderDetail
{
    [Key]
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int BookId { get; set; }

    [Display(Name = "Số lượng mua")]
    [Range(1, 10000, ErrorMessage = "Số lượng phải lớn hơn 0")]
    public int Quantity { get; set; }

    [Display(Name = "Đơn giá")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Book? Book { get; set; }
}
