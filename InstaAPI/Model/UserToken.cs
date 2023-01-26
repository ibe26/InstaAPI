using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InstaAPI.Model
{
    public class UserToken
    {

        [Key] public int ID { get; set; }
        [ForeignKey("User")] public int UserID { get; set; }
        public string Token { get; set; }

    }
}
