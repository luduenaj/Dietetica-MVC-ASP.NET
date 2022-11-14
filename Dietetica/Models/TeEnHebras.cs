using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietetica.Models
{
    public class TeEnHebras
    {
        public int Id { set; get;  }

        public string nombre { set; get; }

        public int precioX100gr { set; get; }

        public int IdTipoVenta { set; get; }
        public TipoVenta tipoVenta { set; get; }

        public int IdProveedor { set; get; }
        public Proveedor proveedor { set; get; }
    }
}
