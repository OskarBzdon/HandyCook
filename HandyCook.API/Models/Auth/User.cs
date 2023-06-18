using Microsoft.AspNetCore.Identity;

namespace HandyCook.API.Models.Auth
{
    public class User : IdentityUser
    {
        public ICollection<Recipe> Recipes { get; set; }
    }
}
