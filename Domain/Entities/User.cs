﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
