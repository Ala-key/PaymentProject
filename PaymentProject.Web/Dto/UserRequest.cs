using System.ComponentModel.DataAnnotations;

namespace PaymentProject.Dto
{
    public class UserRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
