using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.Models
{
    public class ArticuloBarras
    {
        public int codigo { get; set; }
        public string codigobarra { get; set; }
        public int factor { get; set; }
        public int bultos { get; set; }
        public int cajas { get; set; }
        public int unidades { get; set; }
    }
}
