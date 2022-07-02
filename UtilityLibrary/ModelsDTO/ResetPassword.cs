using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO;

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