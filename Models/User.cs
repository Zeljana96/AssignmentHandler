using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasks_Handler.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public List<Assignment> Assignments { get; set; }
        [Required]
        public string Role { get; set; }
    }
}