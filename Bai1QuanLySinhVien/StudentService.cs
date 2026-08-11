public class StudentService()
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
        
    }
    
}