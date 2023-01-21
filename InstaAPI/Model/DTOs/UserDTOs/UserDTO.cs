using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model.DTOs
{
    public class UserDTO
    {
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordKey { get; set; }
    }
}
