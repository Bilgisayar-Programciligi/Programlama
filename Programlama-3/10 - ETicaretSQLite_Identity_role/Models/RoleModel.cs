using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Models
{
    public class RoleModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Display(Name = "UserName")]
        public string UserName { get; set; }
        public string Aciklama { get; set; }
    }
}
