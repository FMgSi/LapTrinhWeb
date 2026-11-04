using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApp.Models
{
    public class Learner
    {
        public Learner()
        {
            Enrollments = new HashSet<Enrollment>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Mã sinh viên")]
        public int LearnerID { get; set; }

        [Required(ErrorMessage = "Họ đệm không được để trống")]
        [Display(Name = "Họ đệm")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên không được để trống")]
        [Display(Name = "Tên")]
        [StringLength(50)]
        public string FirstMidName { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        [Display(Name = "Ngày nhập học")]
        public DateTime EnrollmentDate { get; set; }

        [Display(Name = "Chuyên ngành")]
        public int MajorID { get; set; }

        // Khóa ngoại liên kết tới bảng Major
        [ForeignKey("MajorID")]
        public virtual Major? Major { get; set; }

        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
