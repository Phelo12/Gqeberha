using GqeberhaClinic.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using GqeberhaClinic.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GqeberhaClinic.Models;

namespace GqeberhaClinic.Models
{
    [Keyless]
    public class Appointment_QueVM
    {
        //Appopintmnet
        [Key]
        public int AppointmentID { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [Required(AllowEmptyStrings = false, ErrorMessage = "Reason for appointment should be included")]
        public string? Reason { get; set; }
        public string? Status { get; set; } = "New";
        public String? PatientID { get; set; }
        [ForeignKey("PatientID")]
        public virtual GqebheraUser? MainUser { get; set; }
        [Required]
        [Display(Name = "Appointment Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime Date_Time { get; set; }

        //Que
        public int QueID { get; set; }
        public int AppointmentIDK { get; set; }
        [ForeignKey(nameof(AppointmentIDK))]
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
        public string? ClinicianID { get; set; }
        [ForeignKey(nameof(ClinicianID))]
        public virtual GqebheraUser? Clinician { get; set; }
        public string? QStatus { get; set; }

    }
}
