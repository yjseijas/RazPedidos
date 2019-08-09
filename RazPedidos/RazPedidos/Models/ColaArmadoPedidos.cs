using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.Models
{
    public class ColaArmadoPedidos
    {
        public int idColaArmadoPedidos { get; set; }
        public int idUsuario { get; set; }
        public int idPedido { get; set; }
        public int numItem { get; set; }
        public int idArticulo { get; set; }
        public string idSector { get; set; }
        public int cantidad { get; set; }
        public int status { get; set; }
        public DateTime fechaStatus { get; set; }
        public int pickeado { get; set; }
        public int cantidadPick { get; set; }
    }
}
