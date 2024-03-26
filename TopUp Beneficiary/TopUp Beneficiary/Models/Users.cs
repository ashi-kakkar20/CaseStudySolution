using System.ComponentModel.DataAnnotations;

namespace TopUp_Beneficiary.Models
{
    // Users.cs
    public class Users
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public bool IsVerified { get; set; }
    }
}