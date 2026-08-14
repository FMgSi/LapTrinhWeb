public class Student
{
    public string MaSV {get; set;} = string.Empty;
    public string HoTen {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
    public string sdt {get; set;} = string.Empty;
    public string NganhHoc {get; set;} = string.Empty;
    public DateTime NgaySinh{ get; set;}
    public double dtb{get; set;}
    public string GioiTinh{get; set;} = string.Empty;
    public string TrangThaiHocTap {get; set;} = string.Empty;

    public Student() {}
    public Student(string MaSV, string HoTen, string Email, string sdt, string NganhHoc, DateTime NgaySinh, 
    double dtb, string GioiTinh, string TrangThaiHocTap)
    {
        this.MaSV = MaSV;
        this.HoTen = HoTen;
        this.Email = Email;
        this.sdt = sdt;
        this.NganhHoc = NganhHoc;
        this.NgaySinh = NgaySinh;
        this.dtb = dtb;
        this.GioiTinh = GioiTinh;
        this.TrangThaiHocTap = TrangThaiHocTap;
    }
    public override string ToString()
    {
        return $"[Ma SV: {MaSV}] - {HoTen} | Ngay Sinh: {NgaySinh:dd/MM/yyyy} | Nganh: {NganhHoc} | Trang thai: {TrangThaiHocTap}";
    }
} 