public class StudentService
{
    private List<Student> students = new List<Student>();

    public bool ThemSinhVien(Student student)
    {
        if (TimSinhVienTheoMa(student.MaSV)!= null)
        {
            return false;
        } else
        {
            students.Add(student);
            return true;
        }
    }

    public List<Student> HienThiDanhSach()
    {
        return students;
    }

    public Student? TimSinhVienTheoMa(string masv)
    {
        return students.FirstOrDefault(s => s.MaSV.Equals(masv, StringComparison.OrdinalIgnoreCase));
    }

    public List<Student> SapXepTheoDiemTrungBinh()
    {
        for (int i = 0; i < students.Count-1; i++)
        {
            for (int j = i+1; j < students.Count; j++)
            {
                if (students[i].dtb >students[j].dtb)
                {
                    Student temp = students[i];
                    students[i] = students[j];
                    students[j] = temp;
                }
            }
        }
        return students;
    }

    public bool XoaSinhVienTheoMa(string masv)
    {
    Student? sv = TimSinhVienTheoMa(masv);
    if (sv == null) return false;

    students.Remove(sv);
    return true;
    }

    public bool CapNhatSinhVien(string masv, Student b)
    {

        Student? a = TimSinhVienTheoMa(masv);
        if (a==null) return false;
        a.HoTen = b.HoTen;
        a.dtb = b.dtb;
        a.Email = b.Email;
        a.GioiTinh = b.GioiTinh;
        a.NganhHoc =b.NganhHoc;
        a.NgaySinh =b.NgaySinh;
        a.TrangThaiHocTap = b.TrangThaiHocTap;
        return true;
    }

    public List<Student> HienThiCacSinhVienCoDiemTu8TroLen()
    {
        List<Student> new1 = new List<Student>();
        for (int i = 0; i < students.Count; i++)
        {
            if (students[i].dtb >=8)
            {
                new1.Add(students[i]);
            }
        }
        return new1;
    }
    public List<Student> SapXepTheoHoTen()
    {
        return students.OrderBy(s => s.HoTen).ToList();
    }

    public Student? SinhVienCoDiemCaoNhat()
    {
        double dtbMax = students[0].dtb;
        for (int i = 0; i < students.Count; i++)
        {
            if (dtbMax < students[i].dtb)
            {
                dtbMax = students[i].dtb;
            }
        }
        for (int i = 0; i < students.Count; i++)
        {
            if (dtbMax == students[i].dtb)
            {
                return students[i];
            }
        }
        return null;
    }

    public double TinhDiemTrungBinhToanBo()
    {
        double dtbDtb = 0;
        for (int i = 0; i < students.Count; i++)
        {
            dtbDtb += students[i].dtb;
        }
        return dtbDtb/students.Count;
    }

    public List<Student> TimKiemTheoTen(string ten)
    {
        List<Student> ketQua = new List<Student>();

        if (string.IsNullOrWhiteSpace(ten)) return ketQua;
        foreach (var item in students)
        {
            if (item.HoTen != null && item.HoTen.ToLower().Contains(ten.ToLower()))
            {
                ketQua.Add(item);
            }
        }
        return ketQua;
    }

    public Dictionary<string, int> ThongKeTheoNganhCoBan()
    {
        Dictionary<string, int> ketQua = new Dictionary<string, int>();

        foreach (var s in students)
        {
            string nganh = s.NganhHoc;

            if (ketQua.ContainsKey(nganh))
            {
                ketQua[nganh]++;
            }
            else
            {
                ketQua.Add(nganh, 1);
            }
        }

        return ketQua;
    }

    public Dictionary<string, int> ThongKeTheoTrangThaiHocTap()
    {
        Dictionary<string, int> ketQua = new Dictionary<string, int>();
        foreach (var item in students)
        {
            if (ketQua.ContainsKey(item.TrangThaiHocTap))
            {
                ketQua[item.TrangThaiHocTap]++;
            } else
            {
                ketQua.Add(item.TrangThaiHocTap, 1);
            }
        }
        return ketQua;
    }
    
}

