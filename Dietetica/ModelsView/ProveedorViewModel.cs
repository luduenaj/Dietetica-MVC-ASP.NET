using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dietetica.Models;

namespace Dietetica.ModelsView
{
    public class ProveedorViewModel
    {
        public List<Proveedor> ListaProveedores { get; set; }
        public string busqNombre { get; set; }
        public paginador paginador { get; set; }
    }
}
