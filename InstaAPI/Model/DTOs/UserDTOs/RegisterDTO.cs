﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model.DTOs
{
    public class RegisterDTO:LoginDTO
    {
        public string NickName { get; set; }
    }
}
