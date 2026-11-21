using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HoangBaTrung_2026_QLTS.Models;

public partial class TaiSan
{
    public int MaTS { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại tài sản")]
    public int MaLoai { get; set; }

    [Required(ErrorMessage = "Tên tài sản không được để trống")]
    public string TenTS { get; set; } = null!;

    [Required(ErrorMessage = "Nguyên giá không được để trống")]
    [Range(1, 2000, ErrorMessage = "Nguyên giá phải là số từ 1 đến 2000")]
    public decimal? NguyenGia { get; set; }

    [Required(ErrorMessage = "Tên file ảnh không được để trống")]
    [RegularExpression(@"^.+\.(jpg|jpeg|png|webp|JPG|JPEG|PNG|WEBP)$",
        ErrorMessage = "Ảnh phải có đuôi .jpg, .jpeg, .png hoặc .webp")]
    public string? Anh { get; set; }

    public virtual LoaiTaiSan MaLoaiNavigation { get; set; } = null!;
}
