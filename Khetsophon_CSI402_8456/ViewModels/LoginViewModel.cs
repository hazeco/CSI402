using System.ComponentModel.DataAnnotations;

namespace Khetsophon_CSI402_8456.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "กรุณากรอกอีเมล")]
    [EmailAddress(ErrorMessage = "กรุณากรอกอีเมลให้ถูกต้อง")]
    public string Email { get; set; }

    public string? Phone { get; set; }

    [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}