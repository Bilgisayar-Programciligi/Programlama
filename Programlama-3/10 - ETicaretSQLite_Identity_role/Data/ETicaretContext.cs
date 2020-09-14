using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ETicaret.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ETicaret.Data
{
    public class ETicaretContext : IdentityDbContext<AppUser, AppRole, string>          
    {
        public ETicaretContext (DbContextOptions<ETicaretContext> options)
            : base(options)
        {
        }

        // c#: Urunler listesi    -----migrations, savechanges-----> sqlite:Urunler tablosuna 
        public DbSet<Urun> Urunler { get; set; } //_context.Urunler.Add(new Urun());   _context.Urunler.Remove(x);  _context.Urunler.Where(x=>x.)

        // c#: Resimler listesi    -----migrations, savechanges-----> sqlite:Resimler tablosuna 
        public DbSet<Resim> Resimler { get; set; }

        // c#: Kategoriler listesi    -----migrations, savechanges-----> sqlite:Kategoriler tablosuna 
        public DbSet<Kategori> Kategoriler { get; set; }
        
        public DbSet<KategoriUrun> KategorilerVeUrunleri { get; set; }


        //FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<KategoriUrun>()
                .HasKey(t => new { t.KategoriId, t.UrunId }); //Birleşik Id:PK
        }
    }
}
