using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model
{
    public class Comment
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public int PostID { get; set; }
        public string CommenterUsername { get; set; }

        public DateTime Date { get; set; }
    }
}
