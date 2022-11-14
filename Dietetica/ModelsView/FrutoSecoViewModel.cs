using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dietetica.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dietetica.ModelsView
{
    public class FrutoSecoViewModel
    {
        public List<FrutoSeco> ListaFrutosSecos{ get; set; }
        public SelectList ListaTiposVentas { get; set; }
        public SelectList ListaProveedores { get; set; }
        public int? busqProveedor { get; set; }
        public string busqNombre { get; set; }
        public paginador paginador { get; set; }
    }
}
