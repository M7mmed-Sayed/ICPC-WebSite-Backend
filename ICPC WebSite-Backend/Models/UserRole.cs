﻿using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Models
{
    public class UserRole
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Role { get; set; }

    }

}
