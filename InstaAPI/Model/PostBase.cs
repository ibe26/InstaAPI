using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model
{
    public class PostBase
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
        public int UserID { get; set; }

    }
}
