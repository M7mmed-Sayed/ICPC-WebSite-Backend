﻿using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Data.Models.DTO
{
    public class UserRole
    {
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Role { get; set; }

    }

}
