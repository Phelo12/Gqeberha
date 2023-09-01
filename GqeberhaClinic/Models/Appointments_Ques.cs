using GqeberhaClinic.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GqeberhaClinic.Models
{
    public class Appointments_Ques
    {
        [Key]
        public int QueID { get; set; }
        public int AppointmentID { get; set; }
        [ForeignKey(nameof(AppointmentID))]
        public virtual Appointments? Appointments { get; set; }
        [DataType(DataType.Date)]
        public DateTime dateOFQue { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Delay Time")]
        public DateTime Time { get; set; }
        [Required]
        [Display(Name = "Room Number")]
        public string? RoomNumber { get; set; }
        [Display(Name = "Clinician Infromation")]
        public string? Clinician { get; set; }
        public string? Status { get; set; }

        public Appointments_Ques()
        {
            dateOFQue = DateTime.Now;
            Status = "Queued";
        }

    }
}
