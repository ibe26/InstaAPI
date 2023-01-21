using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model.DTOs
{
    public class LoginReq
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
    }
}
