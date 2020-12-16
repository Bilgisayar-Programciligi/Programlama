using System.Linq;
using System.Threading.Tasks;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    [Authorize(Roles="Admin")]
    public class KullaniciIslemleri: Controller
    {
        private readonly UserManager<AppUser> _userManager;
        public KullaniciIslemleri(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _userManager.Users.ToListAsync();
          return View(kullanicilar);
        }
    }
}