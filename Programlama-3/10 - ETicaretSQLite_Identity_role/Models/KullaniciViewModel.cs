using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Models
{
    public class KullaniciViewModel
    {
        public string EPosta { get; set; }
        public string Sifre { get; set; }
        [Display(Name ="Açıklama")]
        public string Aciklama { get; set; }
    }
}
