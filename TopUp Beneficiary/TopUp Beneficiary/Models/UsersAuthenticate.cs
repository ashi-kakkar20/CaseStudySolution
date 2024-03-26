using System.ComponentModel.DataAnnotations;

namespace TopUp_Beneficiary.Models
{
    public class UsersAuthenticate
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsVerified { get; set; }
    }
}
