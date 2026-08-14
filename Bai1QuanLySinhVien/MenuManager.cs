using System;
using System.Collections.Generic;

public class MenuManager
{
    private StudentService service = new StudentService();
    private StudentConsoleView view = new StudentConsoleView();

    private void Mau()
    {
        service.ThemSinhVien(new Student("SV01", "Nguyen Van An", "an@gmail.com", "0912345678", "CNTT", new DateTime(2004, 1, 15), 8.5, "Nam", "Dang hoc"));
        service.ThemSinhVien(new Student("SV02", "Tran Thi Binh", "binh@gmail.com", "0987654321", "KTPM", new DateTime(2004, 5, 20), 7.2, "Nu", "Dang hoc"));
        service.ThemSinhVien(new Student("SV03", "Le Hoang Cuong", "cuong@gmail.com", "0901234567", "CNTT", new DateTime(2003, 11, 2), 9.2, "Nam", "Tot nghiep"));
        service.ThemSinhVien(new Student("SV04", "Pham Hai Dang", "dang@gmail.com", "0933445566", "ATTT", new DateTime(2004, 8, 10), 6.5, "Nam", "Bao luu"));
    }

    public void Chay()
    {
        Mau();
        bool tiepTuc = true;

        while (tiepTuc)
        {
            Console.WriteLine("1. Xem danh sach sinh vien");
            Console.WriteLine("2. Them sinh vien moi");
            Console.WriteLine("3. Tim kiem theo ma");
            Console.WriteLine("4. Tim kiem theo ten");
            Console.WriteLine("5. Cap nhat thong tin");
            Console.WriteLine("6. Xoa sinh vien");
            Console.WriteLine("7. Sap xep theo diem giam dan");
            Console.WriteLine("8. Sap xep theo ten A-Z");
            Console.WriteLine("9. Loc sinh vien gioi (GPA >= 8.0)");
            Console.WriteLine("10. Tim sinh vien diem cao nhat (Thu khoa)");
            Console.WriteLine("11. Tinh diem trung binh chung");
            Console.WriteLine("12. Thong ke theo nganh");
            Console.WriteLine("13. Thong ke theo trang thai");
            Console.WriteLine("0. Thoat");
            string? chon = Console.ReadLine();

            switch (chon)
            {
                case "1":
                    view.HienThiDanhSach(service.HienThiDanhSach());
                    break;

                case "2":
                    Student svMoi = view.NhapThongTinSinhVien();
                    if (service.ThemSinhVien(svMoi))
                        Console.WriteLine("Done");
                    else
                        Console.WriteLine("Error");
                    break;

                case "3":
                    string maCanTim = view.NhapMaSV();
                    Student? svTim = service.TimSinhVienTheoMa(maCanTim);
                    if (svTim != null)
                        view.HienThiDanhSach(new List<Student> { svTim });
                    else
                        Console.WriteLine("Khong tim thay");
                    break;

                case "4":
                    Console.Write("Nhap ten can tim: ");
                    string tuKhoa = Console.ReadLine() ?? "";
                    var dsTim = service.TimKiemTheoTen(tuKhoa);
                    view.HienThiDanhSach(dsTim);
                    break;

                case "5":
                    Console.WriteLine("\n--- CAP NHAT SINH VIEN ---");
                    string maCanSua = view.NhapMaSV();
                    if (service.TimSinhVienTheoMa(maCanSua) == null)
                    {
                        Console.WriteLine(">> Khong tim thay sinh vien de sua!");
                        break;
                    }
                    Student svSua = view.NhapThongTinSinhVien(sua: true);
                    service.CapNhatSinhVien(maCanSua, svSua);
                    Console.WriteLine("Done!");
                    break;

                case "6":
                    string maCanXoa = view.NhapMaSV();
                    if (service.XoaSinhVienTheoMa(maCanXoa))
                        Console.WriteLine("Done");
                    else
                        Console.WriteLine("Error");
                    break;

                case "7":
                    view.HienThiDanhSach(service.SapXepTheoHoTen());
                    break;

                case "8":
                    view.HienThiDanhSach(service.SapXepTheoDiemTrungBinh());
                    break;

                case "9":
                    view.HienThiDanhSach(service.HienThiCacSinhVienCoDiemTu8TroLen());
                    break;

                case "10":
                    Student? diemMax = service.SinhVienCoDiemCaoNhat();
                    if (diemMax != null)
                        view.HienThiDanhSach(new List<Student> { diemMax });
                    else
                        Console.WriteLine("Rong");
                    break;

                case "11":
                    double dtb = service.TinhDiemTrungBinhToanBo();
                    Console.WriteLine($"\nDiem trung binh toan bo sinh: {dtb:F2}");
                    break;

                case "12":
                    Console.WriteLine("\n Thong ke sinh vien theo nganh: ");
                    foreach (var item in service.ThongKeTheoNganhCoBan())
                    {
                        Console.WriteLine($"- Nganh {item.Key}: {item.Value} sinh vien");
                    }
                    break;

                case "13":
                    Console.WriteLine("\nThong ke sinh vien theo trang thai");
                    foreach (var item in service.ThongKeTheoTrangThaiHocTap())
                    {
                        Console.WriteLine($"- Trang thai [{item.Key}]: {item.Value} sinh vien");
                    }
                    break;

                case "0":
                    tiepTuc = false;
                    Console.WriteLine("Thoat!");
                    break;

                default:
                    Console.WriteLine("Error");
                    break;
            }

            if (tiepTuc)
            {
                Console.WriteLine("\nBam phim bat ky de tiep tuc...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}