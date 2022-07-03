using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO;

public class ChangePassword
{
    [Required]
    public string userId { get; set; }
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