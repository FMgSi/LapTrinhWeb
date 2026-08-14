public class StudentConsoleView{

public string NhapMaSV()
{
    while (true)
    {
        Console.Write("Nhap ma sinh vien: ");
        string? maSV = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(maSV)) return maSV.Trim().ToUpper();
        Console.WriteLine("Nhap lai");
    }
}

public string NhapHoten()
{
    while (true)
    {
        Console.Write("Nhap ho ten: ");
        string? hoTen = Console.ReadLine();
        if (StudentValidator.IsValidName(hoTen ?? "")) return hoTen!.Trim();
        Console.WriteLine("Nhap lai");
    }
}

public string NhapEmail()
{
    while (true)
    {
        Console.Write("Nhap email: ");
        string? input = Console.ReadLine();
        if (StudentValidator.IsValidEmail(input ?? "")) return input!.Trim();
        Console.WriteLine("Nhap lai");
    }
}

public string NhapSdt()
{
    while (true)
    {
        Console.Write("Nhap so dien thoai: ");
        string? sdt = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(sdt)) return sdt.Trim();
        Console.WriteLine("Nhap lai");
    }
}

public double NhapDiemTrungBinh()
{
    while (true)
    {
        Console.Write("Nhap diem trung binh tu 0.0 den 10.0 ");
        string? input = Console.ReadLine();
        if (double.TryParse(input, out double dtb) && StudentValidator.IsValidGpa(dtb))
        {
            return dtb;
        }
        Console.WriteLine("Nhap lai");
    }
}

public Student NhapThongTinSinhVien(bool isUpdate = false)
{
    string maSV = isUpdate ? "" : NhapMaSV();
    string hoTen = NhapHoten();
    string email = NhapEmail();
    string sdt = NhapSdt();

    Console.Write("Nhap nganh hoc ");
    string nganhHoc = Console.ReadLine() ?? "";

    Console.Write("Nhap ngay sinh (dd/MM/yyyy) ");
    DateTime.TryParse(Console.ReadLine(), out DateTime ngaySinh);

    double dtb = NhapDiemTrungBinh();

    Console.Write("Nhap gioi tinh Nam/Nu: ");
    string gioiTinh = Console.ReadLine() ?? "";

    Console.Write("Nhap trang thai hoc tap : Dang hoc, bao luu, tot nghiep ");
    string trangThai = Console.ReadLine() ?? "";

    return new Student(maSV, hoTen, email, sdt, nganhHoc, ngaySinh, dtb, gioiTinh, trangThai);
}
}
