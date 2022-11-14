using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietetica.Models
{
    public class ProductoEmbasado
    {
        public int Id { set; get; }

        public string nombre { set; get; }

        public int precioPorUnidad {set; get;}

        public int gramos { set; get; }

        public string foto { set; get; }
        
        public int IdTipoVenta { set; get; }
        public TipoVenta tipoVenta { set; get; }

        public int IdProveedor { set; get; }
        public Proveedor proveedor { set; get; }
    }
}
