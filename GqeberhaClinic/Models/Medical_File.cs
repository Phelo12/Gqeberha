using GqeberhaClinic.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace GqeberhaClinic.Models
{
	public class Medical_File
	{
		//Personal Information
		[Key]
        public int FileID { get; set; }

        public String? PatientID{ get; set; }
        [ForeignKey("PatientID")]
		public virtual GqebheraUser? mainUser { get; set; }

		//Gender
		[Display(Name = "Gender")]
		[RegularExpression("^[a-zA-Z]*$", ErrorMessage = "Only letters are allowed For Gender.")]
		[MaxLength(10)]
		public string? Gender { get; set; }

		//Date OF Birth
		[DataType(DataType.Date)]
		[Display(Name = "Date Of Birth")]
		public DateTime BirthDate { get; set; }
		//Id NUmber
		[Display(Name = "Identity Number")]
		[MaxLength(13)]
		[Required(AllowEmptyStrings =false,ErrorMessage ="Please note that valid SA ID number is required")]
		[RegularExpression(@"(([0-9]{2})(0|1)([0-9])([0-3])([0-9]))([ ]?)(([0-9]{4})([ ]?)([0-1][8]([ ]?)[0-9]))",
	  ErrorMessage = "Invalid South African ID number")]
		public string? IDNumber { get; set; }
		[Display(Name = "Address")]
		[Required]
		public string? AddressLine1 { get; set; }
		[Display(Name = "Province")]
		[Required]
		public string? Province { get; set; }
		[Display(Name = "Country")]
		[Required]
		public string? Country { get; set; }
		[Display(Name = "Postal Code")]
		[Required]
		public string? PostalCode { get; set; }



		//Incase of Emergence,Emergency Person
		//Emergecy Person
		[Display(Name = "Full Names")]
		[MaxLength(50)]
		[MinLength(3)]
		[AllowNull]
		public string? EmergencyPerson { get; set; }

		//Emergency Person Contact
		[RegularExpression(@"^\+27\d{9}$", ErrorMessage = "Invalid  phone number, Please Start with your Country's Code")]
		[Display(Name = "Contact Number")]
		[AllowNull]
		public string? EmergencyContactNo { get; set; }

		//Emergency Person Relation

		[Display(Name = "Relationship")]
		public string? Relationship { get; set; }

		//Medical History
		[Display(Name = "Patient Blood Type")]
		public string? BloodType { get; set; }
		[Display(Name = "Any Allergies")]
		public string? Allergies { get; set; }
		[Display(Name = "Surgery in the Past?")]
		public string? AnySurgeries { get; set; }
		[Display(Name = "Extra Notes")]
		public string? ExtraNotes { get; set; }
		public Medical_File()
		{
			BloodType = "Not Sure";
			AnySurgeries = "None";
			AnySurgeries = "No";

		}

	}
}
