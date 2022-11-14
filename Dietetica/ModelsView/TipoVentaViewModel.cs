using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dietetica.Models;

namespace Dietetica.ModelsView
{
    public class TipoVentaViewModel
    {
        public List<TipoVenta> ListaTiposVentas { get; set; }
        public paginador paginador { get; set; }
    }
}
