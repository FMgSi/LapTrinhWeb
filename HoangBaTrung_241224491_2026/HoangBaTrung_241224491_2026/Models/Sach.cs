using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HoangBaTrung_241224491_2026.Models;

public partial class Sach
{
    public int MaSach { get; set; }

    public int MaTL { get; set; }

    [Required(ErrorMessage = "Tên sách không được để trống")]
    public string TenSach { get; set; } = null!;

    [Required(ErrorMessage = "Giá không được để trống")]
    [Range(50, 1000, ErrorMessage = "Giá phải là con số từ 50 đến 1000")]
    public decimal? Gia { get; set; }

    [Required(ErrorMessage = "Tên file không được để trống")]
    [RegularExpression(@"^.+\.(jpg|jpeg|png|webp|JPG|JPEG|PNG|WEBP)$", ErrorMessage = "Ảnh phải có đuôi .jpg, .jpeg, .png hoặc .webp")]
    public string? Anh { get; set; }

    public virtual TheLoai MaTLNavigation { get; set; } = null!;
}
