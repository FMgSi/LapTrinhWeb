using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models
{
    public class Major
    {
        public Major()
        {
            Learners = new HashSet<Learner>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã ngành")]
        public int MajorID { get; set; }

        [Required(ErrorMessage = "Tên chuyên ngành không được để trống")]
        [Display(Name = "Tên chuyên ngành")]
        [StringLength(100)]
        public string MajorName { get; set; } = string.Empty;

        // Quan hệ 1 - N: Một chuyên ngành có nhiều sinh viên (Learner)
        public virtual ICollection<Learner> Learners { get; set; }
    }
}
