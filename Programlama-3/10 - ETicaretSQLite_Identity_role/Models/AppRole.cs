using Microsoft.AspNetCore.Identity;

namespace ETicaret.Models
{
    public class AppRole : IdentityRole
    {
        public string Aciklama { get; set; }
    }
}