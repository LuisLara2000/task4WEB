using System.ComponentModel.DataAnnotations;

namespace task4Web.Models
{
    public class UsersLogInModel
    {
       
        [Required(ErrorMessage = "[The email is obligatory]")]
        public string email { get; set; }
        [Required(ErrorMessage = "[The password is obligatory]")]
        public string password { get; set; }
    }
}
