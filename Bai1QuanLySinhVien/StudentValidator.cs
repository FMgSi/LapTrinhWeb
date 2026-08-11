using System.Text.RegularExpressions;

public static class StudentValidator
{
    public static bool IsValidName(string HoTen)
    {
        if (string.IsNullOrWhiteSpace(HoTen) == true)
        {
            return false;
        } else
        {
            return true;
        }
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrEmpty(email))return false;
        return (Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"));
    }
    public static bool IsValidGpa(double dtb)
    {
        return (dtb >=0 && dtb <=10);
    }
}