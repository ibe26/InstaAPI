using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InstaAPI.Model
{
    public class WhoLiked
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int PostID { get; set; }

    }
}
