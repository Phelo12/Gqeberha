using System.ComponentModel.DataAnnotations;

namespace GqeberhaClinic.Models
{
    public class UserInformation
    {
        [Key] public int userId { get; set; }
        [Required] public string? FirstName { get; set; }
        [Required] public string? LastName { get; set; }
        [Required] public string? PhoneNumber { get; set; }
        [Required] public string? Email { get; set; }
        [Required] public string? Address { get; set; }
    }
}
