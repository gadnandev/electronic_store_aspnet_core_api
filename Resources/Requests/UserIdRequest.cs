﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicsStore.Resources.Requests {
    public class UserIdRequest {

        [Required]
        public Guid userId { get; set; }
    }
}
