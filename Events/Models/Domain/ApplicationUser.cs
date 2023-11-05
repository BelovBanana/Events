using Microsoft.AspNetCore.Identity;

namespace Events.Models.Domain
{
    public class ApplicationUser : IdentityUser 
    {
        public string Name { get; set; }
    }
}
