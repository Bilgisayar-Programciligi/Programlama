using System.Linq;
using System.Threading.Tasks;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ETicaret.Controllers
{
    [Authorize(Roles="Admin")]
    public class KullaniciIslemleri: Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        public KullaniciIslemleri(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        #region gizle

        public async Task<IActionResult> Index()
        {
            var kullanicilar = await _userManager.Users.ToListAsync();
          return View(kullanicilar);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(KullaniciViewModel kullanici)
        {
            var yaratilacakKullanici = new AppUser()
            {
                UserName = kullanici.EPosta,
                Email = kullanici.EPosta,
                EmailConfirmed = true,
                Aciklama = kullanici.Aciklama
            };
            await _userManager.CreateAsync(yaratilacakKullanici, kullanici.Sifre);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAsync(string id)
        {
            AppUser silinecekUser = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(silinecekUser);
            return RedirectToAction(nameof(Index));
        }
        #endregion
        public async Task<IActionResult> KullanicininRolleri(string id)
        {
            var kullanici = await _userManager.FindByIdAsync(id);
            var kullanicininRolleri = await _userManager.GetRolesAsync(kullanici);
            var RolIdleri = await _userManager.GetRolesAsync(kullanici);
            ViewBag.KullanicininAdi = kullanici.UserName;
            return View(kullanicininRolleri);
        }
        // GET: Role/Assign

        public async Task<IActionResult> Assign(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            //var Roles = await _roleManager.Roles.ToListAsync();
            ViewData["RoleName"] = new SelectList(_roleManager.Roles, "Name", "Name");
            ViewData["UserName"] = new SelectList(_userManager.Users, "UserName", "UserName", user.UserName);
            return View(new RoleModel());
        }
        // POST: Role/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                if (roleModel.Name == "Admin")
                {
                    ViewData["Message"] = "Invalid Request.";
                    return View("Info");
                }
                var user = await _userManager.FindByEmailAsync(roleModel.UserName);
                if (user != null)
                {
                    if (await _roleManager.RoleExistsAsync(roleModel.Name))
                    {
                        if (await _userManager.IsInRoleAsync(user, roleModel.Name))
                        {
                            ViewData["Message"] = $@"User {roleModel.UserName} already has the {roleModel.Name} role.";
                            return View("Info");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(user, roleModel.Name);
                            ViewData["Message"] = $@"User {roleModel.UserName} was assigned the {roleModel.Name} role.";
                            return View("Info");
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Invalid Request.";
                        return View("Info");
                    }
                }
                else
                {
                    ViewData["Message"] = "Invalid Request.";
                    return View("Info");
                }
            }
            return View(roleModel);
        }

    }
}