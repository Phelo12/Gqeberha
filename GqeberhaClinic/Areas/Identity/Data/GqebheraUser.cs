using Microsoft.AspNetCore.Identity;

namespace GqeberhaClinic.Areas.Identity.Data
{
    public class GqebheraUser : IdentityUser
    {
        public byte[]? MyPicture { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}

    }
}
