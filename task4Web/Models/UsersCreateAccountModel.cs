using System.ComponentModel.DataAnnotations;

namespace task4Web.Models
{
    public class UsersCreateAccountModel
    {
        [Required(ErrorMessage = "[The Name is obligatory]")]
        public string userName { get; set; }
        [Required(ErrorMessage = "[The password is obligatory]")]
        public string hashPassword { get; set; }
        [Required(ErrorMessage = "[The email is obligatory]")]
        public string email { get; set; }
    }
}
