using System.ComponentModel.DataAnnotations;

namespace UtilityLibrary.ModelsDTO
{
    public class UserRole
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Role { get; set; }

    }

}
