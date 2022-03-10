using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Models
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
