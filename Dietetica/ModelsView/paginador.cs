using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dietetica.ModelsView
{
    public class paginador
    {
        public int cantReg { get; set; }
        public int regXpag { get; set; }
        public int pagActual { get; set; }
        public int totalPag => (int)Math.Ceiling((decimal)cantReg / regXpag);
        public Dictionary<string, string> ValoresQueryString { get; set; } = new Dictionary<string, string>();
    }
}
