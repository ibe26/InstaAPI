using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InstaAPI.Model
{
    public class Post
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public ICollection<WhoLiked> WhoLiked { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

    }
}
