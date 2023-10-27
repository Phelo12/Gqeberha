using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class Session_Feedback
    {
        [Key]
        public int Id { get; set; }
        public int SessionID { get; set; }
        [Required]
        public string FeedbackText { get; set; }
        public string? Reason { get; set; }

        // Add foreign key property to Appointments
        public int AppointmentID { get; set; }
        [ForeignKey("AppointmentID")]
        public virtual Appointments Appointment { get; set; }
    }

}
