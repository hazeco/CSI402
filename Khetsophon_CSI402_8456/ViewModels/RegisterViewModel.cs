using System.ComponentModel.DataAnnotations;

namespace Khetsophon_CSI402_8456.ViewModels;

public class RegisterViewModel
{
    [Required(ErrorMessage = "กรุณากรอกอีเมล")]
    [EmailAddress(ErrorMessage = "กรุณากรอกอีเมลให้ถูกต้อง")]
    public string Email { get; set; }

    [Required(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์")]
    [Phone(ErrorMessage = "กรุณากรอกเบอร์โทรศัพท์ให้ถูกต้อง")]
    public string Phone { get; set; }
    
    [Required(ErrorMessage = "กรุณากรอกรหัสผ่าน")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "กรุณากรอกรหัสผ่านอีกครั้ง")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "รหัสผ่านไม่ตรงกัน")]
    public string ConfirmPassword { get; set; }
}