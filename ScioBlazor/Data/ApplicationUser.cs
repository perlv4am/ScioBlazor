using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ScioBlazor.Data
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(100)]
        public string? FirstName { get; set; }
        [MaxLength(100)]
        public string? LastName { get; set; }
    }

}
