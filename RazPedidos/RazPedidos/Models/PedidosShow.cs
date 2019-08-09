using System;
using System.Collections.Generic;
using System.Text;

namespace RazPedidos.Models
{
    public class PedidosShow 
    {
        public int num { get; set; }
        public string direcion { get; set; }
        public int reparto { get; set; }
        public string tipofac { get; set; }
        public string identif { get; set; }
        public int marca { get; set; }
        public string username { get; set; }
        public string impreso { get; set; }
        public int codcli { get; set; }
        public DateTime fechaf { get; set; }
        public DateTime fechacuando { get; set; }
        public string bloqueada { get; set; }
        public string bloqueada2 { get; set; }
        public string recontrainhab { get; set; }
        public string cf { get; set; }
        public char isBlocked { get; set; }
        public bool marcar { get; set; }
    }
}
