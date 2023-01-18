using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model
{
    public class User
    {
        public int UserID { get; set; }
        public string Nickname { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
    }
}
