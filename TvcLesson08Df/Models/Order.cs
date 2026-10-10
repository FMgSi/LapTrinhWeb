using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    [Display(Name = "Ngày đặt hàng")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Display(Name = "Khách hàng")]
    public int CustomerId { get; set; }

    [Display(Name = "Tổng tiền")]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

    [StringLength(50)]
    [Display(Name = "Trạng thái đơn hàng")]
    public string Status { get; set; } = "Đang xử lý";

    [Display(Name = "Khách hàng")]
    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
