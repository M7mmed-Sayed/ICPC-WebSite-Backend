using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO
{
    public class SignIn
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
