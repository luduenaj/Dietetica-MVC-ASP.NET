using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dietetica.Models
{
    public class Proveedor
    {
        public int Id { set; get; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string nombre { set; get; }

        [Display(Name = "Telefono")]
        [Required(ErrorMessage = "El telefono es obligatorio")]
        public string telefono { set; get; }

        public string email { set; get; }
    }
}
