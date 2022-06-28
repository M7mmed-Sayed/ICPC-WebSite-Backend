using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO;

public class ChangePassword
{
    [DataType(DataType.EmailAddress)]
    [Required]
    public string Email { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string CurrentPassword { get; set; }
    [DataType(DataType.Password)]
    [Required]
    public string NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
}