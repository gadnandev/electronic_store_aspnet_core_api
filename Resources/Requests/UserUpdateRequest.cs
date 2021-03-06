﻿using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsStore.Resources.Requests {
    public class UserUpdateRequest {

        [Required]
        public Guid UserId { get; set; }

        [RegularExpression(@"^(?!.*\.\.)(?!.*\.$)[^\W][\w.]{0,29}$")]
        public string Username { get; set; }

        [MinLength(6)]
        public string Password { get; set; }

        public string Fullname { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public IFormFile AvatarImage { get; set; }
    }
}
