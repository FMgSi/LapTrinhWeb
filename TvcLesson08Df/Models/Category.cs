using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TvcLesson08Df.Models;

public partial class Category
{
    [Key]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Tên thể loại không được để trống")]
    [StringLength(100)]
    [Display(Name = "Tên thể loại")]
    public string CategoryName { get; set; } = null!;

    [StringLength(255)]
    [Display(Name = "Mô tả")]
    public string? Description { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
