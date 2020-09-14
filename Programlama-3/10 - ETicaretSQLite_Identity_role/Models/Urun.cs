using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ETicaret.Models
{
    public class Urun //tekil
    {
        public Urun()
        {
            Resimler = new List<Resim>();                   //null referans hatasına düşmemek
            KategoriUrunleri = new List<KategoriUrun>();    //null referans hatasına düşmemek
        }
        [Key]
        //TCKimlikNumarasi
        //UrunId
        public int Id { get; set; }

        [Display(Name="Adı")]
        [Required(ErrorMessage="{0} alanı boş geçilmemelidir")]
        public string Ad { get; set; }

        [Display(Name="Açıklama")]
        [Required(ErrorMessage="{0} alanı boş geçilmemelidir")]
        public string Aciklama { get; set; }

        [Display(Name="Fiyatı")]
        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString="{0:c}")]
        [Required(ErrorMessage="{0} alanı boş geçilmemelidir")]
        public decimal Fiyat { get; set; }

        [NotMapped]  //Veritabanına yansımamasını sağlar
        public IFormFile[] Dosyalar { get; set; }

        // -----ilişkiler---
        public List<Resim> Resimler { get; set; }
        public List<KategoriUrun> KategoriUrunleri { get; set; }
    }
}
