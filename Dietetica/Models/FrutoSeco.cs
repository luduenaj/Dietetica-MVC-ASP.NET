using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietetica.Models
{
    public class FrutoSeco
    {
        public int Id{ set; get; }

        public string nombre { set; get; }

        public int precioXKg { set; get; }

        public int idTipoVenta { set; get; }
        public TipoVenta tipoVenta { set; get; }

        public int idProveedor { set; get; }
        public Proveedor proveedor { set; get; }
    }
}
