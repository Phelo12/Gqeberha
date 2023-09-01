using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GqeberhaClinic.Areas.Identity.Data;

namespace GqeberhaClinic.Models
{
    public class Alert
    {
        [Key]
        public int AlertID { get; set; }
        public string? Message { get; set; }
        public string? Date { get; set; }
        public string? Status { get; set; }
        public int  LastView { get; set; }
        public string? Role { get; set; }
        public string? IntendedUser { get; set; }
        public Alert()
        {
            Date = DateTime.Now.ToString("dd/MMMM/yyyy HH:mm:ss");
            Status = "New";
            LastView = -1;
            Role = "All";
            IntendedUser = "All";
        }

    }
}
