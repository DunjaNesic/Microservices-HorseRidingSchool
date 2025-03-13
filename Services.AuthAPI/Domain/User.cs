using Microsoft.AspNetCore.Identity;

namespace Services.AuthAPI.Domain
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
