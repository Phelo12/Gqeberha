﻿using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GqeberhaClinic.Data
{
    public partial class ApplicationDbContext : IdentityDbContext<GqebheraUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<UserInformation> users { get; set; }
		public DbSet<Alert> Alerts { get; set; }
        public DbSet<GqeberhaClinic.Models.Appointments>? Appointments { get; set; }
        public DbSet<GqeberhaClinic.Models.MedicalRefill>? MedicalRefill { get; set; }
        public DbSet<GqeberhaClinic.Models.Medical_Feedback>? Medical_Feedback { get; set; }
        public DbSet<GqeberhaClinic.Models.Prescription>? Prescription { get; set; }
        public DbSet<GqeberhaClinic.Models.Medical_File>? Medical_File { get; set; }
		public DbSet<GqeberhaClinic.Models.Vaccine_Education>? Vaccine_Education { get; set; }
		public DbSet<GqeberhaClinic.Models.FamilyPlanning_Screening>? FamilyPlanning_Screening { get; set; }
		public DbSet<GqeberhaClinic.Models.UserVM>? UserVM { get; set; }
		public DbSet<GqeberhaClinic.Models.Records>? Records { get; set; }
		public DbSet<GqeberhaClinic.Models.Appointments_Ques>? Appointments_Ques { get; set; }
        public DbSet<GqeberhaClinic.Models.appointmentsGBV>? appointmentsGBVs { get; set; }
        public DbSet<ReportCase>? ReportCases { get; set; }
        public DbSet<Feedback>? Feedbacks { get; set; }
        public DbSet<Session_Feedback>? Session_Feedback { get; set; }
        public DbSet<GqeberhaClinic.Models.Contraceptive>? Contraceptive { get; set; }
        public DbSet<GqeberhaClinic.Models.ContraceptivePrescription>? ContraceptivePrescription { get; set; }
        public DbSet<GqeberhaClinic.Models.Referrals>? Referrals { get; set; }
        public DbSet<GqeberhaClinic.Models.Counselling_Sessions>? Counselling_Sessions { get; set; }
    }   
}