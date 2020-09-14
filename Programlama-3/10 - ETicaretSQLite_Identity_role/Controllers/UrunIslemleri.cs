using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ETicaret.Data;
using ETicaret.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace ETicaret.Controllers
{
    public class UrunIslemleri : Controller
    {
        private readonly ETicaretContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        private string _dosyaYolu;

        //Yapıcı method
        //dependency injection --> bağımlı enjeksiyon
        public UrunIslemleri(ETicaretContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

            _dosyaYolu = Path.Combine(_hostEnvironment.WebRootPath, "resimler");
            if (!Directory.Exists(_dosyaYolu))
            {
                Directory.CreateDirectory(_dosyaYolu);
            }
        }

        // GET: UrunIslemleri
        public async Task<IActionResult> Index()
        {
            //tüm ürünleri  ---> index.cshtml
            ViewBag.KategoriAdi = "Tüm Kategoriler";
            return View(await _context.Urunler.Include(x => x.Resimler).ToListAsync());
        }

        public async Task<IActionResult> KategorininUrunleri(int? id) // id = 7  ---> moda
        {
            // kategorininUrunleri ---> index.cshtml

            var kategori = await _context.Kategoriler
                    .Include(x => x.KagegoriUrunleri).ThenInclude(x => x.Urun).ThenInclude(x => x.Resimler)
                    .SingleOrDefaultAsync(x => x.Id == id);

            //         Urun(Urun) <----select--- KategoriUrun (Kategori,Urun)
            var kategorininUrunleri = kategori.KagegoriUrunleri.Select(x => x.Urun);

            // return View(); //  views/urunislemleri/kategorininurunleri.cshtml
            ViewBag.KategoriAdi = kategori.Adi;
            ViewBag.KategoriId = kategori.Id;
            return View("index", kategorininUrunleri);
        }

        // GET: UrunIslemleri/KategorileriniAyarla/5
        public async Task<IActionResult> KategorileriniAyarla(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                        .Include(x => x.KategoriUrunleri).ThenInclude(x=>x.Kategori) 
                        .SingleOrDefaultAsync(m => m.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            //ViewBag.TumKategoriler = _context.Kategoriler..

            return View(urun); //bir tane Urun
        }

        [HttpPost]
        public async Task<IActionResult> KategorileriniAyarla(int? id, IFormCollection elemanlar)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                        .Include(x => x.KategoriUrunleri).ThenInclude(x=>x.Kategori) //????
                        .SingleOrDefaultAsync(m => m.Id == id);

            if (urun == null)
            {
                return NotFound();
            }

            var seciliKategoriler = elemanlar["SeciliKategoriler"]; //örneğin 1,7,8


            urun.KategoriUrunleri.Clear();
            foreach (var item in seciliKategoriler)   //örneğin 1,7,8
            {
                urun.KategoriUrunleri.Add(  new KategoriUrun{KategoriId = Convert.ToInt32(item)});
            }

            await _context.SaveChangesAsync();
            TempData["Mesaj"] = $"{urun.Ad} ürününün kategorileri başarıyla ayarlandı";

            //ViewBag.TumKategoriler = _context.Kategoriler..

            return View(urun); //bir tane Urun
        }

        // GET: UrunIslemleri/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler.Include(x => x.Resimler)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // GET: UrunIslemleri/Create
        public IActionResult Create(int? id)
        {
            return View(); //form ---> urun
        }

        // POST: UrunIslemleri/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? id, [Bind("Ad,Aciklama,Fiyat,Dosyalar")] Urun urun)
        {
            if (ModelState.IsValid)
            {
                foreach (var item in urun.Dosyalar)
                {
                    // IFormFile   -->  FileStream   :upload
                    using (var dosyaAkisi = new FileStream(Path.Combine(_dosyaYolu, item.FileName), FileMode.Create))
                    {
                        await item.CopyToAsync(dosyaAkisi);
                    }

                    urun.Resimler.Add(new Resim { DosyaAdi = item.FileName });
                }

                //var x = _context.Kategoriler.Find(id);  ---> buna gerek yok
                if (id != null) urun.KategoriUrunleri.Add(new KategoriUrun { KategoriId = (int)id });

                _context.Add(urun);
                await _context.SaveChangesAsync();
                TempData["Mesaj"] = $"{urun.Ad} ürünü başarıyla eklendi";

                if (id != null) return RedirectToAction(nameof(KategorininUrunleri), new { id = id });
                return RedirectToAction(nameof(Index)); //????
            }
            return View(urun);
        }

        // GET: UrunIslemleri/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler.Include(x => x.Resimler).SingleOrDefaultAsync(x => x.Id == id);

            if (urun == null)
            {
                return NotFound();
            }
            return View(urun);
        }

        // POST: UrunIslemleri/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Aciklama,Fiyat")] Urun urun)
        {
            if (id != urun.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urun);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrunExists(urun.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(urun);
        }

        // GET: UrunIslemleri/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var urun = await _context.Urunler
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urun == null)
            {
                return NotFound();
            }

            return View(urun);
        }

        // POST: UrunIslemleri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var urun = await _context.Urunler.Include(x => x.Resimler).SingleOrDefaultAsync(x => x.Id == id);
            _context.Urunler.Remove(urun);
            await _context.SaveChangesAsync();

            foreach (var item in urun.Resimler)
            {
                System.IO.File.Delete(Path.Combine(_dosyaYolu, item.DosyaAdi));
            }
            return RedirectToAction(nameof(Index));
        }

        //urunislemleri/ResimSil/2
        public async Task<IActionResult> ResimSil(int id)
        {
            var resim = await _context.Resimler.FindAsync(id);//resimler tablosu
            _context.Resimler.Remove(resim);//resimler tablosu
            await _context.SaveChangesAsync();//resimler tablosu

            System.IO.File.Delete(Path.Combine(_dosyaYolu, resim.DosyaAdi));//resimler tablosu

            // return RedirectToAction(nameof(Edit), new {id=resim.Urunu.Id});//urunler tablosu -->include
            return RedirectToAction(nameof(Edit), new { id = resim.UrunuId });//resimler tablosu
        }

        private bool UrunExists(int id)
        {
            return _context.Urunler.Any(e => e.Id == id);
        }
    }
}
