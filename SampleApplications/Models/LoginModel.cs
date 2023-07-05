using System.ComponentModel.DataAnnotations;

namespace SampleApplications.Models
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
