public class StudentConsoleView
{
    public void HienThiDanhSach(List<Student> ds)
    {
        if (ds.Count == 0)
        {
            Console.WriteLine("Danh sach hien dang trong");
        } else
        {
            foreach (var i in ds)
            {
                Console.WriteLine(i.MaSV + "\t" + i.HoTen + "\t" + i.Email + "\t" + i.sdt + "\t" + i.NganhHoc + "\t" + i.dtb + "\t" + i.TrangThaiHocTap);
            }
        }

    }

    public string NhapHoten()
    {
        while (true)
        {
            Console.Write("Nhap ho va ten: ");
            string? HoTen = Console.ReadLine;

            if (StudentValidator.IsValidName(HoTen ?? ""))
            {
                return HoTen!.Trim();
            }

            Console.WriteLine("Ten khong duoc de trong!");
        }
    }

    public double NhapDiemTrungBinh()
    {
        while (true)
        {
            Console.Write("Nhap diem trung binh (0.0 - 10.0): ");
            string? input = Console.ReadLine();
            if (double.TryParse(input, out double dtb) && StudentValidator.IsValidGpa(dtb));
            {
                return dtb;
            }
            }
        }
    public string NhapEmail()
    {
        while (true)
        {
            Console.Write("Nhap emial: ");
            string? input = Console.ReadLine();
            if (StudentValidator.IsValidEmail( ?? ""))
            {
                
            }

        }
    }
}