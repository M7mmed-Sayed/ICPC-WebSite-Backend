using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO;

public class ResetPassword
{
    [DataType(DataType.Password)]
    [Required]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Required]
    [Compare("Password")]
    public string ConfirmPassword { get; set; }
}