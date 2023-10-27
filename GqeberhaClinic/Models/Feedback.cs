using System.ComponentModel.DataAnnotations;


namespace GqeberhaClinic.Models
{
    public class Feedback
    {
        [Key] public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Description { get; set; }
        public int SessionID { get; set; }
        public string? Reason { get; set; }
    }
}
