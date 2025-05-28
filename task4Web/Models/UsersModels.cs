using System.ComponentModel.DataAnnotations;

namespace task4Web.Models
{
    public class UsersModels
    {
        public int id { get; set; }
        public string userName {  get; set; }
        [Required(ErrorMessage = "The password is obligatory.")]      
        public string hashPassword { get; set; }
        [Required(ErrorMessage = "The email is obligatory.")]
        public string email { get; set; }
        public string lastDate { get; set; }
        public string lastHour { get; set; }
        public string userState { get; set; }

    }
}
