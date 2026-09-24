using System.ComponentModel.DataAnnotations;

namespace MvcModelDemo.Models;

public class Login
{
    public string userName { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string password { get; set; } = string.Empty;
}