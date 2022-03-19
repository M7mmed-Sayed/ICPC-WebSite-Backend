﻿using System.ComponentModel.DataAnnotations;

namespace ICPC_WebSite_Backend.Models.DTO
{
    public class CommunityDTO
    {
        public string Name { get; set; }
        public string About { get; set; }
        [EmailAddress]
        public string OfficialMail { get; set; }

    }
}
