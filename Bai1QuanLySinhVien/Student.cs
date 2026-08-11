public class Student
{
    public string MaSV {get; set;}
    public string HoTen {get; set;}
    public string Email {get; set;}
    public string sdt {get; set;}
    public string NganhHoc {get; set;}
    public DateTime NgaySinh{ get; set;}
    public double dtb{get; set;}
    public string GioiTinh{get; set;}
    public string TrangThaiHocTap {get; set;}

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