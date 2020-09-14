using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ETicaret.Models
{
    public class AppUser : IdentityUser
    {
        public string Aciklama { get; set; }
    }
}