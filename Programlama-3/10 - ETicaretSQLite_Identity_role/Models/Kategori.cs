using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ETicaret.Models
{
    public class Kategori
    {
        // [Key]
        // public int KatId { get; set; }
        public int Id { get; set; } //varsayılan olarak PK
        [Display(Name="Adı")]
        public string Adi { get; set; }
        [Display(Name="Açıklama")]
        public string Aciklama { get; set; }

        //ilişkileri
        public List<KategoriUrun> KagegoriUrunleri { get; set; }  //KategoriUrun: Join Tablosu
    }
}