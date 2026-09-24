namespace MvcModelDemo.Models;

public class User
{
    public long Id { get; set; }
    public string name { get; set; } = string.Empty;
    public string address { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
}